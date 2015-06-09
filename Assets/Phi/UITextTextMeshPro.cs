using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using TMPro;
#if UNITY_EDITOR
[UnityEditor.InitializeOnLoad]
#endif
public class UITextTextMeshPro : UITextRenderer 
{    
    static UITextTextMeshPro registeredInstance;

    private TextMeshProUGUI rendererComponent;

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
    static UITextTextMeshPro()
    {
        registeredInstance = new UITextTextMeshPro(null);
        UIText.Register(registeredInstance);
    }

    public override UITextRenderer Create(GameObject go)
    {
        rendererComponent = go.GetComponent<TextMeshProUGUI>();
        if (rendererComponent)
        {
            return new UITextTextMeshPro(rendererComponent);
        }
        return null;
    }

    public UITextTextMeshPro(TextMeshProUGUI rendererUGUI)
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
