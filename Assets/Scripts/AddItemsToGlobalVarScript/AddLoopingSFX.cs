using UnityEngine;
using System.Collections;

public class AddLoopingSFX : MonoBehaviour {

	// Use this for initialization
		void Update () {
				Debug.Log ("SoundEngine : [" + UIGlobalVariablesScript.Singleton + "];");
				if (UIGlobalVariablesScript.Singleton != null) {
						Debug.Log ("SoundEngine : [" + UIGlobalVariablesScript.Singleton.SoundEngine + "];");
						Debug.Log ("AudioSource : [" + this.GetComponent<AudioSource> () + "];");
						UIGlobalVariablesScript.Singleton.UIRoot.GetComponent<SoundEngineScript> ().SoundFxLooper1 = this.GetComponent<AudioSource> ();
						Destroy (this);
				}
	}
}
