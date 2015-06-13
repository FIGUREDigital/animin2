using UnityEngine;
using System.Collections;

public class PlayMusic : MonoBehaviour {
	public Music.Playlists onEnable = Music.Playlists.None;
	public Music.Playlists onDisable = Music.Playlists.None;
	void OnEnable () 
	{
		if(onEnable != Music.Playlists.None)
		{
			Music.Instance.Play(onEnable);
		}
	}
		
	void OnDisable() 
	{
		if(onDisable != Music.Playlists.None)
		{
			Music.Instance.Play(onDisable);
		}
	}
}
