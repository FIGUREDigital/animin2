using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CheatDefs : MonoBehaviour 
{
	private Text mOutputLabel;
	private string mOutputText = "";
	public string OutputText
	{
		set
		{
			mOutputText = value;
		}
	}
	// Use this for initialization
	void Start () 
	{
		if(Debug.isDebugBuild || Application.isEditor)
		{
			gameObject.SetActive(true);
			mOutputLabel = transform.FindChild("Output").GetComponent<Text>();
		}
		else
		{
			gameObject.SetActive(false);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		mOutputLabel.text = mOutputText;	
	}
}