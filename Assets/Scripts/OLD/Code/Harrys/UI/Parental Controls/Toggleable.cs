using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Toggleable : MonoBehaviour {

	public delegate void Toggle(bool on);

	private Toggle m_Toggle;
	/*
	[SerializeField]
	private string OnSpriteName = "alarm_active_on";
	[SerializeField]
	private string OffSpriteName = "alarm_active_off";
*/
	private bool m_On;
	//private Button m_Button;

	void Start(){
		m_On = false;
		//m_Button = this.GetComponent<Button> ();
	}

	void OnClick(){

		m_On = !m_On;

//		string newSprite = (m_On ? OnSpriteName : OffSpriteName);


		if (m_Toggle != null) m_Toggle (m_On);
	}
	public void SetToggle(Toggle toggle){
		m_Toggle = toggle;
	}
}
