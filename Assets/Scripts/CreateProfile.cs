using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CreateProfile : MonoBehaviour {

    [SerializeField]
    private Text TextBox;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Submit()
	{
        Debug.Log ("Text Submitted");
        string text = TextBox.text.Trim();
		if (text == null || text == "")
		{
			return;
		}
		Account.Instance.UserName = text;
		bool offlineMode = true;
		if(offlineMode)
		{
			ProfilesManagementScript.Singleton.NewUserProfileAdded (text, text);
		}
		else
		{
			StartCoroutine( Account.Instance.WWWSendData( true, text, "","","", "","" ) );
		}
		UiPages.Next( Pages.AniminSelectPage);

	}
}
