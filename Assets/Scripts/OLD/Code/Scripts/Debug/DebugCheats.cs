using UnityEngine;
using System.Collections;

public enum CheatButtons
{
    NotSet,
    Birthday,
    Tutorial,
    Evolution,
    EvolutionMarker1,
    EvolutionMarker2,
    Evolve,
    Devolve,
    GiveZeff,
    TakeZeff,
    DebugToggle,
    AddTime,
    ResetTutorials,
    ResetParentalPassword,
    Starve,
    MediaDebug,
}

public class DebugCheats : MonoBehaviour
{
    public CheatButtons button = CheatButtons.NotSet;

    public void OnClick()
    {
#if DEBUGCHEATS
        switch (button)
        {

            case CheatButtons.Birthday:
                AchievementsScript.Singleton.Show(AchievementTypeId.Birthday, 0);
                break;

            case CheatButtons.Tutorial:
                AchievementsScript.Singleton.Show(AchievementTypeId.Tutorial, 0);
                break;

            case CheatButtons.Evolution:
                AchievementsScript.Singleton.Show(AchievementTypeId.Evolution, 0);
                break;

            case CheatButtons.EvolutionMarker1:
                AchievementsScript.Singleton.Show(AchievementTypeId.EvolutionExclamation, 0);
                break;

            case CheatButtons.EvolutionMarker2:
                AchievementsScript.Singleton.Show(AchievementTypeId.EvolutionStar, 0);
                break;

            case CheatButtons.Evolve:
                switch (ProfilesManagementScript.Instance.CurrentAnimin.AniminEvolutionId)
                {
                    case AniminEvolutionStageId.Baby:
				ProfilesManagementScript.Instance.CurrentAnimin.ZefTokens = EvolutionManager.Instance.BabyEvolutionThreshold - 1;
                        EvolutionManager.Instance.RemoveZef(0);
                        break;
                    case AniminEvolutionStageId.Kid:
				ProfilesManagementScript.Instance.CurrentAnimin.ZefTokens = EvolutionManager.Instance.KidEvolutionThreshold - 1;
                        EvolutionManager.Instance.RemoveZef(0);
                        break;
                    case AniminEvolutionStageId.Adult:
                    default:
                        break;
                }
                break;

            case CheatButtons.Devolve:
			switch (ProfilesManagementScript.Instance.CurrentAnimin.AniminEvolutionId)
                {
                    case AniminEvolutionStageId.Baby:
                    default:
                        break;
                    case AniminEvolutionStageId.Kid:
				ProfilesManagementScript.Instance.CurrentAnimin.ZefTokens = EvolutionManager.Instance.BabyEvolutionThreshold - 50;
                        EvolutionManager.Instance.RemoveZef(0);
                        break;
                    case AniminEvolutionStageId.Adult:
				ProfilesManagementScript.Instance.CurrentAnimin.ZefTokens = EvolutionManager.Instance.KidEvolutionThreshold - 50;
                        EvolutionManager.Instance.RemoveZef(0);
                        break;
                }
                break;

            case CheatButtons.GiveZeff:
                EvolutionManager.Instance.AddZef(3);
                break;

            case CheatButtons.TakeZeff:
                EvolutionManager.Instance.RemoveZef();
                break;

            case CheatButtons.AddTime:
                EvolutionManager.Instance.HappinessStateTime += 600;
                break;
            case CheatButtons.ResetTutorials:
                GameObject tuthandler = GameObject.Find("TutorialHandler");
                tuthandler.GetComponent<TutorialHandler>().ResetTutorials();
                break;
            case CheatButtons.ResetParentalPassword:
                PlayerPrefs.SetString("ParentalPassword", "");
                break;
            case CheatButtons.Starve:
			ProfilesManagementScript.Instance.CurrentAnimin.Hungry = 0;
                break;
            case CheatButtons.MediaDebug:
                MediaDebugDummy.On = !MediaDebugDummy.On;
                break;

            case CheatButtons.DebugToggle:
                GameObject go = gameObject.transform.parent.FindChild("Toggleable").gameObject;
                go.SetActive(!go.activeSelf);
                break;

            case CheatButtons.NotSet:
            default:
                break;
        }
#endif
    }
}
