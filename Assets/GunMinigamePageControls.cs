using UnityEngine;
using System.Collections;

public enum ReadyStates{
    Ready3,
    Ready2,
    Ready1,
    Go,
    Count,
}


public class GunMinigamePageControls : MonoBehaviour {

    [SerializeField]
    private UnityEngine.UI.Image m_Bar, m_Go321, m_AmmoType;


    [SerializeField]
    private Sprite[] Go321Textures;

    public UnityEngine.UI.Image Bar { get { return m_Bar; } }
    public UnityEngine.UI.Image Go321 { get { return m_Go321; } }



    [SerializeField]
    private UnityEngine.UI.Text m_Points;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetReadyState(ReadyStates newState){
        switch (newState)
        {
            case ReadyStates.Ready3:
                Go321.sprite = Go321Textures[0];
                break;
            case ReadyStates.Ready2:
                Go321.sprite = Go321Textures[1];
                break;
            case ReadyStates.Ready1:
                Go321.sprite = Go321Textures[2];
                break;
            case ReadyStates.Go:
                Go321.sprite = Go321Textures[3];
                break;
        }
    }

    public void SetBarWidth(float width){
        m_Bar.fillAmount = width;
    }

    public void SetAmmoType(){

    }

    void OnEnable(){
        Debug.Log("onEnable");
        if (UiPages.GetPage(Pages.JoystickPage)!=null)UiPages.GetPage(Pages.JoystickPage).SetActive(true);
    }
    void OnDisable(){
        Debug.Log("onDisable");
        if (UiPages.GetPage(Pages.JoystickPage)!=null)UiPages.GetPage(Pages.JoystickPage).SetActive(false);
    }
}
