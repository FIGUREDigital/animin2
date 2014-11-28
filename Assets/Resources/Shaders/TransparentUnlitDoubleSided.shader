Shader "Custom/TransparentSingleColorShader" {
   Properties {
        _MainTex ("Base (RGB) Self-Illumination (A)", 2D) = "white" {}
         _Color ("Color", Color) = (1.0, 1.0, 1.0, 1.0)
    }
    SubShader {
    
    Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}
    
     Blend SrcAlpha OneMinusSrcAlpha
    Cull Off
    
        Pass {

            // Multiply in texture
            SetTexture [_MainTex] {
            constantColor [_Color]
                 Combine texture * constant, texture * constant
            }
        }
    }
}
