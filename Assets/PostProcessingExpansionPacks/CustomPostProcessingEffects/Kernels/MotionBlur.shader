Shader "Hidden/Custom/Kernels/MotionBlur"
{
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always
        
        Pass
        {
            HLSLPROGRAM
            #pragma vertex VertDefault
            #pragma fragment Frag
            
            #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"
            
            TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
            float2 _Center;
            float _NearClip;
            float2 _DistanceStrength;
            float _Strength;
            
            float4 Frag(VaryingsDefault i) : SV_Target
            {
                float2 uv = i.texcoord - _Center;
                float uvDistance = length(uv);
                uvDistance = saturate(uvDistance - _NearClip);
                
                uv *= _DistanceStrength;
                
                const float kernel[8] = {0.01, 0.03, 0.05, 0.09, 0.14, 0.20, 0.28, 0.38};
                float4 sum = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
                for (int it = 0; it < 8; ++it)  
                {  
                    sum += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + uv * uvDistance * kernel[it]); 
                }

                sum /= 9;
            
                float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
                color = color * _Strength + sum;
                color /= _Strength + 1;
                return color;
            }
            ENDHLSL
        }
    }
}
