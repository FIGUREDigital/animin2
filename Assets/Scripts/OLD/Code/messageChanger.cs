using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class messageChanger : MonoBehaviour {


	// Use this for initialization
	void Start () 
	{
		string platform;
#if UNITY_IOS
		platform = "iTunes";
#elif UNITY_ANDROID
		platform = "Play";
#else
		platform = "";
#endif
		string body = "We're having problems connecting to the " + platform + " store at the moment, please try again later or turn on wi-fi.";
		gameObject.GetComponent<Text>().text = body;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
