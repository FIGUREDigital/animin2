using UnityEngine;
using System.Collections;

public class RestorePurchases : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void OnEnable()
	{
#if UNITY_ANDROID
		gameObject.SetActive(false);
#endif
	}

	void OnClick()
	{
		Debug.Log("Restore Purchases");
		if(!Application.isEditor)
		{
			ProfilesManagementScript.Singleton.AniminsScreen.SetActive(false);
			ProfilesManagementScript.Singleton.LoadingSpinner.SetActive(true);
			
			if(ShopManager.Instance.HasBought(UnlockCharacterManager.TBOADULT_PURCHASE) || ShopManager.Instance.HasBought(UnlockCharacterManager.TBOADULT_UNLOCK))
			{
				UnlockCharacterManager.Instance.ID = PersistentData.TypesOfAnimin.TboAdult;
				UnlockCharacterManager.Instance.UnlockCharacter();
			}
			if(ShopManager.Instance.HasBought(UnlockCharacterManager.PI_PURCHASE) || ShopManager.Instance.HasBought(UnlockCharacterManager.PI_UNLOCK))
			{
				UnlockCharacterManager.Instance.ID = PersistentData.TypesOfAnimin.Pi;
				UnlockCharacterManager.Instance.UnlockCharacter();
			}
			if(ShopManager.Instance.HasBought(UnlockCharacterManager.KELSEY_PURCHASE) || ShopManager.Instance.HasBought(UnlockCharacterManager.KELSEY_UNLOCK))
			{
				UnlockCharacterManager.Instance.ID = PersistentData.TypesOfAnimin.Kelsey;
				UnlockCharacterManager.Instance.UnlockCharacter();
			}
			if(ShopManager.Instance.HasBought(UnlockCharacterManager.MANDI_PURCHASE) || ShopManager.Instance.HasBought(UnlockCharacterManager.MANDI_UNLOCK))
			{
				UnlockCharacterManager.Instance.ID = PersistentData.TypesOfAnimin.Mandi;
				UnlockCharacterManager.Instance.UnlockCharacter();
			}
			ShopManager.Instance.RestoreItems();
			//Invoke("Return",3);
		}
	}

	void Return()
	{
		ProfilesManagementScript.Singleton.AniminsScreen.SetActive(true);
		ProfilesManagementScript.Singleton.LoadingSpinner.SetActive(false);
	}
}
