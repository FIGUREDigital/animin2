using UnityEngine;
using System.Collections;

public class PrivacyPolicyBttn : MonoBehaviour {

	[SerializeField]
	private GameObject SettingsMenu;

	void OnClick(){
		GameObject go = (GameObject)Instantiate(Resources.Load("Prefabs/PrivacyPolicyUI"));
		go.GetComponent<PrivacyPolicy>().Open(SettingsMenu);
	}
}
