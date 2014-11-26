using UnityEngine;
using System.Collections;

public class ShowForm : MonoBehaviour
{
    public GameObject Parent;

    public void Start()
    {
        UIGlobalVariablesScript.Singleton.LoadAniminPaypalPurchaseScreen();
    }

    private void OnClick()
    {
//        Debug.Log( "Buying with webview" );

        UIGlobalVariablesScript.Singleton.OpenParentalGateway( Parent, UIGlobalVariablesScript.Singleton.PurchaseAniminViaPaypal, true );
//		LaunchWebview ();

    }
	
}