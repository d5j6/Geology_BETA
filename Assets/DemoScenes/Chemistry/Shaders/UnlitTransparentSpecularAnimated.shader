// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Shader created with Shader Forge v1.26 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.26;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:False,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,rfrpo:False,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:3138,x:33763,y:32723,varname:node_3138,prsc:2|emission-7726-OUT,alpha-2712-OUT;n:type:ShaderForge.SFN_Color,id:7241,x:32177,y:32838,ptovrint:False,ptlb:Color,ptin:_Color,varname:_Color,prsc:0,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0,c2:0.5019608,c3:1,c4:0.5019608;n:type:ShaderForge.SFN_Set,id:4962,x:32374,y:32824,varname:BaseColor,prsc:0|IN-7241-RGB;n:type:ShaderForge.SFN_Set,id:3404,x:32374,y:32931,varname:BaseAlpha,prsc:0|IN-7241-A;n:type:ShaderForge.SFN_NormalVector,id:4326,x:32592,y:32768,prsc:2,pt:False;n:type:ShaderForge.SFN_ViewVector,id:1726,x:32592,y:32931,varname:node_1726,prsc:2;n:type:ShaderForge.SFN_Dot,id:4641,x:32810,y:32845,varname:node_4641,prsc:2,dt:0|A-4326-OUT,B-1726-OUT;n:type:ShaderForge.SFN_Set,id:207,x:32983,y:32845,varname:Dot,prsc:0|IN-4641-OUT;n:type:ShaderForge.SFN_Get,id:4829,x:33191,y:32793,varname:node_4829,prsc:0|IN-4962-OUT;n:type:ShaderForge.SFN_Get,id:5007,x:33191,y:32896,varname:node_5007,prsc:0|IN-207-OUT;n:type:ShaderForge.SFN_Get,id:2712,x:33395,y:33059,varname:node_2712,prsc:2|IN-3404-OUT;n:type:ShaderForge.SFN_Blend,id:2420,x:33416,y:32832,varname:node_2420,prsc:0,blmd:1,clmp:True|SRC-4829-OUT,DST-5007-OUT;n:type:ShaderForge.SFN_TexCoord,id:1620,x:32397,y:33183,varname:node_1620,prsc:2,uv:0;n:type:ShaderForge.SFN_Panner,id:625,x:32578,y:33183,varname:node_625,prsc:0,spu:1,spv:0|UVIN-1620-UVOUT;n:type:ShaderForge.SFN_Tex2d,id:4230,x:32925,y:33183,ptovrint:False,ptlb:Specular,ptin:_Specular,varname:_Specular,prsc:0,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:c9a68a60df6775f41b0488f9e49cf96b,ntxv:0,isnm:False|UVIN-2165-UVOUT;n:type:ShaderForge.SFN_Set,id:611,x:33090,y:33183,varname:Specular,prsc:0|IN-4230-RGB;n:type:ShaderForge.SFN_Get,id:9447,x:33395,y:32717,varname:node_9447,prsc:0|IN-611-OUT;n:type:ShaderForge.SFN_Blend,id:7726,x:33592,y:32771,varname:node_7726,prsc:2,blmd:7,clmp:True|SRC-9447-OUT,DST-2420-OUT;n:type:ShaderForge.SFN_Rotator,id:2165,x:32745,y:33183,varname:node_2165,prsc:2|UVIN-625-UVOUT,ANG-8674-OUT;n:type:ShaderForge.SFN_Vector1,id:8674,x:32578,y:33331,varname:node_8674,prsc:2,v1:1.5708;proporder:7241-4230;pass:END;sub:END;*/

Shader "Shader Forge/UnlitTransparentSpecularAnimated" {
    Properties {
        _Color ("Color", Color) = (0,0.5019608,1,0.5019608)
        _Specular ("Specular", 2D) = "white" {}
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
            uniform float4 _TimeEditor;
            uniform fixed4 _Color;
            uniform sampler2D _Specular; uniform float4 _Specular_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos(v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
////// Lighting:
////// Emissive:
                float node_2165_ang = 1.5708;
                float node_2165_spd = 1.0;
                float node_2165_cos = cos(node_2165_spd*node_2165_ang);
                float node_2165_sin = sin(node_2165_spd*node_2165_ang);
                float2 node_2165_piv = float2(0.5,0.5);
                float4 node_5538 = _Time + _TimeEditor;
                float2 node_2165 = (mul((i.uv0+node_5538.g*float2(1,0))-node_2165_piv,float2x2( node_2165_cos, -node_2165_sin, node_2165_sin, node_2165_cos))+node_2165_piv);
                fixed4 _Specular_var = tex2D(_Specular,TRANSFORM_TEX(node_2165, _Specular));
                fixed3 Specular = _Specular_var.rgb;
                fixed3 BaseColor = _Color.rgb;
                fixed Dot = dot(i.normalDir,viewDirection);
                float3 emissive = saturate((saturate((BaseColor*Dot))/(1.0-Specular)));
                float3 finalColor = emissive;
                fixed BaseAlpha = _Color.a;
                return fixed4(finalColor,BaseAlpha);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
