using UnityEngine;
using System.Collections;

public class BackToProfileSelect : MonoBehaviour {

	void OnClick()
	{
        ProfilesManagementScript.Singleton.NewUser.SetActive(false);
        ProfilesManagementScript.Singleton.LoginUser.SetActive(false);
		ProfilesManagementScript.Singleton.SelectProfile.SetActive(true);
	}
}
