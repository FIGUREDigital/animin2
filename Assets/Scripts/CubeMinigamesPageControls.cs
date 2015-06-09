using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using TMPro;

public class CubeMinigamesPageControls : MonoBehaviour {

    MinigameCollectorScript MinigameScript;
    MinigameAnimationControllerScript CharacterAnimationRef;
    CharacterControllerScript CharacterControllerRef;

    [SerializeField]
    private GameObject[] m_Hearts, m_Stars;
    public GameObject[] Hearts { get { return m_Hearts; } }
    public GameObject[] Stars { get { return m_Stars; } }

    [SerializeField]
    private TextMeshProUGUI m_LevelCounter, m_Points;
	public TextMeshProUGUI LevelCounter { get { return m_LevelCounter; } }
	public TextMeshProUGUI PointLabel { get { return m_Points; } }

    [SerializeField]
    private GameObject m_TutorialMove, m_TutorialJump, m_TutorialSwipe;
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
	// Use this for initialization
    void Start () {
        m_TutorialMove.SetActive(false);
        m_TutorialJump.SetActive(false);
        m_TutorialSwipe.SetActive(false);
    }
	
	// Update is called once per frame
    void Update () {
        if (m_Paused)
            return;
        if (ProfilesManagementScript.Instance.CurrentProfile.TutorialBoxLandPlayed == false)
        {
            switch(TutorialCounter){
                case 0:
					if(!m_Paused){
	                    m_TutorialMove.SetActive(true);
	                    TutorialCounter++;
					}
                    break;
                case 1:
                    if (JoystickControls.IsMovingWithJoystick)
                    {
                        m_TutorialMove.SetActive(false);
                        m_TutorialJump.SetActive(true);
                        TutorialCounter++;
                    }
                    break;
                case 2:
                    if (JoystickControls.PressedJump)
                    {
                        m_TutorialJump.SetActive(false);
                        m_TutorialSwipe.SetActive(true);
                        TutorialCounter++;
                    }
                    break;
                case 3:
                    if (MinigameScript.IsSwiping)
                    {
                        m_TutorialSwipe.SetActive(false);
                        ProfilesManagementScript.Instance.CurrentProfile.TutorialBoxLandPlayed = true;
                        TutorialCounter++;
                    }
                    break;
            }
        }
	}
	public void ResetTutorial(){
		m_TutorialJump.SetActive (false);
		m_TutorialMove.SetActive (false);
		m_TutorialSwipe.SetActive (false);
		TutorialCounter = 0;
		ProfilesManagementScript.Instance.CurrentProfile.TutorialBoxLandPlayed = false;
	}

	void OnEnable(){
        if (!ProfilesManagementScript.Instance.CurrentProfile.TutorialBoxLandPlayed)
            TutorialCounter = 0;
		m_TutorialJump.SetActive (false);
		m_TutorialMove.SetActive (false);
		m_TutorialSwipe.SetActive (false);
        if (UiPages.GetPage(Pages.JoystickPage)!=null)UiPages.GetPage(Pages.JoystickPage).SetActive(true);
        if (UIGlobalVariablesScript.Singleton.CubeRunnerMinigameSceneRef!=null) MinigameScript = UIGlobalVariablesScript.Singleton.CubeRunnerMinigameSceneRef.GetComponent<MinigameCollectorScript>();
    }
    void OnDisable(){
        if (UiPages.GetPage(Pages.JoystickPage)!=null)UiPages.GetPage(Pages.JoystickPage).SetActive(false);
    }
}
