using UnityEngine;
using System.Collections;

public class AddARCameraToGlobalVarScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
				UIGlobalVariablesScript.Singleton.ARCamera = this.gameObject;
				UIGlobalVariablesScript.Singleton.DragableUI3DObject = this.GetComponentInChildren<CameraModelScript> ().gameObject;
	}
}
