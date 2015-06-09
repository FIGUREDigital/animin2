#define  USETMPRO
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[ExecuteInEditMode]
public class UIText : MonoBehaviour {

    [SerializeField, Tooltip("To populate this from the localization database enter the ID here")]
    private string localizeID;

    [SerializeField, TextArea(3,10)]
    private string text;

    //[HideInInspector]
    [NonSerialized]
    public UITextRenderer textRenderer;   // Can't serialize this class as polymorphism does not serialize in unity

    [SerializeField]
    private Color color;
    
    [SerializeField]
    private bool allCaps;

    [SerializeField]
    private Material material;

    private bool isLocalized = false;

    [NonSerialized]
    public Action<string> onTextChanged = null;    // Allow other components to be triggered when the text is changed.

    [NonSerialized]
    public Action<Color> onColorChanged = null;    // Allow other components to be triggered when the color is changed.


    //private static Dictionary<TextTypes, IUITextRenderer> rendererTypes = new Dictionary<TextTypes, IUITextRenderer>();
    private static List<UITextRenderer> rendererTypes = new List<UITextRenderer>();

    public static void Register(UITextRenderer renderer)
    {
        rendererTypes.Add(renderer);
    }

    [ContextMenu("Toggle renderer visibility")]
    void ToggleRVis()
    {
        Debug.Log("Toggle renderer visibility");
        MonoBehaviour component = textRenderer.TextRendererComponent;
        if (component)
        {
            component.hideFlags = component.hideFlags ^ HideFlags.HideInInspector;
        }
    }
    // Returns true if the string currently holds the localized text, false otherwise.
    // Note if the text has been set manually this allways returns false until Localize() is called or the LocalizeID set.
    public bool IsLocalized
    {
        get
        {
            return isLocalized;
        }
    }

    void Start()
    {
        CheckValid();
        Localize();
    }

    void OnEnable()
    {
        CheckValid();
    }

    void OnDestroy()
    {
        if (textRenderer != null)
        {
            MonoBehaviour component = textRenderer.TextRendererComponent;
            if (component)
            {
                component.hideFlags = component.hideFlags & ~HideFlags.HideInInspector;
            }
        }
/*        if (rendererUGUI)
        {
            rendererUGUI.hideFlags = rendererUGUI.hideFlags & ~HideFlags.HideInInspector;
        }
#if USETMPRO
        else if (rendererTextMeshPro)
        {
            rendererTextMeshPro.hideFlags = rendererTextMeshPro.hideFlags & ~HideFlags.HideInInspector;
        }
#endif*/
    }

    void CheckValid()
    {
        if(textRenderer != null)
        {
            if (!textRenderer.TextRendererComponent)
            {
                textRenderer = null;
            }
        }
        if (textRenderer == null)
        {
            for(int i = 0; i < rendererTypes.Count; i++)
            {
                textRenderer = rendererTypes[i].Create(gameObject);
                if (textRenderer != null)
                {
                    if (!Application.isPlaying)
                    {
                        CopySettingsFromRenderer();
                    }
                    UpdateMaterial();
                    Localize();
                    break;
                }                
            }
        }
    }

    public bool AllCaps
    {
        get
        {
            return allCaps;
        }
        set
        {
            if (allCaps != value)
            {
                allCaps = value;
                UpdateText(isLocalized);
            }
        }
    }
    public string Text
    {
        get
        {
            return text;
        }
        set
        {
            if (value != text)
            {
                text = value;
                UpdateText();
            }
        }
    }

    public Color Color
    {
        get
        {
            return color;
        }
        set
        {
            if (value != color)
            {
                color = value;
                UpdateColor();
            }
        }
    }


    public string LocalizeID
    {
        get
        {
            return localizeID;
        }
        set
        {
            if (value != localizeID)
            {
                localizeID = value;
                Localize();
            }
        }
    }

    public void Localize()
    {
        if (localizeID != null && localizeID.Length > 0)
        {
			Text = localizeID;//Language.Get(localizeID);
            isLocalized = true;
        }
    }
        
    public void UpdateText(bool setLocalized = false)
    {
		if (textRenderer == null)
		{
			CheckValid();
		}
		if(textRenderer != null)
		{
			if (allCaps)
	        {
	            textRenderer.Text = text.ToUpper();
	        }
	        else
	        {
	            textRenderer.Text = text;
	        }
		}
		else
		{			
			Debug.Log ("Failed to set Text on "+name+" to "+text);
		}
        isLocalized = setLocalized;
        if(onTextChanged != null)
        {
            onTextChanged(text);
        }
    }


    public void UpdateMaterial()
    {
        if (textRenderer == null)
        {
            CheckValid();
        }
        if (textRenderer != null && material != null)
        {
            textRenderer.Material = material;
        }
    }

    public void UpdateColor()
    {
        if (textRenderer != null)
        {
            textRenderer.Color = color;

            if (onColorChanged != null)
            {
                onColorChanged(color);
            }
        }
    }

    public void CopySettingsFromRenderer()
    {
        if (allCaps)
        {
            text = textRenderer.Text.ToUpper();
            textRenderer.Text = text;
        }
        else
        {
            text = textRenderer.Text;
        }
        color = textRenderer.Color;
    }

    public void OnDidApplyAnimationProperties()
    {
        UpdateText(isLocalized);
        UpdateColor();
    }
}
