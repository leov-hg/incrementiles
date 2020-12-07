Shader "Custom/Vignette" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_VignettePower ("VignettePower", Range(0.0,6.0)) = 5.5
        _VignetteCenter ("VignetteCenter", Vector) = (0.5, 0.5, 0.0, 0.0)
	}
	SubShader 
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#include "UnityCG.cginc"

			uniform sampler2D _MainTex;
			uniform fixed4 _VignetteColor;
			uniform fixed _VignettePower;
			uniform fixed2 _VignetteCenter;

			struct v2f
			{
				float2 texcoord	: TEXCOORD0;
			};
			
			float4 frag(v2f_img i) : COLOR
			{
				float4 tex = tex2D(_MainTex, i.uv);
				
				fixed2 dist = ((i.uv + _VignetteCenter) - 0.5);
				dist.x = 1 - dot(dist, dist) * 1.5;
				//tex *= dist.x;

				float4 col = _VignetteColor * (1 - saturate(dot(dist, dist) * 1.5));
				
				return tex + col * _VignettePower;
			}

			ENDCG
		} 
	}
}    