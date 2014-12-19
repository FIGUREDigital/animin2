using UnityEngine;
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

    public static MainARHandler Instance
    {
        get
        {
            if (s_Instance == null)
            {
                s_Instance = new MainARHandler();

            }
            return s_Instance;
        }
    }

    #endregion

    [SerializeField]
    private Camera ARCamera;
    [SerializeField]
    private SoundEngineScript SoundEngine;

    public Camera MainARCamera { get { return ARCamera; } }
    public GameObject CurrentItem;
    public bool DraggedFromStage;
    private GameScenes m_CurrentGameScene;
    private GameScenes m_PreviousGameScene;

    public GameScenes CurrentGameScene
    {
        get
        {
            return m_CurrentGameScene;
        }
        set
        {
            m_CurrentGameScene = value;
        }
    }

    private GameObject CurrentGameSceneGameObject;

    private bool m_IsTracking, m_CameraUnlock;

    public bool CameraUnlock
    {
        get { return m_CameraUnlock; }
    }

    private AniminTrackableEventHandler mLastTrack;

    private Transform NonARCameraPositionRef;

    public Transform NonARPosRef
    {
        get { return NonARCameraPositionRef; }
    }

    private ValueSmoother SmootherAxisX = new ValueSmoother();
    private ValueSmoother SmootherAxisY = new ValueSmoother();
    private ValueSmootherVector3 CameraPositionSmoother = new ValueSmootherVector3();
    private ValueSmootherVector3 CameraRotationSmoother = new ValueSmootherVector3();

    private SpriteStore mSpriteStore;

    public SpriteStore SpriteStore
    {
        get
        {
            if(mSpriteStore == null)
            {
                mSpriteStore = ((GameObject)Instantiate(Resources.Load("Prefabs/UI/ItemSpriteStore"))).GetComponent<SpriteStore>();
            }
            return mSpriteStore;
        }
    }

    // Use this for initialization
    void Start()
    {
        if (s_Instance == null)
        {
            s_Instance = this;
        }
        ChangeSceneToCaring();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!m_CameraUnlock &&
            ARCamera != null)
        {
            float stableAccelerationX = (float)System.Math.Round(Input.acceleration.x, 2);
            float stableAccelerationY = (float)System.Math.Round(Input.acceleration.y, 2);
            float angle = Mathf.Lerp(180, 360, (stableAccelerationX + 1) / 2 /*(Mathf.Sin(Time.time) + 1) / 2*/);
            SmootherAxisX.ValueNext = (float)System.Math.Round(angle);
			
            Vector3 newPosition = new Vector3(
                                      Mathf.Cos(SmootherAxisX.ValueNow * Mathf.Deg2Rad) * 220,
                                      0,
                                      (Mathf.Sin(SmootherAxisX.ValueNow * Mathf.Deg2Rad) * 220) * 0.2f);
			
			
			
			
            float value = stableAccelerationY;
            if (value > 0)
                value = 0;
            if (value < -1)
                value = -1;
            value *= -1;
            //value = 1 - value;
			
            float angle2 = Mathf.Lerp(360, 180, value/*(Mathf.Sin(Time.time) + 1) / 2*/);
            SmootherAxisY.ValueNext = (float)System.Math.Round(angle2);
			
            Vector3 newPosition2 = new Vector3(
                                       0,
                                       Mathf.Cos(SmootherAxisY.ValueNow * Mathf.Deg2Rad) * 90,
                                       (Mathf.Sin(SmootherAxisY.ValueNow * Mathf.Deg2Rad) * 90) * 0.2f);
			
            Vector3 cameraPoint = CurrentGameSceneGameObject.GetComponent<NonARPosRef>().NonARCameraPositionReference.position;// new Vector3(0, 430f, -630f);
            Transform target = CurrentGameSceneGameObject.transform;
			
            Vector3 finalpos = cameraPoint + newPosition2 + newPosition;

				
            Transform t = ARCamera.gameObject.transform;
            t.localPosition = finalpos;
				
            Vector3 up = Input.acceleration;
            float tempX = up.x;
            up.x = up.z;
            up.z = tempX;
				
            up.z = 0;
				
            //up  = Quaternion.AngleAxis(Time.timeSinceLevelLoad,Vector3.forward) * up;
				
            t.rotation = Quaternion.LookRotation(target.transform.position - t.position, Vector3.up);
				
            Debug.DrawRay(t.position, t.forward);

        }
		
		
        SmootherAxisX.Update();
        SmootherAxisY.Update();
    }

    public void OnTrackableStateChanged(
        AniminTrackableEventHandler AniminTrackScript,
        TrackableBehaviour.Status previousStatus,
        TrackableBehaviour.Status newStatus)
    {
        mLastTrack = AniminTrackScript;
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



    private void OnTrackingFound()
    {
        if (!m_IsTracking)
        {
            AchievementManager.Instance.AddToAchievment(AchievementManager.Achievements.ArMode);
            m_IsTracking = true;
            Debug.Log("OnTrackingFound : [" + mLastTrack.TrackableBehaviour.TrackableName + "];");
            if (m_CurrentGameScene == GameScenes.Caring)
            {
                CaringSceneOnTrackingFound();

            }
            else
            {
                if (m_CurrentGameScene == GameScenes.MinigameCannon)
                    GunMiniGameArenaVisible(false);
                CurrentGameSceneGameObject.transform.parent = mLastTrack.gameObject.transform;
                m_CameraUnlock = true;
            }
        }
    }



    private void OnTrackingLost()
    {
        if (m_IsTracking)
        {
            m_IsTracking = false;
            Debug.Log("OnTrackingLost : [" + mLastTrack.TrackableBehaviour.TrackableName + "];");
            if (m_CurrentGameScene == GameScenes.Caring)
            {
                CaringScreenOnTrackingLost();
            }
            else
            {
                if (m_CurrentGameScene == GameScenes.MinigameCannon)
                    GunMiniGameArenaVisible(true);
                CurrentGameSceneGameObject.transform.parent = this.gameObject.transform;
                m_CameraUnlock = false;
            }
        }
    }

    private void GunMiniGameArenaVisible(bool on)
    {
        GunsMinigameScript script = UIGlobalVariablesScript.Singleton.GunGameScene.GetComponent<GunsMinigameScript>();
        if (script == null)
            return;
        if (script.ArenaStage == null)
            return;
        script.ArenaStage.SetActive(on);
    }

    private void CaringARScene(bool ActivateAR)
    {
        UIGlobalVariablesScript.Singleton.MainCharacterRef.transform.parent = (ActivateAR ? UIGlobalVariablesScript.Singleton.ARWorldRef.transform : UIGlobalVariablesScript.Singleton.NonARWorldRef.transform);
        UIGlobalVariablesScript.Singleton.ARWorldRef.SetActive(ActivateAR);
        UIGlobalVariablesScript.Singleton.NonARWorldRef.SetActive(!ActivateAR);
    }


    #region Caring Screen Handler

    #region Enter Scene

    public void PauseJumpOutIntoAR()
    {
        Debug.Log("PauseJumpOutIntoAR");
        m_CameraUnlock = true;

        UIGlobalVariablesScript.Singleton.MainCharacterRef.transform.localPosition = new Vector3(0, 0.01f, 0);
        UIGlobalVariablesScript.Singleton.MainCharacterRef.transform.rotation = Quaternion.Euler(0, 180, 0);
        UIGlobalVariablesScript.Singleton.MainCharacterRef.transform.localScale = new Vector3(0.035f, 0.035f, 0.035f);
        UIGlobalVariablesScript.Singleton.Shadow.transform.localScale = new Vector3(0.46f, 0.46f, 0.46f);

        UIGlobalVariablesScript.Singleton.MainCharacterRef.transform.parent = UIGlobalVariablesScript.Singleton.ARSceneRef.transform;
        UIGlobalVariablesScript.Singleton.ARSceneRef.transform.parent = mLastTrack.gameObject.transform;
    }

    public void OnCharacterEnterARScene(bool SkipAnimation = false)
    {
        if (m_IsTracking)
        {
            Debug.Log("OnCharacterEnterARScene");
            CharacterProgressScript charprogress = UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>();
            UIGlobalVariablesScript.Singleton.MainCharacterRef.transform.parent = UIGlobalVariablesScript.Singleton.ARSceneRef.transform;
            m_CameraUnlock = true;
            //SavedARPosition.y = 0;
            UIGlobalVariablesScript.Singleton.MainCharacterRef.transform.localPosition = new Vector3(0, 0.01f, 0);
            UIGlobalVariablesScript.Singleton.MainCharacterRef.transform.rotation = Quaternion.Euler(0, 180, 0);
            UIGlobalVariablesScript.Singleton.MainCharacterRef.transform.localScale = new Vector3(0.035f, 0.035f, 0.035f);
            UIGlobalVariablesScript.Singleton.Shadow.transform.localScale = new Vector3(0.46f, 0.46f, 0.46f);

            if (!SkipAnimation &&
            charprogress.CurrentAction != ActionId.Sleep &&
            charprogress.CurrentAction != ActionId.EnterSleep)
            {
                charprogress.Stop(true);
                UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterControllerScript>().ResetRotation();
            }

            charprogress.AnimateJumpOutOfPortal();
            UIGlobalVariablesScript.Singleton.ARSceneRef.transform.parent = mLastTrack.gameObject.transform;
        }
    }

    public void OnCharacterEnterNonARScene(bool SkipAnimation = false)
    {
        Debug.Log("OnCharacterEnterNonARScene");
        m_CameraUnlock = false;
        //UIGlobalVariablesScript.Singleton.ARCameraComponent.transform.position = new Vector3(0, 123.1f, -198.3f);
        if (UIGlobalVariablesScript.Singleton.ARCameraComponent != null)
            UIGlobalVariablesScript.Singleton.ARCameraComponent.transform.rotation = Quaternion.Euler(14.73474f, 0.0f, 0.0f);

        UIGlobalVariablesScript.Singleton.MainCharacterRef.transform.parent = UIGlobalVariablesScript.Singleton.NonSceneRef.transform;
        UIGlobalVariablesScript.Singleton.MainCharacterRef.transform.localPosition = new Vector3(0, 0.01f, 0);
        UIGlobalVariablesScript.Singleton.MainCharacterRef.transform.rotation = Quaternion.Euler(0, 180, 0);
        UIGlobalVariablesScript.Singleton.MainCharacterRef.transform.localScale = new Vector3(0.035f, 0.035f, 0.035f);
        UIGlobalVariablesScript.Singleton.Shadow.transform.localScale = new Vector3(0.46f, 0.46f, 0.46f);

        UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>().ShaderAlpha = 1;

        if (!SkipAnimation &&
            UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>().CurrentAction != ActionId.Sleep &&
            UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>().CurrentAction != ActionId.EnterSleep)
        {
            UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterControllerScript>().ResetRotation();
            UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>().Stop(true);
        }
    }

    #endregion

    #region On Tracking Lost or Found

    private void CaringSceneOnTrackingFound()
    {
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
        UIGlobalVariablesScript.Singleton.MainCharacterRef.transform.parent = UIGlobalVariablesScript.Singleton.NonARWorldRef.transform;
        if (UIGlobalVariablesScript.Singleton.NonSceneRef != null)
            UIGlobalVariablesScript.Singleton.NonSceneRef.SetActive(true);
        if (UIGlobalVariablesScript.Singleton.ARSceneRef != null)
            UIGlobalVariablesScript.Singleton.ARSceneRef.SetActive(false);
        OnCharacterEnterNonARScene();

        CharacterProgressScript progressScript = UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>();

        progressScript.CurrentAction = ActionId.None;
        UIGlobalVariablesScript.Singleton.ARPortal.GetComponent<PortalScript>().Show(PortalStageId.NonARScene, false);

        UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<AnimationControllerScript>().IsExitPortal = true;
        UIGlobalVariablesScript.Singleton.MainCharacterRef.transform.rotation = Quaternion.Euler(0, 180, 0);
        UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterControllerScript>().ResetRotation();

        UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<AnimateCharacterOutPortalScript>().Timer = 0;
        UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<AnimateCharacterOutPortalScript>().JumbId = AnimateCharacterOutPortalScript.JumbStateId.Jumbout;
        UIGlobalVariablesScript.Singleton.SoundEngine.Play(ProfilesManagementScript.Singleton.CurrentAnimin.PlayerAniminId, ProfilesManagementScript.Singleton.CurrentAnimin.AniminEvolutionId, CreatureSoundId.JumbOutPortal);
        progressScript.CurrentAction = ActionId.SmallCooldownPeriod;
        progressScript.SmallCooldownTimer = 0.5f;
    }

    #endregion

    #endregion


    public void ChangeSceneToCaring()
    {
        LoadNewGameScene(GameScenes.Caring);
    }

    public void ChangeSceneToCubeRunner()
    {
        LoadNewGameScene(GameScenes.MinigameCubeRunner);
    }

    public void ChangeSceneToCannon()
    {
        LoadNewGameScene(GameScenes.MinigameCannon);
    }

    private void LoadNewGameScene(GameScenes newScene)
    {
        Debug.Log("LoadNewGameScene : [" + newScene.ToString() + "];");
        if (mLastTrack != null && mLastTrack.gameObject.transform.childCount != 0)
        {
            UnityEngine.Object.Destroy(UIGlobalVariablesScript.Singleton.ARWorldRef);
            UnityEngine.Object.Destroy(UIGlobalVariablesScript.Singleton.NonARWorldRef);
            UnityEngine.Object.Destroy(UIGlobalVariablesScript.Singleton.MainCharacterRef);
        }
        m_PreviousGameScene = m_CurrentGameScene;
        m_CurrentGameScene = newScene;
        UnityEngine.Object.Destroy(CurrentGameSceneGameObject);
        switch (m_CurrentGameScene)
        {
            case GameScenes.Caring:
                CurrentGameSceneGameObject = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/ScenePrefabs/Caring"));
                if (m_PreviousGameScene != null && (m_PreviousGameScene == GameScenes.MinigameCubeRunner || m_PreviousGameScene == GameScenes.MinigameCannon))
                    UiPages.Next(Pages.CaringPage);
                break;

            case GameScenes.MinigameCubeRunner:
                CurrentGameSceneGameObject = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/ScenePrefabs/CubeMinigame"));
                UiPages.Next(Pages.CubeMinigamePage);

                break;

            case GameScenes.MinigameCannon:
                CurrentGameSceneGameObject = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/ScenePrefabs/GunMinigame"));
                UiPages.Next(Pages.GunMinigamePage);
                break;
        }

        Debug.Log("!script Re-initialisation attempted;");
        UIGlobalVariablesScript script = CurrentGameSceneGameObject.GetComponentInChildren<UIGlobalVariablesScript>();
        if (script != null)
        {
            script.Init();
            script.MainCharacterRef.GetComponent<CharacterSwapManagementScript>().LoadCharacter(ProfilesManagementScript.Singleton.CurrentAnimin.PlayerAniminId, ProfilesManagementScript.Singleton.CurrentAnimin.AniminEvolutionId);
        }
        else
        {
            Debug.Log("!SCRIPT NOT RE-INITED;");
        }

        switch (m_CurrentGameScene)
        {
            case GameScenes.Caring:
                if (m_IsTracking)
                {
                    UIGlobalVariablesScript.Singleton.NonSceneRef.SetActive(false);
                    UIGlobalVariablesScript.Singleton.ARSceneRef.SetActive(true);
                    OnCharacterEnterARScene();
                }
                else
                    OnCharacterEnterNonARScene();

                break;

            case GameScenes.MinigameCannon:
                if (m_IsTracking)
                    GunMiniGameArenaVisible(false);
                break;
        }

        Debug.Log("m_PreviousGameScene = [" + m_PreviousGameScene + "]; Returning from minigame = ["+(m_PreviousGameScene == GameScenes.MinigameCannon || m_PreviousGameScene == GameScenes.MinigameCubeRunner)+"]");
        if (m_PreviousGameScene == GameScenes.MinigameCannon || m_PreviousGameScene == GameScenes.MinigameCubeRunner)
        {
            Debug.Log("Minigame Cannon;");
            script.MainCharacterRef.GetComponent<CharacterProgressScript>().CurrentAction = ActionId.ExitPortalMainStage;
        }


        NonARPosRef CamPosRef = CurrentGameSceneGameObject.GetComponent<NonARPosRef>();
        if (CamPosRef != null)
            NonARCameraPositionRef = CamPosRef.NonARCameraPositionReference;
    }
    void OnApplicationPause(bool pauseStatus){
        if (pauseStatus)
            Save();
    }
    void OnApplicationQuit(){
        Save();
    }
    private void Save(){
        SaveAndLoad.Instance.SaveAllData();
    }
}
