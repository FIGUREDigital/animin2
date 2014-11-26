using UnityEngine;
using System.Collections;


public class MultipleCardTrackerScript : MonoBehaviour, ITrackableEventHandler
{
	private TrackableBehaviour mTrackableBehaviour;
		
	void Start()
	{
		mTrackableBehaviour = GetComponent<TrackableBehaviour>();
		if (mTrackableBehaviour)
		{
			mTrackableBehaviour.RegisterTrackableEventHandler(this);
		}

	}
		
	public void OnTrackableStateChanged(
		TrackableBehaviour.Status previousStatus,
		TrackableBehaviour.Status newStatus)
	{
		if (WebCamTexture.devices.Length != 0 && (newStatus == TrackableBehaviour.Status.DETECTED || newStatus == TrackableBehaviour.Status.TRACKED || newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED))
		{
			OnTrackingFound();
		}
		else
		{
			OnTrackingLost();
		}
	}
		
	void Update()
	{

	}
	
	
	void LateUpdate()
	{
		
		
	}
	
			
	private void OnTrackingFound()
	{
		AchievementManager.Instance.AddToAchievment(AchievementManager.Achievements.ArMode);
		/*
		 * I hate this switch statement. It's so ugly. Note to future developers. If you have more than a few days to update and work on this code, write some kind of handler
		 * for all these different classes. Better than that, get rid of the classes and work out some way to work around the issues that caused those classes to exist.
		 * Oh, who am I kidding, nobody's going to read this. Let me know if you're reading this.
		 */

		switch(UIGlobalVariablesScript.Singleton.CurrentlyActive){
			case UIGlobalVariablesScript.ActiveState.Caring:
			{
				TrackVuforiaScript script = GameObject.FindObjectOfType<TrackVuforiaScript>();
				script.OnTrackingFound();
				break;
			}
			case UIGlobalVariablesScript.ActiveState.Collecting:
		{
				TrackVFMG1 script = GameObject.FindObjectOfType<TrackVFMG1>();
				script.OnTrackingFound();
				break;
		}
			case UIGlobalVariablesScript.ActiveState.Gun:
			{
				TrackVFMG2 script = GameObject.FindObjectOfType<TrackVFMG2>();
				script.OnTrackingFound();
				break;
			}
		}
	}
	
	
	public void OnTrackingLost()
	{
		switch(UIGlobalVariablesScript.Singleton.CurrentlyActive){
			case UIGlobalVariablesScript.ActiveState.Caring:
		{
				TrackVuforiaScript.IsTracking = false;
				TrackVuforiaScript script = GameObject.FindObjectOfType<TrackVuforiaScript>();
				if (script == null) break;
				script.OnTrackingLost();
				break;
			}
			case UIGlobalVariablesScript.ActiveState.Collecting:
		{
				TrackVFMG1.IsTracking = false;
				TrackVFMG1 script = GameObject.FindObjectOfType<TrackVFMG1>();
				if (script == null) break;
				script.OnTrackingLost();
				break;
		}
			case UIGlobalVariablesScript.ActiveState.Gun:
			{
				TrackVFMG2.IsTracking = false;
				TrackVFMG2 script = GameObject.FindObjectOfType<TrackVFMG2>();
				if (script == null) break;
				script.OnTrackingLost();
				break;
			}
		}

	}
}