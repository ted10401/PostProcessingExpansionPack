Shader "Hidden/Custom/Blurs/KawaseBlur"
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
    }
}
