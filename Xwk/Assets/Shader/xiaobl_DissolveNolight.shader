// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "xiaobl/Dissolve NotLight"
{
	Properties
	{
		_Alpha("Alpha",Range(0,1)) = 1
		_MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
		_Color ("Main Color", Color) = (1,1,1,1)
		_DissolveSrc ("DissolveSrc", 2D) = "white" {}
		_DissColor ("DissColor", Color) = (1,1,1,1)
		_Amount ("Amount", Range (0, 1)) = 0.5
		_StartAmount("StartAmount", float) = 0.1
		_Illuminate ("Illuminate", Range (0, 1)) = 0.5
		_ColorAnimate ("ColorAnimate", vector) = (1,1,1,1)
	}

	SubShader 
	{
		Tags {"Queue"="Transparent+105"  "IgnoreProjector"="True" "RenderType"="Transparent" }
		LOD 200
		Lighting  off 
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off
		Fog { Mode Off }

		pass
		{
			CGPROGRAM
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _DissolveSrc;
			float4 _DissolveSrc_ST;
			
			uniform float  _Alpha;
			uniform fixed4 _Color;
			uniform half4  _DissColor;
			uniform half   _Amount;
			static  half4  Color = half4(1,1,1,1);
			uniform half4  _ColorAnimate;
			uniform half   _Illuminate;
			uniform half   _StartAmount;
			
			#pragma vertex vert
			#pragma fragment frag

			struct a2v
			{
				float4 vertex:POSITION;
				float4 color:COLOR;
				float2 texcoord:TEXCOORD0;
				float2 texcoord1:TEXCOORD1;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				fixed4 color : COLOR;
				float4 uv:TEXCOORD0;
			};

			v2f vert (a2v v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos (v.vertex);
				o.color = v.color;
				o.uv.xy = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.uv.zw = TRANSFORM_TEX(v.texcoord1, _DissolveSrc);
				return o;
			}

			half4 frag( v2f i ) : COLOR0 
		    {
				fixed4 tex    = tex2D(_MainTex, i.uv.xy);
				half3 Albedo  = tex * _Color;
				float ClipTex = tex2D (_DissolveSrc, i.uv.zw).r ;
				float ClipAmount = ClipTex - _Amount;

				if (ClipAmount < 0)
				{
					 clip(-0.1);
					 return half4(1,1,1,0);
				}
				if (ClipAmount < _StartAmount)
				{
				    float fBack = ClipAmount/_StartAmount;
					Color= (_ColorAnimate * _DissColor + (1-_ColorAnimate)*fBack);
					//Color
					float f = Color.x+Color.y+Color.z;
					Albedo  *= f *  Color * f;
					Albedo  /= (1 - _Illuminate);
				}
				////////////////////////////////// 
				half4 outColor ;
				outColor.rgb = Albedo.rgb;
				outColor.a = tex.a * _Color.a * _Alpha;
				return outColor;
			}
			ENDCG
		}
	} 
	FallBack "Diffuse"
}
