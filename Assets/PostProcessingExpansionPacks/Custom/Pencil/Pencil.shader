//http://wordpress.notargs.com/blog/blog/2016/07/13/unity%E9%89%9B%E7%AD%86%E9%A2%A8%E3%82%B7%E3%82%A7%E3%83%BC%E3%83%80%E3%83%BC%E3%82%92%E4%BD%9C%E3%81%A3%E3%81%A6%E3%81%BF%E3%81%9F/

Shader "Hidden/Custom/Pencil"
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
            
            float rand(float3 co)
            {
                return frac(sin(dot(co.xyz, float3(12.9898, 78.233, 56.787))) * 43758.5453);
            }
 
            float noise(float3 pos)
            {
                float3 ip = floor(pos);
                float3 fp = smoothstep(0, 1, frac(pos));
                float4 a = float4(
                    rand(ip + float3(0, 0, 0)),
                    rand(ip + float3(1, 0, 0)),
                    rand(ip + float3(0, 1, 0)),
                    rand(ip + float3(1, 1, 0)));
                float4 b = float4(
                    rand(ip + float3(0, 0, 1)),
                    rand(ip + float3(1, 0, 1)),
                    rand(ip + float3(0, 1, 1)),
                    rand(ip + float3(1, 1, 1)));
 
                a = lerp(a, b, fp.z);
                a.xy = lerp(a.xy, a.zw, fp.y);
                return lerp(a.x, a.y, fp.x);
            }
 
            float perlin(float3 pos)
            {
                return 
                    (noise(pos) * 32 +
                    noise(pos * 2 ) * 16 +
                    noise(pos * 4) * 8 +
                    noise(pos * 8) * 4 +
                    noise(pos * 16) * 2 +
                    noise(pos * 32) ) / 63;
            }
 
            float monochrome(float3 col)
            {
                return 0.299 * col.r + 0.587 * col.g + 0.114 * col.b;
            }
            
            float4 Frag(VaryingsDefault i) : SV_Target
            {
                float2 screenUV = i.texcoord * _ScreenParams.xy;
                i.texcoord += (float2(perlin(float3(screenUV, _Time.y) * 5), perlin(float3(screenUV, _Time.y + 100) * 5)) - 0.5) * 0.01;
                float col = monochrome(SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord)) + 0.2f;
 
                float2 pixelSize = _ScreenParams.zw - 1;
                col -= abs
                    (
                        monochrome(SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord - float2(pixelSize.x, 0))) -
                        monochrome(SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + float2(pixelSize.x, 0))) +
                        monochrome(SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord - float2(0, pixelSize.y))) -
                        monochrome(SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + float2(0, pixelSize.y)))
                    ) * 0.7;
                col *= perlin(float3(screenUV, _Time.y * 10) * 1) * 0.5f + 0.8f;
                return float4(col, col, col, 1);
            }
            ENDHLSL
        }
    }
}
