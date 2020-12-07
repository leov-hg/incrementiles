Shader "Custom/Particles/ExplosionSmoke"
{
    Properties
    {
		[Header(Base Parameters)]
        _ExplosionColor ("Explosion Color", Color) = (1,1,1,1)
        _SmokeColor ("Smoke Color", Color) = (1,1,1,1)
        _DissolveNoise ("Dissolve Noise", 2D) = "white" {}
        _DisplacementStrength ("Displacement Strength", float) = 1

		[Header(Dissolve Parameters)]
		_ExplosionColorDissolveValue ("Explosion Color Dissolve Value", Range(0,1)) = 1
		_SmokeColorDissolveValue ("Smoke Color Dissolve Value", Range(0,1)) = 1

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
        #pragma surface surf Toon vertex:vert noforwardadd exclude_path:deferred noshadow nolightmap nodynlightmap nodirlightmap nofog nometa nolppv noshadowmask 

        sampler2D _DissolveNoise;

        struct Input
        {
            fixed2 uv_DissolveNoise;
			fixed3 screenPos;
			fixed3 worldPos;
			fixed4 color : COLOR;
        };

        fixed4 _ExplosionColor, _SmokeColor;
		fixed _DisplacementStrength, _ExplosionColorDissolveValue, _SmokeColorDissolveValue;

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

		void vert(inout appdata_full v)
		{
			fixed noise = tex2Dlod(_DissolveNoise, fixed4(v.texcoord.xy, 0, 0));

			v.vertex.xyz += v.normal * noise * _DisplacementStrength;
		}

        void surf (Input IN, inout ToonSurfaceOutput o)
        {
            fixed noise = tex2D(_DissolveNoise, IN.uv_DissolveNoise);

			_ExplosionColorDissolveValue = IN.color.r;
			_SmokeColorDissolveValue = 1 - IN.color.a;

			fixed4 col = _ExplosionColor * step(noise, _ExplosionColorDissolveValue) + _SmokeColor * step(_ExplosionColorDissolveValue, noise);

			clip(noise - _SmokeColorDissolveValue - 0.2);

			//Channels
            o.Albedo = col.rgb;
        }
        ENDCG
    }
    FallBack "VertexLit"
}
