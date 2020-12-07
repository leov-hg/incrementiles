Shader "Custom/Toon/ColoredTexture + Fresnel"
{
	Properties
	{
		[Header(Base Parameters)]
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGBA)", 2D) = "white" {}

		[Space(20)]
		[Header(Lighting Parameters)]
		_ToonRamp ("Toon Ramp", 2D) = "white" {}
		_RampOffset ("Ramp Offset", float) = 0
		_ShadowTint ("Shadow Tint", Color) = (0, 0, 0, 1)

		[Space(20)]
		[Header(Fresnel Parameters)]
		_FresnelColor ("Fresnel Color", Color) = (1, 1, 1, 1)
		_FresnelStrength ("Fresnel Strength", float) = 1
		_FresnelPower ("Fresnel Power", float) = 1
	}
	SubShader
	{
		Tags 
		{ 
			"RenderType"="Opaque" 
			"Queue"="Geometry" 
		}

		Cull off

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Toon noforwardadd exclude_path:deferred noshadow nolightmap nodynlightmap nodirlightmap nofog nometa nolppv noshadowmask 

		sampler2D _MainTex;

		struct Input
		{
			fixed2 uv_MainTex;
			fixed3 viewDir;
			fixed3 worldNormal;
		};

		fixed4 _Color;

		sampler2D _ToonRamp;
		fixed _RampOffset;
		fixed3 _ShadowTint;

		fixed4 _FresnelColor;
		fixed _FresnelStrength, _FresnelPower;

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

		fixed4 LightingToon(ToonSurfaceOutput s, fixed3 lightDir, fixed atten)
		{
			//Usefull Directions
			fixed towardLight = dot(s.Normal, lightDir) * 0.5 + 0.5;

			fixed3 lightIntensity = tex2D(_ToonRamp, towardLight + _RampOffset).rgb;

			fixed3 shadowColor = s.Albedo * _ShadowTint;

			fixed4 col;

			col.rgb = lerp(shadowColor, s.Albedo, lightIntensity) * _LightColor0.rgb;

			col.a = s.Alpha;

			return col;
		}

		void surf (Input IN, inout ToonSurfaceOutput o)
		{
			fixed fresnel = saturate(dot(IN.worldNormal, IN.viewDir) * _FresnelStrength + _FresnelPower);
			fixed inverseFresnel = 1 - fresnel;
			
			//Texture tinted by Color
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;

			//Channels
			o.Albedo = (c.rgb * fresnel) + (_FresnelColor * inverseFresnel);
			o.Alpha = _Color.a;
		}
		ENDCG
	}
	FallBack "VertexLit"
}
