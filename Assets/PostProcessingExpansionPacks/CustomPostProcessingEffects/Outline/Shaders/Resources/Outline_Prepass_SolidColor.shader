Shader "Outline/Prepass/SolidColor"
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
			};
            
			fixed4 _Color;
			
			v2f vert(a2f v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
                
				return o;
			}
			
			fixed4 frag(v2f i) : SV_Target
			{
				return _Color;
			}
			ENDCG
		}
	}
}