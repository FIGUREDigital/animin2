using UnityEngine;
using System.Collections;

public class Music : Phi.SingletonScene<Music> {

	static string currentMusicID = "";

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
	}

	[SerializeField]
	PlaylistDef[] playlistDefs;

	PlaylistDef currentPlayList;

	public override void Init()
	{
	}

	public void Play(Playlists list)
	{
		for(int i = 0; i < playlistDefs.Length; i++)
		{
			if (playlistDefs[i].playlist == list)
			{
				currentPlayList = playlistDefs[i];
				break;
			}
		}
		if(currentPlayList != null)
		{
			AudioController.ClearPlaylist();
			for(int i = 0; i < currentPlayList.musicInfos.Length; i++)
			{
				AudioController.EnqueueMusic(currentPlayList.musicInfos[i].musicID);
			}
			AudioController.PlayMusicPlaylist();
		}
	}
	
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
		if (Phi.UIToast.Exists () && notify && currentPlayList != null) 
		{
			MusicInfo[] musicInfos = currentPlayList.musicInfos;
			for(int i = 0; i < musicInfos.Length; i++)
			{
				if (musicInfos[i].musicID == currentMusicID)
				{
					MusicInfo mi = musicInfos[i];
					if(!string.IsNullOrEmpty(mi.title))
					{
						Phi.UIToast.Instance.Display(mi.title, mi.extraInfo, currentPlayList.notificationSprite, currentPlayList.notificationColor, null, null, null);
					}
					break;
				}
			}
		}
	}
}
