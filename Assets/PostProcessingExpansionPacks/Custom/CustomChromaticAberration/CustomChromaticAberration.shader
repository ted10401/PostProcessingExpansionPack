Shader "Hidden/Custom/CustomChromaticAberration"
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
            float _RedX;
            float _RedY;
            float _GreenX;
            float _GreenY;
            float _BlueX;
            float _BlueY;
            
            float4 Frag(VaryingsDefault i) : SV_Target
            {
                float4 color = float4(1, 1, 1, 1);
                
                float2 red_uv = i.texcoord + float2(_RedX, _RedY);
                float2 green_uv = i.texcoord + float2(_GreenX, _GreenY);
                float2 blue_uv = i.texcoord + float2(_BlueX, _BlueY);

                color.r = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, red_uv).r;
                color.g = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, green_uv).g;
                color.b = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, blue_uv).b;
                
                return color;
            }
            ENDHLSL
        }
    }
}
