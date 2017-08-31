// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

#include "UnityCG.cginc"

#if _USEMAINTEX_ON
    UNITY_DECLARE_TEX2D(_MainTex);
    float4 _MainTex_ST;
#endif

#if _USECOLOR_ON
    float4 _Color;
#endif

struct appdata_t
{
    float4 vertex : POSITION;
    #if _USEMAINTEX_ON
        float2 texcoord : TEXCOORD0;
    #endif
};

struct v2f
{
    float4 vertex : SV_POSITION;
    #if _USEMAINTEX_ON
        float2 texcoord : TEXCOORD0;
    #endif
    UNITY_FOG_COORDS(1)
    #if _NEAR_PLANE_FADE_ON
        float fade : TEXCOORD2;
    #endif

	half3 worldNormal : TEXCOORD3;
};

float _Closed;
float _Diameter;
float _RotationAngle;

//Second Tex
sampler2D _SecondTexture;
float _UseSecondTex;
float _terminatorLineWidth;
float4 _litDirection;

//Alternative Tex
sampler2D _AlternativeSurfaceTexture;
float _AlternativeTextureVisibilityCoeff;

//Hitech
float _GridBrightness;
float _GridVerticalSpreading;
float4 _GridColor;
int _NumOfEquatorialLines;
int _NumOfMeridianialLines;
float _GridLineThickness;
float _XCoordOfTheCenter;

v2f vert(appdata_t v)
{
	v.vertex = float4(cos(_Closed*(v.vertex.r + 0.5f)*6.28318530718f)*sin((v.vertex.b + 0.5f)*3.14159265f)*0.5*_Diameter, cos((v.vertex.b + 0.5f)*3.14159265f) *-0.5*_Diameter, sin(_Closed*(v.vertex.r + 0.5f)*6.28318530718f)*sin((v.vertex.b + 0.5f)*3.14159265f)*0.5*_Diameter, 1);

	v2f o;
	o.worldNormal = normalize(v.vertex);
	o.vertex = UnityObjectToClipPos(v.vertex);

	//o.vertex = float4(cos(_Closed*(o.vertex.r + 0.5f)*6.28318530718f)*sin((o.vertex.b + 0.5f)*3.14159265f)*0.5*_Diameter, cos((o.vertex.b + 0.5f)*3.14159265f) *-0.5*_Diameter, sin(_Closed*(o.vertex.r + 0.5f)*6.28318530718f)*sin((o.vertex.b + 0.5f)*3.14159265f)*0.5*_Diameter, 1);

    #if _USEMAINTEX_ON
        o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
    #endif
    
    #if _NEAR_PLANE_FADE_ON
        o.fade = ComputeNearPlaneFadeLinear(v.vertex);
    #endif

    UNITY_TRANSFER_FOG(o, o.vertex);
    return o;
}

float _SecondTextureColorMultiplier;

float4 frag(v2f i) : SV_Target
{
    float4 c;

    #if _USEMAINTEX_ON
		half2 coods = half2(i.texcoord.r*_Closed + _RotationAngle / 360.0h, i.texcoord.g);

		if (_UseSecondTex)
		{
			half4 d = UNITY_SAMPLE_TEX2D(_MainTex, coods);
			half4 n = tex2D(_SecondTexture, coods) + _SecondTextureColorMultiplier*d;
			half dotProd = dot(UnityObjectToWorldNormal(i.worldNormal), _litDirection.xyz);
			c = d.rgba*clamp(lerp(0.0h, 1.0h, (dotProd - _terminatorLineWidth / -2.0h) / _terminatorLineWidth), 0.0h, 1.0h) + n.rgba*clamp(lerp(0.0h, 1.0h, (dotProd*-1.0h - _terminatorLineWidth / -2.0h) / _terminatorLineWidth), 0.0h, 1.0h);
			if (_AlternativeTextureVisibilityCoeff > 0.0h)
			{
				half4 alternative = tex2D(_AlternativeSurfaceTexture, coods);

				c = c*(1.0h - _AlternativeTextureVisibilityCoeff) + alternative*_AlternativeTextureVisibilityCoeff;
			}
		}
		else
		{
			c = UNITY_SAMPLE_TEX2D(_MainTex, coods);
		}
    #else
        c = 1;
    #endif

    #if _USECOLOR_ON
        c *= _Color;
    #endif
        
    UNITY_APPLY_FOG(i.fogCoord, c);

    #if _NEAR_PLANE_FADE_ON
        c.rgb *= i.fade;
    #endif

    return c;
}