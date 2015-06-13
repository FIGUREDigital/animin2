using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;

[System.Serializable]
public class PlayerProfileData
{

    public bool FirstTimePlaying;

    //private int FileVersion = 1;

    public PersistentData[] Characters = new PersistentData[(int)PersistentData.TypesOfAnimin.Count];
    public PlayerProfileSettings Settings = new PlayerProfileSettings();
    public string ProfileName;
    public string UniqueID;
    public PersistentData.TypesOfAnimin ActiveAnimin;
    public List<PersistentData.TypesOfAnimin> UnlockedAnimins = new List<PersistentData.TypesOfAnimin>();
    public List<AchievementManager.AchievementDetails> Achievements;

    public bool BuyFullTbo;

    public List<string> TutorialsCompleted = new List<string>();
	public List<int>highScores= new List<int>();

    public bool TutorialBoxLandPlayed
    {
        get
        {
            return m_TutorialBoxLandPlayed;
        }
        set
        {
            m_TutorialBoxLandPlayed = value;
        }
    }
    public bool TutorialCanonClashPlayed
    {
        get
        { 
            return m_TutorialCanonClashPlayed;
        }
        set
        {
            m_TutorialCanonClashPlayed = value;
        }
    }

    private bool m_TutorialBoxLandPlayed;
    private bool m_TutorialCanonClashPlayed;
    public bool m_StrawberryAdded = false;
    public bool m_PhoneAdded = false;
    public bool m_ShownStartEatStrawberry = false;
    public bool m_GivenZef = false;
    public bool m_GivenCrystals = false;
    public int m_PhoneMessage = 0;

    public static PlayerProfileData GetDefaultProfile()
    {
//        if(!Directory.Exists(Application.persistentDataPath + "/Profiles"))
//            return null;
//
//        if(!File.Exists(Application.persistentDataPath + "/Profiles/DefaultProfile"))
//            return null;
//
//        PlayerProfileData profile = new PlayerProfileData();
//        profile.Load("DefaultProfile");// ~TODO

        PlayerProfileData profile = new PlayerProfileData(); 

        return profile;
    }

    public static PlayerProfileData CreateNewProfile(string name)
    {
        PlayerProfileData profile = new PlayerProfileData();
        profile.ProfileName = name;
        for (int i = 0; i < profile.Characters.Length; ++i)
        {
            profile.Characters[i] = new PersistentData();
            profile.Characters[i].SetDefault((PersistentData.TypesOfAnimin)i);
            profile.Characters[i].AniminEvolutionId = AniminEvolutionStageId.Baby;
        }

        profile.Settings.AudioEnabled = true;
        profile.UnlockedAnimins.Add(PersistentData.TypesOfAnimin.Tbo);
        return profile;
    }

    /// <summary>No Description</summary>
    //    public static PlayerProfileData[] GetAllProfiles()
    //    {
    //        if(!Directory.Exists(Application.persistentDataPath + "/Profiles"))
    //            return null;
    //
    //        List<PlayerProfileData> allProfiles = new List<PlayerProfileData>();
    //
    //        string[] profileNames = Directory.GetFiles(Application.persistentDataPath + "/Profiles/");
    //        for(int i=0;i<profileNames.Length;++i)
    //        {
    //            Debug.Log(profileNames[i]);
    //            PlayerProfileData profile = new PlayerProfileData();
    //            profile.Load(Helpers.ExtraFilename(profileNames[i]));
    //            allProfiles.Add(profile);
    //        }
    //
    //        return allProfiles.ToArray();
    //    }

    /// <summary>No Description</summary>
    //    public void Save()
    //    {
    //        if(!Directory.Exists(Application.persistentDataPath + "/Profiles"))
    //            Directory.CreateDirectory(Application.persistentDataPath + "/Profiles");
    //
    //        string fileName = Application.persistentDataPath + "/Profiles/" + ProfileName;
    //
    //        using (FileStream writer = new FileStream(fileName, FileMode.Create))
    //        {
    //            using(BinaryWriter binaryWriter = new BinaryWriter(writer))
    //            {
    //                binaryWriter.Write(FileVersion);
    //
    //                binaryWriter.Write(UnlockAnimins.Count);
    //
    //                for(int i=0;i<UnlockAnimins.Count;++i)
    //                {
    //                    binaryWriter.Write((int)UnlockAnimins[i]);
    //                }
    //
    //                SaveLoadDictionary settingsdictionary = new SaveLoadDictionary();
    //                Settings.Write(settingsdictionary);
    //                settingsdictionary.WriteToBytes(binaryWriter);
    //
    //                binaryWriter.Write(Characters.Length);
    //                for(int i=0;i<Characters.Length;++i)
    //                {
    //                    SaveLoadDictionary dictionary = new SaveLoadDictionary();
    //                    Characters[i].Save(dictionary);
    //                    dictionary.WriteToBytes(binaryWriter);
    //                }
    //            }
    //        }
    //    }

    /// <summary>No Description</summary>
    //    public void Load(string profileName)
    //    {
    //        ProfileName = profileName;
    //        string fileName = Application.persistentDataPath + "/Profiles/" + profileName;
    //
    //        using (FileStream reader = new FileStream(fileName, FileMode.Open))
    //        {
    //            using(BinaryReader binaryReader = new BinaryReader(reader))
    //            {
    //                int fileversion = binaryReader.ReadInt32();
    //
    //                int unlockedAnimins = binaryReader.ReadInt32();
    //                for(int i=0;i<unlockedAnimins;++i)
    //                {
    //                    UnlockAnimins.Add((AniminId)binaryReader.ReadInt32());
    //                }
    //
    //                SaveLoadDictionary settingsdictionary = new SaveLoadDictionary(binaryReader);
    //                Settings.Load(settingsdictionary);
    //
    //                int charactersLength = binaryReader.ReadInt32();
    //
    //                for(int i=0;i<charactersLength;++i)
    //                {
    //                    SaveLoadDictionary dictionary = new SaveLoadDictionary(binaryReader);
    //                    Characters[i] = new PersistentData();
    //                    Characters[i].Load(dictionary);
    //                }
    //            }
    //        }
    //    }
}

//public class SaveLoadDictionary
//{
//	private Dictionary<string, string> Values = new Dictionary<string, string>();
//
//	public SaveLoadDictionary()
//	{
//	}
//
//	public SaveLoadDictionary(BinaryReader reader)
//	{
//		int count = reader.ReadInt32();
//		for(int i=0;i<count;++i)
//		{
//			string key = reader.ReadString();
//			string value = reader.ReadString();
//
//			Values.Add(key, value);
//		}
//	}
//
//	public void WriteToBytes(BinaryWriter writer)
//	{
//		string[] keys = new string[Values.Keys.Count];
//		string[] values = new string[Values.Values.Count];
//
//		Values.Keys.CopyTo(keys, 0);
//		Values.Values.CopyTo(values, 0);
//
//		writer.Write(Values.Count);
//		for(int i=0;i<Values.Count;++i)
//		{
//			writer.Write(keys[i]);
//			writer.Write(values[i]);
//		}
//
//	}
//
//	public void Write(string key, object value)
//	{
//		Values.Remove(key);
//		Values.Add(key, value.ToString());
//	}
//
//    public bool ReadAniminId(string key, ref PersistentData.TypesOfAnimin refValue)
//	{
//		int tempValue = 0;
//		if(ReadInt(key, ref tempValue))
//            refValue = (PersistentData.TypesOfAnimin)tempValue;
//
//		return false;
//	}
//
//	public bool ReadAniminEvolutionId(string key, ref AniminEvolutionStageId refValue)
//	{
//		int tempValue = 0;
//		if(ReadInt(key, ref tempValue))
//			refValue = (AniminEvolutionStageId)tempValue;
//
//		return false;
//	}
//
//
//
//	public bool ReadInt(string key, ref int refValue)
//	{
//		string existingValue = null;
//		if(Values.TryGetValue(key, out existingValue))
//		{
//			refValue = int.Parse(existingValue);
//			return true;
//		}
//
//		return false;
//	}
//
//	public bool ReadBool(string key, ref bool refValue)
//	{
//		string existingValue = null;
//		if(Values.TryGetValue(key, out existingValue))
//		{
//			refValue = bool.Parse(existingValue);
//			return true;
//		}
//
//		return false;
//	}
//
//	public bool ReadFloat(string key, ref float refValue)
//	{
//		string existingValue = null;
//		if(Values.TryGetValue(key,  out existingValue))
//		{
//			refValue = float.Parse(existingValue);
//			return true;
//		}
//
//		return false;
//	}
//
//
//	public bool ReadString(string key, ref string refValue)
//	{
//		string existingValue = null;
//		if(Values.TryGetValue(key,  out existingValue))
//		{
//			refValue = existingValue;
//			return true;
//		}
//
//		return false;
//	}
//}

[System.Serializable]
public class PlayerProfileSettings
{
    public bool AudioEnabled;

    public PlayerProfileSettings()
    {
    }

    //	public void Load(SaveLoadDictionary dictionary)
    //	{
    //		dictionary.ReadBool("AudioEnabled", ref AudioEnabled);
    //	}

    //	public void Write(SaveLoadDictionary dictionary)
    //	{
    //		dictionary.Write("AudioEnabled", AudioEnabled);
    //	}
}

/// <summary>No Description</summary>

		
