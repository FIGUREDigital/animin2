using UnityEngine;
using System.Collections;
using DG.Tweening;


public class ZoomBehaviour : MonoBehaviour 
{

	public float perspectiveZoomSpeed = 0.5f;        // The rate of change of the field of view in perspective mode.
	public float orthoZoomSpeed = 0.5f;        // The rate of change of the orthographic size in orthographic mode.

	public static bool canZoom = true;
	public static bool tapZoom = true;
	public static bool canTapZoom = false;


	
	IEnumerator ZoomIn()
	{
		GetComponent<Camera>().fieldOfView = 31.3f;
		yield return new WaitForSeconds (1.0f);
		canTapZoom = false;
	}

	IEnumerator ZoomOut()
	{
		GetComponent<Camera>().fieldOfView = 17.4f;
		yield return new WaitForSeconds (1.0f);
		canTapZoom = false;
	}

	void Update()
	{
		// If there are two touches on the device...
		if (Input.touchCount == 2 && canZoom == true)
		{
			FinishTween();
			// Store both touches.
			Touch touchZero = Input.GetTouch(0);
			Touch touchOne = Input.GetTouch(1);
			
			// Find the position in the previous frame of each touch.
			Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
			Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;
			
			// Find the magnitude of the vector (the distance) between the touches in each frame.
			float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
			float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;
			
			// Find the difference in the distances between each frame.
			float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
			
			// If the camera is orthographic...
			if (GetComponent<Camera>().orthographic)
			{
				// ... change the orthographic size based on the change in distance between the touches.
				GetComponent<Camera>().orthographicSize += deltaMagnitudeDiff * orthoZoomSpeed;
				
				// Make sure the orthographic size never drops below zero.
			//	camera.orthographicSize = Mathf.Max(camera.orthographicSize, 31.3f);

			//	camera.orthographicSize = Mathf.Min(camera.orthographicSize, 17.4f);
			}
			else
			{
				// Otherwise change the field of view based on the change in distance between the touches.
				GetComponent<Camera>().fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed;
				
				// Clamp the field of view to make sure it's between 0 and 180.
				GetComponent<Camera>().fieldOfView = Mathf.Clamp(GetComponent<Camera>().fieldOfView, 17.4f, 31.3f);
			}
		}

		if (tapZoom == true && canTapZoom == true)
		{
			FinishTween();
			tween = GetComponent<Camera>().DOFieldOfView(31.3f, 0.35f).SetEase(Ease.InOutCirc);
			canTapZoom = false;
			//GetComponent<Camera>().DO
			//StartCoroutine("ZoomIn");
		}

		if (tapZoom == false && canTapZoom == true)
		{
			FinishTween();
			tween = GetComponent<Camera>().DOFieldOfView(17.4f, 0.35f).SetEase(Ease.InOutCirc);
			canTapZoom = false;
			//StartCoroutine("ZoomOut");
		}
	}

	Tween tween;

	void FinishTween()
	{
		if(tween != null)
		{
			tween.Kill();
			tween = null;
		}
	}
}