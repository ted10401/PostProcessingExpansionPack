Shader "Hidden/Custom/WindowRaindrops"
{
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

		Pass
        {
            HLSLPROGRAM
            #include "KawaseBlur.hlsl"
            #pragma vertex vertKawaseBlur
            #pragma fragment fragKawaseBlur0
            ENDHLSL
        }
        
        Pass
        {
            HLSLPROGRAM
            #include "KawaseBlur.hlsl"
            #pragma vertex vertKawaseBlur
            #pragma fragment fragKawaseBlur1
            ENDHLSL
        }
        
        Pass
        {
            HLSLPROGRAM
            #include "KawaseBlur.hlsl"
            #pragma vertex vertKawaseBlur
            #pragma fragment fragKawaseBlur2
            ENDHLSL
        }
        
        Pass
        {
            HLSLPROGRAM
            #include "KawaseBlur.hlsl"
            #pragma vertex vertKawaseBlur
            #pragma fragment fragKawaseBlur3
            ENDHLSL
        }
        
        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
			#define S(a, b, t) smoothstep(a, b, t)
            #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"
            
            struct a2v
            {
                float4 vertex : POSITION;
                float4 texcoord : TEXCOORD0;
            };
            
            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 texcoord : TEXCOORD0;
            };
            
            TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
            float4 _MainTex_TexelSize;
			float _Size;
			float _Speed;
			float _Distortion;
			TEXTURE2D_SAMPLER2D(_BlurTex, sampler_BlurTex);
            float4 _BlurTex_TexelSize;
			float4 _BlurColor;
			float _Blur;
            
            v2f vert(a2v v)
            {
                v2f o;
                o.vertex = float4(v.vertex.xy, 0.0, 1.0);
                o.texcoord = TransformTriangleVertexToUV(v.vertex.xy);
                
                #if UNITY_UV_STARTS_AT_TOP
                o.texcoord = o.texcoord * float2(1.0, -1.0) + float2(0.0, 1.0);
                #endif
                
                return o;
            }
			
			float N21(float2 p)
			{
				p = frac(p * float2(123.34, 345.45));
				p += dot(p, p + 34.345);
				return frac(p.x * p.y);
			}

			float3 Layer(float2 texcoord, float t)
			{
				float2 aspect = float2(2, 1);
				float2 uv = texcoord * _Size * aspect;
				uv.y += t * 0.25;
				float2 gv = frac(uv) - 0.5;
				float2 id = floor(uv);
				float n = N21(id);
				t += n * 2 * 3.14155;

				float size = n * 0.5 + 0.5;

				float w = texcoord.y * 10;
				float x = (n - 0.5) * 0.8; // -0.4~0.4
				x += (0.4 - abs(x)) * sin(3 * w) * pow(sin(w), 6) * 0.45;

				float y = -sin(t + sin(t + sin(t) * 0.5)) * 0.45;
				y -= (gv.x - x) * (gv.x - x);

				float2 dropPos = (gv - float2(x, y)) / aspect;
				float drop = S(0.05 * size, 0.03 * size, length(dropPos));

				float2 trailPos = (gv - float2(x, t * 0.25)) / aspect;
				trailPos.y = (frac(trailPos.y * 10) - 0.5) / 10;
				float trail = S(0.03 * size, 0.01 * size, length(trailPos));
				float fogTrail = S(-0.05 * size, 0.05 * size, dropPos.y);
				fogTrail *= S(0.5, y, gv.y);
				trail *= fogTrail;
				fogTrail *= S(0.05 * n, 0.04 * n, abs(dropPos.x));

				float2 offs = drop * dropPos + trail * trailPos;

				return float3(offs, fogTrail);
			}
            
            float4 frag(v2f i) : SV_Target
            {
				float t = fmod(_Time.y * _Speed, 7200);

				float3 drops = Layer(i.texcoord, t);
				drops += Layer(i.texcoord * 1.23 + 7.54, t);
				drops += Layer(i.texcoord * 1.35 + 1.54, t);
				drops += Layer(i.texcoord * 1.57 - 7.54, t);

				float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + drops.xy * _Distortion);
				float4 blurColor = SAMPLE_TEXTURE2D(_BlurTex, sampler_BlurTex, i.texcoord);

				return lerp(color, blurColor * _BlurColor, (1 - drops.z) * _Blur);
            }
            ENDHLSL
        }
    }
}
