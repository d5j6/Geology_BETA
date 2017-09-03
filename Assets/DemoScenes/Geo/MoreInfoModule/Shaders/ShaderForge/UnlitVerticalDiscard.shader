// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.26 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.26;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:3138,x:32837,y:32698,varname:node_3138,prsc:2|emission-2654-RGB,alpha-946-OUT;n:type:ShaderForge.SFN_Slider,id:354,x:32131,y:33026,ptovrint:False,ptlb:VDiscard,ptin:_VDiscard,varname:_VDiscard,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:1;n:type:ShaderForge.SFN_TexCoord,id:9340,x:32288,y:32848,varname:node_9340,prsc:2,uv:0;n:type:ShaderForge.SFN_If,id:7373,x:32485,y:33015,varname:node_7373,prsc:2|A-9340-V,B-354-OUT,GT-8606-OUT,EQ-1774-OUT,LT-1774-OUT;n:type:ShaderForge.SFN_Vector1,id:8606,x:32288,y:33149,varname:node_8606,prsc:2,v1:0;n:type:ShaderForge.SFN_Vector1,id:1774,x:32288,y:33203,varname:node_1774,prsc:2,v1:1;n:type:ShaderForge.SFN_Tex2d,id:2654,x:32485,y:32797,ptovrint:False,ptlb:MainTexture,ptin:_MainTexture,varname:_MainTexture,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:1ad53c3b63a826a41803351de0c7776a,ntxv:3,isnm:False;n:type:ShaderForge.SFN_Multiply,id:946,x:32659,y:33015,varname:node_946,prsc:2|A-2654-A,B-7373-OUT;proporder:354-2654;pass:END;sub:END;*/

Shader "Shader Forge/UnlitVerticalDiscard" {
    Properties {
        _VDiscard ("VDiscard", Range(0, 1)) = 1
        _MainTexture ("MainTexture", 2D) = "bump" {}
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
            uniform float _VDiscard;
            uniform sampler2D _MainTexture; uniform float4 _MainTexture_ST;
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
                float4 _MainTexture_var = tex2D(_MainTexture,TRANSFORM_TEX(i.uv0, _MainTexture));
                float3 emissive = _MainTexture_var.rgb;
                float3 finalColor = emissive;
                float node_7373_if_leA = step(i.uv0.g,_VDiscard);
                float node_7373_if_leB = step(_VDiscard,i.uv0.g);
                float node_1774 = 1.0;
                return fixed4(finalColor,(_MainTexture_var.a*lerp((node_7373_if_leA*node_1774)+(node_7373_if_leB*0.0),node_1774,node_7373_if_leA*node_7373_if_leB)));
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
