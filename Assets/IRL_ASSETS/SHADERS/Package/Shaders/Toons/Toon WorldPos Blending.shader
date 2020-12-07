Shader "Roystan/ToonWorldPosBlending"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Main Texture", 2D) = "white" {}
		_SecondTex("Second Texture", 2D) = "white" {}
		// Ambient light is applied uniformly to all surfaces on the object.
		[HDR]
		_AmbientColor("Ambient Color", Color) = (0.4,0.4,0.4,1)
		_NoiseTex ("Noise Texture", 2D) = "white" {}

		_Tiling ("Tiling", Vector) = (1,1,1)
		_SecondTiling ("SecondTiling", Vector) = (1,1,1)
		_NoiseTiling ("Noise Tiling", Vector) = (1,1,1)
		_NoiseStrength ("Noise Strength", float) = 1

		_GlowTex ("Glow Texture", 2D) = "white" {}
		_GlowColor ("Glow Color", Color) = (1, 1, 1, 1)
		_Side ("Side", Range(0, 1)) = 0
	}
	SubShader
	{
		Pass
		{
			// Setup our pass to use Forward rendering, and only receive
			// data on the main directional light and ambient light.
			Tags
			{
				"LightMode" = "ForwardBase"
				"PassFlags" = "OnlyDirectional"
			}

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// Compile multiple versions of this shader depending on lighting settings.
			#pragma multi_compile_fwdbase
			
			#include "UnityCG.cginc"
			// Files below include macros and functions to assist
			// with lighting and shadows.
			#include "Lighting.cginc"
			#include "AutoLight.cginc"

			struct appdata
			{
				float4 vertex : POSITION;				
				float4 worldPos : TEXCOORD0;
				float2 uv : TEXCOORD1;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float3 worldNormal : NORMAL;
				float3 worldPos : TEXCOORD0;
				float2 uv : TEXCOORD1;
				// Macro found in Autolight.cginc. Declares a vector4
				// into the TEXCOORD2 semantic with varying precision 
				// depending on platform target.
			};

			sampler2D _MainTex;
			fixed4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.worldNormal = UnityObjectToWorldNormal(v.normal);		
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			float4 _Color;

			fixed4 _AmbientColor;
			fixed3 _Tiling;
			fixed3 _SecondTiling;
			fixed3 _NoiseTiling;
			fixed _NoiseStrength;

			sampler2D _NoiseTex;
			sampler2D _SecondTex;

			fixed3 _Source;
			fixed _Side;
			fixed _Range;
			fixed _Activation;

			sampler2D _GlowTex;
			fixed4 _GlowTex_ST;
			fixed4 _GlowColor;
			fixed _GlowRange;
			fixed _GlowStrength;

			float4 frag (v2f i) : SV_Target
			{
				fixed3 normal = normalize(i.worldNormal);

				// Lighting below is calculated using Blinn-Phong,
				// with values thresholded to creat the "toon" look.
				// https://en.wikipedia.org/wiki/Blinn-Phong_shading_model

				// Calculate illumination from directional light.
				// _WorldSpaceLightPos0 is a vector pointing the OPPOSITE
				// direction of the main directional light.
				fixed NdotL = dot(_WorldSpaceLightPos0, normal);

				// Partition the intensity into light and dark, smoothly interpolated
				// between the two to avoid a jagged break.
				fixed lightIntensity = smoothstep(0, 0.01, NdotL);	
				// Multiply by the main directional light's intensity and color.
				fixed4 light = _LightColor0;

				fixed noiseSample = tex2D(_NoiseTex, i.worldPos.xz * _NoiseTiling);
				fixed4 sample = lerp(tex2D(_MainTex, i.worldPos.xz * _Tiling), tex2D(_SecondTex, i.worldPos.xz * _SecondTiling), clamp(noiseSample * _NoiseStrength, 0, 1));


				//CLIPPING
				//fixed dis = distance(_Source * _Activation, i.worldPos);
				//fixed sphere = 1 - saturate(lerp(dis / _Range, _Range / dis, _Side));
				//clip(-sphere);

				//GLOWING CLIPPING
				fixed2 uv_glowTex = TRANSFORM_TEX(i.uv, _GlowTex);
				fixed4 glowTex = tex2D(_GlowTex, uv_glowTex);

				_GlowRange = 0.02;
				_GlowStrength = 100;

				//fixed sphereGlow = saturate(((1 - lerp(dis / _Range, _Range / dis, _Side - (glowTex / 2)) + (_GlowRange / _Range)) * (_GlowStrength * _Range)));
				//fixed4 glow = sphereGlow * _GlowColor;


				return (light + _AmbientColor) * _Color * sample/* + glow*/;
			}
			ENDCG
		}

		// Shadow casting support.
        UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
	}
}