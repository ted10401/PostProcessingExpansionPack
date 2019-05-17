Shader "Outline/Prepass/SolidColorDepth"
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
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
                float4 projPos : TEXCOORD0;
			};
            
            sampler2D_float _CameraDepthTexture;
			fixed4 _Color;
			
			v2f vert(a2f v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
                o.projPos = ComputeScreenPos(o.vertex);
                COMPUTE_EYEDEPTH(o.projPos.z);
                
				return o;
			}
			
			fixed4 frag(v2f i) : SV_Target
			{
                float sceneZ = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)));
                float zCull = ceil(sceneZ - i.projPos.z + 0.5);
            
				return _Color * zCull;
			}
			ENDCG
		}
	}
}