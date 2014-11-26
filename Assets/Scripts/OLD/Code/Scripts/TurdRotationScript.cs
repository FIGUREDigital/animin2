using UnityEngine;
using System.Collections;

public class TurdRotationScript : MonoBehaviour 
{
	private float Timer;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		Timer -= Time.deltaTime;
		if(Timer <= 0)
		{
			this.transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
			Timer = 0.5f;
		}
	
	}
}
