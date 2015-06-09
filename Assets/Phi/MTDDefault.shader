Shader "Custom/MTD Coloured" {
	Properties {
		_MainTex ("Base (RGB), Alpha (A)", 2D) = "white" {}
	}
	SubShader 
	{
		LOD 100
		Tags 
		{
			"RenderType"="Transparent"
			"IgnoreProjector" = "True"
			"Queue" = "Transparent" // We want to render back to front after any geometry, this does nopt mean that we have to use transparency.
		}
		
		Pass
		{
			Cull Off
			Lighting Off
			ZWrite Off
			ColorMask RGB
			AlphaTest Greater .01
			Blend SrcAlpha OneMinusSrcAlpha
			ColorMaterial AmbientAndDiffuse
			SetTexture [_MainTex]
			{
				Combine Texture * Primary
			}
		}
	} 
}
