using UnityEngine;
using System.Collections;

public class CancelPurchaseScreen : MonoBehaviour 
{
	void OnClick()
	{
		ProfilesManagementScript.Singleton.PurchaseChoiceScreen.SetActive(false);
		ProfilesManagementScript.Singleton.AniminsScreen.SetActive(true);
	}
}
