using UnityEngine;
using System.Collections;

public class SelectAniminToPlayClickScript : MonoBehaviour 
{
	public PlayerProfileData ProfileRef;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnClick()
	{

        ProfilesManagementScript.Singleton.CurrentProfile = ProfileRef;

		ProfilesManagementScript.Singleton.PiAge.text = "Age 1";
		ProfilesManagementScript.Singleton.TBOAge.text = "Age 1";
		ProfilesManagementScript.Singleton.MandiAge.text = "Age 1";
		ProfilesManagementScript.Singleton.KelsiAge.text = "Age 1";


	}
}
