Shader "Custom/PlayerShader"       
{      
    Properties       
    {      
        _Color("Color", Color) = (1,1,1,1)        
        _MainTex("Albedo", 2D) = "white" {}      
        _AfterTex("_AfterTex", 2D) = "white" {}      
        _AfterColor ("After Color", Color) = (0, 0, 0, 0.419)
		_Alpha("Alpha",range(0,1)) = 1
		_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5		

		_MixTex("Mix Albedo (RGB)", 2D) = "white" {}
		_BlendTexAlpha("BlendTexAlpha", Range(0, 1)) = 0.5
		_BlendColor("BlendColor", Color) = (1, 1, 1, 1)
    }      
    

SubShader {
	Tags {"Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="TransparentCutout"}
	LOD 100

	Lighting Off

	Pass {  
		CGPROGRAM
			#pragma target 3.0
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"     
            sampler2D _MainTex;      
            sampler2D _AfterTex;      
            float4  _AfterColor;  
			fixed _Alpha;
			float4 _MainTex_ST;
			fixed _Cutoff;

			//dirty mix
			sampler2D _MixTex;
			float4 _MixTex_ST;

			fixed4 _BlendColor;
			half _BlendTexAlpha;

			struct appdata_t {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0; 
				float2 texcoord2 : TEXCOORD1;

                float3 normal : NORMAL;
			};

			struct v2f {
				float4 vertex : SV_POSITION;

				half2 texcoord : TEXCOORD0;
				float2 uvLight : TEXCOORD1;  

				half2 texcoord2 : TEXCOORD2;

				//UNITY_FOG_COORDS(2)
			};

			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.texcoord2 = TRANSFORM_TEX(v.texcoord2, _MixTex);
				//UNITY_TRANSFER_FOG(o,o.vertex);
        
                half2 nor;    
                nor.x = dot(UNITY_MATRIX_IT_MV[0].xyz,v.normal);    
                nor.y = dot(UNITY_MATRIX_IT_MV[1].xyz,v.normal);    
                o.uvLight = nor * 0.5 + 0.5;    
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float4 finalcol;
				float4 baseCol;
				float4 upCol;
				
				float4 col = tex2D(_MainTex, i.texcoord);
				clip(col.a - _Cutoff);
				//UNITY_APPLY_FOG(i.fogCoord, col);
    
                float4 lightCol = tex2D(_AfterTex, i.uvLight);

					baseCol = col + lightCol * 1.0 * _AfterColor;

				upCol = tex2D(_MixTex, i.texcoord2) * _BlendColor;

				if (upCol.a > 0)
				{
					upCol.a *= _BlendTexAlpha;
				}

				//PS 正常(不透明) 混合模式
				finalcol = upCol.a * upCol + (1 - upCol.a)* baseCol;

				return finalcol;
			}
		ENDCG
	}
}

} 