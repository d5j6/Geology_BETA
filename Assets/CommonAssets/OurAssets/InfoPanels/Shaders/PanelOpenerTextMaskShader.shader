// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "HoloWorld/Panel Opener Text Mask Shader"
{
	Properties
	{
		_MaskLayer("Layer of Mask", Float) = 3
	}

	SubShader{
			Tags{ "RenderType" = "Opaque" "Queue" = "Geometry" }

			Pass{
			Stencil
			{
				Ref[_MaskLayer]
				Comp never
				Fail Incrsat
			}

			CGPROGRAM
#pragma vertex vert
#pragma fragment frag
			struct appdata {
			float4 vertex : POSITION;
		};
		struct v2f {
			float4 pos : SV_POSITION;
		};
		v2f vert(appdata v) {
			v2f o;
			o.pos = UnityObjectToClipPos(v.vertex);
			return o;
		}
		half4 frag(v2f i) : SV_Target{
			return half4(1,0.1h,0,1);
		}
			ENDCG
		}
		}
	}