using UnityEngine;
using System.Collections;

public class PlayMusic : MonoBehaviour {

    public string musicID;
	// Use this for initialization
	void Start () {
        AudioController.PlayMusic(musicID);
	}
}
