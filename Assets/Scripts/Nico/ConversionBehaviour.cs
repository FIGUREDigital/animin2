using UnityEngine;
using System.Collections;

public class ConversionBehaviour : MonoBehaviour 
{
	public Animator fadePanel;
	public GameObject fadeObj;
	public static bool startConversion;
	public static bool fadeAlpha;
	public static bool firstTime;
	static bool newSession = true;
	static bool playVideo = false;

	IEnumerator ConversionVideo ()
	{
		yield return new WaitForSeconds (1.0f);
		if (firstTime == false)
		{
			Handheld.PlayFullScreenMovie ("animinConversionVid1.mp4", Color.black, FullScreenMovieControlMode.CancelOnInput);
		}

		if (firstTime == true)
		{
			Handheld.PlayFullScreenMovie ("animinConversionVid1.mp4", Color.black, FullScreenMovieControlMode.Hidden);
		}
		startConversion = false;
		firstTime = false;
		yield return new WaitForSeconds (1.0f);
		newSession = false;
		Debug.Log ("Continue Session");
	}








	IEnumerator FadePanel ()
	{
		fadeObj.SetActive(true);
		fadePanel.speed = 1;
		yield return new WaitForSeconds (1.0f);
		fadePanel.speed = -1;
		yield return new WaitForSeconds (1.0f);
		fadeObj.SetActive(false);
		fadePanel.speed = 0;
		fadeAlpha = false;
	}

	// Use this for initialization
	void Start () 
	{
		fadePanel.speed = 0;
		startConversion = false;
		fadeAlpha = false;
		firstTime = true;
		fadeObj.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () 
	{
//		Debug.Log (fadePanel.speed);
		if (startConversion == true)
		{
			StartCoroutine("ConversionVideo");
		}

		else if (startConversion == false)
		{
			StopCoroutine("ConversionVideo");
		}

		if (fadeAlpha == true)
		{
			StartCoroutine("FadePanel");
		}

		else if (fadeAlpha == false)
		{
			StopCoroutine("FadePanel");
		}
	}

	void OnApplicationPause(bool pause)
	{
		Debug.Log ("OnApplicationPause "+pause+" playVideo = "+playVideo);
		if(!pause)
		{
			// Unpauseing
			if( !playVideo)
			{
				newSession = true;
			}
			playVideo = false;
		}
	}
	
	public void PlayVideo ()
	{
		PlayVideo(true);
	}

	static public void PlayVideo (bool force)
	{
		return;
		/*if(force || newSession)
		{
			playVideo = true;
			newSession = false;
			startConversion = true;
			fadeAlpha = true;
		}*/
	}
}
