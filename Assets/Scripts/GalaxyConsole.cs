using UnityEngine;
using System.Collections;

public class GalaxyConsole : MonoBehaviour 
{
	void Start()
	{
		bool dev = false;
#if DEBUG
		dev = true;
#endif
		transform.GetChild (0).gameObject.SetActive (dev);
		transform.GetChild (1).gameObject.SetActive (false);
	}
	public void ToggleConsole()
	{
		int children = transform.childCount;
		for(int i = 0; i<children; i++)
		{
			GameObject go = transform.GetChild(i).gameObject;
			go.SetActive(!go.activeSelf);
		}
	}
}
