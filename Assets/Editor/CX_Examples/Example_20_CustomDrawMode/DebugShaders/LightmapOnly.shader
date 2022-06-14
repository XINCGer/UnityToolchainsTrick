Shader "CustomDebug/Lightmap Shader" {

  SubShader {
    Tags { "RenderType" = "Opaque" }

    Pass {
      // Disable lighting, only use lightmap.
      Lighting Off

      CGPROGRAM
      #pragma vertex vert
      #pragma fragment frag

      #include "UnityCG.cginc"

      struct v2f {
        float4 pos : SV_POSITION;
        float2 uv1 : TEXCOORD1;
      };

      struct appdata_lightmap {
        float4 vertex : POSITION;
        float2 texcoord1 : TEXCOORD1;
      };

      v2f vert(appdata_lightmap i) {
        v2f o;
        o.pos = UnityObjectToClipPos(i.vertex);
        o.uv1 = i.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
        return o;
      }

      half4 frag(v2f i) : COLOR {
        half4 lightmap_color;
        lightmap_color.rgb = DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap, i.uv1));
        lightmap_color.a = 1.0;
        return lightmap_color;
      }
      ENDCG
    }
  }
}