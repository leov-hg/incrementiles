Shader "Custom/Toon/VertexBlend"
{
    Properties
    {
		[Header(Base Parameters)]
		_Color ("Color", Color) = (0, 0, 0, 1)
		_Specular ("Specular", Color) = (1, 1, 1, 1)

		[Space(20)]
		[Header(Lighting Parameters)]
		_ToonRamp ("Toon Ramp", 2D) = "white" {}
		_RampOffset ("Ramp Offset", float) = 0
		_ShadowTint ("Shadow Tint", Color) = (0, 0, 0, 1)
		_SpecularSize ("Specular Size", float) = 0
		_SpecularFalloff ("Specular Falloff", float) = 0

		[Space(20)]
		[Header(Textures)]
		_1stTex ("1stTex", 2D) = "white" {}
		_Color1 ("Color", Color) = (1,1,1,1)
		_2ndTex ("2stTex", 2D) = "white" {}
		_Color2 ("Color", Color) = (1,1,1,1)
		_3rdTex ("3stTex", 2D) = "white" {}
		_Color3 ("Color", Color) = (1,1,1,1)
		_WorldTexSize ("World Texture Size", Range(0, 0.1)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

		Cull off

        CGPROGRAM

        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Toon fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _1stTex;
        fixed4 _Color1;
        sampler2D _2ndTex;
		fixed4 _Color2;
		sampler2D _3rdTex;
		fixed4 _Color3;
		fixed _WorldTexSize;

        struct Input
        {
            fixed2 uv_MainTex;
			fixed3 worldPos;
			fixed4 color : COLOR;
        };

        fixed4 _Color;
		fixed4 _Emission;
		fixed3 _Specular;

		sampler2D _ToonRamp;
		fixed _RampOffset;
		fixed3 _ShadowTint;
		fixed _SpecularSize;
		fixed _SpecularFalloff;

		uniform fixed3 GLOBAL_LightSource;
		uniform fixed GLOBAL_LightRange;
		uniform fixed GLOBAL_LightIntensity;
		uniform fixed4 GLOBAL_LightTint;

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
            fixed3 Specular;
            fixed Alpha;
            fixed3 Normal;
		};

		fixed4 LightingToon(ToonSurfaceOutput s, float3 lightDir, float3 viewDir, float atten)
		{
			//Usefull Directions
			fixed towardLight = dot(s.Normal, lightDir) / 2 + 0.5;
			fixed towardView = dot(s.Normal, viewDir);
			fixed inverseFresnel = 1 - (dot(viewDir, s.Normal) * -1);

			fixed3 lightIntensity = tex2D(_ToonRamp, towardLight + _RampOffset).rgb;

			fixed attenuationChange = fwidth(atten) * 0.5;
			fixed shadow = smoothstep(0.5 - attenuationChange, 0.5 + attenuationChange, atten);

			lightIntensity = saturate(lightIntensity + _ShadowTint) * shadow;

			fixed3 shadowColor = s.Albedo * _ShadowTint;

			//Specular
			fixed3 reflectionDirection = reflect(lightDir, s.Normal);
			fixed towardReflection = dot(viewDir, -reflectionDirection);
			
			fixed specularFalloff = dot(viewDir, s.Normal);
			specularFalloff = pow(specularFalloff, _SpecularFalloff);
			towardReflection *= specularFalloff;

			fixed specularChange = fwidth(towardReflection);
			fixed specularIntensity = smoothstep(1 - _SpecularSize, 1 - _SpecularSize + specularChange, towardReflection);
			specularIntensity *= shadow;

			fixed4 col;

			col.rgb = lerp(shadowColor, s.Albedo, lightIntensity) * _LightColor0.rgb;
			//col.rgb = s.Albedo * lightIntensity * _LightColor0.rgb;
            col.rgb = lerp(col.rgb, s.Specular * _LightColor0.rgb, saturate(specularIntensity));
			col.a = s.Alpha;

			return col;
		}

        void surf (Input IN, inout ToonSurfaceOutput o)
        {
			//Texture tinted by Color
			fixed4 tex1 = tex2D(_1stTex, (IN.worldPos.xz) * _WorldTexSize) * _Color;
			fixed4 col = lerp(_Color, tex1, IN.color.r);

			//Point lights
			fixed lightDist = distance(GLOBAL_LightSource, IN.worldPos);
			lightDist = 1 - saturate(lightDist / GLOBAL_LightRange);
			fixed lightRamp = tex2D(_ToonRamp, fixed2(lightDist.x, 0.5));
			fixed lightZone = 1 - step(1 - saturate(lightDist / GLOBAL_LightRange), 0);
			fixed light = lightZone * GLOBAL_LightIntensity * lightRamp;


			//Channels
            o.Albedo = col.rgb;
			o.Emission = _Emission;
			o.Specular = _Specular;
        }
        ENDCG
    }
    FallBack "VertexLit"
}
