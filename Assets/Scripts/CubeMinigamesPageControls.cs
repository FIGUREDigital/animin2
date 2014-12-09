using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class CubeMinigamesPageControls : MonoBehaviour {

    MinigameCollectorScript MinigameScript;
    MinigameAnimationControllerScript CharacterAnimationRef;
    CharacterControllerScript CharacterControllerRef;

    [SerializeField]
    private GameObject[] m_Hearts, m_Stars;
    public GameObject[] Hearts { get { return m_Hearts; } }
    public GameObject[] Stars { get { return m_Stars; } }

    [SerializeField]
    private Text m_LevelCounter, m_Points;
    public Text LevelCounter { get { return m_LevelCounter; } }
    public Text PointLabel { get { return m_Points; } }

    [SerializeField]
    private GameObject m_PauseButton, m_PauseMenu;

    private bool m_Paused;
	// Use this for initialization
    void Start () {
        MinigameScript = UIGlobalVariablesScript.Singleton.CubeRunnerMinigameSceneRef.GetComponent<MinigameCollectorScript>();
    }
	
	// Update is called once per frame
    void Update () {

        UiPages.GetPage(Pages.JoystickPage).SetActive(this.gameObject.activeInHierarchy);

        if (m_Paused)
            return;


	}
    public void SetPause(bool On){
        m_Paused = On;
        m_PauseMenu.SetActive(On);
        m_PauseButton.SetActive(!On);
        UIGlobalVariablesScript.Singleton.CubeRunnerMinigameSceneRef.GetComponent<MinigameCollectorScript>().Paused = On;
    }
    public void ExitGame(){
        MinigameScript.ExitMinigame(false);
    }
    public void ToggleSound()
	{
        ProfilesManagementScript.Singleton.CurrentProfile.Settings.AudioEnabled = !ProfilesManagementScript.Singleton.CurrentProfile.Settings.AudioEnabled;


//        AudioListener audio = FindObjectOfType<AudioListener> ();
//        if(audio != null)
//        {
//            audio.enabled = !audio.enabled;
//        }
    }
    void OnEnable(){
        if (UiPages.GetPage(Pages.JoystickPage)!=null)UiPages.GetPage(Pages.JoystickPage).SetActive(true);
    }
    void OnDisable(){
        if (UiPages.GetPage(Pages.JoystickPage)!=null)UiPages.GetPage(Pages.JoystickPage).SetActive(false);
    }
}
