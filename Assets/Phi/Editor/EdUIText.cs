using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CanEditMultipleObjects , CustomEditor(typeof(UIText))]
public class EdUIText : Editor {

    SerializedProperty text;
    SerializedProperty localize;
    SerializedProperty color;
    SerializedProperty allCaps;

    SerializedProperty material;
    Editor textRendrerEditor;
    Editor testEd;
    bool needsLocalizing = false;
    static bool expandRendererInInspector = false;
//    GUIStyle wrapTextArea;
    public void OnEnable()
    {
/*        if (wrapTextArea == null)
        {
            wrapTextArea = new GUIStyle(EditorStyles.textField);
            wrapTextArea.wordWrap = true;
            wrapTextArea.stretchHeight = true;
        }*/
        text = serializedObject.FindProperty("text");
        localize = serializedObject.FindProperty("localizeID");
        color = serializedObject.FindProperty("color");
        allCaps = serializedObject.FindProperty("allCaps");
        material = serializedObject.FindProperty("material");
        List<MonoBehaviour> textRenderers = new List<MonoBehaviour>();
        foreach (UnityEngine.Object targ in targets)
        {
            UIText t = targ as UIText;
            if (t != null)
            {
                if (t.textRenderer != null)
                {
                    MonoBehaviour tr = t.textRenderer.TextRendererComponent;
                    if (tr != null)
                    {
                        textRenderers.Add(tr);
                        tr.hideFlags = tr.hideFlags | HideFlags.HideInInspector;
                    }
                }
            }
        }
        if (textRenderers.Count > 0)
        {
            MonoBehaviour[] textRendererArray = new MonoBehaviour[textRenderers.Count];
            textRenderers.CopyTo(textRendererArray);
            textRendrerEditor = CreateEditor(textRendererArray);
            CanvasRenderer r = textRenderers[0].GetComponent<CanvasRenderer>();
            if (r != null)
            {
                testEd = CreateEditor(r.GetMaterial());
            }
            else
            {
                testEd = null;
            }
        }
        UpdateNeedsLocalizing();
    }


    void OnDisable()
    {
        if(textRendrerEditor)
        {
            DestroyImmediate(textRendrerEditor);
            textRendrerEditor = null;
        }
    }

    private void UpdateNeedsLocalizing()
    {
        needsLocalizing = false;
        foreach (UnityEngine.Object targ in targets)
        {
            UIText t = targ as UIText;
            if (t != null)
            {
                if(t.LocalizeID != null && t.LocalizeID.Length > 0 && !t.IsLocalized)
                {
                    needsLocalizing = true;
                }
            }
        }
    }

	public override void OnInspectorGUI()
	{
        serializedObject.Update();
        EditorGUILayout.BeginHorizontal();
        Color prevCol = GUI.color;
        if(needsLocalizing)
        {
            GUI.color = new Color(1.0f, 0.0f, 0.0f);
        }
        EditorGUILayout.PropertyField(localize);
        bool localizeChanged = false;
        //if (needsLocalizing)  // Commented out as showing a new button takes the input away from text area
        {
            if (GUILayout.Button("Lookup", GUILayout.Width(54)))
            {
                localizeChanged = true;
            };
        }
        GUI.color = prevCol;
        EditorGUILayout.EndHorizontal();
        localizeChanged |= GUI.changed;
        GUI.changed = false;
        EditorGUILayout.PropertyField(text);
        EditorGUILayout.PropertyField(allCaps);
        bool textChanged = GUI.changed;
        GUI.changed = false;
        EditorGUILayout.PropertyField(color);
        bool colorChanged = GUI.changed;


        GUI.changed = false;
        EditorGUILayout.PropertyField(material);
        bool materialChanged = GUI.changed;
        //EditorGUILayout.LabelField("Text");
        //text.stringValue = EditorGUILayout.TextArea(text.stringValue, wrapTextArea, GUILayout.MinHeight(50), GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
        

        if (textRendrerEditor)
        {
            EditorGUILayout.Space();
            expandRendererInInspector = EditorGUILayout.Foldout(expandRendererInInspector, "Render Component");
            if (expandRendererInInspector)
            {
                EditorGUI.indentLevel++;
                textRendrerEditor.OnInspectorGUI();
                EditorGUI.indentLevel--;
            }
        }
        if (testEd)
        {

            EditorGUILayout.Space();
                testEd.OnInspectorGUI();
        }

        if (serializedObject.ApplyModifiedProperties() || GUI.changed || localizeChanged || textChanged || colorChanged || materialChanged)
        {
            foreach (UnityEngine.Object targ in targets)
            {
                UIText t = targ as UIText;
                if (t != null)
                {
                    if(colorChanged)
                    {
                        t.UpdateColor();
                    }
                    if (materialChanged)
                    {
                        t.UpdateMaterial();

                    }
                    else if (textChanged)
                    {
                        t.UpdateText();
                    }
                    else if (localizeChanged)
                    {
                        t.Localize();
                    }
                    else
                    {
                        t.CopySettingsFromRenderer();
                    }
                }
            }
            UpdateNeedsLocalizing();
        }
		//DrawDefaultInspector();
	}
}
