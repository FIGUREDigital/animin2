using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Phi;
#if UNITY_IOS
using Prime31;

#endif


public class BoomBoxUI : MonoBehaviour {
	
	public Button prevTrack;
	public Button nextTrack;
	public Button prevPlaylist;
	public Button nextPlaylist;
	public Button play;
	public Button notPlaying;
	public UIText textArtist;
	public UIText textTrack;
	public UIText textPlayList;
	public Sprite notificationSprite;
	public UnityEngine.UI.Slider volume;
	public Color notificationColor;

	int curPlaylist = 0;
	float currentVolume = 1;

	class PlayList
	{
		public string id;
		public string name;
		public PlayList(string name, string id)
		{
			this.name = name;
			this.id = id;
		}
	}

	List<PlayList> playLists = new List<PlayList>();

	public static bool IsPlaying
	{
		get
		{
#if UNITY_IOS
			return MediaPlayerBinding.isPlaying();
#else
			return false;
#endif
		}
	}

	public static void DoStop()
	{		
		Debug.Log ("Stop");
		#if UNITY_IOS
		MediaPlayerBinding.stop();
		#else
		#endif
	}

	public void OnStop()
	{
		Debug.Log ("OnStop");
		DoStop ();
		RefreshTrackUI();
	}

	public void OnEnable()
	{
#if UNITY_IOS
		currentVolume = MediaPlayerBinding.getVolume();
#endif
		if(volume)
		{
			volume.value = currentVolume;
		}
		nextUpdate = 0;
	}

	public void OnPause()
	{
		#if UNITY_IOS
		MediaPlayerBinding.pause();
		#else
		#endif
		RefreshTrackUI();
	}

	public void OnPlay()
	{		
		#if UNITY_IOS
		Debug.Log ("OnPlay");
		if (!MediaPlayerBinding.isPlaying())
		{			
			Debug.Log ("OnPlay: Not playing");
			if (curPlaylist == 0)
			{
				Debug.Log ("showMediaPicker");
				MediaPlayerBinding.showMediaPicker();
			}
			else
			{
				Debug.Log ("playPlaylist "+curPlaylist);
				Debug.Log ("playPlaylist id = "+playLists[curPlaylist].id);
				MediaPlayerBinding.playPlaylist(playLists[curPlaylist].id);
			}
		}
		#else
		#endif
		RefreshTrackUI();
		// Switch off the radio
		if (Music.Instance.CurentList != Music.Playlists.Caring)
		{
			Music.Instance.Play(Music.Playlists.Caring, true);
		}
	}

	public void OnPlaylist()
	{		
		#if UNITY_IOS
		Debug.Log ("OnPlayList");
		OnStop();
		curPlaylist = 0;
		RefreshTrackUI();
		RefreshPlayListUI();
		MediaPlayerBinding.showMediaPicker();
		#endif
	}

	public void OnNext()
	{		
		Debug.Log ("OnNext");
		#if UNITY_IOS
		MediaPlayerBinding.skipToNextItem();
		#endif
		RefreshTrackUI();
	}
	
	public void OnPrev()
	{		
		Debug.Log ("OnPrev");
		#if UNITY_IOS
		MediaPlayerBinding.skipToPreviousItem();
		#endif
		RefreshTrackUI();
	}

	public void OnNextPlayList(int dir)
	{
		Debug.Log ("OnNextPlayList "+dir);
		nextUpdate = 5;	// Avoid some flicker
		int next = curPlaylist + dir;
		if (next < 0)
		{
			next = 0;
		}
		if (next >= playLists.Count)
		{
			next = playLists.Count - 1;
		}
		Debug.Log ("curPlaylist = "+curPlaylist+" next = "+next);
		if(next != curPlaylist)
		{
			curPlaylist = next;
			
			Debug.Log ("AutoPlay new playlist");
			if (IsPlaying)
			{
				if(curPlaylist != 0)
				{
					Debug.Log ("AutoPlay new playlist");
	#if UNITY_IOS
					
					MediaPlayerBinding.playPlaylist(playLists[curPlaylist].id);
	#endif
					
				}
				else
				{
					OnStop();
				}
			}
			RefreshPlayListUI();
		}

	}

	// Use this for initialization
	public void Start () 
	{
		playLists.Add (new PlayList("Custom Playlist",""));		
		#if UNITY_IOS
		Debug.Log ("Calling MediaPlayerBinding.getPlaylists");
		List<MediaPlayerPlaylist> osLists = MediaPlayerBinding.getPlaylists();
		Debug.Log ("Returned "+osLists.Count);
		foreach(MediaPlayerPlaylist list in osLists)
		{			
			playLists.Add (new PlayList(list.title, list.playlistId));
		}
		Debug.Log ("playLists.Count "+playLists.Count);
		MediaPlayerManager.songChanged += SongChanged;
		#endif
		RefreshPlayListUI();		
		RefreshTrackUI();
	}

	public void RefreshPlayListUI()
	{
		Debug.Log ("RefreshPlayListUI "+curPlaylist);
		prevPlaylist.gameObject.SetActive (curPlaylist > 0);
		nextPlaylist.gameObject.SetActive (curPlaylist < playLists.Count-1);
		textPlayList.Text = playLists[curPlaylist].name;
	}
	
	// Update is called once per frame
	public void RefreshTrackUI()
	{
		bool isPlaying = IsPlaying;
		play.gameObject.SetActive (!isPlaying);		
		notPlaying.gameObject.SetActive (isPlaying);
		prevTrack.gameObject.SetActive (isPlaying);
		nextTrack.gameObject.SetActive (isPlaying);
		
		string title;
		string artist;
		GetTrackInfo(out title, out artist);			
		textTrack.Text = title; 
		textArtist.Text = artist; 
		
		if(volume)
		{			
			#if UNITY_IOS
			volume.value = MediaPlayerBinding.getVolume();
			#endif
		}
    }

	void SongChanged()
	{
		Debug.Log ("SongChanged");
		string title;
		string artist;
		GetTrackInfo(out title, out artist);
		RefreshTrackUI();
		if(Music.Exists() && !gameObject.activeInHierarchy)
		{
			Music.Instance.NewSong(title, artist, notificationSprite, notificationColor);
		}
	}

	void GetTrackInfo(out string title, out string artist)
	{	
		title = "";
		artist = "";
		#if UNITY_IOS
		MediaPlayerTrack track = MediaPlayerBinding.getCurrentTrack();
		title = track.title;
		artist = track.artist;
		#endif            
	}

	public void OnVolumeChange()
	{
		
		if(volume)
		{
			float newV = volume.value;
			if(newV != currentVolume) 
			{
	#if UNITY_IOS
				MediaPlayerBinding.setVolume(newV);
	#endif
				currentVolume = newV;
			}
		}
	}


	float nextUpdate = 0;
	public void Update()
	{
		if(volume)
		{
			#if UNITY_IOS
			float v = MediaPlayerBinding.getVolume();
			if(v != currentVolume)
			{
				currentVolume = v;
				volume.value = v;
			}
			#endif
		}
		nextUpdate-= Time.deltaTime;
		if(nextUpdate < 0)
		{
			nextUpdate = 1;
			RefreshTrackUI();
		}
	}
}
