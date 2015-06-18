using UnityEngine;
using System.Collections;
using Phi;

public class Music : SingletonScene<Music> {

	static string currentMusicID = "";

	UIToast.DisplayData toast = null;

	[System.Serializable]
	public class MusicInfo
	{
		public string musicID;
		public string title;
		public string extraInfo;
	}

	public enum Playlists
	{
		None,
		MainMenu,
		Caring,
		GunGame,
		CubeGame,
		Radio1,
		Radio2
	}

	[System.Serializable]
	public class PlaylistDef
	{
		public Playlists playlist;
		public	MusicInfo[] musicInfos;
		public string audioIDToPlayWhenStarts;	// When set and you start playing this playlist this audioID will also be triggered.
		public Color notificationColor;
		public Sprite notificationSprite;
		public bool isMusicPlayer = false;	// When true this should play instead of caring or minigame music until it is switched off.
		public bool canTransitionIfPlayingMusicPlayer = false;
	}

	[SerializeField]
	PlaylistDef[] playlistDefs;

	PlaylistDef currentPlayList;

	public Playlists CurentList
	{
		get
		{
			return currentPlayList != null ? currentPlayList.playlist : Playlists.None;
		}
	}


	public override void Init()
	{
	}

	public void Play(Playlists list, bool force = false)
	{
		PlaylistDef newPList = null;
		for(int i = 0; i < playlistDefs.Length; i++)
		{
			if (playlistDefs[i].playlist == list)
			{
				newPList = playlistDefs[i];
				break;
			}
		}
		if(newPList == null) return;
		if(!force && currentPlayList != null && currentPlayList.isMusicPlayer && !newPList.canTransitionIfPlayingMusicPlayer) return;
		currentPlayList = newPList;
		if(currentPlayList != null)
		{
			AudioController.ClearPlaylist();
			for(int i = 0; i < currentPlayList.musicInfos.Length; i++)
			{
				AudioController.EnqueueMusic(currentPlayList.musicInfos[i].musicID);
			}
			if(!BoomBoxUI.IsPlaying)
			{
				AudioController.PlayMusicPlaylist();
			}
		}
	}

	string previousNotificationSound = "";

	bool deviceMusicPlaying = false;

	// Update is called once per frame
	void Update ()
	{
		AudioObject ao = AudioController.GetCurrentMusic ();
		bool notify = false;
		if (ao != null && ao.IsPlaying()) {
			if (currentMusicID != ao.audioID) {
				currentMusicID = ao.audioID;
				notify = currentMusicID.Length > 0;
			}
		}
		else if (currentMusicID.Length > 0)
		{
			currentMusicID = "";
		}

		if(BoomBoxUI.IsPlaying != deviceMusicPlaying)
		{
			deviceMusicPlaying = !deviceMusicPlaying;
			if(deviceMusicPlaying)
			{
				AudioController.StopMusic(0.5f);
			}
			else
			{
				// Start curPlaylist!
				notify = true;
				AudioController.PlayMusicPlaylist();
			}
		}

		if (Phi.UIToast.Exists () && notify && currentPlayList != null && !deviceMusicPlaying) 
		{
			if(toast != null)
			{
				UIToast.Instance.Remove(toast);
				toast = null;
			}
			MusicInfo[] musicInfos = currentPlayList.musicInfos;
			if (previousNotificationSound.Length > 0)
			{
				AudioController.Stop (previousNotificationSound, 0.5f);
			}
			for(int i = 0; i < musicInfos.Length; i++)
			{
				if (musicInfos[i].musicID == currentMusicID)
				{
					MusicInfo mi = musicInfos[i];
					NewSong(mi.title, mi.extraInfo, currentPlayList.notificationSprite, currentPlayList.notificationColor, currentPlayList.audioIDToPlayWhenStarts);
					previousNotificationSound = currentPlayList.audioIDToPlayWhenStarts;
					break;
				}
			}
		}
	}

	public void NewSong(string title, string artist, Sprite sprite, Color col, string audioID = "")
	{		
		if(string.IsNullOrEmpty(title)) return;

		if(toast != null)
		{
			UIToast.Instance.Remove(toast);
            toast = null;
        }		
		toast = Phi.UIToast.Instance.Display(title, artist, sprite, col, audioID);
    }
}
