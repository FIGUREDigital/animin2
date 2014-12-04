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
		//ProfilesManagementScript.Singleton.CurrentAnimin.SetDefault();
		//ProfilesManagementScript.Singleton.CurrentAnimin.PlayerAniminId = Animin;
		//ProfilesManagementScript.Singleton.CurrentAnimin.AniminEvolutionId = AniminEvolutionStageId.Baby;
		//ProfilesManagementScript.Singleton.CurrentAnimin.Save();

        ProfilesManagementScript.Singleton.CurrentProfile.ActiveAnimin = Animin;

        ProfilesManagementScript.Singleton.AssignCurrentAniminToVariable();
//        ProfilesManagementScript.Singleton.CurrentAnimin = ProfilesManagementScript.Singleton.CurrentProfile.Characters[(int)Animin];

		//AsyncOperation asyncOp = Application.LoadLevelAsync("VuforiaTest");
		//yield return asyncOp;
		//Debug.Log("Loading complete");
	}
}
