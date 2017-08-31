// Very fast shader that uses the Unity lighting model.
// Compiles down to only performing the operations you're actually using.
// Uses material property drawers rather than a custom editor for ease of maintenance.

Shader "HoloWorld/Earth Magnetic Field Shader"
{
    Properties
    {
		_XSolarWindDirection("Solar Wind X Direction", Float) = 1
		_ZSolarWindDirection("Solar Wind Z Direction", Float) = 1
		_SolarWindSpeed("Solar Wind Speed", Range(0, 100)) = 1
		_DistortionsStrength("Strength of Distortions", Range(0, 1)) = 0.3
		_DistortionsSpeed("Speed of Distortions", Range(0.1, 10)) = 1
		_NoiseTex("Noise Texture", 3D) = "white" {}
		//_NumOfNoiseSamples("Num of Noise Samples", Float) = 4
			_coeff1("Coeff1", Range(0, 1)) = 0
			_coeff2("Coeff2", Range(0, 1)) = 1
			//_distanceMode("Distance from Origin Mode", Float) = 0
			_Color1("Color 1", Color) = (1,1,1,1)
			_Color2("Color 2", Color) = (1,1,1,1)
			_ColorGradientLinesOffset("Color Gradient Lines Offset", Range(-1, 1)) = 0
			_NumOfMeridians("Number of Meridians", Float) = 48
			_ThicknessOfMeridian("Thickness of Meridian", Range(0, 0.5)) = 0.19
			_MeridianFadingCoeff("Meridian Fading Coefficient", Range(0.001, 1)) = 0.001

        [Header(Main Color)]
        [Toggle] _UseColor("Enabled?", Float) = 0
        _Color("Main Color", Color) = (1,1,1,1)
        [Space(20)]

        [Header(Base(RGB))]
        [Toggle] _UseMainTex("Enabled?", Float) = 1
        _MainTex("Base (RGB)", 2D) = "white" {}
        [Space(20)]

        // Uses UV scale, etc from main texture
        [Header(Normalmap)]
        [Toggle] _UseBumpMap("Enabled?", Float) = 0
        [NoScaleOffset] _BumpMap("Normalmap", 2D) = "bump" {}
        [Space(20)]

        // Uses UV scale, etc from main texture
        [Header(Emission(RGB))]
        [Toggle] _UseEmissionTex("Enabled?", Float) = 0
        [NoScaleOffset] _EmissionTex("Emission (RGB)", 2D) = "white" {}
        [Space(20)]

        [Header(Blend State)]
        [Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend("SrcBlend", Float) = 1 //"One"
        [Enum(UnityEngine.Rendering.BlendMode)] _DstBlend("DestBlend", Float) = 0 //"Zero"
        [Space(20)]

        [Header(Other)]
        [Enum(UnityEngine.Rendering.CullMode)] _Cull("Cull", Float) = 2 //"Back"
        [Enum(UnityEngine.Rendering.CompareFunction)] _ZTest("ZTest", Float) = 4 //"LessEqual"
        [Enum(Off,0,On,1)] _ZWrite("ZWrite", Float) = 1.0 //"On"
        [Enum(UnityEngine.Rendering.ColorWriteMask)] _ColorWriteMask("ColorWriteMask", Float) = 15 //"All"
    }

    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" "PerformanceChecks" = "False" }
        Blend[_SrcBlend][_DstBlend]
        ZTest[_ZTest]
        ZWrite[_ZWrite]
        Cull[Off]
        ColorMask[_ColorWriteMask]
        LOD 300

        CGPROGRAM
        // We only target the HoloLens (and the Unity editor), so take advantage of shader model 5.
        #pragma target 5.0
        #pragma only_renderers d3d11

        #pragma surface surf Lambert vertex:vert alpha:fade

        #pragma shader_feature _USECOLOR_ON
        #pragma shader_feature _USEMAINTEX_ON
        #pragma shader_feature _USEBUMPMAP_ON
        #pragma shader_feature _USEEMISSIONTEX_ON
        #pragma shader_feature _NEAR_PLANE_FADE_ON

        #include "HoloToolkitCommon.cginc"
        #include "EarthMagneticFieldShader.cginc"

        ENDCG
    }
}