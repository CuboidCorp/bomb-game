Shader "Custom/TestBrick"
{
    Properties
    {
        _BrickColor ("Brick Color", Color) = (0.1, 0.1, 0.1, 1)
        _MortarColor ("Mortar Color", Color) = (0.3, 0.3, 0.3, 1)
        _BrickWidth ("Brick Width", Float) = 0.9
        _BrickHeight ("Brick Height", Float) = 0.3
        _MortarThickness ("Mortar Thickness", Range(0.0, 0.1)) = 0.02
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

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

            float4 _BrickColor;
            float4 _MortarColor;
            float _BrickWidth;
            float _BrickHeight;
            float _MortarThickness;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 pos = i.uv / float2(_BrickWidth, _BrickHeight);
                float2 offset = float2(0.5 * (step(1.0, fmod(pos.y, 2.0))), 0.0);
                pos = frac(pos + offset);
                
                float2 useBrick = step(_MortarThickness, pos) * step(_MortarThickness, 1.0 - pos);
                float isBrick = useBrick.x * useBrick.y;
                
                return lerp(_MortarColor, _BrickColor, isBrick);
            }
            ENDCG
        }
    }
}