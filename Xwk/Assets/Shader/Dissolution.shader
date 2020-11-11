// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Dissolution" {
    Properties {
        _MainTex ("MainTex", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _ZiFaGuang ("ZiFaGuang", Float ) = 0.5
        _Mask ("Mask", 2D) = "white" {}
        [MaterialToggle] _FanZhuan_Mask ("FanZhuan_Mask", Float ) = 0
        _XiaoRong ("XiaoRong", Range(1, 0)) = 1
        _XiaoRong_Bian ("XiaoRong_Bian", Range(0, 0.2)) = 0
        _XR_color ("XR_color", Color) = (0.5,0.5,1,1)
        [MaterialToggle] _XR_Sceen ("XR_Sceen", Float ) = 0.5
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
            #pragma exclude_renderers xbox360 xboxone ps3 ps4 psp2 
            #pragma target 2.0
            uniform float4 _Color;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform sampler2D _Mask; uniform float4 _Mask_ST;
            uniform float _XiaoRong;
            uniform float _XiaoRong_Bian;
            uniform float4 _XR_color;
            uniform float _ZiFaGuang;
            uniform fixed _XR_Sceen;
            uniform fixed _FanZhuan_Mask;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos(v.vertex );
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float3 node_5941 = (_Color.rgb*_MainTex_var.rgb*_ZiFaGuang);
                float4 _Mask_var = tex2D(_Mask,TRANSFORM_TEX(i.uv0, _Mask));
                float _FanZhuan_Mask_var = lerp( _Mask_var.r, (1.0 - _Mask_var.r), _FanZhuan_Mask );
                float node_1041 = (_XiaoRong*1.3);
                float3 emissive = (lerp(lerp( _XR_color.rgb, saturate((1.0-(1.0-node_5941)*(1.0-_XR_color.rgb))), _XR_Sceen ),node_5941,step(_FanZhuan_Mask_var,(node_1041-_XiaoRong_Bian)))*i.vertexColor.rgb);
                float3 finalColor = emissive;
                return fixed4(finalColor,(step(_FanZhuan_Mask_var,node_1041)*i.vertexColor.a*_MainTex_var.a));
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
