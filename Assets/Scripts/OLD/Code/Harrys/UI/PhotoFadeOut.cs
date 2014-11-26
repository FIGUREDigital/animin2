using UnityEngine;
using System.Collections;

public class PhotoFadeOut : MonoBehaviour {


	private bool m_Show;

	private Sprite sprite;

	[SerializeField]
	private float StartFadeoutAt = 2;

	[SerializeField]
	private float StopFadeoutAt = 4;

	private float m_Timer;

	void Start(){
		//sprite = this.GetComponent<Sprite> ();
	}

	void OnEnable(){
		m_Show = true;
		m_Timer = 0;
		//if (sprite!=null)sprite.alpha = 1;
		Debug.Log ("Enabled!");
	}
	void Update () {
		if (m_Show){
			m_Timer += Time.deltaTime;
			if (m_Timer >= StopFadeoutAt){
				m_Show = false;
			} else if (m_Timer >= StartFadeoutAt && sprite!=null){
				//sprite.alpha -= (1.0f/(StopFadeoutAt - StartFadeoutAt))*Time.deltaTime;
			}
		}
		
		this.gameObject.SetActive(m_Show);
	}
}
