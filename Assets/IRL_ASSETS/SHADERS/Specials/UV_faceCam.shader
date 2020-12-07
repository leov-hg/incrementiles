Shader "Custom/UV_faceCam" 
{
	Properties 
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1, 1, 1, 0.5)
		_TexOffset ("Texture Offset", Vector) = (0, 0, 0, 0)
		_SSUVScale ("UV Scale", Range(0,10)) = 1
		_ClipValue ("Clip Value", float) = 0.1
	}
 
	SubShader 
	{
		Tags 
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType" = "Opaque"
            "DisableBatching"="True"
		}

		Cull off
		//ZWrite off
		//ZTest always
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
 
			sampler2D _MainTex;
			float4 _Color;
			float2 _TexOffset;
			float _SSUVScale, _ClipValue;
			
			uniform fixed GLOBAL_CamSize;

			struct appdata 
			{
				float4 vertex : POSITION;
			};
 
			struct v2f 
			{
				float4 pos : POSITION;
				float4 pos2: TEXCOORD0;
			}; 
 
			float2 GetScreenUV(float2 clipPos, float UVscaleFactor)
			{
				float4 SSobjectPosition = UnityObjectToClipPos (float4(0,0,0,1.0));
				float2 screenUV = float2(clipPos.x,clipPos.y);
				float screenRatio = _ScreenParams.x/_ScreenParams.y;
 
				screenUV.x -= SSobjectPosition.x;
				screenUV.y -= SSobjectPosition.y;
 
				screenUV.x *= screenRatio;
 
				screenUV *= UVscaleFactor;
				//screenUV *= SSobjectPosition.w;
				screenUV *= GLOBAL_CamSize;
 
				return screenUV;
			}; 

			v2f vert(appdata v) 
			{              
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.pos2 = o.pos;
 
				return o;
			}      
 
			half4 frag(v2f i) :COLOR
			{              
				float2 screenUV = GetScreenUV(i.pos2.xy, _SSUVScale);
				float4 screenTexture = tex2D (_MainTex, fixed2(screenUV.x + _TexOffset.x * GLOBAL_CamSize, screenUV.y + _TexOffset.y * GLOBAL_CamSize)) * _Color;
 
				//clip(screenTexture.a - _ClipValue);

				return screenTexture;
			}
			ENDCG              
		} 
	}
	Fallback "Diffuse"
}