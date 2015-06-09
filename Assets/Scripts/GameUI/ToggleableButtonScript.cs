using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ToggleableButtonScript : MonoBehaviour {

    [SerializeField]
    private Sprite m_GraphicOn,m_GraphicOff;
	
	[SerializeField]
	private GameObject m_Visibility;
	
	[SerializeField]
	private float offAlpha;
	[SerializeField]
	private float onAlpha;
	[SerializeField]
	private Graphic[] alphas;
    
    private bool m_On;

    private UnityEngine.UI.Image Image{
        get{
			if (m_Image == null && m_Visibility == null)
                m_Image = this.GetComponent<UnityEngine.UI.Image>();
            return m_Image;
        }
    }
    private Image m_Image;

	// Use this for initialization
	void Start () 
	{
		if(Image != null)
		{
			Set(Image.sprite == m_GraphicOn);
		}
		else if (m_Visibility != null)
		{
			Set(m_Visibility.activeSelf);
		}
		else
		{
			Set(false);
		}
	}

    public void Toggle()
	{
		Set(!m_On);
    }

	public void Set(bool set)
	{		
		m_On = set;
		if(Image != null)
		{
			Image.sprite = set ? m_GraphicOn : m_GraphicOff; 
		}
		if(m_Visibility)
		{
            m_Visibility.SetActive (set);
        }
		float a = set ? onAlpha : offAlpha; 
		for(int i = 0; i < alphas.Length; i++)
		{
			Color c = alphas[i].color;
			c.a = a;
			alphas[i].color = c;
		}
	}

    public void SetOn()
	{
		Set(true);
    }

    public void SetOff()
	{		
		Set(false);
    }
}
