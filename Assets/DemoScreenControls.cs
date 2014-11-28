using UnityEngine;
using System.Collections;

public class DemoScreenControls : MonoBehaviour {

	[SerializeField]
	private GameObject InstuctionsOverlay;

	public void ShowDemoInstuctions()
	{
		if (InstuctionsOverlay != null) 
		{
			InstuctionsOverlay.SetActive(!InstuctionsOverlay.activeSelf);
		}
	}
}
