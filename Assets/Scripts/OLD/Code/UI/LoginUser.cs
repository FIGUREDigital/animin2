using UnityEngine;
using System.Collections;

public class LoginUser : MonoBehaviour 
{
    public PlayerProfileData ThisProfile;

    public void OnClick()
	{
        ProfilesManagementScript.Singleton.LoginExistingUser(ThisProfile);

	}
}
