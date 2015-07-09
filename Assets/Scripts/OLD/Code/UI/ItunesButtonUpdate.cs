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
	[SerializeField]
	private UIText mPriceText;


	private bool mRegistered;

	void Start()
	{
	}

	void OnEnable()
	{
		
		Debug.Log ("OnEnable " + ProfilesManagementScript.Instance.AniminToUnlockId);
		RegisterListeners();
		SetCharacterIcons(ProfilesManagementScript.Instance.AniminToUnlockId);	
		Debug.Log ("OnEnable2 " + ProfilesManagementScript.Instance.AniminToUnlockId);	
		mPriceText.Text = UnlockCharacterManager.Instance.GetPrice(ProfilesManagementScript.Instance.AniminToUnlockId);
	}
	
	void OnDisable()
	{
		//UnregisterListeners();
	}

	public void GoToCodeScreen()
	{
		UnlockCharacterManager.Instance.ID = ProfilesManagementScript.Instance.AniminToUnlockId;
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
			case PersistentData.TypesOfAnimin.Tbo:
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
		Debug.Log ("Going to address screen");
		ReturnToMainScreen ();
		//UiPages.Next (Pages.AddressInputPage);
	}
	void restoreTransactionsFailed( string error )
	{
		//UiPages.Next(Pages.RestoreFailPage);
	}
	
	
	void restoreTransactionsFinished()
	{
		//UiPages.Next(Pages.RestoreSuccessPage);
	}
	
	private void SetRestoreType(string trans)
	{

				Debug.Log ("Setting Restore Type to: " + trans);
				switch (trans) {
				case UnlockCharacterManager.TBOADULT_PURCHASE:
				case UnlockCharacterManager.TBOADULT_UNLOCK:
						UnlockCharacterManager.Instance.ID = PersistentData.TypesOfAnimin.TboAdult;
						break;
				case UnlockCharacterManager.PI_PURCHASE:
				case UnlockCharacterManager.PI_UNLOCK:
						UnlockCharacterManager.Instance.ID = PersistentData.TypesOfAnimin.Pi;
						break;
				case UnlockCharacterManager.KELSEY_PURCHASE:
				case UnlockCharacterManager.KELSEY_UNLOCK:
						UnlockCharacterManager.Instance.ID = PersistentData.TypesOfAnimin.Kelsey;
						break;
				case UnlockCharacterManager.MANDI_PURCHASE:
				case UnlockCharacterManager.MANDI_UNLOCK:
						UnlockCharacterManager.Instance.ID = PersistentData.TypesOfAnimin.Mandi;
						break;
				default:
						break;
				}
	}

#if UNITY_IOS
	void purchaseSuccessful( StoreKitTransaction transaction )
	{
		Debug.Log(string.Format("Purchase of {0} Successful",transaction.productIdentifier));
		SetRestoreType (transaction.productIdentifier);
		UnlockCharacterManager.Instance.UnlockCharacter();
				if (ShopManager.GoToAddress) {
						GoToAddress ();
				}
				else 
				{			
					DialogPage.SetMessage("Your purchases have been successfully restored.\n\nThank you for purchasing Animin.");
					UiPages.SetDialogBackPage(Pages.PurchasePage);
					UiPages.Next(Pages.DialogPage, 1f);
				}

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
        #else
            void purchaseUnsuccessful(string response)
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
		StoreKitManager.restoreTransactionsFailedEvent += restoreTransactionsFailed;
		StoreKitManager.restoreTransactionsFinishedEvent += restoreTransactionsFinished;
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

		StoreKitManager.restoreTransactionsFailedEvent -= restoreTransactionsFailed;
		StoreKitManager.restoreTransactionsFinishedEvent -= restoreTransactionsFinished;
#elif UNITY_ANDROID
        GoogleIABManager.purchaseSucceededEvent -= purchaseSuccessful;
		GoogleIABManager.purchaseFailedEvent -= purchaseUnsuccessful;
#endif
    }
}
