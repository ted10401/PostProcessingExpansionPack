//http://wordpress.notargs.com/blog/blog/2016/01/09/unity3d%E3%83%96%E3%83%A9%E3%82%A6%E3%83%B3%E7%AE%A1%E9%A2%A8%E3%82%B7%E3%82%A7%E3%83%BC%E3%83%80%E3%83%BC%E3%82%92%E4%BD%9C%E3%81%A3%E3%81%9F/

Shader "Hidden/Custom/CRT"
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
            
            float _NoiseX;
            float2 _Offset;
            float _RGBNoise;
            float _SinNoiseWidth;
            float _SinNoiseScale;
            float _SinNoiseOffset;
            float _ScanLineTail;
            float _ScanLineSpeed;
            
            float rand(float2 co)
            {
                return frac(sin(dot(co.xy, float2(12.9898, 78.233))) * 43758.5453);
            }
 
            float2 mod(float2 a, float2 b)
            {
                return a - floor(a / b) * b;
            }
            
            float4 Frag(VaryingsDefault i) : SV_Target
            {
                float2 inUV = i.texcoord;
                float2 uv = i.texcoord - 0.5;
                
                // UV座標を再計算し、画面を歪ませる
                float vignet = length(uv);
                uv /= 1 - vignet * 0.2;
                float2 texUV = uv + 0.5;
 
                // 画面外なら描画しない
                if (max(abs(uv.y) - 0.5, abs(uv.x) - 0.5) > 0)
                {
                    return float4(0, 0, 0, 1);
                }
 
                // 色を計算
                float3 col;
 
                // ノイズ、オフセットを適用
                texUV.x += sin(texUV.y * _SinNoiseWidth + _SinNoiseOffset) * _SinNoiseScale;
                texUV += _Offset;
                texUV.x += (rand(floor(texUV.y * 500) + _Time.y) - 0.5) * _NoiseX;
                texUV = mod(texUV, 1);
 
                // 色を取得、RGBを少しずつずらす
                col.r = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, texUV).r;
                col.g = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, texUV - float2(0.002, 0)).g;
                col.b = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, texUV - float2(0.004, 0)).b;
 
                // RGBノイズ
                if (rand((rand(floor(texUV.y * 500) + _Time.y) - 0.5) + _Time.y) < _RGBNoise)
                {
                    col.r = rand(uv + float2(123 + _Time.y, 0));
                    col.g = rand(uv + float2(123 + _Time.y, 1));
                    col.b = rand(uv + float2(123 + _Time.y, 2));
                }
 
                // ピクセルごとに描画するRGBを決める
                float floorX = fmod(inUV.x * _ScreenParams.x / 3, 1);
                col.r *= floorX > 0.3333;
                col.g *= floorX < 0.3333 || floorX > 0.6666;
                col.b *= floorX < 0.6666;
 
                // スキャンラインを描画
                float scanLineColor = sin(_Time.y * 10 + uv.y * 500) / 2 + 0.5;
                col *= 0.5 + clamp(scanLineColor + 0.5, 0, 1) * 0.5;
 
                // スキャンラインの残像を描画
                float tail = clamp((frac(uv.y + _Time.y * _ScanLineSpeed) - 1 + _ScanLineTail) / min(_ScanLineTail, 1), 0, 1);
                col *= tail;
 
                // 画面端を暗くする
                col *= 1 - vignet * 1.3;
                
                return float4(col, 1);
            }
            ENDHLSL
        }
    }
}
