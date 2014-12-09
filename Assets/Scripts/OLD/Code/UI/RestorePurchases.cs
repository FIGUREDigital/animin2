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

	public void OnClick()
	{
		Debug.Log("Restore Purchases");
		if(!Application.isEditor)
		{
			
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
			UiPages.Next(Pages.AniminSelectPage);
			//Invoke("Return",10);
		}
	}

	void Return()
	{
		UiPages.Back ();
	}
}
