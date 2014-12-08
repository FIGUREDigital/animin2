using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class ProfilesManagementScript : MonoBehaviour 
{
    private static bool m_Set = false;
    private static ProfilesManagementScript m_Singleton;
    public static ProfilesManagementScript Singleton
    {
        get
        {
            return m_Singleton;
        }
        set
        {
            Debug.Log("ProfilesManagementScript Singleton SET");
            if (value != null)
                m_Set = true;
            m_Singleton = value;
        }
    }
    public static bool isSet{ get { return m_Set; } }
	public static bool Initialized;

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

    public bool BeginLoadLevel;



	void Awake()
	{
		Singleton = this;
		if(!Initialized)
		{

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
        CharacterChoiceManager.Instance.Initialised = false;
        LoadProfileData();
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
        SaveAndLoad.Instance.SaveAllData();
		Debug.Log("just saved...new");
		CurrentProfile = tempData;
        AchievementManager.Instance.PopulateAchievements(true);		
        
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

			SaveAndLoad.Instance.SaveAllData();
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
        }
        else if(resultId == "Card number already used")
        {
        }
        else if(resultId == "Animin already activated")
        {
        }
        else if(resultId == "Something went wrong, please try again in a bit...")
        {
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
        if(shouldContinue)
        {
			UiPages.Next(Pages.PurchasePage);
        }
        else
        {
            Debug.Log("Shop Unavailable");
			//error box
			UiPages.Next(Pages.ConnectionErrorPage);
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
            Debug.Log("Loading New level");
            StartCoroutine(LoadLevel(@"ARBase"));
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
