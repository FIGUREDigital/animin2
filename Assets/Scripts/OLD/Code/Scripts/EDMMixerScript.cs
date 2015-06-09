using UnityEngine;
using System.Collections;

public class EDMMixerScript : MonoBehaviour
{
	public static EDMMixerScript Singleton;

	public AudioClip[] Clips;
	private AudioSource[] Sources; 
	private bool isPlaying;
	public bool[] KeysOn;
	
	// Use this for initialization
	void Awake () 
	{
		Singleton = this;

		KeysOn = new bool[Clips.Length];
		Sources = new AudioSource[Clips.Length];
		for(int i=0;i<Sources.Length;++i)
		{
			GameObject newObj = new GameObject("EDM " + i.ToString());
			newObj.transform.parent = this.transform;
			AudioSource source = newObj.AddComponent<AudioSource>();
			
			source.loop = true;
			source.clip = Clips[i];
			Sources[i] = source;
		}
	}
	
	void Enable()
	{
		
	}
	
	void Disable()
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(!ProfilesManagementScript.Instance.CurrentProfile.Settings.AudioEnabled) 
		{
			if(isPlaying)
			{
				for(int i=0;i<Sources.Length;++i)
					Sources[i].Stop();
			}
			
			isPlaying = false;
		}
		else
		{
			if(!isPlaying)
			{
				for(int i=0;i<Sources.Length;++i)
					Sources[i].Play();
			}
			
			isPlaying = true;
		}
		
		for(int i=0;i<Sources.Length;++i)
		{
			if(KeysOn[i]) Sources[i].volume = 1;
			else Sources[i].volume = 0.0f;
		}
	}
}
