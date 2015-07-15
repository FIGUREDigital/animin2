using UnityEngine;
using System.Collections;
using DG.Tweening;

public class CamCorderUI : MonoBehaviour {

	public GameObject recordingMark;
	public void OnTap()
	{
		Debug.Log ("Kamcord IsEnabled() = " + Kamcord.IsEnabled ());
		Debug.Log ("Kamcord IsRecording() = " + Kamcord.IsRecording ());
		if (recordingMark.activeSelf) {
			Kamcord.StopRecording ();
			recordingMark.SetActive (false);
			Kamcord.SetVideoTitle(ProfilesManagementScript.Instance.CurrentAnimin.AniminName+" Age "+ProfilesManagementScript.Instance.CurrentAnimin.Age);
			Kamcord.ShowView ();
		} else {
			recordingMark.SetActive (true);
			Debug.Log ("KamCord.StartRecording()");
			Kamcord.StartRecording();
			Debug.Log ("Kamcord IsRecording() = " + Kamcord.IsRecording ());
		}
		CharacterProgressScript script = UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>();
		script.HidePopupMenus(false);

		//UiPages.GetPage(Pages.CaringPage).GetComponent<CaringPageControls>().TutorialHandler.TriggerAdHoc("Fart");
		//UIGlobalVariablesScript.Singleton.SoundEngine.PlayFart();
	}
}
