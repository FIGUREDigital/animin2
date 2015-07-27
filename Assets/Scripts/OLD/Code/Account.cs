using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Account
{
    #region Singleton

    private static Account s_Instance;

    public static Account Instance
    {
        get
        {
            if( s_Instance == null )
            {
                s_Instance = new Account();
            }
            return s_Instance;
        }
    }

    #endregion

    private const string SERVER_SEND_URL = "http://animin.me/wp-admin/DatabaseAndScripts/AddData.php";

    private const string SERVER_CHECK_URL = "http://animin.me/wp-admin/DatabaseAndScripts/CheckLoginData.php"; 

    private const string SERVER_CHECK_CARD_URL = "http://animin.me/wp-admin/DatabaseAndScripts/CheckCardLegitimacy.php"; 

    private const string SERVER_REALTIME_SEND_URL = "http://terahard.org/Teratest/DatabaseAndScripts/RealtimeDataSend.php"; 

    private const string SERVER_REALTIME_GET_URL = "http://terahard.org/Teratest/DatabaseAndScripts/RealtimeDataGet.php";

    public string UniqueID{get {return ProfilesManagementScript.Instance.CurrentProfile.UniqueID;}}

	public string UserName;

	public string FirstName;

	public string LastName;

	public string Address;

	public string Addressee;

    public string[] DemoCodes =
        {
            "456789",
        };

	public IEnumerator WWWSendData( bool newUser,string name, string character, string address, string addressee, string firstname, string lastname)
    {
        //if(PlayerPrefs.HasKey( "PLAYER_ID" )) yield break;

        WWWForm webForm = new WWWForm();

        webForm.AddField( "Name", name );        
        if( newUser )
        {
            webForm.AddField( "NewUser", "1" );
        }
        else
        {            
			webForm.AddField( "NewUser", "0" );
            webForm.AddField( "ID", UniqueID );
//			Debug.Log ("test"+UniqueID+"test");
        }

        webForm.AddField( "Address", address );
//		Debug.Log (address);

        webForm.AddField( "Addressee", addressee );
//		Debug.Log (addressee);

		webForm.AddField( "FirstName", firstname );
//		Debug.Log (firstname);

		webForm.AddField( "LastName", lastname );
//		Debug.Log (lastname);

        webForm.AddField( "Device", "" + Application.platform );
//		Debug.Log (Application.platform);

        webForm.AddField( "Character", character );
//		Debug.Log (character);

		Debug.Log ("creating user: " + UserName);
		WWW w = new WWW( SERVER_SEND_URL, webForm );
		yield return w;

		if (w.error != null) {
				Debug.Log (w.error);
		} else {
				if (newUser) {

						string tempID = w.text.Replace (System.Environment.NewLine, "");
//				Debug.Log("test"+UniqueID+"test");
						ProfilesManagementScript.Instance.NewUserProfileAdded (name, tempID);
				}

//            Debug.Log( w.text );
				Debug.Log ("Finished uploading name data");
		}

		if (newUser) {

		}


    }
	public IEnumerator WWWCheckLoginCode( string code )
	{   

        WWWForm webForm = new WWWForm();

		webForm.AddField( "Code", code );
		
		WWW w = new WWW( SERVER_CHECK_URL, webForm );

		yield return w;
		
		if( w.error != null )
		{
			Debug.Log( w.error );
		}
		else
		{			
			
			Debug.Log( w.text );
			Debug.Log( "Finished uploading data" );

			string[] tempArray = w.text.Split(':');

			bool tempBool = bool.Parse(tempArray[0]);

            if (tempBool)
            {
                UserName = tempArray[1];
            }
			
			ProfilesManagementScript.Instance.SuccessfulLogin(tempBool,code);						

		}
		
	}


	//Customer Service Codes
	private const string FakeCode1 = "HHFDG015S7";
	private const string FakeCode2 = "HHGL04T9LY";
	private const string FakeCode3 = "HHCY3877T2";
	private const string FakeCode4 = "HHS4G984FS";
	private const string FakeCode5 = "HHTTPM5PFS";
	private const string FakeCode6 = "HHRR85FKL4";
	private const string FakeCode7 = "HH54985TTS";
	private const string FakeCode8 = "HHLS82THLM";
	private const string FakeCode9 = "HHI7SMH42W";
	private const string FakeCode10 = "HH6GWK0H6D";
	private const string MasterReset = "HHAM989FTW";

	//Custom Service Functions
	bool FakeCodeUsed(string code)
	{
		if (PlayerPrefs.GetInt(code) > 0)
		{
			return true;
		}
		return false;
	}
	void SetCodeUsed(string code)
	{
		PlayerPrefs.SetInt(code, 1);
	}
	
	void ResetCodes()
	{
		PlayerPrefs.SetInt(FakeCode1, 0);
		PlayerPrefs.SetInt(FakeCode2, 0);
		PlayerPrefs.SetInt(FakeCode3, 0);
		PlayerPrefs.SetInt(FakeCode4, 0);
		PlayerPrefs.SetInt(FakeCode5, 0);
		PlayerPrefs.SetInt(FakeCode6, 0);
		PlayerPrefs.SetInt(FakeCode7, 0);
		PlayerPrefs.SetInt(FakeCode8, 0);
		PlayerPrefs.SetInt(FakeCode9, 0);
		PlayerPrefs.SetInt(FakeCode10, 0);
		PlayerPrefs.SetInt(MasterReset, 0);
	}
	bool IsServiceCode(string code)
	{
		return code == FakeCode1 ||
			   code == FakeCode2 ||
			   code == FakeCode3 ||
			   code == FakeCode4 ||
			   code == FakeCode5 ||
			   code == FakeCode6 ||
			   code == FakeCode7 ||
			   code == FakeCode8 ||
			   code == FakeCode9 ||
			   code == FakeCode10 ||
			   code == MasterReset;


	}
    public IEnumerator WWCheckPurchaseCode(string code) 
    {
        Debug.Log("Checking purchase code " + code);

		// OLD HARRY STUFF FOR REFERENCE
//        bool demoCode = CheckDemoCode(code);
//
//        Debug.Log("It is a demo code... " + demoCode);
//
//        if (demoCode)
//        {
//            Debug.Log("Demo code entered. Current selected animin is " + ProfilesManagementScript.Singleton.AniminToUnlockId);
//            ProfilesManagementScript.Singleton.AniminToUnlockId = PersistentData.TypesOfAnimin.TboAdult;
//            ProfilesManagementScript.Singleton.ShowDemoCardPopup();
//
//        }
		bool backdoorCode = code == MasterReset;
		bool serviceCode = IsServiceCode(code);


		if (serviceCode || backdoorCode)
		{
			
			#if UNITYANALYTICS
			UnityEngine.Analytics.Analytics.CustomEvent ("CodeBackdoor", new Dictionary<string,object>());
#endif
			Debug.Log("It is a service code... " + code);
			if(backdoorCode)
			{
				Debug.Log("-------------Welcome back, Captain!-------------");
				ResetCodes();
			}
			if(!FakeCodeUsed(code))
			{
				SetCodeUsed(code);
				UnlockCharacterManager.Instance.UnlockCharacter();
				UiPages.Next(Pages.AniminSelectPage);
			}
			else
			{
				Debug.Log("This service code has already been used: " + code);
				DialogPage.SetMessage("This code has already been used to unlock an Animin.");
				//error box
				UiPages.SetDialogBackPage(Pages.PurchasePage);
				UiPages.Next(Pages.DialogPage);
			}
			PlayerPrefs.Save();
		}
        else
		{
			
			#if UNITYANALYTICS
			UnityEngine.Analytics.Analytics.CustomEvent ("CodeCheck", new Dictionary<string,object>());
#endif
			Debug.Log("Sumbitting code: " + code);
            WWWForm data = new WWWForm();

            data.AddField( "CardNumber", code );
            data.AddField( "UserID", UniqueID );
            data.AddField("Animin", ProfilesManagementScript.Instance.AniminToUnlockId.ToString());



            var w = new WWW(SERVER_CHECK_CARD_URL, data);

            yield return w;

            if (w.error != null)
            {
				Debug.Log("GENERAL ERROR ON CODE SUBMIT: " + code);
                Debug.Log(w.error);
                ProfilesManagementScript.Instance.OnAccessCodeResult(code, "Something went wrong, please try again in a bit...");
				
				#if UNITYANALYTICS
				UnityEngine.Analytics.Analytics.CustomEvent ("CodeError", new Dictionary<string, object>{{"code",code},{"error",w.error}});
#endif
            }

            else
            {           
				Debug.Log("CODE RESULT RETURNED FOR: " + code);
                Debug.Log(w.text);                              
                       
                ProfilesManagementScript.Instance.OnAccessCodeResult(code, w.text);

            }
        }
    }

    public bool CheckDemoCode(string code)
    {

        for (int i=0; i < DemoCodes.Length; i++ )
        {
            if (code == DemoCodes[i])
            {
                return true;
            }
        }
        return false;

    }

    public IEnumerator WWSendRealtimeNotification(string dataType, string amount)
    {
        //Data types:
        // LoggedIn
        // Downloads
        // AniminUnlock
        // IOS
        // Android

        Debug.Log("Registering " + amount + " " + dataType + " with realtime database");

        WWWForm webForm = new WWWForm();

        webForm.AddField( "DataType", dataType );

        webForm.AddField("AmountToAdd", amount);

        WWW w = new WWW( SERVER_REALTIME_SEND_URL, webForm );

        yield return w;

        if( w.error != null )
        {
            Debug.Log( w.error );
        }
        else
        {   
            Debug.Log( w.text );
            Debug.Log( "Finished uploading data" );                          

        }

    }

    public void ClearAccountClassData()
    {

        UserName = "";

        FirstName = "";

        LastName = "";

        Address = "";

        Addressee = "";

    }
}
