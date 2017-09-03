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

float _DotsSize;
float _DotsFrequency;
float4 _DotsColor;
float _DotsColorFading;
float _DotsFadingOuterRange;
float _DotsFadingInnerRange;

void surf(Input IN, inout SurfaceOutput o)
{
    float4 c;

    /*#if _USEMAINTEX_ON
        c = UNITY_SAMPLE_TEX2D(_MainTex, IN.uv_MainTex);
    #else
        c = 1;
    #endif

    #if _USECOLOR_ON
        c *= _Color;
    #endif*/

	float2 center = float2(0.5f, 0.5f);
	float dotsFadingCoeff = 1 - clamp((distance(center, IN.uv_MainTex) - _DotsFadingInnerRange) / (_DotsFadingOuterRange - _DotsFadingInnerRange), 0, 1);
	if (dotsFadingCoeff > 0.0f)
	{
		float part = 1.0f / _DotsFrequency;
		float2 normaled = float2(IN.uv_MainTex.x / part - floor(IN.uv_MainTex.x / part), IN.uv_MainTex.y / part - floor(IN.uv_MainTex.y / part));

		float dist = distance(normaled, center);
		if (dist <= _DotsSize)
		{
			float transparentCoeff = dist / _DotsSize;

			c = _DotsColor;
			c.a = clamp(1 - transparentCoeff*_DotsColorFading, 0, 1)*dotsFadingCoeff;
		}
		else
		{
			c = float4(0, 0, 0, 0);
		}
	}
	else
	{
		c = float4(0, 0, 0, 0);
	}

	

    o.Albedo = c.rgb;
    
    #if _NEAR_PLANE_FADE_ON
        o.Albedo.rgb *= IN.fade;
    #endif
    
    o.Alpha = c.a;

    #if _USEBUMPMAP_ON
        o.Normal = UnpackNormal(UNITY_SAMPLE_TEX2D(_BumpMap, IN.uv_MainTex));
    #endif

    #if _USEEMISSIONTEX_ON
        o.Emission = UNITY_SAMPLE_TEX2D(_EmissionTex, IN.uv_MainTex);
    #endif
}