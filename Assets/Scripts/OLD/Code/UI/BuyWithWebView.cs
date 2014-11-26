using UnityEngine;
using System.Collections;

public class BuyWithWebView : MonoBehaviour
{
    public GameObject ExitWebViewObject;
    public GameObject FirstNameInput;
    public GameObject LastNameInput;
	public GameObject AddressInput;
	public GameObject AddresseeInput;

	public GameObject AddressContainer;
	public GameObject NameContainer;

	private Input _inputField1;
	private Input _inputField2;

    private void OnClick()
    {
//        if( Account.Instance.UniqueID != null )
//        {
//            StartCoroutine( Account.Instance.WWWSendData( Account.Instance.UserName, " ", "empty", LastNameInput.GetComponent<Input>().value ) );
//        }        

//		if (AddressContainer.activeInHierarchy) {
//			AddressContainer.SetActive(false);
//			NameContainer.SetActive(true);
//
//			_inputField1 = FirstNameInput.transform.parent.GetComponentInChildren<Input> ();
//			_inputField2 = LastNameInput.transform.parent.GetComponentInChildren<Input> ();
//			Account.Instance.FirstName = NGUIText.StripSymbols (_inputField1.value);
//			Account.Instance.LastName = NGUIText.StripSymbols (_inputField2.value);
//				}
//
//		if (NameContainer.activeInHierarchy)
//		{
//			NameContainer.SetActive(false);
//			AddressContainer.SetActive(false);
//
//			_inputField1 = AddressInput.transform.parent.GetComponentInChildren<Input> ();
//			_inputField2 = AddresseeInput.transform.parent.GetComponentInChildren<Input> ();
//			Account.Instance.Address = NGUIText.StripSymbols (_inputField1.value);
//			Account.Instance.Addressee = NGUIText.StripSymbols (_inputField2.value);
//
//			LaunchWebview();
//
//		}

		//ProfilesManagementScript.Singleton.AniminsScreen.SetActive( false );
		//ProfilesManagementScript.Singleton.CloseWebview.SetActive( true );/

        //UnlockCharacterManager.Instance.BuyCharacter( ProfilesManagementScript.Singleton.AniminToUnlockId, false );

        if( Application.isEditor )
        {
            //ProfilesManagementScript.Singleton.PurchaseChoiceScreen.SetActive( false );
            //ProfilesManagementScript.Singleton.AniminsScreen.SetActive( true );
            return;
        }
        //ProfilesManagementScript.Singleton.LoadingSpinner.SetActive(true);
        //ProfilesManagementScript.Singleton.PurchaseChoiceScreen.SetActive(false);
    }
	

}