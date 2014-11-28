using UnityEngine;
using System.Collections;

public class EnterProfile : MonoBehaviour {

	public bool IsNew = true;
	private PlayerProfileData mThisProfile;

	public PlayerProfileData ThisProfile 
	{
		get 
		{
			return mThisProfile;
		}
		set
		{
			IsNew = false;
			mThisProfile = value;
		}
	}

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnClick()
	{
		if(IsNew)
		{
			UiPages.Next(Pages.NewProfilePage);
		}
		else
		{
			ProfilesManagementScript.Singleton.LoginExistingUser(ThisProfile);
			UiPages.Next(Pages.AniminSelectPage);
		}
	}
}
