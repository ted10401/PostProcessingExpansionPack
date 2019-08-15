Shader "Hidden/Custom/GlobalFog"
{
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always
        
        Pass
        {
            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            
            #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"
            
            struct a2v
            {
                float3 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };
            
            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 texcoord : TEXCOORD0;
                float2 texcoordStereo : TEXCOORD1;
                float4 interpolatedRay : TEXCOORD2;
            };
            
            TEXTURE2D_SAMPLER2D(_CameraDepthTexture, sampler_CameraDepthTexture);
            TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
            TEXTURE2D_SAMPLER2D(_NoiseTex, sampler_NoiseTex);
            
			float _Weight;
            float4x4 _FrustumCornersRay;
            float4 _FogDepthColor;
			float _FogDepthStrength;
			float _FogDepthPower;
			float _FogDepthDensity;
			float4 _FogHeightColor;
			float _FogHeightStart;
            float _FogHeightRange;
			float _FogHeightDensity;
            float _FogXSpeed;
            float _FogYSpeed;
            float _NoiseAmount;
            
            v2f Vert(a2v v)
            {
                v2f o;
                o.vertex = float4(v.vertex.xy, 0.0, 1.0);
                o.texcoord = TransformTriangleVertexToUV(v.vertex.xy);

            #if UNITY_UV_STARTS_AT_TOP
                o.texcoord = o.texcoord * float2(1.0, -1.0) + float2(0.0, 1.0);
            #endif
            
                o.texcoordStereo = TransformStereoScreenSpaceTex(o.texcoord, 1.0);

				int x = (int)v.texcoord.x;
				int y = (int)v.texcoord.y;
                o.interpolatedRay = _FrustumCornersRay[x + 2 * y];

                return o;
            }
            
            float4 Frag(v2f i) : SV_Target
            {
                float depth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, sampler_CameraDepthTexture, i.texcoordStereo);
                float linearEyeDepth = LinearEyeDepth(depth);
                float linear01Depth = Linear01Depth(depth);
				float3 worldPos = _WorldSpaceCameraPos + linearEyeDepth * i.interpolatedRay.xyz;
                
                float2 speed = float2(_FogXSpeed, _FogYSpeed) * _Time.y;
				float2 uv = i.texcoord + speed;
                float noise = (SAMPLE_TEXTURE2D(_NoiseTex, sampler_NoiseTex, uv).r - 0.5) * _NoiseAmount;

				float fogDepthDensity = pow(linear01Depth * _FogDepthStrength, _FogDepthPower);
				fogDepthDensity *= (1 + noise) * _FogDepthDensity;
                fogDepthDensity = saturate(fogDepthDensity);
				fogDepthDensity *= _Weight;

				float fogHeightDensity = (_FogHeightStart + _FogHeightRange - worldPos.y) / _FogHeightRange;
				fogHeightDensity *= (1 + noise) * _FogHeightDensity;
				fogHeightDensity = saturate(fogHeightDensity);
				fogHeightDensity *= _Weight;
 
                float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
				float4 fogColor = lerp(color, _FogDepthColor, fogDepthDensity);
				fogColor = lerp(fogColor, _FogHeightColor, fogHeightDensity);
            
                return lerp(color, fogColor, _Weight);
            }
            ENDHLSL
        }
    }
}
