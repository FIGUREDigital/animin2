using UnityEngine;
using System.Collections;

public class TboEvolutionWarningScript : MonoBehaviour {

		public bool ContinueToPurchase;


		public void OnClick()
		{
			ProfilesManagementScript.Instance.CloseEvolutionPurchaseWarning (ContinueToPurchase);

		}
}
