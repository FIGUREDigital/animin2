using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SetTimerOnLabelScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {


		string text = string.Empty;
		if(System.DateTime.Now.Hour < 10) text += "0";
		text += System.DateTime.Now.Hour.ToString();
		text += ":";
		if(System.DateTime.Now.Minute < 10) text += "0";
		text += System.DateTime.Now.Minute.ToString();

		GetComponent<Text>().text = text;
	
	}
}
