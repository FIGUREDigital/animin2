using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ZefCount : MonoBehaviour 
{
	void OnEnable()
	{
		GetComponent<Text>().text = ProfilesManagementScript.Singleton.CurrentAnimin.ZefTokens.ToString();
	}
}
