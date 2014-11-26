using UnityEngine;
using System.Collections;

public class TutorialNextButton : MonoBehaviour {

[SerializeField]
	TutorialHandler handler;

	void OnClick(){
		handler.NextButtonPress ();
	}
}
