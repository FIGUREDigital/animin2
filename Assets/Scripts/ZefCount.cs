using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class ZefCount : MonoBehaviour 
{
	int curValue = -1;
	void OnEnable()
	{
		Update();
	}

	void Update()
	{
		if (curValue != ProfilesManagementScript.Instance.CurrentAnimin.ZefTokens)
		{
			curValue = ProfilesManagementScript.Instance.CurrentAnimin.ZefTokens;
			GetComponent<TextMeshProUGUI>().text = curValue.ToString();
		}
	}
}
