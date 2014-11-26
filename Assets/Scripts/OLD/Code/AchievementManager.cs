using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AchievementManager  
{
	public enum Achievements
	{
		EatFruit = 0,
		PlayMinigames,
		PlayMusic,
		ArMode,
		Heal,
		Count
	}
	[System.Serializable]
	public class AchievementDetails
	{
		public Achievements Type;
		public bool Status;
	}

	public List<AchievementDetails> ListOfAchievements = new List<AchievementDetails>();

	private int mCompletedAchievements;
	private int[] mCount = new int[(int)Achievements.Count];
	private int[] mRequiredCount = new int[(int)Achievements.Count]{10,5,3,1,3};
	private bool[] mAchievmentsFired = new bool[(int)Achievements.Count];
	private const string mDescription = "Congratulations, you unlocked the achievement \n{0}!"; 
	private const string mFruit = "Devour 10 pieces of juicy fruit"; 
	private const string mGames = "Push yourself to the limit with 5 games"; 
	private const string mMusic = "Create experimental music 3 times"; 
	private const string mArMode = "Use the AR card to unleash your Animin on the world"; 
	private const string mHeal = "Care for your Animin 3 times"; 

	public int CompletedAchievements
	{
		get
		{
			return mCompletedAchievements;
		}
	}
	public bool Achieved(int i)
	{
		return mAchievmentsFired[i];
	}

	public string Description(int i)
	{
		return Description((Achievements)i);
	}
	public string Description(Achievements item)
	{
		string achievement = "";
		switch(item)
		{
		case Achievements.EatFruit:
			achievement = mFruit;
			break;
		case Achievements.PlayMinigames:
			achievement = mGames;
			break;
		case Achievements.PlayMusic:
			achievement = mMusic;
			break;
		case Achievements.ArMode:
			achievement = mArMode;
			break;
		case Achievements.Heal:
			achievement = mHeal;
			break;
		default:
			break;
			
		}

		return achievement;
	}

	#region Singleton
	
	private static AchievementManager s_Instance;
	
	public static AchievementManager Instance
	{
		get
		{
			if ( s_Instance == null )
			{
				s_Instance = new AchievementManager();
//				s_Instance.PopulateAchievements();
			}
			return s_Instance;
		}
	}
	
	#endregion

	private void ResetAchievments()
	{
		mCompletedAchievements = 0;

		PlayerPrefs.SetString("AchievementsActive","false");
		for(int i =0; i < (int)Achievements.Count; i++)
		{
			AchievementDetails tempAchievement = new AchievementDetails ();
			tempAchievement.Type = (Achievements)i;
			tempAchievement.Status = false;
			if(ListOfAchievements == null)
			{
				ListOfAchievements = new List<AchievementDetails>();
			}
			ListOfAchievements.Add (tempAchievement);
            ProfilesManagementScript.Singleton.CurrentProfile.Achievements = ListOfAchievements;
			PlayerPrefs.SetInt("Achievement" + i, 0);
			PlayerPrefs.SetInt("AchievementFired" + i, 0);
		}
		PlayerPrefs.Save();
	}

    public void PopulateAchievements(bool newUser)
	{
//		bool po = "true" == PlayerPrefs.GetString("AchievementsActive");

        if (ListOfAchievements!=null)ListOfAchievements.Clear ();

        if(newUser)
		{
			ResetAchievments();
		}

        else
        {
            ListOfAchievements = ProfilesManagementScript.Singleton.CurrentProfile.Achievements;
        }

//		for(int i =0; i < (int)Achievements.Count; i++)
//		{
//
//			mCount[i] = PlayerPrefs.GetInt("Achievement" + i);
//			bool fired = PlayerPrefs.GetInt("AchievementFired" + i) == 1;
//			mAchievmentsFired[i] = fired;
//			mCompletedAchievements += fired?1:0;
//
//			AchievementDetails tempAchievement = new AchievementDetails ();
//			tempAchievement.Type = (Achievements)i;
//			tempAchievement.Status = fired;
//			ListOfAchievements.Add (tempAchievement);
//
//		}

	}

	public void SaveAchievments()
	{
		PlayerPrefs.SetString("AchievementsActive","true");
		for(int i =0; i < (int)Achievements.Count; i++)
		{
			PlayerPrefs.SetInt("Achievement" + i, mCount[i]);
			PlayerPrefs.SetInt("AchievementFired" + i, mAchievmentsFired[i]?1:0);
		}
		PlayerPrefs.Save();
        ProfilesManagementScript.Singleton.CurrentProfile.Achievements = ListOfAchievements;
	}

	public void AddToAchievment(Achievements item)
	{
		Debug.Log("ACHIEVEMENT MANAGER: Added achievement part for " + item);
		mCount[(int)item]++;
		CheckAchievments();
	}

	private void CheckAchievments()
	{
		for(int i = 0; i < (int)Achievements.Count; i++)
		{
			if(mCount[i] >= mRequiredCount[i] && !mAchievmentsFired[i])
			{
				mCompletedAchievements++;
				FireAchievment((Achievements)i);
				mAchievmentsFired[i] = true;
			}
		}
        ProfilesManagementScript.Singleton.CurrentProfile.Achievements = ListOfAchievements;
	}


	private void FireAchievment(Achievements item)
	{
	
		string achievement = Description(item);

		AchievementsScript.Singleton.Show(AchievementTypeId.Achievement,1000);
		AchievementsScript.Singleton.Description.text = string.Format(mDescription, achievement);	                                                       ;
	}
}
