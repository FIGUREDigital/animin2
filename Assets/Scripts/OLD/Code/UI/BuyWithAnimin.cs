using UnityEngine;
using System.Collections;

public class BuyWithAnimin : MonoBehaviour 
{
	void OnClick()
	{
		Debug.Log("Buying with animin \n Opening Webpage");
		Application.OpenURL("http://animinme.wpengine.com/shop/");
	}
}
