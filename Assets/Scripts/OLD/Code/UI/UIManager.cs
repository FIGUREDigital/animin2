using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour 
{
	private const float tweenDuration = 0.3f;
	private const float tweenDelay = 0.1f;
	private static float mClock;
	private static GameObject mOldScreen;

	#region Singleton
	
	private static UIManager s_Instance;
	
	public static UIManager Instance
	{
		get
		{
			if ( s_Instance == null )
			{
				s_Instance = new UIManager();
			}
			return s_Instance;
		}
	}
	
	#endregion

	public void UITransition(GameObject oldScreen, GameObject newScreen)
	{
		Vector3 offset = new Vector3(Screen.width*3, 0, 0);
//		TweenPosition tpOld = oldScreen.AddComponent<TweenPosition>();
//		TweenPosition tpNew = newScreen.AddComponent<TweenPosition>();
//		tpOld.from = oldScreen.transform.position;
//		tpNew.to = tpOld.from;
//		tpOld.to = tpOld.from + offset;
//		tpNew.from = tpNew.to - offset;
//		tpNew.gameObject.transform.position = tpNew.from;
//
//		tpOld.duration = tweenDuration;
//		tpNew.duration = tweenDuration;
//		tpNew.delay = tweenDelay;

//		newScreen.SetActive(true);
//		oldScreen.transform.position = tpNew.to;
//
//		float totalTime = tweenDelay + tweenDuration;
//		Destroy(tpOld, totalTime);
//		Destroy(tpNew, totalTime);

		mOldScreen = oldScreen;
	}

	private void DeactivateScreen()
	{
		mOldScreen.transform.position = new Vector3();
		mOldScreen.SetActive(false);
		mOldScreen = null;
	}
}
