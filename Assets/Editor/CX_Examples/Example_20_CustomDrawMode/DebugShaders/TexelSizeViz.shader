Shader "CustomDebug/TexelSizeViz"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _MipTex ("Texel Map (RGB)", 2D) = "red" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        float4 _MainTex_TexelSize;
        float MipLevel = 0.0;
        sampler2D _MipTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Get appropriate Mip Level according to Texture Size (height).
            MipLevel = log2(4096 / _MainTex_TexelSize.z);
            // Display Mip Level of _MipTex instead of albedo texture.
            fixed4 c = tex2Dlod (_MipTex, float4(IN.uv_MainTex.xy, 0, MipLevel));
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = 1.0;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
