Shader "Hidden/Custom/VerticalFog"
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
            
            float4x4 _FrustumCornersRay;
            float _FogDensity;
            float4 _FogColor;
            float _FogStart;
            float _FogEnd;
            
            v2f Vert(a2v v)
            {
                v2f o;
                o.vertex = float4(v.vertex.xy, 0.0, 1.0);
                o.texcoord = TransformTriangleVertexToUV(v.vertex.xy);

            #if UNITY_UV_STARTS_AT_TOP
                o.texcoord = o.texcoord * float2(1.0, -1.0) + float2(0.0, 1.0);
            #endif
            
                o.texcoordStereo = TransformStereoScreenSpaceTex(o.texcoord, 1.0);
            
                int index = 0;
                if(v.texcoord.x < 0.5 && v.texcoord.y < 0.5)
                {
                    index = 0;
                }
                else if(v.texcoord.x > 0.5 && v.texcoord.y < 0.5)
                {
                    index = 1;
                }
                else if(v.texcoord.x > 0.5 && v.texcoord.y > 0.5)
                {
                    index = 2;
                }
                else
                {
                    index = 3;
                }

                o.interpolatedRay = _FrustumCornersRay[index];

                return o;
            }
            
            float4 Frag(v2f i) : SV_Target
            {
                float depth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, sampler_CameraDepthTexture, i.texcoordStereo);
                float linearEyeDepth = LinearEyeDepth(depth);
                float linearDepth = Linear01Depth(depth);
                
                float3 worldPos = _WorldSpaceCameraPos + linearEyeDepth * i.interpolatedRay.xyz;

                float fogDensity = (_FogEnd - worldPos.y) / (_FogEnd - _FogStart);
                fogDensity = saturate(fogDensity * _FogDensity);

                fogDensity += linearDepth;
                fogDensity = saturate(fogDensity * _FogDensity);
 
                float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
                color.rgb = lerp(color.rgb, _FogColor.rgb, fogDensity);
            
                return color;
            }
            ENDHLSL
        }
    }
}
