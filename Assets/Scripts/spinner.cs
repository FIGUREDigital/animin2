using UnityEngine;
using System.Collections;

public class spinner : MonoBehaviour 
{
	float speed = 200.0f;
	RectTransform trans;
	// Use this for initialization
	void Start () {
		trans = GetComponent<RectTransform> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		trans.Rotate (new Vector3 (0, 0, -Time.deltaTime * speed));
	}
}
