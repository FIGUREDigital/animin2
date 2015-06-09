using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class CrystalCount : MonoBehaviour 
{
//	private Text crystalAmount;

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
	//	crystalAmount = GetComponent<Text>(); 
	//	string numCrystals = PersistentData.CrystalCount.ToString();
	//	crystalAmount.text = numCrystals;
	}

	void OnEnable ()
	{
		GetComponent<TextMeshProUGUI>().text = ProfilesManagementScript.Instance.CurrentAnimin.CrystalCount.ToString();
	}

}
