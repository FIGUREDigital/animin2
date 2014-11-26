using UnityEngine;
using System.Collections;

public class LoadProfileMG2 : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		
		//From UIFunctionalityId.StartSelectedMinigame
		
		//		UIGlobalVariablesScript.Singleton.ARWorldRef.SetActive(false);
		//		UIGlobalVariablesScript.Singleton.NonARWorldRef.SetActive(false);
		
		UIGlobalVariablesScript.Singleton.PauseGameButton.SetActive(true);
		UIGlobalVariablesScript.Singleton.PausedScreen.SetActive(false);
		
		UIGlobalVariablesScript.Singleton.InsideMinigamesMasterScreenRef.SetActive(true);
		//		UIGlobalVariablesScript.Singleton.MinigamesMenuMasterScreenRef.SetActive(false);
		//	UIGlobalVariablesScript.Singleton.SpaceshipGameScreenRef.SetActive(false);
		UIGlobalVariablesScript.Singleton.GunGameInterface.SetActive(false);
		
		//UIGlobalVariablesScript.Singleton.MainCharacterRef.SetActive(false);
		//		UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>().enabled = false;
		//		UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<ObjectLookAtDeviceScript>().enabled = false;
		//
		//			UIGlobalVariablesScript.Singleton.Joystick.CharacterAnimationRef = UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<AnimationControllerScript>();
		//			UIGlobalVariablesScript.Singleton.Joystick.CharacterControllerRef = UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterControllerScript>();
		
		
		//UIGlobalVariablesScript.Singleton.ARSceneRef.SetActive(false);
		UIGlobalVariablesScript.Singleton.JumbButton.SetActive(false);
		UIGlobalVariablesScript.Singleton.ShootButton.SetActive(false);
		
		UIGlobalVariablesScript.Singleton.Shadow.transform.localScale = new Vector3(0.79f, 0.79f, 0.79f);
		
		
		
		
		
		
		
		//From case UIFunctionalityId.PlayMinigameCubeRunners:
		
		UIGlobalVariablesScript.Singleton.GunGameScene.GetComponent<GunsMinigameScript>().Reset();
		//UIGlobalVariablesScript.Singleton.CuberunnerGamesScreenRef.SetActive(true);
		UIGlobalVariablesScript.Singleton.GunGameScene.SetActive(true);
		UIGlobalVariablesScript.Singleton.GunGameInterface.SetActive(true);
		//UIGlobalVariablesScript.Singleton.CubeRunnerMinigameSceneRef.GetComponent<MinigameCollectorScript>().HardcoreReset();
		UIGlobalVariablesScript.Singleton.GunGameScene.transform.localPosition = Vector3.zero;

		//UIClickButtonMasterScript.SavedRadius = UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterController>().radius;
		UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterController>().radius = 0.51f;
		UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterControllerScript>().SetLocal(true);
		
		UIGlobalVariablesScript.Singleton.Joystick.gameObject.SetActive(true);	
		UIGlobalVariablesScript.Singleton.JoystickArt.SetActive(true);	
		
		
		UIGlobalVariablesScript.Singleton.Joystick.CharacterAnimationRef = UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<MinigameAnimationControllerScript>();
		UIGlobalVariablesScript.Singleton.Joystick.CharacterControllerRef = UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterControllerScript>();
		
		
		//UIGlobalVariablesScript.Singleton.ShootButton.SetActive(true);
		
		UIGlobalVariablesScript.Singleton.MainCharacterRef.transform.parent = UIGlobalVariablesScript.Singleton.GunGameScene.transform;
		
		//UIGlobalVariablesScript.Singleton.MainCharacterRef.transform.localScale = new Vector3(0.026f, 0.026f, 0.025f);
		//				Camera.main.GetComponent<MusicScript>().PlayGun();
	}
	
	// Update is called once per frame
	void Update () {

	}
	void Awake(){

        if (ProfilesManagementScript.Singleton.CurrentProfile == null) {
			Debug.Log ("PlayerProfileData.ActiveProfile = null!");
            ProfilesManagementScript.Singleton.CurrentProfile = PlayerProfileData.CreateNewProfile("Dummy");
            ProfilesManagementScript.Singleton.CurrentAnimin = ProfilesManagementScript.Singleton.CurrentProfile.Characters [(int)PersistentData.TypesOfAnimin.Pi];
		}


		this.GetComponent<CharacterSwapManagementScript>().LoadCharacter(ProfilesManagementScript.Singleton.CurrentAnimin.PlayerAniminId, ProfilesManagementScript.Singleton.CurrentAnimin.AniminEvolutionId);
		this.GetComponent<MinigameAnimationControllerScript>().LoadAnimator(this.GetComponent<CharacterSwapManagementScript>().CurrentModel);

		Debug.Log("Awake");
	}
}
