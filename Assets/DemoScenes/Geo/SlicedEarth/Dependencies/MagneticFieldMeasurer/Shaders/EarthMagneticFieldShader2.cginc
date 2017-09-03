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
float _NumOfNoiseSamples;
float _coeff1;
float _coeff2;
float _distanceMode;

sampler2D _MKGlowTex;
half _MKGlowTexStrength;
fixed4 _MKGlowTexColor;

void vert(inout appdata_full v, out Input o)
{
	float4 directionVector = float4(0, 0, 0, 0) - float4(_XSolarWindDirection, 0, _ZSolarWindDirection, 0);
	directionVector = normalize(directionVector);

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
float4 _Color3;
/*float _ColorGradientLinesOffset;
float _NumOfMeridians;
float _ThicknessOfMeridian;
float _MeridianFadingCoeff;*/

sampler2D _ColorNoiseTex;
float _ColorNoiseYMoveSpeed;
float _ColorNoiseXMoveSpeed;
uniform float4 _ColorNoiseTex_ST;

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

		/*float2 NoiseColorsUVs = IN.uv_MainTex * _ColorNoiseTex_ST.xy + _ColorNoiseTex_ST.zw;
		NoiseColorsUVs.x += _Time[1] * _ColorNoiseXMoveSpeed;
		NoiseColorsUVs.y += _Time[1] * _ColorNoiseYMoveSpeed;*/
		half4 n = tex2D(_ColorNoiseTex, half2(IN.uv_MainTex * _ColorNoiseTex_ST.xy + _ColorNoiseTex_ST.zw) + half2(_Time[1] * _ColorNoiseXMoveSpeed, _Time[1] * _ColorNoiseYMoveSpeed));
		half numOfColors = 3.0f;
		/*half color1Part = clamp(distance(sin(n.r*3.14159265359f), 0)/(1/ numOfColors), 0, 1);
		half color2Part = clamp(distance(sin(n.r*3.14159265359f), sin(3.14159265359f / numOfColors)) / (1 / numOfColors), 0, 1);
		half color3Part = clamp(distance(sin(n.r*3.14159265359f), sin((3.14159265359f / numOfColors)*2)) / (1 / numOfColors), 0, 1);*/
		c.rgb = clamp(distance(sin(n.r*3.14159265359h), 0.0h) / (1.0h/ numOfColors), 0.0h, 1.0h) * _Color1 + clamp(distance(sin(n.r*3.14159265359h), sin(3.14159265359h / numOfColors)) / (1.0h / numOfColors), 0.0h, 1.0h) * _Color2 + clamp(distance(sin(n.r*3.14159265359h), sin((3.14159265359h / numOfColors) * 2.0h)) / (1.0h / numOfColors), 0.0h, 1.0h) * _Color3;

		//float gradientCoeff = cos((IN.uv_MainTex.y + _ColorGradientLinesOffset)*3.14159265359f);
		//c.rgb = _Color1*gradientCoeff + _Color2*(1 - gradientCoeff);

		fixed4 d = tex2D(_MKGlowTex, IN.uv_MainTex) * _MKGlowTexColor;
		c += (d * _MKGlowTexStrength);

    o.Albedo = c.rgb;
    
    #if _NEAR_PLANE_FADE_ON
        o.Albedo.rgb *= IN.fade;
    #endif
    
		/*if (_NumOfMeridians > 0)
		{
			float section = 1.0f / _NumOfMeridians;
			float xPos = IN.uv_MainTex.x / section - floor(IN.uv_MainTex.x / section);
			float dist = distance(0.5f, xPos);
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
		}*/

    o.Alpha = c.a;

    #if _USEBUMPMAP_ON
        o.Normal = UnpackNormal(UNITY_SAMPLE_TEX2D(_BumpMap, IN.uv_MainTex));
    #endif

    #if _USEEMISSIONTEX_ON
        o.Emission = UNITY_SAMPLE_TEX2D(_EmissionTex, IN.uv_MainTex);
    #endif
}