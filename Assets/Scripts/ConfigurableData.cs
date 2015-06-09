using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Xml.Serialization;

public class ConfigurableData : Phi.SingletonMonoBehaviour<ConfigurableData> {

	public enum NotificationTypes
	{
		Time,
		Health,
		Hungry,
		Fitness
	}

	
	[System.Serializable]
	public class Notification
	{
		public NotificationTypes type;
		public float compare;
		public string body;
		public string action;
		public Notification()
		{
		}

		public Notification(NotificationTypes type, float compare, string body, string action)
		{
			this.type = type;
			this.compare = compare;
			this.body = body;
			this.action = action;
		}

		public float GetNotifyTime(PersistentData animin)
		{
			switch (type) {
			case NotificationTypes.Time:
				if(animin == null)
				{
					return compare;
				}
				else
				{
					return 0;
				}
			case NotificationTypes.Fitness:
				return GetNotifyTime(ConfigurableData.Instance.data.fitness, animin.Fitness);
			case NotificationTypes.Health:
				return GetNotifyTime(ConfigurableData.Instance.data.health, animin.Health);
			case NotificationTypes.Hungry:
				return GetNotifyTime(ConfigurableData.Instance.data.hungry, animin.Hungry);
			}
			return 0;
		}

		public float GetNotifyTime(List<DeltaRate> deltas, float curValue)
		{
			if (curValue < compare)
				return 0;	// Already too late
			// Step through deltas and see when value will be less than compare
			float time = 0;
			foreach (DeltaRate d in deltas) 
			{
				float nextValue = curValue;
				if (nextValue > d.minimum)
				{
					nextValue += d.change;
					if (nextValue < d.minimum)
					{
						nextValue = d.minimum;
					}
				}
				if (nextValue < compare)
				{
					// Should trigger in this delta
					float dist = compare - curValue;
					float duration = d.minutes * (d.change / dist);
					time += duration;
					return time;
				}
				else
				{
					time += d.minutes;
				}
				curValue = nextValue;
			}
			return 0;	// Won't occur
		}

		public void Add(PersistentData animin)
		{			
			float minutes = GetNotifyTime(animin);
			if(minutes > 1)
			{
				UtilsLocalNotification notification = new UtilsLocalNotification ();
				
				notification.fireDate = DateTime.Now.AddMinutes(minutes);
				notification.alertAction = action;
				notification.alertBody = body;	
				notification.hasAction = true;
			}
		}
	}

	[System.Serializable]   
	public class DeltaRate
	{
		public float minutes;
		public float change;
		public float minimum;
		public string comment;
		public DeltaRate()
		{
		}
		public DeltaRate(float minutes, float change, float minimum, string comment = "")
		{
			this.minutes = minutes;
			this.change = change;
			this.minimum = minimum;
			this.comment = comment;
		}
	}
		
	[System.Serializable]
	public class GameData
	{
		public string version;
		public List<DeltaRate> health = new List<DeltaRate>(); 
		public List<DeltaRate> hungry = new List<DeltaRate>();
		public List<DeltaRate> fitness = new List<DeltaRate>();
		public List<Notification> notifications = new List<Notification>();
		public void SetAsDefault()
		{
			version = "Default";
			health.Clear ();
			fitness.Clear ();
			hungry.Clear ();
			health.Add (new DeltaRate(5,-10,30, "-10 over first 5 minutes"));
			health.Add (new DeltaRate(720,-10,30, "-10 over next 12 hours"));
			health.Add (new DeltaRate(720,0,30, "Followed by none for 12 hours"));
			health.Add (new DeltaRate(7200,-10,30, "Followed by -10 over for 5 days"));
			hungry.Add (new DeltaRate(5,-10,30, "-10 over first 5 minutes"));
			fitness.Add (new DeltaRate(5,-10,30, "-10 over first 5 minutes"));
			notifications.Add (new Notification(NotificationTypes.Time, 1, "It's been 1 minute since you last played Animin", "Play"));
			notifications.Add (new Notification(NotificationTypes.Hungry, 50, "Your Animin is rather hungry", "Feed"));
			notifications.Add (new Notification(NotificationTypes.Hungry, 31, "Your Animin is starving", "Feed"));
		}
    }

	public GameData data;

	public override void Init()
	{
		DoApplicationFocus (false);
		if(File.Exists(Application.persistentDataPath + "/gamedata.dat")) 
		{
			XmlSerializer bf = new XmlSerializer(typeof(GameData));
			FileStream file = File.Open(Application.persistentDataPath + "/gamedata.dat", FileMode.Open);
			data = (GameData)bf.Deserialize(file);
			file.Close();
			Debug.Log ("Config data loaded from " + Application.persistentDataPath + "/gamedata.dat");
		}
		else
		{
			data = new GameData ();	
			data.SetAsDefault();
			Save ();
			Debug.Log ("No Config data fond at" + Application.persistentDataPath + "/gamedata.dat");
        }
		// And kick off a download to allow updating
		//www
		StartCoroutine (UpdateStats ());
	}
	
	void OnApplicationPause(bool pause)
	{
		DoApplicationFocus(pause);
	}

	void DoApplicationFocus(bool pause)
	{
		Debug.Log ("DoApplicationFocus " + pause);
		UtilsLocalNotification.CancelAllLocalNotifications();
		if (pause) {
			SetupNotifications();
		}
		else {
			UtilsLocalNotification.AskUserForPermission();
		}
	}

	void SetupNotifications()
	{
		foreach(Notification n in data.notifications)
		{
			if (n.type == NotificationTypes.Time)
			{
				n.Add(null);
			}
			else if(ProfilesManagementScript.Instance.CurrentAnimin != null)
			{
				n.Add(ProfilesManagementScript.Instance.CurrentAnimin);
			}
		}
		UtilsLocalNotification.Submit ("Animin");

	}

	IEnumerator UpdateStats()
	{
		WWW www = new WWW ("https://www.dropbox.com/s/mb7kux6ol6lta1j/gamedata.xml?dl=1");
		
		yield return www;
		
		if (www.error != null) {
			Debug.Log (www.error);
		} else {
			using (TextReader reader = new StringReader(www.text))
			{
				XmlSerializer bf = new XmlSerializer(typeof(GameData));
				data = (GameData)bf.Deserialize(reader);
				Save ();
				Debug.Log ("Updated GameData "+data.version);
			}
		}
    }
    
    public void Save()
	{
		Debug.Log("Save Config");		
		XmlSerializer bf = new XmlSerializer(typeof(GameData));
		FileStream file = File.Create (Application.persistentDataPath + "/gamedatatemp.dat");
		bf.Serialize(file, data);
		file.Close();
		if (File.Exists(Application.persistentDataPath + "/gamedata.dat"))
		{
			Debug.Log("Deleting old");
			File.Delete(Application.persistentDataPath + "/gamedatabackup.dat");
			File.Replace(Application.persistentDataPath + "/gamedatatemp.dat", Application.persistentDataPath + "/gamedata.dat", Application.persistentDataPath + "/gamedatabackup.anidat");
			#if UNITY_IOS
			UnityEngine.iOS.Device.SetNoBackupFlag(Application.persistentDataPath + "/gamedatabackup.dat");
			#endif
		}
		else
        {
			File.Copy(Application.persistentDataPath + "/gamedatatemp.dat", Application.persistentDataPath + "/gamedata.dat");
		}
		#if UNITY_IOS
		UnityEngine.iOS.Device.SetNoBackupFlag(Application.persistentDataPath + "/gamedata.dat");
		#endif
    }

	public void UpdateAfterPause(float minutes)
	{
		Debug.Log ("Update after " + minutes);
		ProfilesManagementScript.Instance.CurrentAnimin.Health = UpdateStat (ProfilesManagementScript.Instance.CurrentAnimin.Health, data.health, minutes, "Health");
		ProfilesManagementScript.Instance.CurrentAnimin.Hungry = UpdateStat (ProfilesManagementScript.Instance.CurrentAnimin.Hungry, data.hungry, minutes, "Hungry");
		ProfilesManagementScript.Instance.CurrentAnimin.Fitness = UpdateStat (ProfilesManagementScript.Instance.CurrentAnimin.Fitness, data.fitness, minutes, "Fitness");
	}

	float UpdateStat(float curValue, List<DeltaRate> deltas, float duration, string comment)
	{
		foreach (DeltaRate delta in deltas) 
		{
			float startValue = curValue;
			float d = delta.change;
			if (duration >= delta.minutes)
			{
				duration -= delta.minutes;
			}
			else
			{
				d *= duration / delta.minutes;
				duration = 0;
			}
			if (curValue > delta.minimum)
			{
				curValue = curValue + d;
				if (curValue < delta.minimum)
				{
					curValue = delta.minimum;
				}
				Debug.Log ("UpdateStat "+comment+" ["+delta.comment+"] changed from "+startValue+" to "+curValue);
			}
			if (duration <= 0)
			{
				break;
			}
		}
		return curValue;
	}

}
