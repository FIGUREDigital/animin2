using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItunesButtonUpdate : MonoBehaviour 
{
	[SerializeField]
	private GameObject mPiHeader;
	[SerializeField]
	private GameObject mTboHeader;
	[SerializeField]
	private GameObject mKelseyHeader;
	[SerializeField]
	private GameObject mMandiHeader;

	private bool mRegistered;

	void Start()
	{
	}

	void OnEnable()
	{
		RegisterListeners();
        SetCharacterIcons(ProfilesManagementScript.Singleton.AniminToUnlockId);
	}
	
	void OnDisable()
	{
		//UnregisterListeners();
	}

	public void GoToCodeScreen()
	{
		UiPages.Next (Pages.CodeInputPage);
	}

    public void SetCharacterIcons(PersistentData.TypesOfAnimin typeToPurchase)
	{
		mPiHeader.SetActive(false);
		mTboHeader.SetActive(false);
		mKelseyHeader.SetActive(false);
		mMandiHeader.SetActive(false);		

        switch(typeToPurchase)
		{
            case PersistentData.TypesOfAnimin.Pi:
			mPiHeader.SetActive(true);
			break;
            case PersistentData.TypesOfAnimin.TboAdult:
			mTboHeader.SetActive(true);
			break;
            case PersistentData.TypesOfAnimin.Kelsey:
			mKelseyHeader.SetActive(true);
			break;
            case PersistentData.TypesOfAnimin.Mandi:
			mMandiHeader.SetActive(true);
			break;
		default:
			break;

		}
		
	}


	void ReturnToMainScreen()
	{
		UnregisterListeners();
		UiPages.Back ();
	}

	void GoToAddress()
	{
		UnregisterListeners();
		UiPages.Next (Pages.AddressInputPage);
	}
	void restoreTransactionsFailed( string error )
	{
		UiPages.Next (Pages.AniminSelectPage);		
	}
	
	
	void restoreTransactionsFinished()
	{
		UiPages.Next (Pages.AniminSelectPage);
	}

#if UNITY_IOS
	void purchaseSuccessful( StoreKitTransaction transaction )
	{
		Debug.Log(string.Format("Purchase of {0} Successful",transaction.productIdentifier));
		UnlockCharacterManager.Instance.UnlockCharacter();
		GoToAddress();
	}

	void purchaseCancelled( string response )
	{
		Debug.Log("Purchase Cancelled. Response "+ response);
		ShopManager.Instance.EndStore();
		ReturnToMainScreen();

	}


#elif UNITY_ANDROID
    void purchaseSuccessful( GooglePurchase transaction)
	{
		Debug.Log(string.Format("Purchase of {0} Successful",transaction.productId));
		UnlockCharacterManager.Instance.UnlockCharacter();
		GoToAddress();
	}
	void purchaseCancelled( string response )
	{
		Debug.Log("Purchase Cancelled. Response "+ response);
		ShopManager.Instance.EndStore();
		ReturnToMainScreen ();
	}
#endif

		#if UNITY_IOS
		void purchaseUnsuccessful( string response )
		#elif UNITY_ANDROID
		void purchaseUnsuccessful( string response, int errorcode )
		#endif
	{
		Debug.Log("Purchase Unsuccessful, response: " + response);
		ShopManager.Instance.EndStore();
		ReturnToMainScreen();
	}

	void RegisterListeners()
	{
		if(mRegistered)
		{
			return;
		}
		mRegistered = true;
		Debug.Log("Register Itunes Listeners from itunes button script");
		if(Application.isEditor){ return; }
        
#if UNITY_IOS
		StoreKitManager.purchaseSuccessfulEvent += purchaseSuccessful;
		StoreKitManager.purchaseCancelledEvent += purchaseCancelled;
		StoreKitManager.purchaseFailedEvent += purchaseUnsuccessful;
		StoreKitManager.restoreTransactionsFinishedEvent += restoreTransactionsFinished;
		StoreKitManager.restoreTransactionsFailedEvent += restoreTransactionsFailed;
#elif UNITY_ANDROID
		GoogleIABManager.purchaseSucceededEvent += purchaseSuccessful;
		GoogleIABManager.purchaseFailedEvent += purchaseUnsuccessful;
#endif
    }
	void UnregisterListeners()
	{
		if(!mRegistered)
		{
			return;
		}
		mRegistered = false;
		Debug.Log("Unregister Itunes Listeners");
		if(Application.isEditor){ return; }
#if UNITY_IOS
		StoreKitManager.purchaseSuccessfulEvent -= purchaseSuccessful;
		StoreKitManager.purchaseCancelledEvent -= purchaseCancelled;
		StoreKitManager.purchaseFailedEvent -= purchaseUnsuccessful;
        StoreKitManager.restoreTransactionsFinishedEvent -= restoreTransactionsFinished;
        StoreKitManager.restoreTransactionsFailedEvent -= restoreTransactionsFailed;
#elif UNITY_ANDROID
        GoogleIABManager.purchaseSucceededEvent -= purchaseSuccessful;
		GoogleIABManager.purchaseFailedEvent -= purchaseUnsuccessful;
#endif
    }
}
