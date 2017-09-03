// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.26 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.26;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:3138,x:33252,y:32713,varname:node_3138,prsc:2|emission-3421-OUT,alpha-8614-OUT;n:type:ShaderForge.SFN_Color,id:7241,x:32617,y:32723,ptovrint:False,ptlb:Color,ptin:_Color,varname:_Color,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0,c2:0.5019608,c3:1,c4:1;n:type:ShaderForge.SFN_TexCoord,id:3390,x:32098,y:33058,varname:node_3390,prsc:2,uv:0;n:type:ShaderForge.SFN_RemapRange,id:2803,x:32265,y:33058,varname:node_2803,prsc:2,frmn:0,frmx:1,tomn:-1,tomx:1|IN-3390-UVOUT;n:type:ShaderForge.SFN_Length,id:9809,x:32440,y:33058,varname:node_9809,prsc:2|IN-2803-OUT;n:type:ShaderForge.SFN_Floor,id:2845,x:32617,y:33058,varname:node_2845,prsc:2|IN-9809-OUT;n:type:ShaderForge.SFN_OneMinus,id:6700,x:32789,y:33058,varname:node_6700,prsc:2|IN-2845-OUT;n:type:ShaderForge.SFN_OneMinus,id:3536,x:32617,y:32924,varname:node_3536,prsc:2|IN-9809-OUT;n:type:ShaderForge.SFN_Multiply,id:3421,x:33003,y:32813,varname:node_3421,prsc:2|A-7241-RGB,B-9867-OUT;n:type:ShaderForge.SFN_Sqrt,id:9867,x:32789,y:32924,varname:node_9867,prsc:2|IN-3536-OUT;n:type:ShaderForge.SFN_Multiply,id:8614,x:33003,y:32987,varname:node_8614,prsc:2|A-7241-A,B-6700-OUT;proporder:7241;pass:END;sub:END;*/

Shader "Shader Forge/UnlitTransparentBillboardSphereSpecular" {
    Properties {
        _Color ("Color", Color) = (0,0.5019608,1,1)
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _Color;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos(v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
////// Lighting:
////// Emissive:
                float node_9809 = length((i.uv0*2.0+-1.0));
                float3 emissive = (_Color.rgb*sqrt((1.0 - node_9809)));
                float3 finalColor = emissive;
                return fixed4(finalColor,(_Color.a*(1.0 - floor(node_9809))));
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
