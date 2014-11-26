using UnityEngine;
using System.Collections;

public class PurchaseBackButton : MonoBehaviour 
{
	void OnClick()
	{
		ProfilesManagementScript.Singleton.PurchaseChoiceScreen.SetActive(false);
        ProfilesManagementScript.Singleton.AniminsScreen.SetActive(true);
		
	}
}
