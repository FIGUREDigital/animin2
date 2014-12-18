﻿using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using System.Collections.Generic;

public class EvolutionManager
{
    public enum HappinessState
    {
        NotSet,
        Failing,
        Normal,
        Winning,
    }

    [SerializeField]
    private const string FILENAME = "Assets/Resources/MarkerGuide.xml";
    private const int MARKER_RATE = 20;
    private const int BABY_EVOLVE_THRESHOLD = 200;
    private const int KID_EVOLVE_THRESHOLD = 600;
    private const int ADULT_EVOLVE_THRESHOLD = 1400;
    private const int HAPPINESS_FAIL_THRESHOLD = 30;
    private const int HAPPINESS_WIN_THRESHOLD = 80;
    private const int HAPPINESS_BIG_WIN_THRESHOLD = 110;
    private const float TIME_FOR_REWARD = 600;
    private int mNextMarker = (int)(MARKER_RATE * 0.5f);
    private int mZefProgress;
    private int mCurrentMarker = 0;
    private string mReward;
    private AniminEvolutionStageId mCorrectStage;
    private HappinessState mHappinessState;
    private HappinessState mPrevHappinessState;
    private float mTimeInHappinessState;
    private bool mEvoStar = false;

    private struct UnlockItem
    {
        public InventoryItemId Id;
        public int Marker;

        public UnlockItem(InventoryItemId id, int marker)
        {
            Id = id;
            Marker = marker;
        }
    }

    UnlockItem[] m_Unlocks = new UnlockItem[]
    {
        new UnlockItem(InventoryItemId.Clock, 1),
        new UnlockItem(InventoryItemId.EDMJuno, 2),
        new UnlockItem(InventoryItemId.FartButton, 3),
        new UnlockItem(InventoryItemId.EDM808, 4),
        new UnlockItem(InventoryItemId.woodFrame, 5),
        new UnlockItem(InventoryItemId.EDMKsynth, 6),
        new UnlockItem(InventoryItemId.Camera, 7),
        new UnlockItem(InventoryItemId.Boombox, 8),
        new UnlockItem(InventoryItemId.woodSword, 9),
        new UnlockItem(InventoryItemId.Radio, 10),
        new UnlockItem(InventoryItemId.paperCalendar, 11)
    };


    public static List<string> mMarkers = new List<string>();

    public HappinessState HappyState
    {
        get
        {
            return mHappinessState;
        }
    }

    public int BabyEvolutionThreshold
    {
        get
        {
            return BABY_EVOLVE_THRESHOLD;
        }
    }

    public int KidEvolutionThreshold
    {
        get
        {
            return KID_EVOLVE_THRESHOLD;
        }
    }

    public int AdultEvolutionThreshold
    {
        get
        {
            return ADULT_EVOLVE_THRESHOLD;
        }
    }

    public int MarkerRate
    {
        get
        {
            return MARKER_RATE;
        }
    }

    public float HappinessStateTime
    {
        get
        {
            return mTimeInHappinessState;
        }
        set
        {
            mTimeInHappinessState = value;
        }
    }

    #region Singleton

    private static EvolutionManager s_Instance;

    public static EvolutionManager Instance
    {
        get
        {
            if (s_Instance == null)
            {
                s_Instance = new EvolutionManager();
                s_Instance.Init();
            }
            return s_Instance;
        }
    }

    #endregion

    public void Init()
    {
        while (ProfilesManagementScript.Singleton.CurrentAnimin.ZefTokens >= mNextMarker)
        {
            mNextMarker += MARKER_RATE;
            mEvoStar = !mEvoStar;
            mCurrentMarker++;
        }
    }

    public void UpdateEvo()
    {
        CheckHappiness();
    }

    public void AddZef()
    {
        AddZef(1);
    }

    public void AddZef(int amount)
    {
        int AmtToAdd = amount;
        AmtToAdd *= 3;
        Debug.Log("Adding Zef : [" + AmtToAdd + "];");
        ProfilesManagementScript.Singleton.CurrentAnimin.ZefTokens += AmtToAdd;
        ZefChanged();
    }

    public void RemoveZef()
    {
        RemoveZef(1);
    }

    public void RemoveZef(int amount)
    {
        int AmtToSub = amount;
        AmtToSub *= 3;
        ProfilesManagementScript.Singleton.CurrentAnimin.ZefTokens -= AmtToSub;
        if (ProfilesManagementScript.Singleton.CurrentAnimin.ZefTokens < 0)
        {
            ProfilesManagementScript.Singleton.CurrentAnimin.ZefTokens = 0;
        }
        ZefChanged();
    }

    private void ZefChanged()
    {
        while (ProfilesManagementScript.Singleton.CurrentAnimin.ZefTokens >= mNextMarker)
        {
            Debug.Log("ZefTokens : [" + ProfilesManagementScript.Singleton.CurrentAnimin.ZefTokens + "]; Next Marker : " + mNextMarker + "];");
            mNextMarker += MARKER_RATE;
            if (mMarkers.Count > mCurrentMarker)
            {
                mReward = mMarkers[mCurrentMarker];
            }
            AchievementsScript.Singleton.Show(mEvoStar ? AchievementTypeId.EvolutionStar : AchievementTypeId.EvolutionExclamation, 0);
            mEvoStar = !mEvoStar;
            mCurrentMarker++;

            for (int i = 0; i < m_Unlocks.Length; i++)
            {
                if (m_Unlocks[i].Marker == mCurrentMarker)
                {
                    GameObject chest = UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>().GetAndSpawnChests(4);
                    chest.GetComponent<EvolutionChestItem>().id = m_Unlocks[i].Id;
                    break;
                }
            }
        }
        //if (ProfilesManagementScript.Singleton.CurrentProfile.ActiveAnimin == PersistentData.TypesOfAnimin.Tbo)
        if (false)
        {
            //ProfilesManagementScript.Singleton.ShowEvolutionPurchaseWarning();
        }
        else
        {
            CheckEvolution();
        }
        UpdateEvoBar();

    }

    private void UpdateEvoBar()
    {
        Debug.Log("Updating Evo bar!");
        int min = 0;
        int max = 0;
        int curZef = 0;
        curZef = ProfilesManagementScript.Singleton.CurrentAnimin.ZefTokens;
        AniminEvolutionStageId stage = ProfilesManagementScript.Singleton.CurrentAnimin.AniminEvolutionId;
        switch (stage)
        {
            case AniminEvolutionStageId.Baby:
                min = 0;
                max = BABY_EVOLVE_THRESHOLD;
                break;
            case AniminEvolutionStageId.Kid:
                min = BABY_EVOLVE_THRESHOLD;
                max = KID_EVOLVE_THRESHOLD;
                break;
            case AniminEvolutionStageId.Adult:
                min = KID_EVOLVE_THRESHOLD;
                max = ADULT_EVOLVE_THRESHOLD;
                break;
            default:
                break;
        }

        curZef -= min;
        int diff = max - min;
        float percentage = (((float)curZef) / ((float)diff)) * 100f;

        Debug.Log("Evo bar should be : [" + percentage + "]; full! Min/Max : [" + min + "/" + max + "];");

        ProfilesManagementScript.Singleton.CurrentAnimin.Evolution = percentage;
    }

    private void CheckHappiness()
    {
        float happiness = ProfilesManagementScript.Singleton.CurrentAnimin.Happy;
        if (happiness < HAPPINESS_FAIL_THRESHOLD)
        {
            mHappinessState = HappinessState.Failing;
        }
        else if (happiness < HAPPINESS_WIN_THRESHOLD)
        {
            mHappinessState = HappinessState.NotSet;
        }
        else if (happiness < HAPPINESS_BIG_WIN_THRESHOLD)
        {
            mHappinessState = HappinessState.Normal;
        }
        else if (happiness > HAPPINESS_BIG_WIN_THRESHOLD)
        {
            mHappinessState = HappinessState.Winning;
        }

        if (mHappinessState != mPrevHappinessState)
        {
            mTimeInHappinessState = 0;
            mPrevHappinessState = mHappinessState;
        }
        mTimeInHappinessState += Time.deltaTime;

        if (mTimeInHappinessState > TIME_FOR_REWARD)
        {
            mTimeInHappinessState = 0;
            switch (mHappinessState)
            {
                case HappinessState.Failing:
                    RemoveZef(1);
                    Debug.Log("Happy Fail! Zef Removed");
                    break;
                case HappinessState.Normal:
                    AddZef(1);
                    Debug.Log("Happy Normal, Zef Added");
                    break;
                case HappinessState.Winning:
                    AddZef(3);
                    Debug.Log("Happy Win! 3 Zef Added");
                    break;
				
                case HappinessState.NotSet:
                default:
                    break;
            }
        }
    }

    private void CheckEvolution()
    {
        int zef = ProfilesManagementScript.Singleton.CurrentAnimin.ZefTokens;
        AniminEvolutionStageId stage = ProfilesManagementScript.Singleton.CurrentAnimin.AniminEvolutionId;
        AniminEvolutionStageId correctStage = AniminEvolutionStageId.Count;
        if (zef < BABY_EVOLVE_THRESHOLD)
        {
            correctStage = AniminEvolutionStageId.Baby;
        }
        else if (zef < KID_EVOLVE_THRESHOLD)
        {
            correctStage = AniminEvolutionStageId.Kid;
        }
        else if (zef < ADULT_EVOLVE_THRESHOLD)
        {
            correctStage = AniminEvolutionStageId.Adult;
        }
        else
        {
            correctStage = AniminEvolutionStageId.Adult;
        }
        if (stage != correctStage)
        {
            mCorrectStage = correctStage;
            if (stage < correctStage)
            {
                Evolve();
            }
            if (stage > correctStage)
            {
                Devolve();
            }
        }
    }

    private void Evolve()
    {
        ProfilesManagementScript.Singleton.CurrentAnimin.AniminEvolutionId = mCorrectStage;
        UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterSwapManagementScript>().LoadCharacter(ProfilesManagementScript.Singleton.CurrentAnimin.PlayerAniminId, mCorrectStage);
        AchievementsScript.Singleton.Show(AchievementTypeId.Evolution, 100);
    }

    private void Devolve()
    {
        ProfilesManagementScript.Singleton.CurrentAnimin.AniminEvolutionId = mCorrectStage;
        UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterSwapManagementScript>().LoadCharacter(ProfilesManagementScript.Singleton.CurrentAnimin.PlayerAniminId, mCorrectStage);
    }

    public List<string> Deserialize()
    {
        List<string> data = null;
		
        XmlSerializer serializer = new XmlSerializer(typeof(List<string>));
		
        StreamReader reader = new StreamReader(FILENAME);
        data = (List<string>)serializer.Deserialize(reader);
        reader.Close();
		
        return data;
    }

    public void Serialize()
    {
        XmlSerializer ser = new XmlSerializer(typeof(List<string>));
        TextWriter writer = new StreamWriter(FILENAME);
        ser.Serialize(writer, mMarkers);
        writer.Close();
    }
}
