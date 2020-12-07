Shader "Custom/Outline"
{
	Properties
	{
		[Header(Base Parameters)]
		_Color ("Color", Color) = (1,1,1,1)

		[Space(20)]
		_OutlineWidth ("Outline Width", Range(0, 1)) = 0
		[HDR] _OutlineColor ("Outline Color", Color) = (1, 1, 1, 1)
	}
	SubShader
	{			
		Pass
		{
			Tags 
			{ 
				"RenderType"="Opaque" 
				"Queue"="Geometry-1" 
			}

			ZWrite off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float4 normal : NORMAL;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _OutlineTex;
			float4 _OutlineTex_ST;

			fixed _OutlineWidth;
			fixed4 _OutlineColor;

			uniform fixed4 GLOBAL_CameraOrientation;

			v2f vert (appdata v)
			{
				v2f o;

				v.vertex.xyz += _OutlineWidth * v.normal;

				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _OutlineTex);
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				return _OutlineColor;
			}
			ENDCG
		}
	}
}
