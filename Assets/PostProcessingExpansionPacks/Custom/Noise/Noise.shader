Shader "Hidden/Custom/Noise"
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
            TEXTURE2D_SAMPLER2D(_NoiseTex, sampler_NoiseTex);
            
            float _NoiseSpeedX;
            float _NoiseSpeedY;
            float _Blend;
            
            float hash11(float n)
            {
                return frac(sin(n) * 43758.5453123);
            }
            
            float4 Frag(VaryingsDefault i) : SV_Target
            {
                float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
                
                float2 noiseUV = i.texcoord.xy + float2(_NoiseSpeedX , _NoiseSpeedY) * hash11(_Time.y);
                float4 noiseColor = SAMPLE_TEXTURE2D(_NoiseTex, sampler_NoiseTex, noiseUV);
                
                color = lerp(color, color * noiseColor, _Blend);
                
                return color;
            }
            ENDHLSL
        }
    }
}
