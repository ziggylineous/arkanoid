Shader "Blocks/Portal"
{
	Properties
	{
		_ColorA ("Color A", Color) = (1, 1, 1, 1)
		_ColorB ("Color B", Color) = (0, 0, 0, 1)

		_FranjaCount("Franj Count", Range(2, 20)) = 10
		_ColorCount("Color Count", Range(2, 5)) = 2
		_Speed("Move Speed", Float) = 5.0
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
			};

			fixed4 _ColorA;
			fixed4 _ColorB;
			int _FranjaCount;
			int _ColorCount;
			float _Speed;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				int colorIndex = floor((i.uv.x + _Time.x * _Speed) * _FranjaCount) % _ColorCount;
				fixed interpolateValue = colorIndex;
				fixed4 col = lerp(_ColorA, _ColorB, interpolateValue);

				return col;
			}
			ENDCG
		}
	}
}
