// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

#include "UnityCG.cginc"

/*#if _USEMAINTEX_ON
    UNITY_DECLARE_TEX2D(_MainTex);
    float4 _MainTex_ST;
#endif*/
UNITY_DECLARE_TEX2D(_MainTex);
float4 _MainTex_ST;

#if _USECOLOR_ON
    float4 _Color;
#endif

struct appdata_t
{
    float4 vertex : POSITION;
    /*#if _USEMAINTEX_ON
        float2 texcoord : TEXCOORD0;
    #endif*/
		float2 texcoord : TEXCOORD0;
};

struct v2f
{
    float4 vertex : SV_POSITION;
    /*#if _USEMAINTEX_ON
        float2 texcoord : TEXCOORD0;
    #endif*/
		float2 texcoord : TEXCOORD0;
    UNITY_FOG_COORDS(1)
    #if _NEAR_PLANE_FADE_ON
        float fade : TEXCOORD2;
    #endif
};

v2f vert(appdata_t v)
{
    v2f o;
    o.vertex = UnityObjectToClipPos(v.vertex);

    /*#if _USEMAINTEX_ON
        o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
    #endif*/
		o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
    
    #if _NEAR_PLANE_FADE_ON
        o.fade = ComputeNearPlaneFadeLinear(v.vertex);
    #endif

    UNITY_TRANSFER_FOG(o, o.vertex);
    return o;
}

float4 _RimColor;
float _RimThickness;

float4 frag(v2f i) : SV_Target
{
    float4 c;

    #if _USEMAINTEX_ON
        c = UNITY_SAMPLE_TEX2D(_MainTex, i.texcoord);
    #else
        c = 1;
    #endif

    #if _USECOLOR_ON
		float dist = distance(i.texcoord, float2(0.5, 0.5));
		if (dist < 0.5 - _RimThickness)
		{
			c *= _Color;
		}
		else if(dist < 0.5)
		{
			c *= _RimColor;
			c.a = _Color.a;
		}
		else
		{
			c.a = 0;
		}
    #endif
        
    UNITY_APPLY_FOG(i.fogCoord, c);

    #if _NEAR_PLANE_FADE_ON
        c.rgb *= i.fade;
    #endif

    return c;
}