using UnityEngine;
using System.Collections;

public class IngamePauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject m_PauseMenu, m_PauseButton;

    // Use this for initialization
    void Awake()
    {
        m_PauseMenu.SetActive(false);
        m_PauseButton.SetActive(true);
    }

    public void SetPause(bool On)
    {
		JoystickPageControls.Paused = On;
		m_PauseMenu.SetActive (On);
		m_PauseButton.SetActive (!On);
        if (MainARHandler.Instance.CurrentGameScene == GameScenes.MinigameCubeRunner)
		{
			//UiPages.GetPage(Pages.CubeMinigamePage).GetComponent<CubeMinigamesPageControls>().Paused = On;
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
            UIGlobalVariablesScript.Singleton.CubeRunnerMinigameSceneRef.GetComponent<MinigameCollectorScript>().EndMinigame(true);
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
        SetPause(false);
	}

    public void ToggleSound()
    {
        ProfilesManagementScript.Instance.CurrentProfile.Settings.AudioEnabled = !ProfilesManagementScript.Instance.CurrentProfile.Settings.AudioEnabled;
    }
}
