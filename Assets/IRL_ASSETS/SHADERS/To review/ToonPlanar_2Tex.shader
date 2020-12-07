Shader "Custom/Toon/Planar_2Tex"
{
	Properties
	{
		[Header(Base Parameters)]
		_Color ("Color", Color) = (1, 1, 1, 1)
		_MainTex("Main Texture", 2D) = "white" {}
		_SecondTex("Second Texture", 2D) = "white" {}
		[Space(20)]
		_BlendingNoise ("Blending Noise", 2D) = "white" {}

		[Space(20)]
		[Header(Lighting Parameters)]
		_ToonRamp("Toon Ramp", 2D) = "white" {}
		_RampOffset("Ramp Offset", float) = 0
		_ShadowTint("Shadow Tint", Color) = (0, 0, 0, 1)
	}
		SubShader
		{
			Tags
			{
				"RenderType" = "Opaque"
				"Queue" = "Geometry"
			}

			CGPROGRAM
			// Physically based Standard lighting model, and enable shadows on all light types
			#pragma surface surf Toon noforwardadd exclude_path:deferred nolightmap nodynlightmap nodirlightmap nofog nometa nolppv noshadowmask 

			sampler2D _MainTex, _SecondTex, _BlendingNoise;
			fixed4 _MainTex_ST, _SecondTex_ST, _BlendingNoise_ST;

			struct Input
			{
				fixed2 uv_MainTex;
				fixed2 uv2_SecondTex;
				fixed2 uv3_BlendingNoise;
				fixed3 screenPos;
				fixed3 worldPos;
			};

			fixed4 _Color;

			sampler2D _ToonRamp;
			fixed _RampOffset;
			fixed3 _ShadowTint;

			// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
			// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
			// #pragma instancing_options assumeuniformscaling
			UNITY_INSTANCING_BUFFER_START(Props)
				// put more per-instance properties here
			UNITY_INSTANCING_BUFFER_END(Props)

			struct ToonSurfaceOutput
			{
				fixed3 Albedo;
				fixed3 Emission;
				fixed Alpha;
				fixed3 Normal;
			};

			fixed4 LightingToon(ToonSurfaceOutput s, float3 lightDir, float atten)
			{
				//Usefull Directions
				fixed towardLight = dot(s.Normal, lightDir) * 0.5 + 0.5;

				fixed3 lightIntensity = tex2D(_ToonRamp, towardLight + _RampOffset).rgb;

				fixed attenuationChange = fwidth(atten) * 0.5;
				fixed shadow = smoothstep(0.5 - attenuationChange, 0.5 + attenuationChange, atten);

				lightIntensity = saturate(lightIntensity + _ShadowTint) * shadow;

				fixed3 shadowColor = s.Albedo * _ShadowTint;

				fixed4 col;

				col.rgb = lerp(shadowColor, s.Albedo, lightIntensity) * _LightColor0.rgb;
				//col.rgb = s.Albedo * lightIntensity * _LightColor0.rgb;

				col.a = s.Alpha;

				return col;
			}

			void surf(Input IN, inout ToonSurfaceOutput o)
			{
				fixed2 mainUV = TRANSFORM_TEX(IN.worldPos.xz, _MainTex);
				fixed2 secondUV = TRANSFORM_TEX(IN.worldPos.xz, _SecondTex);
				fixed2 noiseUV = TRANSFORM_TEX(IN.worldPos.xz, _BlendingNoise);

				fixed noise = tex2D(_BlendingNoise, noiseUV);
				fixed4 col = lerp(tex2D(_MainTex, mainUV), tex2D(_SecondTex, secondUV), noise) * _Color;

				//Channels
				o.Albedo = col.rgb;
				o.Alpha = _Color.a;
			}
			ENDCG
		}
			FallBack "VertexLit"
}
