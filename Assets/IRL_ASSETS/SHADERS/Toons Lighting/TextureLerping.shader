Shader "Custom/Toon/TextureLerping"
{
    Properties
    {
		[Header(Base Parameters)]
        _Color1 ("Color 1", Color) = (1,1,1,1)
        _Tex1 ("Tex 1", 2D) = "white" {}
        _Color2 ("Color 2", Color) = (1,1,1,1)
        _Tex2 ("Tex 2", 2D) = "white" {}
		_Lerping ("Lerping Value", Range(0, 1)) = 0

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
        #pragma surface surf Toon noforwardadd exclude_path:deferred noshadow nolightmap nodynlightmap nodirlightmap nofog nometa nolppv noshadowmask 

        sampler2D _Tex1, _Tex2;

        struct Input
        {
            fixed2 uv_Tex1;
            fixed2 uv_Tex2;
        };

        fixed4 _Color1, _Color2;

		sampler2D _ToonRamp;
		fixed _RampOffset;
		fixed3 _ShadowTint;

		fixed _Lerping;

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
            fixed4 c1 = tex2D(_Tex1, IN.uv_Tex1) * _Color1;
            fixed4 c2 = tex2D(_Tex2, IN.uv_Tex2) * _Color2;

			fixed4 c = lerp(c1, c2, _Lerping);

			//Channels
            o.Albedo = c.rgb;
        }
        ENDCG
    }
    FallBack "VertexLit"
}
