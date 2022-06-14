Shader "CustomDebug/MipCheckerboard"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Density ("Density", Range(2,50)) = 30
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }

        Pass
        {
            Lighting Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"


            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
			float4 _MainTex_ST;
            float4 _MainTex_TexelSize;
            float MipFactor = 0.0;
            float _Density;

            v2f vert (float4 pos : POSITION, float2 uv : TEXCOORD0)
            {
                // Get appropriate Mip Level according to Texture Size (height).
                MipFactor = 4096 / _MainTex_TexelSize.z;
                v2f o;
                o.vertex = UnityObjectToClipPos(pos);
                o.uv = uv * _Density / MipFactor;
				o.uv = TRANSFORM_TEX(o.uv, _MainTex);
                return o;
            }
            
            half4 frag (v2f i) : COLOR
            {
                float2 c = i.uv;
                c = floor(c) / 2;
                float checker = frac(c.x + c.y) * 2;
                return half4(checker,checker,checker,1);
            }
            ENDCG
        }
    }
}