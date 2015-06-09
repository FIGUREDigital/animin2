using UnityEngine;
using System.Collections;
using UnityEngine.UI;
[RequireComponent(typeof(Canvas))]
public class UISetCanvasCamera : MonoBehaviour {

    public string cameraPrefabName = "UICamera";
	// Use this for initialization
	void Awake () {
        Canvas c = GetComponent<Canvas>();
        c.worldCamera = Phi.SingletonPrefab.instances[cameraPrefabName].GetComponent<Camera>();
        Vector3 pos = c.worldCamera.transform.position;
        pos.z = 0;
        c.transform.position = pos;
	}
}
