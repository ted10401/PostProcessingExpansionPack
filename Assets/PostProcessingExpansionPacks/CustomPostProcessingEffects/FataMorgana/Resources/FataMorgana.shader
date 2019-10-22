Shader "Hidden/Custom/FataMorgana"
{
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

		HLSLINCLUDE
		#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"
		TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
		float4 _MainTex_TexelSize;
		TEXTURE2D_SAMPLER2D(_CameraDepthTexture, sampler_CameraDepthTexture);
		float _DepthMutiplier;
		float2 _DepthRange;
		TEXTURE2D_SAMPLER2D(_DistortionTex, sampler_DistortionTex);
		float _DistortionStrength;
		float _DistortionSpeed;

		float getDepth(VaryingsDefault i)
		{
			float depth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, sampler_CameraDepthTexture, i.texcoordStereo);
			float linear01Depth = Linear01Depth(depth);
			depth = linear01Depth * _DepthMutiplier;

			float more = step(_DepthRange.x, depth);
			float less = step(depth, _DepthRange.y);
			return depth * more * less;
		}

		float2 getDistortionOffset(VaryingsDefault i)
		{
			float depth = getDepth(i);

			float2 distortionOffset = SAMPLE_TEXTURE2D(_DistortionTex, sampler_DistortionTex, i.texcoord - float2(0, _DistortionSpeed * _Time.y)).xy;
			distortionOffset = (2.0 * distortionOffset - 1.0) * (1.0 - _MainTex_TexelSize.y);
			distortionOffset *= _DistortionStrength * 0.01;
			return lerp(0, distortionOffset, depth);
		}
		ENDHLSL
        
        Pass
        {
            HLSLPROGRAM
            #pragma vertex VertDefault
            #pragma fragment Frag
            
            float4 Frag(VaryingsDefault i) : SV_Target
            {
				float2 texcoord = i.texcoord + getDistortionOffset(i);
                float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, texcoord);

                return color;
            }
            ENDHLSL
        }

		Pass //Debug Distortion
        {
            HLSLPROGRAM
            #pragma vertex VertDefault
            #pragma fragment Frag
            
            float4 Frag(VaryingsDefault i) : SV_Target
            {
				float2 texcoord = i.texcoord + getDistortionOffset(i);
				return float4(texcoord, 0, 0);
            }
            ENDHLSL
        }

		Pass //Debug Depth
        {
            HLSLPROGRAM
            #pragma vertex VertDefault
            #pragma fragment Frag
            
            float4 Frag(VaryingsDefault i) : SV_Target
            {
				float depth = getDepth(i);
				return float4(depth, depth, depth, 1);
            }
            ENDHLSL
        }
    }
}
