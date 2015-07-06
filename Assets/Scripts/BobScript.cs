using UnityEngine;
using System.Collections;

public class BobScript : MonoBehaviour {
	public float bobHeight1 = 0f;
	public float bobHeight2 = 0.5f;
	public float bobSpeed = 1;
	public GameObject spin;
	public float timeOrigin = 0;
	float baseY; 
	// Use this for initialization
	void Start () 
	{
		baseY = transform.localPosition.y;
		if (spin == null) 
		{
			spin = transform.parent.gameObject;
		}
	}

	void OnEnable()
	{
		timeOrigin = Time.time;
	}
	
	// Update is called once per frame
	void Update () 
	{
		SpinObjectScript spinScript = spin.GetComponent<SpinObjectScript> ();
		float offset = 0;
		if (spinScript != null && spinScript.enabled)
		{
			offset = (Mathf.Sin ((Time.time - timeOrigin) * Mathf.PI * bobSpeed) + 1) * 0.5f;
			offset = Mathf.Lerp(bobHeight1, bobHeight2, offset);
		}
		Vector3 pos = transform.localPosition;
		pos.y = baseY + offset;
		transform.localPosition = pos;
	}
}
