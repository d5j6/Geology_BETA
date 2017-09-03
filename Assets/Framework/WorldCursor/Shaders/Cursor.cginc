#if _USEMAINTEX_ON
    UNITY_DECLARE_TEX2D(_MainTex);
#endif

#if _USECOLOR_ON
    float4 _Color;
#endif

#if _USEBUMPMAP_ON
    UNITY_DECLARE_TEX2D(_BumpMap);
#endif

#if _USEEMISSIONTEX_ON
    UNITY_DECLARE_TEX2D(_EmissionTex);
#endif

struct Input
{
    // Will get compiled out if not touched
    float2 uv_MainTex;

    #if _NEAR_PLANE_FADE_ON
        float fade;
    #endif
};

void vert(inout appdata_full v, out Input o)
{
    UNITY_INITIALIZE_OUTPUT(Input, o);
    
    #if _NEAR_PLANE_FADE_ON
        o.fade = ComputeNearPlaneFadeLinear(v.vertex);
    #endif
}

float _TimePulsingInterval;
float _SectorWidth;
float _MinAlpha;
float _MaxAlpha;

void surf(Input IN, inout SurfaceOutput o)
{
    float4 c;

    #if _USEMAINTEX_ON
        c = UNITY_SAMPLE_TEX2D(_MainTex, IN.uv_MainTex);
    #else
        c = 1;
    #endif

    #if _USECOLOR_ON
        c *= _Color;
    #endif

    o.Albedo = c.rgb;
    
    #if _NEAR_PLANE_FADE_ON
        o.Albedo.rgb *= IN.fade;
    #endif
    
		o.Alpha = 0;
		if (c.a > 0)
		{
			//float timeFactor = _Time.g / _TimePulsingInterval - floor(_Time.g / _TimePulsingInterval);
			float dist = distance(float2(0.5, 0.5), float2(IN.uv_MainTex.r, IN.uv_MainTex.g))*2;
			//float alphaFactor = (distance / _SectorWidth) - floor((distance / _SectorWidth));
			float alphaFactor = cos(((dist - _Time.g/_TimePulsingInterval)/_SectorWidth)*3.14159265);
			//o.Alpha = ((timeFactor - )/0.5);
			o.Alpha = lerp(_MinAlpha, _MaxAlpha, alphaFactor)*(1 - dist);
		}

    #if _USEBUMPMAP_ON
        o.Normal = UnpackNormal(UNITY_SAMPLE_TEX2D(_BumpMap, IN.uv_MainTex));
    #endif

    #if _USEEMISSIONTEX_ON
        o.Emission = UNITY_SAMPLE_TEX2D(_EmissionTex, IN.uv_MainTex);
    #endif
}