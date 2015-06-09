using UnityEngine;
using System.Collections;

public class DemoScreenControls : MonoBehaviour {

	int curPage = 0;
	[SerializeField]
	GameObject[] pages;

	public void ShowDemoInstuctions()
	{
		curPage = curPage + 1;
		if (curPage >= pages.Length)
		{
			curPage = 0;
		}
		for(int i = 0; i < pages.Length; i++)
		{
			pages[i].SetActive (i == curPage);
		}
	}
}
