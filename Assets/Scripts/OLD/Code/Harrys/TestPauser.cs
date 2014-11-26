using UnityEngine;
using System.Collections;

public class TestPauser : MonoBehaviour {
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        #if UNITY_EDITOR
		if (Input.GetKeyDown (KeyCode.P)) {
			UnityEditor.EditorApplication.isPaused = true;
		}
        #endif
	}
}