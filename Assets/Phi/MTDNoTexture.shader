Shader "Custom/MTD No Texture Coloued" {
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
		}
	} 
}
