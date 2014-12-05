using UnityEngine;
using System.Collections;

public class PressEdmBoxKeyScript : MonoBehaviour 
{
	public bool SwitchOn;
	public int KeyIndex;
	public GameObject OtherOnOffIcon;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnClick()
	{
        UiPages.GetPage(Pages.CaringPage).GetComponent<CaringPageControls>().TutorialHandler.TriggerAdHocStartCond("EDM");

		//EDMBoxScript script = this.transform.parent.transform.parent.GetComponent<UIWidget>().bottomAnchor.target.GetComponent<EDMBoxScript>();//GameObject.FindObjectOfType<EDMBoxScript>();
		//script.SetKeyOn(KeyIndex, SwitchOn);

		OtherOnOffIcon.SetActive(true);
		this.gameObject.SetActive(false);
	}
}
