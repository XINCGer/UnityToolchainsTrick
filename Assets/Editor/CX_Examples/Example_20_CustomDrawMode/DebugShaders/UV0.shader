Shader "CustomDebug/UV0"
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

            // vertex input: position, UV
            struct appdata 
            {
                float4 vertex : POSITION;
                float4 texcoord : TEXCOORD0;
            };

            struct v2f 
            {
                float4 pos : SV_POSITION;
                float4 color : COLOR;
            };
            
            v2f vert (appdata v) 
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex );
                o.color = v.texcoord;
                o.color.w = 1.0;
                
                return o;
            }
            
            half4 frag( v2f i ) : SV_Target 
            {
                return i.color;
            }
            ENDCG
        }
    }
}