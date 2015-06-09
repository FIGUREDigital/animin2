using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public class DebugSlider : MonoBehaviour {

	[SerializeField]
	UIText text;

	[SerializeField]
	float defaultValue;

	[SerializeField]
	new string name;

	[SerializeField]
	UnityEngine.UI.Slider slider;

	public static float GetFloat(string name)
	{
		return PlayerPrefs.GetFloat("DebugSlider"+name, 0);
	}

	// Use this for initialization
	void Start () 
	{
		slider.value = PlayerPrefs.GetFloat("DebugSlider"+name, defaultValue);
		UpdateText ();
		//slider.
	}
	
	// Update is called once per frame
	public void OnChange (float v)
	{
		PlayerPrefs.SetFloat("DebugSlider"+name, slider.value);
		UpdateText ();
	}

	void UpdateText()
	{
		if (text) {
			text.Text = slider.value.ToString ("n2");
		}
	}
}
