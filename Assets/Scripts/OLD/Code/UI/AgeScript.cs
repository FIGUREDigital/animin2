using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AgeScript : MonoBehaviour {

	void OnEnable()
	{
		Text label = gameObject.GetComponent<Text>();
		label.text = "Age "+ ProfilesManagementScript.Singleton.CurrentAnimin.Age;
	}
}
