using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Stereo : MonoBehaviour {

	private bool m_IsLoading;
    private int m_CurrentPlaying_i;
    private bool m_IsPlaying;

	List <AudioFileInfo> m_AudioList;

    [SerializeField]
    private Text m_ArtistTextBox, m_SongTextBox; 

    [SerializeField]
    private GameObject m_MainScreen, m_Error;

	// Use this for initialization
	void Start () {

	}
	void ClearList(){
		m_AudioList = new List<AudioFileInfo>();
	}

	void OnEnable(){
		if (m_AudioList == null)
        {
            GetAudio();
        }

        Debug.Log("Argh : [" + (m_AudioList!=null) + "|" + m_AudioList.Count + "];");

        if (m_AudioList != null && m_AudioList.Count > 0){
            Debug.Log("Showing Controls!");
            m_MainScreen.SetActive(true);
            m_Error.SetActive(false);
        }
        else
        {
            Debug.Log("Hiding Controls...");
            m_MainScreen.SetActive(false);
            m_Error.SetActive(true);
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (m_AudioList == null || m_AudioList.Count <= 0)
            return;
        m_ArtistTextBox.text = m_AudioList [m_CurrentPlaying_i].artist;
        m_SongTextBox.text = m_AudioList [m_CurrentPlaying_i].title;
	}
    private void UpdateListing(){

    }
    public void Play(){
        if (m_IsPlaying)
        {
            NativeToolbox.MediaKit.PlayMedia("");
        } else
        {
            NativeToolbox.MediaKit.PlayMedia(m_AudioList[m_CurrentPlaying_i].uri);
        }
    }
    public void NextTrack(){
        if (m_CurrentPlaying_i >= m_AudioList.Count - 1)
            m_CurrentPlaying_i = 0;
        else
            m_CurrentPlaying_i += 1;
        UpdateListing();
    }
    public void PrevTrack(){
        if (m_CurrentPlaying_i <= 0)
            m_CurrentPlaying_i = m_AudioList.Count - 1;
        else
            m_CurrentPlaying_i -= 1;
        UpdateListing();
    }




	public void GetAudio(){
		Debug.Log ("GetAudio");
	
		ClearList ();
		NativeToolbox.MediaKit.ExploreAudioFiles ();
	}

	public void AddAudio(AudioFileInfo audioInfo){
		if (m_AudioList == null)
            return;
		Debug.Log ("AddAudio:\n" +
                   " -Title : ["+ audioInfo.title + "];\n" +
                   " -Artist : ["  + audioInfo.artist   + "];\n" +
                   " -Duration : ["  + audioInfo.duration  + "];\n" +
                   " -uri : ["  + audioInfo.uri + "];\n" +
                   " -ArtFile : [" + audioInfo.artFile);
		m_AudioList.Add (audioInfo);
	}
}
