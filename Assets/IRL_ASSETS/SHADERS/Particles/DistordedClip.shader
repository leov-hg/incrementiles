Shader "Custom/Particles/DistordedClip"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _DissolveTex ("Dissolve Texture", 2D) = "white" {}
		[HDR] _EmissiveColor ("Emissive Color", Color) = (1, 1, 1, 1)
		_DissolveValue ("Dissolve Value", Range(0, 1)) = 0
		[Space(20)]
		_DistortionTex ("Distortion Texture", 2D) = "black" {}
		_DistortionStrength ("Distortion Strength", float) = 1
		_DistortionSpeed ("Distortion Speed", float) = 1
    }
    SubShader
    {
        Tags 
		{ 
			"LightMode" = "ForwardBase"
			"PassFlags" = "OnlyDirectional"
			"Queue" = "Transparent"
		}

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
			#include "Lighting.cginc"

            struct appdata
            {
				float2 uv : TEXCOORD0;
                float4 vertex : POSITION;
				float4 color : COLOR;
            };

            struct v2f
            {
				float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
				float4 color : COLOR;
            };

			sampler2D _DissolveTex, _MainTex, _DistortionTex;
			fixed4 _DissolveTex_ST, _MainTex_ST, _DistortionTex_ST;
            fixed4 _EndColor;
			fixed4 _EmissiveColor;
			fixed _DissolveValue, _DistortionStrength, _DistortionSpeed;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.color = v.color;

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				float dissolve = tex2D(_DissolveTex, TRANSFORM_TEX(i.uv, _DissolveTex));
				float distortion = tex2D(_DistortionTex, TRANSFORM_TEX(i.uv, _DistortionTex) + _Time.x * _DistortionSpeed);
				float tex = tex2D(_MainTex, i.uv + (distortion * 2 - 1) * _DistortionStrength).a;

				//clip
				clip(dissolve * tex - (1 - i.color.a + 0.01));

				//fixed4 col = lerp(i.color, _EndColor, _DissolveValue);

                return i.color * _EmissiveColor;
            }
            ENDCG
        }
    }
}
