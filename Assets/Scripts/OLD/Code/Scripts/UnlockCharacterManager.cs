using UnityEngine;
using System.Collections;

public class UnlockCharacterManager
{
#if UNITY_IOS
	public const string PI_UNLOCK = "com.apples.animin.characterunlock1";
	public const string KELSEY_UNLOCK = "com.apples.animin.characterunlock2";
	public const string MANDI_UNLOCK = "com.apples.animin.characterunlock3";
	public const string TBOADULT_UNLOCK = "com.apples.animin.characterunlock4";
	public const string PI_PURCHASE = "com.apples.animin.characterpurchase1";
	public const string KELSEY_PURCHASE = "com.apples.animin.characterpurchase2";
	public const string MANDI_PURCHASE = "com.apples.animin.characterpurchase3";
    public const string TBOADULT_PURCHASE = "com.apples.animin.characterpurchase4";
#elif UNITY_ANDROID
	public const string PI_UNLOCK = "com.android.animin.characterunlock1";
	public const string KELSEY_UNLOCK = "com.android.animin.characterunlock2";
	public const string MANDI_UNLOCK = "com.android.animin.characterunlock3";
	public const string TBOADULT_UNLOCK = "com.android.animin.characterunlock4";
	public const string PI_PURCHASE = "com.android.animin.characterpurchase1";
	public const string KELSEY_PURCHASE = "com.android.animin.characterpurchase2";
	public const string MANDI_PURCHASE = "com.android.animin.characterpurchase3";
	public const string TBOADULT_PURCHASE = "com.android.animin.characterpurchase4";
#else
	public const string PI_UNLOCK = "com.android.animin.characterunlock1";
	public const string KELSEY_UNLOCK = "com.android.animin.characterunlock2";
	public const string MANDI_UNLOCK = "com.android.animin.characterunlock3";
	public const string TBOADULT_UNLOCK = "com.android.animin.characterunlock4";
	public const string PI_PURCHASE = "com.android.animin.characterpurchase1";
	public const string KELSEY_PURCHASE = "com.android.animin.characterpurchase2";
	public const string MANDI_PURCHASE = "com.android.animin.characterpurchase3";
	public const string TBOADULT_PURCHASE = "com.android.animin.characterpurchase4";
#endif

	private static string mBuyItem;
    private static PersistentData.TypesOfAnimin m_CurrentCharacterFocus;
	public PersistentData.TypesOfAnimin ID
	{
		get
		{
			return m_CurrentCharacterFocus;
		}
		set
		{
			m_CurrentCharacterFocus = value;
		}
	}


	#region Singleton
	
	private static UnlockCharacterManager s_Instance;
	
	public static UnlockCharacterManager Instance
	{
		get
		{
			if ( s_Instance == null )
			{
				s_Instance = new UnlockCharacterManager();
			}
			return s_Instance;
		}
	}
	
	#endregion

    public void BuyCharacter(PersistentData.TypesOfAnimin type, bool free)
	{
        m_CurrentCharacterFocus = type;
        switch(m_CurrentCharacterFocus)
		{
            case PersistentData.TypesOfAnimin.Pi:
			mBuyItem = free ? PI_UNLOCK : PI_PURCHASE;
			break;
            case PersistentData.TypesOfAnimin.TboAdult:
			mBuyItem = free ? TBOADULT_UNLOCK : TBOADULT_PURCHASE;
			break;
            case PersistentData.TypesOfAnimin.Kelsey:
			mBuyItem = free ? KELSEY_UNLOCK : KELSEY_PURCHASE;
			break;
            case PersistentData.TypesOfAnimin.Mandi:
			mBuyItem = free ? MANDI_UNLOCK : MANDI_PURCHASE;
			break;
		default:
			Debug.LogError("No animin ID supplied");
			break;
			
		}
		ShopManager.Instance.BuyItem (mBuyItem);
		UiPages.Next (Pages.LoadingPage);
	}

	public void OpenShop()
	{
		Debug.Log("Opening Shop");
		string[] shopItems = new string[8];
		shopItems [0] = PI_UNLOCK;
		shopItems [1] = KELSEY_UNLOCK;
		shopItems [2] = MANDI_UNLOCK;
		shopItems [3] = TBOADULT_UNLOCK;
		shopItems [4] = PI_PURCHASE;
		shopItems [5] = KELSEY_PURCHASE;
		shopItems [6] = MANDI_PURCHASE;
		shopItems [7] = TBOADULT_PURCHASE;
		ShopManager.Instance.StartStore (shopItems);
	}

	private IEnumerator WaitForResponse()
	{
		Debug.Log ("Start Coroutine!");
		bool complete = false;
		while(!complete)
		{
			Debug.Log(ShopManager.CurrentPurchaseStatus.ToString());
			switch(ShopManager.CurrentPurchaseStatus)
			{
			case ShopManager.PurchaseStatus.Success:
                UnlockCharacter ();
				complete = true;
				break;
			case ShopManager.PurchaseStatus.Fail:
				Debug.Log("Purchase Failed!");
				complete = true;
				break;
			case ShopManager.PurchaseStatus.Cancel:
				Debug.Log("Purchase Failed!");
				complete = true;
				break;
			default:
				yield return new WaitForSeconds(0.2f);
				break;
			}
		}
		if(ShopManager.CurrentPurchaseStatus == ShopManager.PurchaseStatus.Success)
		{
			ShopManager.CurrentPurchaseStatus = ShopManager.PurchaseStatus.Idle;
		}
		Debug.Log ("Finish Coroutine!");
		
		yield return true;
	}   

    public bool CheckCharacterPurchased(PersistentData.TypesOfAnimin Id)
	{
		string s1 = "";
		string s2 = "";
		switch(Id)
		{
            case PersistentData.TypesOfAnimin.Pi:
                s1 = PI_UNLOCK;
                s2 = PI_PURCHASE;
			break;
            case PersistentData.TypesOfAnimin.Kelsey:
                s1 = KELSEY_UNLOCK;
                s2 = KELSEY_PURCHASE;
			break;
            case PersistentData.TypesOfAnimin.Mandi:
                s1 = MANDI_UNLOCK;
                s2 = MANDI_PURCHASE;
			break;
			case PersistentData.TypesOfAnimin.TboAdult:
                s2 = TBOADULT_PURCHASE;
			break;
			
		default:
            case PersistentData.TypesOfAnimin.Tbo:
			break;
		}
		return ShopManager.Instance.HasBought(s1) || ShopManager.Instance.HasBought(s2);
	}

    public void UnlockCharacter()
	{  	

        switch(m_CurrentCharacterFocus)
		{
            case PersistentData.TypesOfAnimin.Pi:
			break;
            case PersistentData.TypesOfAnimin.Kelsey:
			break;
            case PersistentData.TypesOfAnimin.Mandi:
			break;
            case PersistentData.TypesOfAnimin.TboAdult:
            break;
		default:
			break;
		}	

        CharacterChoiceManager.Instance.UnlockCharacterPortrait(m_CurrentCharacterFocus);
        ProfilesManagementScript.Singleton.CurrentProfile.UnlockedAnimins.Add(m_CurrentCharacterFocus);
        SaveAndLoad.Instance.SaveAllData();
		Debug.Log("just saved...unlock");
        ShopManager.Instance.EndStore(); 

       // ProfilesManagementScript.Singleton.SendRealTimeNotification("AniminUnlocked",1);
	}

	public void CheckInitialCharacterUnlock()
	{
        Debug.Log("Unlock started...");
        for (int i = 0; i < ProfilesManagementScript.Singleton.CurrentProfile.UnlockedAnimins.Count; i++)
        {   
            PersistentData.TypesOfAnimin typeToUnlock = ProfilesManagementScript.Singleton.CurrentProfile.UnlockedAnimins[i];
            Debug.Log("Unlock Character " + typeToUnlock);
            CharacterChoiceManager.Instance.UnlockCharacterPortrait(typeToUnlock);
        }

	}

	void OnApplicationPause()
	{
		ShopManager.Instance.EndStore();
		PlayerPrefs.Save();
	}
	void OnApplicationResume()
	{
		OpenShop();
	}
}
