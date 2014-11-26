using UnityEngine;
using System.Collections;

public class RetrieveUser : MonoBehaviour {

	void OnClick()
	{
		ProfilesManagementScript.Singleton.SelectProfile.SetActive(false);
		ProfilesManagementScript.Singleton.LoginUser.SetActive(true);
	}
}
