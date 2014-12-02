using UnityEngine;
using System.Collections;

public class PhotoFadeOut : MonoBehaviour {


	private bool m_Show;

	private UnityEngine.UI.Image sprite;

	[SerializeField]
	private float StartFadeoutAt = 2;

	[SerializeField]
	private float StopFadeoutAt = 4;

	private float m_Timer;

	void Start(){
		sprite = this.GetComponent<UnityEngine.UI.Image> ();
	}

	void OnEnable(){
		m_Show = true;
		m_Timer = 0;
		if (sprite != null) 
		{
			sprite.color = new Color (sprite.color.r, sprite.color.g, sprite.color.b, 1);
		}
		Debug.Log ("Enabled!");
	}
	void Update () {
		if (m_Show){
			m_Timer += Time.deltaTime;
			if (m_Timer >= StopFadeoutAt){
				m_Show = false;
			} else if (m_Timer >= StartFadeoutAt && sprite!=null){
				sprite.color = new Color(sprite.color.r,sprite.color.g,sprite.color.b, sprite.color.a - (1.0f/(StopFadeoutAt - StartFadeoutAt))*Time.deltaTime);
			}
		}
		
		this.gameObject.SetActive(m_Show);
	}
}
