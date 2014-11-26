using UnityEngine;
using System.Collections;

public class MusicScript : MonoBehaviour 
{
	// Use this for initialization
	void Start () {
		LooperPlaying=true;
	}
	
	// Update is called once per frame
	void Update () 
	{
        if(!ProfilesManagementScript.Singleton.CurrentProfile.Settings.AudioEnabled) 
		{
			if(this.GetComponent<AudioSource>().isPlaying)
				this.GetComponent<AudioSource>().Stop();
		}
		else
		{
			if(LooperPlaying && !this.GetComponent<AudioSource>().isPlaying)
				this.GetComponent<AudioSource>().Play();
		}
	}

	private bool LooperPlaying;

	public void Stop()
	{
		this.GetComponent<AudioSource>().Stop(); 
		LooperPlaying = false;
	}
}
