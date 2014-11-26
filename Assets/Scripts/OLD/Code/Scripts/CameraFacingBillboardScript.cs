using UnityEngine;
using System.Collections;

public class CameraFacingBillboardScript : MonoBehaviour
{
	public float Alpha;
	public bool IsShowing;
	//public Camera CameraRef;

	void Update()
	{

		transform.LookAt(Camera.main.transform.position/* + CameraRef.transform.rotation * Vector3.back,
		                 CameraRef.transform.rotation * Vector3.up*/);

		//transform.LookAt(CameraRef.transform.rotation * Vector3.back,
		//                 CameraRef.transform.rotation * Vector3.up);

		/*Vector3 v = CameraRef.transform.position - transform.position;
		v.x = v.z = 0.0f;
		transform.LookAt(CameraRef.transform.position - v);*/
		//transform.localRotation = Quaternion.Euler(90, 0, 0);

		/*if(IsShowing)
		{
			Alpha += Time.deltaTime;
			if(Alpha >= 1) Alpha = 1;
		}
		else
		{
			Alpha -= Time.deltaTime;
			if(Alpha <= 0) Alpha = 0;
		}*/

		//this.renderer.material.color = new Color(1, 1, 1, Alpha);

	}
}