using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Stereo : MonoBehaviour {

	private bool m_IsLoading;
    private int m_CurrentPlaying_i;
    private bool m_IsPlaying;

	List <AudioFileInfo> m_AudioList;

	// Use this for initialization
	void Start () {

	}
	void ClearList(){
		m_AudioList = new List<AudioFileInfo>();
	}

	void OnEnable(){
		if (m_AudioList == null) {
			GetAudio();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Play(){
        if (m_IsPlaying)
        {
            NativeToolbox.MediaKit.PlayMedia("");
        } else
        {
            NativeToolbox.MediaKit.PlayMedia("");

        }
    }
    public void NextTrack(){

    }
    public void PrevTrack(){

    }





	public void GetAudio(){
		Debug.Log ("GetAudio");
	
		ClearList ();
		NativeToolbox.MediaKit.ExploreAudioFiles ();
	}

	public void AddAudio(AudioFileInfo audioInfo){
		if (m_AudioList == null)
            return;
		Debug.Log (this.GetType().ToString() + "AddAudio: title : ["+ audioInfo.title + "]; Artist : ["  + audioInfo.artist   + "]; Duration : ["  + audioInfo.duration  + "]; uri : ["  + audioInfo.uri + "]; ArtFile : [" + audioInfo.artFile);
		m_AudioList.Add (audioInfo);
	}
}
