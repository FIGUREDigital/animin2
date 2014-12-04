using UnityEngine;
using System.Collections;

public class SettingsPageControls : MonoBehaviour {

	public void MainMenuButton()
	{
		Application.LoadLevel (Application.loadedLevel - 1);
	}

	public void PrivacyPolicyButton()
	{
		UiPages.Next (Pages.PrivacyPolicyPage);
	}

	public void CreditsButton()
	{
		UiPages.Next (Pages.CreditsPage);
	}

	public void CloseButton()
	{
		UiPages.Close ();
	}

	public void MuteButton()
	{
		AudioListener audio = FindObjectOfType<AudioListener> ();
		if(audio != null)
		{
			audio.enabled = !audio.enabled;
		}
	}
}
