Shader "Custom/Toon/Triplanar"
{
	Properties
	{
		[Header(Base Parameters)]
		_Color ("Color", Color) = (1, 1, 1, 1)
		_MainTex("Albedo (RGBA)", 2D) = "white" {}
		_Sharpness("Sharpness", float) = 1

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

			sampler2D _MainTex;
			float4 _MainTex_ST;

			struct Input
			{
				fixed2 uv_MainTex;
				fixed3 worldPos;
				fixed3 worldNormal;
			};

			fixed4 _Color;
			fixed4 _Emission;
			fixed _Sharpness;

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
				//calculate UV coordinates for three projections
				float2 uv_front = TRANSFORM_TEX(IN.worldPos.xy, _MainTex);
				float2 uv_side = TRANSFORM_TEX(IN.worldPos.zy, _MainTex);
				float2 uv_top = TRANSFORM_TEX(IN.worldPos.xz, _MainTex);

				//read texture at uv position of the three projections
				fixed4 col_front = tex2D(_MainTex, uv_front);
				fixed4 col_side = tex2D(_MainTex, uv_side);
				fixed4 col_top = tex2D(_MainTex, uv_top);

				//generate weights from world normals
				float3 weights = IN.worldNormal;
				//show texture on both sides of the object (positive and negative)
				weights = abs(weights);
				//make the transition sharper
				weights = pow(weights, _Sharpness);
				//make it so the sum of all components is 1
				weights = weights / (weights.x + weights.y + weights.z);

				//combine weights with projected colors
				col_front *= weights.z;
				col_side *= weights.x;
				col_top *= weights.y;

				//combine the projected colors
				fixed4 col = col_front + col_side + col_top;

				//multiply texture color with tint color
				col *= _Color;

				//Channels
				o.Albedo = col.rgb;
				o.Emission = _Emission;
				o.Alpha = _Color.a;
			}
			ENDCG
		}
			FallBack "VertexLit"
}
