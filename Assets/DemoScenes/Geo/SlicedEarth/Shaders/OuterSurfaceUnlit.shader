// Very fast unlit shader.
// No lighting, lightmap support, etc.
// Compiles down to only performing the operations you're actually using.
// Uses material property drawers rather than a custom editor for ease of maintenance.

Shader "HoloWorld/Earth Shaders/Unlit Outer Surface"
{
    Properties
    {
		//Base
		_Closed("How much it closed", Range(0, 1)) = 1
		_Diameter("Diameter of Sphere", Float) = 1
		_RotationAngle("Rotation Angle", Float) = 0
		[Space(35)]

		//Second Tex
		[Toggle] _UseSecondTex("Need to use second texture?", Float) = 0
			_SecondTextureColorMultiplier("Second Texture Color Multiplier", Float) = 0.15
			//_SecondTextureColorAddition("Second Texture Color Addition", Float) = 0.2
		_terminatorLineWidth("TerminatorLineWidth", Range(0,1)) = 0.2
		_SecondTexture("SecondTexture", 2D) = "white" {}
		_litDirection("Light Direction", Vector) = (0.5,-0.7,-0.35)

		//Alternative Tex
		_AlternativeSurfaceTexture("Alternative Surface Texture", 2D) = "white" {}
		_AlternativeTextureVisibilityCoeff("Alternative Texture Visibility Coefficient", Range(0, 1)) = 0
		[Space(35)]

		//Hitech
		_GridBrightness("Brightness of the Grid", Range(0, 10)) = 0
		_GridVerticalSpreading("Vertical Spreading of the Grid", Range(0, 1)) = 1
		_GridColor("Color of the Grid", Color) = (0, 1, 0, 1)
		_GridHorizontalSpreading("Horizontal Spreading of the Grid", Range(0, 1)) = 1
		_NumOfEquatorialLines("Number of Equatorial Lines", Int) = 16
		_NumOfMeridianialLines("Number of Meridianial Lines", Int) = 32
		_GridLineThickness("Thickness of Lines", Range(0, 0.1)) = 0.01
		_XCoordOfTheCenter("X Coordinate of the Center", Range(0, 1)) = 0
		[Space(35)]

        [Header(Main Color)]
        [Toggle] _UseColor("Enabled?", Float) = 0
        _Color("Main Color", Color) = (1,1,1,1)
        [Space(20)]

        [Header(Base(RGB))]
        [Toggle] _UseMainTex("Enabled?", Float) = 1
        _MainTex("Base (RGB)", 2D) = "white" {}
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
        Tags { "RenderType" = "Opaque" }
        LOD 100
        Blend[_SrcBlend][_DstBlend]
        ZTest[_ZTest]
        ZWrite[_ZWrite]
        Cull[_Cull]
        ColorMask[_ColorWriteMask]

        Pass
        {
            Name "FORWARD"
            Tags { "LightMode" = "Always" }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog

            // We only target the HoloLens (and the Unity editor), so take advantage of shader model 5.
            #pragma target 5.0
            #pragma only_renderers d3d11

            #pragma shader_feature _USECOLOR_ON
            #pragma shader_feature _USEMAINTEX_ON
            #pragma shader_feature _NEAR_PLANE_FADE_ON

            #include "HoloToolkitCommon.cginc"
            #include "OuterSurfaceUnlit.cginc"

            ENDCG
        }
    }
}