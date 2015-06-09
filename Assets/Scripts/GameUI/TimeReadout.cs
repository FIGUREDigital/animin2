using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using TMPro;

public class TimeReadout : MonoBehaviour {

    public TextMeshProUGUI time;

	void OnEnable()
	{
		DoUpdate();
	}
	
	// Update is called once per frame
	void Update () 
	{
		DoUpdate();
	}

	void DoUpdate()
	{
		time.text = DateTime.Now.ToString ("hh:mm tt");
	}
}
