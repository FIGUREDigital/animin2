using UnityEngine;
using System.Collections;

public class EndFlurryLoadingTimer : MonoBehaviour {

	// Use this for initialization
	void Start () {
		FlurryLogger.Instance.EndMainScreenTimer ();
	}
}
