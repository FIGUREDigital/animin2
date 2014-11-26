using UnityEngine;
using System.Collections;

public enum Pages
{
	ProfileSelectPage,
	NewProfilePage,
	AniminSelectPage,
	Count
}
public class UiPages : MonoBehaviour
{
	public const string PROFILE_SELECT_PAGE = "Prefabs/UI/ProfilePage";
	public const string NEW_PROFILE_PAGE = "Prefabs/UI/NewProfilePage";
	public const string ANIMIN_SELECT_PAGE = "Prefabs/UI/AniminSelectPage";



	private static string GetPrefabName(Pages page)
	{
		switch(page)
		{
		case Pages.ProfileSelectPage:
			return PROFILE_SELECT_PAGE;
			break;
		case Pages.NewProfilePage:
			return NEW_PROFILE_PAGE;
			break;
		case Pages.AniminSelectPage:
			return ANIMIN_SELECT_PAGE;
			break;
		default:
			Debug.LogError("NO SUCH PAGE");
			return null;
			break;
		}
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

	public static void LeavePage(GameObject called, Pages nextPage)
	{
		LeavePage (called, GetPrefabName (nextPage));
	}

	public static void LeavePage(GameObject called, string nextPage)
	{
		Instantiate(Resources.Load(nextPage));
		Destroy(findRoot(called));
	}

	public static void LeaveScene(string name)
	{
	}

	public static void LeaveScene(int num)
	{
	}

}
