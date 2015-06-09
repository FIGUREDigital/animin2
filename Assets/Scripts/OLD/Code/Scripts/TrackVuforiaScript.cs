using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Vuforia;

/*
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
*/

public class TrackVuforiaScript : MonoBehaviour, ITrackableEventHandler
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
        //Debug.Log("OnTrackableStateChanged!");
		
		if (WebCamTexture.devices.Length != 0 && (newStatus == TrackableBehaviour.Status.DETECTED || newStatus == TrackableBehaviour.Status.TRACKED || newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED))
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
        if (CameraDevice.Instance.GetCameraDirection() == CameraDevice.CameraDirection.CAMERA_BACK || CameraDevice.Instance.GetCameraDirection() == CameraDevice.CameraDirection.CAMERA_DEFAULT)
        {
            CameraDevice.Instance.Init(CameraDevice.CameraDirection.CAMERA_FRONT);


            //.reflection = QCARRenderer.VideoBackgroundReflection.OFF;

        }
        else
        {
            //QCARRenderer.Instance.GetVideoBackgroundConfig().reflection = QCARRenderer.VideoBackgroundReflection.ON;;
            CameraDevice.Instance.Init(CameraDevice.CameraDirection.CAMERA_BACK);

        }

        CameraDevice.Instance.Start();
    }

    void Update()
    {

    }


    void LateUpdate()
    {
        if (UIGlobalVariablesScript.Singleton.NonSceneRef.activeInHierarchy)
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
            if (value > 0) value = 0;
            if (value < -1) value = -1;
            value *= -1;
            //value = 1 - value;

            float angle2 = Mathf.Lerp(360, 180, value/*(Mathf.Sin(Time.time) + 1) / 2*/);
            SmootherAxisY.ValueNext = (float)System.Math.Round(angle2);

            Vector3 newPosition2 = new Vector3(
                0,
                Mathf.Cos(SmootherAxisY.ValueNow * Mathf.Deg2Rad) * 90,
                (Mathf.Sin(SmootherAxisY.ValueNow * Mathf.Deg2Rad) * 90) * 0.2f);

            Vector3 cameraPoint = new Vector3(0, 233.1f, -198.3f);
            Transform target = UIGlobalVariablesScript.Singleton.NonSceneRef.transform;

			Vector3 finalpos = cameraPoint + newPosition2 + newPosition;

			if (UIGlobalVariablesScript.Singleton.ARCameraComponent != null){

				Transform t = UIGlobalVariablesScript.Singleton.ARCameraComponent.transform;
				t.localPosition = finalpos;

				Vector3 up = Input.acceleration;
				float tempX = up.x;
				up.x=up.z;
				up.z = tempX;

				up.z = 0;

				//up  = Quaternion.AngleAxis(Time.timeSinceLevelLoad,Vector3.forward) * up;

				t.rotation = Quaternion.LookRotation(target.transform.position-t.position,Vector3.up);

	            Debug.DrawRay(t.position,t.forward);
			}
        }


        SmootherAxisX.Update();
        SmootherAxisY.Update();

    }
    public static void EnableDisableMinigamesBasedOnARStatus()
    {
        GameObject sprite = GameObject.Find("SpriteCubeWorld");

//        if (sprite != null)
//        {
//            if (IsTracking)
//            {
//                sprite.GetComponent<UIWidget>().color = new Color(1, 1, 1, 1);
//                sprite.GetComponent<UIClickButtonMasterScript>().FunctionalityId = UIFunctionalityId.PlayMinigameCubeRunners;
//
//                sprite.GetComponent<Button>().defaultColor = new Color(
//                    sprite.GetComponent<Button>().defaultColor.r,
//                    sprite.GetComponent<Button>().defaultColor.g,
//                    sprite.GetComponent<Button>().defaultColor.b,
//                    1);
//
//                sprite.GetComponent<Button>().hover = new Color(
//                    sprite.GetComponent<Button>().hover.r,
//                    sprite.GetComponent<Button>().hover.g,
//                    sprite.GetComponent<Button>().hover.b,
//                    1);
//
//                //UIGlobalVariablesScript.Singleton.RequiresGamecardScreenRef.SetActive(false);
//            }
//            else
//            {
//                sprite.GetComponent<UIWidget>().color = new Color(1, 1, 1, 0.5f);
//                sprite.GetComponent<UIClickButtonMasterScript>().FunctionalityId = UIFunctionalityId.None;
//
//                sprite.GetComponent<Button>().defaultColor = new Color(
//                    sprite.GetComponent<Button>().defaultColor.r,
//                    sprite.GetComponent<Button>().defaultColor.g,
//                    sprite.GetComponent<Button>().defaultColor.b,
//                    0.5f);
//
//                sprite.GetComponent<Button>().hover = new Color(
//                    sprite.GetComponent<Button>().hover.r,
//                    sprite.GetComponent<Button>().hover.g,
//                    sprite.GetComponent<Button>().hover.b,
//                    0.5f);
//            }
//        }
    }


    public void OnCharacterEnterARScene()
    {
        UIGlobalVariablesScript.Singleton.MainCharacterRef.transform.parent = UIGlobalVariablesScript.Singleton.ARSceneRef.transform;
        SavedARPosition.y = 0;
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

    }

    public void OnCharacterEnterNonARScene()
    {
        if (IsTracking)
            SavedARPosition = UIGlobalVariablesScript.Singleton.MainCharacterRef.transform.localPosition;

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


    public void OnTrackingFound()
    {

        //Debug.Log("OnTrackingFound called");

        IsTracking = true;

        UIGlobalVariablesScript.Singleton.AROnIndicator.SetActive(true);
        UIGlobalVariablesScript.Singleton.AROffIndicator.SetActive(false);

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
                UIGlobalVariablesScript.Singleton.SoundEngine.Play(ProfilesManagementScript.Instance.CurrentAnimin.PlayerAniminId, ProfilesManagementScript.Instance.CurrentAnimin.AniminEvolutionId, CreatureSoundId.JumbInPortal);
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


    public void OnTrackingLost()
    {
        IsTracking = false;
        /*
        bool isPlayingMinigame = false;


        if(UIGlobalVariablesScript.Singleton.CubeRunnerMinigameSceneRef.activeInHierarchy || 
           UIGlobalVariablesScript.Singleton.GunGameScene.activeInHierarchy)
            isPlayingMinigame = true;
*/

		if (UIGlobalVariablesScript.Singleton.AROnIndicator!=null)  UIGlobalVariablesScript.Singleton.AROnIndicator.SetActive(false);
		if (UIGlobalVariablesScript.Singleton.AROffIndicator!=null) UIGlobalVariablesScript.Singleton.AROffIndicator.SetActive(true);

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
                UIGlobalVariablesScript.Singleton.SoundEngine.Play(ProfilesManagementScript.Instance.CurrentAnimin.PlayerAniminId, ProfilesManagementScript.Instance.CurrentAnimin.AniminEvolutionId, CreatureSoundId.JumbOutPortal);
                //UIGlobalVariablesScript.Singleton.ARPortal.GetComponent<PortalScript>().Show(true);
                progressScript.CurrentAction = ActionId.SmallCooldownPeriod;
                progressScript.SmallCooldownTimer = 0.5f;

                //OnExitAR();
                //UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>().CurrentAction = ActionId.EnterPortalToNonAR;
            }
        }

    }
}