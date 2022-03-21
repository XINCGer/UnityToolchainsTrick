Shader "CXTool/CreateVFXImage(AphaBlend)_Shader"
{
    Properties
    {
        _WhiteBgTex ("Texture", 2D) = "white" {}//ÏÈÅÄÉãµÄ°×É«±³¾°µÄÍ¼Æ¬
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

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

            sampler2D _MainTex;
            sampler2D _WhiteBgTex;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 colWBg = tex2D(_WhiteBgTex, i.uv);
                // just invert the colors
                fixed apha = 1 - (colWBg.x - col.x);
                if(apha == 0)
                {
                    col = fixed4(0.0,0.0,0.0,0.0);
                }
                else
                {
                    col.w = apha;
                }
                return col;
            }
            ENDCG
        }
    }
}
