Shader "Unlit/Distortion"
{
    Properties
    {
        _DistortionTex ("Distortion Texture", 2D) = "white" {}
		_DistortionStrength ("Distortion Strength", float) = 0
    }
    SubShader
    {
		Tags
		{
			"Queue" = "Transparent"
		}

		GrabPass
		{
			"_BehindTexture"
		}

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
				float4 grabPos : TEXCOORD3;
				float4 screenPosition : TEXCOORD1;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
				o.grabPos = ComputeGrabScreenPos(o.vertex);
				o.screenPosition = ComputeScreenPos(o.vertex);
                return o;
            }

			sampler2D _CameraDepthTexture;

			sampler2D _BehindTexture;
			sampler2D _DistortionTex;
			fixed _DistortionStrength;

            fixed4 frag (v2f i) : SV_Target
            {
				// Retrieve the current depth value of the surface behind the
				// pixel we are currently rendering.
				float existingDepth01 = tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenPosition)).r;
				// Convert the depth from non-linear 0...1 range to linear
				// depth, in Unity units.
				float existingDepthLinear = LinearEyeDepth(existingDepth01);

				// Difference, in Unity units, between the water's surface and the object behind it.
				float depthDifference = existingDepthLinear - i.screenPosition.w;


				//Distortion
				fixed distortionMask = tex2D(_DistortionTex, i.uv + _Time.x) * (1 - step(depthDifference, 0));
				fixed4 behindCol = tex2Dproj(_BehindTexture, i.grabPos + distortionMask * _DistortionStrength);


                return behindCol;
            }
            ENDCG
        }
    }
}
