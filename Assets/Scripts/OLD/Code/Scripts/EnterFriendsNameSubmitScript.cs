using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnterFriendsNameSubmitScript : MonoBehaviour 
{
	InputField mInput;

	public Text ReportingLabel;
	
	/// <summary>
	/// Add some dummy text to the text list.
	/// </summary>
	
	void Start ()
	{
		mInput = GetComponent<InputField>();
	}
	
	/// <summary>
	/// Submit notification is sent by Input when 'enter' is pressed or iOS/Android keyboard finalizes input.
	/// </summary>
	public void OnSubmit ()
	{
		// It's a good idea to strip out all symbols as we don't want user input to alter colors, add new lines, etc
		string text = mInput.text.Trim ();
		
		if (!string.IsNullOrEmpty(text))
		{
			ReportingLabel.gameObject.SetActive(true);
			ReportingLabel.text = "Connecting...";

			GameController.instance.SetFriendUsername(mInput.text);
			GameObject.Find("MultiplayerObject").GetComponent<GameController>().SetMultiplayerJoinFriend();
			this.gameObject.SetActive(false);
	
			mInput.text = "";
		}
	}
}
