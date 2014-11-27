using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class SaveAndLoad {

	#region Singleton
	
	private static SaveAndLoad s_Instance;
	
	public static SaveAndLoad Instance
	{
		get
		{
			if( s_Instance == null )
			{
				s_Instance = new SaveAndLoad();
			}
			return s_Instance;
		}
	}
	
	#endregion
    [System.Serializable]
    public class ProfileStateData
    {
        public List<PlayerProfileData> ProfileList; 
        public PlayerProfileData CurrentProfile;
        public bool UpgradeTbo;
    }

    public ProfileStateData StateData;

    public void Awake()
    {
        Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes");
    }

	public SaveAndLoad()
	{
        StateData = new ProfileStateData();

        StateData.ProfileList = new List<PlayerProfileData> ();
//		PlayerPrefs.DeleteAll ();
	}

	public void LoadAllData()
	{
		
//        File.Delete(Application.persistentDataPath + "/savedGames.aniÍdat");

        if(File.Exists(Application.persistentDataPath + "/savedGames.anidat")) 
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/savedGames.anidat", FileMode.Open);
            StateData = (ProfileStateData)bf.Deserialize(file);
			file.Close();
			Debug.Log ("Save data loaded");
            RepopulateData();
		}
		else
		{
						/*
            ProfilesManagementScript.Singleton.SendRealTimeNotification("Downloads",1);
            #if UNITY_IOS
            ProfilesManagementScript.Singleton.SendRealTimeNotification("IOS",1);
            #elif UNITY_ANDROID
            ProfilesManagementScript.Singleton.SendRealTimeNotification("Android",1);
            #endif
            */
		}
	}

	public void SaveAllData()
	{
        File.Delete(Application.persistentDataPath + "/savedGames.anidat");
        StateData.ProfileList.Clear();
        for (int i =0; i< ProfilesManagementScript.Singleton.ListOfPlayerProfiles.Count; i++)
        {
            PlayerProfileData tempProfile = new PlayerProfileData();
            tempProfile = ProfilesManagementScript.Singleton.ListOfPlayerProfiles[i];
            StateData.ProfileList.Add(tempProfile);
        }
        StateData.CurrentProfile = ProfilesManagementScript.Singleton.CurrentProfile;
        StateData.UpgradeTbo = ProfilesManagementScript.Singleton.SentToPurchaseAdultTBOFromMainScene;
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create (Application.persistentDataPath + "/savedGames.anidat");
        bf.Serialize(file, StateData);

		file.Close();

	}	

//    public List<PlayerProfileData> LoadProfileData()
//    {
//        ProfileStateData tempstate = new ProfileStateData();
//
//        tempstate.ProfileList = new List<PlayerProfileData> ();
//
//        for (int i =0; i< ProfilesManagementScript.Singleton.ListOfPlayerProfiles.Count; i++)
//        {
//            tempProfile.Add(ProfileList[i]);
//        }
//
//        return tempProfile;
//    }

	public void RepopulateData()
	{
        ProfilesManagementScript.Singleton.ListOfPlayerProfiles.Clear();

        for (int i =0; i< StateData.ProfileList.Count; i++)
        {
            ProfilesManagementScript.Singleton.ListOfPlayerProfiles.Add(StateData.ProfileList[i]);            
        }
        ProfilesManagementScript.Singleton.CurrentProfile = StateData.CurrentProfile;
        ProfilesManagementScript.Singleton.SentToPurchaseAdultTBOFromMainScene = StateData.UpgradeTbo;

	}
}
