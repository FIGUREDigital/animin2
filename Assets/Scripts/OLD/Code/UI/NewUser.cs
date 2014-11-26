using UnityEngine;
using System.Collections;

public class NewUser : MonoBehaviour 
{
	void OnClick()
	{
		ProfilesManagementScript.Singleton.SelectProfile.SetActive(false);
		ProfilesManagementScript.Singleton.NewUser.SetActive(true);
	}
}
