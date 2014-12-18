using UnityEngine;
using System.Collections;

public class SelectCharacterClickScript : MonoBehaviour 
{
    public PersistentData.TypesOfAnimin Animin;
	private bool BeginLoadLevel;
	public GameObject UnlockButton;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{

	
	}

	public void OnClick()
	{
        ProfilesManagementScript.Singleton.CurrentProfile.ActiveAnimin = Animin;
        ProfilesManagementScript.Singleton.AssignCurrentAniminToVariable();
		ProfilesManagementScript.Singleton.BeginLoadLevel = true;
		UiPages.Next (Pages.LevelLoadingPage);

	}
}
