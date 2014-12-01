using UnityEngine;
using System.Collections;

//Defines the different sections of menu which will be loaded and used
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
	DemoCardPage,
	FRONTEND_COUNT,
	CaringPage,
	StatsPage,
	MinigamesPage,
	SettingsPage,
	AchievementsPage,
	PrivacyPolicyPage,
	CreditsPage,
	MAINSCENE_COUNT
}


public class PageID : MonoBehaviour
{
	public Pages ID;
}
public class UiPages : MonoBehaviour
{
	public const string RESOURCE_PATH = "Prefabs/UI/";
	public const string PROFILE_SELECT_PAGE = "ProfilePage";
	public const string NEW_PROFILE_PAGE = "NewProfilePage";
	public const string ANIMIN_SELECT_PAGE = "AniminSelectPage";
	public const string DEMO_CARD_PAGE = "DemoCardPage";
	public const string CARING_PAGE = "CaringPage";
	public const string STATS_PAGE = "StatsPage";
	public const string MINIGAMES_PAGE = "MinigamesPage";
	public const string SETTINGS_PAGE = "SettingsPage";
	public const string ACHIEVEMENTS_PAGE = "AchievementsPage";
	public const string PRIVICY_POLICY_PAGE = "PrivacyPolicyPage";
	public const string CREDITS_PAGE = "CreditsPage";
	private static Pages mCurrentPage;
	private static GameObject[] mPages;
	private static GameObject[] mBackMap;
	private static UiState mCurrentState;

	void Start ()
	{
		SwitchState();
		LoadPrefabs ();
		SetupBackMap();
		Init ();
	}
	void SwitchState()
	{
		if(Application.loadedLevelName == "Menu")
		{
			mCurrentState = UiState.Frontend;
		}
		else //TODO Update to a proper switch
		{
			mCurrentState = UiState.MainScene;
		}
	}
	void Init()
	{
		switch(mCurrentState)
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

		mPages [(int)mCurrentPage].SetActive (true);
	}

	private void LoadPrefabs()
	{
		mPages = new GameObject[(int)Pages.MAINSCENE_COUNT];
		int start = mCurrentState == UiState.Frontend ? 0 : ((int)Pages.FRONTEND_COUNT) + 1;
		int end =(int)( mCurrentState == UiState.Frontend ? Pages.FRONTEND_COUNT : Pages.MAINSCENE_COUNT);
		for (int i = start; i < end; i++)
		{
			Pages page = (Pages)i;
			string name = GetPrefabName(page);
			Object obj = Resources.Load(name);
			GameObject go = (GameObject)Instantiate(obj);
			go.AddComponent<PageID>().ID = page;
			go.SetActive(false);
			mPages[i] = go;
		}
	}
	private void SetupBackMap()
	{
		mBackMap = new GameObject[(int)Pages.MAINSCENE_COUNT];
		mBackMap [(int)Pages.ProfileSelectPage] = null;
		mBackMap [(int)Pages.NewProfilePage] = mPages[(int)Pages.ProfileSelectPage];
		mBackMap [(int)Pages.AniminSelectPage] = mPages[(int)Pages.ProfileSelectPage];
		mBackMap [(int)Pages.DemoCardPage] = mPages[(int)Pages.ProfileSelectPage];
		mBackMap [(int)Pages.CaringPage] = null;
		mBackMap [(int)Pages.StatsPage] = mPages [(int)Pages.CaringPage];
		mBackMap [(int)Pages.MinigamesPage] = mPages [(int)Pages.CaringPage];
		mBackMap [(int)Pages.SettingsPage] = mPages [(int)Pages.CaringPage];
		mBackMap [(int)Pages.AchievementsPage] = mPages [(int)Pages.CaringPage];
		mBackMap [(int)Pages.PrivacyPolicyPage] = mPages [(int)Pages.SettingsPage];
		mBackMap [(int)Pages.CreditsPage] = mPages [(int)Pages.SettingsPage];
	}
	private static string GetPrefabName(Pages page)
	{
		string name = "";
		switch(page)
		{
		case Pages.ProfileSelectPage:
			name = PROFILE_SELECT_PAGE;
			break;
		case Pages.NewProfilePage:
			name = NEW_PROFILE_PAGE;
			break;
		case Pages.AniminSelectPage:
			name = ANIMIN_SELECT_PAGE;
			break;
		case Pages.DemoCardPage:
			name = DEMO_CARD_PAGE;
			break;
		case Pages.CaringPage:
			name = CARING_PAGE;
			break;
		case Pages.StatsPage:
			name = STATS_PAGE;
			break;
		case Pages.MinigamesPage:
			name = MINIGAMES_PAGE;
			break;
		case Pages.SettingsPage:
			name = SETTINGS_PAGE;
			break;
		case Pages.AchievementsPage:
			name = ACHIEVEMENTS_PAGE;
			break;
		case Pages.PrivacyPolicyPage:
			name = PRIVICY_POLICY_PAGE;
			break;
		case Pages.CreditsPage:
			name = CREDITS_PAGE;
			break;
		default:
			Debug.LogError("NO SUCH PAGE: " + page.ToString());
			name = null;
			break;
		}

		return RESOURCE_PATH + name;
	}

	private static GameObject findRoot(GameObject caller)
	{
		GameObject g = caller;
		while (g.transform.parent != null) 
		{
			g = g.transform.parent.gameObject;
		}
		return g;
	}

	public static void LeaveScene(string name)
	{
		Application.LoadLevel (name);
	}

	public static void LeaveScene(int num)
	{
	}

	public static void Back()
	{
		GameObject oldPage = mPages [(int)mCurrentPage];
		GameObject newPage = mBackMap [(int)mCurrentPage];
		Transition (oldPage, newPage);
	}

	public static void Next(Pages next)
	{
		GameObject oldPage = mPages [(int)mCurrentPage];
		GameObject newPage = mPages [(int)next];
		Transition (oldPage, newPage);
	}

	private static void Transition(GameObject from, GameObject to)
	{
		from.SetActive (false);
		to.SetActive (true);
		mCurrentPage = to.GetComponent<PageID> ().ID;
	}


}
