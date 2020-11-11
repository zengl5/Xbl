// Shader created with Shader Forge v1.37 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.37;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:2,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:3138,x:33857,y:32910,varname:node_3138,prsc:2|diff-1296-OUT,alpha-6451-OUT,voffset-1524-OUT;n:type:ShaderForge.SFN_Tex2d,id:1075,x:33394,y:32982,ptovrint:False,ptlb:node_1852_copy,ptin:_node_1852_copy,varname:_node_1852_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:b66bceaf0cc0ace4e9bdc92f14bba709,ntxv:0,isnm:False;n:type:ShaderForge.SFN_VertexColor,id:3918,x:33394,y:32824,varname:node_3918,prsc:2;n:type:ShaderForge.SFN_Multiply,id:1296,x:33659,y:32886,varname:node_1296,prsc:2|A-3918-RGB,B-1075-RGB;n:type:ShaderForge.SFN_Multiply,id:6451,x:33597,y:32999,varname:node_6451,prsc:2|A-3918-A,B-1075-A;n:type:ShaderForge.SFN_TexCoord,id:1319,x:32986,y:33302,varname:node_1319,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Add,id:953,x:33185,y:33199,varname:node_953,prsc:2|A-1319-U,B-1895-OUT;n:type:ShaderForge.SFN_Add,id:1790,x:33239,y:33404,varname:node_1790,prsc:2|A-1319-V,B-3748-OUT;n:type:ShaderForge.SFN_Multiply,id:1895,x:32943,y:33130,varname:node_1895,prsc:2|A-9260-OUT,B-5222-T;n:type:ShaderForge.SFN_Multiply,id:3748,x:32972,y:33496,varname:node_3748,prsc:2|A-5222-T,B-634-OUT;n:type:ShaderForge.SFN_Time,id:5222,x:32680,y:33348,varname:node_5222,prsc:2;n:type:ShaderForge.SFN_Vector1,id:634,x:32728,y:33599,varname:node_634,prsc:2,v1:1;n:type:ShaderForge.SFN_Vector1,id:9260,x:32720,y:33121,varname:node_9260,prsc:2,v1:1;n:type:ShaderForge.SFN_Append,id:4947,x:33421,y:33292,varname:node_4947,prsc:2|A-953-OUT,B-1790-OUT;n:type:ShaderForge.SFN_Vector4Property,id:2925,x:33438,y:33484,ptovrint:False,ptlb:node_2925,ptin:_node_2925,varname:node_2925,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0,v2:0,v3:0,v4:0;n:type:ShaderForge.SFN_Tex2d,id:2277,x:33597,y:33216,ptovrint:False,ptlb:node_2277,ptin:_node_2277,varname:node_2277,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-4947-OUT;n:type:ShaderForge.SFN_Multiply,id:1524,x:33701,y:33406,varname:node_1524,prsc:2|A-2277-RGB,B-2925-XYZ;proporder:1075-2925-2277;pass:END;sub:END;*/

Shader "xiaobl/qizi" {
    Properties {
        _node_1852_copy ("node_1852_copy", 2D) = "white" {}
        _node_2925 ("node_2925", Vector) = (0,0,0,0)
        _node_2277 ("node_2277", 2D) = "white" {}
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
            uniform float4 _LightColor0;
            uniform float4 _TimeEditor;
            uniform sampler2D _node_1852_copy; uniform float4 _node_1852_copy_ST;
            uniform float4 _node_2925;
            uniform sampler2D _node_2277; uniform float4 _node_2277_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float4 vertexColor : COLOR;
                UNITY_FOG_COORDS(3)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                float4 node_5222 = _Time + _TimeEditor;
                float2 node_4947 = float2((o.uv0.r+(1.0*node_5222.g)),(o.uv0.g+(node_5222.g*1.0)));
                float4 _node_2277_var = tex2Dlod(_node_2277,float4(TRANSFORM_TEX(node_4947, _node_2277),0.0,0));
                v.vertex.xyz += (_node_2277_var.rgb*_node_2925.rgb);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                i.normalDir = normalize(i.normalDir);
                i.normalDir *= faceSign;
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = 1;
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb; // Ambient Light
                float4 _node_1852_copy_var = tex2D(_node_1852_copy,TRANSFORM_TEX(i.uv0, _node_1852_copy));
                float3 diffuseColor = (i.vertexColor.rgb*_node_1852_copy_var.rgb);
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse;
                fixed4 finalRGBA = fixed4(finalColor,(i.vertexColor.a*_node_1852_copy_var.a));
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            Cull Off
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdadd
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal d3d11_9x xboxone ps4 psp2 n3ds wiiu 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform float4 _TimeEditor;
            uniform sampler2D _node_1852_copy; uniform float4 _node_1852_copy_ST;
            uniform float4 _node_2925;
            uniform sampler2D _node_2277; uniform float4 _node_2277_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float4 vertexColor : COLOR;
                LIGHTING_COORDS(3,4)
                UNITY_FOG_COORDS(5)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                float4 node_5222 = _Time + _TimeEditor;
                float2 node_4947 = float2((o.uv0.r+(1.0*node_5222.g)),(o.uv0.g+(node_5222.g*1.0)));
                float4 _node_2277_var = tex2Dlod(_node_2277,float4(TRANSFORM_TEX(node_4947, _node_2277),0.0,0));
                v.vertex.xyz += (_node_2277_var.rgb*_node_2925.rgb);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                i.normalDir = normalize(i.normalDir);
                i.normalDir *= faceSign;
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float4 _node_1852_copy_var = tex2D(_node_1852_copy,TRANSFORM_TEX(i.uv0, _node_1852_copy));
                float3 diffuseColor = (i.vertexColor.rgb*_node_1852_copy_var.rgb);
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse;
                fixed4 finalRGBA = fixed4(finalColor * (i.vertexColor.a*_node_1852_copy_var.a),0);
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
            uniform float4 _node_2925;
            uniform sampler2D _node_2277; uniform float4 _node_2277_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv0 : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                float4 node_5222 = _Time + _TimeEditor;
                float2 node_4947 = float2((o.uv0.r+(1.0*node_5222.g)),(o.uv0.g+(node_5222.g*1.0)));
                float4 _node_2277_var = tex2Dlod(_node_2277,float4(TRANSFORM_TEX(node_4947, _node_2277),0.0,0));
                v.vertex.xyz += (_node_2277_var.rgb*_node_2925.rgb);
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
