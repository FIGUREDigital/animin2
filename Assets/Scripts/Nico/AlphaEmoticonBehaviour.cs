using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AlphaEmoticonBehaviour : MonoBehaviour 
{
	public EmoticonBehaviour emote;
	public float fadeDuration = 0.5f;
	public float stayDuration = 2.0f;
	
	public static bool canFade;
	float fading = 0;

	// Use this for initialization
	void Start () 
	{		
		emote.UpdateAlpha(0);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (fading == 0)
		{
			if (canFade)
			{
				// Start a fade;
				fading = fadeDuration * 2.0f + stayDuration;
				emote.UpdateIcons();
			}
		}
		else
		{
			fading -= Time.deltaTime;
			if (fading < 0)
			{
				fading = 0;
				canFade = false;
			}
			float total = (fadeDuration * 2.0f + stayDuration) * 0.5f;
			float r = Mathf.Abs (total - fading);
			float a = (total - r)/fadeDuration;
			if (a > 1)
			{
				a = 1;
			}
			emote.UpdateAlpha(a);
		}
	}
}
