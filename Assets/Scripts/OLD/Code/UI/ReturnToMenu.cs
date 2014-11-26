using UnityEngine;
using System.Collections;

public class ReturnToMenu : MonoBehaviour {

	void OnClick()
	{
		ProfilesManagementScript.Singleton.ErrorBox.SetActive(false);
		ProfilesManagementScript.Singleton.AniminsScreen.SetActive(true);
	}
}
