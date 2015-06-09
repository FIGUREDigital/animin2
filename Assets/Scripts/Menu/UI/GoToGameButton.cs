using UnityEngine;
using System.Collections;

public class GoToGameButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void OnClick(int AniminID){
        //Application.LoadLevel ("ARBase");
		ProfilesManagementScript.Instance.CurrentProfile.ActiveAnimin = (PersistentData.TypesOfAnimin) AniminID;
        ProfilesManagementScript.Instance.AssignCurrentAniminToVariable();
        ProfilesManagementScript.Instance.BeginLoadLevel = true;
	}
}
