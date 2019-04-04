Shader "Hidden/Custom/Outline"
{
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always
        
        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert_blur
            #pragma fragment frag_blur
            
            #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"
            
            struct a2v
            {
                float4 vertex : POSITION;
                float4 texcoord : TEXCOORD0;
            };
            
            struct v2f_blur
            {
                float4 vertex : SV_POSITION;
                float2 texcoord : TEXCOORD0;
                float4 texcoord01 : TEXCOORD1;
                float4 texcoord23 : TEXCOORD2;
                float4 texcoord45 : TEXCOORD3;
            };
            
            TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
            float4 _MainTex_TexelSize;
            float4 _Offset;
            
            v2f_blur vert_blur(a2v v)
            {
                v2f_blur o;
                o.vertex = float4(v.vertex.xy, 0.0, 1.0);
                o.texcoord = TransformTriangleVertexToUV(v.vertex.xy);
                
                #if UNITY_UV_STARTS_AT_TOP
                o.texcoord = o.texcoord * float2(1.0, -1.0) + float2(0.0, 1.0);
                #endif
                
                o.texcoord01 = o.texcoord.xyxy + _Offset.xyxy * _MainTex_TexelSize.xyxy * float4(1, 1, -1, -1);
                o.texcoord23 = o.texcoord.xyxy + _Offset.xyxy * _MainTex_TexelSize.xyxy * float4(1, 1, -1, -1) * 2.0;
                o.texcoord45 = o.texcoord.xyxy + _Offset.xyxy * _MainTex_TexelSize.xyxy * float4(1, 1, -1, -1) * 3.0;
                
                return o;
            }
            
            float4 frag_blur(v2f_blur i) : SV_Target
            {
                float4 color;
                color  = 0.40 * SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
                color += 0.15 * SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord01.xy);
                color += 0.15 * SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord01.zw);
                color += 0.10 * SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord23.xy);
                color += 0.10 * SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord23.zw);
                color += 0.05 * SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord45.xy);
                color += 0.05 * SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord45.zw);
                return color;
            }
            ENDHLSL
        }
        
        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert_culling
            #pragma fragment frag_culling
            
            #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"
            
            struct a2v
            {
                float4 vertex : POSITION;
                float4 texcoord : TEXCOORD0;
            };
            
            struct v2f_culling
            {
                float4 vertex : SV_POSITION;
                float2 texcoord : TEXCOORD0;
            };
            
            TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
            TEXTURE2D_SAMPLER2D(_BlurTex, sampler_BlurTex);
            
            v2f_culling vert_culling(a2v v)
            {
                v2f_culling o;
                o.vertex = float4(v.vertex.xy, 0.0, 1.0);
                o.texcoord = TransformTriangleVertexToUV(v.vertex.xy);
                
                #if UNITY_UV_STARTS_AT_TOP
                o.texcoord = o.texcoord * float2(1.0, -1.0) + float2(0.0, 1.0);
                #endif
                
                return o;
            }
            
            float4 frag_culling(v2f_culling i) : SV_Target
            {
                float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
                float4 colorBlur = SAMPLE_TEXTURE2D(_BlurTex, sampler_BlurTex, i.texcoord);
                return colorBlur - color;
            }
            ENDHLSL
        }
        
        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert_merge
            #pragma fragment frag_merge
            
            #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"
            
            struct a2v
            {
                float4 vertex : POSITION;
                float4 texcoord : TEXCOORD0;
            };
            
            struct v2f_merge
            {
                float4 vertex : SV_POSITION;
                float2 texcoord : TEXCOORD0;
            };
            
            TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
            TEXTURE2D_SAMPLER2D(_BlurTex, sampler_BlurTex);
            float _Strength;
            
            v2f_merge vert_merge(a2v v)
            {
                v2f_merge o;
                o.vertex = float4(v.vertex.xy, 0.0, 1.0);
                o.texcoord = TransformTriangleVertexToUV(v.vertex.xy);
                
                #if UNITY_UV_STARTS_AT_TOP
                o.texcoord = o.texcoord * float2(1.0, -1.0) + float2(0.0, 1.0);
                #endif
                
                return o;
            }
            
            float4 frag_merge(v2f_merge i) : SV_Target
            {
                float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
                float4 colorBlur = SAMPLE_TEXTURE2D(_BlurTex, sampler_BlurTex, i.texcoord);
                return color + colorBlur * _Strength;
            }
            ENDHLSL
        }
    }
}
