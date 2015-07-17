#define EVERYPLAY
using UnityEngine;
using System.Collections;
using DG.Tweening;
public class CamCorderUI : MonoBehaviour {

	public GameObject recordingMark;
	public void OnTap()
	{
#if KAMCORD
		Debug.Log ("Kamcord IsEnabled() = " + Kamcord.IsEnabled ());
		Debug.Log ("Kamcord IsRecording() = " + Kamcord.IsRecording ());
#endif
		if (recordingMark.activeSelf) {
#if KAMCORD
			Kamcord.StopRecording ();
			Kamcord.SetVideoTitle(ProfilesManagementScript.Instance.CurrentAnimin.AniminName+" Age "+ProfilesManagementScript.Instance.CurrentAnimin.Age);
			Kamcord.ShowView ();
#endif
#if EVERYPLAY
			Everyplay.StopRecording();
			Everyplay.SetMetadata("Animin",ProfilesManagementScript.Instance.CurrentAnimin.AniminName);
			Everyplay.SetMetadata("Age",ProfilesManagementScript.Instance.CurrentAnimin.Age);
			Everyplay.ShowSharingModal();
#endif
			recordingMark.SetActive (false);

		} else {
			bool recording = false;
#if EVERYPLAY
			if(Everyplay.IsRecordingSupported())
			{
				Everyplay.StartRecording();
				recording = true;
			}
#endif

#if KAMCORD
			Debug.Log ("KamCord.StartRecording()");
			Kamcord.StartRecording();
			Debug.Log ("Kamcord IsRecording() = " + Kamcord.IsRecording ());
			recording = Kamcord.IsRecording();
#endif
			if(recording)
			{
				recordingMark.SetActive (true);			
            }
        }
		CharacterProgressScript script = UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>();
		script.HidePopupMenus(false);

		//UiPages.GetPage(Pages.CaringPage).GetComponent<CaringPageControls>().TutorialHandler.TriggerAdHoc("Fart");
		//UIGlobalVariablesScript.Singleton.SoundEngine.PlayFart();
	}
}
