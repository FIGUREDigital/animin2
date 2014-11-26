using UnityEngine;
using System.Collections;

public class SendAddress : MonoBehaviour 
{
	void OnClick()
	{

		AddressScreen screen = transform.parent.GetComponent<AddressScreen>();
		if(!Application.isEditor)
		{
			screen.Send();
		}
		ProfilesManagementScript.Singleton.AddressInput.SetActive(false);
		ProfilesManagementScript.Singleton.AniminsScreen.SetActive(true);
	}
}
