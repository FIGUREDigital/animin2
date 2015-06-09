using UnityEngine;
using System.Collections;

public class NonARPosRef : MonoBehaviour
{
	[SerializeField]
	private Vector3 lookAtOffset;

	float currentVelocity = 0.2f;
	[SerializeField]
	float smoothTime = 0.2f;
	
	[SerializeField]
	string debugEditSmoothTime = "";

	float vOffset = 0;

	public Transform additionalYOffset;
	public Vector3 LookAtOffset
	{
		get
		{
			Vector3 r = lookAtOffset;
			r.y += vOffset;
			return r;
		}
	}
	[SerializeField]
	private Transform m_NonARCameraPositionReference;

	public Transform NonARCameraPositionReference {
		get {
			return m_NonARCameraPositionReference;
		}
	}
	// Use this for initialization
	void OnEnable ()
	{
		vOffset = 0;
	}

	void OnDisable()
	{
		vOffset = 0;
	}

	// Update is called once per frame
	void Update ()
	{
		float target = 0;
		if(additionalYOffset != null)
		{
			target = additionalYOffset.position.y - transform.position.y;
			if(target < 0)
			{
				target =0;
			}
		}
		if (debugEditSmoothTime.Length > 0) 
		{
			smoothTime = DebugSlider.GetFloat(debugEditSmoothTime);
		}
		vOffset = Mathf.SmoothDamp(vOffset, target, ref currentVelocity, smoothTime);
	}

}
