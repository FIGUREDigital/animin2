using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
#if UNITY_EDITOR
[UnityEditor.InitializeOnLoad]
#endif
[Serializable]
public class UITextUGUI : UITextRenderer 
{    
    static UITextUGUI registeredInstance;

    [SerializeField]
    private UnityEngine.UI.Text rendererComponent;

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
    static UITextUGUI()
    {
        registeredInstance = new UITextUGUI(null);
        UIText.Register(registeredInstance);
    }

    public override UITextRenderer Create(GameObject go)
    {
        rendererComponent = go.GetComponent<UnityEngine.UI.Text>();
        if (rendererComponent)
        {
            return new UITextUGUI(rendererComponent);
        }
        return null;
    }

    public UITextUGUI(UnityEngine.UI.Text rendererUGUI)
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
            return rendererComponent.material;
        }
        set
        {
            rendererComponent.material = value;
        }
    }
}
