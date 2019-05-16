Shader "Hidden/Custom/Mosaic"
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
                float2 uv = i.texcoord;
                uv = float2((int)(uv.x / _PixelSize), (int)(uv.y / _PixelSize)) * _PixelSize;
            
                float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);
                return color;
            }
            ENDHLSL
        }
    }
}
