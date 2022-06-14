Shader "CustomDebug/Color"
{
    SubShader 
    {
        Tags {"Queue"="Transparent" "RenderType"="Transparent" }

        Pass 
        {
            ZWrite On
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata 
            {
                float4 vertex : POSITION;
                float3 color : COLOR;
            };

            struct v2f 
            {
                float4 pos : SV_POSITION;
                fixed4 color : COLOR;
            };
            
            v2f vert (appdata v) 
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex );
                o.color.xyz = v.color.xyz;
                o.color.w = 1.0;
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target { return i.color; }
            ENDCG
        }
    }
}