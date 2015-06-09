using UnityEngine;
using System.Collections;
using System.Collections.Generic;
// Add this component to a game object and the game object and all of it's children will
// remain loaded between scene changes
public class UtilsKeepLoaded : MonoBehaviour 
{
	// Use this for initialization
	void Awake () 
	{
		GameObject.DontDestroyOnLoad(gameObject);
	}
	
	public void Unload()
	{
		GameObject.Destroy(gameObject);
	}

}
