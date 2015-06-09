using UnityEngine;
using UnityEditor;
using System.Collections;


// This class displays a button in the same style as a normal EditorGUI.PopUp field, the difference being that you 
// do not have to provide the list of entries for the menu, instead you provide a callback which will be called
// when the user clicks the button allowing you to delay any possibly expensive menu creation to when it's actually
// needed or even allowing you to open something other than a standard menu.
public class EdDelayedPopup
{
    #region Public interface
    public static void Popup(string curentlySelected, System.Action<object> popup, object userData, params GUILayoutOption[] options)
    {
        Popup(curentlySelected, popup, userData, EditorStyles.popup, options);
    }

    public static void Popup(string curentlySelected, System.Action<object> popup, object userData, GUIStyle style, params GUILayoutOption[] options)
    {
        Rect position = EditorGUILayout.GetControlRect(false, 16f, style, options);
        Popup(position, curentlySelected, popup, userData, style);
    }
    public static void Popup(Rect position, string curentlySelected, System.Action<object> popup, object userData)
    {
        Popup(position, curentlySelected, popup, userData, EditorStyles.popup);
    }

    public static void Popup(Rect position, string curentlySelected, System.Action<object> popup, object userData, GUIStyle style)
    {
        DoPopup(EditorGUI.IndentedRect(position), GUIUtility.GetControlID(s_PopupHash, EditorGUIUtility.native, position), new GUIContent(curentlySelected), popup, userData, style);
    }

    public static void Popup(GUIContent curentlySelected, System.Action<object> popup, object userData, params GUILayoutOption[] options)
    {
        Popup(curentlySelected, popup, userData, EditorStyles.popup, options);
    }

    public static void Popup(GUIContent curentlySelected, System.Action<object> popup, object userData, GUIStyle style, params GUILayoutOption[] options)
    {
        Rect position = EditorGUILayout.GetControlRect(false, 16f, style, options);
        Popup(position, curentlySelected, popup, userData, style);
    }

    public static void Popup(Rect position, GUIContent curentlySelected, System.Action<object> popup, object userData)
    {
        Popup(position, curentlySelected, popup, userData, EditorStyles.popup);
    }

    public static void Popup(Rect position, GUIContent curentlySelected, System.Action<object> popup, object userData, GUIStyle style)
    {
        DoPopup(EditorGUI.IndentedRect(position), GUIUtility.GetControlID(s_PopupHash, EditorGUIUtility.native, position), curentlySelected, popup, userData, style);
    }

    // Now the same again but with a label
    public static void Popup(string label, string curentlySelected, System.Action<object> popup, object userData, params GUILayoutOption[] options)
    {
        Popup(label, curentlySelected, popup, userData, EditorStyles.popup, options);
    }

    public static void Popup(string label, string curentlySelected, System.Action<object> popup, object userData, GUIStyle style, params GUILayoutOption[] options)
    {
        Rect position = EditorGUILayout.GetControlRect(true, 16f, style, options);
        Popup(position, label, curentlySelected, popup, userData, style);
    }

    public static void Popup(Rect position, string label, string curentlySelected, System.Action<object> popup, object userData)
    {
        Popup(position, label, curentlySelected, popup, userData, EditorStyles.popup);
    }

    public static void Popup(Rect position, string label, string curentlySelected, System.Action<object> popup, object userData, GUIStyle style)
    {        
		int controlID = GUIUtility.GetControlID(s_PopupHash, EditorGUIUtility.native, position);
        DoPopup(EditorGUI.PrefixLabel(position, controlID, new GUIContent(label)), controlID, new GUIContent(curentlySelected), popup, userData, style);
    }

    public static void Popup(GUIContent label, GUIContent curentlySelected, System.Action<object> popup, object userData, params GUILayoutOption[] options)
    {
        Popup(label, curentlySelected, popup, userData, EditorStyles.popup, options);
    }

    public static void Popup(GUIContent label, GUIContent curentlySelected, System.Action<object> popup, object userData, GUIStyle style, params GUILayoutOption[] options)
    {
        Rect position = EditorGUILayout.GetControlRect(false, 16f, style, options);
        Popup(position, label, curentlySelected, popup, userData, style);
    }

    public static void Popup(Rect position, GUIContent label, GUIContent curentlySelected, System.Action<object> popup, object userData)
    {
        Popup(position, label, curentlySelected, popup, userData, EditorStyles.popup);
    }

    private static void Popup(Rect position, GUIContent label, GUIContent curentlySelected, System.Action<object> popup, object userData, GUIStyle style)
    {
        int controlID = GUIUtility.GetControlID(s_PopupHash, EditorGUIUtility.native, position);
        DoPopup(EditorGUI.PrefixLabel(position, controlID, label), controlID, curentlySelected, popup, userData, style);
    }

	// Returns the rectangle that the button occupies.
	static public Rect ButtonRect
	{		
		get {return s_ButtonRect;}
	}

	static public bool isFontBold = false; // Set to true before using if you want the text rendered in bold to signify that it's the same as a prefab.

    #endregion

    #region Private implimentation
	static Rect s_ButtonRect;
    private static int s_PopupHash = "PopupHash".GetHashCode();
    private static void DoPopup(Rect position, int controlID, GUIContent curentlySelected, System.Action<object> popup, object userData, GUIStyle style)
    {
        Event current = Event.current;
        EventType type = current.type;
        switch (type)
        {
            case EventType.KeyDown:
                if (MainActionKeyForControl(current, controlID))
                {
                    s_ButtonRect = position;
                    popup(userData); 
                    current.Use();
                }
                break;

            case EventType.Repaint:
                {
                    if (EditorGUI.showMixedValue)
                    {
                        curentlySelected = s_MixedValueContent;
                    }
                    Font font = style.font;
                    if (((font != null) && isFontBold) && (font == EditorStyles.miniFont))
                    {
                        style.font = EditorStyles.miniBoldFont;
                    }

                    s_MixedValueContentColorTemp = GUI.contentColor;
                    GUI.contentColor = !EditorGUI.showMixedValue ? GUI.contentColor : (GUI.contentColor * s_MixedValueContentColor);
                    style.Draw(position, curentlySelected, controlID, false);

                    GUI.contentColor = s_MixedValueContentColorTemp;
                    style.font = font;
                }
                break;
        }
        if ((type == EventType.MouseDown) && ((current.button == 0) && position.Contains(current.mousePosition)))
        {
            s_ButtonRect = position;
            popup(userData); 
            GUIUtility.keyboardControl = controlID;
            current.Use();
        }
    }

    static Color s_MixedValueContentColor = new Color(1f, 1f, 1f, 0.5f);
    static Color s_MixedValueContentColorTemp = Color.white;
    static GUIContent s_MixedValueContent = new GUIContent("—", "Mixed Values");
    
    static bool MainActionKeyForControl(Event evt, int controlId)
    {
        if (GUIUtility.keyboardControl != controlId)
        {
            return false;
        }
        bool flag = ((evt.alt || evt.shift) || evt.command) || evt.control;
        if (((evt.type == EventType.KeyDown) && (evt.character == ' ')) && !flag)
        {
            evt.Use();
            return false;
        }
        return (((evt.type == EventType.KeyDown) && (((evt.keyCode == KeyCode.Space) || (evt.keyCode == KeyCode.Return)) || (evt.keyCode == KeyCode.KeypadEnter))) && !flag);
    }
    #endregion
}
