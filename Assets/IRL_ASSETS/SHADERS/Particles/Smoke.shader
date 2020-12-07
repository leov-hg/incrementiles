Shader "Custom/Particles/SmokeToon"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _DissolveTex ("Dissolve Texture", 2D) = "white" {}
		[HDR] _EmissiveColor ("Emissive Color", Color) = (1, 1, 1, 1)
		_DissolveValue ("Dissolve Value", Range(0, 1)) = 0
    }
    SubShader
    {
        Tags 
		{ 
			"LightMode" = "ForwardBase"
			"PassFlags" = "OnlyDirectional"
		}

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
			#include "Lighting.cginc"

            struct appdata
            {
				float2 uv : TEXCOORD0;
                float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 color : COLOR;
            };

            struct v2f
            {
				float2 uv : TEXCOORD0;
				float3 worldPos : TEXCOORD1;
                float4 vertex : SV_POSITION;
				float3 worldNormal : NORMAL;
				float4 color : COLOR;
            };

			sampler2D _DissolveTex, _MainTex;
            fixed4 _EndColor;
			fixed4 _EmissiveColor;
			fixed _DissolveValue;

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
				float dissolve = tex2D(_DissolveTex, i.uv);
				float4 tex = tex2D(_MainTex, i.uv);

				//clip
				clip(dissolve - (1 - i.color.a) - (1 - tex.a));

				//fixed4 col = lerp(i.color, _EndColor, _DissolveValue);

                return i.color * _EmissiveColor;
            }
            ENDCG
        }
    }
}
