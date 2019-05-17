Shader "Rim/Prepass/Invert"
{
	Properties
	{
		_RimColor("Rim Color", Color) = (1, 1, 1, 1)
		_RimPower("Rim Power", Float) = 1
		_RimIntensity("Rim Intensity", Float) = 1
	}
	SubShader
	{
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Back
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
				fixed3 normal : NORMAL;
				fixed3 viewDir : TEXCOORD0;
                float4 projPos : TEXCOORD1;
			};
            
            sampler2D_float _CameraDepthTexture;
			fixed4 _RimColor;
            fixed _RimPower;
            fixed _RimIntensity;
			
			v2f vert(a2f v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.normal = v.normal;
				o.viewDir = ObjSpaceViewDir(v.vertex);
                o.projPos = ComputeScreenPos(o.vertex);
                COMPUTE_EYEDEPTH(o.projPos.z);
                
				return o;
			}
			
			fixed4 frag(v2f i) : SV_Target
			{
				fixed3 normal = normalize(i.normal);
				fixed3 viewDir = normalize(i.viewDir);
				fixed rim = 1.0 - saturate(dot(normal, viewDir));
				rim = pow(rim, _RimPower) *_RimIntensity;
                
                float sceneZ = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)));
                float zCull = ceil(sceneZ - i.projPos.z + 0.95);
				zCull = 1 - zCull;
            
				return _RimColor * rim * zCull;
			}
			ENDCG
		}
	}
}