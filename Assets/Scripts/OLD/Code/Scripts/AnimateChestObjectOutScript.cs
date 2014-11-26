using UnityEngine;
using System.Collections;

public class AnimateChestObjectOutScript : MonoBehaviour 
{

	public Vector3 Destination;
	private int Sign = 1;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		Vector3 localPosition = this.transform.localPosition;

		localPosition.x = Mathf.Lerp(localPosition.x, Destination.x, Time.deltaTime * 0.2f); 
		localPosition.z = Mathf.Lerp(localPosition.z, Destination.z, Time.deltaTime * 0.2f);

		localPosition.y += Time.deltaTime * 3 * Sign;

		if(localPosition.y >= 0.4f)
			Sign = -1;

		if(localPosition.y <= 0.0f)
		{
			localPosition.y = 0;
			Destroy(this);
		}

		this.transform.localPosition = localPosition;
	
	}
}
