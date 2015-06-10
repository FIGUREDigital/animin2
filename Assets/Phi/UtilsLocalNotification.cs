using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UtilsLocalNotification 
{	
    public System.DateTime		fireDate;
    public string		alertBody;
	public string		alertAction;
	public bool			hasAction = false;
	public int			applicationIconBadgeNumber = 0;
	
	static List<UtilsLocalNotification> allNotifications = new List<UtilsLocalNotification>();
#if UNITY_IPHONE
	UnityEngine.iOS.LocalNotification	localN;
#endif
#if UNITY_ANDROID
	int			id;
#endif
	
	public UtilsLocalNotification()
	{
		allNotifications.Add(this);
//		Debug.Log("Num notifications="+allNotifications.Count);
	}

	~UtilsLocalNotification()
	{
//		Debug.Log("Remove="+allNotifications.Count);
		allNotifications.Remove(this);
#if UNITY_IPHONE
		localN = null;
#endif		
	}

	public static void AskUserForPermission()
	{
#if UNITY_IPHONE
		UnityEngine.iOS.NotificationServices.RegisterForNotifications(UnityEngine.iOS.NotificationType.Alert, false);
#endif
	}
	
	public static void Submit(string gameName)
	{
		allNotifications.Sort((a, b) => System.DateTime.Compare(a.fireDate, b.fireDate));
		// clear all existing notifications
		CancelAllLocalNotifications(false);
#if UNITY_EDITOR
/*		foreach (UtilsLocalNotification n in allNotifications)
		{			
			Debug.Log("Notification "+n.alertBody+" "+n.fireDate+" "+n.applicationIconBadgeNumber);
		}*/
#endif
#if UNITY_IPHONE
//		Debug.Log("Submit iPhone"+ allNotifications.Count);
		
		// Calculate what the badge number should be at the given time.
		int badgeTotal = 0;
		foreach (UtilsLocalNotification n in allNotifications)
		{		
//			Debug.Log(n.alertAction+", "+n.alertBody+", "+n.fireDate);
			if(n.localN == null)
			{
				n.localN = new UnityEngine.iOS.LocalNotification();
			}
			n.localN.fireDate = n.fireDate;
			n.localN.alertBody = n.alertBody;
			n.localN.alertAction = n.alertAction;
			n.localN.hasAction = n.hasAction;
			n.localN.fireDate = n.fireDate;
			badgeTotal += n.applicationIconBadgeNumber;
			n.localN.applicationIconBadgeNumber = badgeTotal;
			Debug.Log("Notification "+n.alertBody+" "+n.fireDate+" "+n.applicationIconBadgeNumber);
			UnityEngine.iOS.NotificationServices.ScheduleLocalNotification(n.localN);
		}	
#endif
		
#if UNITY_ANDROID
		int total = 0;
		for (int i = 0; i < allNotifications.Count; i++)
        {
            UtilsLocalNotification n = allNotifications[i];
            Debug.Log("Notification " + n.alertBody + " " + n.fireDate);
			int seconds = (int)((n.fireDate - System.DateTime.Now).TotalSeconds);
			if(seconds > 0)
			{
				int id = EtceteraAndroid.scheduleNotification(seconds, gameName, n.alertBody, n.alertBody, "");
				
				Debug.Log("Notification "+n.alertBody+" "+seconds+"s ");
				PlayerPrefs.SetInt("LNotif"+total,id);
				total++;
			}
		}		
		PlayerPrefs.SetInt("LNotifNumOf",total);
		PlayerPrefs.Save();
#endif
	}
	
	public static void CancelAllLocalNotifications(bool includeOurList = true)
	{
#if UNITY_IPHONE
		UnityEngine.iOS.NotificationServices.CancelAllLocalNotifications();
#endif

		
#if UNITY_ANDROID
		int num = PlayerPrefs.GetInt("LNotifNumOf",0);
		for(int i = 0; i < num ; i++)
		{
			int id = PlayerPrefs.GetInt("LNotif"+i,0);
			EtceteraAndroid.cancelNotification(id);
		}
		PlayerPrefs.SetInt("LNotifNumOf",0);
        PlayerPrefs.Save();
#endif
		if(includeOurList)
		{
			allNotifications.Clear();
		}
	}
	
}
