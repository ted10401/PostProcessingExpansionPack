#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

struct v2f
{
	float4 vertex : SV_POSITION;
	float2 texcoord : TEXCOORD0;
};

TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
float4 _MainTex_TexelSize;

v2f vertKawaseBlur (AttributesDefault v)
{
	v2f o;
    o.vertex = float4(v.vertex.xy, 0.0, 1.0);
    o.texcoord = TransformTriangleVertexToUV(v.vertex.xy);
    
    #if UNITY_UV_STARTS_AT_TOP
    o.texcoord = o.texcoord * float2(1.0, -1.0) + float2(0.0, 1.0);
    #endif

	return o;
}

float4 kawaseBlur(float2 uv, int pixelOffset)
{
	float2 uv1 = uv + _MainTex_TexelSize * float2(0.5 + pixelOffset, 0.5 + pixelOffset);
	float2 uv2 = uv + _MainTex_TexelSize * float2(-0.5 - pixelOffset, 0.5 + pixelOffset);
	float2 uv3 = uv + _MainTex_TexelSize * float2(0.5 + pixelOffset, -0.5 - pixelOffset);
	float2 uv4 = uv + _MainTex_TexelSize * float2(-0.5 - pixelOffset, -0.5 - pixelOffset);

	float4 sum = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv1);
	sum += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv2);
	sum += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv3);
	sum += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv4);

	sum *= 0.25;

	return sum;
}

float4 fragKawaseBlur0 (v2f i) : SV_Target
{
	return kawaseBlur(i.texcoord, 0);
}

float4 fragKawaseBlur1 (v2f i) : SV_Target
{
	return kawaseBlur(i.texcoord, 1);
}

float4 fragKawaseBlur2 (v2f i) : SV_Target
{
	return kawaseBlur(i.texcoord, 2);
}

float4 fragKawaseBlur3 (v2f i) : SV_Target
{
	return kawaseBlur(i.texcoord, 3);
}