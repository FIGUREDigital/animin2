using UnityEngine;
using System.Collections;

public class CancelCreateAccessCodeScript : MonoBehaviour 
{
	void OnClick()
	{
		ProfilesManagementScript.Singleton.CreateAccessCodeScreen.SetActive(false);
		ProfilesManagementScript.Singleton.PurchaseChoiceScreen.SetActive(true);
	}
}
