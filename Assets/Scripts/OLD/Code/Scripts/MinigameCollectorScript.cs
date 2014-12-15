using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;


public class MinigameCollectorScript : MonoBehaviour 
{
	public enum TutorialStateId
	{
		None = 0,
		ShowMovement,
		ShowJumb,
		ShowSwipeLevel,
		Completed,
	}

	private enum MinigameStateId
	{
		Playing = 0,
		ExitMinigame,
	}

	private MinigameStateId State;

	//private GameObject[,] CubeMatrix;

	private List<GameObject> Collections = new List<GameObject>();
	public GameObject CharacterRef;

	//private List<GameObject> AvailableSpotsToPlaceStars = new List<GameObject>();

	public List<GameObject> EvilCharacters = new List<GameObject>();
	public GameObject[] EvilCharacterPool;
	public GameObject StarShardPrefab;

	private int oldLevelId = -1;
	private int currentLevelId = -1;
	public List<int> CompletedLevels = new List<int>();
	public const int MaxLevels = 4;

	private int Hearts;
	private int StarsCollected;
	private int Points;
	public TutorialStateId TutorialId;
    public void ResetTutorial(){
        TutorialId = TutorialStateId.None;
    }
	public GameObject TutorialMoveGraphic;
	public GameObject TutorialJumbGraphic;
	public GameObject TutorialSwipeLevelGraphic;
	public GameObject TutorialHandGraphic;

	//private const int MapWidth = 5;
	//private const int MapHeight = 5;

	private List<int> LevelsToComplete = new List<int>();

	//private Vector3 CenterOffset = new Vector3(0.5f, 0, 0.5f);
	//private Vector3 CubeScaleSize = new Vector3(0.2f, 0.20f, 0.2f);
	public GameObject Stage;
	private float? GameStartDelay;

    private GameObject[] m_HeartUI, m_StarsUI;
    private GameObject[] HeartUI{
        get {
            if (m_HeartUI==null)m_HeartUI = UiPages.GetPage(Pages.CubeMinigamePage).GetComponent<CubeMinigamesPageControls>().Hearts;
            return m_HeartUI;
        }
    }
    private GameObject[] StarsUI{ 
        get {
            if (m_StarsUI==null)m_StarsUI = UiPages.GetPage(Pages.CubeMinigamePage).GetComponent<CubeMinigamesPageControls>().Stars;
            return m_StarsUI;
        }
    }

    void Start(){
        UIGlobalVariablesScript.Singleton.Shadow.transform.localScale = new Vector3(0.79f, 0.79f, 0.79f);
        UIGlobalVariablesScript.Singleton.CubeRunnerMinigameSceneRef.SetActive(true);
        UIGlobalVariablesScript.Singleton.CubeRunnerMinigameSceneRef.GetComponent<MinigameCollectorScript>().HardcoreReset();

        UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterController>().radius = 0.51f;

        UIGlobalVariablesScript.Singleton.MainCharacterRef.transform.parent = UIGlobalVariablesScript.Singleton.CubeRunnerMinigameSceneRef.transform;

        UIGlobalVariablesScript.Singleton.MainCharacterRef.transform.localScale = new Vector3(0.026f, 0.026f, 0.025f);
    }

	// Use this for initialization
	void Awake () 
	{
        if (ProfilesManagementScript.Singleton.CurrentProfile == null) {
            Debug.Log ("PlayerProfileData.ActiveProfile = null!");
            ProfilesManagementScript.Singleton.CurrentProfile = PlayerProfileData.CreateNewProfile("Dummy");
            ProfilesManagementScript.Singleton.CurrentAnimin = ProfilesManagementScript.Singleton.CurrentProfile.Characters [(int)PersistentData.TypesOfAnimin.Pi];
        }

        UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterSwapManagementScript>().LoadCharacter(ProfilesManagementScript.Singleton.CurrentAnimin.PlayerAniminId, ProfilesManagementScript.Singleton.CurrentAnimin.AniminEvolutionId);
        UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<MinigameAnimationControllerScript>().LoadAnimator(UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterSwapManagementScript>().CurrentModel);

		//Debug.Log("STAGES AWAKE!!: " + Stage.transform.childCount.ToString());

		for(int i=0;i<Stage.transform.childCount;++i)
		{
			// its a level!
			if(Stage.transform.GetChild(i).childCount > 0)
			{
				//Debug.Log("STAGES 1");
				Transform stageTransform = Stage.transform.GetChild(i);

				for(int a=0;a<stageTransform.childCount;++a)
				{
					//Debug.Log("STAGES 2");
					if(stageTransform.GetChild(a).name.StartsWith("cubes"))
					{
						//Debug.Log("STAGES 3");
						Transform stageCubes = stageTransform.GetChild(a);
						for(int b=0;b<stageCubes.childCount;++b)
						{
							stageCubes.GetChild(b).gameObject.AddComponent<MeshCollider>();
							CubeAnimatonScript script = stageCubes.GetChild(b).gameObject.AddComponent<CubeAnimatonScript>();
							script.ResetPosition = script.transform.localPosition;
							script.ValueNext = script.transform.localPosition;
							script.enabled = false;


//							if(script.transform.childCount > 0)
//							{
//								EvilCharacterPatternMovementScript component = script.gameObject.AddComponent<EvilCharacterPatternMovementScript>();
//								
//								component.Pattern = new Vector3[script.transform.childCount];
//								for(int c=0;c<script.transform.childCount;++c)
//								{
//									component.Pattern[c] = script.transform.GetChild(c).transform.localPosition;
//								}
//								
//								component.transform.localPosition = script.transform.GetChild(0).transform.localPosition;
//								component.Speed = 2.1f;
//								component.Lerp = 0;
//								component.Index = 0;
//								component.enabled = false;
//							}

						}
					}
				}
			}
		}
	}


	void OnGUI()
	{
//		GameObject obj = GameObject.Find("TextureBufferCamera");
//
//		GameObject hole = GameObject.Find("insideHole");
//
//		Texture texture = obj.GetComponent<Camera>().targetTexture;
//		hole.renderer.material.mainTexture = texture;
//
//
//		hole.renderer.material.SetTextureScale (
//			"_MainTex", 
//			new Vector2(0.05f, 0.05f)
//			);
//		
//		hole.renderer.material.SetTextureOffset (
//			"_MainTex", 
//			new Vector2(0.5f, 0.5f)
//			);

		//GUI.DrawTexture(new Rect(0, 0, 200, 200), texture);
	}

	private bool CanBeginLevelSwipe()
	{
        //Debug.Log("Begin Level Swipe : [" + UiPages.IsMouseOverUI() + "];");
        return !UiPages.IsMouseOverUI();
	}


	private void EnterMinigame()
	{
		CharacterProgressScript progressScript = UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>();

		
		
		UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<AnimationControllerScript>().IsExitPortal = true;
		UIGlobalVariablesScript.Singleton.MainCharacterRef.transform.rotation = Quaternion.Euler(0, 180, 0);
		UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterControllerScript>().ResetRotation();
		
		UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<AnimateCharacterOutPortalScript>().Timer = 0;
		UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<AnimateCharacterOutPortalScript>().JumbId = AnimateCharacterOutPortalScript.JumbStateId.Jumbout;
		UIGlobalVariablesScript.Singleton.SoundEngine.Play(ProfilesManagementScript.Singleton.CurrentAnimin.PlayerAniminId, ProfilesManagementScript.Singleton.CurrentAnimin.AniminEvolutionId, CreatureSoundId.JumbOutPortal);
		//progressScript.CurrentAction = ActionId.EnterPortalToNonAR;
		

		UIGlobalVariablesScript.Singleton.ARPortal.GetComponent<PortalScript>().Show(PortalStageId.MinigameCuberRunners, false);
	}
	
	
	public void ExitMinigame(bool succesfullyCompleted)
	{
		UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterControllerScript>().Forces.Clear();
        //UIClickButtonMasterScript.HandleClick(UIFunctionalityId.CloseCurrentMinigame, null);
        BetweenSceneData.Instance.ReturnFromMiniGame = true;
        MainARHandler.Instance.ChangeSceneToCaring();
	}
	
	
	Vector3 lastMousePosition;

	private bool isSwipingAllowed;
	private float snapAngle;
	private float AngleInTransform;
	private float TimeStartedSwipe;
	private float SnapAngleDifference;
	// Update is called once per frame

    private bool m_IsSwiping;
    public bool IsSwiping{ get { return m_IsSwiping; } }

    private bool m_Paused;
    public bool Paused
    {
        set
        {
            m_Paused = value;

            Animator animator = UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponentInChildren<Animator>();
            if (animator!=null) animator.enabled = !value;                           //Pause the character's Animation
            EvilCharacterPatternMovementScript[] evilScripts = this.GetComponentsInChildren<EvilCharacterPatternMovementScript>();
            for (int i = 0; i < evilScripts.Length; i++)
            {
                evilScripts[i].Paused = value;                                                                                      //Pause each enemy individually
            }
        }
        get { return m_Paused; }
    }

	void Update () 
	{
        if (Paused) return;
        UiPages.GetPage(Pages.CubeMinigamePage).GetComponent<CubeMinigamesPageControls>().PointLabel.text = Points.ToString() + " pts";
		BetweenSceneData.Instance.Points = Points;
		CharacterProgressScript progressScript = UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>();

		if(State == MinigameStateId.ExitMinigame)
		{
			progressScript.CurrentAction = ActionId.ExitPortalMainStage;
		}
		else
        {
			if(Input.GetButtonDown("Fire1") && CanBeginLevelSwipe())
            {
                m_IsSwiping = true;
				lastMousePosition = Input.mousePosition;
				isSwipingAllowed = true;
				TimeStartedSwipe = Time.time;
				SnapAngleDifference = 0;

			}
			else if(Input.GetButton("Fire1") && isSwipingAllowed)
            {
                m_IsSwiping = true;
				SnapAngleDifference += (lastMousePosition.x - Input.mousePosition.x) * 0.09f;
				snapAngle += (lastMousePosition.x - Input.mousePosition.x) * 0.09f;
				lastMousePosition = Input.mousePosition;
			}
			else if(Input.GetButtonUp("Fire1") && isSwipingAllowed)
			{
                m_IsSwiping = true;
				//fast swipe
				if((Time.time - TimeStartedSwipe) <= 0.4f)
				{
					snapAngle -= SnapAngleDifference;

					// rotate right
					if(Input.mousePosition.x >= lastMousePosition.x)
					{
						snapAngle  -= 90.0f;
						snapAngle = (int)(snapAngle / 90.0f) * 90f;
						//Debug.Log("PATH A");/
					}
					// rotate left
					else
					{
						snapAngle  += 90.0f;
						snapAngle = (int)(snapAngle / 90.0f) * 90f;
						//Debug.Log("PATH B");
					}
				}
				// slow swipe
				else
				{
				}
            }
			else
            {
                m_IsSwiping = false;
                isSwipingAllowed = false;
			}


			AngleInTransform = Mathf.Lerp(AngleInTransform, snapAngle, Time.deltaTime * 7);

			float finalTransform = AngleInTransform;

			UIGlobalVariablesScript.Singleton.CubeRunnerMinigameSceneRef.transform.rotation = Quaternion.Euler(
				UIGlobalVariablesScript.Singleton.CubeRunnerMinigameSceneRef.transform.rotation.eulerAngles.x,
				finalTransform,
				UIGlobalVariablesScript.Singleton.CubeRunnerMinigameSceneRef.transform.rotation.eulerAngles.z
				);



			//Debug.Log("AngleInTransform - snapAngle:" + Mathf.Abs(AngleInTransform - snapAngle).ToString());

			if( Mathf.Abs(AngleInTransform - snapAngle) <= 0.1f)
			{
				AngleInTransform = snapAngle;
				UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterControllerScript>().FreezeCollisionDetection = false;
			}
			else
			{
				UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterControllerScript>().FreezeCollisionDetection = true;
			}
			if(GameStartDelay.HasValue)
			{
				GameStartDelay -= Time.deltaTime;

				if(GameStartDelay <= 0)
				{
					for(int i=0;i<EvilCharacters.Count;++i)
						EvilCharacters[i].SetActive(true);

					if(oldLevelId != -1)
					{
						Stage.transform.GetChild(oldLevelId).gameObject.SetActive(false);
					}
					UIGlobalVariablesScript.Singleton.MainCharacterRef.SetActive(true);
					GameStartDelay = null;
					ResetCharacter();

					Transform newTransform = Stage.transform.GetChild(currentLevelId);
					
					for(int a=0;a<newTransform.childCount;++a)
					{
						if(newTransform.GetChild(a).name.StartsWith("cubes"))
						{
							Transform cubes = newTransform.GetChild(a);
						}
					}
				}
			}
			else
			{
				CheckForPickupCollision ();

				if (CharacterRef.transform.localPosition.y <= -1f) //This is for falling, I think.
				{
					UIGlobalVariablesScript.Singleton.SoundEngine.Play(GenericSoundId.Fall_Through_Levels);

					LoseHeart(true);
				}
			}
			//UICOMMENT: UIGlobalVariablesScript.Singleton.TextForStarsInMiniCollector.text = StarsCollected.ToString();
            UiPages.GetPage(Pages.CubeMinigamePage).GetComponent<CubeMinigamesPageControls>().LevelCounter.text= StarsCollected.ToString();
		}
	}
	private void LoseHeart(bool resetIfZero)
	{
		Hearts--;
		if( Hearts >0)
		{
            GameObject heart = UiPages.GetPage(Pages.CubeMinigamePage).GetComponent<CubeMinigamesPageControls>().Hearts[Hearts];
            if (heart!=null) heart.SetActive(false);

			if(resetIfZero) Reset();
		}
		else
		{
			ExitMinigame(false);

		}
	}

	void ResetCharacter()
	{
		//CharacterRef.transform.localPosition = Vector3.zero;
		CharacterRef.GetComponent<CharacterControllerScript>().IsResetFalling = true;


		Transform stageTransform = Stage.transform.GetChild(currentLevelId);
		
		for(int a=0;a<stageTransform.childCount;++a)
		{
			//Debug.Log("STAGES 2");
			if(stageTransform.GetChild(a).name.StartsWith("dummies"))
			{
				//Debug.Log("STAGES 3");
				Transform dummies = stageTransform.GetChild(a);
				for(int b=0;b<dummies.childCount;++b)
				{
					if(dummies.GetChild(b).name.StartsWith("start"))
					{
//						Debug.Log("STAGES 4: " + dummies.GetChild(b).transform.position.ToString());
						CharacterRef.transform.position = dummies.GetChild(b).transform.position;
					}
				}
			}
		}
			


	}
	/*
	void OnGUI()
	{
		//GUI.skin.label.fontSize = 30;
		//GUI.skin.button.fontSize = 30;


		if (GUI.Button (new Rect (10, 10, 120, 50), "Reset")) {
			Reset();
			
		}
		//GUI.Label (new Rect (Screen.width - 500, 10, 200, 50), "Stars: " + StarsOwned.ToString());
	}
*/

	public void OnEvilCharacterHitFromTop(GameObject gameObject)
	{
		for(int i=0;i<EvilCharacters.Count;++i)
		{
			if(EvilCharacters[i] == gameObject)
			{
				//Debug.Log("HIT FROM ABOVE");
				UIGlobalVariablesScript.Singleton.SoundEngine.Play(GenericSoundId.Kill_Baddy);

				gameObject.AddComponent<EnemyDeathAnimationScript>();
				EvilCharacterPatternMovementScript movementScript = gameObject.GetComponent<EvilCharacterPatternMovementScript>();
				if(movementScript != null) 
					movementScript.enabled = false;

				//EvilCharacters.RemoveAt(i);
				//i--;

				//Debug.Log("chars found:" + EvilCharacters.Count.ToString());
			}
		}
	}

	public void OnEvilCharacterHit(GameObject gameObject)
	{
		//StarsCollected -= 3;

		gameObject.GetComponent<BoxCollider>().enabled = false;

		gameObject.AddComponent<TemporaryDisableCollisionEvent>();

		//TemporaryDisableCollisionEvent collisionEvent = new TemporaryDisableCollisionEvent(gameObject);
		//PresentationEventManager.Create(collisionEvent);
		UIGlobalVariablesScript.Singleton.SoundEngine.Play(GenericSoundId.Bump_Into_Baddy);

		Debug.Log("ADDING FORCE");
		CharacterRef.GetComponent<CharacterControllerScript>().Forces.Add(
			new CharacterForces() { Speed = 900, Direction = -CharacterRef.transform.forward, Length = 0.3f }
		);


		LoseHeart(false);

		//CharacterRef.AddComponent<FlashMaterialColorScript>();
	}

	/*private void UpdateAnimations()
	{
		for (int i=0; i<AnimationJobs.Count; ++i) {

			AnimationJobs [i].Lerp += Time.deltaTime;
			if (AnimationJobs [i].Lerp >= 1) {
				AnimationJobs [i].Lerp = 1;
			}

			AnimationJobs [i].AnimatedObject.transform.localPosition = Vector3.Lerp (AnimationJobs [i].StartPosition, AnimationJobs [i].EndPosition, Mathf.Sin (AnimationJobs [i].Lerp * 90 * Mathf.Deg2Rad));
		
			if (AnimationJobs [i].Lerp >= 1) {
				AnimationJobs.RemoveAt (i);
				i--;
			}
		
		}
	}*/

	private void CheckForPickupCollision()
	{
		for (int i=0; i<Collections.Count; ++i) {
				
			float d = Vector3.Distance(Collections[i].transform.position, CharacterRef.transform.position);
//			Debug.Log(d);
			if(d <= 25)
			{
				Points += 200;
				ProfilesManagementScript.Singleton.CurrentAnimin.Fitness += 10;
				GameObject.Destroy(Collections[i]);

				UIGlobalVariablesScript.Singleton.SoundEngine.Play(GenericSoundId.Star_Collect);

				int starsActive = 0;
				for(int a=0;a<StarsUI.Length;++a)
				{
                    if (StarsUI[a] == null)
                        continue;
					if(!StarsUI[a].activeSelf)
					{
						starsActive++;
						StarsUI[a].SetActive(true);
						break;
					}
					else starsActive++;
				}

				if(starsActive == 1) UIGlobalVariablesScript.Singleton.SoundEngine.Play(GenericSoundId.CollectStar1);
				else if(starsActive == 2) UIGlobalVariablesScript.Singleton.SoundEngine.Play(GenericSoundId.CollectStar2);
				else if(starsActive == 3) UIGlobalVariablesScript.Singleton.SoundEngine.Play(GenericSoundId.CollectStar3);
				else if(starsActive == 4) UIGlobalVariablesScript.Singleton.SoundEngine.Play(GenericSoundId.CollectStar4);
				else if(starsActive == 5) UIGlobalVariablesScript.Singleton.SoundEngine.Play(GenericSoundId.CollectStar5);

				if(starsActive >= 5) 
				{

					Points += 1500;
					StarsCollected++;
//					for(int a=0;a<StarsUI.Length;++a)
//					{
//						StarsUI[a].SetActive(false);
//					}
				}

				Collections.RemoveAt(i);
				i--;

				if(StarsCollected >= 10)
				{
					ExitMinigame(true);
					//UIClickButtonMasterScript.HandleClick(UIFunctionalityId.CloseCurrentMinigame, null);
					break;
				}
				else if (Collections.Count <= 0) {
					UIGlobalVariablesScript.Singleton.SoundEngine.Play(GenericSoundId.Star_Complete);
					LevelsToComplete.Remove(currentLevelId);
					Reset ();
				}
			}
		}
	}


	public void HardcoreReset()
	{
		TutorialId = TutorialStateId.None;
		Points = 0;
		State = MinigameStateId.Playing;
		Hearts = 3;
		oldLevelId = -1;
		currentLevelId = -1;
		CompletedLevels.Clear();
		StarsCollected = 0;

		for(int i=0;i<StarsUI.Length;++i)
            if (StarsUI[i] != null) StarsUI[i].SetActive(false);
		for(int i=0;i<HeartUI.Length;++i)
            if (HeartUI[i] != null) HeartUI[i].SetActive(true);

		LevelsToComplete.Clear();
		for(int i=0;i<Stage.transform.childCount;++i)
		{
			if(Stage.transform.GetChild(i).transform.childCount > 0)
			{
				LevelsToComplete.Add(i);
			}
		}


		for(int i=0;i<Stage.transform.childCount;++i)
		{
			// its a level!
			if(Stage.transform.GetChild(i).childCount > 0)
			{
				//Debug.Log("STAGES 1");
				Transform stageTransform = Stage.transform.GetChild(i);
				
				for(int a=0;a<stageTransform.childCount;++a)
				{
					//Debug.Log("STAGES 2");
					if(stageTransform.GetChild(a).name.StartsWith("cubes"))
					{
						//Debug.Log("STAGES 3");
						Transform stageCubes = stageTransform.GetChild(a);
						for(int b=0;b<stageCubes.childCount;++b)
						{

							CubeAnimatonScript script = stageCubes.GetChild(b).gameObject.GetComponent<CubeAnimatonScript>();
							script.transform.localPosition = script.ResetPosition;
							script.ValueNext = script.ResetPosition;
							script.enabled = false;

							//EvilCharacterPatternMovementScript patternScript = script.GetComponent<EvilCharacterPatternMovementScript>();
							//if(patternScript != null) patternScript.enabled = false;
						}
					}
				}
			}
		}
		Reset();
	}

	public void Reset()
	{
		EvilCharacters.Clear();
		UIGlobalVariablesScript.Singleton.SoundEngine.Play(GenericSoundId.Grid_Cubes_Fall);


		for(int i=0;i<StarsUI.Length;++i)
            if (StarsUI[i]!=null) StarsUI[i].SetActive(false);

		for(int i=0;i<EvilCharacterPool.Length;++i)
		{
			EvilCharacterPool[i].SetActive(false);

			EvilCharacterPatternMovementScript component = EvilCharacterPool[i].GetComponent<EvilCharacterPatternMovementScript>();
			if(component != null) Destroy(component);
		}

		//AvailableSpotsToPlaceStars.Clear ();


		for (int i=0; i<Collections.Count; ++i) {
			GameObject.Destroy (Collections [i]);
		}
		Collections.Clear ();

		Transform stageTransformOld = null;
		// FADE OUT EXISTING LEVEL
		if(currentLevelId != -1)
		{
			stageTransformOld = Stage.transform.GetChild(currentLevelId);

			for(int a=0;a<stageTransformOld.childCount;++a)
			{
				if(stageTransformOld.GetChild(a).name.StartsWith("cubes"))
				{
					Transform cubes = stageTransformOld.GetChild(a);
					
					for(int b=0;b<cubes.childCount;++b)
					{
						CubeAnimatonScript cubeScript = cubes.GetChild(b).gameObject.GetComponent<CubeAnimatonScript>();

						cubeScript.ValueNext = cubeScript.transform.localPosition + new Vector3(0, -35, 0);
						//cubeScript.transform.position = cubeScript.transform.position + new Vector3(0, -200, 0);
						cubeScript.Delay = 0.03f * b;
						cubeScript.ResetPosition = cubeScript.transform.localPosition;
						cubeScript.enabled = true;

						//EvilCharacterPatternMovementScript patternScript = cubeScript.GetComponent<EvilCharacterPatternMovementScript>();
						//if(patternScript != null) patternScript.enabled = false;
					}
				}
			}
		}

		oldLevelId = currentLevelId;
		if(currentLevelId == -1)
		{
			currentLevelId = LevelsToComplete[0];
		}
		else if(LevelsToComplete.Count > 0)
		{
			LevelsToComplete.Remove(currentLevelId);
			currentLevelId = LevelsToComplete[UnityEngine.Random.Range(0, LevelsToComplete.Count)];
		}


		//Debug.Log("Stage.transform.childCount: " + Stage.transform.childCount.ToString());
		for(int i=0;i<Stage.transform.childCount;++i)
		{
			if(Stage.transform.GetChild(i).childCount > 0)
			{
				if(stageTransformOld != Stage.transform.GetChild(i).transform)
					Stage.transform.GetChild(i).gameObject.SetActive(false);
			}
		}

		Stage.transform.GetChild(currentLevelId).gameObject.SetActive(true);
		//Debug.Log("Stage.transform.GetChild(currentLevelId): " + Stage.transform.GetChild(currentLevelId).name);


		GameStartDelay = 0.5f;

		int badGuyCounter = 0;
		Transform stageTransform = Stage.transform.GetChild(currentLevelId);

		// BUILD BAD GYES
		for(int a=0;a<stageTransform.childCount;++a)
		{

			if(stageTransform.GetChild(a).name.StartsWith("dummies"))
			{
				Transform dummies = stageTransform.GetChild(a);
				for(int b=0;b<dummies.childCount;++b)
				{
					if(dummies.GetChild(b).name.StartsWith("badguy"))
					{
						if(badGuyCounter >= EvilCharacterPool.Length) continue;


						CharacterRef.transform.position = dummies.GetChild(b).transform.position;

						EvilCharacterPool[badGuyCounter].SetActive(true);
						EvilCharacterPool[badGuyCounter].transform.parent = dummies.GetChild(b).transform.parent;
						//EvilCharacterPool[badGuyCounter].transform.localPosition = dummies.GetChild(b).transform.localPosition;

						EvilCharacterPatternMovementScript component = EvilCharacterPool[badGuyCounter].AddComponent<EvilCharacterPatternMovementScript>();
						component.Pattern = new Vector3[dummies.GetChild(b).transform.childCount];
						for(int c=0;c<dummies.GetChild(b).childCount;++c)
						{
							component.Pattern[c] = dummies.GetChild(b).transform.GetChild(c).transform.localPosition;
						}
						component.transform.localPosition = dummies.GetChild(b).transform.GetChild(0).transform.localPosition;
						component.Speed = 2.1f;
						component.Lerp = 0;
						component.Index = 0;
						component.ApplyRotation = true;

						EvilCharacterPool[badGuyCounter].GetComponent<BoxCollider>().enabled = true;
						for(int i=0;i<EvilCharacterPool[badGuyCounter].transform.childCount;++i)
						{
							if(EvilCharacterPool[badGuyCounter].transform.GetChild(i).name == "Sphere")
								EvilCharacterPool[badGuyCounter].transform.GetChild(i).gameObject.SetActive(false);
							else 
								EvilCharacterPool[badGuyCounter].transform.GetChild(i).gameObject.SetActive(true);
						}



						//EvilCharacterPool[badGuyCounter].GetComponent<Animator>().SetBool("None", false );
						//EvilCharacterPool[badGuyCounter].transform.localScale = new Vector3(EvilCharacterPool[badGuyCounter].transform.localScale.x, EvilCharacterPool[badGuyCounter].transform.localScale.x, EvilCharacterPool[badGuyCounter].transform.localScale.x);
						EvilCharacters.Add(EvilCharacterPool[badGuyCounter]);
						badGuyCounter++;

					}

					// BUILD STARS
					if(dummies.GetChild(b).name.StartsWith("star") && !dummies.GetChild(b).name.StartsWith("start"))
					{
						//GameObject randomParent = CubeMatrix [(int)builder.CollectionPoints[i].x, (int)builder.CollectionPoints[i].y];
						
						GameObject collection = GameObject.CreatePrimitive(PrimitiveType.Sphere);
						Destroy (collection.rigidbody);
						
						Collections.Add (collection);
						
						
						//GameObject randomParent = AvailableSpotsToPlaceStars [Randomizer.Next (0, AvailableSpotsToPlaceStars.Count)];
						//AvailableSpotsToPlaceStars.Remove (randomParent);

						collection.transform.parent = dummies.GetChild(b).transform.parent;
						collection.transform.localPosition = dummies.GetChild(b).transform.localPosition;
						collection.transform.localScale = new Vector3(1, 1, 1);
						//collection.transform.localRotation = Quaternion.identity;
						//collection.transform.localPosition = new Vector3(-1, 0.3f, -1.0f);
						
						collection.AddComponent<OscillationUpDownScript>();
						
						SphereCollider colliderToKill = collection.GetComponent<SphereCollider>();
						Destroy(colliderToKill);
						
					}
				}
			}
		}

	

		for(int i=0;i<EvilCharacters.Count;++i)
			EvilCharacters[i].SetActive(false);

		UIGlobalVariablesScript.Singleton.MainCharacterRef.SetActive(false);

		//Debug.Log("stageTransform.childCount: " + stageTransform.childCount.ToString());

		// BUILD ANIMATION FOR CUBES
		for(int a=0;a<stageTransform.childCount;++a)
		{
			if(stageTransform.GetChild(a).name.StartsWith("cubes"))
			{
				Transform cubes = stageTransform.GetChild(a);
				//Debug.Log("cubes: " + cubes.childCount.ToString());

				for(int b=0;b<cubes.childCount;++b)
				{
//					Debug.Log("adding cube animation down to: " + cubes.GetChild(b).name);
					CubeAnimatonScript cubeScript = cubes.GetChild(b).gameObject.GetComponent<CubeAnimatonScript>();
					cubeScript.ValueNext = cubeScript.ResetPosition;
					cubeScript.transform.localPosition = cubeScript.ResetPosition + new Vector3(0, 80, 0);
					cubeScript.Delay = 0.2f + 0.04f * b;
					GameStartDelay += 0.04f;
					cubeScript.enabled = true;

					//EvilCharacterPatternMovementScript patternScript = cubeScript.GetComponent<EvilCharacterPatternMovementScript>();
					//if(patternScript != null) patternScript.enabled = false;
					
				}
			}
		}

	}
}

