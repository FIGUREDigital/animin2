using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class ProfilesManagementScript : MonoBehaviour 
{
	public static ProfilesManagementScript Singleton;
	public static bool Initialized;
	public GameObject PrefabProfile;
	public GameObject ProfilesRoot;
	public GameObject OLD_ProfilesScreen;
	public GameObject AniminsScreen;
	public GameObject LoadingScreen;
	public GameObject CreateUsernameScreen;
	public GameObject PurchaseChoiceScreen;
	public GameObject CreateAccessCodeScreen;
	public GameObject LoadingSpinner;
	public GameObject ErrorBox;
	public GameObject CloseWebview;
	public GameObject SelectProfile;
	public GameObject NewUser;
	public GameObject LoginUser;
	public GameObject LoginCheckingDialogue;
	public GameObject NoSuchUserCodeDialogue;
	public GameObject AddressInput;
	public bool BeginLoadLevel;
	public GameObject[] AniminSprites;
    public GameObject DemoCardPopup;

    private GameObject EvolveTboToAdultWarning;

    public ItunesButtonUpdate ItunesScript;

	public Text PiAge;
	public Text TBOAge;
	public Text KelsiAge;
	public Text MandiAge;

    public bool SentToPurchaseAdultTBOFromMainScene;

    public PersistentData.TypesOfAnimin AniminToUnlockId;

    public List<PlayerProfileData> ListOfPlayerProfiles;

    public PlayerProfileData CurrentProfile; 

    public PersistentData CurrentAnimin;



	void Awake()
	{
		Singleton = this;
		if(!Initialized)
		{
			Initialized = true;
			GameObject go = new GameObject();
			go.name = "ArCameraManager";
			go.AddComponent<ArCameraManager>();
            //TempDebugPanel.text = "Awake";
            SendRealTimeNotification("LoggedIn", 1);

		}

	}

    void LoadProfileData()
    {
        SaveAndLoad.Instance.Awake();
        CurrentProfile = new PlayerProfileData();
        ListOfPlayerProfiles = new List<PlayerProfileData>();
        SaveAndLoad.Instance.LoadAllData();
    }

	
	void Start ()
	{
        //TempDebugPanel.text = "Start";
		//PlayerProfileData.ActiveProfile = PlayerProfileData.GetDefaultProfile();
		//if(PlayerProfileData.ActiveProfile == null)
		//{
        //ProfilesManagementScript.Singleton.CurrentProfile = PlayerProfileData.CreateNewProfile("DefaultProfile");
		//}
        AniminsScreen.SetActive(true);
        CharacterChoiceManager.Instance.Initialised = false;
        CharacterChoiceManager.Instance.FindCharacterChoices(AniminsScreen);
        AniminsScreen.SetActive(false);
        //TempDebugPanel.text = "About to refresh";
        LoadProfileData();
        RefreshProfiles ();

        //EvolutionManager.Instance.Deserialize();

		//ServerManager.Register("ServerProfile");
		//AppDataManager.SetUsername("ServerProfile");

		Debug.Log("-----Registered----");
        

		//ServerManager.AddLeaderboardScore(15, 1);
		//ServerManager.GetLeaderboardScores(1);
	}

	public void AssignCurrentAniminToVariable()
	{
		CurrentAnimin = CurrentProfile.Characters[(int)CurrentProfile.ActiveAnimin];
	}

    public void NewUserProfileAdded(string name, string id)
	{
		NewUser.SetActive(false);
		AniminsScreen.SetActive(true);
        PlayerProfileData tempData = new PlayerProfileData();
        tempData = PlayerProfileData.CreateNewProfile(name);
        tempData.UniqueID = id;
        ListOfPlayerProfiles.Add(tempData);
        SaveAndLoad.Instance.SaveAllData();
		Debug.Log("just saved...new");
		CurrentProfile = tempData;
        AchievementManager.Instance.PopulateAchievements(true);		
        
        ProfilesManagementScript.Singleton.AniminsScreen.SetActive(true);
        UnlockCharacterManager.Instance.CheckInitialCharacterUnlock();

	}
    public void LoginExistingUser(PlayerProfileData userToLogin)
    {
        foreach(PlayerProfileData tempPlayerProfileData in ProfilesManagementScript.Singleton.ListOfPlayerProfiles)
        {
            if (tempPlayerProfileData == userToLogin)
            {
                CurrentProfile = tempPlayerProfileData;
                break;
            }
        }
         
        AchievementManager.Instance.PopulateAchievements(false);

        CurrentAnimin = CurrentProfile.Characters[(int)CurrentProfile.ActiveAnimin];
        ProfilesManagementScript.Singleton.AniminsScreen.SetActive(true);
        UnlockCharacterManager.Instance.CheckInitialCharacterUnlock();
    }

	public void CheckProfileLoginPasscode(string code)
	{
		StartCoroutine( Account.Instance.WWWCheckLoginCode( code ) );

	}

	public void SuccessfulLogin(bool successful, string code)
	{
		ProfilesManagementScript.Singleton.LoginCheckingDialogue.SetActive(false);

		if (successful) 
        {
			ProfilesManagementScript.Singleton.LoginUser.SetActive (false);
			ProfilesManagementScript.Singleton.AniminsScreen.SetActive (true);			
            NewUser.SetActive(false);
            AniminsScreen.SetActive(true);
            PlayerProfileData tempData = new PlayerProfileData();
            tempData = PlayerProfileData.CreateNewProfile(Account.Instance.UserName);
            tempData.UniqueID = code;
            ListOfPlayerProfiles.Add(tempData);
            
			CurrentProfile = tempData;
            AchievementManager.Instance.PopulateAchievements(true);
			
            CurrentAnimin = CurrentProfile.Characters[(int)CurrentProfile.ActiveAnimin];

			SaveAndLoad.Instance.SaveAllData();
			Debug.Log("Saved succesful login");
            ProfilesManagementScript.Singleton.AniminsScreen.SetActive(true);
            UnlockCharacterManager.Instance.CheckInitialCharacterUnlock();
		} 
		else 
		{
			ProfilesManagementScript.Singleton.NoSuchUserCodeDialogue.SetActive(true);	
		}

	}

    private void RefreshProfiles()
    {
        List<PlayerProfileData> profiles = SaveAndLoad.Instance.StateData.ProfileList;

        if(profiles != null)
        {
            //            Debug.Log(profiles.Count);
            //TempDebugPanel.text = profiles.Count.ToString();
            for(int i=0;i<profiles.Count;++i)
            {
                GameObject newProfile = (GameObject)Instantiate(PrefabProfile);
                newProfile.transform.parent = ProfilesRoot.transform;

                newProfile.transform.localScale = new Vector3(1,1,1);
                newProfile.transform.GetChild(1).GetComponent<Text>().text = profiles[i].ProfileName;
                newProfile.transform.localPosition = new Vector3(i * 180 + 360, 0, 0);
                newProfile.GetComponent<LoginUser>().ThisProfile = profiles[i];
            }
        }
        else
        {
            Debug.Log("No profiles found");
        }
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
#if UNITY_ANDROID
		Debug.Log("Set up timeout");
		Invoke ("CheckTimeout", 30);
#endif
    }

	private void CheckTimeout()
	{
		if(!ShopManager.Instance.ShopReady)
		{
			Debug.Log("GIAB Timeout");
			ShopManager.Instance.ShopReady = true;
			ProfilesManagementScript.Singleton.ContinueToInAppPurchase(false);
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
        if(resultId == "Card successfully activated")
        {
            UnlockCharacterManager.Instance.BuyCharacter(AniminToUnlockId, true);
        }
        else if(resultId == "Card number not valid")
        {
            LoadingSpinner.SetActive(false);
            AniminsScreen.SetActive(true);
        }
        else if(resultId == "Card number already used")
        {
            LoadingSpinner.SetActive(false);
            AniminsScreen.SetActive(true);
        }
        else if(resultId == "Animin already activated")
        {
            LoadingSpinner.SetActive(false);
            AniminsScreen.SetActive(true);
        }
        else if(resultId == "Something went wrong, please try again in a bit...")
        {
            LoadingSpinner.SetActive(false);
            AniminsScreen.SetActive(true);
        }
        else
        {
            Debug.Log("INVALID CODE RESPONSE");
            LoadingSpinner.SetActive(false);
            AniminsScreen.SetActive(true);
        }
    }

    public void ShowDemoCardPopup()
    {
        PurchaseChoiceScreen.SetActive(false);
        DemoCardPopup.SetActive(true);
        LoadingSpinner.SetActive(false);
    }

    public void CloseDemoCardPopup()
    {
        Debug.Log("Switching purchase to " + AniminToUnlockId);
        ItunesScript.SetCharacterIcons(AniminToUnlockId);
        DemoCardPopup.SetActive(false);
        PurchaseChoiceScreen.SetActive(true);
    }

    public void ShowEvolutionPurchaseWarning()
    {
        EvolveTboToAdultWarning = (GameObject)GameObject.Instantiate (Resources.Load("NGUIPrefabs/UI - EvolveBabyTboWarning"));
        EvolveTboToAdultWarning.SetActive( true );

    }

	public void CloseEvolutionPurchaseWarning(bool andContinue)
    {
        Destroy(EvolveTboToAdultWarning);

        if (andContinue)
        {
            SentToPurchaseAdultTBOFromMainScene = true;
            SaveAndLoad.Instance.SaveAllData();
            Application.LoadLevel(0);
        }
    }

    public void FasttrackThroughProfileSelect()
    {
        Debug.Log("Fast track initialised...");
        SelectProfile.SetActive(false);
        AniminsScreen.SetActive(false);
        LoadingSpinner.SetActive(true);
        AniminToUnlockId = PersistentData.TypesOfAnimin.TboAdult;
        if( Application.isEditor)
        {
            ProfilesManagementScript.Singleton.ContinueToInAppPurchase(true);
        }
        else
        {
            ActivateShopItemCheck();
        }
    }

    public void ContinueToInAppPurchase(bool shouldContinue)
    {
        ProfilesManagementScript.Singleton.LoadingSpinner.SetActive(false);
        if(shouldContinue)
        {
            ProfilesManagementScript.Singleton.PurchaseChoiceScreen.SetActive(true);
        }
        else
        {
            Debug.Log("Shop Unavailable");
            ProfilesManagementScript.Singleton.ErrorBox.SetActive(true);
        }

    }

    public void SendRealTimeNotification(string dataType, int amount)
    {
        StartCoroutine( Account.Instance.WWSendRealtimeNotification( dataType, amount.ToString() ) );
    }

    void OnLevelWasLoaded(int level)
    {
        SaveAndLoad.Instance.LoadAllData();
        Debug.Log(SentToPurchaseAdultTBOFromMainScene);
        if (level == 0 && SentToPurchaseAdultTBOFromMainScene)
        {
            Debug.Log("Level loaded");
            SentToPurchaseAdultTBOFromMainScene = false;
            FasttrackThroughProfileSelect();
        }

    }
	
	// Update is called once per frame
	void Update () 
	{
		if(BeginLoadLevel)
		{
			BeginLoadLevel = false;
			StartCoroutine(LoadLevel(@"VuforiaTest"));
			ProfilesManagementScript.Singleton.AniminsScreen.SetActive(false);
			ProfilesManagementScript.Singleton.LoadingScreen.SetActive(true);
		}
	}

	
	public IEnumerator LoadLevel(string name)
	{
		yield return new WaitForSeconds(0.1f);
		
		//nextLevel is one of the class fields
		AsyncOperation	nextLevel = Application.LoadLevelAsync(name);
		while (!nextLevel.isDone)
		{ 
			yield return new WaitForEndOfFrame(); 
		}       
	}
	

}
