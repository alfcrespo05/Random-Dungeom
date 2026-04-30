Shader "Custom/OutlineSprite"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _OutlineSize ("Outline Size", Float) = 2
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _MainTex_TexelSize;
            float4 _OutlineColor;
            float _OutlineSize;

            struct appdata { float4 vertex : POSITION; float2 uv : TEXCOORD0; };
            struct v2f { float2 uv : TEXCOORD0; float4 vertex : SV_POSITION; };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                
                float2 offset = _MainTex_TexelSize.xy * _OutlineSize;
                float a = tex2D(_MainTex, i.uv + float2(offset.x, 0)).a;
                a = max(a, tex2D(_MainTex, i.uv - float2(offset.x, 0)).a);
                a = max(a, tex2D(_MainTex, i.uv + float2(0, offset.y)).a);
                a = max(a, tex2D(_MainTex, i.uv - float2(0, offset.y)).a);

                if (col.a < 0.1 && a > 0.1)
                    return _OutlineColor;

                return col;
            }
            ENDCG
        }
    }
}