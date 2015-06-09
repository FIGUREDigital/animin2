using UnityEngine;
using System.Collections;
using TMPro;
public class PhotoFadeOut : MonoBehaviour {


	private bool m_Show;


	public UnityEngine.UI.Graphic[] graphics;
	public TextMeshProUGUI[] texts;

	[SerializeField]
	private float StartFadeoutAt = 2;

	[SerializeField]
	private float StopFadeoutAt = 4;

	private float m_Timer;


	void OnEnable(){
		m_Show = true;
		m_Timer = 0;
		SetAlpha(1);
		Debug.Log ("Enabled!");
	}
	void Update () {
		if (m_Show){
			m_Timer += Time.deltaTime;
			if (m_Timer >= StopFadeoutAt){
				m_Show = false;
			} else if (m_Timer >= StartFadeoutAt)
			{
				SetAlpha(graphics[0].color.a - (1.0f/(StopFadeoutAt - StartFadeoutAt))*Time.deltaTime);
			}
		}
		
		this.gameObject.SetActive(m_Show);
	}

	void SetAlpha(float a)
	{
		Color c;
		for(int i = 0; i < graphics.Length; i++)
		{
			c = graphics[i].color;
			c.a = a;
			graphics[i].color = c;
		}
		for(int i = 0; i < texts.Length; i++)
		{
			c = texts[i].color;
			c.a = a;
			texts[i].color = c;
		}
	}
}
