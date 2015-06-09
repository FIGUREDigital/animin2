using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
public class EdResizeDialog : EditorWindow
{

	Vector2 newSize = new Vector2(2048,1500);
	bool shouldClose = false;

	void OnGUI()
	{
		foreach(NewScale ns in newScales)
		{
			ns.newSize = EditorGUILayout.Vector2Field("New Size", ns.newSize);
			if(ns.newSize != ns.scaler.referenceResolution)
			{				
				if (GUILayout.Button ("Change Resolution"))
                {
					ChangeResolution(ns.scaler, ns.newSize);
				}
			}
		}
		//		newSize = EditorGUILayout
		if (GUILayout.Button ("Cancel"))
		{
			this.Close ();
		}
	}

	class NewScale
	{
		public CanvasScaler scaler;
		public Vector2 newSize;
	}
	List<NewScale> newScales = new List<NewScale>();

	[MenuItem("CONTEXT/CanvasScaler/Resize Canvas")]
	public static void OpenHierarchyWindow()
	{
		Rect pos = EditorWindow.focusedWindow.position;
		EdResizeDialog popup = ScriptableObject.CreateInstance<EdResizeDialog>();
		popup.ShowAsDropDown(new Rect(pos.position.x, pos.position.y, pos.width, 1), new Vector2(pos.width, pos.height));

		popup.newScales.Clear ();
		Object[] objects = Selection.GetFiltered(typeof(CanvasScaler), SelectionMode.Editable);

		foreach(Object o in objects)
		{
			CanvasScaler cs = o as CanvasScaler;
			if(cs == null) continue;
			NewScale ns = new NewScale();
			ns.scaler = cs;
			ns.newSize = new Vector2(2048, 1536);
			popup.newScales.Add (ns);
		}
    }
    
	void ChangeResolution(CanvasScaler scaler, Vector2 newSize)
	{
		RectTransform rt = scaler.transform as RectTransform;
		//Vector2 scale = new Vector2(newSize.x / scaler.referenceResolution.x, newSize.y / scaler.referenceResolution.y);
		Vector2 scale = new Vector2(newSize.x / rt.sizeDelta.x , newSize.y / rt.sizeDelta.y);
		scale.x = scale.y;
		foreach(Transform t in scaler.transform)
		{
			Recurse(t, scale);
		}
		Undo.RecordObject(scaler,"Change resolution");
		scaler.referenceResolution = newSize;
	}
	    
	void Recurse(Transform t, Vector2 scale)
	{
		foreach(Transform ct in t)
		{
			Recurse(ct, scale);
        }
		RectTransform rt = t as RectTransform;
		if(rt != null)
		{
			Undo.RecordObject(rt,"Change resolution");
			rt.anchoredPosition = new Vector2(rt.anchoredPosition.x * scale.x, rt.anchoredPosition.y * scale.y);
			rt.sizeDelta = new Vector2(rt.sizeDelta.x * scale.x, rt.sizeDelta.y * scale.y); 
		}
		GridLayoutGroup gl = t.GetComponent<GridLayoutGroup>();
		if(gl != null)
		{			
			Undo.RecordObject(gl,"Change resolution");
			gl.padding = new RectOffset((int)(gl.padding.left * scale.x), (int)(gl.padding.top * scale.y), (int)(gl.padding.right * scale.x), (int)(gl.padding.bottom * scale.y));

			gl.cellSize = Vector2.Scale(gl.cellSize, scale);
			gl.spacing = Vector2.Scale(gl.spacing, scale);
		}
		Text text = t.GetComponent<Text>();
		if (text != null)
		{
			Undo.RecordObject(text,"Change resolution");
			text.fontSize = Mathf.RoundToInt(text.fontSize * scale.y);
			text.resizeTextMaxSize = Mathf.RoundToInt(text.resizeTextMaxSize * scale.y);
			text.resizeTextMinSize = Mathf.RoundToInt(text.resizeTextMinSize * scale.y);
		}

	}
}
