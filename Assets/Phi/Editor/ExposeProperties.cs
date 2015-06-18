using UnityEditor;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
    
    
public class ExposeProperties
{

	private bool listeningForGuiChanges = false;
	private string undoName;
	private bool readOnly = false;

    public void OnInspectorGUIStart( SerializedObject serializedObject )
    {
		EdUtilsInspector.LookLikeControls();
		if (undoName != null)
		{
			Event e = Event.current;
			Undo.RecordObjects(serializedObject.targetObjects, undoName);
			if ((e.type == EventType.MouseDown && e.button == 0) || (e.type == EventType.KeyUp && e.keyCode == KeyCode.Tab) || (e.type == EventType.DragUpdated))
			{
				// When the LMB is pressed or the TAB key is released,
				// store a snapshot, but don't register it as an undo
                                                                    				// (so that if nothing changes we avoid storing a useless undo).
				listeningForGuiChanges = true;
			}
		}
	}

	public void OnInspectorGUIRender(SerializedObject serializedObject)
	{
		EdUtilsInspector.compact = false;
		serializedObject.Update();
		//		if (serializedObject.isEditingMultipleObjects)
		{
			// First we need to mark any properties that are different in each object so that they show -- until edited
			bool isFirstObject = true;
			foreach (UnityEngine.Object obj in serializedObject.targetObjects)
			{
		        foreach ( PropertyField field in propertyFields )
		        {
					field.CheckIsDifferent(obj, isFirstObject);
				}
				isFirstObject = false;
			}

			EditorGUI.showMixedValue = false;
		}
        
        GUILayoutOption[] emptyOptions = new GUILayoutOption[0];
        
        EditorGUILayout.BeginVertical( emptyOptions );
        
        foreach ( PropertyField field in propertyFields )
        {
			System.Object newValue = field.currentValue;
			EditorGUI.showMixedValue = field.hasMultipleValues;
			readOnly = field.readOnly;
			EdUtilsInspector.namedFloats = field.namedFloats;
			EdUtilsInspector.minValue = field.minValue;

			if (field.customEditor == null && field.serializedPropertyName != null && field.serializedPropertyName.Length > 0)
			{
				SerializedProperty prop = serializedObject.FindProperty(field.serializedPropertyName);
				if (prop != null)
				{
					EditorGUILayout.PropertyField(prop, new GUIContent(ObjectNames.NicifyVariableName(field.Name)), true, emptyOptions);
				}
			}
			else
			{
				bool prevChanged = GUI.changed;
				if (field.customEditor != null)
				{
					System.Object[] p = new System.Object[6];
					p[0] = field.Type;
					p[1] = ObjectNames.NicifyVariableName(field.Name);
					p[2] = newValue;
					p[3] = field.m_Info.PropertyType;
					p[4] = field;
					p[5] = emptyOptions;
 
					newValue = field.customEditor.Invoke(field.customEditorinstance, p);
				}
				else
				{
					newValue = DisplayField(field.Type, ObjectNames.NicifyVariableName(field.Name), newValue, field.m_Info.PropertyType, field, emptyOptions);
					//LayerMask mask;
					//newValue = Convert.ChangeType(result, field.m_Info.PropertyType);
				}
				if (GUI.changed && !prevChanged && !field.readOnly)
				{
//					Debug.Log("Changed "+field.Name);
// 				}
// 				if ((newValue == null && field.currentValue != null) ||
// 				   (newValue != null && field.currentValue == null) ||
// 				   (newValue != null && field.currentValue != null && !newValue.Equals(field.currentValue)))
// 				{
					// Value changed
					foreach (UnityEngine.Object obj in serializedObject.targetObjects)
					{
						field.SetValue(obj, newValue);
					}
				}
			}
        }
		EdUtilsInspector.namedFloats = null;
		EdUtilsInspector.minValue = float.MinValue;
		serializedObject.ApplyModifiedProperties();
        
        EditorGUILayout.EndVertical();
	}


	private System.Object DisplayField(PropertyField.PropertyTypes pType, string label, System.Object newValue, Type type, PropertyField field, GUILayoutOption[] options)
	{
		EdUtilsInspector.editAsInt = field.editAsInt;
        if (readOnly && pType != PropertyField.PropertyTypes.List && pType != PropertyField.PropertyTypes.Dictionary)
		{
			GUIStyle style = new GUIStyle(EditorStyles.label);
			if (EdUtilsInspector.compact)
			{
				style.margin.top = style.margin.bottom = 0;
			}
			string label2;
			if (newValue == null)
			{
				label2 = "None (" + type.ToString() + ")";
			}
			else
			{
				label2 = newValue.ToString();
			}
			EdUtilsInspector.LabelField(label, label2, style, options);
		}
		else
		{
			switch (pType)
			{
				case PropertyField.PropertyTypes.Integer:
					newValue = EdUtilsInspector.IntField(label, (int)newValue, options);
					break;

				case PropertyField.PropertyTypes.Boolean:
					newValue = EdUtilsInspector.Toggle(label, (bool)newValue, options);
					break;

				case PropertyField.PropertyTypes.Float:
					newValue = EdUtilsInspector.FloatField(label, (float)newValue, options);
					break;

				case PropertyField.PropertyTypes.String:
					newValue = EdUtilsInspector.TextField(label, (String)newValue, options);
					break;

				case PropertyField.PropertyTypes.Color:
					newValue = EditorGUILayout.ColorField(label, (Color)newValue, options);
					break;

				case PropertyField.PropertyTypes.ObjectReference:
					if (newValue == null)
					{
						newValue = EdUtilsInspector.ObjectField(label, null, type, true, options);
					}
					else
					{
						newValue = EdUtilsInspector.ObjectField(label, (UnityEngine.Object)newValue, type, true, options);
					}
					break;

				case PropertyField.PropertyTypes.LayerMask:
					{
						List<string> layers = new List<string>();
						for (int i = 0; i < 32; i++)
						{
							string layerName = LayerMask.LayerToName(i);
							if (layerName == null || layerName.Length == 0)
							{
								layerName = "(Layer "+i+")";
							}
							layers.Add (layerName);
						}
						newValue = (LayerMask)EditorGUILayout.MaskField(label, (LayerMask)newValue, layers.ToArray(), options);
						//newValue = EditorGUILayout.LayerField(label, (LayerMask)newValue, options);
						break;
					}
				case PropertyField.PropertyTypes.Enum:
					if (field.enumAsMask)
					{
						newValue = EditorGUILayout.EnumMaskField(label, (Enum)newValue, options);						
					}
					else
					{
						newValue = EditorGUILayout.EnumPopup(label, (Enum)newValue, options);
					}
					break;

				case PropertyField.PropertyTypes.Vector2:
					if (field.labels != null)
					{
						newValue = EdUtilsInspector.Vector2Field(label, field.labels[0], field.labels[1], (Vector2)newValue, field.readOnly);
					}
					else
					{
						newValue = EditorGUILayout.Vector2Field(label, (Vector2)newValue, options);
					}
					break;

				case PropertyField.PropertyTypes.Vector3:
					newValue = EditorGUILayout.Vector3Field(label, (Vector3)newValue, options);
					break;

				case PropertyField.PropertyTypes.Vector4:
					newValue = EditorGUILayout.Vector4Field(label, (Vector4)newValue, options);
					break;

				case PropertyField.PropertyTypes.Rect:
					newValue = EdUtilsInspector.RectField(label, field.labels, (Rect)newValue);
//					newValue = EditorGUILayout.RectField(label, (Rect)newValue, options);
					break;

				case PropertyField.PropertyTypes.Array:
					//newValue = EditorGUILayout.BoundsField(label, (Bounds)newValue, emptyOptions);
					break;

				case PropertyField.PropertyTypes.AnimationCurve:
					newValue = EditorGUILayout.CurveField(label, (AnimationCurve)newValue, options);
					break;

				case PropertyField.PropertyTypes.Bounds:
					newValue = EditorGUILayout.BoundsField(label, (Bounds)newValue, options);
					break;

				case PropertyField.PropertyTypes.Interface:
					{
						System.Object checkInterface;
						if ((UnityEngine.Object)newValue)
						{
							checkInterface = EdUtilsInspector.ObjectField(label, (UnityEngine.Object)newValue, field.interfaceType, true, options);
						}
						else
						{
							checkInterface = EdUtilsInspector.ObjectField(label, null, field.interfaceType, true, options);
						}
						if (checkInterface == null)
						{
							newValue = null;
						}
						else
						{
							if (field.interfaceType == typeof(Component))
							{
								Component comp = checkInterface as Component;
								checkInterface = comp.gameObject.GetComponent(type);
							}
							else if (!type.IsAssignableFrom(checkInterface.GetType()))
							{
								checkInterface = null;
							}
							if (checkInterface != null)
							{
								newValue = checkInterface;
							}
						}
					}
					break;

				case PropertyField.PropertyTypes.List:
					IList list = (IList)newValue;
					if (list.Count == 0)
					{
						label = label + " (Empty List)";
					}
					field.foldedOut = EditorGUILayout.Foldout(field.foldedOut, label);
					if (field.foldedOut)
					{

						bool prevCompact = EdUtilsInspector.compact;
						EdUtilsInspector.compact = true;
						EditorGUI.indentLevel += 1;
						int newSize = list.Count;
						if (readOnly)
						{
							EdUtilsInspector.LabelField("Size", newSize.ToString(), options);
						}
						else
						{
							newSize = EdUtilsInspector.IntField("Size", newSize, options);
						}
						newSize = Math.Max(0, newSize);
						Type[] types = field.m_Info.PropertyType.GetGenericArguments();
						UtilsDebug.Assert(types.Length == 1, "Bug, need to check this code it's assuming only one type, that of the object in the list");
						if (list.Count != newSize)
						{
							if (list.Count > newSize)
							{
								for (int i = list.Count - 1; i >= newSize; i--)
								{
									list.RemoveAt(i);
								}
							}
							else
							{
								for (int i = list.Count; i < newSize; i++)
								{
									if (types[0].IsValueType)
									{
										list.Add(Activator.CreateInstance(types[0]));
									}
									else
									{
										list.Add(null);
									}
								}
							}
						}
						// TODO: Not sure how to cause array elements to be grouped tgether closer vertically like standard inspector
						PropertyField.PropertyTypes elementType;
						if (PropertyField.GetPropertyType(types[0], out elementType))
						{
							if (newSize != 0)
							{
								for (int i = 0; i < newSize; i++)
								{
									System.Object entry = list[i];
									list[i] = DisplayField(elementType, "Element " + i, entry, types[0], field, options);
								}
							}
						}

						EditorGUI.indentLevel -= 1;
						EdUtilsInspector.compact = prevCompact;
					}
					break;

                case PropertyField.PropertyTypes.Dictionary:
                    IDictionary dict = (IDictionary)newValue;
                    if (dict.Count == 0)
                    {
                        label = label + " (Empty Dictionary)";
                    }
                    field.foldedOut = EditorGUILayout.Foldout(field.foldedOut, label);
                    if (field.foldedOut)
                    {
                        readOnly = true;
                        bool prevCompact = EdUtilsInspector.compact;
                        EdUtilsInspector.compact = true;
                        EditorGUI.indentLevel += 1;
                        int newSize = dict.Count;
                        EdUtilsInspector.LabelField("Size", newSize.ToString(), options);
                        Type[] types = field.m_Info.PropertyType.GetGenericArguments();
                        UtilsDebug.Assert(types.Length == 2, "Bug, need to check this code it's assuming only two type, that of the key and value");
 
                        // TODO: Not sure how to cause array elements to be grouped together closer vertically like standard inspector
                        PropertyField.PropertyTypes elementType;
                        if (PropertyField.GetPropertyType(types[1], out elementType))
                        {
                            if (newSize != 0)
                            {
                                foreach(var key in dict.Keys)
                                {
                                    System.Object entry = dict[key];
                                    dict[key] = DisplayField(elementType, key.ToString(), entry, types[1], field, options);
                                }
                            }
                        }

                        EditorGUI.indentLevel -= 1;
                        EdUtilsInspector.compact = prevCompact;
                    }
                    break;

				default:
					break;
			}
		}
		return newValue;
	}

	public enum ChangeReasons
	{
		notChanged,
		fieldEdited,
		undo
	};

	public ChangeReasons OnInspectorGUIFinish(SerializedObject serializedObject)
	{
		if (listeningForGuiChanges && GUI.changed)
		{
			// Some GUI value changed after pressing the mouse
			// or releasing the TAB key.
			// Register the previous snapshot as a valid undo.
			Undo.RecordObjects(serializedObject.targetObjects, undoName);

			foreach (UnityEngine.Object o in serializedObject.targetObjects)
			{
				EditorUtility.SetDirty(o);
			}
			listeningForGuiChanges = false;
		}
		//Debug.Log ("event "+Event.current.type+"  "+Event.current.commandName);
		if (Event.current.type == EventType.ExecuteCommand && Event.current.commandName == "UndoRedoPerformed")
		{
			return ChangeReasons.undo;
		}
		if (GUI.changed)
		{
			return ChangeReasons.fieldEdited;
		}
		return ChangeReasons.notChanged;
    }

	PropertyField[] propertyFields;


	public ExposeProperties(System.Object obj, string undoName)
	{
		InitExposeProperties(obj, undoName, null);
	}

	public ExposeProperties(System.Object obj, string undoName, System.Object customEditors)
	{
		InitExposeProperties(obj, undoName, customEditors);
	}

	public void InitExposeProperties(System.Object obj, string undoName, System.Object customEditors)
	{
		this.undoName = undoName;
    
 //   public static PropertyField[] GetProperties( System.Object obj )
 //   {
        
        List< PropertyField > fields = new List<PropertyField>();
        
        PropertyInfo[] infoArray = obj.GetType().GetProperties( BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
		List<PropertyInfo> infoList = new List<PropertyInfo>(infoArray);
		Type[] tInterfaces = obj.GetType().GetInterfaces();
		foreach (Type t in tInterfaces)
		{
			PropertyInfo[] interfaceInfos = t.GetProperties(BindingFlags.Public | BindingFlags.Instance| BindingFlags.NonPublic);
			infoList.AddRange(interfaceInfos);
		}

		foreach (PropertyInfo info in infoList)
        {
			if (!info.CanRead)
                continue;
            
            object[] attributes = info.GetCustomAttributes( true );
            
            ExposePropertyAttribute attribute = null;
            
            foreach( object o in attributes )
            {
                if ( o.GetType() == typeof( ExposePropertyAttribute ) )
                {
                    attribute = o as ExposePropertyAttribute;
                    break;
                }
            }
            
            if ( attribute == null )
                continue;

			PropertyField.PropertyTypes type = PropertyField.PropertyTypes.Integer;

			if (PropertyField.GetPropertyType(info.PropertyType, out type) || (attribute.customEditor != null && attribute.customEditor.Length > 0))
            {
                PropertyField field = new PropertyField( obj, info, type );
				if (attribute.Labels != null)
				{
					field.labels = attribute.Labels;
                }
				field.readOnly = attribute.readOnly || !info.CanWrite;
				field.editAsInt = attribute.editAsInt;
				field.enumAsMask = attribute.enumAsMask;
				field.interfaceType = attribute.interfaceType;
				if (attribute.floatNames != null && attribute.floatValues != null && attribute.floatNames.Length > 0)
				{
					field.namedFloats = new Dictionary<float, string>();
					for (int i = 0; i < Math.Min(attribute.floatNames.Length, attribute.floatValues.Length); i++)
					{
						field.namedFloats[attribute.floatValues[i]] = attribute.floatNames[i];
					}
				}
				field.serializedPropertyName = attribute.serializedPropertyName;
				field.minValue = attribute.minValue;
//				field.customEditor = attribute.customEditor;
				if (attribute.customEditor != null && attribute.customEditor.Length > 0)
				{
					System.Type[] types = {typeof(PropertyField.PropertyTypes),
										  typeof(string),
										  typeof(System.Object),
										  typeof(System.Type),
										  typeof(PropertyField),
										  typeof(GUILayoutOption[])};
					field.customEditor = customEditors.GetType().GetMethod(attribute.customEditor, types );
					if (field.customEditor != null)
					{
						field.customEditorinstance = customEditors;
					}
				}
                fields.Add( field );
            }
            
        }
        
        propertyFields = fields.ToArray();        
    }
    
}

