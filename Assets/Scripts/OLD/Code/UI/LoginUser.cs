using UnityEngine;
using System.Collections;

public class LoginUser : MonoBehaviour 
{
    public PlayerProfileData ThisProfile;

    void OnClick()
	{
        ProfilesManagementScript.Singleton.LoginExistingUser(ThisProfile);

        ProfilesManagementScript.Singleton.SelectProfile.SetActive(false);
//        ProfilesManagementScript.Singleton.AniminsScreen.SetActive(true);

	}
}
