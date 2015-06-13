using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using Phi;

using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Xml.Serialization;

public class ProfilesManagementScript : Phi.SingletonMonoBehaviour<ProfilesManagementScript>
{
	//public static bool Initialized;

    //private GameObject EvolveTboToAdultWarning;

    public ItunesButtonUpdate ItunesScript;

    public bool SentToPurchaseAdultTBOFromMainScene;

    public PersistentData.TypesOfAnimin AniminToUnlockId;

    public List<PlayerProfileData> ListOfPlayerProfiles
	{
		get
		{
			return StateData.ProfileList;
		}
	}

    public PlayerProfileData CurrentProfile; 

    public PersistentData CurrentAnimin;

    public bool BeginLoadLevel;



	[System.Serializable]
	public class ProfileStateData
	{
		public List<PlayerProfileData> ProfileList; 
		//public PlayerProfileData CurrentProfile;
		public bool UpgradeTbo;
	}
	
	static public ProfileStateData StateData;
    
	public void OnDestroy()
	{		
		Debug.LogError ("DESTROY");
	}

    public void Save()
	{		
		Debug.LogWarning ("SAVE");
		if (!this) {
			Debug.LogWarning ("AVOID WIPE");
			return;
		}
		#if UNITY_IOS
		UnityEngine.iOS.Device.SetNoBackupFlag(Application.persistentDataPath);
		UnityEngine.iOS.Device.SetNoBackupFlag(Application.persistentDataPath + "/unity.txt");
		UnityEngine.iOS.Device.SetNoBackupFlag(Application.persistentDataPath + "/storeKitReceipts.archive");
		#endif
		
		Debug.Log ("Saving Profiles");
		/*for (int i =0; i< ProfilesManagementScript.Instance.ListOfPlayerProfiles.Count; i++)
		{
			StateData.ProfileList.Add(ProfilesManagementScript.Instance.ListOfPlayerProfiles[i]);
		}*/
		//StateData.CurrentProfile = ProfilesManagementScript.Instance.CurrentProfile;
		//StateData.UpgradeTbo = ProfilesManagementScript.Singleton.SentToPurchaseAdultTBOFromMainScene;
		StateData.UpgradeTbo = false;
		XmlSerializer bf = new XmlSerializer(typeof(ProfileStateData));
		Debug.Log ("Creating file");
		FileStream file = File.Create (Application.persistentDataPath + "/savedGamesTemp.anidat");
		Debug.Log ("Serializing");
		string output = "StateData:\nProfiles:";
		foreach (PlayerProfileData ppd in StateData.ProfileList)
		{
			output += "\n"  + ppd.ProfileName;
		}
		//output += "\nCurrent Profile: " + StateData.CurrentProfile.ProfileName;
		output += "\nTbo Unlock: " + StateData.UpgradeTbo;
		Debug.Log(output);
		bf.Serialize(file, StateData);
		Debug.Log ("Closing File");
		file.Close();
		if (File.Exists(Application.persistentDataPath + "/savedGames.anidat"))
		{
			Debug.Log("Deleting old");
			File.Delete(Application.persistentDataPath + "/savedGamesBackup.anidat");
			File.Replace(Application.persistentDataPath + "/savedGamesTemp.anidat", Application.persistentDataPath + "/savedGames.anidat", Application.persistentDataPath + "/savedGamesBackup.anidat");
			#if UNITY_IOS
			UnityEngine.iOS.Device.SetNoBackupFlag(Application.persistentDataPath + "/savedGamesBackup.anidat");
			#endif
		}
		else
		{
			File.Copy(Application.persistentDataPath + "/savedGamesTemp.anidat", Application.persistentDataPath + "/savedGames.anidat");
		}
		#if UNITY_IOS
		UnityEngine.iOS.Device.SetNoBackupFlag(Application.persistentDataPath + "/savedGames.anidat");
        #endif
    }
    
    public List<PlayerProfileData> ProfileList
    {
        get
        {
            return StateData.ProfileList;
        }
    }
    
    override public void Init()
    {
		Debug.Log ("Init ProfilesManagementScript");
        CharacterChoiceManager.Instance.Initialised = false;
        
        // Forces a different code path in the BinaryFormatter that doesn't rely on on - required for correct iOS saves
		Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes");
		CurrentProfile = new PlayerProfileData();
		//ListOfPlayerProfiles = new List<PlayerProfileData>();
		
		StateData = new ProfileStateData();		
		StateData.ProfileList = new List<PlayerProfileData> ();
        
        if(File.Exists(Application.persistentDataPath + "/savedGames.anidat")) 
		{
			XmlSerializer bf = new XmlSerializer(typeof(ProfileStateData));
			FileStream file = File.Open(Application.persistentDataPath + "/savedGames.anidat", FileMode.Open);
			StateData = (ProfileStateData)bf.Deserialize(file);
			file.Close();
			Debug.Log ("Save data loaded from " + Application.persistentDataPath + "/savedGames.anidat");
			
			//ListOfPlayerProfiles.Clear();			
			//for (int i =0; i< StateData.ProfileList.Count; i++)
			//{
			//	ListOfPlayerProfiles.Add(StateData.ProfileList[i]);            
			//}
			CurrentProfile = null;//StateData.ProfileList[0];//StateData.CurrentProfile;
            SentToPurchaseAdultTBOFromMainScene = StateData.UpgradeTbo;
		}
		else
		{			
			Debug.Log ("No Save data fond at" + Application.persistentDataPath + "/savedGames.anidat");			/*
            ProfilesManagementScript.Singleton.SendRealTimeNotification("Downloads",1);
            #if UNITY_IOS
            ProfilesManagementScript.Singleton.SendRealTimeNotification("IOS",1);
            #elif UNITY_ANDROID
            ProfilesManagementScript.Singleton.SendRealTimeNotification("Android",1);
            #endif
            */
        }

        RefreshProfiles ();
        
        Debug.Log("-----Registered----");
	}

	public void AssignCurrentAniminToVariable()
	{
		CurrentAnimin = CurrentProfile.Characters[(int)CurrentProfile.ActiveAnimin];
	}

    public void NewUserProfileAdded(string name, string id)
	{
        PlayerProfileData tempData = new PlayerProfileData();
        tempData = PlayerProfileData.CreateNewProfile(name);
        tempData.UniqueID = id;
        ListOfPlayerProfiles.Add(tempData);
		
		Save();
        Debug.Log("just saved...new");
		CurrentProfile = tempData;
        AchievementManager.Instance.PopulateAchievements(true);
        UnlockCharacterManager.Instance.CheckInitialCharacterUnlock();
	}
    public void LoginExistingUser(PlayerProfileData userToLogin)
    {
        foreach(PlayerProfileData tempPlayerProfileData in ProfilesManagementScript.Instance.ListOfPlayerProfiles)
        {
            if (tempPlayerProfileData == userToLogin)
            {
				//AH CurentProfile set
                CurrentProfile = tempPlayerProfileData;
                break;
            }
        }
         
        AchievementManager.Instance.PopulateAchievements(false);

        CurrentAnimin = CurrentProfile.Characters[(int)CurrentProfile.ActiveAnimin];
        UnlockCharacterManager.Instance.CheckInitialCharacterUnlock();
    }

	public void CheckProfileLoginPasscode(string code)
	{
		StartCoroutine( Account.Instance.WWWCheckLoginCode( code ) );

	}

	public void SuccessfulLogin(bool successful, string code)
	{

		if (successful) 
        {
            PlayerProfileData tempData = new PlayerProfileData();
            tempData = PlayerProfileData.CreateNewProfile(Account.Instance.UserName);
            tempData.UniqueID = code;
            ListOfPlayerProfiles.Add(tempData);
            
			CurrentProfile = tempData;
            AchievementManager.Instance.PopulateAchievements(true);
			
            CurrentAnimin = CurrentProfile.Characters[(int)CurrentProfile.ActiveAnimin];

			Save();
			Debug.Log("Saved succesful login");
            UnlockCharacterManager.Instance.CheckInitialCharacterUnlock();
		} 
		else 
		{
		}

	}

    private void RefreshProfiles()
    {
        
    }

	public void OnAllowCreateProfile(string name)
	{
////		ProfilesManagementScript.Singleton.CreateUsernameScreen.SetActive(false);
////		ProfilesManagementScript.Singleton.ProfilesScreen.SetActive(true);
////
////		PlayerProfileData newprof = CreateNewProfile(name);
////		newprof.Save();
////		RefreshProfiles();
//
	}

	public void OnRejectedProfile()
	{
		Debug.Log("NO PROFILE FOR YOU");
	}

    public void ActivateShopItemCheck()
    {
        Debug.Log("Activate shop");
        ShopManager.Instance.EndStore();
        UnlockCharacterManager.Instance.OpenShop();

		Debug.Log("Set up timeout");
		Invoke ("CheckTimeout", 15);

    }

	private void CheckTimeout()
	{
		if(!ShopManager.Instance.ShopReady)
		{
			Debug.Log("IAP Timeout");
			ShopManager.Instance.ShopReady = true;
			ProfilesManagementScript.Instance.ContinueToInAppPurchase(false);
		}
	}

    public void CheckCharacterCodeValidity(string code)
    {
        Debug.Log("Passing character code through singleton...");
        StartCoroutine(Account.Instance.WWCheckPurchaseCode(code));
    }

    public void OnAccessCodeResult(string resultId)
    {
        Debug.Log("Access code result is... "+resultId);
		if(resultId.StartsWith("Card successfully activated") || resultId.StartsWith("Animin already activated"))
        {
			UnlockCharacterManager.Instance.UnlockCharacter();
			UiPages.Next(Pages.AniminSelectPage);
        }
        if(resultId.StartsWith ("Card number not valid"))
        {
			
			DialogPage.SetMessage("We cannot verify this code, please check carefully and try again.");
			//error box
			UiPages.SetDialogBackPage(Pages.PurchasePage);
			UiPages.Next(Pages.DialogPage);
        }
		else if(resultId.StartsWith("Card number already used"))
        {
			//DialogPage.SetMessage("We're very sorry but there has been an error connecting to the internet.\n\nPlease check your internet connection and try again.");			
			//UiPages.SetDialogBackPage(Pages.AniminSelectPage);			
			DialogPage.SetMessage("This code has already been used to unlock an Animin.");
			//error box
			UiPages.SetDialogBackPage(Pages.PurchasePage);
			UiPages.Next(Pages.DialogPage);
        }
		else if(resultId.StartsWith("Animin already activated"))
        {
			Debug.Log("WHOA: Something went weird, we shouldn't be able to do this.");
			UiPages.Next(Pages.AniminSelectPage);
        }
		else if(resultId.StartsWith("Something went wrong, please try again in a bit..."))
        {
			
			DialogPage.SetMessage("There has been a problem connecting to the verification server.\n\nPlease check your internet connection and try again.");
			//error box
			UiPages.SetDialogBackPage(Pages.PurchasePage);
			UiPages.Next(Pages.DialogPage);
        }
        else
        {
            Debug.Log("INVALID CODE RESPONSE");
        }
    }

    public void ShowDemoCardPopup()
    {
    }

    public void CloseDemoCardPopup()
    {
        Debug.Log("Switching purchase to " + AniminToUnlockId);
        ItunesScript.SetCharacterIcons(AniminToUnlockId);
    }

    public void ShowEvolutionPurchaseWarning()
    {
        //EvolveTboToAdultWarning = (GameObject)GameObject.Instantiate (Resources.Load("NGUIPrefabs/UI - EvolveBabyTboWarning"));
        //EvolveTboToAdultWarning.SetActive( true );

    }

	public void CloseEvolutionPurchaseWarning(bool andContinue)
    {
        //Destroy(EvolveTboToAdultWarning);

        if (andContinue)
        {
            SentToPurchaseAdultTBOFromMainScene = true;
			Save ();
            Application.LoadLevel(0);
        }
    }

    public void FasttrackThroughProfileSelect()
    {
        Debug.Log("Fast track initialised...");
        AniminToUnlockId = PersistentData.TypesOfAnimin.TboAdult;
        if( Application.isEditor)
        {
            ProfilesManagementScript.Instance.ContinueToInAppPurchase(true);
        }
        else
        {
            ActivateShopItemCheck();
        }
    }

    public void ContinueToInAppPurchase(bool shouldContinue)
    {
        if(shouldContinue)
        {
			UiPages.Next(Pages.PurchasePage);
        }
        else
        {
            Debug.Log("Shop Unavailable");
			
			DialogPage.SetMessage("We're very sorry but there has been an error connecting to the internet.\n\nPlease check your internet connection and try again.");
			//error box
			UiPages.SetDialogBackPage(Pages.AniminSelectPage);
			UiPages.Next(Pages.DialogPage);
        }

    }

    public void SendRealTimeNotification(string dataType, int amount)
    {
        StartCoroutine( Account.Instance.WWSendRealtimeNotification( dataType, amount.ToString() ) );
    }

    void OnLevelWasLoaded(int level)
    {
//        SaveAndLoad.Instance.LoadAllData();
        Debug.Log(SentToPurchaseAdultTBOFromMainScene);
        if (level == 0 && SentToPurchaseAdultTBOFromMainScene)
        {
            Debug.Log("Level loaded");
            SentToPurchaseAdultTBOFromMainScene = false;
            FasttrackThroughProfileSelect();
        }

    }

	bool loading = false;
	// Update is called once per frame
	void Update () 
	{
		if(BeginLoadLevel && !loading)
        {
            BeginLoadLevel = false;
            Debug.Log("Loading New level");
            StartCoroutine(LoadLevel(@"ARBase"));
        }
	}
	AsyncOperation async;
	
	public IEnumerator LoadLevel(string name)
	{

//		yield return new WaitForSeconds(0.1f);
//		
//		//nextLevel is one of the class fields
//		AsyncOperation	nextLevel = Application.LoadLevelAsync(name);
//		while (!nextLevel.isDone)
//		{ 
//			yield return new WaitForEndOfFrame(); 
//		}    

		Debug.LogWarning("ASYNC LOAD STARTED - " +
		                 "DO NOT EXIT PLAY MODE UNTIL SCENE LOADS... UNITY WILL CRASH");
		async = Application.LoadLevelAsync(name);
		//async.allowSceneActivation = false;

		yield return async;
		loading = false;
	}

}
