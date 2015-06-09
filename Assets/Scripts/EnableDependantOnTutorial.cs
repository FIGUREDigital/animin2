using UnityEngine;
using System.Collections;

public class EnableDependantOnTutorial : MonoBehaviour {
	public string tutorialName;
	public MonoBehaviour enableThis;
	// Use this for initialization
	void OnEnable () 
	{
		Tutorial t = TutorialHandler.CurrentTutorial;
		enableThis.enabled = t != null && t.Name == tutorialName;

	}
}
