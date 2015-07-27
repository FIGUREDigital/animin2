using UnityEngine;
using System.Collections;

public class SyncCollider : MonoBehaviour {

	public NetScore score;

	void Start()
	{
		Physics.IgnoreCollision (GetComponent<Collider> (), score.GetComponent<Collider> ());
	}
	// Use this for initialization
	void OnEnable () 
	{
		score.gameObject.SetActive(true);
		score.textScore = GetComponentInChildren<UIText> ();
	}

	void OnDisable()
	{
		score.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () 
	{		
		if (score.gameObject.transform.parent != transform.parent) 
		{
			score.gameObject.transform.parent = transform.parent;
		}
		score.gameObject.transform.localScale = transform.localScale;
		score.gameObject.transform.localPosition = transform.localPosition;
		score.gameObject.transform.localRotation = transform.localRotation;
		

		/*if (gameObject.layer != score.gameObject.layer)
		{
			score.gameObject.layer = gameObject.layer;
		}*/
	}
}
