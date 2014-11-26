﻿using UnityEngine;
using System.Collections;

public class CloseWebView : MonoBehaviour 
{
	void OnClick()
	{
	    Debug.Log( "Closing webview" );
		Debug.Log ("Current Scene Name : ["+Application.loadedLevelName+"];");

		if (Application.loadedLevelName == "Menu") 
        {
			ProfilesManagementScript.Singleton.CloseWebview.SetActive (false);
			ProfilesManagementScript.Singleton.AniminsScreen.SetActive (true);
		} 
        else if (Application.loadedLevelName == "VuforiaTest") 
        {
            UIGlobalVariablesScript.Singleton.TutHandler.Lock = false;
			
			UIGlobalVariablesScript.Singleton.CaringScreenRef.SetActive(true);
			UIGlobalVariablesScript.Singleton.CaringScreenRef.GetComponent<CloseButtons>().Open();
            if (UIGlobalVariablesScript.Singleton.ParentalGateway != null && UIGlobalVariablesScript.Singleton.ParentalGateway.activeInHierarchy)
            {
                Destroy(UIGlobalVariablesScript.Singleton.ParentalGateway);
            }
            if (UIGlobalVariablesScript.Singleton.PurchaseAniminViaPaypal != null && UIGlobalVariablesScript.Singleton.PurchaseAniminViaPaypal.activeInHierarchy)
            {
                Destroy(UIGlobalVariablesScript.Singleton.PurchaseAniminViaPaypal);
            }
            transform.gameObject.SetActive(false);
		}
        CardPostedUI.Instantiate();
#if UNITY_IOS
      EtceteraBinding.inlineWebViewClose();  
#elif UNITY_ANDROID
		EtceteraAndroid.inlineWebViewClose();

#endif
	}
}
