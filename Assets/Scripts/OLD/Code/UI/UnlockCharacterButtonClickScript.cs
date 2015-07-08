using UnityEngine;
using System.Collections;

public class UnlockCharacterButtonClickScript : MonoBehaviour 
{

	private const string PI_UNLOCK = "com.apples.animin.characterunlock1";
	private const string KELSEY_UNLOCK = "com.apples.animin.characterunlock2";
	private const string MANDI_UNLOCK = "com.apples.animin.characterunlock3";
	private const string PI_PURCHASE = "com.apples.animin.characterpurchase1";
	private const string KELSEY_PURCHASE = "com.apples.animin.characterpurchase2";
	private const string MANDI_PURCHASE = "com.apples.animin.characterpurchase3";
    public PersistentData.TypesOfAnimin Id;

	public void OnClick()
	{
		ProfilesManagementScript.Instance.AniminToUnlockId = Id;
		Debug.Log ("On Click " + Id);
        
//		UIManager.Instance.UITransition(ProfilesManagementScript.Singleton.AniminsScreen, ProfilesManagementScript.Singleton.PurchaseChoiceScreen);

        if(! Application.isEditor)
        {
            ProfilesManagementScript.Instance.ActivateShopItemCheck();
			UiPages.Next(Pages.LoadingPage);
        }
		else
		{
			UiPages.Next(Pages.PurchasePage);
		}
		
	}

}
