Shader "Hidden/Custom/Spotlight"
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
            float _CenterX;
            float _CenterY;
            float _Radius;
            float _Sharpness;
            
            float4 Frag(VaryingsDefault i) : SV_Target
            {
                float2 uv = i.texcoord - 0.5;
                uv.y *= _ScreenParams.y / _ScreenParams.x;
                float dist = distance(float2(_CenterX, _CenterY), uv);
            
                float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
                return color * (1 - pow(dist / _Radius, _Sharpness));
            }
            ENDHLSL
        }
    }
}
