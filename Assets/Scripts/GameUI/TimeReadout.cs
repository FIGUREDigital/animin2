using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class TimeReadout : MonoBehaviour {

    Text time;
	// Use this for initialization
	void Start () {
        time = this.GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        time.text = DateTime.Now.ToString ("hh:mm tt");
	}
}
