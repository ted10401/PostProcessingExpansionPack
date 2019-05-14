Shader "Rim/Prepass"
{
	SubShader
	{
        Blend One One
        Cull Off
        ZWrite Off
        ZTest Always
        
		Pass
		{      
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct a2f
			{
				fixed4 vertex : POSITION;
                fixed3 normal : NORMAL;
			};
			
			struct v2f
			{
				fixed4 vertex : SV_POSITION;
                fixed3 worldPos : TEXCOORD0;
                fixed3 viewDir : TEXCOORD1;
                fixed3 viewNormal : NORMAL;
                float4 projPos : TEXCOORD2;
			};
            
            sampler2D_float _CameraDepthTexture;
			fixed4 _RimColor;
            fixed _RimPower;
            fixed _RimIntensity;
			
			v2f vert(a2f v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.viewDir = (_WorldSpaceCameraPos - o.worldPos).xyz;
                o.viewNormal = mul(v.normal, unity_WorldToObject);
                o.projPos = ComputeScreenPos(o.vertex);
                COMPUTE_EYEDEPTH(o.projPos.z);
                
				return o;
			}
			
			fixed4 frag(v2f i) : SV_Target
			{
                fixed3 viewDir = normalize(i.viewDir);
                fixed3 viewNormal = normalize(i.viewNormal);
            
                fixed vDotN = dot(viewDir, viewNormal);
                fixed rim = 1 - max(0, vDotN);
                rim = pow(rim, _RimPower) *_RimIntensity;
                
                float sceneZ = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)));
                float zCull = ceil(sceneZ - i.projPos.z + 0.05);
            
				return _RimColor * rim * zCull;
			}
			ENDCG
		}
	}
}