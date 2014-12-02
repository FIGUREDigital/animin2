using UnityEngine;
using System.Collections;

public class DebugUI : MonoBehaviour {

    #if !DEBUG
	// Use this for initialization
	void Start () {
        UnityEngine.Object.Destroy(this.gameObject);
	}
    #endif
}
