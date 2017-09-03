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

float _XSolarWindDirection;
float _ZSolarWindDirection;
float _SolarWindSpeed;
float _DistortionsStrength;
float _DistortionsSpeed;
sampler3D _NoiseTex;
//float _NumOfNoiseSamples;
float _coeff1;
float _coeff2;
float _distanceMode;

void vert(inout appdata_full v, out Input o)
{
	float4 directionVector = normalize(float4(0, 0, 0, 0) - float4(_XSolarWindDirection, 0, _ZSolarWindDirection, 0));
	//directionVector = normalize(directionVector);

	float distanceFromOrigin = distance(v.vertex.xz, float2(0, 0))/2;

	//float currentFrame = lerp(_coeff1, _coeff2, _Time.y / _DistortionsSpeed - floor(_Time.y / _DistortionsSpeed));
	float4 noiseComponent = tex3Dlod(_NoiseTex, float4(v.texcoord.xy, lerp(_coeff1, _coeff2, _Time.y / _DistortionsSpeed - floor(_Time.y / _DistortionsSpeed)), 0));
	/*float4 finalDistortion = 0;
	if (_distanceMode < 1)
	{
		finalDistortion = directionVector*distanceFromOrigin*noiseComponent.a*_DistortionsStrength + directionVector*distanceFromOrigin*_SolarWindSpeed;
	}
	else if (_distanceMode < 2)
	{
		finalDistortion = directionVector*distanceFromOrigin*noiseComponent.a*_DistortionsStrength + directionVector*distanceFromOrigin*distanceFromOrigin*_SolarWindSpeed;
	}
	else if (_distanceMode < 3)
	{
		finalDistortion = directionVector*distanceFromOrigin*noiseComponent.a*_DistortionsStrength + directionVector*(1 - cos(distanceFromOrigin))*_SolarWindSpeed;
	}*/
	float4 finalDistortion = directionVector*distanceFromOrigin*noiseComponent.a*_DistortionsStrength + directionVector*(1 - cos(distanceFromOrigin))*_SolarWindSpeed;
	//float4 finalDistortion = directionVector*distanceFromOrigin*noiseComponent.a*_DistortionsStrength + directionVector*(1 - cos(distanceFromOrigin))*_SolarWindSpeed;
	//float4 finalDistortion = directionVector*distanceFromOrigin*noiseComponent.a*_DistortionsStrength + directionVector*distanceFromOrigin*_SolarWindSpeed;
	v.vertex = v.vertex + finalDistortion;

    UNITY_INITIALIZE_OUTPUT(Input, o);
    
    #if _NEAR_PLANE_FADE_ON
        o.fade = ComputeNearPlaneFadeLinear(v.vertex);
    #endif
}

float4 _Color1;
float4 _Color2;
float _ColorGradientLinesOffset;
float _NumOfMeridians;
float _ThicknessOfMeridian;
float _MeridianFadingCoeff;

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

		float gradientCoeff = cos((IN.uv_MainTex.y + _ColorGradientLinesOffset)*3.14159265359f);
		c.rgb = _Color1*gradientCoeff + _Color2*(1 - gradientCoeff);

    o.Albedo = c.rgb;
    
    #if _NEAR_PLANE_FADE_ON
        o.Albedo.rgb *= IN.fade;
    #endif
    
		/*if (_NumOfMeridians > 0)
		{*/
			float section = 1.0f / _NumOfMeridians;
			//float xPos = IN.uv_MainTex.x / section - floor(IN.uv_MainTex.x / section);
			float dist = distance(0.5f, IN.uv_MainTex.x / section - floor(IN.uv_MainTex.x / section));
			float opaquePoint = _ThicknessOfMeridian*_MeridianFadingCoeff;
			if (dist <= opaquePoint)
			{
				c.a = c.a;
			}
			else
			{
				c.a = c.a*lerp(1, 0, (dist - opaquePoint) / (_ThicknessOfMeridian - opaquePoint));
			}

			//c.a = lerp(1, 0, ((dist*(1 - _MeridianFadingCoeff) - _MeridianFadingCoeff)*_ThicknessOfMeridian) / 0.5f);
		//}

    o.Alpha = c.a;

    #if _USEBUMPMAP_ON
        o.Normal = UnpackNormal(UNITY_SAMPLE_TEX2D(_BumpMap, IN.uv_MainTex));
    #endif

    #if _USEEMISSIONTEX_ON
        o.Emission = UNITY_SAMPLE_TEX2D(_EmissionTex, IN.uv_MainTex);
    #endif
}