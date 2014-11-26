using UnityEngine;
using System.Collections;

public class BuyWithItunes : MonoBehaviour 
{
	void OnClick()
	{
		Debug.Log("Buying with itunes \n Opening IAP");
		UnlockCharacterManager.Instance.BuyCharacter(ProfilesManagementScript.Singleton.AniminToUnlockId, false);
		if(Application.isEditor)
		{
			ProfilesManagementScript.Singleton.PurchaseChoiceScreen.SetActive(false);
			ProfilesManagementScript.Singleton.AniminsScreen.SetActive(true);
			return;
		}
		ProfilesManagementScript.Singleton.LoadingSpinner.SetActive(true);
		ProfilesManagementScript.Singleton.PurchaseChoiceScreen.SetActive(false);
	}
}
