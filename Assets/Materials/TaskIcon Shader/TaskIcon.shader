Shader "Unlit/SpriteUnlit"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        [HDR]_HDRColor("HDR Color", Color) = (1, 1, 1, 1)
        _AlphaClip("Alpha Clip", Range(0, 1)) = 0.5
    }

        SubShader
        {
            Tags { "Queue" = "Overlay+1" "RenderType" = "Transparent" }
            ZTest Always
            LOD 100

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                struct appdata_t
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f
                {
                    float2 uv : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                };

                sampler2D _MainTex;
                float4 _MainTex_ST;
                float4 _Color;
                float4 _HDRColor;
                float _AlphaClip;

                v2f vert(appdata_t v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    return o;
                }

                half4 frag(v2f i) : SV_Target
                {
                    half4 col = tex2D(_MainTex, i.uv) * _HDRColor;
                    clip(col.a - _AlphaClip);
                    return col;
                }
                ENDCG
            }
        }
}