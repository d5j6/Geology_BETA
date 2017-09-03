// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

#include "HLSLSupport.cginc"
#include "UnityCG.cginc"
#include "Lighting.cginc"
#include "AutoLight.cginc"

#if _USEMAINTEX_ON
UNITY_DECLARE_TEX2D(_MainTex);
UNITY_DECLARE_TEX2D(_PerlinsNoise);
#endif

#if _USECOLOR_ON
    float4 _Color;
#endif

#if _USEEMISSIONTEX_ON
    UNITY_DECLARE_TEX2D(_EmissionTex);
#endif

#if _USEMAINTEX_ON || _USEEMISSIONTEX_ON
	float4 _MainTex_ST;
	float4 _PerlinsNoise_ST;
#endif

struct appdata_t
{
    float4 vertex   : POSITION;
    #if _USEMAINTEX_ON || _USEEMISSIONTEX_ON
        float2 texcoord : TEXCOORD0;
    #endif
    float3 normal : NORMAL;
};

struct v2f_surf
{

    float4 pos : SV_POSITION;
    #if _USEMAINTEX_ON || _USEEMISSIONTEX_ON
        float2 pack0 : TEXCOORD0;
    #endif
    #ifndef LIGHTMAP_OFF
        float2 lmap : TEXCOORD1;
    #else
        float3 vlight : TEXCOORD1;
    #endif
    LIGHTING_COORDS(2, 3)
    UNITY_FOG_COORDS(4)
    #if _NEAR_PLANE_FADE_ON
        float fade : TEXCOORD5;
    #endif
	float4 localPos : TEXCOORD6;
};

inline float3 LightingLambertVS(float3 normal, float3 lightDir)
{
    float diff = max(0, dot(normal, lightDir));
    return _LightColor0.rgb * diff;
}

float4 _BordersColor;
float _BordersThickness;

v2f_surf vert(appdata_t v)
{
    v2f_surf o;
    UNITY_INITIALIZE_OUTPUT(v2f_surf, o);

    o.pos = UnityObjectToClipPos(v.vertex);

    #if _USEMAINTEX_ON || _USEEMISSIONTEX_ON
        o.pack0.xy = TRANSFORM_TEX(v.texcoord, _MainTex);
    #endif

    #ifndef LIGHTMAP_OFF
        o.lmap.xy = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
    #else
        float3 worldN = UnityObjectToWorldNormal(v.normal);
        o.vlight = ShadeSH9(float4(worldN, 1.0));
        o.vlight += LightingLambertVS(worldN, _WorldSpaceLightPos0.xyz);
    #endif
    
    #if _NEAR_PLANE_FADE_ON
        o.fade = ComputeNearPlaneFadeLinear(v.vertex);
    #endif

    TRANSFER_VERTEX_TO_FRAGMENT(o);
    UNITY_TRANSFER_FOG(o, o.pos);

	o.localPos = v.vertex;
    return o;
}

float _PerlinsNoiseSpeed;
float _TotalAlpha;

float4 frag(v2f_surf IN) : SV_Target
{
    #if _USEMAINTEX_ON || _USEEMISSIONTEX_ON
        float2 uv_MainTex = IN.pack0.xy;
    #endif

    float4 surfaceColor;
    #if _USEMAINTEX_ON

	if ((abs(uv_MainTex.x - 0.5h) >= 0.5h - _BordersThickness) || (abs(uv_MainTex.y - 0.5h) >= 0.5h - _BordersThickness))
	{
		surfaceColor = _BordersColor;
		surfaceColor.a = 1;
	}
	else
	{
		//surfaceColor = UNITY_SAMPLE_TEX2D(_MainTex, uv_MainTex);
		surfaceColor = UNITY_SAMPLE_TEX2D(_MainTex, uv_MainTex);
		float4 perlinsNoise = UNITY_SAMPLE_TEX2D(_PerlinsNoise, uv_MainTex*_PerlinsNoise_ST.xy + _PerlinsNoise_ST.zw + float2(1, 1)*_Time[1]* _PerlinsNoiseSpeed);
		

		float2 s = surfaceColor + 0.2127 + surfaceColor.x*0.3713*surfaceColor.y;
		float2 r = 4.789*sin(489.123*s);
		float4 outputColor = frac(r.x*r.y*(1 + s.x) * _Time[1]);

		surfaceColor = outputColor + perlinsNoise;

		surfaceColor.a = _BordersColor.a;
	}

        //surfaceColor = UNITY_SAMPLE_TEX2D(_MainTex, uv_MainTex);
    #else
        surfaceColor = 1;
    #endif

    #if _USECOLOR_ON
        surfaceColor *= _Color;
    #endif

		surfaceColor.a *= _TotalAlpha;

    float atten = LIGHT_ATTENUATION(IN);
    float4 finalColor = 0;

    #ifdef LIGHTMAP_OFF
        finalColor.rgb = surfaceColor.rgb * IN.vlight * atten;
    #else
        float3 lm = DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap, IN.lmap.xy));
        #ifdef SHADOWS_SCREEN
            finalColor.rgb = surfaceColor.rgb * min(lm, atten * 2);
        #else
            finalColor.rgb = surfaceColor.rgb * lm;
        #endif
    #endif

    finalColor.a = surfaceColor.a;

    #ifdef _USEEMISSIONTEX_ON
        finalColor.rgb += UNITY_SAMPLE_TEX2D(_EmissionTex, uv_MainTex);
    #endif

    #if _NEAR_PLANE_FADE_ON
        finalColor.rgb *= IN.fade;
    #endif

    UNITY_APPLY_FOG(IN.fogCoord, finalColor);

    return finalColor;
}