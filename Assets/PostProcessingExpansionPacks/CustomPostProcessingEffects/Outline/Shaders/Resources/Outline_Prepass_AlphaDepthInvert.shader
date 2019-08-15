Shader "Outline/Prepass/AlphaDepthInvert"
{
	SubShader
	{
        Blend SrcAlpha OneMinusSrcAlpha
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
                float2 texcoord : TEXCOORD0;
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
                float4 projPos : TEXCOORD0;
                float2 texcoord : TEXCOORD1;
			};
            
            sampler2D_float _CameraDepthTexture;
            sampler _MainTex;
			fixed4 _Color;
			
			v2f vert(a2f v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                o.projPos = ComputeScreenPos(o.vertex);
                COMPUTE_EYEDEPTH(o.projPos.z);
                
				return o;
			}
			
			fixed4 frag(v2f i) : SV_Target
			{
                float sceneZ = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)));
                float zCull = ceil(sceneZ - i.projPos.z + 0.95);
				zCull = 1 - zCull;
                
                float alpha = tex2D(_MainTex, i.texcoord).a;
                alpha = floor(alpha);
            
				return _Color * zCull * alpha;
			}
			ENDCG
		}
	}
}