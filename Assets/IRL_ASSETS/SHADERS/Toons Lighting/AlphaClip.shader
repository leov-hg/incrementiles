﻿Shader "Custom/Toon/AlphaClip"
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
        #pragma surface surf Toon noforwardadd exclude_path:deferred nolightmap nodynlightmap nodirlightmap nofog nometa nolppv noshadowmask addshadow

        sampler2D _MainTex;

        struct Input
        {
            fixed2 uv_MainTex;
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

		fixed4 LightingToon(ToonSurfaceOutput s, fixed3 lightDir, fixed atten)
		{
			//Usefull Directions
			fixed towardLight = dot(s.Normal, lightDir) * 0.5 + 0.5;

			fixed3 lightIntensity = tex2D(_ToonRamp, towardLight + _RampOffset).rgb;

			//fixed attenuationChange = fwidth(1) * 0.5;
			//fixed shadow = smoothstep(0.5 - attenuationChange, 0.5 + attenuationChange, 1);

			//lightIntensity = saturate(lightIntensity + _ShadowTint);

			fixed3 shadowColor = s.Albedo * _ShadowTint;

			fixed4 col;

			col.rgb = lerp(shadowColor, s.Albedo, lightIntensity) * _LightColor0.rgb;
			//col.rgb = s.Albedo * lightIntensity * _LightColor0.rgb;

			col.a = s.Alpha;

			return col;
		}

        void surf (Input IN, inout ToonSurfaceOutput o)
        {
			//Texture tinted by Color
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;

			clip(c.a - 0.1);

			//Channels
            o.Albedo = c.rgb;
            o.Alpha = _Color.a;
        }
        ENDCG
    }
    FallBack "VertexLit"
}
