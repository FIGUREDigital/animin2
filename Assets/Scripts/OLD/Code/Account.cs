using UnityEngine;
using System.Collections;

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

    public string UniqueID{get {return ProfilesManagementScript.Singleton.CurrentProfile.UniqueID;}}

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

        if( w.error != null )
        {
            Debug.Log( w.error );
        }
        else
        {
            if( newUser )
            {

                string tempID = w.text.Replace(System.Environment.NewLine, "");
//				Debug.Log("test"+UniqueID+"test");
                ProfilesManagementScript.Singleton.NewUserProfileAdded(name, tempID);
            }

//            Debug.Log( w.text );
            Debug.Log( "Finished uploading name data" );
        }

		if (newUser) 
		{
			
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
			
			ProfilesManagementScript.Singleton.SuccessfulLogin(tempBool,code);						

		}
		
	}

    public IEnumerator WWCheckPurchaseCode(string code) 
    {
        Debug.Log("Checking purchase code " + code);

        bool demoCode = CheckDemoCode(code);

        Debug.Log("It is a demo code... " + demoCode);

        if (demoCode)
        {
            Debug.Log("Demo code entered. Current selected animin is " + ProfilesManagementScript.Singleton.AniminToUnlockId);
            ProfilesManagementScript.Singleton.AniminToUnlockId = PersistentData.TypesOfAnimin.TboAdult;
            ProfilesManagementScript.Singleton.ShowDemoCardPopup();

        }
        else
        {
            WWWForm data = new WWWForm();

            data.AddField( "CardNumber", code );
            data.AddField( "UserID", UniqueID );
            data.AddField("Animin", ProfilesManagementScript.Singleton.AniminToUnlockId.ToString());



            var w = new WWW(SERVER_CHECK_CARD_URL, data);

            yield return w;

            if (w.error != null)
            {
                Debug.Log(w.error);
                ProfilesManagementScript.Singleton.OnAccessCodeResult("Something went wrong, please try again in a bit...");
            }

            else
            {           

                Debug.Log(w.text);                              
                       
                ProfilesManagementScript.Singleton.OnAccessCodeResult(w.text);

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
