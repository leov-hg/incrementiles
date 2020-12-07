Shader "Custom/Water/Complex"
{
    Properties
    {
		[Header(Displacement)]
        _DisplacementTex ("Displacement Texture", 2D) = "white" {}
		_DisplacementTexPan ("Displacement Strength Pan", Vector) = (0, 0, 0, 0)
		_DisplacementTexScale ("Displacement Strength Scale", float) = 1
		_DisplacementStrength ("Displacement Strength", float) = 1

		[Header(Caustics)]
        _Caustics ("Texture", 2D) = "white" {}
		_CausticsPan ("Caustics Pan", Vector) = (0, 0, 0, 0)
		_CausticsScale ("Caustics Scale", float) = 1
		_CausticsStrength ("Caustics Strength", float) = 1
		_CausticsPower ("Caustics Power", float) = 1
		_CausticsLevel ("Caustics Clamp", float) = 1

		[Header(Deformation)]
		_DeformationTex ("Deformation Texture", 2D) = "white" {}
		_DeformationScale ("Deformation Scale", float) = 1
		_DeformationPan ("Deformation Pan", Vector) = (0, 0, 0, 0)
		_DeformationStrength ("Deformation Strength", float) = 1

		[Header(Colors)]
		_Color ("Color", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

		Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                fixed2 uv : TEXCOORD0;
                fixed4 vertex : SV_POSITION;
				fixed4 worldPos : TEXCOORD1;
            };

			sampler2D _DisplacementTex;
			fixed2 _DisplacementTexPan;
			fixed _DisplacementTexScale;
			fixed _DisplacementStrength;

            sampler2D _Caustics;
            fixed4 _Caustics_ST;
			fixed2 _CausticsPan;
			fixed _CausticsScale;
			fixed _CausticsStrength;
			fixed _CausticsPower;
			fixed _CausticsLevel;

			sampler2D _DeformationTex;
			fixed _DeformationScale;
			fixed2 _DeformationPan;
			fixed _DeformationStrength;

			fixed4 _Color;

            v2f vert (appdata v)
            {
                v2f o;

				o.worldPos = mul(unity_ObjectToWorld, v.vertex);

				//Map caustics over world
				fixed disp = tex2Dlod(_DisplacementTex, fixed4(o.worldPos.x + _Time.y * _DisplacementTexPan.x, o.worldPos.z + _Time.y * _DisplacementTexPan.y, 0, 0) / _DisplacementTexScale);

				v.vertex.y += disp * _DisplacementStrength;

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _Caustics);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
				fixed disp = tex2D(_DisplacementTex, fixed2(i.worldPos.x + _Time.y * _DisplacementTexPan.x + _Time.y * _DisplacementTexPan.y, i.worldPos.z) / _DisplacementTexScale);
                fixed causticDeform = tex2D(_Caustics, fixed2(i.uv.x + _Time.x * _DeformationPan.x, i.uv.y + _Time.x * _DeformationPan.y)) * 2 - 1;
                fixed caustics = pow(tex2D(_Caustics, fixed2(i.uv.x + _Time.x * _CausticsPan.x, i.uv.y + _Time.x * _CausticsPan.y) + causticDeform * _DeformationStrength * disp), _CausticsPower) * _CausticsStrength - _CausticsLevel;



				fixed deformation = tex2D(_DeformationTex, fixed2(i.worldPos.x + _Time.y * _DeformationPan.x, i.worldPos.z + _Time.y * _DeformationPan.y) * _DeformationScale) * _DeformationStrength;

				fixed4 foamSurface = tex2D(_Caustics, fixed2(i.worldPos.x + deformation + _Time.y * _CausticsPan.x, i.worldPos.z + deformation + _Time.y * _CausticsPan.y) * _CausticsScale) * _CausticsStrength * _Color;


                return foamSurface;
				//return disp;
            }
            ENDCG
        }
    }
}
