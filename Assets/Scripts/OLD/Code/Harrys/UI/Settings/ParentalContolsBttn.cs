using UnityEngine;
using System.Collections;

public class ParentalContolsBttn : MonoBehaviour {
	
	[SerializeField]
	private GameObject SettingsMenu;
	[SerializeField]
	private ParentalControls ParentalControls;
	
	void OnClick(){
		UIGlobalVariablesScript.Singleton.OpenParentalGateway (SettingsMenu, ParentalControls.gameObject);
	}
}
