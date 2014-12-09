using UnityEngine;
using System.Collections;

public class AddressInputPageControls : MonoBehaviour {

	[SerializeField]
	private GameObject Address;
	[SerializeField]
	private GameObject Leave;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void GoToLeave()
	{
		Address.SetActive (false);
		Leave.SetActive (true);
	}
	public void GoBackToAddress()
	{
		Address.SetActive (true);
		Leave.SetActive (false);
	}
}
