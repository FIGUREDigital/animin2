using UnityEngine;
using System.Collections;
[RequireComponent(typeof(UIText))]
public class ConfigurableDataVersion : MonoBehaviour {

	// Use this for initialization
	void OnEnable () 
	{
		GetComponent<UIText> ().Text = ConfigurableData.Instance.data.version;
	}
}
