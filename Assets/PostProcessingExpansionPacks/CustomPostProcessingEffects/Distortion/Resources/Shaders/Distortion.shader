Shader "Hidden/Custom/Distortion"
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
            TEXTURE2D_SAMPLER2D(_GlobalDistortionTex, sampler_GlobalDistortionTex);
            float4 _MainTex_TexelSize;
            float _Magnitude;
            
            float4 Frag(VaryingsDefault i) : SV_Target
            {
                float2 mag = _Magnitude * _MainTex_TexelSize.xy;
                float2 distortion = SAMPLE_TEXTURE2D(_GlobalDistortionTex, sampler_GlobalDistortionTex, i.texcoord).xy * mag;
                float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + distortion);
                return color;
            }
            ENDHLSL
        }
    }
}
