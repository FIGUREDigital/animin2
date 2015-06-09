using UnityEngine;
using System.Collections;

public class PlayMusicOnEnable : MonoBehaviour
{
	public string audioID;
	public bool restorePreviousOnDisable = true;
	public string previousMusic;

	void OnEnable()
	{
		previousMusic = "";
		if( !string.IsNullOrEmpty( audioID ) )
		{
			AudioObject ao = AudioController.GetCurrentMusic();
			if (ao != null)
			{
				previousMusic = ao.audioID;
			}
			Debug.Log ("Music Start "+audioID+" was playing "+previousMusic);
			AudioController.PlayMusic( audioID );
		}		
	}

	void OnDisable()
	{
		if(restorePreviousOnDisable && !string.IsNullOrEmpty(previousMusic))
		{						
			Debug.Log ("Music Restore "+previousMusic);
			AudioController.PlayMusic( previousMusic );
		}
	}
}
