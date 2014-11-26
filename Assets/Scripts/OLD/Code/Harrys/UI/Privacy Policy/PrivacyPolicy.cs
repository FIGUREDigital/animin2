using UnityEngine;
using System.Collections;

public class PrivacyPolicy : MonoBehaviour 
{
	[SerializeField]
	private PrvPlcyOKBttn correctButton;
	private GameObject prevScreen;
	
	public void Open(GameObject prev)
	{
		prevScreen = prev;
		Debug.Log ("PrevScreen : [" + prevScreen.name + "];");
		prevScreen.SetActive(false);
		this.gameObject.SetActive(true);
	}
	
	public void Okay()
	{
		Destroy(this.gameObject, 0);
		prevScreen.SetActive(true);
	}
}
