Shader "Hidden/Custom/WindowRaindrops2"
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
            float _TrailBlur;
            
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
            
            float3 N13(float p)
            {
                //  from DAVE HOSKINS
               float3 p3 = frac(p * float3(0.1031, 0.11369, 0.13787));
               p3 += dot(p3, p3.yzx + 19.19);
               return frac(float3((p3.x + p3.y) * p3.z, (p3.x + p3.z) * p3.y, (p3.y + p3.z) * p3.x));
            }

            float4 N14(float t)
            {
                return frac(sin(t * float4(123, 1024, 1456, 264)) * float4(6547, 345, 8799, 1564));
            }
            
            float N(float t)
            {
                return frac(sin(t * 12345.564) * 7658.76);
            }

            float Saw(float b, float t)
            {
                return S(0., b, t) * S(1., b, t);
            }


            float2 DropLayer2(float2 uv, float t) {
                float2 UV = uv;
                
                uv.y += t*0.75;
                float2 a = float2(6., 1.);
                float2 grid = a*2.;
                float2 id = floor(uv*grid);
                
                float colShift = N(id.x); 
                uv.y += colShift;
                
                id = floor(uv*grid);
                float3 n = N13(id.x*35.2+id.y*2376.1);
                float2 st = frac(uv*grid)-float2(.5, 0);
                
                float x = n.x-.5;
                
                float y = UV.y*20.;
                float wiggle = sin(y+sin(y));
                x += wiggle*(.5-abs(x))*(n.z-.5);
                x *= .7;
                float ti = frac(t+n.z);
                y = (Saw(.85, ti)-.5)*.9+.5;
                float2 p = float2(x, y);
                
                float d = length((st-p)*a.yx);
                
                float mainDrop = S(.4, .0, d);
                
                float r = sqrt(S(1., y, st.y));
                float cd = abs(st.x-x);
                float trail = S(.23*r, .15*r*r, cd);
                float trailFront = S(-.02, .02, st.y-y);
                trail *= trailFront*r*r;
                
                y = UV.y;
                float trail2 = S(.2*r, .0, cd);
                float droplets = max(0., (sin(y*(1.-y)*120.)-st.y))*trail2*trailFront*n.z;
                y = frac(y*10.)+(st.y-.5);
                float dd = length(st-float2(x, y));
                droplets = S(.3, 0., dd);
                float m = mainDrop+droplets*r*trailFront;
                
                //m += st.x>a.y*.45 || st.y>a.x*.165 ? 1.2 : 0.;
                return float2(m, trail);
            }

            float StaticDrops(float2 uv, float t) {
                uv *= 40.;
                
                float2 id = floor(uv);
                uv = frac(uv)-.5;
                float3 n = N13(id.x*107.45+id.y*3543.654);
                float2 p = (n.xy-.5)*.7;
                float d = length(uv-p);
                
                float fade = Saw(.025, frac(t+n.z));
                float c = S(.3, 0., d)*frac(n.z*10.)*fade;
                return c;
            }

            float2 Drops(float2 uv, float t, float l0, float l1, float l2) {
                float s = StaticDrops(uv, t)*l0; 
                float2 m1 = DropLayer2(uv, t)*l1;
                float2 m2 = DropLayer2(uv*1.85, t)*l2;
                
                float c = s+m1.x+m2.x;
                c = S(.3, 1., c);
                
                return float2(c, max(m1.y*l0, m2.y*l1));
            }
            
            float4 frag(v2f i) : SV_Target
            {
                float2 uv = (i.texcoord.xy - 0.5) * _MainTex_TexelSize.y / _MainTex_TexelSize.x * _Size;
                float2 UV = i.texcoord.xy;
                float T = _Time.y * _Speed;
                
                float t = T*.2;
    
                float rainAmount = sin(T*.05)*.3+.7;
                
                float staticDrops = S(-.5, 1., rainAmount)*2.;
                float layer1 = S(.25, .75, rainAmount);
                float layer2 = S(.0, .5, rainAmount);
                
                float2 c = Drops(uv, t, staticDrops, layer1, layer2);
                
                float2 e = float2(.001, 0.);
                float cx = Drops(uv+e, t, staticDrops, layer1, layer2).x;
                float cy = Drops(uv+e.yx, t, staticDrops, layer1, layer2).x;
                float2 n = float2(cx-c.x, cy-c.x);      // expensive normals
                
                float focus = lerp(_Blur * rainAmount - c.y, _TrailBlur, S(.1, .2, c.x));
                float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, UV+n * _Distortion);
                float4 blurColor = SAMPLE_TEXTURE2D(_BlurTex, sampler_BlurTex, i.texcoord);
                return lerp(color, blurColor * _BlurColor, focus);
            }
            ENDHLSL
        }
    }
}
