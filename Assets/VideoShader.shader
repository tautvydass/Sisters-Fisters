Shader "Unlit/VideoShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Transparency ("Transparency", Range(0, 1)) = 0.5
		_CutColor("Cut Color", Color) = (0.1, 0.1, 0.1, 1)
		_ColorTint("Color", Color) = (0, 0, 0, 0)
	}
	SubShader
	{
		Tags {"RenderType"="Transparent" "IgnoreProjector"="True"}
		Blend SrcAlpha OneMinusSrcAlpha
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _Transparency;
			float4 _CutColor;
			float4 _ColorTint;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				if(col.r < _CutColor.r && col.g < _CutColor.g && col.b < _CutColor.b)
					return float4(0, 0, 0, 0);
				return col - float4(0, 0, 0, _Transparency) + _ColorTint;
			}
			ENDCG
		}
	}
}
