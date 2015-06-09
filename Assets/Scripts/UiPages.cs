using UnityEngine;

//Defines the different sections of menu which will be loaded and used
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;


#region Enums

public enum UiState
{
    Frontend,
    MainScene,
    Count
}
//Defines all pages, seperated by their counts

public enum Pages
{
    ProfileSelectPage,
    NewProfilePage,
    AniminSelectPage,
    PurchasePage,
    LoadingPage,
    LevelLoadingPage,
    DemoCardPage,
    DialogPage,
    NOTUSED_CodeErrorPage,
    NOTUSED_CodeUsedErrorPage,
    NOTUSED_NotValidCodeErrorPage,
    AddressInputPage,
    CodeInputPage,
    NOTUSED_RestoreSuccessPage,
    NOTUSED_RestoreFailPage,
    FRONTEND_COUNT,
    CaringPage,
    StatsPage,
    MinigamesPage,
    SettingsPage,
    AchievementsPage,
    PrivacyPolicyPage,
    CreditsPage,
    CubeMinigamePage,
    GunMinigamePage,
    JoystickPage,
    MAINSCENE_COUNT
}

// Enums
#endregion


public class UiPages : MonoBehaviour
{
    #region Variables

    #region Public

    /// <summary>
    /// List of prefabs loaded with the UI
    /// </summary>
    public UiPageInfo[] UIPrefabPages;


    public static Pages CurrentPage { get { return mCurrentPage; } }

    // Public
    #endregion

    #region Private

    private static Pages mCurrentPage;
    private static Pages mPrevPage;

    //private static GameObject[] uiPages;
    private static GameObject[] uiPages;
    private static GameObject[] mBackMap;
    private static UiState mCurrentState;
    private static bool mNextScheduled;
    private static float mCountdown;
    private static Pages mScheduledPage;

    /// <summary>
    /// Static instance
    /// </summary>
    private static UiPages instance;

    // Private
    #endregion

    // Variables
    #endregion

    #region Awake

    /// <summary>
    /// Awake function
    /// </summary>
    void Awake()
    {
        instance = this;

        // fix the pages
        uiPages = new GameObject[(int)Pages.MAINSCENE_COUNT];
        for (int i = 0, j = UIPrefabPages.Length; i < j; i++)
        {
            // get the object
            GameObject UIObject = UIPrefabPages[i].UIObject;

            // set the page in the array
            uiPages[(int)UIPrefabPages[i].page] = UIObject;

            // DIRTY hack until we can fix this better, since so many other scripts rely on this
            UIPrefabPages[i].UIObject.GetComponent<PageID>().ID = UIPrefabPages[i].page;
        }
    }

    // Awake
    #endregion

    #region GetPage

    public static GameObject GetPage(Pages page)
    {
        return uiPages[(int)page];
    }

    // GetPage
    #endregion

    #region Start

    void Start()
    {
        SwitchState();
        //LoadPrefabs();
        SetupBackMap();
        Init();
    }

    // Start
    #endregion

    #region Init

    void Init()
    {
        switch (mCurrentState)
        {
            case UiState.Frontend:
                mCurrentPage = Pages.ProfileSelectPage;
                break;
            case UiState.MainScene:
                mCurrentPage = Pages.CaringPage;
                break;
            default:
                mCurrentPage = Pages.ProfileSelectPage;
                break;
        }

        uiPages[(int)mCurrentPage].SetActive(true);
    }

    // Init
    #endregion

    #region SwitchState

    void SwitchState()
    {
        if (Application.loadedLevelName == "Menu")
        {
            mCurrentState = UiState.Frontend;
        }
        else //TODO Update to a proper switch
        {
            mCurrentState = UiState.MainScene;
        }
    }

    // SwitchState
    #endregion

    #region LoadPrefabs

    //private void LoadPrefabs()
    //{
    //    uiPages = new GameObject[(int)Pages.MAINSCENE_COUNT];
    //    int start = mCurrentState == UiState.Frontend ? 0 : ((int)Pages.FRONTEND_COUNT) + 1;
    //    int end = (int)(mCurrentState == UiState.Frontend ? Pages.FRONTEND_COUNT : Pages.MAINSCENE_COUNT);
    //    for (int i = start; i < end; i++)
    //    {
    //        Pages page = (Pages)i;
    //        string name = GetPrefabName(page);
    //        Object obj = Resources.Load(name);
    //        if (obj == null)
    //        {
    //            Debug.Log("Failed to load prefab for page " + page.ToString());
    //        }
    //        GameObject go = (GameObject)Instantiate(obj);
    //        go.AddComponent<PageID>().ID = page;
    //        go.SetActive(false);
    //        uiPages[i] = go;
    //    }
    //}

    // LoadPrefabs
    #endregion

    #region SetupBackMap

    private void SetupBackMap()
    {
        mBackMap = new GameObject[(int)Pages.MAINSCENE_COUNT];
        mBackMap[(int)Pages.ProfileSelectPage] = null;
        mBackMap[(int)Pages.NewProfilePage] = uiPages[(int)Pages.ProfileSelectPage];
        mBackMap[(int)Pages.AniminSelectPage] = uiPages[(int)Pages.ProfileSelectPage];
        mBackMap[(int)Pages.PurchasePage] = uiPages[(int)Pages.AniminSelectPage];
        mBackMap[(int)Pages.LoadingPage] = uiPages[(int)Pages.PurchasePage];
        mBackMap[(int)Pages.DemoCardPage] = uiPages[(int)Pages.ProfileSelectPage];
        mBackMap[(int)Pages.AddressInputPage] = uiPages[(int)Pages.AniminSelectPage];
        mBackMap[(int)Pages.CodeInputPage] = uiPages[(int)Pages.PurchasePage];
        mBackMap[(int)Pages.DialogPage] = uiPages[(int)Pages.AniminSelectPage];
        mBackMap[(int)Pages.NOTUSED_RestoreSuccessPage] = uiPages[(int)Pages.AniminSelectPage];
        mBackMap[(int)Pages.NOTUSED_RestoreFailPage] = uiPages[(int)Pages.AniminSelectPage];
        mBackMap[(int)Pages.NOTUSED_NotValidCodeErrorPage] = uiPages[(int)Pages.PurchasePage];
        mBackMap[(int)Pages.NOTUSED_CodeUsedErrorPage] = uiPages[(int)Pages.PurchasePage];
        mBackMap[(int)Pages.NOTUSED_CodeErrorPage] = uiPages[(int)Pages.PurchasePage];
        mBackMap[(int)Pages.CaringPage] = null;
        mBackMap[(int)Pages.StatsPage] = uiPages[(int)Pages.CaringPage];
        mBackMap[(int)Pages.MinigamesPage] = uiPages[(int)Pages.CaringPage];
        mBackMap[(int)Pages.SettingsPage] = uiPages[(int)Pages.CaringPage];
        mBackMap[(int)Pages.AchievementsPage] = uiPages[(int)Pages.CaringPage];
        mBackMap[(int)Pages.PrivacyPolicyPage] = uiPages[(int)Pages.SettingsPage];
        mBackMap[(int)Pages.CreditsPage] = uiPages[(int)Pages.SettingsPage];
        mBackMap[(int)Pages.MinigamesPage] = uiPages[(int)Pages.CaringPage];
    }

	public static void SetDialogBackPage(Pages page)
	{		
		mBackMap[(int)Pages.DialogPage] = uiPages[(int)page];//Pages.AniminSelectPage];
	}

    // SetupBackMap
    #endregion

    #region GetPrefabName

    //private static string GetPrefabName(Pages page)
    //{
    //    string name = "";
    //    switch (page)
    //    {
    //        case Pages.ProfileSelectPage:
    //            name = PROFILE_SELECT_PAGE;
    //            break;
    //        case Pages.NewProfilePage:
    //            name = NEW_PROFILE_PAGE;
    //            break;
    //        case Pages.AniminSelectPage:
    //            name = ANIMIN_SELECT_PAGE;
    //            break;
    //        case Pages.PurchasePage:
    //            name = PURCHASE_PAGE;
    //            break;
    //        case Pages.LoadingPage:
    //            name = LOADING_PAGE;
    //            break;
    //        case Pages.LevelLoadingPage:
    //            name = LEVEL_LOADING_PAGE;
    //            break;
    //        case Pages.DemoCardPage:
    //            name = DEMO_CARD_PAGE;
    //            break;
    //        case Pages.ConnectionErrorPage:
    //            name = CONNECTION_ERROR_PAGE;
    //            break;
    //        case Pages.CodeErrorPage:
    //            name = CODE_ERROR_PAGE;
    //            break;
    //        case Pages.CodeUsedErrorPage:
    //            name = CODE_USED_ERROR_PAGE;
    //            break;
    //        case Pages.NotValidCodeErrorPage:
    //            name = NOT_VALID_CODE_ERROR_PAGE;
    //            break;
    //        case Pages.AddressInputPage:
    //            name = ADDRESS_INPUT_PAGE;
    //            break;
    //        case Pages.CodeInputPage:
    //            name = CODE_INPUT_PAGE;
    //            break;
    //        case Pages.RestoreSuccessPage:
    //            name = RESTORE_SUCCESS_PAGE;
    //            break;
    //        case Pages.RestoreFailPage:
    //            name = RESTORE_FAIL_PAGE;
    //            break;
    //        case Pages.CaringPage:
    //            name = CARING_PAGE;
    //            break;
    //        case Pages.StatsPage:
    //            name = STATS_PAGE;
    //            break;
    //        case Pages.MinigamesPage:
    //            name = MINIGAMES_PAGE;
    //            break;
    //        case Pages.SettingsPage:
    //            name = SETTINGS_PAGE;
    //            break;
    //        case Pages.AchievementsPage:
    //            name = ACHIEVEMENTS_PAGE;
    //            break;
    //        case Pages.PrivacyPolicyPage:
    //            name = PRIVICY_POLICY_PAGE;
    //            break;
    //        case Pages.CreditsPage:
    //            name = CREDITS_PAGE;
    //            break;
    //        case Pages.CubeMinigamePage:
    //            name = CUBE_MINIGAME_PAGE;
    //            break;
    //        case Pages.GunMinigamePage:
    //            name = GUN_MINIGAME_PAGE;
    //            break;
    //        case Pages.JoystickPage:
    //            name = JOYSTICK_PAGE;
    //            break;
    //        default:
    //            Debug.LogError("NO SUCH PAGE: " + page.ToString());
    //            name = null;
    //            break;
    //    }

    //    return RESOURCE_PATH + name;
    //}

    // GetPrefabName
    #endregion

    #region LeaveScene

    public static void LeaveScene(string name)
    {
        Application.LoadLevel(name);
    }

    public static void LeaveScene(int num)
    {
    }

    // LeaveScene
    #endregion

    #region Back

    public static void Back()
    {
        GameObject oldPage = uiPages[(int)mCurrentPage];
        GameObject newPage = mBackMap[(int)mCurrentPage];
        Transition(oldPage, newPage);
    }

    // Back
    #endregion

    #region Next

    public static void Next(Pages next, float delay)
    {
        mNextScheduled = true;
        mCountdown = delay;
        mScheduledPage = next;
    }
    public static void Next(Pages next)
    {
        mNextScheduled = false;
        Debug.Log("Next Page : [" + next.ToString() + "]");
        GameObject oldPage = uiPages[(int)mCurrentPage];
        GameObject newPage = uiPages[(int)next];
        Transition(oldPage, newPage);
    }

    // Next
    #endregion

    #region OpenSettings

    public static void OpenSettings()
    {
        mPrevPage = mCurrentPage;
        mCurrentPage = Pages.SettingsPage;
        uiPages[(int)mCurrentPage].SetActive(true);
        uiPages[(int)mPrevPage].SetActive(false);
    }

    // OpenSettings
    #endregion

    #region Close

    public static void Close()
    {
        uiPages[(int)mCurrentPage].SetActive(false);
        uiPages[(int)mPrevPage].SetActive(true);
        mCurrentPage = mPrevPage;
    }

    // Close
    #endregion

    #region Transition

    private static void Transition(GameObject from, GameObject to)
    {
        if (from != null)
            from.SetActive(false);
        to.SetActive(true);
        mCurrentPage = to.GetComponent<PageID>().ID;
    }

    // Transition
    #endregion

    #region IsMouseOverUI

    public static bool IsMouseOverUI()
    {
        PointerEventData pe = new PointerEventData(EventSystem.current);

		pe.position = Input.mousePosition;//Phi.SingletonPrefab.instances["UICamera"].GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);//Input.mousePosition;
		
//		Debug.Log ("IsMouseOver "+Input.mousePosition+", "+pe.position+ " "+EventSystem.current.IsPointerOverGameObject()+" "+EventSystem.current.IsPointerOverGameObject(0));

        List<RaycastResult> hits = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pe, hits);

        bool hit = false;
        GameObject hgo = null;
        string gos = "";
        foreach (RaycastResult h in hits)
        {
            GameObject g = h.gameObject;
            gos += (gos == "") ? g.name : " | " + g.name;
            hit = (g.name != "BackgroundEventCatcher" &&
            (g.GetComponent<Button>() || g.GetComponent<Canvas>() || g.GetComponent<InputField>() || g.GetComponent<UnityEngine.UI.Graphic>())
            );
            if (hit)
            {
                break;
            }
        }
//        Debug.Log("gos : ["+gos+"]; hit="+hit);
        //if (hit) Debug.Log("Hit");
        return hit;
    }

    // IsMouseOverUI
    #endregion

    #region Update

    void Update()
    {
        // exit if not scheduled
        if (!mNextScheduled)
            return;

        if (mCountdown > 0)
        {
            mCountdown -= Time.deltaTime;
        }
        else
        {
            Next(mScheduledPage);
        }
    }

    // Update
    #endregion
}
