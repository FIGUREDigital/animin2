using UnityEngine;
using System.Collections;

public class AchievementMover : MonoBehaviour 
{
	private Vector3 startPos;
	private RectTransform rect;
	private float speed = 20f;
	void OnEnable()
	{
		rect = GetComponent<RectTransform> ();
		startPos = rect.transform.position;
	}
	void OnDisable()
	{
		rect.transform.position = startPos;
	}
	void Update()
	{
		rect.transform.position = new Vector3 (rect.transform.position.x, rect.transform.position.y + (Time.deltaTime * speed), rect.transform.position.z);
	}
}
