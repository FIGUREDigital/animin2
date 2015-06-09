using UnityEngine;
using UnityEditor;
using System.Collections;

using System;
using System.Reflection;
[CanEditMultipleObjects , CustomEditor(typeof(TDRotaryStripes))]
public class EdRotaryStripes : Editor {

	public override void OnInspectorGUI()
	{
		if (target == null)
			return;

        bool changed = DrawDefaultInspector();
		
        serializedObject.Update();
//        EditorGUILayout.Space();
//        EditorGUILayout.PropertyField(this.sortingOrder, new GUIContent("Sorting Order"), new GUILayoutOption[0]);

 //       methodSortingLayerField.Invoke(null, new object[] { guicSortingLayer, this.sortingLayerID, EditorStyles.popup });
 //       EditorGUILayout.PropertyField(this.sortingOrder, new GUIContent("Sorting Order"), new GUILayoutOption[0]);


        changed |= serializedObject.ApplyModifiedProperties();

		// Consider adding callback delegate to expose properties code.

        if (changed)
		{
			foreach (object o in serializedObject.targetObjects)
			{
                TDRotaryStripes rs = o as TDRotaryStripes;
				if (rs)
				{
                    rs.UpdateMesh();
					EditorUtility.SetDirty(rs);
				}
			}
		}

	}

}

