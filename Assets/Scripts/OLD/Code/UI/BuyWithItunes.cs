using UnityEngine;
using System.Collections;

public class BuyWithItunes : MonoBehaviour 
{
	public void OnClick()
	{
		if(Application.isEditor)
		{						
			UnlockCharacterManager.Instance.ID = ProfilesManagementScript.Instance.AniminToUnlockId;
			UnlockCharacterManager.Instance.UnlockCharacter();
			return;
		}
		Debug.Log("Buying with itunes \n Opening IAP");
		UnlockCharacterManager.Instance.BuyCharacter(ProfilesManagementScript.Instance.AniminToUnlockId, false);

	}
}
