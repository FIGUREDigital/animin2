using UnityEngine;
using System.Collections;
using DG.Tweening;

public class FartUI : MonoBehaviour {

	public void OnTap()
	{
		UiPages.GetPage(Pages.CaringPage).GetComponent<CaringPageControls>().TutorialHandler.TriggerAdHoc("Fart");
		UIGlobalVariablesScript.Singleton.SoundEngine.PlayFart();
	}
}
