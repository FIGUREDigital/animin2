﻿using UnityEngine;
using System.Collections;

public enum GameScenes
{
	Caring,
	MinigameCubeRunner,
	MinigameCannon,

	Count,
};

#region ValueSmoother : Not entirely sure why we need this. We almost definitley don't
public class ValueSmoother
{
    public float ValueNow;
    public float ValueNext;

    public void Update()
    {
        if (Mathf.Abs(ValueNow - ValueNext) >= 0.15f)
            ValueNow = Mathf.Lerp(ValueNow, ValueNext, Time.deltaTime * 6);
    }
}

public class ValueSmootherVector3
{
    public Vector3 ValueNow;
    public Vector3 ValueNext;

    public void Update()
    {
        if (Vector3.Distance(ValueNow, ValueNext) >= 0.05f)
            ValueNow = Vector3.Lerp(ValueNow, ValueNext, Time.deltaTime * 6);
    }
}
#endregion

public class MainARHandler : MonoBehaviour
{
    #region Singleton

    private static MainARHandler s_Instance;

    public static MainARHandler Get
    {
        get
        {
            if ( s_Instance == null )
            {
                s_Instance = new MainARHandler();

            }
            return s_Instance;
        }
    }

    #endregion

	[SerializeField]
	private Camera ARCamera;

	private GameScenes m_CurrentGameScene;

	public GameScenes CurrentGameScene {
		get {
			return m_CurrentGameScene;
		}
		set {
			m_CurrentGameScene = value;
		}
	}
	private GameObject CurrentGameSceneGameObject;

    private bool m_IsTracking, m_CameraUnlock;
	private AniminTrackableEventHandler mLastTrack;

	private Transform NonARCameraPositionRef;

    private ValueSmoother SmootherAxisX = new ValueSmoother();
    private ValueSmoother SmootherAxisY = new ValueSmoother();
    private ValueSmootherVector3 CameraPositionSmoother = new ValueSmootherVector3();
    private ValueSmootherVector3 CameraRotationSmoother = new ValueSmootherVector3();

	// Use this for initialization
	void Start ()
	{
        if (s_Instance == null)
        {
            s_Instance = this;
        }
		ChangeSceneToCaring ();
	}
	
	// Update is called once per frame
	void LateUpdate ()
	{
        if (!m_CameraUnlock &&
            ARCamera != null)
        {
            ARCamera.gameObject.transform.position = NonARCameraPositionRef.position;
            ARCamera.gameObject.transform.rotation = NonARCameraPositionRef.rotation;

        }
	}

	public void OnTrackableStateChanged (
		AniminTrackableEventHandler AniminTrackScript,
		TrackableBehaviour.Status previousStatus,
		TrackableBehaviour.Status newStatus)
	{
		mLastTrack = AniminTrackScript;
		if (newStatus == TrackableBehaviour.Status.DETECTED ||
		      newStatus == TrackableBehaviour.Status.TRACKED ||
		      newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED) {
			OnTrackingFound ();
		} else {
			OnTrackingLost ();
		}
	}



	private void OnTrackingFound ()
	{
		if (!m_IsTracking) {
			m_IsTracking = true;
			Debug.Log ("OnTrackingFound : [" + mLastTrack.TrackableBehaviour.TrackableName + "];");
            if (m_CurrentGameScene == GameScenes.Caring)
            {
                CaringSceneOnTrackingFound();

            }
            else
            {
                CurrentGameSceneGameObject.transform.parent = mLastTrack.gameObject.transform;
                m_CameraUnlock = true;
            }
		}
	}



	private void OnTrackingLost ()
	{
		if (m_IsTracking) {
			m_IsTracking = false;
            Debug.Log ("OnTrackingLost : [" + mLastTrack.TrackableBehaviour.TrackableName + "];");
            if (m_CurrentGameScene == GameScenes.Caring)
            {
                CaringScreenOnTrackingLost();
            }
            else
            {
                CurrentGameSceneGameObject.transform.parent = this.gameObject.transform;
                m_CameraUnlock = false;
            }
		}
	}

	

    private void CaringARScene(bool ActivateAR){
        UIGlobalVariablesScript.Singleton.MainCharacterRef.transform.parent = (ActivateAR ? UIGlobalVariablesScript.Singleton.ARWorldRef.transform : UIGlobalVariablesScript.Singleton.NonARWorldRef.transform);
        UIGlobalVariablesScript.Singleton.ARWorldRef.SetActive(ActivateAR);
        UIGlobalVariablesScript.Singleton.NonARWorldRef.SetActive(!ActivateAR);
    }


	#region Caring Screen Handler


	public void OnCharacterEnterARScene()
	{
        Debug.Log("OnCharacterEnterARScene");
		UIGlobalVariablesScript.Singleton.MainCharacterRef.transform.parent = UIGlobalVariablesScript.Singleton.ARSceneRef.transform;
        m_CameraUnlock = true;
		//SavedARPosition.y = 0;
		UIGlobalVariablesScript.Singleton.MainCharacterRef.transform.localPosition = new Vector3(0, 0.01f, 0);
		UIGlobalVariablesScript.Singleton.MainCharacterRef.transform.rotation = Quaternion.Euler(0, 180, 0);
		UIGlobalVariablesScript.Singleton.MainCharacterRef.transform.localScale = new Vector3(0.035f, 0.035f, 0.035f);
		UIGlobalVariablesScript.Singleton.Shadow.transform.localScale = new Vector3(0.46f, 0.46f, 0.46f);

		if (UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>().CurrentAction != ActionId.Sleep &&
			UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>().CurrentAction != ActionId.EnterSleep)
		{
			UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>().Stop(true);
			UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterControllerScript>().ResetRotation();
		}

		UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>().AnimateJumpOutOfPortal();

        UIGlobalVariablesScript.Singleton.ARSceneRef.transform.parent = mLastTrack.gameObject.transform;

	}
	public void OnCharacterEnterNonARScene()
	{
        m_CameraUnlock = false;
		//UIGlobalVariablesScript.Singleton.ARCameraComponent.transform.position = new Vector3(0, 123.1f, -198.3f);
		if (UIGlobalVariablesScript.Singleton.ARCameraComponent != null) UIGlobalVariablesScript.Singleton.ARCameraComponent.transform.rotation = Quaternion.Euler(14.73474f, 0.0f, 0.0f);

		UIGlobalVariablesScript.Singleton.MainCharacterRef.transform.parent = UIGlobalVariablesScript.Singleton.NonSceneRef.transform;
		UIGlobalVariablesScript.Singleton.MainCharacterRef.transform.localPosition = new Vector3(0, 0.01f, 0);
		UIGlobalVariablesScript.Singleton.MainCharacterRef.transform.rotation = Quaternion.Euler(0, 180, 0);
		UIGlobalVariablesScript.Singleton.MainCharacterRef.transform.localScale = new Vector3(0.035f, 0.035f, 0.035f);
		UIGlobalVariablesScript.Singleton.Shadow.transform.localScale = new Vector3(0.46f, 0.46f, 0.46f);

		if (UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>().CurrentAction != ActionId.Sleep &&
			UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>().CurrentAction != ActionId.EnterSleep)
		{
			UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterControllerScript>().ResetRotation();
			UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>().Stop(true);
		}
	}
	private void CaringSceneOnTrackingFound(){
		CharacterProgressScript progress = UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>();

		if (UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>().CurrentAction == ActionId.Sleep ||
			UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>().CurrentAction == ActionId.EnterSleep)
		{
			//Debug.Log("OnTrackingFound: SLEEPING");

			UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>().exitSleep();

			UIGlobalVariablesScript.Singleton.NonSceneRef.SetActive(false);
			UIGlobalVariablesScript.Singleton.ARSceneRef.SetActive(true);
			OnCharacterEnterARScene();
		}
		else
		{
			//Debug.Log("OnTrackingFound: caring screen");
			{
				//return;
				UIGlobalVariablesScript.Singleton.SoundEngine.Play(ProfilesManagementScript.Singleton.CurrentAnimin.PlayerAniminId, ProfilesManagementScript.Singleton.CurrentAnimin.AniminEvolutionId, CreatureSoundId.JumbInPortal);
				UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<AnimationControllerScript>().IsEnterPortal = true;
				progress.CurrentAction = ActionId.EnterPortalToAR;

				UIGlobalVariablesScript.Singleton.ARPortal.GetComponent<PortalScript>().Show(PortalStageId.NonARScene, true);
				progress.Stop(true);
				progress.PortalTimer = 0;
				//Debug.Log("ENTERING AR STATE ");
				//UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>().CurrentAction = ActionId.EnterPortalToNonAR;


				UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<AnimateCharacterOutPortalScript>().Timer = 0;
				UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<AnimateCharacterOutPortalScript>().JumbId = AnimateCharacterOutPortalScript.JumbStateId.JumbIn;
			}
		}
	}
	public void CaringScreenOnTrackingLost()
	{
		/*
        bool isPlayingMinigame = false;


        if(UIGlobalVariablesScript.Singleton.CubeRunnerMinigameSceneRef.activeInHierarchy || 
           UIGlobalVariablesScript.Singleton.GunGameScene.activeInHierarchy)
            isPlayingMinigame = true;
		*/
		UIGlobalVariablesScript.Singleton.MainCharacterRef.transform.parent = UIGlobalVariablesScript.Singleton.NonARWorldRef.transform;
		if (UIGlobalVariablesScript.Singleton.NonSceneRef != null) UIGlobalVariablesScript.Singleton.NonSceneRef.SetActive(true);
		if (UIGlobalVariablesScript.Singleton.ARSceneRef != null)  UIGlobalVariablesScript.Singleton.ARSceneRef.SetActive(false);

		//		if(isPlayingMinigame)
		{
			//			Debug.Log("OnTrackingLost: playing mini game");
		}
		//		else
		{
			OnCharacterEnterNonARScene();

			//return;
			if (UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>().CurrentAction == ActionId.Sleep ||
				UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>().CurrentAction == ActionId.EnterSleep)
			{

				//				Debug.Log("OnTrackingLost: sleeping");
			}
			else
			{
				CharacterProgressScript progressScript = UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>();

				//				Debug.Log("OnTrackingLost: caring screen");
				//UIGlobalVariablesScript.Singleton.MainUIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>().PortalTimer = 0;
				//Debug.Log("ENTERING NON AR STATE ");
				//UIGlobalVariablesScript.Singleton.MainUIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>().Stop(true);
				//UIGlobalVariablesScript.Singleton.MainCharacterAnimationControllerRef.IsEnterPortal = true;
				progressScript.CurrentAction = ActionId.None;
				//UIGlobalVariablesScript.Singleton.Vuforia.OnExitAR();
				UIGlobalVariablesScript.Singleton.ARPortal.GetComponent<PortalScript>().Show(PortalStageId.NonARScene, false);

				UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<AnimationControllerScript>().IsExitPortal = true;
				UIGlobalVariablesScript.Singleton.MainCharacterRef.transform.rotation = Quaternion.Euler(0, 180, 0);
				UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterControllerScript>().ResetRotation();

				UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<AnimateCharacterOutPortalScript>().Timer = 0;
				UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<AnimateCharacterOutPortalScript>().JumbId = AnimateCharacterOutPortalScript.JumbStateId.Jumbout;
				UIGlobalVariablesScript.Singleton.SoundEngine.Play(ProfilesManagementScript.Singleton.CurrentAnimin.PlayerAniminId, ProfilesManagementScript.Singleton.CurrentAnimin.AniminEvolutionId, CreatureSoundId.JumbOutPortal);
				//UIGlobalVariablesScript.Singleton.ARPortal.GetComponent<PortalScript>().Show(true);
				progressScript.CurrentAction = ActionId.SmallCooldownPeriod;
				progressScript.SmallCooldownTimer = 0.5f;

				//OnExitAR();
				//UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>().CurrentAction = ActionId.EnterPortalToNonAR;
			}
		}
	}

	#endregion


	public void ChangeSceneToCaring ()
	{
		LoadNewGameScene(GameScenes.Caring);
	}
	public void ChangeSceneToCubeRunner ()
	{
		LoadNewGameScene(GameScenes.MinigameCubeRunner);
	}
	public void ChangeSceneToCannon ()
	{
		LoadNewGameScene(GameScenes.MinigameCannon);
	}
	private void LoadNewGameScene(GameScenes newScene){
		m_CurrentGameScene = newScene;
		UnityEngine.Object.Destroy (CurrentGameSceneGameObject);
		switch (m_CurrentGameScene) {
		case GameScenes.Caring:
			CurrentGameSceneGameObject = (GameObject)GameObject.Instantiate (Resources.Load ("Prefabs/ScenePrefabs/Caring"));
			break;

		case GameScenes.MinigameCubeRunner:
			CurrentGameSceneGameObject = (GameObject)GameObject.Instantiate (Resources.Load ("Prefabs/TestDummies/Cube"));
			break;

		case GameScenes.MinigameCannon:
			CurrentGameSceneGameObject = (GameObject)GameObject.Instantiate (Resources.Load ("Prefabs/TestDummies/Gunner"));
			break;

		}
		NonARCameraPositionRef = CurrentGameSceneGameObject.GetComponent<NonARPosRef> ().NonARCameraPositionReference;
		Debug.Log ("NonARCameraPositionRef : [" + NonARCameraPositionRef + "];");
	}
}
