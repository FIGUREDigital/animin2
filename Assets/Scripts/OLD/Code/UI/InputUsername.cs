using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InputUsername : MonoBehaviour {

	InputField mInput;
	void OnClick()
	{
		mInput = gameObject.transform.parent.GetComponentInChildren<InputField>();
		string name = mInput.text.Trim();
		if(name == "" || name == " " || name == null)
		{
			return;
		}
		Account.Instance.UserName = name;
	    StartCoroutine( Account.Instance.WWWSendData( true, name, "","","", "","" ) );

	}
}
