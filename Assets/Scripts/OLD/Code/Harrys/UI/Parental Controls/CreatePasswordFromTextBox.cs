using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CreatePasswordFromTextBox : MonoBehaviour {
	
	InputField mInput;
	public GameObject ReportingLabel;
	
	/// <summary>
	/// Add some dummy text to the text list.
	/// </summary>
	
	void Start ()
	{
		mInput = GetComponent<InputField>();
		ReportingLabel.SetActive(false);
	}
	
	/// <summary>
	/// Submit notification is sent by Input when 'enter' is pressed or iOS/Android keyboard finalizes input.
	/// </summary>
	public void OnSubmit ()
	{
		// It's a good idea to strip out all symbols as we don't want user input to alter colors, add new lines, etc
		string text = mInput.text.Trim();
		
		if (!string.IsNullOrEmpty(text))
		{
			//ServerManager.Register(text);
			
			ReportingLabel.SetActive(true);
			//AppDataManager.SetUsername("serverProfile");
			
			
			GameObject.Find("MultiplayerObject").GetComponent<GameController>().SetMultiplayerJoinFriend();
			
			//ServerManager.Register("serverProfile");
			
			
			//mInput.value = "";
			
		}
	}
}
