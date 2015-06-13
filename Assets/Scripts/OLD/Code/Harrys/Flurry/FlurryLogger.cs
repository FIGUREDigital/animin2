using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_IOS || UNITY_ANDROID
using Prime31;
#endif

public class FlurryLogger
{
	
    bool m_Inited = false;
    //private string m_SessionTime = "SessionTime";
    //private string m_MainMenuStart = "MainMenuStart";
    //private string m_TimeInMinigame = "TimeInMinigame";

    public void Init()
    {
        if (m_Inited)
            return;
        Debug.Log("FLURRY LOGGER : [Init];");
        StartSession();

		
        m_Inited = true;
    }
    public void StartSession(){
        Debug.Log("FLURRY LOGGER : [Start];");

        #if UNITY_IOS
        FlurryAnalytics.startSession("VWK79FC9CHBVXY4VXXY6");
        FlurryAnalytics.setSessionReportsOnCloseEnabled(true);
        FlurryAnalytics.setSessionReportsOnPauseEnabled(true);
        #elif UNITY_ANDROID
        FlurryAnalytics.setContinueSessionMillis(20000);
        FlurryAnalytics.startSession("K98433N9XWCVX5NHTPCN", false);
        FlurryAnalytics.setLogEnabled(true);

        #endif


        var dict = new Dictionary<string,string>();
        dict.Add("DeviceModel", SystemInfo.deviceModel);

        #if UNITY_IOS
        FlurryAnalytics.logEventWithParameters("DeviceStats", dict, false);
        FlurryAnalytics.logEvent("SessionTime", true);
        #elif UNITY_ANDROID
        FlurryAnalytics.logEvent("DeviceStats", dict, false);
        FlurryAnalytics.logEvent("SessionTime", true);
        #endif
    }

    public void EndSession()
    {
        Debug.Log("FLURRY LOGGER : [End Session];");
        FlurryAnalytics.endTimedEvent("SessionTime");
        //ProfilesManagementScript.Singleton.SendRealTimeNotification("LoggedIn",-1);
    }



    private bool TimingMainScreen;

    public void StartMainScreenTimer()
    {
        Debug.Log("FLURRY LOGGER : [StartMainScreenTimer];");
        FlurryAnalytics.logEvent("MainMenuStart", true);
        TimingMainScreen = true;
    }

    public void EndMainScreenTimer()
    {
        Debug.Log("FLURRY LOGGER : [EndMainScreenTimer];");
        if (!TimingMainScreen)
            return;
        FlurryAnalytics.endTimedEvent("MainMenuStart");
        TimingMainScreen = false;
    }




    private bool TimingMinigame;

    public void StartMinigame(Minigame game)
    {
		
        Debug.Log("FLURRY LOGGER : [StartMinigame];");

        Dictionary<string,string> dict = new Dictionary<string,string>();
        dict.Add("Minigame", game.ToString());
        #if UNITY_IOS
            FlurryAnalytics.logEventWithParameters("TimeInMinigame", dict, true);
        #elif UNITY_ANDROID
        FlurryAnalytics.logEvent("TimeInMinigame", dict, true);
        #endif
        TimingMinigame = true;
    }

    public void EndMinigame()
    {
        if (!TimingMinigame)
            return;
        Debug.Log("FLURRY LOGGER : [EndMinigame];");

        Dictionary<string,string> dict = new Dictionary<string,string>();
        dict.Add("DateTime", DateTime.UtcNow.ToString("dd:MM:yyyy hh:mm"));
        #if UNITY_IOS
            FlurryAnalytics.logEventWithParameters("TimeInMinigame", dict, true);
        #elif UNITY_ANDROID
        FlurryAnalytics.logEvent("TimeInMinigame", dict, true);
        #endif

        TimingMinigame = false;
    }



    private bool TimingARCard;

    public void StartARCard()
    {
        Debug.Log("FLURRY LOGGER : [StartARCard];");
        #if UNITY_IOS
            FlurryAnalytics.logEvent("ARCardTime", false);
        #elif UNITY_ANDROID
        FlurryAnalytics.logEvent("ARCardTime", false);
        #endif
        TimingARCard = true;
    }

    public void EndARCard()
    {
        Debug.Log("FLURRY LOGGER : [EndARCard];");
        if (!TimingARCard)
            return;
        #if UNITY_IOS
            FlurryAnalytics.endTimedEvent("ARCardTime");
        #elif UNITY_ANDROID
        FlurryAnalytics.endTimedEvent("ARCardTime");
        #endif
        TimingARCard = false;
    }

    public void CharacterPurchasedIAP()
    {
        TimeSpan useLength = DateTime.UtcNow.Subtract(ProfilesManagementScript.Instance.CurrentProfile.Characters[0].CreatedOn);
        Debug.Log("FLURRY LOGGER : [ConversionTime];");

        Dictionary<string,string> dict = new Dictionary<string,string>();
        dict.Add("TimeToBuy", useLength.ToString());
        dict.Add("ItemBought", UnlockCharacterManager.Instance.ID.ToString());
        dict.Add("ItemBought", "In App Purchase");
        #if UNITY_IOS
            FlurryAnalytics.logEventWithParameters("ConversionTime", dict, false);
        #elif UNITY_ANDROID
        FlurryAnalytics.logEvent("ConversionTime", dict, false);
        #endif
    }

    public void CharacterPurchasedWeb()
    {
        TimeSpan useLength = DateTime.UtcNow.Subtract(ProfilesManagementScript.Instance.CurrentProfile.Characters[0].CreatedOn);
        Debug.Log("FLURRY LOGGER : [ConversionTime];");
		
        Dictionary<string,string> dict = new Dictionary<string,string>();
        dict.Add("TimeToBuy", useLength.ToString());
        dict.Add("ItemBought", UnlockCharacterManager.Instance.ID.ToString());
        dict.Add("ItemBought", "Webview");
        #if UNITY_IOS
            FlurryAnalytics.logEventWithParameters("ConversionTime", dict, false);
        #elif UNITY_ANDROID
        FlurryAnalytics.logEvent("ConversionTime", dict, false);
        #endif
    }


    private static FlurryLogger m_Instance;

    public static FlurryLogger Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = new FlurryLogger();
            }
            return m_Instance;
        }
    }
}
