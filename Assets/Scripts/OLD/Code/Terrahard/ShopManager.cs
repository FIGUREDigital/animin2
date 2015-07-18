/*
* STEP 1: Install Prime31 Plugin
* STEP 2: StartStore with ids setup in apple and google play servers when you start the app
* STEP 3: Call BuyItem(string id) to buy item
* STEP 4: Wait for CurrentPurchaseStatus to go to either success or fail
*/
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ShopManager
{
	private const string publicKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAqWSuyg1d82CxPSUf3S/qMjxGZ4JsCEMPzfTAbqxVUTS9QSt7bdtJP8EJyWfEr/OlNKWtAk+ZeqS6fzZnwv3Z+/F2Wv1UUIEkI8qFlwaFtljFCpR2AAZnUlK0myym5LNs8yTvmX5shGe3SNGW5tDsj4RLuP9pq+iiJer6ZjbXthubABF6VF6xPH4Dy4MOQOw52JaOJxPaLXpthEWdytlxOcSR1IlzDfOz+Ky7QH1Li0TV9PPlgGXlHLAOwGG5Tyw4iJmjeLuBrNLH0e8ihp2im9gWnrMzVMTMHN8xzfKv+ZiwDaFaP9FY5srMljdnyCtZg9tMjPVf6yAZKIeJGA2eFwIDAQAB";

	#region Singleton
    private static ShopManager s_Instance;

    public static ShopManager Instance
    {
        get
        {
            if ( s_Instance == null )
            {
                s_Instance = new ShopManager();
            }
            return s_Instance;
        }
    }

    #endregion

    public enum ShopState
    {
        Idle,
        TransitioningToShop,
        InShop
    }
    
    private ShopState m_CurrentShopState = ShopState.Idle;

    public string Error;

	private bool mShopReady;
	public bool ShopReady
	{
		get 
		{
			return mShopReady;
		}
		set
		{
			mShopReady = value;
		}
	}

	private List<Item> mItems = new List<Item>();

    public class Item
    {
        public string ID;
        public string Price;

        public Item( string id )
        {
            ID = id;
        }
    }


    private const float TIMEOUT_TIME = 10.0f;
    private float m_TimeoutTimer = TIMEOUT_TIME;
    

    public ShopManager()
    {
#if UNITY_IOS
		StoreKitManager.purchaseSuccessfulEvent+=purchaseSuccessful;
		StoreKitManager.purchaseCancelledEvent += purchaseCancelled;
		StoreKitManager.purchaseFailedEvent += purchaseFailed;
		StoreKitManager.productListReceivedEvent += productListReceivedEvent;
		StoreKitManager.productListRequestFailedEvent += productListRequestFailedEvent;
		StoreKitManager.restoreTransactionsFailedEvent += restoreTransactionsFailed;
		StoreKitManager.restoreTransactionsFinishedEvent += restoreTransactionsFinished;
#elif UNITY_ANDROID
        GoogleIABManager.billingSupportedEvent += billingSupportedEvent;
        GoogleIABManager.billingNotSupportedEvent += billingNotSupportedEvent;
        GoogleIABManager.queryInventorySucceededEvent += queryInventorySucceededEvent;
        GoogleIABManager.queryInventoryFailedEvent += queryInventoryFailedEvent;
        GoogleIABManager.purchaseCompleteAwaitingVerificationEvent += purchaseCompleteAwaitingVerificationEvent;
        GoogleIABManager.purchaseSucceededEvent += purchaseSucceededEvent;
        GoogleIABManager.purchaseFailedEvent += purchaseFailed;
        GoogleIABManager.consumePurchaseSucceededEvent += consumePurchaseSucceededEvent;
        GoogleIABManager.consumePurchaseFailedEvent += consumePurchaseFailedEvent;

		GoogleIAB.init(publicKey);
#endif
        
    }

    public enum PurchaseStatus
    {
        Idle,
        InProgress,
        Fail,
        Success,
        Cancel
    }

    public static PurchaseStatus CurrentPurchaseStatus;

    public enum RestoreStatus
    {
        Idle,
        InProgress,
        Fail,
        Success
    }

    public static RestoreStatus CurrentRestoreStatus;

	public static bool GoToAddress;

	void restoreTransactionsFailed( string error )
	{
        CurrentRestoreStatus = RestoreStatus.Fail;
		Debug.Log( "restoreTransactionsFailed: " + error );
		
		DialogPage.SetMessage("We're very sorry but there ha been an error restoring your purchases, please check your internet connection and try again.");
		UiPages.SetDialogBackPage(Pages.AniminSelectPage);
		UiPages.Next(Pages.DialogPage);
	}
	
	
	void restoreTransactionsFinished()
	{
        CurrentRestoreStatus = RestoreStatus.Success;
		Debug.Log( "restoreTransactionsFinished" );
		
		DialogPage.SetMessage("Your purchases have been successfully restored.\n\nThank you for purchasing Animin.");
		UiPages.SetDialogBackPage(Pages.AniminSelectPage);
		UiPages.Next(Pages.DialogPage, 5f);
	}

#if UNITY_IOS

	static List<StoreKitProduct> productsList;

    private void productListReceivedEvent( List<StoreKitProduct> productList )
    {
        Debug.Log( "productListReceivedEvent. total products received: " + productList.Count );
		productsList = productList;
        // print the products to the console
        foreach( StoreKitProduct product in productList )
            Debug.Log( product.ToString() + "\n" );

		if (!ContinueRestore () && !mShopReady) 
		{
			if (productList.Count > 0) {
				mShopReady = true;
				Debug.Log ("Go to shop");
				ProfilesManagementScript.Instance.ContinueToInAppPurchase (true);

			} else if (productList.Count <= 0) {
				mShopReady = true;
				Debug.Log ("Avoid shop");
				ProfilesManagementScript.Instance.ContinueToInAppPurchase (false);

			}
		}

    }

	private void productListRequestFailedEvent( string error)
	{
		Debug.Log( "productListRequestFailedEvent. Result: " + error );
		StartStore(mItems);
	}
#endif

		#if UNITY_IOS
		void purchaseFailed( string error )
		#elif UNITY_ANDROID
		void purchaseFailed( string error, int errorcode )
        #else
        void purchaseFailed(string error)
		#endif
	{
        
        CurrentPurchaseStatus = PurchaseStatus.Fail;
	    Error = error;
	
		Debug.Log( "purchase failed with error: " + error );
        //GUIDebug.Log("purchase failed with error: " + error);
     }   
	

	void purchaseCancelled( string error )
	{
        CurrentPurchaseStatus = PurchaseStatus.Cancel;
		Debug.Log( "purchase cancelled with error: " + error );
        //GUIDebug.Log("purchase cancelled with error: " + error);
	}
				

#if UNITY_IOS
	StoreKitProduct GetProduct(string id)
	{
		if (productsList != null) {
			for (int i = 0; i < productsList.Count; i++) {
				if (productsList [i].productIdentifier == id) {
					return productsList [i];
				}
			}
		}
		return null;
	}
	public string GetPrice(string id)
	{

		StoreKitProduct p = GetProduct (id);
		if (p != null) 
		{
			Debug.Log ("GetPrice " + id +" found "+p.formattedPrice);
			return p.formattedPrice;
		}
		Debug.Log ("GetPrice " + id +" not found");
		return "£1.99";
	}
	void purchaseSuccessful( StoreKitTransaction transaction )
	{		
		Debug.Log ("Purchase " + transaction.productIdentifier + " " + productsList.Count);
		StoreKitProduct product = null;
		if (productsList != null) {
			for (int i = 0; i < productsList.Count; i++) {
				if (productsList [i].productIdentifier == transaction.productIdentifier) {
					product = productsList [i];
					break;
				}
			}
		}

		UnlockCharacterManager.PurchaseSuccessful(transaction.productIdentifier);

		if (product != null) {
			
			Debug.Log ("Cost " + product.price+" "+product.currencyCode);
			decimal cost = 0;
			decimal.TryParse (product.price, out cost);
			UnityEngine.Analytics.Analytics.Transaction (transaction.productIdentifier, cost, product.currencyCode, transaction.base64EncodedTransactionReceipt, "");
		} else {
			UnityEngine.Analytics.Analytics.Transaction (transaction.productIdentifier, 0, "GBP");
		}
        if( CurrentRestoreStatus == RestoreStatus.InProgress || CurrentRestoreStatus == RestoreStatus.Success )
        {

        }
        else
        {
            CurrentPurchaseStatus = PurchaseStatus.Success;
        }
		Debug.Log( "purchased product: " + transaction );
	}

#elif UNITY_ANDROID


    GoogleSkuInfo GetProduct(string id)
    {
        if (skus != null)
        {
            for (int i = 0; i < skus.Count; i++)
            {
                if (skus[i].productId == id)
                {
                    return skus[i];
                }
            }
        }
        return null;
    }
    public string GetPrice(string id)
    {

        GoogleSkuInfo p = GetProduct(id);
        if (p != null)
        {
            Debug.Log("GetPrice " + id + " found " + p.price);
            return p.price;
        }
        Debug.Log("GetPrice " + id + " not found");
        return "£1.99";
    }

    void purchaseSucceededEvent(GooglePurchase purchase)
    {
        CurrentPurchaseStatus = PurchaseStatus.Success;
		
		UnlockCharacterManager.PurchaseSuccessful(purchase.productIdentifier);
        Debug.Log("purchaseSucceededEvent: " + purchase);
    }

    void consumePurchaseSucceededEvent(GooglePurchase purchase)
    {
        Debug.Log("consumePurchaseSucceededEvent: " + purchase);
    }

    void consumePurchaseFailedEvent(string error)
    {
        Debug.Log("consumePurchaseFailedEvent: " + error);
    }

    List<GoogleSkuInfo> skus = null;
    private void queryInventorySucceededEvent( List<GooglePurchase> purchases, List<GoogleSkuInfo> skus )
    {
        this.skus = skus;
        Debug.Log( "queryInventorySucceededEvent" );
        //GUIDebug.Log("queryInventorySucceededEvent");
        Prime31.Utils.logObject( purchases );
        Prime31.Utils.logObject( skus );
		if(!ContinueRestore() && !mShopReady)
		{
			if(skus.Count > 0)
			{
				mShopReady = true;
				Debug.Log("Go to shop");
				ProfilesManagementScript.Instance.ContinueToInAppPurchase(true);
				
			}
			else
			{
				mShopReady = true;
				Debug.Log("Avoid shop");
				ProfilesManagementScript.Instance.ContinueToInAppPurchase(false);				
			}
		}
		Debug.Log ("Number of purchases: " + purchases.Count.ToString ());
        for( int i = 0; i < purchases.Count; i++ )
        {
            Debug.Log( "purchases " + purchases[ i ] );
        }
		Debug.Log ("Number of skus: " + skus.Count.ToString ());
        for( int i = 0; i < skus.Count; i++ )
        {
            Debug.Log( "skus " + skus[ i ].productId );
        }
    }


    void queryInventoryFailedEvent(string error)
    {
		Debug.Log("queryInventoryFailedEvent: " + error);
		StartStore(mItems);
    }

    void purchaseCompleteAwaitingVerificationEvent(string purchaseData, string signature)
    {
        Debug.Log("purchaseCompleteAwaitingVerificationEvent. purchaseData: " + purchaseData + ", signature: " + signature);
    }

    void billingSupportedEvent()
    {
        Debug.Log("billingSupportedEvent");
    }


    void billingNotSupportedEvent(string error)
    {
        Debug.Log("billingNotSupportedEvent: " + error);
    }
#endif

	bool continueRestore = false;
	public void RestoreItems()
	{
		GoToAddress = false;
		CurrentRestoreStatus = RestoreStatus.InProgress;
		continueRestore = true;
#if UNITY_IPHONE
		if (productsList != null)
#elif UNITY_ANDROID
		if(skus != null)
#endif 
		{
			ContinueRestore();
		} 
		else
		{
			UnlockCharacterManager.OpenShop();
		}
	}

	// Can be called at any point and if we are about to restore, it will return true.
	public bool ContinueRestore()
	{
		if (continueRestore) 
		{
			#if UNITY_IOS
			StoreKitBinding.restoreCompletedTransactions();
			#elif UNITY_ANDROID
			RestoreAndroidPurchases();
			#endif
			continueRestore = false;
			return true;
		}
		return false;
	}
    
	private void RestoreAndroidPurchases()
	{
#if UNITY_ANDROID
		foreach(GooglePurchase gp in IAP.androidPurchasedItems)
		{

		}
#endif
	}


    public void Update()
    {
        switch ( CurrentPurchaseStatus )
        {
            case PurchaseStatus.Idle:
                break;
            case PurchaseStatus.InProgress:
            {
                if ( m_TimeoutTimer > 0.0f )
                {
                    m_TimeoutTimer = Mathf.Max(0.0f, m_TimeoutTimer - Time.deltaTime);
                }
                else if ( m_TimeoutTimer == 0.0f )
                {
                    CurrentPurchaseStatus = PurchaseStatus.Fail;
                }
                break;
            }
            case PurchaseStatus.Success:
            {
                m_TimeoutTimer = TIMEOUT_TIME;
                DrawTransactionCompleted();
                GetItem();
                break;
            }
            case PurchaseStatus.Cancel:
            case PurchaseStatus.Fail:
            {
                m_TimeoutTimer = TIMEOUT_TIME;
                DrawTransactionFailed();
                break;
            }
        }
    }

    private void DrawTransactionFailed()
    {
        DrawPopupScreen();
        
    }

    private void DrawTransactionCompleted()
    {
        DrawPopupScreen();
    }

    private void GetItem()
    {
        m_CurrentShopState = ShopState.Idle;
    }

    public void BuyItem( string productID = "" )
    {
		GoToAddress = true;
		Debug.Log( "AAAAAAAAAAAAAAAAAAAAAAAAAA " + CurrentPurchaseStatus + " " + productID );
        if( CurrentPurchaseStatus == PurchaseStatus.Idle )
        {
            CurrentPurchaseStatus = PurchaseStatus.InProgress;

            string id = productID; //GetProductID( "LevelsBundle" + LevelSelectScreen.LevelToLoad  );

			if( HasBought() || Application.isEditor )
            {
                CurrentPurchaseStatus = PurchaseStatus.Success;
				UnlockCharacterManager.Instance.UnlockCharacter();
				UiPages.Back();
            }
            else
            {
#if UNITY_ANDROID
                GoogleIAB.purchaseProduct( productID );
#elif UNITY_IOS
                StoreKitBinding.purchaseProduct( id, 1 );
#endif
            }
        }
    }

	public bool HasBought(string productID = "")
	{
		bool beenBought = false;
		#if UNITY_IOS
		List<StoreKitTransaction> transactions =  StoreKitBinding.getAllSavedTransactions();
		
		
		foreach( StoreKitTransaction storeKitTransaction in transactions )
		{
			if( storeKitTransaction.productIdentifier == productID )
			{
				beenBought = true;
			}
		}
		#elif UNITY_ANDROID
		List<GooglePurchase> transactions = IAP.androidPurchasedItems;
		foreach( GooglePurchase storeKitTransaction in transactions )
		{
			if( storeKitTransaction.productId == productID )
			{
				beenBought = true;
			}
		}
		#endif

		return beenBought;
	}


    private void DrawPopupScreen()
    {

    }

    private void GoInShopCallback( int dummy )
    {
        if ( m_CurrentShopState != ShopState.InShop )
        {
            m_CurrentShopState = ShopState.InShop;

        }
    }


    private void ReturnToStateCallback( int dummy )
    {
        //FlurryBinding.logEvent( "Shop - Return", false );
        
        m_CurrentShopState = ShopState.Idle;
    }

    public void StartStore(string[] shopItems)
    {

        Debug.Log("Start store array");
        mShopReady = false;
		mItems.Clear();
        //m_CurrentShopState = ShopState.InShop;
        string[] ids = new string[ shopItems.Length ];
        for( int i = 0; i < ids.Length; i++ )
        {
            ids[ i ] = shopItems[ i ];
			mItems.Add(new Item(ids[i]));
        }
#if UNITY_IOS
		StoreKitBinding.requestProductData( ids );
#elif UNITY_ANDROID
        ids = RemoveUnneccesaryIDsForAndroid(ids);
        GoogleIAB.queryInventory( ids );
#endif
    }
	public void StartStore(List<Item> shopItems)
	{

        Debug.Log("Start store list");
        mShopReady = false;
		//m_CurrentShopState = ShopState.InShop;
		string[] ids = new string[ shopItems.Count ];
		for( int i = 0; i < ids.Length; i++ )
		{
			ids[ i ] = shopItems[ i ].ID;
		}
		#if UNITY_IOS
		StoreKitBinding.requestProductData( ids );
		#elif UNITY_ANDROID
        ids = RemoveUnneccesaryIDsForAndroid(ids);
		GoogleIAB.queryInventory( ids );
		#endif
	}

    public string[] RemoveUnneccesaryIDsForAndroid(string[] tempStringArray)
    {
        List<string> tempStringList = new List<string>();

        foreach(string tempString in tempStringArray)
        {
            if (!tempString.Contains("_UNLOCK"))
            {
                tempStringList.Add(tempString);
                Debug.Log(tempString);
            }
        }

        tempStringArray = tempStringList.ToArray();

        return tempStringArray;
    }

    public void EndStore()
    {
		Debug.Log("Close Shop");
		mShopReady = true;
        CurrentPurchaseStatus = PurchaseStatus.Idle;
    }
}
//#endif
