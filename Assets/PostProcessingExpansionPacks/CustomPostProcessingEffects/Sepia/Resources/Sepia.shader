Shader "Hidden/Custom/Sepia"
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
            float _SepiaIntensity;
            
            float4 Frag(VaryingsDefault i) : SV_Target
            {
                float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
                
                float3 sepiaColor = 1;
                sepiaColor.r = dot(color.rgb, half3(0.393, 0.769, 0.189));
                sepiaColor.g = dot(color.rgb, half3(0.349, 0.686, 0.168));
                sepiaColor.b = dot(color.rgb, half3(0.272, 0.534, 0.131));
                
                color.rgb = lerp(color, sepiaColor, _SepiaIntensity);
                
                return color;
            }
            ENDHLSL
        }
    }
}
