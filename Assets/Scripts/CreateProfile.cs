using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CreateProfile : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Submit(Text input)
	{
		string text = input.text.Trim();
		if (text == null || text == "")
		{
			return;
		}
		Account.Instance.UserName = text;
		StartCoroutine( Account.Instance.WWWSendData( true, text, "","","", "","" ) );
		UiPages.LeavePage(gameObject, Pages.AniminSelectPage);

	}
}
