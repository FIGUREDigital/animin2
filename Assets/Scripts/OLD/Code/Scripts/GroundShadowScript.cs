using UnityEngine;
using System.Collections;

public class GroundShadowScript : MonoBehaviour 
{
	//public GameObject P

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		RaycastHit hitInfo;
		if(Physics.Raycast(new Ray(this.transform.parent.position, Vector3.down), out hitInfo))
		{
			this.transform.position = hitInfo.point + new Vector3(0, 1, 0);
			//this.transform.localPosition = new Vector3(0, 0.1f, 0);
		}
	
	}
}
