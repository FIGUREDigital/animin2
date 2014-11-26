using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnterParentalPasswordButton : MonoBehaviour {

	InputField mInput;

	[SerializeField]
	private Text ErrorBox;

	void OnClick(){
		mInput = gameObject.transform.parent.GetComponentInChildren<InputField>();
		if (mInput.text.Trim() == PlayerPrefs.GetString ("ParentalPassword")) {
			GetComponentInParent<ParentalGateway> ().Pass();
		} else {
			GetComponentInParent<ParentalGateway> ().Fail ();
		}
	}
}
