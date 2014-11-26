using UnityEngine;
using System.Collections;

public class StartCreateProfileButtonScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Onclick()
	{
		ProfilesManagementScript.Singleton.CreateUsernameScreen.SetActive(true);
		ProfilesManagementScript.Singleton.OLD_ProfilesScreen.SetActive(false);

	}
}
