// Very fast unlit shader.
// No lighting, lightmap support, etc.
// Compiles down to only performing the operations you're actually using.
// Uses material property drawers rather than a custom editor for ease of maintenance.

Shader "HoloWorld/Earth Shaders/Unlit Right Slice"
{
    Properties
    {
		//Base
		_Closed("How much it closed", Range(0, 1)) = 1
		_DiameterOuter("Diameter of Outer Sphere", Float) = 1
		_DiameterInner("Diameter of Inner Sphere", Float) = 0.95
		[Space(35)]

		//Heat and Flowing
		_HeatMap("Heat Map", 2D) = "white" {}
		_HeatDistortionMap("Heat Disctortion Map (Normal)", 2D) = "white" {}
		_HeatStrength("Distort strength", Float) = 1.0
		_HeatSpeed("Distort speed", Float) = 3.0
		_HeatEdgeFade("Edge fade (0-1)", Range(0, 1)) = 1.0
		_HeatHazeSizes("The size of Heat Hazes", Range(0, 1)) = 0.1
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
            #include "RightSliceUnlit.cginc"

            ENDCG
        }
    }
}