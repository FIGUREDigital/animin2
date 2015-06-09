using UnityEngine;
using System.Collections;
using TMPro;
public class DialogPage : MonoBehaviour {

	[SerializeField]
	TextMeshProUGUI message;
	static DialogPage instance;
	static string curMessage;

	void Awake()
	{
		instance = this;
	}

	void OnDestroy()
	{
		if (instance == this)
		{
			instance = null;
		}
	}

	public static void SetMessage(string m)
	{
		curMessage = m;
		if (instance != null)
		{
			instance.DoSetMessage();
		}
	}

	void DoSetMessage () 
	{
		message.text = curMessage;
	}

	void OnEnable ()
	{
		DoSetMessage();
	}
}
