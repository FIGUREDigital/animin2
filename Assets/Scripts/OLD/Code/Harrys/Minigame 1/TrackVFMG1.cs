using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TrackVFMG1 : MonoBehaviour, ITrackableEventHandler
{
	private TrackableBehaviour mTrackableBehaviour;
	
	Vector3 SavedARPosition;
	
	public static bool IsTracking;
	
	
	private ValueSmoother SmootherAxisX = new ValueSmoother();
	private ValueSmoother SmootherAxisY = new ValueSmoother();
	private ValueSmootherVector3 CameraPositionSmoother = new ValueSmootherVector3();
	private ValueSmootherVector3 CameraRotationSmoother = new ValueSmootherVector3();
	
	void Start()
	{
		mTrackableBehaviour = GetComponent<TrackableBehaviour>();
		if (mTrackableBehaviour)
		{
			mTrackableBehaviour.RegisterTrackableEventHandler(this);
		}
		SavedARPosition = new Vector3(0, 0.0f, 0);
		Input.gyro.enabled = true;
	}
	
	public void OnTrackableStateChanged(
		TrackableBehaviour.Status previousStatus,
		TrackableBehaviour.Status newStatus)
	{
		
		if (newStatus == TrackableBehaviour.Status.DETECTED ||
		    newStatus == TrackableBehaviour.Status.TRACKED ||
		    newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
		{
			OnTrackingFound();
		}
		else
		{
			OnTrackingLost();
		}
	}
	
	
	
	public void FlipFrontBackCamera()
	{
		CameraDevice.Instance.Stop();
		
		// This assumes that the back facing camera was used before:
		if(CameraDevice.Instance.GetCameraDirection() == CameraDevice.CameraDirection.CAMERA_BACK || CameraDevice.Instance.GetCameraDirection() == CameraDevice.CameraDirection.CAMERA_DEFAULT)
		{
			CameraDevice.Instance.Init(CameraDevice.CameraDirection.CAMERA_FRONT);
			
		}
		else
		{
			CameraDevice.Instance.Init(CameraDevice.CameraDirection.CAMERA_BACK);
			
		}
		
		CameraDevice.Instance.Start();
	}
	
	void Update()
	{
		
	}
	
	
	void LateUpdate()
	{
		
		if(UIGlobalVariablesScript.Singleton.NonSceneRef.activeInHierarchy)
		{
			
			
			float stableAccelerationX = (float)System.Math.Round(Input.acceleration.x, 2);
			float stableAccelerationY = (float)System.Math.Round(Input.acceleration.y, 2);
			
			float angle = Mathf.Lerp(180, 360, (stableAccelerationX + 1) / 2 /*(Mathf.Sin(Time.time) + 1) / 2*/);
			SmootherAxisX.ValueNext = (float)System.Math.Round(angle);
			
			Vector3 newPosition = new Vector3(
				Mathf.Cos(SmootherAxisX.ValueNow* Mathf.Deg2Rad ) * 220,
				0,
				(Mathf.Sin(SmootherAxisX.ValueNow* Mathf.Deg2Rad ) * 220) * 0.2f);
			
			
			
			
			float value = stableAccelerationY;
			if(value > 0) value = 0;
			if(value < -1) value = -1;
			value *= -1;
			
			float angle2 = Mathf.Lerp(360, 180, value/*(Mathf.Sin(Time.time) + 1) / 2*/);
			SmootherAxisY.ValueNext = (float)System.Math.Round(angle2);
			
			Vector3 newPosition2 = new Vector3(
				0,
				Mathf.Cos(SmootherAxisY.ValueNow* Mathf.Deg2Rad ) * 90,
				(Mathf.Sin(SmootherAxisY.ValueNow* Mathf.Deg2Rad ) * 90) * 0.2f);
			
			//		Vector3 cameraPoint = new Vector3(0, 233.1f, -198.3f);
			//		Transform target = UIGlobalVariablesScript.Singleton.NonSceneRef.transform;
			
			Vector3 cameraPoint = new Vector3(0, 500.6f, -250.63f);
			Transform target = UIGlobalVariablesScript.Singleton.CubeRunnerMinigameSceneRef.transform;
			
			Camera.main.transform.localPosition = cameraPoint + newPosition2 + newPosition;
			Camera.main.transform.LookAt(target);
		}
		
		
		SmootherAxisX.Update();
		SmootherAxisY.Update();
		
	}
	public static void EnableDisableMinigamesBasedOnARStatus()
	{
		GameObject sprite = GameObject.Find("SpriteCubeWorld");
		
		if(sprite != null)
		{
			if(IsTracking)
			{
				sprite.GetComponent<UIClickButtonMasterScript>().FunctionalityId = UIFunctionalityId.PlayMinigameCubeRunners;

				
			}
			else
			{
				sprite.GetComponent<UIClickButtonMasterScript>().FunctionalityId = UIFunctionalityId.None;

			}
		}
	}
	
	
	public void OnCharacterEnterARScene()
	{
		UIGlobalVariablesScript.Singleton.MainCharacterRef.transform.parent = UIGlobalVariablesScript.Singleton.ARSceneRef.transform;
		SavedARPosition.y = 0;
		UIGlobalVariablesScript.Singleton.MainCharacterRef.transform.localPosition = new Vector3(0, 0.01f, 0);
		UIGlobalVariablesScript.Singleton.MainCharacterRef.transform.rotation = Quaternion.Euler(0, 180, 0);
		UIGlobalVariablesScript.Singleton.MainCharacterRef.transform.localScale = new Vector3(0.035f, 0.035f, 0.035f);
		UIGlobalVariablesScript.Singleton.Shadow.transform.localScale = new Vector3(0.46f, 0.46f, 0.46f);
		
		if(UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>().CurrentAction != ActionId.Sleep &&
		   UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>().CurrentAction != ActionId.EnterSleep)
		{
			UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>().Stop(true);
			UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterControllerScript>().ResetRotation();
		}
	}
	
	public void OnCharacterEnterNonARScene()
	{
		if(IsTracking)
			SavedARPosition = UIGlobalVariablesScript.Singleton.MainCharacterRef.transform.localPosition;
		
		
		//HARRYS POSSIBILITY
		Camera.main.transform.position = new Vector3(0, 123.1f, -198.3f);
		Camera.main.transform.rotation = Quaternion.Euler(14.73474f, 0.0f, 0.0f);
		
		UIGlobalVariablesScript.Singleton.MainCharacterRef.transform.parent = UIGlobalVariablesScript.Singleton.NonSceneRef.transform;
		UIGlobalVariablesScript.Singleton.MainCharacterRef.transform.localPosition = new Vector3(0, 0.01f, 0);
		UIGlobalVariablesScript.Singleton.MainCharacterRef.transform.rotation = Quaternion.Euler(0, 180, 0);
		UIGlobalVariablesScript.Singleton.MainCharacterRef.transform.localScale = new Vector3(0.035f, 0.035f, 0.035f);
		UIGlobalVariablesScript.Singleton.Shadow.transform.localScale = new Vector3(0.46f, 0.46f, 0.46f);
		
		
	}
	
	
	public void OnTrackingFound()
	{
		
		IsTracking = true;
		bool isPlayingMinigame = false;
		
		UIGlobalVariablesScript.Singleton.CubeRunnerMinigameSceneRef.transform.parent = UIGlobalVariablesScript.Singleton.ARSceneRef.transform;
		
		UIGlobalVariablesScript.Singleton.NonSceneRef.SetActive (false);
		UIGlobalVariablesScript.Singleton.ARSceneRef.SetActive(true);
	}
	
	
	public void OnTrackingLost()
	{
		IsTracking = false;
		
		bool isPlayingMinigame = false;
		if(UIGlobalVariablesScript.Singleton.CubeRunnerMinigameSceneRef.activeInHierarchy)
			isPlayingMinigame = true;
		
		UIGlobalVariablesScript.Singleton.CubeRunnerMinigameSceneRef.transform.parent = UIGlobalVariablesScript.Singleton.NonSceneRef.transform;
		
		UIGlobalVariablesScript.Singleton.NonSceneRef.SetActive (true);
		UIGlobalVariablesScript.Singleton.ARSceneRef.SetActive(false);
		
		if(isPlayingMinigame)
		{
		}
		else
		{
			OnCharacterEnterNonARScene();
			
			//return;
			/*
			if(UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>().CurrentAction == ActionId.Sleep ||
			   UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>().CurrentAction == ActionId.EnterSleep) */
			if(false){
				
			}
			else
			{
				//		CharacterProgressScript progressScript = UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>();
				
				//				progressScript.CurrentAction = ActionId.None;
				//				UIGlobalVariablesScript.Singleton.ARPortal.GetComponent<PortalScript>().Show(PortalStageId.NonARScene, false);
				
				//UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<MinigameAnimationControllerScript>().IsExitPortal = true; <Looks dodgy
				UIGlobalVariablesScript.Singleton.MainCharacterRef.transform.rotation = Quaternion.Euler(0, 180, 0);
				UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterControllerScript>().ResetRotation();
				
				//				UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<AnimateCharacterOutPortalScript>().Timer = 0;
				//				UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<AnimateCharacterOutPortalScript>().JumbId = AnimateCharacterOutPortalScript.JumbStateId.Jumbout;
				//UIGlobalVariablesScript.Singleton.SoundEngine.Play(ProfilesManagementScript.Singleton.CurrentAnimin.PlayerAniminId, ProfilesManagementScript.Singleton.CurrentAnimin.AniminEvolutionId, CreatureSoundId.JumbOutPortal); <Also looks dodgy
				//				progressScript.CurrentAction = ActionId.SmallCooldownPeriod;
				//				progressScript.SmallCooldownTimer = 0.5f;
				
			}
		}
		
	}
}