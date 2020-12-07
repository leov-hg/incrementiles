Shader "Custom/Particles/ThinSmoke"
{
    Properties
    {
        _DistortionTex ("Distortion Texture", 2D) = "gray" {}
		_Speed ("Distortion Speed", Vector) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

		Blend SrcAlpha OneMinusSrcAlpha

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
				float4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
				float4 color : COLOR;
            };

            sampler2D _DistortionTex;
            float4 _DistortionTex_ST;
			fixed2 _Speed;

            v2f vert (appdata v)
            {
                v2f o;

				//Move vertices
				fixed3 worldPos = mul(unity_ObjectToWorld, v.vertex);
				fixed distortion = tex2Dlod(_DistortionTex, fixed4(worldPos.x * _DistortionTex_ST.x + _Time.y * _Speed.x, worldPos.y * _DistortionTex_ST.y + _Time.y * _Speed.y, 0, 0));
				v.vertex += distortion * 2 - 1;

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _DistortionTex);
                o.color = v.color;

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return i.color;
            }
            ENDCG
        }
    }
}
