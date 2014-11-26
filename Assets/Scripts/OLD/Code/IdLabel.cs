using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class IdLabel : MonoBehaviour {

	Text label;
	// Use this for initialization
	void Start () 
	{
		FindLabel();
	}

	void FindLabel()
	{
		label = gameObject.GetComponent<Text>();
	}
	void OnEnable()
	{
		if(label == null)
		{
			FindLabel();
		}
		label.text = "Profile ID: " + Account.Instance.UniqueID;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
