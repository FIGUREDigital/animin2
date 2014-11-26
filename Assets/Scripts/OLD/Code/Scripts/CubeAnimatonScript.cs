using UnityEngine;
using System.Collections;

public class CubeAnimatonScript : MonoBehaviour 
{
	public Vector3 ValueNext;
	public float Delay;
	public Vector3 ResetPosition;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		Delay -= Time.deltaTime;

		if(Delay <= 0)
		{
			this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, ValueNext, Time.deltaTime * 9);

			if(this.transform.localPosition == ValueNext)
			{
				this.enabled = false;
			}
			else if(Vector3.Distance(this.transform.localPosition, ValueNext) <= 0.1f)
			{
				this.transform.localPosition = ValueNext;
				//Destroy(this);


			}
		}
	}
}
