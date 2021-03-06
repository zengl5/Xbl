// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "xiaobl/Scroll 2 Layers Sine AlphaBlended" 
{
	Properties 
	{
		_MainTex ("Base layer (RGB)", 2D) = "white" {}
		_DetailTex ("2nd layer (RGB)", 2D) = "white" {}
		_ScrollX ("Base layer Scroll speed X", Float) = 1.0
		_ScrollY ("Base layer Scroll speed Y", Float) = 0.0
		_Scroll2X ("2nd layer Scroll speed X", Float) = 1.0
		_Scroll2Y ("2nd layer Scroll speed Y", Float) = 0.0
		_Color("Color", Color) = (1,1,1,1)

		_MMultiplier ("Layer Multiplier", Float) = 2.0
	}

	SubShader 
	{
		Tags { "Queue"="Transparent+100" "IgnoreProjector"="True" "RenderType"="Transparent" }
	
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off Lighting Off ZWrite Off Fog { Color (0,0,0,0) }
	
		LOD 100
	
		CGINCLUDE
		#pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
		#pragma exclude_renderers molehill    
		#include "UnityCG.cginc"
		sampler2D _MainTex;
		sampler2D _DetailTex;

		fixed4 _MainTex_ST;
		fixed4 _DetailTex_ST;
	
		fixed _ScrollX;
		fixed _ScrollY;
		fixed _Scroll2X;
		fixed _Scroll2Y;
		fixed _MMultiplier;
		fixed4 _Color;

		struct v2f 
		{
			fixed4 pos : SV_POSITION;
			fixed4 uv : TEXCOORD0;
			fixed4 color : TEXCOORD1;
		};

		v2f vert (appdata_full v)
		{
			v2f o;
			o.pos = UnityObjectToClipPos(v.vertex);
			o.uv.xy = TRANSFORM_TEX(v.texcoord.xy,_MainTex) + frac(float2(_ScrollX, _ScrollY) * _Time);
			o.uv.zw = TRANSFORM_TEX(v.texcoord.xy,_DetailTex) + frac(float2(_Scroll2X, _Scroll2Y) * _Time);

			o.color = _MMultiplier * _Color * v.color;
			return o;
		}
		ENDCG

		Pass 
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
//			#pragma fragmentoption ARB_precision_hint_fastest		
			fixed4 frag (v2f i) : COLOR
			{
				fixed4 o;
				fixed4 tex = tex2D (_MainTex, i.uv.xy);
				fixed4 tex2 = tex2D (_DetailTex, i.uv.zw);
			
				o = tex * tex2 * i.color;
						
				return o;
			}
			ENDCG 
		}	
	}
}
