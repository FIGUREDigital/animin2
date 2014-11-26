﻿using UnityEngine;
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
		ProfilesManagementScript.Singleton.LoadingSpinner.SetActive(false);
		ProfilesManagementScript.Singleton.AniminsScreen.SetActive(true);
		UnregisterListeners();
	}

	void GoToAddress()
	{
		ProfilesManagementScript.Singleton.LoadingSpinner.SetActive(false);
        //ProfilesManagementScript.Singleton.AddressInput.SetActive(true);
        ProfilesManagementScript.Singleton.AniminsScreen.SetActive(true);
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
		ReturnToMainScreen();
	}
#endif

    void purchaseUnsuccessful( string response )
	{
		Debug.Log("Purchase Unsuccessful, response: " + response);
		ProfilesManagementScript.Singleton.LoadingSpinner.SetActive(false);
		ProfilesManagementScript.Singleton.AniminsScreen.SetActive(true);
		ShopManager.Instance.EndStore();
		UnregisterListeners();
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
#if UNITY_IOS
		if(Application.isEditor){ return; }
		StoreKitManager.purchaseSuccessfulEvent -= purchaseSuccessful;
		StoreKitManager.purchaseCancelledEvent -= purchaseCancelled;
		StoreKitManager.purchaseFailedEvent -= purchaseUnsuccessful;
#elif UNITY_ANDROID
		if(Application.isEditor){ return; }
        GoogleIABManager.purchaseSucceededEvent -= purchaseSuccessful;
		GoogleIABManager.purchaseFailedEvent -= purchaseUnsuccessful;
#endif
    }
}
