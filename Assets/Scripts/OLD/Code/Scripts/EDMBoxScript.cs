using UnityEngine;
using System.Collections;



public class EDMBoxScript : MonoBehaviour 
{

	//public AudioClip[] Clips;
	/*private AudioSource[] Sources; 
	private bool isPlaying;*/
//	public bool[] KeysOn
//	{
//		get
//		{
//			return EDMMixerScript.Singleton.KeysOn;
//		}
//
//		set
//		{
//
//
//		}
//	}
	public int StartIndexInMixer;
	public void SetKeyOn(int index, bool value)
	{
		EDMMixerScript.Singleton.KeysOn[StartIndexInMixer + index] = value;
	}
	public bool GetKeyOn(int index)
	{
		return EDMMixerScript.Singleton.KeysOn[StartIndexInMixer + index];
	}

	// Use this for initialization
	void Awake () 
	{
		/*KeysOn = new bool[Clips.Length];
		Sources = new AudioSource[Clips.Length];
		for(int i=0;i<Sources.Length;++i)
		{
			GameObject newObj = new GameObject("EDM " + i.ToString());
			newObj.transform.parent = this.transform;
			AudioSource source = newObj.AddComponent<AudioSource>();

			source.loop = true;
			source.clip = Clips[i];
			Sources[i] = source;
		}*/
	}

	public void Stop()
	{
		for(int i=0;i<8;++i)
			SetKeyOn(i, false);
	}

	public void SetInterface(GameObject InterfaceUI)
	{

		GameObject sprite = InterfaceUI.transform.GetChild(0).gameObject;

		for(int i=0;i<sprite.transform.childCount;++i)
		{
			PressEdmBoxKeyScript key = sprite.transform.GetChild(i).GetComponent<PressEdmBoxKeyScript>();

			if(GetKeyOn(key.KeyIndex) == key.SwitchOn)
			{
				key.gameObject.SetActive(false);
			}
			else
			{
				key.gameObject.SetActive(true);
			}
		}
	}

	// Update is called once per frame
	void Update () 
	{
		/*if(!PlayerProfileData.ActiveProfile.Settings.AudioEnabled) 
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
			else Sources[i].volume = 0.01f;
		}*/
	}
}
