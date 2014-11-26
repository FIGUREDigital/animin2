using UnityEngine;
using System.Collections;

public class CloseParentalControls : MonoBehaviour {

	[SerializeField]
	ParentalControls controls;
	// Update is called once per frame
	void OnClick () {
		controls.gameObject.SetActive (false);

		UIGlobalVariablesScript.Singleton.CaringScreenRef.SetActive (true);
	}
}
