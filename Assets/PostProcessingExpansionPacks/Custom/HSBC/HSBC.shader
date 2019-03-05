Shader "Hidden/Custom/HSBC"
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
            
            float _Hue;
            float _Saturation;
            float _Brightness;
            float _Contrast;
            
            inline float3 applyHue(float3 aColor, float aHue)
            {
                float angle = radians(aHue);
                float3 k = float3(0.57735, 0.57735, 0.57735);
                float cosAngle = cos(angle);
                 
                return aColor * cosAngle + cross(k, aColor) * sin(angle) + k * dot(k, aColor) * (1 - cosAngle);
            }
            
            inline float3 applyContrast(float3 color, float contrast)
            {
                #if !UNITY_COLORSPACE_GAMMA
                color = LinearToGammaSpace(color);
                #endif
                
                color = saturate(lerp(half3(0.5, 0.5, 0.5), color, contrast));
                
                #if !UNITY_COLORSPACE_GAMMA
                color = GammaToLinearSpace(color);
                #endif
                return color;
            }
            
            inline float4 applyHSBCEffect(float4 startColor, float h, float s, float b, float c)
            {
                float hue = 360 * h;
                float saturation = s * 2;
                float brightness = b * 2 - 1;
                float contrast = c * 2;
  
                float4 outputColor = startColor;
                outputColor.rgb = applyHue(outputColor.rgb, hue);
                outputColor.rgb = applyContrast(outputColor.rgb, contrast);
                outputColor.rgb = outputColor.rgb + brightness;
                float3 intensity = dot(outputColor.rgb, float3(0.39, 0.59, 0.11));
                outputColor.rgb = lerp(intensity, outputColor.rgb, saturation);
                  
                return outputColor;
            }
            
            float4 Frag(VaryingsDefault i) : SV_Target
            {
                float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
                color = applyHSBCEffect(color, _Hue, _Saturation, _Brightness, _Contrast);
                return color;
            }
            ENDHLSL
        }
    }
}
