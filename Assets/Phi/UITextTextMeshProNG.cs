using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using TMPro;
#if UNITY_EDITOR
[UnityEditor.InitializeOnLoad]
#endif
// Can't serialize this class as polymorphism does not serialize in unity
public class UITextTextMeshProNG : UITextRenderer 
{    
    static UITextTextMeshProNG registeredInstance;

    private TextMeshPro rendererComponent;

    public override MonoBehaviour TextRendererComponent
    {
        get
        {
            return rendererComponent;
        }
    }
	static public void Init()
	{
		//No need to do anything the static constructor will do it for us.
	}
    static UITextTextMeshProNG()
    {
        registeredInstance = new UITextTextMeshProNG(null);
        UIText.Register(registeredInstance);
    }
    public override UITextRenderer Create(GameObject go)
    {
        rendererComponent = go.GetComponent<TextMeshPro>();
        if (rendererComponent)
        {
            return new UITextTextMeshProNG(rendererComponent);
        }
        return null;
    }

    public UITextTextMeshProNG(TextMeshPro rendererUGUI)
    {
        this.rendererComponent = rendererUGUI;
    }

    public override string Text
    {
        get
        {
            return rendererComponent.text;
        }
        set
        {
            rendererComponent.text = value;
        }
    }

    public override Color Color
    {
        get
        {
            return rendererComponent.color;
        }
        set
        {
            rendererComponent.color = value;
        }
    }


    public override Material Material
    {
        get
        {
            return rendererComponent.fontSharedMaterial;
        }
        set
        {
            rendererComponent.fontSharedMaterial = value;
        }
    }
}
