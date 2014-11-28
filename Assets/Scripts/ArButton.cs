using UnityEngine;
using System.Collections;

public class ArButton : MonoBehaviour {

	public void OnClick()
	{
		UiPages.Next(Pages.DemoCardPage);
	}
}
