Shader "Hidden/Custom/Cloud"
{
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always
        
        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
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
            
            TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
			float4 _MainTex_TexelSize;
			float _CloudScale;
			float2 _CloudSpeed;
			float _CloudCover;
			float _CloudAlpha;
			float3 _CloudDarkColor;
			float3 _CloudLightColor;
			float3 _SkyUpColor;
			float3 _SkyDownColor;

			float2 hash(float2 p)
			{
				p = float2(dot(p, float2(127.1, 311.7)), dot(p, float2(269.5,183.3)));
				return -1.0 + 2.0 * frac(sin(p) * 43758.5453123);
			}

			float noise(in float2 p)
			{
				const float K1 = 0.366025404; // (sqrt(3)-1)/2;
				const float K2 = 0.211324865; // (3-sqrt(3))/6;
				float2 i = floor(p + (p.x + p.y) * K1);	
				float2 a = p - i + (i.x + i.y) * K2;
				float2 o = (a.x > a.y) ? float2(1.0, 0.0) : float2(0.0, 1.0); //float2 of = 0.5 + 0.5*float2(sign(a.x-a.y), sign(a.y-a.x));
				float2 b = a - o + K2;
				float2 c = a - 1.0 + 2.0 * K2;
				float3 h = max(0.5 - float3(dot(a, a), dot(b, b), dot(c, c)), 0.0);
				float3 n = h * h * h * h * float3(dot(a, hash(i)), dot(b, hash(i + o)), dot(c, hash(i + 1.0)));
				return dot(n, float3(70.0, 70.0, 70.0));	
			}

			float fbm(float2 n)
			{
				const float2x2 m = float2x2(1.6,  1.2, -1.2, 1.6);
				float total = 0.0, amplitude = 0.1;
				for (int i = 0; i < 7; i++) {
					total += noise(n) * amplitude;
					n = mul(m, n);
					amplitude *= 0.4;
				}
				return total;
			}
            
            float4 frag(v2f input) : SV_Target
            {
				const float2x2 m = float2x2( 1.6,  1.2, -1.2,  1.6 );
				float2 p = input.texcoord;
				float2 uv = p;
				float2 uv_delta = _CloudSpeed * _Time.y;
				float q = fbm(uv * _CloudScale * 0.5);
    
				//ridged noise shape
				float r = 0.0;
				uv *= _CloudScale;
				uv -= q - uv_delta;
				float weight = 0.8;
				for (int i = 0; i < 4; i++)
				{
					r += abs(weight * noise(uv));
					uv = mul(m, uv) + uv_delta;
					weight *= 0.7;
				}
    
				//noise shape
				float f = 0.0;
				uv = p;
				uv *= _CloudScale;
				uv -= q - uv_delta;
				weight = 0.7;
				for (int i = 0; i < 8; i++)
				{
					f += weight * noise(uv);
					uv = mul(m, uv) + uv_delta;
					weight *= 0.6;
				}
    
				f *= r + f;
    
				//noise colour
				float c = 0.0;
				uv_delta = uv_delta * 2.0;
				uv = p;
				uv *= _CloudScale * 2.0;
				uv -= q - uv_delta;
				weight = 0.4;
				for (int i = 0; i < 7; i++)
				{
					c += weight * noise(uv);
					uv = mul(m, uv) + uv_delta;
					weight *= 0.6;
				}
    
				//noise ridge colour
				float c1 = 0.0;
				uv_delta = uv_delta * 3.0;
				uv = p;
				uv *= _CloudScale * 3.0;
				uv -= q - uv_delta;
				weight = 0.4;
				for (int i=0; i<7; i++)
				{
					c1 += abs(weight * noise(uv));
					uv = mul(m, uv) + uv_delta;
					weight *= 0.6;
				}
	
				c += c1;
    
				float3 skyColor = lerp(_SkyDownColor, _SkyUpColor, p.y);
				float3 cloudColor = saturate(c);
				cloudColor = lerp(_CloudDarkColor, _CloudLightColor, cloudColor);
   
				f = _CloudCover + _CloudAlpha * f * r;
				f = saturate(f + c);
    
				float3 result = lerp(skyColor, cloudColor, f);
				float4 finalColor = float4(result, 1.0);
				return finalColor;
            }
            ENDHLSL
        }
    }
}
