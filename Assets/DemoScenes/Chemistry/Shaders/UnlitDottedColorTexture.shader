// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Shader created with Shader Forge v1.27 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.27;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:0,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:False,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:3138,x:33020,y:32878,varname:node_3138,prsc:2|emission-4353-OUT;n:type:ShaderForge.SFN_Color,id:7241,x:32284,y:32839,ptovrint:False,ptlb:Color,ptin:_Color,varname:_Color,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Tex2d,id:9017,x:32284,y:33071,ptovrint:False,ptlb:MainTexture,ptin:_MainTexture,varname:_MainTexture,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Set,id:4996,x:32461,y:32839,varname:Color,prsc:2|IN-7241-RGB;n:type:ShaderForge.SFN_Set,id:6105,x:32461,y:33071,varname:Texture,prsc:2|IN-9017-RGB;n:type:ShaderForge.SFN_Get,id:2301,x:32628,y:32915,varname:node_2301,prsc:2|IN-4996-OUT;n:type:ShaderForge.SFN_Get,id:7660,x:32628,y:32982,varname:node_7660,prsc:2|IN-6105-OUT;n:type:ShaderForge.SFN_Multiply,id:4353,x:32834,y:32961,varname:node_4353,prsc:2|A-2301-OUT,B-7660-OUT,C-5061-OUT;n:type:ShaderForge.SFN_NormalVector,id:9383,x:31640,y:32936,prsc:2,pt:False;n:type:ShaderForge.SFN_ViewVector,id:1224,x:31640,y:33094,varname:node_1224,prsc:2;n:type:ShaderForge.SFN_Dot,id:7018,x:31863,y:33011,varname:node_7018,prsc:2,dt:0|A-9383-OUT,B-1224-OUT;n:type:ShaderForge.SFN_Set,id:2508,x:32033,y:33011,varname:Specular,prsc:2|IN-7018-OUT;n:type:ShaderForge.SFN_Get,id:5061,x:32628,y:33046,varname:node_5061,prsc:2|IN-2508-OUT;proporder:7241-9017;pass:END;sub:END;*/

Shader "Gemeleon/UnlitDottedColorTexture" {
    Properties {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTexture ("MainTexture", 2D) = "white" {}
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _Color;
            uniform sampler2D _MainTexture; uniform float4 _MainTexture_ST;
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
                float3 Color = _Color.rgb;
                float4 _MainTexture_var = tex2D(_MainTexture,TRANSFORM_TEX(i.uv0, _MainTexture));
                float3 Texture = _MainTexture_var.rgb;
                float Specular = dot(i.normalDir,viewDirection);
                float3 emissive = (Color*Texture*Specular);
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
