using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UiImage = UnityEngine.UI.Image;

public class GunsMinigameScript : MonoBehaviour//Photon.MonoBehaviour
{
    [SerializeField]
    private GameObject m_MainCharacter;

    public enum GameStateId
    {
        Initialize = 0,
        PrepareToStart,
        PrepareToStart3,
        PrepareToStart2,
        PrepareToStart1,
        PrepareToStartGO,
        Countdown,
        Playing,
        Paused,
        Completed,
        PrepareToExit,
        WaitForPlayersToConnect,
    }

    //public GameObject GunPrefab;
    public string[] BulletPrefab;
    public string[] Barrels;
    public string[] EnemyPrefabs;
    public string[] SpecialBarrels;
    public string[] BulletSplats;

    //public Image MeterBarBackground;
	public List<string> CurrentBullets = new List<string>();
	private GameStateId m_State;
	public GameStateId State{ get { return m_State; } }
	private float AmmoTimer;
    private float NextBarrelSpawnTimer;
    // public List<GameObject> SpawnedObjects = new List<GameObject>();
    public Texture2D[] SlimeTextures;
    public Texture2D[] SlimeLevel2Textures;
    public Color[] SlimeColors;
    public GameObject[] MonsterSplatPrefabs;
    //public Texture2D[] BarTextures;
    public Texture FrontBar;

    private const float BarrelSpawnMinTime = 1;
    private const float BarrelSpawnMaxTime = 7;
    private int Wave;
    private int[] WaveTimers = new int[] { 2, 10, 10, 10, 10, 10, 10, 10, 10, 10 };
    private int[] WaveMinEnemies = new int[] { 2, 3, 4, 5, 6, 7, 8, 9, 11, 15 };
    private int[] WaveMaxEnemies = new int[] { 3, 4, 5, 6, 7, 8, 9, 10, 12, 17 };
    private float WaveTimerForNext;
    private float AutoShootCooldown;
    private float RandomCubeTimer;
    public GameObject RandomCubePrefab;
    public Text PointsLabel;
    public int Points;
    public Sprite BulletIcon;
    public GameObject ArenaStage;

    public GameObject LocalPlayerCharacter;
    public List<GameObject> PlayersCharacters = new List<GameObject>();

    public GameObject SpawnedObjectsEnemies;
    public GameObject SpawnedObjectsAllOthers;

	private bool m_Go321Done = false;
	public bool Go321Done { get { return m_Go321Done; } }

    private float FillUpTimer;

    private GunMinigamePageControls m_UIControls;

    private GunMinigamePageControls UIControls
    {
        get
        {
            if (m_UIControls == null)
                m_UIControls = UiPages.GetPage(Pages.GunMinigamePage).GetComponent<GunMinigamePageControls>();
            return m_UIControls;
        }
    }
    [SerializeField]
    private GameObject TutorialEnemies;
    [SerializeField]
    private GameObject TutorialMove;

    private GameStateId m_StateBeforePaused = GameStateId.Paused;
    private bool m_Paused;

    private float m_InvulnTimer = 0f;

    public bool Paused
    {
        set
        {
            m_Paused = value;
			
			UiPages.GetPage(Pages.GunMinigamePage).GetComponent<GunMinigamePageControls>().Paused = value;
            Animator animator = UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponentInChildren<Animator>();
            if (animator != null)
                animator.enabled = !value;                           //Pause the character's Animation

            GunGameEnemyScript[] evilScripts = this.GetComponentsInChildren<GunGameEnemyScript>();
            for (int i = 0; i < evilScripts.Length; i++)
            {
                evilScripts[i].Paused = value;                                                                       //Pause each enemy individually
            }
            if (m_Paused)
            {
                m_StateBeforePaused = m_State;
                m_State = GameStateId.Paused;
                UIGlobalVariablesScript.Singleton.SoundEngine.StopLoop();
            }
            else
            {
                m_State = m_StateBeforePaused;
                UIGlobalVariablesScript.Singleton.SoundEngine.PlayLoop(GenericSoundId.GunLoop);
            }
        }
        get { return m_Paused; }
    }

	// New Wave System
	public static int waveCount;
	public static int enemyCount;
	private bool canSpawn;
	public static int destroyedEnemyCount;
	public Sprite blankSprite;
	
	public Sprite clear3;
	public Sprite clear2;
	public Sprite clear1;
	public Sprite clearGo;

//	public GameObject uiParticles;

	private bool clearWave;
	private bool spawnBarrels;
	private bool canShoot;

	public Sprite blueBar;
	public Sprite blueberryIcon;

	IEnumerator ClearWave(int wave)
	{
		clearWave = true;
		spawnBarrels = false;
		canShoot = false;
		UIGlobalVariablesScript.Singleton.SoundEngine.StopLoop();
		
		yield return StartCoroutine(UIControls.ShowWaveComplete(wave));
		
		if(wave < 10)
        {
            UIGlobalVariablesScript.Singleton.SoundEngine.PlayLoop(GenericSoundId.GunLoop);
		}

		clearWave = false;
		
		if(wave < 10)
		{
			spawnBarrels = true;
			canShoot = true;
			WaveTimerForNext = 1;
		}
		else
		{
            ExitMinigame();
		}
	}

	IEnumerator WaveOneEnemies ()
	{
		if (clearWave == false)
		{
			enemyCount = 3;
			for (int a = 0; a < enemyCount; ++a)
			{
				SpawnWaveOne(0);
			}
			yield return new WaitForSeconds (3.0f);
			enemyCount = 3;
			for (int a = 0; a < enemyCount; ++a)
			{
				SpawnWaveOne(0);
			}
		}
		// 6
	}

	IEnumerator WaveTwoEnemies ()
	{
		if (clearWave == false)
		{
			enemyCount = 3;
			for (int a = 0; a < enemyCount; ++a)
			{
				SpawnWaveTwo(0);
			}
			yield return new WaitForSeconds (3.0f);
			enemyCount = 3;
			for (int a = 0; a < enemyCount; ++a)
			{
				SpawnWaveTwo(0);
			}
			yield return new WaitForSeconds (5.0f);
			enemyCount = 4;
			for (int a = 0; a < enemyCount; ++a)
			{
				SpawnWaveTwo(0);
			}
			// 10
		}
	}

	IEnumerator WaveThreeEnemies ()
	{
		if (clearWave == false)
		{
			enemyCount = 4;
			for (int a = 0; a < enemyCount; ++a)
			{
				SpawnWaveThree(0);
			}
			yield return new WaitForSeconds (4.0f);
			enemyCount = 4;
			for (int a = 0; a < enemyCount; ++a)
			{
				SpawnWaveThree(0);
			}
			yield return new WaitForSeconds (6.0f);
			enemyCount = 6;
			for (int a = 0; a < enemyCount; ++a)
			{
				SpawnWaveThree(0);
			}
			// 14
		}
	}

	IEnumerator WaveFourEnemies ()
	{
		if (clearWave == false)
		{
			enemyCount = 6;
			for (int a = 0; a < enemyCount; ++a)
			{
				SpawnWaveFour(0);
			}
			yield return new WaitForSeconds (4.0f);
			enemyCount = 6;
			for (int a = 0; a < enemyCount; ++a)
			{
				SpawnWaveFour(0);
			}
			yield return new WaitForSeconds (6.0f);
			enemyCount = 6;
			for (int a = 0; a < enemyCount; ++a)
			{
				SpawnWaveFour(0);
			}
			// 18
		}
	}

	IEnumerator WaveFiveEnemies ()
	{
		if (clearWave == false)
		{
			enemyCount = 7;
			for (int a = 0; a < enemyCount; ++a)
			{
				SpawnWaveFive(0);
			}
			yield return new WaitForSeconds (4.0f);
			enemyCount = 7;
			for (int a = 0; a < enemyCount; ++a)
			{
				SpawnWaveFive(0);
			}
			yield return new WaitForSeconds (6.0f);
			enemyCount = 8;
			for (int a = 0; a < enemyCount; ++a)
			{
				SpawnWaveFive(0);
			}
			// 22
		}
	}

	IEnumerator WaveSixEnemies ()
	{
		if (clearWave == false)
		{
			enemyCount = 8;
			for (int a = 0; a < enemyCount; ++a)
			{
				SpawnWaveSix(0);
			}
			yield return new WaitForSeconds (5.0f);
			enemyCount = 8;
			for (int a = 0; a < enemyCount; ++a)
			{
				SpawnWaveSix(0);
			}
			yield return new WaitForSeconds (7.0f);
			enemyCount = 10;
			for (int a = 0; a < enemyCount; ++a)
			{
				SpawnWaveSix(0);
			}
			// 26
		}
	}

	IEnumerator WaveSevenEnemies ()
	{
		if (clearWave == false)
		{
			enemyCount = 9;
			for (int a = 0; a < enemyCount; ++a)
			{
				SpawnWaveSix(0);
			}
			yield return new WaitForSeconds (5.0f);
			enemyCount = 9;
			for (int a = 0; a < enemyCount; ++a)
			{
				SpawnWaveSix(0);
			}
			yield return new WaitForSeconds (7.0f);
			enemyCount = 12;
			for (int a = 0; a < enemyCount; ++a)
			{
				SpawnWaveSix(0);
			}
			// 30
		}
	}

	IEnumerator WaveEightEnemies ()
	{
		if (clearWave == false)
		{
			enemyCount = 10;
			for (int a = 0; a < enemyCount; ++a)
			{
				SpawnWaveSix(0);
			}
			yield return new WaitForSeconds (5.0f);
			enemyCount = 10;
			for (int a = 0; a < enemyCount; ++a)
			{
				SpawnWaveSix(0);
			}
			yield return new WaitForSeconds (7.0f);
			enemyCount = 14;
			for (int a = 0; a < enemyCount; ++a)
			{
				SpawnWaveSix(0);
			}
			// 34
		}
	}

	IEnumerator WaveNineEnemies ()
	{
		if (clearWave == false)
		{
			enemyCount = 12;
			for (int a = 0; a < enemyCount; ++a)
			{
				SpawnWaveSix(0);
			}
			yield return new WaitForSeconds (6.0f);
			enemyCount = 12;
			for (int a = 0; a < enemyCount; ++a)
			{
				SpawnWaveSix(0);
			}
			yield return new WaitForSeconds (8.0f);
			enemyCount = 14;
			for (int a = 0; a < enemyCount; ++a)
			{
				SpawnWaveSix(0);
			}
			// 38
		}
	}

	IEnumerator WaveTenEnemies ()
	{
		if (clearWave == false)
		{
			enemyCount = 14;
			for (int a = 0; a < enemyCount; ++a)
			{
				SpawnWaveSix(0);
			}
			yield return new WaitForSeconds (6.0f);
			enemyCount = 14;
			for (int a = 0; a < enemyCount; ++a)
			{
				SpawnWaveSix(0);
			}
			yield return new WaitForSeconds (8.0f);
			enemyCount = 14;
			for (int a = 0; a < enemyCount; ++a)
			{
				SpawnWaveSix(0);
			}
			// 42
		}
	}

	// Use this for initialization
    void Start()
    {
        CurrentBullets.Clear();
       // CurrentBullets.Add(BulletPrefab[Random.Range(0, BulletPrefab.Length)]);
		CurrentBullets.Add(BulletPrefab[1]);
		Paused = false;
		JoystickPageControls.Paused = false;

		GameObject.FindGameObjectWithTag("meterfill").GetComponent<UiImage>().sprite = blueBar;
		GameObject.FindGameObjectWithTag("metericon").GetComponent<UiImage>().sprite = blueberryIcon;

        m_State = GameStateId.Initialize;
		m_Go321Done = false;
		// Starting Wave
		waveCount = 1;
		enemyCount = 0;
		destroyedEnemyCount = 0;
		canSpawn = true;

		clearWave = false;
		spawnBarrels = true;
		canShoot = true;
		UIControls.SetWaveComplete(-1);

	//	uiParticles = GameObject.FindGameObjectWithTag("uiparticles");
	//	uiParticles.GetComponent<ParticleSystem>().particleSystem.Stop();

    }

    void Awake()
    {
        if (ProfilesManagementScript.Instance.CurrentProfile == null)
        {
            Debug.Log("PlayerProfileData.ActiveProfile = null!");
            ProfilesManagementScript.Instance.CurrentProfile = PlayerProfileData.CreateNewProfile("Dummy");
            ProfilesManagementScript.Instance.CurrentAnimin = ProfilesManagementScript.Instance.CurrentProfile.Characters[(int)PersistentData.TypesOfAnimin.Pi];
        }

        UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterSwapManagementScript>().LoadCharacter(ProfilesManagementScript.Instance.CurrentAnimin.PlayerAniminId, ProfilesManagementScript.Instance.CurrentAnimin.AniminEvolutionId, false);
        UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<MinigameAnimationControllerScript>().LoadAnimator(UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterSwapManagementScript>().CurrentModel);
    }

    // Update is called once per frame
    void Update()
    {
        float LerpFromVal = 0.5f;
        float LerpToVal = 1.5f;
        Vector3 LerpFrom = new Vector3(LerpFromVal, LerpFromVal, LerpFromVal);
        Vector3 LerpTo = new  Vector3(LerpToVal, LerpToVal, LerpToVal);

        float LerpTime = 0.3f;

//		Debug.Log ("Wave Count " + waveCount);
	
		if (destroyedEnemyCount == 6)
		{
			if (waveCount == 1)
			{
				StartCoroutine(ClearWave(waveCount));
			}
			waveCount = 2;
			canSpawn = true;
		}

		if (destroyedEnemyCount == 16)
		{
			if (waveCount == 2)
			{
				StartCoroutine(ClearWave(waveCount));
            }
			
			waveCount = 3;
			canSpawn = true;
		}

		if (destroyedEnemyCount == 30)
		{
			if (waveCount == 3)
			{
				StartCoroutine(ClearWave(waveCount));
            }

			waveCount = 4;
			canSpawn = true;
		}

		if (destroyedEnemyCount == 48)
		{
			if (waveCount == 4)
			{
				StartCoroutine(ClearWave(waveCount));
            }
			waveCount = 5;
			canSpawn = true;
		}

		if (destroyedEnemyCount == 70)
		{
			if (waveCount == 5)
			{
				StartCoroutine(ClearWave(waveCount));
            }

			waveCount = 6;
			canSpawn = true;
		}

		if (destroyedEnemyCount == 96)
		{
			if (waveCount == 6)
			{
				StartCoroutine(ClearWave(waveCount));
            }

			waveCount = 7;
			canSpawn = true;
		}

		if (destroyedEnemyCount == 126)
		{
			if (waveCount == 7)
			{
				StartCoroutine(ClearWave(waveCount));
            }

			waveCount = 8;
			canSpawn = true;
		}

		if (destroyedEnemyCount == 160)
		{
			if (waveCount == 8)
			{
				StartCoroutine(ClearWave(waveCount));
            }

			waveCount = 9;
			canSpawn = true;
		}

		if (destroyedEnemyCount == 198)
		{
			if (waveCount == 9)
			{
				StartCoroutine(ClearWave(waveCount));
            }

			waveCount = 10;
			canSpawn = true;
		}

		if (destroyedEnemyCount == 240)
		{
			canSpawn = false;
			if (waveCount == 10)
			{
				StartCoroutine(ClearWave(waveCount));
			}
			waveCount = 11;
        }
        
        switch (m_State)
        {

            case GameStateId.Initialize:
                {
                    if (UIControls.WaitingForTouch)
                        break;
                    UIControls.SetReadyState(ReadyStates.Ready3);

                    FillUpTimer = 0;
                    AmmoTimer = 1;
                    
                    NextBarrelSpawnTimer = Random.Range(BarrelSpawnMinTime, BarrelSpawnMaxTime);
                    Wave = 0;
                    WaveTimerForNext = WaveTimers[0];
                    RandomCubeTimer = Random.Range(8, 20);
                    Points = 0;
                    m_State = GameStateId.PrepareToStart3;
                    
                    LocalPlayerCharacter = UIGlobalVariablesScript.Singleton.MainCharacterRef;
                    PlayersCharacters.Add(UIGlobalVariablesScript.Singleton.MainCharacterRef);

                    for (int i = 0; i < LocalPlayerCharacter.transform.childCount; ++i)
                    {
                        GameObject childGun = LocalPlayerCharacter.transform.GetChild(i).gameObject;
                        if (childGun.name == "gun")
                        {
                            childGun.SetActive(true);
                        }
                    }
                    break;
                }
            case GameStateId.PrepareToStart3:
                {
                    UIControls.m_321.gameObject.SetActive(true);
                    if (FillUpTimer >= 0.25f)
                    {
                        UIControls.SetReadyState(ReadyStates.Ready2);
						UIControls.m_321.gameObject.transform.localScale = LerpFrom;
                        m_State = GameStateId.PrepareToStart2;
                    }
					UIControls.m_321.gameObject.transform.localScale = Vector3.Lerp(UIControls.m_321.gameObject.transform.localScale, LerpTo, LerpTime);

                    FillUpTimer += Time.deltaTime * 0.40f;
                    UIControls.SetBarWidth(FillUpTimer);

                    break;
                }
            case GameStateId.PrepareToStart2:
                {
                    if (FillUpTimer >= 0.5f)
                    {
                        UIControls.SetReadyState(ReadyStates.Ready1);
						UIControls.m_321.gameObject.transform.localScale = LerpFrom;
                        m_State = GameStateId.PrepareToStart1;
                    }
					UIControls.m_321.gameObject.transform.localScale = Vector3.Lerp(UIControls.m_321.gameObject.transform.localScale, LerpTo, LerpTime);

                    FillUpTimer += Time.deltaTime * 0.40f;
                    UIControls.SetBarWidth(FillUpTimer);

                    break;
                }
            case GameStateId.PrepareToStart1:
                {
                    if (FillUpTimer >= 0.75f)
                    {
                        UIControls.SetReadyState(ReadyStates.Go);
						UIControls.m_321.gameObject.transform.localScale = LerpFrom;
                        m_State = GameStateId.PrepareToStartGO;
                    }
					UIControls.m_321.gameObject.transform.localScale = Vector3.Lerp(UIControls.m_321.gameObject.transform.localScale, LerpTo, LerpTime);

                    FillUpTimer += Time.deltaTime * 0.40f;
                    UIControls.SetBarWidth(FillUpTimer);

                    break;
                }
            case GameStateId.PrepareToStartGO:
                {
                    if (FillUpTimer >= 1f)
                    {
                        UIControls.SetReadyState(ReadyStates.Ready3);
						UIControls.m_321.gameObject.SetActive(false);
                        m_State = GameStateId.Countdown;
                    }
					UIControls.m_321.gameObject.transform.localScale = Vector3.Lerp(UIControls.m_321.gameObject.transform.localScale, LerpTo, LerpTime);

                    FillUpTimer += Time.deltaTime * 0.40f;
                    UIControls.SetBarWidth(FillUpTimer);

                    break;
                }

            case GameStateId.PrepareToStart:
                {
                    m_State = GameStateId.Countdown;
                    break;
                }

            case GameStateId.Countdown:
				{
					m_Go321Done = true;
                    m_State = GameStateId.Playing;

                    UIGlobalVariablesScript.Singleton.SoundEngine.PlayLoop(GenericSoundId.GunLoop);
                    //GameObject.Find("GunfireLoop").GetComponent<AudioSource>().Play();

                    break;
                }

            case GameStateId.Playing:
                {
                    //if(Input.GetButtonDown("Fire1"))
                    //	ShootBulletForward();

			if(UIControls.WaitingForTouch&&!Paused){
				Paused = true;
				Debug.Log ("Turn on Pause");
			}
			if (Paused){
				Debug.Log ("Paused");
				break;
			}
				if (spawnBarrels == true)
				{
                    NextBarrelSpawnTimer -= Time.deltaTime;
                    if (NextBarrelSpawnTimer <= 1)
                    {
                        NextBarrelSpawnTimer = Random.Range(BarrelSpawnMinTime, BarrelSpawnMaxTime);
                        if (Random.Range(0, 10) == 0)
                            SpawnBarrelStart(true);
                        else
                            SpawnBarrelStart(false);
                    }


                    RandomCubeTimer -= Time.deltaTime;
                    if (RandomCubeTimer <= 0)
                    {
                        SpawnRandomCube();
                        RandomCubeTimer = Random.Range(8, 20);
                    }

                    AmmoTimer -= Time.deltaTime * 0.03f;
                    if (AmmoTimer <= 0.001f)
                    {
                        AmmoTimer = 0.001f;
                        m_State = GameStateId.Completed;
                    }
				}
                    //ReceiveEventAmmoTimer(AmmoTimer);

                    WaveTimerForNext -= Time.deltaTime;
                    if (WaveTimerForNext <= 1 && canSpawn == true)
                    {
                       // Wave++;
                        if (Wave == WaveTimers.Length)
                        {
                           // m_State = GameStateId.Completed;
                        }
                        else
                        {
                            //WaveTimerForNext = WaveTimers[Wave];
							WaveTimerForNext = 10;
                          //int enemyCount = Random.Range(WaveMinEnemies[Wave], WaveMaxEnemies[Wave]);
							if (waveCount == 1)
							{
								enemyCount = 3;
								StartCoroutine("WaveOneEnemies");
	
                            //	for (int a = 0; a < enemyCount; ++a)
                            //	{
								//	SpawnWaveOne(0);
                            //	}
							}
							
							else if (waveCount == 2 && destroyedEnemyCount == 6)
							{
								enemyCount = 10;
								StartCoroutine("WaveTwoEnemies");	
							//	for (int a = 0; a < enemyCount; ++a)
							//	{
							//		SpawnWaveTwo(0);
							//	}
							}

							else if (waveCount == 3 && destroyedEnemyCount == 16)
							{
								enemyCount = 14;
								StartCoroutine("WaveThreeEnemies");
							//	for (int a = 0; a < enemyCount; ++a)
							//	{
							//		SpawnWaveThree(0);
							//	}
							}
					
							else if (waveCount == 4 && destroyedEnemyCount == 30)
							{
								enemyCount = 18;
								StartCoroutine("WaveFourEnemies");
							//	for (int a = 0; a < enemyCount; ++a)
							//	{
							//		SpawnWaveFour(0);
							//	}
							}

							else if (waveCount == 5 && destroyedEnemyCount == 48)
							{
								enemyCount = 22;
								StartCoroutine("WaveFiveEnemies");
							//	for (int a = 0; a < enemyCount; ++a)
							//	{
							//		SpawnWaveFive(0);
							//	}
							}

							else if (waveCount == 6 && destroyedEnemyCount == 70)
							{
								enemyCount = 26;
								StartCoroutine("WaveSixEnemies");
							//	for (int a = 0; a < enemyCount; ++a)
							//	{
							//		SpawnWaveSix(0);
							//	}
							}

							else if (waveCount == 7 && destroyedEnemyCount == 96)
							{
								enemyCount = 30;
								StartCoroutine("WaveSevenEnemies");
							//	for (int a = 0; a < enemyCount; ++a)
							//	{
							//		SpawnWaveSeven(0);
							//	}
							}

							else if (waveCount == 8 && destroyedEnemyCount == 126)
							{
								enemyCount = 34;
								StartCoroutine("WaveEightEnemies");
							//	for (int a = 0; a < enemyCount; ++a)
							//	{
							//		SpawnWaveEight(0);
							//	}
							}

							else if (waveCount == 9 && destroyedEnemyCount == 160)
							{
								enemyCount = 38;
								StartCoroutine("WaveNineEnemies");
							//	for (int a = 0; a < enemyCount; ++a)
							//	{
							//		SpawnWaveNine(0);
							//	}
							}

							else if (waveCount == 10 && destroyedEnemyCount == 198)
							{
								enemyCount = 42;
								StartCoroutine("WaveTenEnemies");
							//	for (int a = 0; a < enemyCount; ++a)
							//	{
							//		SpawnWaveTen(0);
							//	}
							}
                        }
                    }

                    AutoShootCooldown -= Time.deltaTime;
                    if (AutoShootCooldown <= 0)
                    {
					if (canShoot == true)
					{
                        	AutoShootCooldown = 0.18f;
                        	//UIGlobalVariablesScript.Singleton.SoundEngine.Play(GenericSoundId.Jump);

                        	for (int a = 0; a < PlayersCharacters.Count; ++a)
                        	{
                            	ShootBulletForwardStart(a);
                        	}
                        
                    	}
					}
                    UIControls.SetBarWidth(AmmoTimer);
                    //MeterBar.renderer.set = (int)((AmmoTimer) * MeterBarBackground.width);

                    break;
                }

            case GameStateId.Paused:
                {
			
			if(!UIControls.Paused && !UIControls.WaitingForTouch&&Paused){
				Paused = false;
				Debug.Log ("Turn off Pause");
			}

                    break;
                }

            case GameStateId.Completed:
                {
                    m_State = GameStateId.PrepareToExit;
                    UIGlobalVariablesScript.Singleton.SoundEngine.StopLoop();
                    //GameObject.Find("GunfireLoop").GetComponent<AudioSource>().Stop();

                    break;
                }

            case GameStateId.PrepareToExit:
                {
                    ExitMinigame();

                    /*
                    UIGlobalVariablesScript.Singleton.GunGameInterface.SetActive(false);
                    UIGlobalVariablesScript.Singleton.JoystickArt.SetActive(false);
                    UIGlobalVariablesScript.Singleton.LoadingScreenRef.SetActive(true);
                    */
                    break;
                }
        }

        UIControls.Points.text = Points.ToString() + " pts";

        if (m_InvulnTimer >= 0)
            m_InvulnTimer -= Time.deltaTime;
        
    }

    public void ExitMinigame()
    {		
		UIGlobalVariablesScript.Singleton.SoundEngine.StopLoop();
		Paused = true;
		JoystickPageControls.Paused = true;
		UiPages.GetPage (Pages.GunMinigamePage).GetComponent<ShowHide> ().Show (false);
		//if (!skipScores) {
		ScoringPage.Show (Minigame.Gungame, Points, 0, LeaveMinigame);
		//} else {
		//	LeaveMinigame ();
		//}
	}

	public void LeaveMinigame()
	{
        BetweenSceneData.Instance.ReturnFromMiniGame = true;
        BetweenSceneData.Instance.Points = Points;
        MainARHandler.Instance.ChangeSceneToCaring();
    }
    protected void ReceiveEventAcquireControlOfCharacter(int playerIndex)
    {
        UIGlobalVariablesScript.Singleton.Joystick.CharacterAnimationRef = PlayersCharacters[playerIndex].GetComponent<MinigameAnimationControllerScript>();
        UIGlobalVariablesScript.Singleton.Joystick.CharacterControllerRef = PlayersCharacters[playerIndex].GetComponent<CharacterControllerScript>();
    }

    protected void ReceiveBeginGame()
    {
        m_State = GameStateId.PrepareToStart3;
    }
	
    public void Reset()
    {
        m_State = GameStateId.Initialize;
    }

    public void CloseGame()
    {
        UIGlobalVariablesScript.Singleton.SoundEngine.StopLoop();

        for (int i = 0; i < LocalPlayerCharacter.transform.childCount; ++i)
        {
            GameObject childGun = LocalPlayerCharacter.transform.GetChild(i).gameObject;
            if (childGun.name == "gun")
            {
                childGun.SetActive(false);
            }
        }
        this.gameObject.SetActive(false);

        for (int i = 0; i < SpawnedObjectsAllOthers.transform.childCount; ++i)
            Destroy(SpawnedObjectsAllOthers.transform.GetChild(i).gameObject);

        PlayersCharacters.Clear();
        LocalPlayerCharacter = null;
    }

    public void OnHitByEnemy(GameObject enemy, GameObject character)
    {
        if (m_InvulnTimer > 0)
            return;
        m_InvulnTimer = 1.0f;
        AmmoTimer -= 0.20f;

        character.GetComponent<CharacterControllerScript>().Forces.Add(
            new CharacterForces() { Speed = 800, Direction = -character.transform.forward, Length = 0.6f }
        );


        UIGlobalVariablesScript.Singleton.SoundEngine.Play(GenericSoundId.GunGame_Bump_Into_Baddy);
    }
	
    public void OnBulletHitBarrel(BarrelCollisionScript barrel)
    {
        UIControls.Bar.sprite = barrel.BarFrontTexture;
        UIControls.Icon.sprite = barrel.BuletIcon;

        //Destroy(barrel);
        AmmoTimer += 0.15f;
        if (AmmoTimer >= 1)
            AmmoTimer = 1;
        string[] prefabs = barrel.BulletPrefabs;
        CurrentBullets.Clear();
        CurrentBullets.AddRange(prefabs);

        Points += 150;

        UIGlobalVariablesScript.Singleton.SoundEngine.Play(GenericSoundId.GunGame_barrel_destroy);

        if (barrel.DestroyedPrefab != null)
        {
            GameObject newProjectile = Instantiate(barrel.DestroyedPrefab) as GameObject;
            newProjectile.transform.parent = SpawnedObjectsAllOthers.transform;
            newProjectile.transform.position = Vector3.zero;
            newProjectile.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
            newProjectile.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
            newProjectile.transform.localPosition = barrel.transform.localPosition;
        
        }
        Destroy(barrel.gameObject);
    }

    public GameObject SpawnAniminStart(PersistentData.TypesOfAnimin animinid, AniminEvolutionStageId evolution)
    {
        GameObject instance = null;//GameObject.Instantiate(resource1) as GameObject;

        Object resource1 = Resources.Load("Prefabs/tbo_baby_multi");
        instance = GameObject.Instantiate(resource1) as GameObject;
        

        LocalPlayerCharacter = instance;

        return instance;
    }

    public void SpawnAniminEnd(GameObject instance)
    {
        Vector3 scale = instance.transform.localScale;

        instance.transform.parent = SpawnedObjectsAllOthers.transform;
        instance.transform.localPosition = new Vector3(0, 0.1f, 0);
        instance.transform.localScale = scale;
        instance.transform.localRotation = Quaternion.identity;

        for (int i = 0; i < instance.transform.childCount; ++i)
        {
            GameObject childGun = instance.transform.GetChild(i).gameObject;
            if (childGun.name == "gun")
            {
                childGun.SetActive(true);
            }
        }

        PlayersCharacters.Add(instance);
    }

   /* public GameObject SpawnEnemyStart(int level)
    {
        if (level >= EnemyPrefabs.Length)
            level = EnemyPrefabs.Length - 1;

        GameObject resourceLoad = Resources.Load(EnemyPrefabs[level]) as GameObject;

        Texture2D[] textures = SlimeTextures;

        if (level == 1)
		
        textures = SlimeLevel2Textures;
      //	int textureIndex = Random.Range(0, textures.Length);
		int textureIndex = 3;

        Vector3 position = new Vector3(Random.Range(-1.5f, 1.5f), 0, Random.Range(-1.5f, 1.5f));

        GameObject newEnemy = null;

        newEnemy = Instantiate(resourceLoad) as GameObject;
        SpawnEnemyEnd(newEnemy, level, textureIndex, position);

		return newEnemy;
    }

    public void SpawnEnemyEnd(GameObject newEnemy, int level, int textureIndex, Vector3 position)
    {
        Texture2D[] textures = SlimeTextures;
        if (level == 1)
            textures = SlimeLevel2Textures;

        float scale = 1;
        if (level == 1)
            scale = 3;

        newEnemy.transform.parent = SpawnedObjectsAllOthers.transform;
        newEnemy.transform.position = new Vector3(0, 0, 0);
        newEnemy.transform.rotation = Quaternion.identity;
        newEnemy.transform.localScale = new Vector3(0.06f * scale, 0.06f * scale, 0.06f * scale);
        newEnemy.transform.localPosition = position;

        //if(level == 1)
        //	newProjectile.transform.rotation = Quaternion.Euler(15, 0, 0);

       
        Texture2D texture = textures[textureIndex];

        for (int i = 0; i < newEnemy.transform.childCount; ++i)
            if (newEnemy.transform.GetChild(i).renderer != null)
                newEnemy.transform.GetChild(i).renderer.material.mainTexture = texture;

        newEnemy.GetComponent<GunGameEnemyScript>().BulletSplat = BulletSplats[textureIndex];
        newEnemy.GetComponent<GunGameEnemyScript>().SkinColor = SlimeColors[textureIndex];
        newEnemy.GetComponent<GunGameEnemyScript>().Speed = Random.Range(0.05f, 0.11f) * (level + 1);
        newEnemy.GetComponent<GunGameEnemyScript>().SplatSetByCode = MonsterSplatPrefabs[textureIndex];
        newEnemy.GetComponent<GunGameEnemyScript>().Level = level;
        newEnemy.GetComponent<GunGameEnemyScript>().TargetToFollow = PlayersCharacters[Random.Range(0, PlayersCharacters.Count)];
        newEnemy.name = texture.name;
        //SpawnedObjects.Add(newEnemy);
    }
*/
	public GameObject SpawnWaveOne(int level)
	{
		// Blue enemies
		if (level >= EnemyPrefabs.Length)
			level = EnemyPrefabs.Length - 1;
		
		GameObject resourceLoad = Resources.Load(EnemyPrefabs[level]) as GameObject;
		
//		Texture2D[] textures = SlimeTextures;
//		if (level == 1)
//			textures = SlimeLevel2Textures;
		//	int textureIndex = Random.Range(0, textures.Length);
		// Blue
		int textureIndex = 0;
		
		Vector3 position = new Vector3(Random.Range(-1.5f, 1.5f), 0, Random.Range(-1.5f, 1.5f));
		
		GameObject newEnemy = null;
		
		newEnemy = Instantiate(resourceLoad) as GameObject;
		SpawnWaveOneEnd(newEnemy, level, textureIndex, position);

		canSpawn = false;

		return newEnemy;
	}

	public void SpawnWaveOneEnd(GameObject newEnemy, int level, int textureIndex, Vector3 position)
	{
		Texture2D[] textures = SlimeTextures;
		if (level == 1)
			textures = SlimeLevel2Textures;
		
		float scale = 1;
		if (level == 1)
			scale = 3;
		
		newEnemy.transform.parent = SpawnedObjectsAllOthers.transform;
		newEnemy.transform.position = new Vector3(0, 0, 0);
		newEnemy.transform.rotation = Quaternion.identity;
		newEnemy.transform.localScale = new Vector3(0.06f * scale, 0.06f * scale, 0.06f * scale);
		newEnemy.transform.localPosition = position;
		
		//if(level == 1)
		//	newProjectile.transform.rotation = Quaternion.Euler(15, 0, 0);
		
		
		Texture2D texture = textures[textureIndex];
		
		for (int i = 0; i < newEnemy.transform.childCount; ++i)
			if (newEnemy.transform.GetChild(i).GetComponent<Renderer>() != null)
				newEnemy.transform.GetChild(i).GetComponent<Renderer>().material.mainTexture = texture;
		
		newEnemy.GetComponent<GunGameEnemyScript>().BulletSplat = BulletSplats[textureIndex];
		newEnemy.GetComponent<GunGameEnemyScript>().SkinColor = SlimeColors[textureIndex];
		newEnemy.GetComponent<GunGameEnemyScript>().Speed = Random.Range(0.05f, 0.11f) * (level + 1);
		newEnemy.GetComponent<GunGameEnemyScript>().SplatSetByCode = MonsterSplatPrefabs[textureIndex];
		newEnemy.GetComponent<GunGameEnemyScript>().Level = level;
		newEnemy.GetComponent<GunGameEnemyScript>().TargetToFollow = PlayersCharacters[Random.Range(0, PlayersCharacters.Count)];
		newEnemy.name = texture.name;
		//SpawnedObjects.Add(newEnemy);
	}

	public GameObject SpawnWaveTwo(int level)
	{
		if (level >= EnemyPrefabs.Length)
			level = EnemyPrefabs.Length - 1;
		
		GameObject resourceLoad = Resources.Load(EnemyPrefabs[level]) as GameObject;
		
//		Texture2D[] textures = SlimeTextures;		
//		if (level == 1)			
//			textures = SlimeLevel2Textures;
		// Blue & red
			int textureIndex = Random.Range(0, 2);

		Vector3 position = new Vector3(Random.Range(-1.5f, 1.5f), 0, Random.Range(-1.5f, 1.5f));
		
		GameObject newEnemy = null;
		
		newEnemy = Instantiate(resourceLoad) as GameObject;
		SpawnWaveTwoEnd(newEnemy, level, textureIndex, position);
		
		return newEnemy;
	}
	
	public void SpawnWaveTwoEnd(GameObject newEnemy, int level, int textureIndex, Vector3 position)
	{
		Texture2D[] textures = SlimeTextures;
		if (level == 1)
			textures = SlimeLevel2Textures;
		
		float scale = 1;
		if (level == 1)
			scale = 3;
		
		newEnemy.transform.parent = SpawnedObjectsAllOthers.transform;
		newEnemy.transform.position = new Vector3(0, 0, 0);
		newEnemy.transform.rotation = Quaternion.identity;
		newEnemy.transform.localScale = new Vector3(0.06f * scale, 0.06f * scale, 0.06f * scale);
		newEnemy.transform.localPosition = position;
		
		//if(level == 1)
		//	newProjectile.transform.rotation = Quaternion.Euler(15, 0, 0);
		
		
		Texture2D texture = textures[textureIndex];
		
		for (int i = 0; i < newEnemy.transform.childCount; ++i)
			if (newEnemy.transform.GetChild(i).GetComponent<Renderer>() != null)
				newEnemy.transform.GetChild(i).GetComponent<Renderer>().material.mainTexture = texture;
		
		newEnemy.GetComponent<GunGameEnemyScript>().BulletSplat = BulletSplats[textureIndex];
		newEnemy.GetComponent<GunGameEnemyScript>().SkinColor = SlimeColors[textureIndex];
		newEnemy.GetComponent<GunGameEnemyScript>().Speed = Random.Range(0.05f, 0.11f) * (level + 1);
		newEnemy.GetComponent<GunGameEnemyScript>().SplatSetByCode = MonsterSplatPrefabs[textureIndex];
		newEnemy.GetComponent<GunGameEnemyScript>().Level = level;
		newEnemy.GetComponent<GunGameEnemyScript>().TargetToFollow = PlayersCharacters[Random.Range(0, PlayersCharacters.Count)];
		newEnemy.name = texture.name;
		//SpawnedObjects.Add(newEnemy);
	}

	public GameObject SpawnWaveThree(int level)
	{
		if (level >= EnemyPrefabs.Length)
			level = EnemyPrefabs.Length - 1;
		
		GameObject resourceLoad = Resources.Load(EnemyPrefabs[level]) as GameObject;
		
//		Texture2D[] textures = SlimeTextures;

//		if (level == 1)
			
//			textures = SlimeLevel2Textures;
		// Blue & red & yellow
		int textureIndex = Random.Range(0, 3);
		
		Vector3 position = new Vector3(Random.Range(-1.5f, 1.5f), 0, Random.Range(-1.5f, 1.5f));
		
		GameObject newEnemy = null;
		
		newEnemy = Instantiate(resourceLoad) as GameObject;
		SpawnWaveThreeEnd(newEnemy, level, textureIndex, position);
		
		return newEnemy;
	}

	public void SpawnWaveThreeEnd(GameObject newEnemy, int level, int textureIndex, Vector3 position)
	{
		Texture2D[] textures = SlimeTextures;
		if (level == 1)
			textures = SlimeLevel2Textures;
		
		float scale = 1;
		if (level == 1)
			scale = 3;
		
		newEnemy.transform.parent = SpawnedObjectsAllOthers.transform;
		newEnemy.transform.position = new Vector3(0, 0, 0);
		newEnemy.transform.rotation = Quaternion.identity;
		newEnemy.transform.localScale = new Vector3(0.06f * scale, 0.06f * scale, 0.06f * scale);
		newEnemy.transform.localPosition = position;
		
		//if(level == 1)
		//	newProjectile.transform.rotation = Quaternion.Euler(15, 0, 0);
		
		
		Texture2D texture = textures[textureIndex];
		
		for (int i = 0; i < newEnemy.transform.childCount; ++i)
			if (newEnemy.transform.GetChild(i).GetComponent<Renderer>() != null)
				newEnemy.transform.GetChild(i).GetComponent<Renderer>().material.mainTexture = texture;
		
		newEnemy.GetComponent<GunGameEnemyScript>().BulletSplat = BulletSplats[textureIndex];
		newEnemy.GetComponent<GunGameEnemyScript>().SkinColor = SlimeColors[textureIndex];
		newEnemy.GetComponent<GunGameEnemyScript>().Speed = Random.Range(0.05f, 0.11f) * (level + 1);
		newEnemy.GetComponent<GunGameEnemyScript>().SplatSetByCode = MonsterSplatPrefabs[textureIndex];
		newEnemy.GetComponent<GunGameEnemyScript>().Level = level;
		newEnemy.GetComponent<GunGameEnemyScript>().TargetToFollow = PlayersCharacters[Random.Range(0, PlayersCharacters.Count)];
		newEnemy.name = texture.name;
		//SpawnedObjects.Add(newEnemy);
	}

	public GameObject SpawnWaveFour(int level)
	{
		if (level >= EnemyPrefabs.Length)
			level = EnemyPrefabs.Length - 1;
		
		GameObject resourceLoad = Resources.Load(EnemyPrefabs[level]) as GameObject;
		
//		Texture2D[] textures = SlimeTextures;
		
//		if (level == 1)
			
//			textures = SlimeLevel2Textures;
		// Blue & red & yellow & green
		int textureIndex = Random.Range(0, 4);
		
		Vector3 position = new Vector3(Random.Range(-1.5f, 1.5f), 0, Random.Range(-1.5f, 1.5f));
		
		GameObject newEnemy = null;
		
		newEnemy = Instantiate(resourceLoad) as GameObject;
		SpawnWaveFourEnd(newEnemy, level, textureIndex, position);
		
		return newEnemy;
	}

	public void SpawnWaveFourEnd(GameObject newEnemy, int level, int textureIndex, Vector3 position)
	{
		Texture2D[] textures = SlimeTextures;
		if (level == 1)
			textures = SlimeLevel2Textures;
		
		float scale = 1;
		if (level == 1)
			scale = 3;
		
		newEnemy.transform.parent = SpawnedObjectsAllOthers.transform;
		newEnemy.transform.position = new Vector3(0, 0, 0);
		newEnemy.transform.rotation = Quaternion.identity;
		newEnemy.transform.localScale = new Vector3(0.06f * scale, 0.06f * scale, 0.06f * scale);
		newEnemy.transform.localPosition = position;
		
		//if(level == 1)
		//	newProjectile.transform.rotation = Quaternion.Euler(15, 0, 0);
		
		
		Texture2D texture = textures[textureIndex];
		
		for (int i = 0; i < newEnemy.transform.childCount; ++i)
			if (newEnemy.transform.GetChild(i).GetComponent<Renderer>() != null)
				newEnemy.transform.GetChild(i).GetComponent<Renderer>().material.mainTexture = texture;
		
		newEnemy.GetComponent<GunGameEnemyScript>().BulletSplat = BulletSplats[textureIndex];
		newEnemy.GetComponent<GunGameEnemyScript>().SkinColor = SlimeColors[textureIndex];
		newEnemy.GetComponent<GunGameEnemyScript>().Speed = Random.Range(0.05f, 0.11f) * (level + 1);
		newEnemy.GetComponent<GunGameEnemyScript>().SplatSetByCode = MonsterSplatPrefabs[textureIndex];
		newEnemy.GetComponent<GunGameEnemyScript>().Level = level;
		newEnemy.GetComponent<GunGameEnemyScript>().TargetToFollow = PlayersCharacters[Random.Range(0, PlayersCharacters.Count)];
		newEnemy.name = texture.name;
		//SpawnedObjects.Add(newEnemy);
	}

	public GameObject SpawnWaveFive(int level)
	{
		if (level >= EnemyPrefabs.Length)
			level = EnemyPrefabs.Length - 1;
		
		GameObject resourceLoad = Resources.Load(EnemyPrefabs[level]) as GameObject;
		
//		Texture2D[] textures = SlimeTextures;
		
//		if (level == 1)
			
//			textures = SlimeLevel2Textures;
		// Blue & red & yellow & green
		int textureIndex = Random.Range(0, 4);
		
		Vector3 position = new Vector3(Random.Range(-1.5f, 1.5f), 0, Random.Range(-1.5f, 1.5f));
		
		GameObject newEnemy = null;
		
		newEnemy = Instantiate(resourceLoad) as GameObject;
		SpawnWaveFiveEnd(newEnemy, level, textureIndex, position);
		
		return newEnemy;
	}

	public void SpawnWaveFiveEnd(GameObject newEnemy, int level, int textureIndex, Vector3 position)
	{
		Texture2D[] textures = SlimeTextures;
		if (level == 1)
			textures = SlimeLevel2Textures;
		
		float scale = 1;
		if (level == 1)
			scale = 3;
		
		newEnemy.transform.parent = SpawnedObjectsAllOthers.transform;
		newEnemy.transform.position = new Vector3(0, 0, 0);
		newEnemy.transform.rotation = Quaternion.identity;
		newEnemy.transform.localScale = new Vector3(0.06f * scale, 0.06f * scale, 0.06f * scale);
		newEnemy.transform.localPosition = position;
		
		//if(level == 1)
		//	newProjectile.transform.rotation = Quaternion.Euler(15, 0, 0);
		
		
		Texture2D texture = textures[textureIndex];
		
		for (int i = 0; i < newEnemy.transform.childCount; ++i)
			if (newEnemy.transform.GetChild(i).GetComponent<Renderer>() != null)
				newEnemy.transform.GetChild(i).GetComponent<Renderer>().material.mainTexture = texture;
		
		newEnemy.GetComponent<GunGameEnemyScript>().BulletSplat = BulletSplats[textureIndex];
		newEnemy.GetComponent<GunGameEnemyScript>().SkinColor = SlimeColors[textureIndex];
		newEnemy.GetComponent<GunGameEnemyScript>().Speed = Random.Range(0.05f, 0.11f) * (level + 1);
		newEnemy.GetComponent<GunGameEnemyScript>().SplatSetByCode = MonsterSplatPrefabs[textureIndex];
		newEnemy.GetComponent<GunGameEnemyScript>().Level = level;
		newEnemy.GetComponent<GunGameEnemyScript>().TargetToFollow = PlayersCharacters[Random.Range(0, PlayersCharacters.Count)];
		newEnemy.name = texture.name;
		//SpawnedObjects.Add(newEnemy);
	}

	public GameObject SpawnWaveSix(int level)
	{
		if (level >= EnemyPrefabs.Length)
			level = EnemyPrefabs.Length - 1;
		
		GameObject resourceLoad = Resources.Load(EnemyPrefabs[level]) as GameObject;
		
//		Texture2D[] textures = SlimeTextures;
		
//		if (level == 1)
			
//			textures = SlimeLevel2Textures;
		// Blue & red & yellow & green
		int textureIndex = Random.Range(0, 4);
		
		Vector3 position = new Vector3(Random.Range(-1.5f, 1.5f), 0, Random.Range(-1.5f, 1.5f));
		
		GameObject newEnemy = null;
		
		newEnemy = Instantiate(resourceLoad) as GameObject;
		SpawnWaveSixEnd(newEnemy, level, textureIndex, position);
		
		return newEnemy;
	}
	
	public void SpawnWaveSixEnd(GameObject newEnemy, int level, int textureIndex, Vector3 position)
	{
		Texture2D[] textures = SlimeTextures;
		if (level == 1)
			textures = SlimeLevel2Textures;
		
		float scale = 1;
		if (level == 1)
			scale = 3;
		
		newEnemy.transform.parent = SpawnedObjectsAllOthers.transform;
		newEnemy.transform.position = new Vector3(0, 0, 0);
		newEnemy.transform.rotation = Quaternion.identity;
		newEnemy.transform.localScale = new Vector3(0.06f * scale, 0.06f * scale, 0.06f * scale);
		newEnemy.transform.localPosition = position;
		
		//if(level == 1)
		//	newProjectile.transform.rotation = Quaternion.Euler(15, 0, 0);
		
		
		Texture2D texture = textures[textureIndex];
		
		for (int i = 0; i < newEnemy.transform.childCount; ++i)
			if (newEnemy.transform.GetChild(i).GetComponent<Renderer>() != null)
				newEnemy.transform.GetChild(i).GetComponent<Renderer>().material.mainTexture = texture;
		
		newEnemy.GetComponent<GunGameEnemyScript>().BulletSplat = BulletSplats[textureIndex];
		newEnemy.GetComponent<GunGameEnemyScript>().SkinColor = SlimeColors[textureIndex];
		newEnemy.GetComponent<GunGameEnemyScript>().Speed = Random.Range(0.05f, 0.11f) * (level + 1);
		newEnemy.GetComponent<GunGameEnemyScript>().SplatSetByCode = MonsterSplatPrefabs[textureIndex];
		newEnemy.GetComponent<GunGameEnemyScript>().Level = level;
		newEnemy.GetComponent<GunGameEnemyScript>().TargetToFollow = PlayersCharacters[Random.Range(0, PlayersCharacters.Count)];
		newEnemy.name = texture.name;
		//SpawnedObjects.Add(newEnemy);
	}

	public GameObject SpawnWaveSeven(int level)
	{
		if (level >= EnemyPrefabs.Length)
			level = EnemyPrefabs.Length - 1;
		
		GameObject resourceLoad = Resources.Load(EnemyPrefabs[level]) as GameObject;
		
//		Texture2D[] textures = SlimeTextures;
		
//		if (level == 1)
			
//			textures = SlimeLevel2Textures;
		// Blue & red & yellow & green
		int textureIndex = Random.Range(0, 4);
		
		Vector3 position = new Vector3(Random.Range(-1.5f, 1.5f), 0, Random.Range(-1.5f, 1.5f));
		
		GameObject newEnemy = null;
		
		newEnemy = Instantiate(resourceLoad) as GameObject;
		SpawnWaveSevenEnd(newEnemy, level, textureIndex, position);
		
		return newEnemy;
	}
	
	public void SpawnWaveSevenEnd(GameObject newEnemy, int level, int textureIndex, Vector3 position)
	{
		Texture2D[] textures = SlimeTextures;
		if (level == 1)
			textures = SlimeLevel2Textures;
		
		float scale = 1;
		if (level == 1)
			scale = 3;
		
		newEnemy.transform.parent = SpawnedObjectsAllOthers.transform;
		newEnemy.transform.position = new Vector3(0, 0, 0);
		newEnemy.transform.rotation = Quaternion.identity;
		newEnemy.transform.localScale = new Vector3(0.06f * scale, 0.06f * scale, 0.06f * scale);
		newEnemy.transform.localPosition = position;
		
		//if(level == 1)
		//	newProjectile.transform.rotation = Quaternion.Euler(15, 0, 0);
		
		
		Texture2D texture = textures[textureIndex];
		
		for (int i = 0; i < newEnemy.transform.childCount; ++i)
			if (newEnemy.transform.GetChild(i).GetComponent<Renderer>() != null)
				newEnemy.transform.GetChild(i).GetComponent<Renderer>().material.mainTexture = texture;
		
		newEnemy.GetComponent<GunGameEnemyScript>().BulletSplat = BulletSplats[textureIndex];
		newEnemy.GetComponent<GunGameEnemyScript>().SkinColor = SlimeColors[textureIndex];
		newEnemy.GetComponent<GunGameEnemyScript>().Speed = Random.Range(0.05f, 0.11f) * (level + 1);
		newEnemy.GetComponent<GunGameEnemyScript>().SplatSetByCode = MonsterSplatPrefabs[textureIndex];
		newEnemy.GetComponent<GunGameEnemyScript>().Level = level;
		newEnemy.GetComponent<GunGameEnemyScript>().TargetToFollow = PlayersCharacters[Random.Range(0, PlayersCharacters.Count)];
		newEnemy.name = texture.name;
		//SpawnedObjects.Add(newEnemy);
	}

	public GameObject SpawnWaveEight(int level)
	{
		if (level >= EnemyPrefabs.Length)
			level = EnemyPrefabs.Length - 1;
		
		GameObject resourceLoad = Resources.Load(EnemyPrefabs[level]) as GameObject;
		
//		Texture2D[] textures = SlimeTextures;
		
//		if (level == 1)
			
//			textures = SlimeLevel2Textures;
		// Blue & red & yellow & green
		int textureIndex = Random.Range(0, 4);
		
		Vector3 position = new Vector3(Random.Range(-1.5f, 1.5f), 0, Random.Range(-1.5f, 1.5f));
		
		GameObject newEnemy = null;
		
		newEnemy = Instantiate(resourceLoad) as GameObject;
		SpawnWaveEightEnd(newEnemy, level, textureIndex, position);
		
		return newEnemy;
	}
	
	public void SpawnWaveEightEnd(GameObject newEnemy, int level, int textureIndex, Vector3 position)
	{
		Texture2D[] textures = SlimeTextures;
		if (level == 1)
			textures = SlimeLevel2Textures;
		
		float scale = 1;
		if (level == 1)
			scale = 3;
		
		newEnemy.transform.parent = SpawnedObjectsAllOthers.transform;
		newEnemy.transform.position = new Vector3(0, 0, 0);
		newEnemy.transform.rotation = Quaternion.identity;
		newEnemy.transform.localScale = new Vector3(0.06f * scale, 0.06f * scale, 0.06f * scale);
		newEnemy.transform.localPosition = position;
		
		//if(level == 1)
		//	newProjectile.transform.rotation = Quaternion.Euler(15, 0, 0);
		
		
		Texture2D texture = textures[textureIndex];
		
		for (int i = 0; i < newEnemy.transform.childCount; ++i)
			if (newEnemy.transform.GetChild(i).GetComponent<Renderer>() != null)
				newEnemy.transform.GetChild(i).GetComponent<Renderer>().material.mainTexture = texture;
		
		newEnemy.GetComponent<GunGameEnemyScript>().BulletSplat = BulletSplats[textureIndex];
		newEnemy.GetComponent<GunGameEnemyScript>().SkinColor = SlimeColors[textureIndex];
		newEnemy.GetComponent<GunGameEnemyScript>().Speed = Random.Range(0.05f, 0.11f) * (level + 1);
		newEnemy.GetComponent<GunGameEnemyScript>().SplatSetByCode = MonsterSplatPrefabs[textureIndex];
		newEnemy.GetComponent<GunGameEnemyScript>().Level = level;
		newEnemy.GetComponent<GunGameEnemyScript>().TargetToFollow = PlayersCharacters[Random.Range(0, PlayersCharacters.Count)];
		newEnemy.name = texture.name;
		//SpawnedObjects.Add(newEnemy);
	}

	public GameObject SpawnWaveNine(int level)
	{
		if (level >= EnemyPrefabs.Length)
			level = EnemyPrefabs.Length - 1;
		
		GameObject resourceLoad = Resources.Load(EnemyPrefabs[level]) as GameObject;
		
//		Texture2D[] textures = SlimeTextures;
		
//		if (level == 1)
			
//			textures = SlimeLevel2Textures;
		// Blue & red & yellow & green
		int textureIndex = Random.Range(0, 4);
		
		Vector3 position = new Vector3(Random.Range(-1.5f, 1.5f), 0, Random.Range(-1.5f, 1.5f));
		
		GameObject newEnemy = null;
		
		newEnemy = Instantiate(resourceLoad) as GameObject;
		SpawnWaveNineEnd(newEnemy, level, textureIndex, position);
		
		return newEnemy;
	}
	
	public void SpawnWaveNineEnd(GameObject newEnemy, int level, int textureIndex, Vector3 position)
	{
		Texture2D[] textures = SlimeTextures;
		if (level == 1)
			textures = SlimeLevel2Textures;
		
		float scale = 1;
		if (level == 1)
			scale = 3;
		
		newEnemy.transform.parent = SpawnedObjectsAllOthers.transform;
		newEnemy.transform.position = new Vector3(0, 0, 0);
		newEnemy.transform.rotation = Quaternion.identity;
		newEnemy.transform.localScale = new Vector3(0.06f * scale, 0.06f * scale, 0.06f * scale);
		newEnemy.transform.localPosition = position;
		
		//if(level == 1)
		//	newProjectile.transform.rotation = Quaternion.Euler(15, 0, 0);
		
		
		Texture2D texture = textures[textureIndex];
		
		for (int i = 0; i < newEnemy.transform.childCount; ++i)
			if (newEnemy.transform.GetChild(i).GetComponent<Renderer>() != null)
				newEnemy.transform.GetChild(i).GetComponent<Renderer>().material.mainTexture = texture;
		
		newEnemy.GetComponent<GunGameEnemyScript>().BulletSplat = BulletSplats[textureIndex];
		newEnemy.GetComponent<GunGameEnemyScript>().SkinColor = SlimeColors[textureIndex];
		newEnemy.GetComponent<GunGameEnemyScript>().Speed = Random.Range(0.05f, 0.11f) * (level + 1);
		newEnemy.GetComponent<GunGameEnemyScript>().SplatSetByCode = MonsterSplatPrefabs[textureIndex];
		newEnemy.GetComponent<GunGameEnemyScript>().Level = level;
		newEnemy.GetComponent<GunGameEnemyScript>().TargetToFollow = PlayersCharacters[Random.Range(0, PlayersCharacters.Count)];
		newEnemy.name = texture.name;
		//SpawnedObjects.Add(newEnemy);
	}

	public GameObject SpawnWaveTen(int level)
	{
		if (level >= EnemyPrefabs.Length)
			level = EnemyPrefabs.Length - 1;
		
		GameObject resourceLoad = Resources.Load(EnemyPrefabs[level]) as GameObject;
		
//		Texture2D[] textures = SlimeTextures;
		
//		if (level == 1)
			
//			textures = SlimeLevel2Textures;
		// Blue & red & yellow & green
		int textureIndex = Random.Range(0, 4);
		
		Vector3 position = new Vector3(Random.Range(-1.5f, 1.5f), 0, Random.Range(-1.5f, 1.5f));
		
		GameObject newEnemy = null;
		
		newEnemy = Instantiate(resourceLoad) as GameObject;
		SpawnWaveTenEnd(newEnemy, level, textureIndex, position);
		
		return newEnemy;
	}
	
	public void SpawnWaveTenEnd(GameObject newEnemy, int level, int textureIndex, Vector3 position)
	{
		Texture2D[] textures = SlimeTextures;
		if (level == 1)
			textures = SlimeLevel2Textures;
		
		float scale = 1;
		if (level == 1)
			scale = 3;
		
		newEnemy.transform.parent = SpawnedObjectsAllOthers.transform;
		newEnemy.transform.position = new Vector3(0, 0, 0);
		newEnemy.transform.rotation = Quaternion.identity;
		newEnemy.transform.localScale = new Vector3(0.06f * scale, 0.06f * scale, 0.06f * scale);
		newEnemy.transform.localPosition = position;
		
		//if(level == 1)
		//	newProjectile.transform.rotation = Quaternion.Euler(15, 0, 0);
		
		
		Texture2D texture = textures[textureIndex];
		
		for (int i = 0; i < newEnemy.transform.childCount; ++i)
			if (newEnemy.transform.GetChild(i).GetComponent<Renderer>() != null)
				newEnemy.transform.GetChild(i).GetComponent<Renderer>().material.mainTexture = texture;
		
		newEnemy.GetComponent<GunGameEnemyScript>().BulletSplat = BulletSplats[textureIndex];
		newEnemy.GetComponent<GunGameEnemyScript>().SkinColor = SlimeColors[textureIndex];
		newEnemy.GetComponent<GunGameEnemyScript>().Speed = Random.Range(0.05f, 0.11f) * (level + 1);
		newEnemy.GetComponent<GunGameEnemyScript>().SplatSetByCode = MonsterSplatPrefabs[textureIndex];
		newEnemy.GetComponent<GunGameEnemyScript>().Level = level;
		newEnemy.GetComponent<GunGameEnemyScript>().TargetToFollow = PlayersCharacters[Random.Range(0, PlayersCharacters.Count)];
		newEnemy.name = texture.name;
		//SpawnedObjects.Add(newEnemy);
	}



	public void WaveCompleted()
	{
		// Next Wave
	}

    public void SpawnBarrelStart(bool special)
    {
        GameObject newProjectile = null;

        Vector3 randomPosition = new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f));
        if (special)
        {
            GameObject resourceLoad = Resources.Load(SpecialBarrels[Random.Range(0, SpecialBarrels.Length)]) as GameObject;
            newProjectile = Instantiate(resourceLoad) as GameObject;
        }
        else
        {
            //GameObject resourceLoad = Resources.Load(Barrels[Random.Range(0, Barrels.Length)]) as GameObject;
			if (waveCount == 1)
			{
				// Blue Barrels
				GameObject resourceLoad = Resources.Load(Barrels[0]) as GameObject;
				newProjectile = Instantiate(resourceLoad) as GameObject;
			}

			if (waveCount == 2)
			{
				// Blue & Red Barrels
				GameObject resourceLoad = Resources.Load(Barrels[Random.Range(0, 2)]) as GameObject;
				newProjectile = Instantiate(resourceLoad) as GameObject;
			}

			if (waveCount == 3)
			{
				// Blue & Red & Yellow Barrels
				GameObject resourceLoad = Resources.Load(Barrels[Random.Range(0, 3)]) as GameObject;
				newProjectile = Instantiate(resourceLoad) as GameObject;
			}

			if (waveCount >= 4)
			{
				// Blue & Red & Yellow & Green
				GameObject resourceLoad = Resources.Load(Barrels[Random.Range(0, 4)]) as GameObject;
				newProjectile = Instantiate(resourceLoad) as GameObject;
			}
           // newProjectile = Instantiate(resourceLoad) as GameObject;
        }

        // newProjectile.GetComponent<BarrelCollisionScript>().SetLocal(true);

        SpawnBarrelEnd(newProjectile, randomPosition);
    }

    public void SpawnBarrelEnd(GameObject newProjectile, Vector3 position)
    {
        newProjectile.transform.parent = SpawnedObjectsAllOthers.transform;
        newProjectile.transform.position = Vector3.zero;
        newProjectile.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
        newProjectile.transform.localScale = new Vector3(0.19f, 0.19f, 0.19f);
        newProjectile.transform.localPosition = position;

        // SpawnedObjects.Add(newProjectile);
    }

    public void SpawnRandomCube()
    {
        GameObject newProjectile = Instantiate(RandomCubePrefab) as GameObject;

        newProjectile.transform.parent = SpawnedObjectsAllOthers.transform;
        newProjectile.transform.position = Vector3.zero;
        newProjectile.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
        newProjectile.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
        newProjectile.transform.localPosition = new Vector3(Random.Range(-0.5f, 0.5f), 0, Random.Range(-0.5f, 0.5f));

        // SpawnedObjects.Add(newProjectile);
    }

    public void ShootBulletForwardStart(int playerIndex)
    {
//        bool isMasterClientShooting = false;

        GameObject newProjectile = null;
        GameObject resourceLoad = Resources.Load(CurrentBullets[Random.Range(0, CurrentBullets.Count)]) as GameObject;
        newProjectile = Instantiate(resourceLoad) as GameObject;

        newProjectile.GetComponent<ProjectileScript>().SetLocal(true);

        newProjectile.transform.parent = SpawnedObjectsAllOthers.transform;
        newProjectile.transform.position = UIGlobalVariablesScript.Singleton.MainCharacterRef.transform.position;
        newProjectile.transform.rotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
        newProjectile.transform.localScale = new Vector3(0.116f, 0.116f, 0.116f);
        newProjectile.transform.position = PlayersCharacters[playerIndex].transform.position + ((PlayersCharacters[playerIndex].transform.forward * 20f) + new Vector3(0, 10f, 0));
        newProjectile.GetComponent<Rigidbody>().AddForce(PlayersCharacters[playerIndex].transform.forward * 20000);
    }

			
    public void ShootBulletLost(float speedVariationFactor, GameObject character)
    {
        Debug.Log("When am I called? If you see this Debug, let Harry know, and tell him when this was called. Also tell him he's looking good today.");
        GameObject resourceLoad = Resources.Load(CurrentBullets[Random.Range(0, CurrentBullets.Count)]) as GameObject;

        GameObject newProjectile = Instantiate(resourceLoad) as GameObject;
        // Destroy(newProjectile.GetComponent<ProjectileScript>());
        newProjectile.transform.parent = SpawnedObjectsAllOthers.transform;
        newProjectile.transform.position = UIGlobalVariablesScript.Singleton.MainCharacterRef.transform.position;
        newProjectile.transform.rotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
        newProjectile.transform.localScale = new Vector3(0.116f, 0.116f, 0.116f) * Random.Range(0.80f, 1.0f);
        newProjectile.transform.position = character.transform.position + character.transform.forward * 1f + new Vector3(0, 1f, 0);
        //newProjectile.AddComponent<ProjectileScript>();
        //newProjectile.velocity = transform.TransformDirection( Vector3( 0, 0, speed) );
        //newProjectile.AddComponent<MeshCollider>();

        Vector3 direction = Vector3.zero;
        direction.x = Random.Range(-0.3f, 0.3f);
        direction.z = Random.Range(-0.3f, 0.3f);


        newProjectile.GetComponent<Rigidbody>().AddForce((character.transform.up + direction) * 20000 * speedVariationFactor);

        // SpawnedObjects.Add(newProjectile);
    }


    public void ShootEnemyDestroyedEffects(float speedVariationFactor, Vector3 position, Color color, string bulletSplat)
    {
        GameObject newProjectile = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        newProjectile.transform.parent = SpawnedObjectsAllOthers.transform;
        newProjectile.transform.position = UIGlobalVariablesScript.Singleton.MainCharacterRef.transform.position;
        newProjectile.transform.rotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
        newProjectile.transform.localScale = new Vector3(0.116f, 0.116f, 0.116f) * Random.Range(0.20f, 0.70f);
        newProjectile.transform.localPosition = position;

        Vector3 direction = Vector3.zero;
        direction.x = Random.Range(-0.3f, 0.3f);
        direction.z = Random.Range(-0.3f, 0.3f);

        //newProjectile.layer = LayerMask.NameToLayer("Default");
        //newProjectile.tag = "Untagged";

        newProjectile.layer = LayerMask.NameToLayer("Projectiles");
        newProjectile.AddComponent<Rigidbody>();
        newProjectile.GetComponent<Rigidbody>().velocity = (Vector3.up + direction) * 500 * speedVariationFactor;
        newProjectile.AddComponent<MonsterSplatExplosionScript>();
        newProjectile.GetComponent<MonsterSplatExplosionScript>().SplatPrefab = bulletSplat;

        //newProjectile.GetComponent<Rigidbody>().AddForce(Vector3.down * 20000);
        newProjectile.GetComponent<Renderer>().material.color = color;
        //newProjectile.GetComponent<Rigidbody>().AddForce((Vector3.up + direction) * 5000 * speedVariationFactor);
        //newProjectile.GetComponent<Rigidbody>().AddExplosionForce(10, position + new Vector3(0,1,0), 1);


        //SpawnedObjects.Add(newProjectile);
    }
}
