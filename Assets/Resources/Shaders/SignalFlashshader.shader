﻿Shader "Custom/Flash Select" {
        Properties {
            _Color ("Main Color", Color) = (1,1,1,1)
            _MainTex ("Texture", 2D) = "white" { }
            _BlendFactor ("BlendFactor", float) = 0.0
        }
        SubShader 
        {
            Pass 
            {
		        CGPROGRAM

		        #pragma vertex vert
		        #pragma fragment frag


		        #include "UnityCG.cginc"

		        float4 _Color;
		        sampler2D _MainTex;
		        float _BlendFactor;

		        struct v2f {
		            float4 pos : SV_POSITION;
		            float2 uv : TEXCOORD0;
		        };

		        float4 _MainTex_ST;

		        v2f vert (appdata_base v)
		        {
		            v2f o;
		            o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
		            o.uv = TRANSFORM_TEX (v.texcoord, _MainTex);
		            return o;
		        }

		        float4 frag (v2f i) : COLOR
		        {
		            float4 texcol = tex2D (_MainTex, i.uv);
		            texcol.rgb = lerp(texcol.rgb, _Color.rgb, _BlendFactor);
		            return texcol;
		        }
		        ENDCG

            }
        }
        Fallback "VertexLit"
    }