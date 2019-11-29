Shader "Postprocess/Fade"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Percent("Percent",Range(0,1)) = 0
    }
    SubShader
    {
		Cull Off
		ZWrite Off
		ZTest Always
		Pass
		{
			CGPROGRAM

			#include "UnityCG.cginc"

			#pragma vertex vert_img
			#pragma fragment frag

            sampler2D _MainTex;
            half _Percent;

			//色を決める関数
			fixed4 frag(v2f_img i) : COLOR
			{
                fixed4 col = tex2D(_MainTex, i.uv);
                col = col * (1-_Percent);
                return col;
			}
			ENDCG
		}
    }
    FallBack "Diffuse"
}