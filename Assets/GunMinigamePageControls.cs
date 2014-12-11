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
    private UnityEngine.UI.Image m_Bar, m_Go321, m_GunIcon;


    [SerializeField]
    private Sprite[] Go321Textures;

    public UnityEngine.UI.Image Bar { get { return m_Bar; } }
    public UnityEngine.UI.Image Icon { get { return m_GunIcon; } }
    public UnityEngine.UI.Image Go321 { get { return m_Go321; } }



    [SerializeField]
    private UnityEngine.UI.Text m_Points;
    public UnityEngine.UI.Text Points { get { return m_Points; } }


    [SerializeField]
    private GameObject m_TutorialEnemies, m_TutorialMove;
    private int TutorialCounter;

	private bool m_Paused;
	public bool Paused {
		get{ return m_Paused;}
		set{ m_Paused = value;}
	}

    private JoystickPageControls JoystickControls{
        get{
            if (m_JoystickControls == null)
                m_JoystickControls = UiPages.GetPage(Pages.JoystickPage).GetComponent<JoystickPageControls>();

            return m_JoystickControls;
        }
    }
    private JoystickPageControls m_JoystickControls;

    public bool WaitingForTouch{
        get{
            return TutorialCounter == 0 || TutorialCounter == 1;
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
    void Update () {
        if (ProfilesManagementScript.Singleton.CurrentProfile.TutorialBoxLandPlayed == false)
        {
            switch(TutorialCounter){
                case 0:
					if (!m_Paused){
	                    m_Points.transform.parent.gameObject.SetActive(false);
	                    m_Go321.gameObject.SetActive(false);

	                    m_TutorialEnemies.SetActive(true);

	                    TutorialCounter++;
					}
                    break;
                case 1:
                    if (Input.GetButtonUp("Fire1"))
                    {
                        m_TutorialEnemies.SetActive(false);
                        m_TutorialMove.SetActive(true);
                        TutorialCounter++;
                    }
                    break;
                case 2:
                    if (JoystickControls.IsMovingWithJoystick)
                    {

                        m_TutorialMove.SetActive(false);
                        m_Points.transform.parent.gameObject.SetActive(true);
						GunsMinigameScript script = UIGlobalVariablesScript.Singleton.GunGameScene.GetComponent<GunsMinigameScript>();
						if (script!=null)
							if (!script.Go321Done)
                        		m_Go321.gameObject.SetActive(true);
                        ProfilesManagementScript.Singleton.CurrentProfile.TutorialBoxLandPlayed = true;
                        TutorialCounter++;
                    }
                    break;
            }
        }
	}
	public void ResetTutorial(){
		m_TutorialEnemies.SetActive (false);
		m_TutorialMove.SetActive(false);
		TutorialCounter = 0;
		ProfilesManagementScript.Singleton.CurrentProfile.TutorialBoxLandPlayed = false;
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
