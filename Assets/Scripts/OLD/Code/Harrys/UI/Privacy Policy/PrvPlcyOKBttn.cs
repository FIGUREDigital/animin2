using UnityEngine;
using System.Collections;

public class PrvPlcyOKBttn : MonoBehaviour {

	[SerializeField]
	private PrivacyPolicy policy;

	void OnClick () {
		policy.Okay ();
	}
}
