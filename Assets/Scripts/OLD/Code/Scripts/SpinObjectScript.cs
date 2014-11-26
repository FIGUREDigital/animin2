using UnityEngine;
using System.Collections;

public class SpinObjectScript : MonoBehaviour
{

	float rotationAngle;
	float rotaitonSpeed;
	float maxRotationSpeed;

	// Use this for initialization
	void Start () 
	{
		//rotationAngle = Random.Range(0, 360) + this.transform.rotation.eulerAngles.y;
		maxRotationSpeed = Random.Range(45, 55);
		rotaitonSpeed = 0;
	}

	public void SetRotationAngle()
	{
		rotationAngle = /*Random.Range(0, 360) +*/ this.transform.rotation.eulerAngles.y;
	}
	
	// Update is called once per frame
	void Update () 
	{
		rotaitonSpeed += Time.deltaTime * 20;
		if(rotaitonSpeed >= maxRotationSpeed)
			rotaitonSpeed = maxRotationSpeed;

		rotationAngle += Time.deltaTime * rotaitonSpeed;

		transform.rotation = Quaternion.Euler(0, rotationAngle, 0);
	
	}
}
