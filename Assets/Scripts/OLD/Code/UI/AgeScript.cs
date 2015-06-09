using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class AgeScript : MonoBehaviour {

	void OnEnable()
	{
		TextMeshProUGUI label = gameObject.GetComponent<TextMeshProUGUI>();
		label.text = "Age "+ ProfilesManagementScript.Instance.CurrentAnimin.Age;
	}
}
