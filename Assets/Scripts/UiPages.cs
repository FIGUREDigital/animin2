using UnityEngine;
using System.Collections;

public enum Pages
{
	ProfileSelectPage,
	NewProfilePage,
	AniminSelectPage,
	DemoCardPage,
	Count
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
	private static Pages mCurrentPage;
	private static GameObject[] mPages;
	private static GameObject[] mBackMap;

	void Start ()
	{
		LoadPrefabs ();
		SetupBackMap();
		Init ();
	}
	void Init()
	{
		mCurrentPage = Pages.ProfileSelectPage;
		mPages [(int)mCurrentPage].SetActive (true);
	}

	private void LoadPrefabs()
	{
		mPages = new GameObject[(int)Pages.Count];
		for (int i = 0; i < (int)Pages.Count; i++)
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
		mBackMap = new GameObject[(int)Pages.Count];
		mBackMap [(int)Pages.ProfileSelectPage] = null;
		mBackMap [(int)Pages.NewProfilePage] = mPages[(int)Pages.ProfileSelectPage];
		mBackMap [(int)Pages.AniminSelectPage] = mPages[(int)Pages.ProfileSelectPage];
		mBackMap [(int)Pages.DemoCardPage] = mPages[(int)Pages.ProfileSelectPage];
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
		default:
			Debug.LogError("NO SUCH PAGE");
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
