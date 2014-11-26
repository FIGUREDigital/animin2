using UnityEngine;
using System.Collections;

public class BackToProfilesClickScript : MonoBehaviour 
{
	public GameObject ProfilesScreen;
	public GameObject AniminScreen;

	void OnClick()
	{
//		if (ProfilesManagementScript.Singleton.LoadingScreen.activeInHierarchy) 
//		{

			if (ProfilesManagementScript.Singleton.SelectProfile.activeInHierarchy) 
			{
				AniminScreen.SetActive (true);
			ProfilesManagementScript.Singleton.SelectProfile.SetActive (false);
			}
			else 
			{
				AniminScreen.SetActive (false);
			ProfilesManagementScript.Singleton.SelectProfile.SetActive (true);
			}
//		}
	}
}
