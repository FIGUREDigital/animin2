using UnityEngine;
using System.Collections;

public class OpenInGamePurchaseView : MonoBehaviour
{
    public GameObject Parent;
    public GameObject Form;
    public GameObject ExitWebview;
	private bool mOpen;

    private void Start()
    {
        Parent.SetActive( false );
        Form.SetActive( false );
        ExitWebview.SetActive( false );
    }

	void OnClick()
    {
        if (Application.loadedLevelName == "VuforiaTest")
            UIGlobalVariablesScript.Singleton.TutHandler.Lock = !mOpen;

		mOpen = !mOpen;
		Debug.Log( "opening" + name );
		Parent.SetActive( mOpen );
		ExitWebview.SetActive( mOpen );
		CloseButtons cb = UIGlobalVariablesScript.Singleton.CaringScreenRef.GetComponent<CloseButtons>();

		if(mOpen)
		{
			cb.Close();
		}
		else
		{
			cb.Open();
		}
	}
}
