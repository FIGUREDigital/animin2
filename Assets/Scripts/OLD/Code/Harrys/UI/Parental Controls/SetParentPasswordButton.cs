using UnityEngine;
using System.Collections;

public class SetParentPasswordButton : MonoBehaviour {

	[SerializeField]
	private RectTransform SetParentPasswordPanel;

	void OnClick(){
		if (SetParentPasswordPanel != null) {
			this.transform.parent.parent.gameObject.SetActive (false);
			SetParentPasswordPanel.gameObject.SetActive(true);
		}
	}
}
