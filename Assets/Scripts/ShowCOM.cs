using UnityEngine;
using System.Collections;

public class ShowCOM : MonoBehaviour {

	public Rigidbody body;
	
	// Update is called once per frame
	void Update () {
		transform.localPosition = body.centerOfMass;
	}
}
