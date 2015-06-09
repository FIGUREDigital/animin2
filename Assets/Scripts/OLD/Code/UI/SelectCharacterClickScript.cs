using UnityEngine;
using System.Collections;

public class SelectCharacterClickScript : MonoBehaviour 
{
    public PersistentData.TypesOfAnimin Animin;
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
        ProfilesManagementScript.Instance.CurrentProfile.ActiveAnimin = Animin;
        ProfilesManagementScript.Instance.AssignCurrentAniminToVariable();
		ProfilesManagementScript.Instance.BeginLoadLevel = true;
		UiPages.Next (Pages.LevelLoadingPage);

	}
}
