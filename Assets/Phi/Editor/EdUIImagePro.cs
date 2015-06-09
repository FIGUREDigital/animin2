
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[CanEditMultipleObjects, CustomEditor(typeof(UIImagePro), true)]
public class EdUIImagePro : UnityEditor.UI.GraphicEditor
{
    private GUIContent m_ClockwiseContent;
    private SerializedProperty m_FillAmount;
    private SerializedProperty m_FillCenter;
    private SerializedProperty m_FillClockwise;
    private SerializedProperty m_FillMethod;
    private SerializedProperty m_FillOrigin;
    private SerializedProperty m_PreserveAspect;
    private AnimBool m_ShowFilled;
    private AnimBool m_ShowSliced;
    private AnimBool m_ShowSlicedOrTiled;
    private AnimBool m_ShowType;
    private SerializedProperty m_Sprite;
    private GUIContent m_SpriteContent;
    private GUIContent m_SpriteTypeContent;
    private SerializedProperty m_Type;

    private AnimBool m_ShowTiled;
    private GUIContent m_ImageTransformContent;
    private SerializedProperty m_ImageTransform;
    private GUIContent m_RoundTilingContent;
    private SerializedProperty m_RoundTiling;
    private SerializedProperty m_ButterflyX;
    private SerializedProperty m_ButterflyY;
    private SerializedProperty m_ScalePixelSize;
    private SerializedProperty m_SizeByCenter;
    private GUIContent m_SetSizeContent;
    private SerializedProperty m_SetSize;
    private SerializedProperty m_Scale;
    private Vector2 m_CurrentSize;
    private bool m_CurrentSizeMultipleX;
    private bool m_CurrentSizeMultipleY;
    public override string GetInfoString()
    {
        UIImagePro target = this.target as UIImagePro;
        Sprite sprite = target.sprite;
        int num = (sprite == null) ? 0 : Mathf.RoundToInt(sprite.rect.width);
        int num2 = (sprite == null) ? 0 : Mathf.RoundToInt(sprite.rect.height);
        return string.Format("Image Size: {0}x{1}", num, num2);
    }

    public override bool HasPreviewGUI()
    {
        return true;
    }

    protected override void OnDisable()
    {
        this.m_ShowType.valueChanged.RemoveListener(new UnityAction(this.Repaint));
        this.m_ShowSlicedOrTiled.valueChanged.RemoveListener(new UnityAction(this.Repaint));
        this.m_ShowSliced.valueChanged.RemoveListener(new UnityAction(this.Repaint));
        this.m_ShowFilled.valueChanged.RemoveListener(new UnityAction(this.Repaint));
        this.m_ShowTiled.valueChanged.RemoveListener(new UnityAction(this.Repaint));
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        this.m_SpriteContent = new GUIContent("Source Image");
        this.m_SpriteTypeContent = new GUIContent("Image Type");
        this.m_ImageTransformContent = new GUIContent("Image Transform");
        this.m_RoundTilingContent = new GUIContent("Round tiling");
        this.m_ClockwiseContent = new GUIContent("Clockwise");
        this.m_SetSizeContent = new GUIContent("Set size");
        this.m_Sprite = base.serializedObject.FindProperty("m_Sprite");
        this.m_Type = base.serializedObject.FindProperty("m_Type");
        this.m_ImageTransform = base.serializedObject.FindProperty("m_ImageTransform");
        this.m_RoundTiling = base.serializedObject.FindProperty("m_RoundTiling");
        this.m_ButterflyX = base.serializedObject.FindProperty("m_ButterflyX");
        this.m_ButterflyY = base.serializedObject.FindProperty("m_ButterflyY");
        this.m_ScalePixelSize = base.serializedObject.FindProperty("m_ScalePixelSize");
        this.m_SizeByCenter = base.serializedObject.FindProperty("m_SizeByCenter");
        this.m_SetSize = base.serializedObject.FindProperty("m_SetSize");
        this.m_Scale = base.serializedObject.FindProperty("m_Scale");
        
        this.m_FillCenter = base.serializedObject.FindProperty("m_FillCenter");
        this.m_FillMethod = base.serializedObject.FindProperty("m_FillMethod");
        this.m_FillOrigin = base.serializedObject.FindProperty("m_FillOrigin");
        this.m_FillClockwise = base.serializedObject.FindProperty("m_FillClockwise");
        this.m_FillAmount = base.serializedObject.FindProperty("m_FillAmount");
        this.m_PreserveAspect = base.serializedObject.FindProperty("m_PreserveAspect");
        this.m_ShowType = new AnimBool(this.m_Sprite.objectReferenceValue != null);
        this.m_ShowType.valueChanged.AddListener(new UnityAction(this.Repaint));
        this.m_ShowSlicedOrTiled = new AnimBool(ShowSlicedOrTiled);
        this.m_ShowTiled = new AnimBool(ShowTiled);
        this.m_ShowSliced = new AnimBool(ShowSliced);
        this.m_ShowFilled = new AnimBool(ShowFilled);

        this.m_ShowSlicedOrTiled.valueChanged.AddListener(new UnityAction(this.Repaint));
        this.m_ShowTiled.valueChanged.AddListener(new UnityAction(this.Repaint));
        this.m_ShowSliced.valueChanged.AddListener(new UnityAction(this.Repaint));
        this.m_ShowFilled.valueChanged.AddListener(new UnityAction(this.Repaint));
        this.SetShowNativeSize(true);
    }

    void UpdateCurrentSize()
    {
        // Work out what current sizes are.
        bool sizeSet = false;
        m_CurrentSizeMultipleX = false;
        m_CurrentSizeMultipleY = false;
        foreach(object t in targets)
        {
            UIImagePro imagePro = t as UIImagePro;
            if (imagePro != null)
            {
                Vector2 size = imagePro.GetCurrentRelativeSize();
                if (!sizeSet)
                {
                    m_CurrentSize = size;
                    sizeSet = true;
                }
                else
                {
                    if (m_CurrentSize.x != size.x)
                    {
                        m_CurrentSizeMultipleX = true;
                    }
                    if (m_CurrentSize.y != size.y)
                    {
                        m_CurrentSizeMultipleY = true;
                    }
                }
            }
        }
    }

    public override void OnInspectorGUI()
    {
        base.serializedObject.Update();
        SpriteGUI();
        base.AppearanceControlsGUI();  
        
        this.m_ShowType.target = this.m_Sprite.objectReferenceValue != null;
        if (EditorGUILayout.BeginFadeGroup(this.m_ShowType.faded))
        {
            this.TypeGUI();
        }
        EditorGUILayout.EndFadeGroup();
        this.SetShowNativeSize(false);
        if (EditorGUILayout.BeginFadeGroup(base.m_ShowNativeSize.faded))
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(this.m_PreserveAspect, new GUILayoutOption[0]);
            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndFadeGroup();
        //EditorGUILayout.PropertyField(this.m_SetSize, new GUILayoutOption[0]);
        //EditorGUILayout.PropertyField(this.m_Scale, new GUILayoutOption[0]);
        AutoSetSize();
        //base.NativeSizeButtonGUI();   // No longer need this as you can just set the current size to 1
        base.serializedObject.ApplyModifiedProperties();
    }
    public void AutoSetSize()
    {
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        var indent = EditorGUI.indentLevel;
        
        Rect position = EditorGUILayout.BeginHorizontal();
        EditorGUI.BeginProperty(position, m_SetSizeContent, m_SetSize);

        bool updateX = false;
        bool updateY = false;
        EditorGUILayout.PropertyField(this.m_SetSize, new GUILayoutOption[0]);
        if (this.m_SetSize.hasMultipleDifferentValues || m_SetSize.boolValue)
        {
            EditorGUI.indentLevel = 0;
            Vector2 oldValue = this.m_Scale.vector2Value;
            EditorGUILayout.PropertyField(this.m_Scale, GUIContent.none);//new GUILayoutOption[0]);
            if (oldValue != m_Scale.vector2Value)
            {
                // User edited scale...
                m_CurrentSize = m_Scale.vector2Value;
                updateX = oldValue.x != m_Scale.vector2Value.x;
                updateY = oldValue.y != m_Scale.vector2Value.y;
            }
            EditorGUI.indentLevel = indent;
        }
        else
        {
            // Allow user to edit size
            UpdateCurrentSize();    // If caching this then need to set showMixedValue when edited.
            EditorGUI.indentLevel = 0;
            float labelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 13;
            EditorGUI.showMixedValue = m_CurrentSizeMultipleX;
            EditorGUI.BeginChangeCheck();
            m_CurrentSize.x = EditorGUILayout.FloatField("X", m_CurrentSize.x);
            updateX = EditorGUI.EndChangeCheck();
            EditorGUI.showMixedValue = m_CurrentSizeMultipleY;
            EditorGUI.BeginChangeCheck();
            m_CurrentSize.y = EditorGUILayout.FloatField("Y", m_CurrentSize.y);
            updateY = EditorGUI.EndChangeCheck();
            EditorGUIUtility.labelWidth = labelWidth;
            EditorGUI.indentLevel = indent;
        }
        EditorGUI.EndProperty();
        EditorGUILayout.EndHorizontal();

        if (updateX || updateY)
        {
            foreach(UnityEngine.Object t in targets)
            {
                UIImagePro imagePro = t as UIImagePro;
                if(imagePro != null && !imagePro.setSize)
                {
                    if (updateX)
                    {
                        Undo.RecordObject(imagePro.transform, "Edit width");
                        imagePro.SetRelativeWidth(m_CurrentSize.x);
                    }
                    if (updateY)
                    {
                        Undo.RecordObject(imagePro.transform, "Edit height");
                        imagePro.SetRelativeHeight(m_CurrentSize.y);
                    }
                }
            }
        }


        /*

        // Draw label
        //position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), m_SetSizeContent);

        // Don't make child fields be indented

        // Calculate rects
        var amountRect = new Rect(position.x, position.y, 30, position.height);
        var unitRect = new Rect(position.x + 35, position.y, 50, position.height);
        var nameRect = new Rect(position.x + 90, position.y, position.width - 90, position.height);

        // Draw fields - passs GUIContent.none to each so they are drawn without labels
        EditorGUI.PropertyField(amountRect, property.FindPropertyRelative("amount"), GUIContent.none);
        EditorGUI.PropertyField(unitRect, property.FindPropertyRelative("unit"), GUIContent.none);
        EditorGUI.PropertyField(nameRect, property.FindPropertyRelative("name"), GUIContent.none);

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
        */
    }
    
    public override void OnPreviewGUI(Rect rect, GUIStyle background)
    {
        UIImagePro target = this.target as UIImagePro;
        if (target != null)
        {
            Sprite sprite = target.sprite;
            if (sprite != null)
            {
                //UnityEditor.UI.EditorTools.DrawSprite(sprite, rect, target.canvasRenderer.GetColor());
            }
        }
    }

    private void SetShowNativeSize(bool instant)
    {
//        UIImagePro.Type enumValueIndex = (UIImagePro.Type)this.m_Type.enumValueIndex;
        bool show = true;// (enumValueIndex == UIImagePro.Type.Simple) || (enumValueIndex == UIImagePro.Type.Filled);
        base.SetShowNativeSize(show, instant);
    }

    protected void SpriteGUI()
    {
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(this.m_Sprite, this.m_SpriteContent, new GUILayoutOption[0]);
        if (EditorGUI.EndChangeCheck())
        {
            Sprite objectReferenceValue = this.m_Sprite.objectReferenceValue as Sprite;
            if (objectReferenceValue != null)
            {
                UIImagePro.Type enumValueIndex = (UIImagePro.Type)this.m_Type.enumValueIndex;
                if (objectReferenceValue.border.SqrMagnitude() > 0f)
                {
                    this.m_Type.enumValueIndex = 1;
                }
                else if (enumValueIndex == UIImagePro.Type.Sliced)
                {
                    this.m_Type.enumValueIndex = 0;
                }
            }
        }
    }


    bool ShowTiled
    {
        get
        {
            UIImagePro.Type enumValueIndex = (UIImagePro.Type)this.m_Type.enumValueIndex;
            return !this.m_Type.hasMultipleDifferentValues && enumValueIndex == UIImagePro.Type.Tiled;
        }
    }

    bool ShowSliced
    {
        get
        {
            UIImagePro.Type enumValueIndex = (UIImagePro.Type)this.m_Type.enumValueIndex;
            return !this.m_Type.hasMultipleDifferentValues && enumValueIndex == UIImagePro.Type.Sliced;
        }
    }

    bool ShowFilled
    {
        get
        {
            UIImagePro.Type enumValueIndex = (UIImagePro.Type)this.m_Type.enumValueIndex;
            return !this.m_Type.hasMultipleDifferentValues && enumValueIndex == UIImagePro.Type.Filled;
        }
    }

    bool ShowSlicedOrTiled
    {
        get
        {
            UIImagePro.Type enumValueIndex = (UIImagePro.Type)this.m_Type.enumValueIndex;
            bool flag = !this.m_Type.hasMultipleDifferentValues && ((enumValueIndex == UIImagePro.Type.Sliced) || (enumValueIndex == UIImagePro.Type.Tiled));
            if (!flag && this.m_Type.hasMultipleDifferentValues)
            {
                // See if it's only Sliced and Tiled items
                flag = true;
                foreach (UnityEngine.Object o in targets)
                {
                    UIImagePro image = o as UIImagePro;
                    UIImagePro.Type type = image.type;
                    if (type != UIImagePro.Type.Sliced && type != UIImagePro.Type.Tiled)
                    {
                        flag = false;
                        break;
                    }
                }
            }
            return flag;
        }
    }

    protected void TypeGUI()
    {
        EditorGUILayout.PropertyField(this.m_ImageTransform, this.m_ImageTransformContent, new GUILayoutOption[0]);
        EditorGUILayout.PropertyField(this.m_Type, this.m_SpriteTypeContent, new GUILayoutOption[0]);
        EditorGUI.indentLevel++;
     
        this.m_ShowSlicedOrTiled.target = ShowSlicedOrTiled;
        this.m_ShowTiled.target = ShowTiled;
        this.m_ShowSliced.target = ShowSliced;
        this.m_ShowFilled.target = ShowFilled;

        UIImagePro target = this.target as UIImagePro;
        if (EditorGUILayout.BeginFadeGroup(this.m_ShowSlicedOrTiled.faded) && target.hasBorder)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(this.m_FillCenter, new GUILayoutOption[0]);

            float labelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 110;
            EditorGUILayout.PropertyField(this.m_SizeByCenter, new GUILayoutOption[0]);
            EditorGUIUtility.labelWidth = labelWidth;
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.PropertyField(this.m_ScalePixelSize, new GUILayoutOption[0]);
        }
        EditorGUILayout.EndFadeGroup();
        if (EditorGUILayout.BeginFadeGroup(this.m_ShowTiled.faded))
        {
            EditorGUILayout.PropertyField(this.m_RoundTiling, this.m_RoundTilingContent, new GUILayoutOption[0]);
            EditorGUILayout.PropertyField(this.m_ButterflyX, new GUILayoutOption[0]);
            EditorGUILayout.PropertyField(this.m_ButterflyY, new GUILayoutOption[0]);
        }
        EditorGUILayout.EndFadeGroup();
        if ((EditorGUILayout.BeginFadeGroup(this.m_ShowSliced.faded) && (target.sprite != null)) && !target.hasBorder)
        {
            EditorGUILayout.HelpBox("This Image doesn't have a border.", MessageType.Warning);
        }
        EditorGUILayout.EndFadeGroup();
        if (EditorGUILayout.BeginFadeGroup(this.m_ShowFilled.faded))
        {
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(this.m_FillMethod, new GUILayoutOption[0]);
            if (EditorGUI.EndChangeCheck())
            {
                this.m_FillOrigin.intValue = 0;
            }
            switch (this.m_FillMethod.enumValueIndex)
            {
                case 0:
                    this.m_FillOrigin.intValue = (int)((UIImagePro.OriginHorizontal)EditorGUILayout.EnumPopup("Fill Origin", (UIImagePro.OriginHorizontal)this.m_FillOrigin.intValue, new GUILayoutOption[0]));
                    break;

                case 1:
                    this.m_FillOrigin.intValue = (int)((UIImagePro.OriginVertical)EditorGUILayout.EnumPopup("Fill Origin", (UIImagePro.OriginVertical)this.m_FillOrigin.intValue, new GUILayoutOption[0]));
                    break;

                case 2:
                    this.m_FillOrigin.intValue = (int)((UIImagePro.Origin90)EditorGUILayout.EnumPopup("Fill Origin", (UIImagePro.Origin90)this.m_FillOrigin.intValue, new GUILayoutOption[0]));
                    break;

                case 3:
                    this.m_FillOrigin.intValue = (int)((UIImagePro.Origin180)EditorGUILayout.EnumPopup("Fill Origin", (UIImagePro.Origin180)this.m_FillOrigin.intValue, new GUILayoutOption[0]));
                    break;

                case 4:
                    this.m_FillOrigin.intValue = (int)((UIImagePro.Origin360)EditorGUILayout.EnumPopup("Fill Origin", (UIImagePro.Origin360)this.m_FillOrigin.intValue, new GUILayoutOption[0]));
                    break;
            }
            EditorGUILayout.PropertyField(this.m_FillAmount, new GUILayoutOption[0]);
            if (this.m_FillMethod.enumValueIndex > 1)
            {
                EditorGUILayout.PropertyField(this.m_FillClockwise, this.m_ClockwiseContent, new GUILayoutOption[0]);
            }
        }
        EditorGUILayout.EndFadeGroup();
        EditorGUI.indentLevel--;
    }
}
