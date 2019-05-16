Shader "Hidden/Custom/OilPainting"
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
            float4 _MainTex_TexelSize;
            int _Radius;
            
            float4 Frag(VaryingsDefault i) : SV_Target
            {
                half2 uv = i.texcoord;
 
                float3 mean[4] = {
                    {0, 0, 0},
                    {0, 0, 0},
                    {0, 0, 0},
                    {0, 0, 0}
                };
 
                float3 sigma[4] = {
                    {0, 0, 0},
                    {0, 0, 0},
                    {0, 0, 0},
                    {0, 0, 0}
                };
                
                float2 start[4] = {{-_Radius, -_Radius}, {-_Radius, 0}, {0, -_Radius}, {0, 0}};
 
                float2 pos;
                float3 col;
                for (int k = 0; k < 4; k++) {
                    for(int i = 0; i <= _Radius; i++) {
                        for(int j = 0; j <= _Radius; j++) {
                            pos = float2(i, j) + start[k];
                            col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, float4(uv + float2(pos.x * _MainTex_TexelSize.x, pos.y * _MainTex_TexelSize.y), 0., 0.)).rgb;
                            mean[k] += col;
                            sigma[k] += col * col;
                        }
                    }
                }
                
                float sigma2;
 
                float n = pow(_Radius + 1, 2);
                float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);
                float min = 1;
 
                for (int l = 0; l < 4; l++) {
                    mean[l] /= n;
                    sigma[l] = abs(sigma[l] / n - mean[l] * mean[l]);
                    sigma2 = sigma[l].r + sigma[l].g + sigma[l].b;
 
                    if (sigma2 < min) {
                        min = sigma2;
                        color.rgb = mean[l].rgb;
                    }
                }
                return color;
            }
            ENDHLSL
        }
    }
}
