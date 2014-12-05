using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NonARSceneReadout : MonoBehaviour {
	
	Text m_label;
	// Use this for initialization
	void Start () {
		if (!Debug.isDebugBuild) {
			gameObject.SetActive(false);
		} else {
			m_label = this.GetComponent<Text> ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (gameObject.activeInHierarchy) {
			bool isActive = UIGlobalVariablesScript.Singleton.NonARWorldRef.activeInHierarchy;
            m_label.text = ("Tutorial Playing : [" + (UiPages.GetPage(Pages.CaringPage).GetComponent<CaringPageControls>().TutorialHandler.IsPlaying) + "]");
		}
	}
}
