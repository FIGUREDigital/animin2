// From http://unifycommunity.com/wiki/index.php?title=Expose_properties_in_inspector

//using UnityEditor;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

[AttributeUsage( AttributeTargets.Property )]
public class ExposePropertyAttribute : Attribute
{

	public string[] Labels
	{
		get { return labels; }
		set { labels = value; }
	}
	public float[] floatValues = null;
	public string[] floatNames = null;
	public float minValue = float.MinValue;
	public bool editAsInt = false;
	public bool enumAsMask = false;
	public bool readOnly = false;
	public string serializedPropertyName; /// If set then this item will be visualized by rendering the named property instead.
	public System.Type interfaceType = null;/// What type should the object be that has the interface defined by the property.
	public string customEditor;
	private string[] labels = null;
}


public class PropertyField
{
	public delegate System.Object CustomEditorDelegate(PropertyField.PropertyTypes pType, string label, System.Object newValue, Type type, PropertyField field, GUILayoutOption[] options);

	public enum PropertyTypes
	{
		Generic = -1,
		Integer,
		Boolean,
		Float,
		String,
		Color,
		ObjectReference,
		LayerMask,
		Enum,
		Vector2,
		Vector3,
		Vector4,
		Rect,
		Array,
		AnimationCurve,
		Bounds,
		Interface,
		List,
        Dictionary
	}
	public bool hasMultipleValues;
	public System.Object currentValue;
	public PropertyInfo m_Info;
	PropertyTypes m_Type;
	public string[] labels = null;
	public bool readOnly = false;
	public bool enumAsMask = false;
	public bool editAsInt = false;
	public bool foldedOut = false;
	public float minValue = float.MinValue;
	public Dictionary<float, string> namedFloats = null;
	public string serializedPropertyName = null;
	public System.Type interfaceType = null;
	MethodInfo m_Getter;
	MethodInfo m_Setter;

	public MethodInfo customEditor = null;
	public System.Object customEditorinstance = null;

	public PropertyTypes Type
	{
		get
		{
			return m_Type;
		}
	}

	public String Name
	{
		get
		{
			return m_Info.Name;
		}
	}

	public PropertyField(System.Object instance, PropertyInfo info, PropertyTypes type)
	{
		m_Info = info;
		m_Type = type;

		m_Getter = m_Info.GetGetMethod(true);
		m_Setter = m_Info.GetSetMethod(true);
//         if (m_Getter == null)
//         {
//             Debug.Log("No getter for " + info.Name);
//         }
	}

	public void CheckIsDifferent(System.Object o, bool isFirstObject)
	{
		if (isFirstObject)
		{
			currentValue = GetValue(o);
			hasMultipleValues = false;
		}
		else if (!hasMultipleValues)
		{
			if (GetValue(o) != null && currentValue != null)
			{
				if (!GetValue(o).Equals(currentValue))
				{
					// Different to flag as multiple values;
					hasMultipleValues = true;
				}
			}
			else if (GetValue(o) != null || currentValue != null)
			{
				hasMultipleValues = true;
			}
		}
	}

	public System.Object GetValue(System.Object o)
	{
		return m_Getter.Invoke(o, null);
	}

	public void SetValue(System.Object o, System.Object value)
	{
		m_Setter.Invoke(o, new System.Object[] { value });
	}

	public static bool GetPropertyType(Type type, out PropertyTypes propertyType)
	{
		propertyType = PropertyTypes.Generic;

		if (type == typeof(int))
		{
			propertyType = PropertyTypes.Integer;
			return true;
		}
		else if (type == typeof(bool))
		{
			propertyType = PropertyTypes.Boolean;
			return true;
		}
		else if (type == typeof(float))
		{
			propertyType = PropertyTypes.Float;
			return true;
		}
		else if (type == typeof(string))
		{
			propertyType = PropertyTypes.String;
			return true;
		}
		else if (type == typeof(Color))
		{
			propertyType = PropertyTypes.Color;
			return true;
		}
		else if (type.IsSubclassOf(typeof(UnityEngine.Object)))
		{
			propertyType = PropertyTypes.ObjectReference;
			return true;
		}
		else if (type == typeof(LayerMask))
		{
			propertyType = PropertyTypes.LayerMask;
			return true;
		}
		else if (type.IsEnum)
		{
			propertyType = PropertyTypes.Enum;
			return true;
		}
		else if (type == typeof(Vector2))
		{
			propertyType = PropertyTypes.Vector2;
			return true;
		}
		else if (type == typeof(Vector3))
		{
			propertyType = PropertyTypes.Vector3;
			return true;
		}
		else if (type == typeof(Vector4))
		{
			propertyType = PropertyTypes.Vector4;
			return true;
		}
		else if (type == typeof(Rect))
		{
			propertyType = PropertyTypes.Rect;
			return true;
		}
		else if (type.BaseType == typeof(Array))
		{
			propertyType = PropertyTypes.Array;
			return true;
		}
		else if (type == typeof(AnimationCurve))
		{
			propertyType = PropertyTypes.AnimationCurve;
			return true;
		}
		else if (type == typeof(Bounds))
		{
			propertyType = PropertyTypes.Bounds;
			return true;
		}
		else if (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(List<>)))
		{
			propertyType = PropertyTypes.List;
			return true;
        }
        else if (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(Dictionary<,>)))
        {
            propertyType = PropertyTypes.Dictionary;
            return true;
        }
		else if (type.IsInterface)
		{
			propertyType = PropertyTypes.Interface;
			return true;
		}
		return false;
	}

}
