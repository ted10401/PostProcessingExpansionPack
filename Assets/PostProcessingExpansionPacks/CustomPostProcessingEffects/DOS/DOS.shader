Shader "Hidden/Custom/DOS"
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
            float _PixelSize;
            
            float4 Frag(VaryingsDefault i) : SV_Target
            {
                float2 pixelUV = float2((int)(i.texcoord.x / _PixelSize), (int)(i.texcoord.y / _PixelSize)) * _PixelSize;
                float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, pixelUV);
                
                //Convert to grayscale
                color = dot(color.rgb, float3(0.3, 0.59, 0.11));

                if (color.r <= 0.5)
                {
                    color = float4(0.18, 0.16, 0.2, 1.0);
                }
                else
                {
                    color = float4(0.93, 0.91, 0.8, 1.0);
                }
                
                return color;
            }
            ENDHLSL
        }
    }
}
