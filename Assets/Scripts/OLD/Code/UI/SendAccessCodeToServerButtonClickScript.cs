using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SendAccessCodeToServerButtonClickScript : MonoBehaviour {

	public SendAccesCodeFromChatboxClickScript SubmitFunction;
	private const string FakeCode1 = "FDG015S7";
    private const string FakeCode2 = "GL04T9LY";
    private const string FakeCode3 = "CY3877T2";
    private const string FakeCode4 = "S4G984FS";
    private const string FakeCode5 = "TTPM5PFS";
    private const string FakeCode6 = "RR85FKL4";
    private const string FakeCode7 = "54985TTS";
    private const string FakeCode8 = "LS82THLM";
    private const string FakeCode9 = "I7SMH42W";
    private const string FakeCode10 = "6GWK0H6D";
    private const string MasterReset = "AM989FTW";

	// Use this for initialization
	void Start () 
	{
		//RegisterListeners();
	}
	public void OnClick()
	{
        
//		if (SubmitFunction.gameObject.GetComponent<InputField>().text == "18271425")
//        {
//
//            UnlockCharacterManager.Instance.ID = PersistentData.TypesOfAnimin.TboAdult;
//            UnlockCharacterManager.Instance.UnlockCharacter();
//        }
//        else
        {
            SubmitFunction.OnSubmit();
        }
	}

#if UNITY_IOS
	void purchaseSuccessful( StoreKitTransaction transaction )
#elif UNITY_ANDROID
    void purchaseSuccessful(GooglePurchase transaction)
#else 
    void purchaseSuccessful()
#endif
	{
		UnregisterListeners();
		UiPages.Back();
	}

#if UNITY_IOS
		void purchaseUnsuccessful( string transaction )
#elif UNITY_ANDROID
		void purchaseUnsuccessful( string transaction, int errorcode )
#else
    void purchaseUnsuccessful(string transaction)
#endif
	{
		UnregisterListeners();
		UiPages.Back();
	}
	void RegisterListeners()
	{
		if(Application.isEditor){ return; }
		#if UNITY_IOS
		StoreKitManager.purchaseSuccessfulEvent += purchaseSuccessful;
		StoreKitManager.purchaseCancelledEvent += purchaseUnsuccessful;
		StoreKitManager.purchaseFailedEvent += purchaseUnsuccessful;
		#elif UNITY_ANDROID
        GoogleIABManager.purchaseSucceededEvent += purchaseSuccessful;
        GoogleIABManager.purchaseFailedEvent += purchaseUnsuccessful;
		#endif
	}
	void UnregisterListeners()
	{
		if(Application.isEditor){ return; }
		#if UNITY_IOS
		StoreKitManager.purchaseSuccessfulEvent -= purchaseSuccessful;
		StoreKitManager.purchaseCancelledEvent -= purchaseUnsuccessful;
		StoreKitManager.purchaseFailedEvent -= purchaseUnsuccessful;
		#elif UNITY_ANDROID
        GoogleIABManager.purchaseSucceededEvent -= purchaseSuccessful;
        GoogleIABManager.purchaseFailedEvent -= purchaseUnsuccessful;
		#endif
	}
}
