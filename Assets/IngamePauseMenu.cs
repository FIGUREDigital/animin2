using UnityEngine;
using System.Collections;

public class IngamePauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject m_PauseMenu, m_PauseButton;

    private JoystickPageControls JoystickScript{
        get{
            if (m_JoystickScript == null)
                m_JoystickScript = UiPages.GetPage(Pages.JoystickPage).GetComponent<JoystickPageControls>();
            return m_JoystickScript;
        }
    }
    private JoystickPageControls m_JoystickScript;

    bool m_Paused;
    // Use this for initialization
    void Awake()
    {

        m_Paused = false;
        m_PauseMenu.SetActive(false);
        m_PauseButton.SetActive(true);
    }
	
    void OnEnable()
    {
        //SetPause(false);
    }


    public void SetPause(bool On)
    {
        m_Paused = On;
        JoystickScript.Paused = On;
        m_PauseMenu.SetActive(On);
        m_PauseButton.SetActive(!On);
        if (MainARHandler.Instance.CurrentGameScene == GameScenes.MinigameCubeRunner)
		{
			UiPages.GetPage(Pages.CubeMinigamePage).GetComponent<CubeMinigamesPageControls>().Paused = On;
            UIGlobalVariablesScript.Singleton.CubeRunnerMinigameSceneRef.GetComponent<MinigameCollectorScript>().Paused = On;
        } else if (MainARHandler.Instance.CurrentGameScene == GameScenes.MinigameCannon)
		{
			UiPages.GetPage(Pages.GunMinigamePage).GetComponent<GunMinigamePageControls>().Paused = On;
            UIGlobalVariablesScript.Singleton.GunGameScene.GetComponent<GunsMinigameScript>().Paused = On;
        }
    }

    public void ExitGame()
    {
        //MinigameScript.ExitMinigame(false);
        SetPause(false);
        if (MainARHandler.Instance.CurrentGameScene == GameScenes.MinigameCubeRunner)
        {
            UIGlobalVariablesScript.Singleton.CubeRunnerMinigameSceneRef.GetComponent<MinigameCollectorScript>().ExitMinigame(false);
        } else if (MainARHandler.Instance.CurrentGameScene == GameScenes.MinigameCannon)
        {
            UIGlobalVariablesScript.Singleton.GunGameScene.GetComponent<GunsMinigameScript>().ExitMinigame();
        }
    }
	public void ResetInGameTutorials(){
		if (MainARHandler.Instance.CurrentGameScene == GameScenes.MinigameCubeRunner)
		{
			UiPages.GetPage(Pages.CubeMinigamePage).GetComponent<CubeMinigamesPageControls>().ResetTutorial();
		} else if (MainARHandler.Instance.CurrentGameScene == GameScenes.MinigameCannon)
		{
			UiPages.GetPage(Pages.GunMinigamePage).GetComponent<GunMinigamePageControls>().ResetTutorial();
		}
	}

    public void ToggleSound()
    {
        ProfilesManagementScript.Singleton.CurrentProfile.Settings.AudioEnabled = !ProfilesManagementScript.Singleton.CurrentProfile.Settings.AudioEnabled;
    }
}
