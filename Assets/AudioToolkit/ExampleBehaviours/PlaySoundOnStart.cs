using UnityEngine;
using System.Collections;

public class PlaySoundOnStart : MonoBehaviour
{
    public string audioID;
    public bool onStart = true;
    public bool onEnable = false;
    public bool onDisable = false;

    public bool atTransform = false;

    void Start()
    {
        if (onStart) Play();
    }
    void OnEnable()
    {
        if (onEnable) Play();
    }
    void OnDisable()
    {
        if (onDisable) Play();
    }
    void Play()
    {		
        if( !string.IsNullOrEmpty( audioID ) )
		{
            if (atTransform)
            {
                AudioController.Play(audioID, transform);
            }
            else
            {
                AudioController.Play(audioID);
            }
		}		
	}
}
