Shader "Custom/UI_ShowLeftSideOnly"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _LeftSideCutoff ("Left Side Cutoff", Range(0,1)) = 0.5
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float _LeftSideCutoff;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                // Controla qué parte de la textura se muestra según el valor de _LeftSideCutoff.
                if (i.uv.x > _LeftSideCutoff)
                {
                    col.a = 0; // Hace que la parte derecha de la textura sea transparente.
                }

                return col;
            }
            ENDCG
        }
    }
}
