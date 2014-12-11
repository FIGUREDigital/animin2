using UnityEngine;
using System.Collections;

public class ToggleableButtonScript : MonoBehaviour {

    [SerializeField]
    private Sprite m_GraphicOn,m_GraphicOff;

    private bool m_On;

    private UnityEngine.UI.Image Image{
        get{
            if (m_Image == null)
                m_Image = this.GetComponent<UnityEngine.UI.Image>();
            return m_Image;
        }
    }
    private UnityEngine.UI.Image m_Image;

	// Use this for initialization
	void Start () {
        if (Image.sprite == m_GraphicOn)
            m_On = true;
        else if (Image.sprite == m_GraphicOff)
            m_On = false;
        else
        {
            Image.sprite = m_GraphicOff;
            m_On = false;
        }
	}

    public void Toggle(){
        m_On = !m_On;
        Image.sprite = m_On ? m_GraphicOn : m_GraphicOff; 
    }
    public void SetOn(){
        m_On = true;
        Image.sprite = m_GraphicOn; 
    }
    public void SetOff(){
        m_On = false;
        Image.sprite = m_GraphicOff; 
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
