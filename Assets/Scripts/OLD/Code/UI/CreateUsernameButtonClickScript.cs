using UnityEngine;
using System.Collections;

public class CreateUsernameButtonClickScript : MonoBehaviour 
{
	public CreateUsernameFromTextboxClickScript SubmitFunction;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void OnClick()
	{
		SubmitFunction.OnSubmit();
	}
}
