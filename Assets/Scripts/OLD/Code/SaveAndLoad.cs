using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using System.Xml.Serialization;
/*
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


	public SaveAndLoad()
	{
        StateData = new ProfileStateData();

        StateData.ProfileList = new List<PlayerProfileData> ();
	//	PlayerPrefs.DeleteAll ();
	}


	public void SaveAllData()
	{
        StateData.ProfileList.Clear();
        #if UNITY_IOS
        UnityEngine.iOS.Device.SetNoBackupFlag(Application.persistentDataPath);
        UnityEngine.iOS.Device.SetNoBackupFlag(Application.persistentDataPath + "/unity.txt");
        UnityEngine.iOS.Device.SetNoBackupFlag(Application.persistentDataPath + "/storeKitReceipts.archive");
        #endif

		Debug.Log ("Saving Profiles");
        for (int i =0; i< ProfilesManagementScript.Instance.ListOfPlayerProfiles.Count; i++)
        {
            PlayerProfileData tempProfile = new PlayerProfileData();
			tempProfile = ProfilesManagementScript.Instance.ListOfPlayerProfiles[i];
            StateData.ProfileList.Add(tempProfile);
			Debug.Log ("Adding profile " + i + ": " + tempProfile.ProfileName );
        }
		StateData.CurrentProfile = ProfilesManagementScript.Instance.CurrentProfile;
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

}*/
