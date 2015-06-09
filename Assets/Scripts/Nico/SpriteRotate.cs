using UnityEngine;
using System.Collections;

public class SpriteRotate : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.Rotate(0, 0, Time.deltaTime * -8.5f);
	}
}
