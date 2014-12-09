using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

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
    private GameStateId State;
    private float AmmoTimer;
    private float NextBarrelSpawnTimer;
    // public List<GameObject> SpawnedObjects = new List<GameObject>();
    public Texture2D[] SlimeTextures;
    public Texture2D[] SlimeLevel2Textures;
    public Color[] SlimeColors;
    public GameObject[] MonsterSplatPrefabs;
    //public Texture2D[] BarTextures;
    public Texture FrontBar;

    private const float BarrelSpawnMinTime = 2;
    private const float BarrelSpawnMaxTime = 5;
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


    public enum TutorialStateId
    {
        None = 0,
        ShowEnemies,
        ShowMove,
        Completed,
    }

    private TutorialStateId m_TutorialID = TutorialStateId.None;

    public TutorialStateId TutorialID
    {
        get
        {
            return m_TutorialID;
        }
    }

    public void ResetTutorial()
    {
        m_TutorialID = TutorialStateId.None;
        m_TutPause = true;
    }

    private bool m_TutPause;

    [SerializeField]
    private GameObject TutorialEnemies;
    [SerializeField]
    private GameObject TutorialMove;

    private GameStateId m_StateBeforePaused = GameStateId.Paused;
    private bool m_Paused;

    public bool Paused
    {
        set
        {
            m_Paused = value;

            UIGlobalVariablesScript.Singleton.Joystick.GetComponent<JoystiqScript>().Paused = value;                //Disable the joystick
            UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponentInChildren<Animator>().enabled = !value; //Pause the character's Animation
            GunGameEnemyScript[] evilScripts = this.GetComponentsInChildren<GunGameEnemyScript>();
            for (int i = 0; i < evilScripts.Length; i++)
            {
                //evilScripts[i].Paused = value;                                                                       //Pause each enemy individually
            }
            if (m_Paused)
            {
                m_StateBeforePaused = State;
                State = GameStateId.Paused;
                UIGlobalVariablesScript.Singleton.SoundEngine.StopLoop();
            }
            else
            {
                State = m_StateBeforePaused;
                UIGlobalVariablesScript.Singleton.SoundEngine.PlayLoop(GenericSoundId.GunLoop);
            }
        }
        get { return m_Paused; }
    }

    // Use this for initialization
    void Start()
    {
        CurrentBullets.Clear();
        CurrentBullets.Add(BulletPrefab[Random.Range(0, BulletPrefab.Length)]);

        State = GameStateId.Initialize;

        //AdvanceTutorial();
    }

    void Awake()
    {
        if (ProfilesManagementScript.Singleton.CurrentProfile == null)
        {
            Debug.Log("PlayerProfileData.ActiveProfile = null!");
            ProfilesManagementScript.Singleton.CurrentProfile = PlayerProfileData.CreateNewProfile("Dummy");
            ProfilesManagementScript.Singleton.CurrentAnimin = ProfilesManagementScript.Singleton.CurrentProfile.Characters[(int)PersistentData.TypesOfAnimin.Pi];
        }

        UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterSwapManagementScript>().LoadCharacter(ProfilesManagementScript.Singleton.CurrentAnimin.PlayerAniminId, ProfilesManagementScript.Singleton.CurrentAnimin.AniminEvolutionId);
        UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<MinigameAnimationControllerScript>().LoadAnimator(UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterSwapManagementScript>().CurrentModel);
    }



    // Update is called once per frame
    void Update()
    {
        float LerpFromVal = 0.5f;
        float LerpToVal = 1.5f;
        Vector3 LerpFrom = new Vector3(LerpFromVal, LerpFromVal, LerpFromVal);
        Vector3 LerpTo = new  Vector3(LerpToVal, LerpToVal, LerpToVal);

        float LerpTime = 0.6f;

        switch (State)
        {

            case GameStateId.Initialize:
                {

                    UIControls.SetReadyState(ReadyStates.Ready3);

                    if (TutorialID == TutorialStateId.ShowEnemies)
                        break;
                    FillUpTimer = 0;
                    AmmoTimer = 1;
                    //				GunPrefab.SetActive(true);
                    
                    NextBarrelSpawnTimer = Random.Range(BarrelSpawnMinTime, BarrelSpawnMaxTime);
                    Wave = 0;
                    WaveTimerForNext = WaveTimers[0];
                    RandomCubeTimer = Random.Range(8, 20);
                    Points = 0;

                    //if(GameController.instance.gameType == GameType.SOLO || PhotonNetwork.isMasterClient)

                    //SpawnAnimin(AniminId.Tbo, AniminEvolutionStageId.Baby);
//                    Go321Sprite.gameObject.SetActive(false);
//                    Go321Sprite.mainTexture = Go321Textures[0];
//                    Go321Sprite.gameObject.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
//                    TweenScale.Begin(Go321Sprite.gameObject, 0.6f, new Vector3(1.1f, 1.1f, 1.1f));
//                    MeterBar.width = 0;
//
                    //if (GameController.instance.gameType == GameType.NETWORK)
                    if (false)
                    {
                        State = GameStateId.WaitForPlayersToConnect;
                    }
                    else
                    {
                        State = GameStateId.PrepareToStart3;
                    }

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

                    // GameObject animin = SpawnAniminStart(AniminId.Tbo, AniminEvolutionStageId.Baby);

                    // UIGlobalVariablesScript.Singleton.Joystick.CharacterAnimationRef = animin.GetComponent<AnimationControllerScript>();
                    // UIGlobalVariablesScript.Singleton.Joystick.CharacterControllerRef = animin.GetComponent<CharacterControllerScript>();


                    break;
                }
        /*
            case GameStateId.WaitForPlayersToConnect:
                {
                    if (PhotonNetwork.countOfPlayers == 2)
                    {
                        // List<GameObject> animinsSpawned = new List<GameObject>();

                        //for (int i = 0; i < PhotonNetwork.countOfPlayers; ++i)
                        // {
                        //GameObject animin = SpawnAniminStart(AniminId.Tbo, AniminEvolutionStageId.Baby);
                        //animinsSpawned.Add(animin);
                        //GetComponent<PhotonView>().RPC("ReceiveEventAcquireControlOfCharacter", PhotonNetwork.playerList[i], i);
                        // }

                        //UIGlobalVariablesScript.Singleton.Joystick.CharacterAnimationRef = animin.GetComponent<AnimationControllerScript>();
                        //UIGlobalVariablesScript.Singleton.Joystick.CharacterControllerRef = animin.GetComponent<CharacterControllerScript>();

                        SendEventBeginGame();
                        State = GameStateId.PrepareToStart3;
                    }
                    break;
                }
        */

            case GameStateId.PrepareToStart3:
                {
                    UIControls.Go321.gameObject.SetActive(true);
                    if (FillUpTimer >= 0.25f)
                    {
                        UIControls.SetReadyState(ReadyStates.Ready2);
                        UIControls.Go321.gameObject.transform.localScale = LerpFrom;
                        State = GameStateId.PrepareToStart2;
                    }
                    UIControls.Go321.gameObject.transform.localScale = Vector3.Lerp(UIControls.Go321.gameObject.transform.localScale, LerpTo, LerpTime);

                    FillUpTimer += Time.deltaTime * 0.40f;
                    UIControls.SetBarWidth(FillUpTimer);

                    break;
                }
            case GameStateId.PrepareToStart2:
                {
                    if (FillUpTimer >= 0.5f)
                    {
                        UIControls.SetReadyState(ReadyStates.Ready1);
                        UIControls.Go321.gameObject.transform.localScale = LerpFrom;
                        State = GameStateId.PrepareToStart1;
                    }
                    UIControls.Go321.gameObject.transform.localScale = Vector3.Lerp(UIControls.Go321.gameObject.transform.localScale, LerpTo, LerpTime);

                    FillUpTimer += Time.deltaTime * 0.40f;
                    UIControls.SetBarWidth(FillUpTimer);

                    break;
                }
            case GameStateId.PrepareToStart1:
                {
                    if (FillUpTimer >= 0.75f)
                    {
                        UIControls.Go321.gameObject.SetActive(false);
                        UIControls.SetReadyState(ReadyStates.Go);
                        UIControls.Go321.gameObject.transform.localScale = LerpFrom;
                        State = GameStateId.PrepareToStartGO;
                    }
                    UIControls.Go321.gameObject.transform.localScale = Vector3.Lerp(UIControls.Go321.gameObject.transform.localScale, LerpTo, LerpTime);

                    FillUpTimer += Time.deltaTime * 0.40f;
                    UIControls.SetBarWidth(FillUpTimer);

                    break;
                }
            case GameStateId.PrepareToStartGO:
                {
                    if (FillUpTimer >= 1f)
                    {
                        UIControls.SetReadyState(ReadyStates.Ready3);
                        UIControls.Go321.gameObject.SetActive(false);
                        State = GameStateId.Countdown;
                    }
                    UIControls.Go321.gameObject.transform.localScale = Vector3.Lerp(UIControls.Go321.gameObject.transform.localScale, LerpTo, LerpTime);

                    FillUpTimer += Time.deltaTime * 0.40f;
                    UIControls.SetBarWidth(FillUpTimer);

                    break;
                }



                
            case GameStateId.PrepareToStart:
                {
                    State = GameStateId.Countdown;
                    break;
                }

            case GameStateId.Countdown:
                {
                    State = GameStateId.Playing;

                    UIGlobalVariablesScript.Singleton.SoundEngine.PlayLoop(GenericSoundId.GunLoop);
                    //GameObject.Find("GunfireLoop").GetComponent<AudioSource>().Play();

                    break;
                }

            case GameStateId.Playing:
                {
                    //if(Input.GetButtonDown("Fire1"))
                    //	ShootBulletForward();


                    NextBarrelSpawnTimer -= Time.deltaTime;
                    if (NextBarrelSpawnTimer <= 0)
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
                        State = GameStateId.Completed;
                    }

                      
                    //ReceiveEventAmmoTimer(AmmoTimer);


                    WaveTimerForNext -= Time.deltaTime;
                    if (WaveTimerForNext <= 0)
                    {
                        Wave++;
                        if (Wave == WaveTimers.Length)
                        {
                            State = GameStateId.Completed;
                        }
                        else
                        {
                            WaveTimerForNext = WaveTimers[Wave];
                            int enemyCount = Random.Range(WaveMinEnemies[Wave], WaveMaxEnemies[Wave]);
                            for (int a = 0; a < enemyCount; ++a)
                            {
                                SpawnEnemyStart(0);
                            }
                        }

                    }

                    AutoShootCooldown -= Time.deltaTime;
                    if (AutoShootCooldown <= 0)
                    {
                        AutoShootCooldown = 0.18f;
                        //UIGlobalVariablesScript.Singleton.SoundEngine.Play(GenericSoundId.Jump);

                        for (int a = 0; a < PlayersCharacters.Count; ++a)
                        {
                            ShootBulletForwardStart(a);
                        }
                        
                    }
                    UIControls.SetBarWidth(AmmoTimer);
                    //MeterBar.renderer.set = (int)((AmmoTimer) * MeterBarBackground.width);

                    break;
                }

            case GameStateId.Paused:
                {
                    break;
                }

            case GameStateId.Completed:
                {
                    State = GameStateId.PrepareToExit;
                    UIGlobalVariablesScript.Singleton.SoundEngine.StopLoop();
                    //GameObject.Find("GunfireLoop").GetComponent<AudioSource>().Stop();

                    break;
                }

            case GameStateId.PrepareToExit:
                {
                    UIClickButtonMasterScript.HandleClick(UIFunctionalityId.CloseCurrentMinigame, null);

                    BetweenSceneData.Instance.Points = Points;
                    CharacterProgressScript progressScript = UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>();
                    //progressScript.CurrentAction = ActionId.ExitPortalMainStage;

                    /*
                    UIGlobalVariablesScript.Singleton.GunGameInterface.SetActive(false);
                    UIGlobalVariablesScript.Singleton.JoystickArt.SetActive(false);
                    UIGlobalVariablesScript.Singleton.LoadingScreenRef.SetActive(true);
                    */
                    break;
                }
        }

        UIControls.Points.text = Points.ToString() + " pts";

        if (m_TutorialID == TutorialStateId.ShowEnemies && Input.GetButtonUp("Fire1") && !m_TutPause)
            AdvanceTutorial();
        m_TutPause = false;
    }

    public void AdvanceTutorial()
    {
        if (TutorialEnemies != null)
            TutorialEnemies.SetActive(false);
        if (TutorialMove != null)
            TutorialMove.SetActive(false);

        if (ProfilesManagementScript.Singleton.CurrentProfile.TutorialCanonClashPlayed == false)
        {
            m_TutorialID = (TutorialStateId)((int)m_TutorialID + 1);

            if (m_TutorialID == TutorialStateId.ShowEnemies)
            {
                if (TutorialEnemies != null)
                    TutorialEnemies.SetActive(true);
            }
            else if (m_TutorialID == TutorialStateId.ShowMove)
            {
                if (TutorialMove != null)
                    TutorialMove.SetActive(true);
            }
            else if (m_TutorialID == TutorialStateId.Completed)
            {
                ProfilesManagementScript.Singleton.CurrentProfile.TutorialCanonClashPlayed = true;
                SaveAndLoad.Instance.SaveAllData();
            }
        }
        else
            Debug.Log("False...");
    }
    protected void ReceiveEventAcquireControlOfCharacter(int playerIndex)
    {
        UIGlobalVariablesScript.Singleton.Joystick.CharacterAnimationRef = PlayersCharacters[playerIndex].GetComponent<MinigameAnimationControllerScript>();
        UIGlobalVariablesScript.Singleton.Joystick.CharacterControllerRef = PlayersCharacters[playerIndex].GetComponent<CharacterControllerScript>();
    }
    protected void ReceiveBeginGame()
    {
        State = GameStateId.PrepareToStart3;
    }





    public void Reset()
    {
        State = GameStateId.Initialize;
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

        //GunPrefab.SetActive(false);
        this.gameObject.SetActive(false);

        for (int i = 0; i < SpawnedObjectsAllOthers.transform.childCount; ++i)
            Destroy(SpawnedObjectsAllOthers.transform.GetChild(i).gameObject);

        //for (int i = 0; i < PlayersCharacters.Count; ++i)
        //    Destroy(PlayersCharacters[i]);

        PlayersCharacters.Clear();
        LocalPlayerCharacter = null;

        // SpawnedObjects.Clear();
    }

    public void OnHitByEnemy(GameObject enemy, GameObject character)
    {
        AmmoTimer -= 0.2f;

        //TemporaryDisableCollisionEvent collisionEvent = new TemporaryDisableCollisionEvent(character);
        //PresentationEventManager.Create(collisionEvent);
        character.GetComponent<CharacterControllerScript>().Forces.Add(
            new CharacterForces() { Speed = 800, Direction = -character.transform.forward, Length = 0.3f }
        );


        //Debug.Log("OnHitByEnemy");

        /*   int randomCount = Random.Range(5, 8);
        for (int i = 0; i < randomCount; ++i)
        {
            ShootBulletLost(Random.Range(0.30f, 0.60f), character);
        }*/

        UIGlobalVariablesScript.Singleton.SoundEngine.Play(GenericSoundId.GunGame_Bump_Into_Baddy);

        //		GameObject instance = (GameObject)Instantiate(enemy.GetComponent<GunGameEnemyScript>().Splat);
        //		instance.transform.parent = enemy.transform.parent;
        //		instance.transform.position = enemy.transform.position;
        //		instance.transform.rotation = Quaternion.Euler(instance.transform.rotation.eulerAngles.x, instance.transform.rotation.eulerAngles.y, Random.Range(0, 360));
        //		
        //		UIGlobalVariablesScript.Singleton.GunGameScene.GetComponent<GunsMinigameScript>().SpawnedObjects.Add(instance);
    }


    public void OnBulletHitBarrel(BarrelCollisionScript barrel)
    {
        UIControls.Bar.sprite = barrel.BarFrontTexture;
        UIControls.Icon.sprite = barrel.BuletIcon;

        //Destroy(barrel);
        AmmoTimer += 0.07f;
        if (AmmoTimer >= 1) AmmoTimer = 1;
        string[] prefabs = barrel.BulletPrefabs;
        CurrentBullets.Clear();
        CurrentBullets.AddRange(prefabs);

        Points += 100;

        UIGlobalVariablesScript.Singleton.SoundEngine.Play(GenericSoundId.GunGame_barrel_destroy);

        if (barrel.DestroyedPrefab != null)
        {
            GameObject newProjectile = Instantiate(barrel.DestroyedPrefab) as GameObject;
			newProjectile.transform.parent = SpawnedObjectsAllOthers.transform;
            newProjectile.transform.position = Vector3.zero;
            newProjectile.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
            newProjectile.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
            newProjectile.transform.localPosition = barrel.transform.localPosition;
            //UIGlobalVariablesScript.Singleton.GunGameScene.GetComponent<GunsMinigameScript>().SpawnedObjects.Add(newProjectile);
        
		}

        //UIGlobalVariablesScript.Singleton.GunGameScene.GetComponent<GunsMinigameScript>().SpawnedObjects.Remove(this.gameObject);
        Destroy(barrel.gameObject);
    }
	
    public GameObject SpawnAniminStart(PersistentData.TypesOfAnimin animinid, AniminEvolutionStageId evolution)
    {
        //string modelPath = UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterSwapManagementScript>().GetModelPath(animinid, evolution);
        //RuntimeAnimatorController controller = UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterSwapManagementScript>().GetAnimationControlller(animinid, evolution);

        //Object resource1 = Resources.Load("Prefabs/tbo_baby_multi");
        //	Object resource = Resources.Load(modelPath);
        //GameObject childModel = GameObject.Instantiate(resource) as GameObject;

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

    public GameObject SpawnEnemyStart(int level)
    {
        if (level >= EnemyPrefabs.Length)
            level = EnemyPrefabs.Length - 1;

        GameObject resourceLoad = Resources.Load(EnemyPrefabs[level]) as GameObject;

        Texture2D[] textures = SlimeTextures;
        if (level == 1)
            textures = SlimeLevel2Textures;
        int textureIndex = Random.Range(0, textures.Length);

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
            GameObject resourceLoad = Resources.Load(Barrels[Random.Range(0, Barrels.Length)]) as GameObject;
            newProjectile = Instantiate(resourceLoad) as GameObject;
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
        bool isMasterClientShooting = false;

        GameObject newProjectile = null;
        GameObject resourceLoad = Resources.Load(CurrentBullets[Random.Range(0, CurrentBullets.Count)]) as GameObject;
        newProjectile = Instantiate(resourceLoad) as GameObject;

        newProjectile.GetComponent<ProjectileScript>().SetLocal(true);

        ShootBulletForwardEnd(newProjectile, PlayersCharacters[playerIndex]);
    }

    public void ShootBulletForwardEnd(GameObject newProjectile, GameObject characterShooting)
    {
        newProjectile.transform.parent = SpawnedObjectsAllOthers.transform;
        newProjectile.transform.position = UIGlobalVariablesScript.Singleton.MainCharacterRef.transform.position;
        newProjectile.transform.rotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
        newProjectile.transform.localScale = new Vector3(0.116f, 0.116f, 0.116f);
        newProjectile.transform.position = characterShooting.transform.position + characterShooting.transform.forward * 0.14f + new Vector3(0, 0.05f, 0);
        //newProjectile.AddComponent<ProjectileScript>();
        //newProjectile.velocity = transform.TransformDirection( Vector3( 0, 0, speed) );
        //newProjectile.AddComponent<MeshCollider>();
        newProjectile.GetComponent<Rigidbody>().AddForce(characterShooting.transform.forward * 20000);
        // SpawnedObjects.Add(newProjectile);
    }



    public void ShootBulletLost(float speedVariationFactor, GameObject character)
    {
        GameObject resourceLoad = Resources.Load(CurrentBullets[Random.Range(0, CurrentBullets.Count)]) as GameObject;

        GameObject newProjectile = Instantiate(resourceLoad) as GameObject;
        // Destroy(newProjectile.GetComponent<ProjectileScript>());
        newProjectile.transform.parent = SpawnedObjectsAllOthers.transform;
        newProjectile.transform.position = UIGlobalVariablesScript.Singleton.MainCharacterRef.transform.position;
        newProjectile.transform.rotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
        newProjectile.transform.localScale = new Vector3(0.116f, 0.116f, 0.116f) * Random.Range(0.80f, 1.0f);
        newProjectile.transform.position = character.transform.position + character.transform.forward * 0.14f + new Vector3(0, 0.3f, 0);
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
        newProjectile.renderer.material.color = color;
        //newProjectile.GetComponent<Rigidbody>().AddForce((Vector3.up + direction) * 5000 * speedVariationFactor);
        //newProjectile.GetComponent<Rigidbody>().AddExplosionForce(10, position + new Vector3(0,1,0), 1);


        //SpawnedObjects.Add(newProjectile);
    }
}
