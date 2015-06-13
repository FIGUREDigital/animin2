using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[AddComponentMenu("UI/Effects/Gradient Pro")]
public class UIGradientPro : BaseVertexEffect
{
	public enum GradientDir
	{
		Vertical,
		Horizontal
		//Free
	}
	public GradientDir gradientDir = GradientDir.Vertical;
	public bool overwriteAllColor = false;
	public UnityEngine.Gradient grad = new UnityEngine.Gradient();
	//public Color vertex1 = Color.white;
	//public Color vertex2 = Color.black;
	private Graphic targetGraphic;
	
	protected override void Start ()
	{
		targetGraphic = GetComponent<Graphic> ();
	}
	
	public override void ModifyVertices (List<UIVertex> vertexList)
	{
		if(targetGraphic == null)
		{
			targetGraphic = GetComponent<Graphic> ();
		}
		if (!IsActive () || vertexList.Count == 0 || targetGraphic == null) {
			return;
		}
		int count = vertexList.Count;
		UIVertex uiVertex = vertexList [0];

		Rect r = graphic.GetPixelAdjustedRect();
		float bottomY = gradientDir == GradientDir.Vertical ? r.yMin : r.xMin;// vertexList [vertexList.Count - 1].position.y : vertexList [vertexList.Count - 1].position.x;
		float topY = gradientDir == GradientDir.Vertical ? vertexList [0].position.y : vertexList [0].position.x;
		
		float uiElementHeight = gradientDir == GradientDir.Vertical ? r.height : r.width;//Y - bottomY;
		
		for (int i = 0; i < count; i++) {
			uiVertex = vertexList [i];
			if (!overwriteAllColor && uiVertex.color != targetGraphic.color)
				continue;
			uiVertex.color *= grad.Evaluate(((gradientDir == GradientDir.Vertical ? uiVertex.position.y : uiVertex.position.x) - bottomY) / uiElementHeight);
			vertexList [i] = uiVertex;
		}

	}
	private bool CompareCarefully (Color col1, Color col2)
	{
		if (Mathf.Abs (col1.r - col2.r) < 0.003f && Mathf.Abs (col1.g - col2.g) < 0.003f && Mathf.Abs (col1.b - col2.b) < 0.003f && Mathf.Abs (col1.a - col2.a) < 0.003f)
			return true;
		return false;
	}
}

//enum color mode Additive, Multiply, Overwrite