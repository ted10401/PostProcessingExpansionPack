Shader "Hidden/Custom/GameBoy"
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
                
                // Original Gameboy RGB Colors :
                // 15, 56, 15
                // 48, 98, 48
                // 139, 172, 15
                // 155, 188, 15

                if (color.r <= 0.25)
                {
                    color = float4(0.06, 0.22, 0.06, 1.0);
                }
                else if (color.r > 0.75)
                {
                    color = float4(0.6, 0.74, 0.06, 1.0);
                }
                else if (color.r > 0.25 && color.r <= 0.5)
                {
                    color = float4(0.19, 0.38, 0.19, 1.0);
                }
                else
                {
                    color = float4(0.54, 0.67, 0.06, 1.0);
                }
                
                return color;
            }
            ENDHLSL
        }
    }
}
