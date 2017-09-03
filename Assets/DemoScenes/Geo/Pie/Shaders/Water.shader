// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.26 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.26;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:3138,x:35437,y:33273,varname:node_3138,prsc:2|emission-1570-OUT,alpha-625-OUT,refract-1888-OUT;n:type:ShaderForge.SFN_Color,id:7241,x:30883,y:33441,ptovrint:False,ptlb:Color,ptin:_Color,varname:_Color,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0,c2:0.5019608,c3:1,c4:0.2509804;n:type:ShaderForge.SFN_TexCoord,id:6598,x:31434,y:33362,varname:node_6598,prsc:2,uv:0;n:type:ShaderForge.SFN_RemapRange,id:6103,x:32780,y:33382,varname:node_6103,prsc:2,frmn:0,frmx:1,tomn:-0.06,tomx:0.06|IN-9991-OUT;n:type:ShaderForge.SFN_Tex2d,id:4097,x:32207,y:33382,ptovrint:False,ptlb:Noise,ptin:_Noise,varname:_Noise,prsc:2,glob:False,taghide:True,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:28c7aad1372ff114b90d330f8a2dd938,ntxv:0,isnm:False|UVIN-2465-UVOUT;n:type:ShaderForge.SFN_Panner,id:2465,x:32030,y:33382,varname:node_2465,prsc:2,spu:0,spv:-0.6|UVIN-2317-OUT,DIST-1024-OUT;n:type:ShaderForge.SFN_Multiply,id:6080,x:31662,y:33445,varname:node_6080,prsc:2|A-6598-V,B-5385-OUT;n:type:ShaderForge.SFN_Slider,id:5385,x:31277,y:33543,ptovrint:False,ptlb:VTile,ptin:_VTile,varname:_VTile,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:1,cur:6,max:24;n:type:ShaderForge.SFN_Append,id:2317,x:31825,y:33382,varname:node_2317,prsc:2|A-6598-U,B-6080-OUT;n:type:ShaderForge.SFN_Set,id:6727,x:31118,y:33543,varname:Alpha,prsc:2|IN-7241-A;n:type:ShaderForge.SFN_Set,id:4420,x:31118,y:33441,varname:Color,prsc:2|IN-7241-RGB;n:type:ShaderForge.SFN_Set,id:8521,x:32946,y:33382,varname:Refraction,prsc:2|IN-6103-OUT;n:type:ShaderForge.SFN_Get,id:2641,x:33764,y:33351,varname:node_2641,prsc:2|IN-4420-OUT;n:type:ShaderForge.SFN_Get,id:1888,x:35194,y:33583,varname:node_1888,prsc:2|IN-8521-OUT;n:type:ShaderForge.SFN_Append,id:9991,x:32591,y:33382,varname:node_9991,prsc:2|A-1737-OUT,B-1737-OUT;n:type:ShaderForge.SFN_ComponentMask,id:1737,x:32392,y:33382,varname:node_1737,prsc:2,cc1:1,cc2:-1,cc3:-1,cc4:-1|IN-4097-RGB;n:type:ShaderForge.SFN_Time,id:4119,x:31662,y:33592,varname:node_4119,prsc:2;n:type:ShaderForge.SFN_Multiply,id:1024,x:31825,y:33545,varname:node_1024,prsc:2|A-4119-T,B-7542-OUT;n:type:ShaderForge.SFN_Slider,id:7542,x:31505,y:33804,ptovrint:False,ptlb:WaveSpeed,ptin:_WaveSpeed,varname:_WaveSpeed,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.9766991,max:16;n:type:ShaderForge.SFN_Set,id:8850,x:32570,y:33537,varname:Specular,prsc:2|IN-1737-OUT;n:type:ShaderForge.SFN_Get,id:589,x:33140,y:33302,varname:node_589,prsc:2|IN-8850-OUT;n:type:ShaderForge.SFN_Add,id:9579,x:33969,y:33373,varname:node_9579,prsc:2|A-2641-OUT,B-6218-OUT;n:type:ShaderForge.SFN_Slider,id:9400,x:33004,y:33609,ptovrint:False,ptlb:Specular,ptin:_Specular,varname:_Specular,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.2,max:2;n:type:ShaderForge.SFN_Set,id:4062,x:33534,y:33411,varname:ResultSpecular,prsc:2|IN-3835-OUT;n:type:ShaderForge.SFN_Get,id:6218,x:33764,y:33428,varname:node_6218,prsc:2|IN-4062-OUT;n:type:ShaderForge.SFN_Set,id:4156,x:34151,y:33373,varname:ResultEmission,prsc:2|IN-9579-OUT;n:type:ShaderForge.SFN_DepthBlend,id:7762,x:33982,y:33586,varname:node_7762,prsc:2|DIST-3978-OUT;n:type:ShaderForge.SFN_Slider,id:3978,x:33662,y:33586,ptovrint:False,ptlb:DepthBlendValue,ptin:_DepthBlendValue,varname:_DepthBlendValue,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:8;n:type:ShaderForge.SFN_Set,id:9389,x:34314,y:33586,varname:DepthBlend,prsc:2|IN-5536-OUT;n:type:ShaderForge.SFN_Vector1,id:516,x:34580,y:33513,varname:node_516,prsc:2,v1:0;n:type:ShaderForge.SFN_Lerp,id:4370,x:34800,y:33493,varname:node_4370,prsc:2|A-6956-OUT,B-516-OUT,T-512-OUT;n:type:ShaderForge.SFN_Get,id:512,x:34559,y:33588,varname:node_512,prsc:2|IN-9389-OUT;n:type:ShaderForge.SFN_Set,id:2856,x:34971,y:33493,varname:ResultAlpha,prsc:2|IN-4370-OUT;n:type:ShaderForge.SFN_OneMinus,id:5536,x:34151,y:33586,varname:node_5536,prsc:2|IN-7762-OUT;n:type:ShaderForge.SFN_Get,id:6338,x:34371,y:33328,varname:node_6338,prsc:2|IN-6727-OUT;n:type:ShaderForge.SFN_Get,id:6993,x:34371,y:33415,varname:node_6993,prsc:2|IN-4062-OUT;n:type:ShaderForge.SFN_Add,id:6956,x:34580,y:33361,varname:node_6956,prsc:2|A-6338-OUT,B-6993-OUT;n:type:ShaderForge.SFN_Get,id:1570,x:35194,y:33372,varname:node_1570,prsc:2|IN-4156-OUT;n:type:ShaderForge.SFN_RemapRangeAdvanced,id:3835,x:33368,y:33411,varname:node_3835,prsc:2|IN-589-OUT,IMIN-9983-OUT,IMAX-7974-OUT,OMIN-9983-OUT,OMAX-9400-OUT;n:type:ShaderForge.SFN_Vector1,id:9983,x:33161,y:33411,varname:node_9983,prsc:2,v1:0;n:type:ShaderForge.SFN_Vector1,id:7974,x:33161,y:33514,varname:node_7974,prsc:2,v1:1;n:type:ShaderForge.SFN_Get,id:625,x:35194,y:33517,varname:node_625,prsc:2|IN-2856-OUT;proporder:7241-9400-5385-7542-3978-4097;pass:END;sub:END;*/

Shader "Shader Forge/WaterOptimazed" {
    Properties {
        _Color ("Color", Color) = (0,0.5019608,1,0.2509804)
        _Specular ("Specular", Range(0, 2)) = 0.2
        _VTile ("VTile", Range(1, 24)) = 6
        _WaveSpeed ("WaveSpeed", Range(0, 16)) = 0.9766991
        _DepthBlendValue ("DepthBlendValue", Range(0, 8)) = 1
        [HideInInspector]_Noise ("Noise", 2D) = "white" {}
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        GrabPass{ }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite On
			Cull off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform sampler2D _GrabTexture;
            uniform sampler2D _CameraDepthTexture;
            uniform float4 _TimeEditor;
            uniform float4 _Color;
            uniform sampler2D _Noise; uniform float4 _Noise_ST;
            uniform float _VTile;
            uniform float _WaveSpeed;
            uniform float _Specular;
            uniform float _DepthBlendValue;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 screenPos : TEXCOORD1;
                float4 projPos : TEXCOORD2;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos(v.vertex );
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                o.screenPos = o.pos;
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                #if UNITY_UV_STARTS_AT_TOP
                    float grabSign = -_ProjectionParams.x;
                #else
                    float grabSign = _ProjectionParams.x;
                #endif
                i.screenPos = float4( i.screenPos.xy / i.screenPos.w, 0, 0 );
                i.screenPos.y *= _ProjectionParams.x;
                float sceneZ = max(0,LinearEyeDepth (UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)))) - _ProjectionParams.g);
                float partZ = max(0,i.projPos.z - _ProjectionParams.g);
                float4 node_4119 = _Time + _TimeEditor;
                float2 node_2465 = (float2(i.uv0.r,(i.uv0.g*_VTile))+(node_4119.g*_WaveSpeed)*float2(0,-0.6));
                float4 _Noise_var = tex2D(_Noise,TRANSFORM_TEX(node_2465, _Noise));
                float node_1737 = _Noise_var.rgb.g;
                float2 Refraction = (float2(node_1737,node_1737)*0.12+-0.06);
                float2 sceneUVs = float2(1,grabSign)*i.screenPos.xy*0.5+0.5 + Refraction;
                float4 sceneColor = tex2D(_GrabTexture, sceneUVs);
////// Lighting:
////// Emissive:
                float3 Color = _Color.rgb;
                float Specular = node_1737;
                float node_589 = Specular;
                float node_9983 = 0.0;
                float ResultSpecular = (node_9983 + ( (node_589 - node_9983) * (_Specular - node_9983) ) / (1.0 - node_9983));
                float3 ResultEmission = (Color+ResultSpecular);
                float3 emissive = ResultEmission;
                float3 finalColor = emissive;
                float Alpha = _Color.a;
                float DepthBlend = (1.0 - saturate((sceneZ-partZ)/_DepthBlendValue));
                float ResultAlpha = lerp((Alpha+ResultSpecular),0.0,DepthBlend);
                return fixed4(lerp(sceneColor.rgb, finalColor,ResultAlpha),1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
