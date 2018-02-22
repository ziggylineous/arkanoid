Shader "Board/BallBoardPaint"
{
	Properties
	{
		_PrevFrameAlpha("Prev frame alpha", Float) = 0.5

        _MaxDistance("Max distance", Float) = 100.0

        _Color("Paint color", Color) = (1.0, 0.0, 0.0, 1.0)
	}
	SubShader
	{
		// No culling or depth
		// ZWrite Off ZTest Always

        Cull Off

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
                float2 screen : TEXCOORD1;
			};

            float2 _HalfScreen;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
                o.screen = o.vertex.xy / o.vertex.w;
                o.screen.x = _HalfScreen.x + o.screen.x * _HalfScreen.x;
                o.screen.y = _HalfScreen.y + o.screen.y * _HalfScreen.y;
				return o;
			}

            fixed2 _Position;
            fixed _MaxDistance;

			sampler2D _PrevFrameTex;
            fixed _PrevFrameAlpha;
            fixed4 _Color;


			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 colTex = tex2D(_PrevFrameTex, i.uv);
                

                float distToPos = distance(i.screen.xy, _Position);
                float distNormalized = smoothstep(_MaxDistance, 0.0, distToPos);
                fixed4 col = distNormalized * _Color;
                col += colTex * _PrevFrameAlpha;
                col.a = 1.0;
				
				return col;
			}
			ENDCG
		}
	}
}
