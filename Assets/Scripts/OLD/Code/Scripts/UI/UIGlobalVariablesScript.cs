using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIGlobalVariablesScript : MonoBehaviour
{
    public static UIGlobalVariablesScript Singleton;

    public GameObject ARCamera{ get { return MainARHandler.Instance.gameObject; } }
    public Camera ARCameraComponent{ get { return MainARHandler.Instance.gameObject.GetComponent<Camera>(); } }
    // Set from the Editor

    public GameObject MainMenuPopupObjectRef;
    public GameObject MinigamesMenuMasterScreenRef;
    public GameObject CaringScreenRef;
    public GameObject StatsScreenRef;
    public GameObject PicturesScreenRef;
    public GameObject StartMinigameScreenRef;
    public GameObject MinigameMenuScreeRef;
    public GameObject InsideMinigamesMasterScreenRef;
    public GameObject SpaceshipGameScreenRef;
    public GameObject CuberunnerGamesScreenRef;
    public GameObject AROnIndicator;
    public GameObject AROffIndicator;
    public GameObject SettingsScreenRef;
    public GameObject RequiresGamecardScreenRef;
    public GameObject AchievementsScreenRef;
    public GameObject LoadingScreenRef;
    public GameObject CreditsScreenRef;
    public GameObject ExitWebviewButton;

    public GameObject SpaceshipMinigameSceneRef;
    public GameObject CubeRunnerMinigameSceneRef;
    public GameObject GardenSceneRef;
    public GameObject ARSceneRef;
    public GameObject NonSceneRef;
    public GameObject NonARWorldRef;
    public GameObject ARWorldRef;
    public GameObject DragableUI3DObject;
    //public GameObject ARSceneContainer;


    // Interface Bars
    public GameObject FitnessControlBarRef;
    public GameObject HealthControlBarRef;
    public GameObject HungryControlBarRef;
    public GameObject HapynessControlBarRef;
    public GameObject EvolutionControlBarRef;
    public GameObject ARPortal;
    public GameObject Shadow;
    public GameObject SoundSprite;

    //public AnimationControllerScript MainCharacterAnimationControllerRef;
    public GameObject MainCharacterRef;
    public GameObject PanelMedicine;
    public GameObject PanelFoods;
    public GameObject PanelItems;
    public Text TextForStarsInMiniCollector;
    public Sprite EvolutionProgressSprite;

    public GameObject PopupIndicator;
    //public GameObject ImageTarget;
    public GameObject MinigameInterruptedMenu;

    //public TrackVuforiaScript Vuforia;
    //public GameObject IndicatorAboveHead;
    public GameObject AlarmUI;
    public SoundEngineScript SoundEngine;
    public JoystiqScript Joystick;
    public GameObject JoystickArt;
    public GameObject UIRoot;
    public GameObject StereoUI;
    public Text CurrentArtistLabel;
    public Text CurrentSongLabel;
    public Image PlayPauseButton;
    public Image ProgressSongBar;
    public GameObject LightbulbUI;
    public GameObject LightbulbSwitchButton;
    public GameObject LightbulbRedButton;
    public GameObject LightbulbGreenButton;

    public GameObject TracklistPanel;
    public GameObject PlaySongPanel;
    public GameObject PanelWithAllSongs;

    // Set from the code, keep static
    public static GameObject ButtonTriggeredMainMenuPopupRef;
    public static UIFunctionalityId SelectedMinigameToPlay;

    public GameObject GunGameScene;
    public GameObject GunGameInterface;

    public GameObject ShootButton;
    public GameObject JumbButton;
    public GameObject PauseGameButton;
    public GameObject PausedScreen;
    public GameObject MultiplayerOptionsScreen;
    public GameObject EnterFriendsNameChat;
    public Text ZefTokensUI;
    public Text ItemsFoodMedicineLabel;
    public GameObject EDMBoxUI;
    public GameObject JunoUI;
    public GameObject PianoUI;
    public GameObject ItemScrollView;
    public GameObject FoodButton;
    public GameObject MedicineButton;
    public GameObject ItemsButton;
    public GameObject StatsButton;
    public GameObject MinigamesButton;

    public GameObject ParentalGateway;
    public GameObject PurchaseAniminViaPaypal;


    public enum ActiveState
    {
Caring,
        Collecting,
Gun,
        None}

    ;

    public ActiveState CurrentlyActive
    {
        get
        {
            if (CubeRunnerMinigameSceneRef != null)
                return ActiveState.Collecting;
            else if (GunGameScene != null)
                return ActiveState.Gun;
            else
                return ActiveState.Caring;
        }
    }

    public GameObject CurrentMiniGameSceneRef
    {
        get
        {
            switch (UIGlobalVariablesScript.Singleton.CurrentlyActive)
            {
                case UIGlobalVariablesScript.ActiveState.Caring:
                    return GardenSceneRef;
                    break;
                case UIGlobalVariablesScript.ActiveState.Collecting:
                    return CubeRunnerMinigameSceneRef;
                    break;
                case UIGlobalVariablesScript.ActiveState.Gun:
                    return GunGameScene;
                    break;
            }
            return null;
        }
    }

    public GameObject CurrentUIScreenRef
    {
        get
        {
			
            switch (UIGlobalVariablesScript.Singleton.CurrentlyActive)
            {
                case UIGlobalVariablesScript.ActiveState.Caring:
                    return CaringScreenRef;
                    break;
                case UIGlobalVariablesScript.ActiveState.Collecting:
                    return CuberunnerGamesScreenRef;
                    break;
                case UIGlobalVariablesScript.ActiveState.Gun:
                    return GunGameInterface;
                    break;
            }
            return null;
        }
    }

    public void LoadAniminPaypalPurchaseScreen()
    {
        PurchaseAniminViaPaypal = (GameObject)GameObject.Instantiate(Resources.Load("NGUIPrefabs/UI - PaypalCharacterSelect"));
        PurchaseAniminViaPaypal.SetActive(false);
    }

    public void OpenParentalGateway(GameObject prev, GameObject next, bool LogPurchase = false)
    {
        ParentalGateway = (GameObject)Instantiate(Resources.Load("Prefabs/Parental Gateway"));
        //UIRoot root = GameObject.FindObjectOfType<UIRoot>();
        //ParentalGateway.transform.parent = root.gameObject.transform;
        ParentalGateway.transform.localScale = Vector3.one;
        ParentalGateway.transform.localPosition = Vector3.zero;
        ParentalGateway.GetComponent<ParentalGateway>().Open(prev, next, LogPurchase);
    }

    public void LaunchWebview(PersistentData.TypesOfAnimin type, decimal price)
    {
        Debug.Log("Buying with webview");

        StartCoroutine(Account.Instance.WWWSendData(false, Account.Instance.UserName, "", "", "", "", ""));
        ExitWebviewButton.SetActive(true);
		
        int x = 0;
        int y = 100;
        int width = Screen.width - (x * 4);
        int height = Screen.height - (y * 4);

        Debug.Log(string.Format("Screen width : {0}\nScreen height : {1}\nScreen Dpi : {2}\nScreen Dimensions : {3}, {4}\nWebview x = {5}, y = {6}",
                Screen.width,
                Screen.height,
                Screen.dpi,
                width,
                height,
                x,
                y));
        string GetUrlAddition = "?Type=" + type + "&Price=" + price + "&UserID=" + Account.Instance.UniqueID; 
#if UNITY_IOS
		int w = 360;
		int h = 300;
		
		if( Screen.width > 960 )
		{
			w *= 2;
			h *= 2;
		}

		EtceteraBinding.inlineWebViewShow( x, y, w, h );
        EtceteraBinding.inlineWebViewSetUrl( "http://terahard.org/Teratest/DatabaseAndScripts/BuyAniminFromPaypal.php"+ GetUrlAddition );
//        Debug.Log("http://terahard.org/Teratest/DatabaseAndScripts/BuyAniminFromPaypal.php"+ GetUrlAddition);0
#elif UNITY_ANDROID

        EtceteraAndroid.inlineWebViewShow("http://terahard.org/Teratest/DatabaseAndScripts/BuyAniminFromPaypal.php" + GetUrlAddition, x, y, Screen.width - (x * 2), Screen.height - (y * 2));

#endif
		
    }

    void Awake()
    {
        Init();
    }

    // Use this for initialization
    void Start()
    {

	
    }

    public void Init(){
        Debug.Log("New UIGlobalVariablesSingleton Set");
        Singleton = this;
    }
	
    // Update is called once per frame
    void Update()
    {
        //Debug.Log ("According to the scientific method, we're are in the state: "+CurrentlyActive+";");
    }

    void OnApplicationPause()
    {

    }

    void OnApplicationResume()
    {
    }
}
