
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ConfirmParentalPasswordButton : MonoBehaviour {

	InputField[] mInput;
	[SerializeField]
	Text ErrorBox;
	[SerializeField]
	GameObject ParentalControlsScreen;
	void OnClick()
	{
		mInput = gameObject.transform.parent.GetComponentsInChildren<InputField>();
		
		if ( (mInput [0].text.Trim()) != (mInput [1].text.Trim())) {
			if (ErrorBox!=null){
				ErrorBox.gameObject.SetActive(true);
				ErrorBox.text = "Passwords do not match!";
			}
		} else if(mInput [0].text.Trim() == "" && mInput [1].text.Trim() =="") {
			if (ErrorBox!=null){
				ErrorBox.gameObject.SetActive(true);
				ErrorBox.text = "Please enter a password";
			}
		} else {
			Debug.Log ("Passwords Match!");
			string password = mInput [0].text.Trim();
			PlayerPrefs.SetString ("ParentalPassword", password);
			Account.Instance.UserName = password;
			if (ErrorBox!=null)ErrorBox.gameObject.SetActive(false);
			if (ParentalControlsScreen!=null) ParentalControlsScreen.SetActive(true);
			this.transform.parent.gameObject.SetActive(false);
		}
	}
}
