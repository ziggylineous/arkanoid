Shader "Blocks/RadialGradient"
{
	Properties
	{
		_CenterColor ("Center Color", Color) = (1, 1, 1, 1)
		_OuterColor("Outer Color", Color) = (0, 0, 0, 1)
		_GradientTexture("Gradient Texture", 2D) = "white" {}
	}

	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			//#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				//UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				//UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}


			fixed4 _CenterColor;
			fixed4 _OuterColor;
			sampler2D _GradientTexture;

			fixed4 frag(v2f input) : COLOR
			{
				fixed4 interpolationValue = tex2D(_GradientTexture, input.uv);
				return lerp(_CenterColor, _OuterColor, interpolationValue.r);
			}

			ENDCG
		}
	}
}