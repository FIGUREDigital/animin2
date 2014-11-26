using UnityEngine;

using System;
using System.Collections;
using UnityEngine.UI;

public class CurrentTime : MonoBehaviour {

	Text m_CurrentLabel;
	// Use this for initialization
	void Start () {
		m_CurrentLabel = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		m_CurrentLabel.text = DateTime.Now.ToString ("hh:mm tt");
	}
}
