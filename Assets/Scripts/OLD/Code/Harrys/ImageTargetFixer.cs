using UnityEngine;
using System.Collections;

public class ImageTargetFixer : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (UIGlobalVariablesScript.Singleton.ImageTarget != null) {
			if (UIGlobalVariablesScript.Singleton.ImageTarget.activeSelf){
				UnityEngine.Object.Destroy(this.gameObject);
			}
			else {
				UIGlobalVariablesScript.Singleton.ImageTarget.SetActive(true);
			}
		}
	}
}
