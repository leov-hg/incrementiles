Shader "Custom/Fog"
{
    Properties
    {
        _Strength ("Strength", float) = 1
        _Tint ("Tint", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags { "Queue"="Transparent" }

		Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
				float4 screenPos : TEXCOORD;
            };

			sampler2D _CameraDepthTexture;
			fixed _Strength;
			fixed4 _PlaneColor, _Tint;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.screenPos = ComputeScreenPos(o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				half depth = LinearEyeDepth(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenPos)).r);

				half fog = (_Strength * (depth - i.screenPos.w));
				half4 col = saturate(fog) * _Tint;

                return col;
            }
            ENDCG
        }
    }
}
