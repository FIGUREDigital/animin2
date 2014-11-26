using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InputSecretCode : MonoBehaviour 
{
	InputField mInput;

	void OnClick()
	{
		if (!ProfilesManagementScript.Singleton.LoginCheckingDialogue.activeInHierarchy) 
		{
						mInput = gameObject.transform.parent.GetComponentInChildren<InputField> ();
						string accessCode = mInput.text.Trim();

						ProfilesManagementScript.Singleton.CheckProfileLoginPasscode (accessCode);
						ProfilesManagementScript.Singleton.LoginCheckingDialogue.SetActive (true);
						ProfilesManagementScript.Singleton.NoSuchUserCodeDialogue.SetActive(false);
				}

	}
}
