Shader "Hidden/Custom/Lightning"
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

			float _LightningFlicker;
			float _LightningFlash;
            
            float4 Frag(VaryingsDefault i) : SV_Target
            {
                float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);

				float t = _Time.y;
				float lightning = sin(t * sin(t * _LightningFlicker));
                lightning *= pow(max(0, sin(t + sin(t))), _LightningFlash);

                return color * (1 + lightning);
            }
            ENDHLSL
        }
    }
}
