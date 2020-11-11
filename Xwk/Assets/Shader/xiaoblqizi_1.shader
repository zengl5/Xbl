// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Shader created with Shader Forge v1.37 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.37;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:3138,x:33894,y:32919,varname:node_3138,prsc:2|emission-1416-OUT,alpha-1218-OUT,voffset-4756-OUT;n:type:ShaderForge.SFN_Tex2d,id:1075,x:33426,y:33011,ptovrint:False,ptlb:node_1852_copy,ptin:_node_1852_copy,varname:_node_1852_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:b66bceaf0cc0ace4e9bdc92f14bba709,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Sin,id:7489,x:33572,y:33278,varname:node_7489,prsc:2|IN-4176-OUT;n:type:ShaderForge.SFN_Time,id:6644,x:33233,y:33283,varname:node_6644,prsc:2;n:type:ShaderForge.SFN_Add,id:4176,x:33426,y:33197,varname:node_4176,prsc:2|A-1944-OUT,B-2724-OUT;n:type:ShaderForge.SFN_Vector3,id:6777,x:33513,y:33489,varname:node_6777,prsc:2,v1:1,v2:0.5,v3:0.5;n:type:ShaderForge.SFN_Multiply,id:4756,x:33700,y:33458,varname:node_4756,prsc:2|A-7489-OUT,B-6777-OUT,C-6857-OUT,D-598-U;n:type:ShaderForge.SFN_Multiply,id:2724,x:33329,y:33429,varname:node_2724,prsc:2|A-6644-T,B-7338-OUT;n:type:ShaderForge.SFN_Slider,id:7338,x:32989,y:33449,ptovrint:False,ptlb:pinlu,ptin:_pinlu,varname:node_7338,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:2.186373,max:10;n:type:ShaderForge.SFN_Slider,id:6857,x:33292,y:33598,ptovrint:False,ptlb:zhenfu,ptin:_zhenfu,varname:node_6857,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1.865579,max:5;n:type:ShaderForge.SFN_Slider,id:9903,x:32823,y:33066,ptovrint:False,ptlb:bochang,ptin:_bochang,varname:node_9903,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1.951753,max:5;n:type:ShaderForge.SFN_Multiply,id:1944,x:33233,y:33123,varname:node_1944,prsc:2|A-9903-OUT,B-5552-OUT;n:type:ShaderForge.SFN_TexCoord,id:598,x:33588,y:33627,varname:node_598,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_FragmentPosition,id:3346,x:32576,y:33165,varname:node_3346,prsc:2;n:type:ShaderForge.SFN_ObjectPosition,id:704,x:32576,y:33304,varname:node_704,prsc:2;n:type:ShaderForge.SFN_Subtract,id:6925,x:32769,y:33304,varname:node_6925,prsc:2|A-3346-Y,B-704-Y;n:type:ShaderForge.SFN_Subtract,id:5765,x:32769,y:33165,varname:node_5765,prsc:2|A-3346-X,B-704-X;n:type:ShaderForge.SFN_Add,id:5552,x:32955,y:33191,varname:node_5552,prsc:2|A-5765-OUT,B-6925-OUT;n:type:ShaderForge.SFN_Multiply,id:1218,x:33693,y:33130,varname:node_1218,prsc:2|A-4228-A,B-1075-A;n:type:ShaderForge.SFN_Multiply,id:1416,x:33678,y:32886,varname:node_1416,prsc:2|A-4228-RGB,B-1075-RGB;n:type:ShaderForge.SFN_VertexColor,id:4228,x:33483,y:32843,varname:node_4228,prsc:2;proporder:1075-7338-6857-9903;pass:END;sub:END;*/

Shader "xiaobl/qizi_1" {
    Properties {
        _node_1852_copy ("node_1852_copy", 2D) = "white" {}
        _pinlu ("pinlu", Range(0, 10)) = 2.186373
        _zhenfu ("zhenfu", Range(0, 5)) = 1.865579
        _bochang ("bochang", Range(0, 5)) = 1.951753
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
            Cull Off
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal d3d11_9x xboxone ps4 psp2 n3ds wiiu 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform sampler2D _node_1852_copy; uniform float4 _node_1852_copy_ST;
            uniform float _pinlu;
            uniform float _zhenfu;
            uniform float _bochang;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float4 vertexColor : COLOR;
                UNITY_FOG_COORDS(2)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                float4 objPos = mul ( unity_ObjectToWorld, float4(0,0,0,1) );
                float4 node_6644 = _Time + _TimeEditor;
                v.vertex.xyz += (sin(((_bochang*((mul(unity_ObjectToWorld, v.vertex).r-objPos.r)+(mul(unity_ObjectToWorld, v.vertex).g-objPos.g)))+(node_6644.g*_pinlu)))*float3(1,0.5,0.5)*_zhenfu*o.uv0.r);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                float4 objPos = mul ( unity_ObjectToWorld, float4(0,0,0,1) );
////// Lighting:
////// Emissive:
                float4 _node_1852_copy_var = tex2D(_node_1852_copy,TRANSFORM_TEX(i.uv0, _node_1852_copy));
                float3 emissive = (i.vertexColor.rgb*_node_1852_copy_var.rgb);
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,(i.vertexColor.a*_node_1852_copy_var.a));
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            Cull Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal d3d11_9x xboxone ps4 psp2 n3ds wiiu 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform float _pinlu;
            uniform float _zhenfu;
            uniform float _bochang;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv0 : TEXCOORD1;
                float4 posWorld : TEXCOORD2;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                float4 objPos = mul ( unity_ObjectToWorld, float4(0,0,0,1) );
                float4 node_6644 = _Time + _TimeEditor;
                v.vertex.xyz += (sin(((_bochang*((mul(unity_ObjectToWorld, v.vertex).r-objPos.r)+(mul(unity_ObjectToWorld, v.vertex).g-objPos.g)))+(node_6644.g*_pinlu)))*float3(1,0.5,0.5)*_zhenfu*o.uv0.r);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                float4 objPos = mul ( unity_ObjectToWorld, float4(0,0,0,1) );
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
