using UnityEngine;
using System.Collections;

public class NonARPosRef : MonoBehaviour
{

	[SerializeField]
	private Transform m_NonARCameraPositionReference;

	public Transform NonARCameraPositionReference {
		get {
			return m_NonARCameraPositionReference;
		}
	}
	// Use this for initialization
	void Start ()
	{

	}

	// Update is called once per frame
	void Update ()
	{

	}

}
