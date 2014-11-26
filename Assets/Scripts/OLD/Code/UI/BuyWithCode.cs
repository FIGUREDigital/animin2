using UnityEngine;
using System.Collections;

public class BuyWithCode : MonoBehaviour 
{
	void OnClick()
	{
		Debug.Log("Buying with code \n Opening Code Input");
		ProfilesManagementScript.Singleton.PurchaseChoiceScreen.SetActive(false);
		ProfilesManagementScript.Singleton.CreateAccessCodeScreen.SetActive(true);
	}
}
