Shader "PostEffect"
{
    Properties{
        _MainTex ("Texture", 2D) = "white" {}
        _Right("Right",Color) =  (1, 1, 1, 1)
        _Left("Left",Color) =  (1, 1, 1, 1)
    }
    SubShader
    {
        Cull Off
        ZTest Always
        ZWrite Off

        Tags { "RenderType"="Opaque" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex   : POSITION;
                float2 uv       : TEXCOORD0;
            };
 
            struct v2f
            {
                float2 uv       : TEXCOORD0;
                float4 vertex   : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            fixed4 _Right;
            fixed4 _Left;
 
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                if(i.uv.x > 0.5f)
                {
                    col = col * _Right;
                }
                else
                {
                    col = col * _Left;
                }
                return col;
            }
            ENDCG
        }
    }
}