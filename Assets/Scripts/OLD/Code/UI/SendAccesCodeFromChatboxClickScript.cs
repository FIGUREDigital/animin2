using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SendAccesCodeFromChatboxClickScript : MonoBehaviour {

	InputField mInput;
	public GameObject ReportingLabel;
	
	/// <summary>
	/// Add some dummy text to the text list.
	/// </summary>
	
	void Start ()
	{
		mInput = GetComponent<InputField>();
	}
	void OnEnable()
	{
		ReportingLabel.SetActive(false);
		if(mInput != null)
		{
			mInput.enabled = true;
		}

	}
	
	/// <summary>
	/// Submit notification is sent by Input when 'enter' is pressed or iOS/Android keyboard finalizes input.
	/// </summary>
	public void OnSubmit ()
	{

		// It's a good idea to strip out all symbols as we don't want user input to alter colors, add new lines, etc
		string text = mInput.text.Trim();
        Debug.Log("Submitting code..." + text);
		
		UiPages.Next(Pages.LoadingPage);
		if (!string.IsNullOrEmpty(text))
		{
            Debug.Log("Code is not empty, continuing...");
            ProfilesManagementScript.Instance.CheckCharacterCodeValidity(text);

			//ServerManager.Register(text);
			mInput.enabled = false;
			ReportingLabel.SetActive(true);
			//AppDataManager.SetUsername("serverProfile");
			
			
			//GameObject.Find("MultiplayerObject").GetComponent<GameController>().SetMultiplayerJoinFriend();
			
			//ServerManager.Register("serverProfile");
			
			
			//mInput.value = "";

			
		}

	}
}