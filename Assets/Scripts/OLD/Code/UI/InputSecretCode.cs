using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InputSecretCode : MonoBehaviour 
{
	InputField mInput;

	void OnClick()
	{

		{
						mInput = gameObject.transform.parent.GetComponentInChildren<InputField> ();
						string accessCode = mInput.text.Trim();

						ProfilesManagementScript.Instance.CheckProfileLoginPasscode (accessCode);
				}

	}
}
