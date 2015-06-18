using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;

public class EdUtilsInspector
{
	static public bool compact = false;
	static public bool editAsInt = false;
	static public float minValue = float.MinValue;
	static public Dictionary<float, string> namedFloats = null;

	private const float TwoFieldLabelSize = 47.0f;
	private const float LabelSize = 137.0f;
	/// <summary>
	/// first and second  will be altered so they are both >= 0 and <= size and first+second <= size
	/// </summary>
	/// <param name="first"></param>
	/// <param name="second"></param>
	/// <param name="size"></param>
	public static void RangeCheck(ref int first, ref int second, int size) 
	{
		if (first < 0)
		{
			first = 0;
		}
		if (first > size)
		{
			first = size;
		}
		if (first > size - second)
		{
			second = size - first;
		}
	}

	public static Vector2 Vector2Field(string label, string labelX, string labelY, Vector2 size, bool readOnly)
	{
		float x = size.x;
		float y = size.y;

		EditorGUIUtility.labelWidth = LabelSize - TwoFieldLabelSize;
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel(label);
		EditorGUILayout.BeginHorizontal();
		EditorGUIUtility.labelWidth = TwoFieldLabelSize;
		if (readOnly)
		{
			EditorGUILayout.LabelField(labelX, GUILayout.Width(TwoFieldLabelSize));
			if (EditorGUI.showMixedValue)
			{
				EditorGUILayout.LabelField("—", GUILayout.MinWidth(40.0f));
			}
			else
			{
				EditorGUILayout.LabelField(x.ToString(), GUILayout.MinWidth(40.0f));
			}
			EditorGUILayout.LabelField(labelY, GUILayout.Width(TwoFieldLabelSize));
			if (EditorGUI.showMixedValue)
			{
				EditorGUILayout.LabelField("—", GUILayout.MinWidth(40.0f));
			}
			else
			{
				EditorGUILayout.LabelField(y.ToString(), GUILayout.MinWidth(40.0f));
			}
		}
		else
		{
			x = FloatField(labelX, x, GUILayout.MinWidth(40.0f));
			y = FloatField(labelY, y, GUILayout.MinWidth(40.0f));
		}
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.EndHorizontal();
		LookLikeControls();

		return new Vector2(x, y);
	}

	public static Rect PaddingIntField(Rect rect, Vector2 size, string label, string labelLeft, string labelTop, string labelRight, string labelBottom)
	{
		int width = Mathf.RoundToInt(size.x);
		int height = Mathf.RoundToInt(size.y);
		int left = Mathf.RoundToInt(rect.xMin);
		int right = Mathf.RoundToInt(rect.width);
		int top = Mathf.RoundToInt(rect.yMin);
		int bottom = Mathf.RoundToInt(rect.height);


		EditorGUIUtility.labelWidth = (LabelSize - TwoFieldLabelSize);
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel(label);
		EditorGUILayout.BeginVertical();
		EditorGUILayout.BeginHorizontal();
		EditorGUIUtility.labelWidth = TwoFieldLabelSize;
		int newLeft = EditorGUILayout.IntField(labelLeft, left, GUILayout.MinWidth(40.0f));
		int newTop = EditorGUILayout.IntField(labelTop, top, GUILayout.MinWidth(40.0f));
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();
		int newRight = EditorGUILayout.IntField(labelRight, right, GUILayout.MinWidth(40.0f));
		int newBottom = EditorGUILayout.IntField(labelBottom, bottom, GUILayout.MinWidth(40.0f));
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.EndVertical();
		EditorGUILayout.EndHorizontal(); 
		LookLikeControls();

		if (newLeft != left)
		{
			RangeCheck(ref newLeft, ref newRight, width);
		} 
		else if (newRight != right)
		{
			RangeCheck(ref newRight, ref newLeft, width);
		}

		if (newTop != top)
		{
			RangeCheck(ref newTop, ref newBottom, height);
		}
		else
		{
			RangeCheck(ref newBottom, ref newTop, height);
		}
		return new Rect(newLeft, newTop, newRight, newBottom);
	}

	public static void LookLikeControls()
	{
		EditorGUIUtility.labelWidth = LabelSize;
	}

	public static Rect RectField(string label, string[] labels, Rect rect)//, string labelLeft, string labelTop, string labelRight, string labelBottom)
	{
		EditorGUIUtility.labelWidth = (LabelSize - TwoFieldLabelSize);
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel(label);
		EditorGUILayout.BeginVertical();
		EditorGUILayout.BeginHorizontal();
		EditorGUIUtility.labelWidth = (TwoFieldLabelSize);
		float newLeft, newTop, newRight, newBottom;

		newLeft = FloatField(labels != null && labels.Length > 0 ? labels[0] : "Left", rect.x, GUILayout.MinWidth(40.0f));
		newTop = FloatField(labels != null && labels.Length > 1 ? labels[1] : "Top", rect.y, GUILayout.MinWidth(40.0f));
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();
		newRight = FloatField(labels != null && labels.Length > 2 ? labels[2] : "Right", rect.width, GUILayout.MinWidth(40.0f));
		newBottom = FloatField(labels != null && labels.Length > 3 ? labels[3] : "Bot", rect.height, GUILayout.MinWidth(40.0f));

        EditorGUILayout.EndHorizontal();
		EditorGUILayout.EndVertical();
		EditorGUILayout.EndHorizontal();
		EditorGUIUtility.LookLikeControls();

		return new Rect(newLeft, newTop, newRight, newBottom);
	}


	public static int IntField(string label, int value, params GUILayoutOption[] options)
	{
		GUIStyle style = ModifyStyle(EditorStyles.numberField);
		return EditorGUILayout.IntField(label, (int)value, style, options);
	}

	public static bool Toggle(string label, bool value, params GUILayoutOption[] options)
	{
		GUIStyle style = ModifyStyle(EditorStyles.toggle);
		return EditorGUILayout.Toggle(label, value, style, options);
	}


	static public GUIStyle ModifyStyle(GUIStyle style)
	{
		GUIStyle modified = new GUIStyle(style);
		if (compact)
		{
			modified.margin.top = 0;
			modified.margin.bottom = 0;
		}
		return modified;
	}


	static public float FloatEnum(string label, float value, GUIStyle style, params GUILayoutOption[] options)
	{
		string[] floatNames = new string[1 + namedFloats.Count];
		floatNames[0] = "Number";
		namedFloats.Values.CopyTo(floatNames, 1);
		int i = 0;

		if (namedFloats.ContainsKey(value))
		{
			string name = namedFloats[value];
			for (i = 1; i < floatNames.Length; i++)
			{
				if (floatNames[i] == name)
				{
					break;
				}
			}
		}

		int newSelection;
		if (label != null && label.Length > 0)
		{
			newSelection = EditorGUILayout.Popup(label, i, floatNames, style, options);
		}
		else
		{
			newSelection = EditorGUILayout.Popup(i, floatNames, style, options);
		}
		if (newSelection == i)
		{
			return value;
		}
		if (newSelection == 0)
		{
			return 1.0f;
		}
		foreach (KeyValuePair<float, string> pair in namedFloats)
		{
			string selectedName = floatNames[newSelection];
			if (pair.Value == selectedName)
			{
				return pair.Key;
			}
		}
		return 1.0f;
	}

	static public float FloatField(string label, float value, params GUILayoutOption[] options)
	{
//		Debug.Log(r.ToString());
//		GUILayout.BeginArea(r);
		GUIStyle style = ModifyStyle(EditorStyles.numberField);
//		EditorGUILayout.PrefixLabel(label, style);
		GUILayout.BeginHorizontal();
		float newValue = value;
		if (namedFloats != null && namedFloats.ContainsKey(value))
		{
			//EditorGUILayout.(("", namedFloats[value], style);
			newValue = FloatEnum(label, value, style, GUILayout.MinWidth(50.0f));//ModifyStyle(EditorStyles.popup));//, GUILayout.MaxWidth(200.0f + TwoFieldLabelSize));

			//			GUILayout.EndHorizontal();
			//			return newValue;
		}
		else
		{
//			float minW,maxW;
//			style.CalcMinMaxWidth(new GUIContent("10"), out minW, out maxW);
//			Debug.Log(minW+","+maxW);
			if (editAsInt && value > int.MinValue && value < int.MaxValue)
			{
				int rounded = Mathf.RoundToInt(value);
				newValue = (float)EditorGUILayout.IntField(label, (int)rounded, style, GUILayout.MinWidth(50.0f));//, GUILayout.MaxWidth(200.0f - 17.0f));
			}
			else
			{
				newValue = EditorGUILayout.FloatField(label, (float)value, style, GUILayout.MinWidth(50.0f));//, GUILayout.MaxWidth(200.0f - 17.0f));
			}
		}
		float newValue2 = value;
		if (namedFloats != null)
		{

			GUILayoutOption[] opt = new GUILayoutOption[1];
			opt[0] = GUILayout.MaxWidth(10.0f);//		options.CopyTo(opt, 1);

			style = ModifyStyle(EditorStyles.popup);
			style.fixedWidth = 10;
			newValue2 = FloatEnum(null, value, style, opt);
		}

		GUILayout.EndHorizontal();
//		GUILayout.EndArea();
		if (newValue < minValue)
		{
			newValue = minValue;
		}
		if (newValue != value)
		{
			return newValue;
		}
		if (newValue2 < minValue)
		{
			newValue2 = minValue;
		}
		return newValue2;
	}

	public static void LabelField(string label, string label2, params GUILayoutOption[] options)
	{
		GUIStyle style = ModifyStyle(EditorStyles.label);
		LabelField(label, label2, style, options);
	}

	public static string TextField(string label, string value, params GUILayoutOption[] options)
	{
		GUIStyle style = ModifyStyle(EditorStyles.label);
		return EditorGUILayout.TextField(label, value, style, options);
	}

#if UNITY_3_5
	public static bool LookLikeInspector
	{
		get
		{
			Type type = typeof(UnityEditor.EditorGUIUtility);

			FieldInfo field = type.GetField("look", BindingFlags.NonPublic | BindingFlags.Static);

			return ((int)field.GetValue(null) == 1);
		}
	}
	
	public static Rect GetControlRect(bool hasLabel, float height, GUIStyle style, params GUILayoutOption[] options)
	{
		Type type;
		FieldInfo field;
		float minWidth = 0.0f;
		if (hasLabel)
		{
			type = typeof(UnityEditor.EditorGUILayout);
			field = type.GetField("kLabelFloatMinW", BindingFlags.NonPublic | BindingFlags.Static);
			minWidth = (float)field.GetValue(null);
		}
		else
		{
			type = typeof(UnityEditor.EditorGUI);
			field = type.GetField("kNumberW", BindingFlags.NonPublic | BindingFlags.Static);
			minWidth = (float)field.GetValue(null);
		}

		type = typeof(UnityEditor.EditorGUILayout);
		field = type.GetField("kLabelFloatMaxW", BindingFlags.NonPublic | BindingFlags.Static);
		float kLabelFloatMaxW = (float)field.GetValue(null);

		return GUILayoutUtility.GetRect(minWidth, kLabelFloatMaxW, height, height, style, options);

	}
	
#endif
	public static UnityEngine.Object ObjectField(string label, UnityEngine.Object obj, Type objType, bool allowSceneObjects, params GUILayoutOption[] options)
	{

#if UNITY_3_5		
		if (!LookLikeInspector && EditorGUIUtility.HasObjectThumbnail(objType))
		{
			GUIStyle objectField = ModifyStyle(EditorStyles.objectField);
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			EditorGUILayout.PrefixLabel(label, objectField);
			GUILayoutOption[] optionArray1 = new GUILayoutOption[] { GUILayout.Width(64f) };
			Rect rect = GUILayoutUtility.GetAspectRect(1f, objectField, optionArray1);
			UnityEngine.Object obj2 = EditorGUI.ObjectField(rect, obj, objType, allowSceneObjects);
			GUILayout.EndHorizontal();
			return obj2;
		}
		GUIStyle style = ModifyStyle(EditorStyles.numberField);
		Rect position = GetControlRect(true, 16f, style, options);
		return EditorGUI.ObjectField(position, label, obj, objType, allowSceneObjects);
#else
		return EditorGUILayout.ObjectField(label, obj, objType, allowSceneObjects, options);
#endif
	}

	// Same as EditorGUILayout.LabelField except our version uses the style correctly when calculating size
	// Unity 3.5 has a hardcoded textField style.
	public static void LabelField(string label, string label2, GUIStyle style, params GUILayoutOption[] options)
	{
#if UNITY_3_5
		LabelField(new GUIContent(label), new GUIContent(label2), style, options);
#else
		EditorGUILayout.LabelField(label, label2, style, options);
#endif
	}

	
#if UNITY_3_5
	public static void LabelField(GUIContent label, GUIContent label2, GUIStyle style, params GUILayoutOption[] options)
	{
		if (!style.wordWrap)
		{
			Rect position = GetControlRect(true, 16f, style, options); // this is the only line that's different to Unity's internal call which passes EditorStyles.textField instead of style.
			EditorGUI.LabelField(position, label, label2, style);
		}
		else
		{
			EditorGUILayout.BeginHorizontal(new GUILayoutOption[0]);
			EditorGUILayout.PrefixLabel(label, style);
			Rect rect2 = GUILayoutUtility.GetRect(label2, style, options);
			int indentLevel = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;
			EditorGUI.LabelField(rect2, label2, style);
			EditorGUI.indentLevel = indentLevel;
			EditorGUILayout.EndHorizontal();
		}
	}
#endif
}
