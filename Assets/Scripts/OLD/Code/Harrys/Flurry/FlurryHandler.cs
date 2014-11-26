using UnityEngine;
using System.Collections;

public class FlurryHandler : MonoBehaviour {

	void Start () {
		FlurryLogger.Instance.Init ();
	}

	void OnApplicationPause(){
		FlurryLogger.Instance.EndSession ();
    }
    void OnApplicationQuit(){
        FlurryLogger.Instance.EndSession ();
    }
    void OnApplicationResumer(){
        FlurryLogger.Instance.StartSession();
    }
}
