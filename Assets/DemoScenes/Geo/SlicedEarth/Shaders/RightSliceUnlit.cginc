// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

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
};

float _Closed;
float _DiameterOuter;
float _DiameterInner;

//heat and flow
sampler2D _HeatMap;
sampler2D _HeatDistortionMap;

float _HeatStrength;
float _HeatSpeed;
float _HeatEdgeFade;
float _HeatHazeSizes;

v2f vert(appdata_t v)
{
	if (v.vertex.r < 0)
	{
		v.vertex = float4(sin((v.vertex.b + 0.5)*3.14159265f)*0.5*_DiameterInner, cos((v.vertex.b + 0.5)*3.14159265f) *-0.5*_DiameterInner, 0, 1);
	}
	else
	{
		v.vertex = float4(sin((v.vertex.b + 0.5)*3.14159265f)*0.5*_DiameterOuter, cos((v.vertex.b + 0.5) * 3.14159265f) *-0.5*_DiameterOuter, 0, 1);
	}

	v2f o;
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

float4 frag(v2f i) : SV_Target
{
	half4 c;

    #if _USEMAINTEX_ON
		half2 coods = half2(0.5h + 0.5h* sin(i.texcoord.g*3.14159265359h)*lerp(_DiameterOuter, _DiameterInner, i.texcoord.r), 0.5h - 0.5h * cos(i.texcoord.g*3.14159265359h)*lerp(_DiameterOuter, _DiameterInner, i.texcoord.r));

		//START Heat and Flow
		//half2 uv1 = coods + _Time.x * _HeatSpeed;

		//half2 edgeFactor = 1.0h;

		half fadeStart = 1.0h - _HeatEdgeFade;
		//half fadeEnd = 1.0h;


		//edgeFactor.x = lerp(fadeStart, fadeEnd, coods.x) * lerp(fadeStart, fadeEnd, saturate(1.0h - coods.x));
		//edgeFactor.y = lerp(fadeStart, fadeEnd, coods.y) * lerp(fadeStart, fadeEnd, saturate(1.0h - coods.y));

		half2 normal = tex2D(_HeatDistortionMap, (coods + _Time.x * _HeatSpeed) / _HeatHazeSizes).rg * 2.0 - 1.0;

		half4 heatLocalStrength = tex2D(_HeatMap, coods);

		coods = coods + half4(normal * lerp(fadeStart, 1.0h, coods.y) * lerp(fadeStart, 1.0h, saturate(1.0h - coods.y)) * lerp(fadeStart, 1.0h, coods.x) * lerp(fadeStart, 1.0h, saturate(1.0h - coods.x))*4.0h, 0.0h, 0.0h) * _HeatStrength*heatLocalStrength.r;
		//END Heat and Flow

		c = UNITY_SAMPLE_TEX2D(_MainTex, coods);
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