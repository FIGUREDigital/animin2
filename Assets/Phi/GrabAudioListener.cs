using UnityEngine;
using System.Collections;

public class GrabAudioListener : MonoBehaviour {

    static AudioListener listener;
	bool isListener = false;
	// Use this for initialization
	void Awake () 
    {
        AudioListener al = GetComponent<AudioListener>();
	    if (al != null)
        {
			isListener = true;
			listener = al;
		}
	}

	void Update()
	{
		if (!isListener && listener != null)
		{
			listener.transform.position = transform.position;
			listener.transform.rotation = transform.rotation;
		}
	}	

	void OnDestroy()
	{
		if (!isListener && listener != null)
		{
			listener.transform.position = Vector3.zero;;
			listener.transform.rotation = Quaternion.identity;
		}
	}
}
