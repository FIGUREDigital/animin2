using UnityEngine;
using System.Collections;

public class ResetMinigameTutorials : MonoBehaviour {

    void OnClick(){
        if (UIGlobalVariablesScript.Singleton.CubeRunnerMinigameSceneRef != null)
        {
            MinigameCollectorScript script = UIGlobalVariablesScript.Singleton.CubeRunnerMinigameSceneRef.GetComponent<MinigameCollectorScript>();

            ProfilesManagementScript.Singleton.CurrentProfile.TutorialBoxLandPlayed = false;
            script.ResetTutorial();
            script.AdvanceTutorial();
        }
        else if (UIGlobalVariablesScript.Singleton.GunGameScene != null)
        {
            GunsMinigameScript script = UIGlobalVariablesScript.Singleton.GunGameScene.GetComponent<GunsMinigameScript>();

            ProfilesManagementScript.Singleton.CurrentProfile.TutorialCanonClashPlayed = false;
            script.ResetTutorial();
            script.AdvanceTutorial();
        }
        UIClickButtonMasterScript.HandleClick(UIFunctionalityId.ResumeGame,this.gameObject);
    }
}
