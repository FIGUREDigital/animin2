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

    void OnEnable(){
        if (UiPages.GetPage(Pages.JoystickPage)!=null)UiPages.GetPage(Pages.JoystickPage).SetActive(true);
    }
    void OnDisable(){
        if (UiPages.GetPage(Pages.JoystickPage)!=null)UiPages.GetPage(Pages.JoystickPage).SetActive(false);
    }
}
