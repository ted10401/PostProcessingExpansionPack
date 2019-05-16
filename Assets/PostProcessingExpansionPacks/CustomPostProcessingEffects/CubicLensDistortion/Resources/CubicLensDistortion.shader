Shader "Hidden/Custom/CubicLensDistortion"
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
            
            float _K;
            float _KCube;
            
            inline float2 getDistortionUV(float2 uv)
            {
                float2 r = uv - 0.5;
                float rLength = length(r);
                float f = 1 + rLength * rLength * (_K + _KCube * rLength);
                   
                return f * r + 0.5;
            }
            
            float4 Frag(VaryingsDefault i) : SV_Target
            {
                float2 distortionUV = getDistortionUV(i.texcoord);
                float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, distortionUV);
                return color;
            }
            ENDHLSL
        }
    }
}
