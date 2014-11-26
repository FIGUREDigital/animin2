using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ProfileChecker : MonoBehaviour 
{
	//script no longer in use with new profile system

    GameObject Login;
	GameObject User;
	GameObject NewUser;

	bool initialStartup;
	void Start()
	{
		Login = transform.FindChild("Login").gameObject;
		User = transform.FindChild("User").gameObject;
		NewUser = transform.FindChild("NewUser").gameObject;

		initialStartup = PlayerPrefs.GetString("First Login") != "true";
//		SaveAndLoad.Instance.LoadAllData ();
		if(initialStartup)
		{
			PlayerPrefs.SetString("First Login", "true");
			User.SetActive(false);
		}
		else
		{
            User.SetActive(true);
            if (ProfilesManagementScript.Singleton.ListOfPlayerProfiles.Count > 0)
            {
                User.GetComponentInChildren<Text>().text = ProfilesManagementScript.Singleton.ListOfPlayerProfiles[0].ProfileName;				
            }
            else
            {
                User.SetActive(false);
            }
		}
	}
}
