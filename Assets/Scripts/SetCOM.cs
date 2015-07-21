using UnityEngine;
using System.Collections;

public class SetCOM : MonoBehaviour {
	void Start () 
	{
		GetComponent<Rigidbody> ().centerOfMass = GetComponent<SphereCollider>().center;
	}
}
