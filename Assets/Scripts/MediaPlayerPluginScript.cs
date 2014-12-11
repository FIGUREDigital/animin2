using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine.UI;


public class MediaPlayerPluginScript : MonoBehaviour
{
	public GameObject TrackSongPrefab;
	public static bool IsPlaying;

	void Start () 
	{
		// not necessary now, but we'll use it later
		if (Application.platform == RuntimePlatform.IPhonePlayer) 
		{
			int maxSongs = _initMediaPlayer();
			//Debug.Log("MAX SONGS: " + maxSongs.ToString());

			for(int i=0;i<maxSongs;++i)
			{
				GameObject instance = (GameObject)Instantiate(TrackSongPrefab);
				instance.transform.parent = UIGlobalVariablesScript.Singleton.PanelWithAllSongs.transform;
				instance.transform.localPosition = new Vector3(0, 30 * i, 0);
				instance.transform.localScale = Vector3.one;
			
				Text childLabel = instance.transform.GetChild(0).gameObject.GetComponent<Text>();

				childLabel.text = _getNextSongFromList();
				instance.GetComponent<TrackSongIndexScript>().TrackIndex = i;
				//childLabel.tag = i.ToString();

			}
		}

		GameObject ninstance = (GameObject)Instantiate(TrackSongPrefab);
		ninstance.transform.parent = UIGlobalVariablesScript.Singleton.PanelWithAllSongs.transform;
		ninstance.transform.localPosition = new Vector3(0, 30, 0);
		ninstance.transform.localScale = Vector3.one;
	}

	public void PlayPause()
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer) 
		{
			_play();
		}
	}

	public void NextSong()
	{

		if (Application.platform == RuntimePlatform.IPhonePlayer) 
		{
			_moveToNextSong();
		}
	}

	public void PreviousSong()
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer) 
		{
			_moveToPreviousSong();
		}
	}

	void Update()
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer) 
		{
			//Debug.Log("progress: " + _getProgress().ToString());

			UIGlobalVariablesScript.Singleton.ProgressSongBar.Width = (int)(698 * _getProgress());
		}
	}

	public void PlaySongAtIndex(int index)
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer) 
		{
			_playSongAtIndex(index);
		}
	}

	public void UpdateTrackInfo(string artist)
	{
		//Debug.Log("THIS IS A UpdateTrackInfo MESSAGE!!!!!!!!!!!!!:" + artist);
		UIGlobalVariablesScript.Singleton.CurrentSongLabel.text = artist;
	}

	public void UpdateArtistInfo(string artist)
	{
		//Debug.Log("THIS IS A UpdateArtistInfo MESSAGE!!!!!!!!!!!!!:" + artist);
		UIGlobalVariablesScript.Singleton.CurrentArtistLabel.text = artist;
	}

	public void UpdatePlayingStatus(string isPlaying)
	{
		IsPlaying = bool.Parse(isPlaying);
		Debug.Log("MUSIC PLAYER STATUS: " + isPlaying);
		if(!bool.Parse(isPlaying))
		{
			//UIGlobalVariablesScript.Singleton.PlayPauseButton. = "playIcon";
			//UIGlobalVariablesScript.Singleton.PlayPauseButton.GetComponent<UIButton>().normalSprite = "playIcon";
		}
		else
		{
			//UIGlobalVariablesScript.Singleton.PlayPauseButton.spriteName = "pause";
			//UIGlobalVariablesScript.Singleton.PlayPauseButton.GetComponent<UIButton>().normalSprite = "pause";
		}
	}



	[DllImport("__Internal")]
	private static extern void _setVideo(string filename);

	[DllImport("__Internal")]
	private static extern int _initMediaPlayer();

	[DllImport("__Internal")]
	private static extern string _getNextSongFromList();

	[DllImport("__Internal")]
	private static extern void _playSongAtIndex(int index);

	[DllImport("__Internal")]
	private static extern float _getProgress();

	[DllImport("__Internal")]
	private static extern void _play();

	[DllImport("__Internal")]
	private static extern void _moveToPreviousSong();

	[DllImport("__Internal")]
	private static extern void _moveToNextSong();
}