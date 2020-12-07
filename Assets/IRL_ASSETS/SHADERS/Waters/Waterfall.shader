Shader "Custom/Waterfall"
{
    Properties
    {
		[Header(Water)]
		_WaterColor ("Water Color", Color) = (1, 1, 1, 1)

		[Header(Foam)]
		_FoamTex ("Foam Texture", 2D) = "white" {}
		_FoamColor ("Foam Color", Color) = (1, 1, 1, 1)
		_FoamPanSpeed ("Foam Pan Speed", float) = 1

		[Header(Deformation)]
		_DeformationTex ("Deformation Texture", 2D) = "white" {}
		_DeformationStrength ("Deformation Strength", float) = 1
    }
    SubShader
    {
        Tags {
		
			"RenderType"="Transparent" 
			"Queue"="Transparent" 
		}
        LOD 100

		ZWrite off
		Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                fixed4 vertex : POSITION;
				fixed2 uv : TEXCOORD;
				fixed2 uv2 : TEXCOORD1;
            };

            struct v2f
            {
                fixed4 vertex : SV_POSITION;
				fixed2 uv : TEXCOORD;
				fixed2 uv2 : TEXCOORD1;
            };

			sampler2D _FoamTex, _DeformationTex;
			fixed4 _FoamTex_ST, _DeformationTex_ST;
			fixed4 _WaterColor, _FoamColor;
			fixed _FoamPanSpeed, _DeformationStrength;

            v2f vert (appdata v)
            {
                v2f o;

                o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _FoamTex);
				o.uv2 = TRANSFORM_TEX(v.uv2, _DeformationTex);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				fixed deformation = tex2D(_DeformationTex, i.uv2);

				fixed foam = tex2D(_FoamTex, fixed2(i.uv.y + _Time.y * _FoamPanSpeed + deformation * _DeformationStrength, i.uv.x + deformation * _DeformationStrength)).a;

				fixed4 water = _WaterColor * (1 - foam);

				fixed4 col = foam + water;

                return water + foam * _FoamColor;
            }
            ENDCG
        }
    }
}
