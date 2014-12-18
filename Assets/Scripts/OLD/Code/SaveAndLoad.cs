using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using System.Xml.Serialization;

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
            XmlSerializer bf = new XmlSerializer(typeof(ProfileStateData));
			FileStream file = File.Open(Application.persistentDataPath + "/savedGames.anidat", FileMode.Open);
            StateData = (ProfileStateData)bf.Deserialize(file);
			file.Close();
			Debug.Log ("Save data loaded from " + Application.persistentDataPath + "/savedGames.anidat");
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
        StateData.ProfileList.Clear();
		Debug.Log ("Saving Profiles");
        for (int i =0; i< ProfilesManagementScript.Singleton.ListOfPlayerProfiles.Count; i++)
        {
            PlayerProfileData tempProfile = new PlayerProfileData();
            tempProfile = ProfilesManagementScript.Singleton.ListOfPlayerProfiles[i];
            StateData.ProfileList.Add(tempProfile);
			Debug.Log ("Adding profile " + i + ": " + tempProfile.ProfileName );
        }
        StateData.CurrentProfile = ProfilesManagementScript.Singleton.CurrentProfile;
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
        output += "\nCurrent Profile: " + StateData.CurrentProfile.ProfileName;
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
        }
        else
        {
            File.Copy(Application.persistentDataPath + "/savedGamesTemp.anidat", Application.persistentDataPath + "/savedGames.anidat");
        }

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
