using UnityEngine;
using System.Collections;

public class CameraChecker : MonoBehaviour {

	private QCARBehaviour m_script;

	// Use this for initialization
	void Start () {
		m_script = this.GetComponent<QCARBehaviour> ();
	}
	
	// Update is called once per frame
	void Update () {
		m_script.enabled = (WebCamTexture.devices.Length != 0);
	}
}
