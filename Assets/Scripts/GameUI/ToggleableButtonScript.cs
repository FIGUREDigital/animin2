using UnityEngine;
using System.Collections;

public class ToggleableButtonScript : MonoBehaviour {

    [SerializeField]
    private Sprite m_GraphicOn,m_GraphicOff;

    private bool m_On;

    private UnityEngine.UI.Image image;

	// Use this for initialization
	void Start () {
        image = this.GetComponent<UnityEngine.UI.Image>();
        if (image.sprite == m_GraphicOn)
            m_On = true;
        else if (image.sprite == m_GraphicOff)
            m_On = false;
        else
        {
            image.sprite = m_GraphicOff;
            m_On = false;
        }
	}

    public void Toggle(){
        m_On = !m_On;
        image.sprite = m_On ? m_GraphicOn : m_GraphicOff; 
    }
    public void SetOn(){
        m_On = true;
        image.sprite = m_GraphicOn; 
    }
    public void SetOff(){
        m_On = false;
        image.sprite = m_GraphicOff; 
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
