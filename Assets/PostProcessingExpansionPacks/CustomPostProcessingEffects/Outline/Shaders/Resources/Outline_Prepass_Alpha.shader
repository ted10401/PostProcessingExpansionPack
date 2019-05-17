Shader "Outline/Prepass/Alpha"
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
                fixed2 texcoord : TEXCOORD0;
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
                fixed2 texcoord : TEXCOORD0;
			};
            
            sampler2D _MainTex;
			fixed4 _Color;
			
			v2f vert(a2f v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                
				return o;
			}
			
			fixed4 frag(v2f i) : SV_Target
			{
                float alpha = tex2D(_MainTex, i.texcoord).a;
                alpha = floor(alpha);
                
				return _Color * alpha;
			}
			ENDCG
		}
	}
}