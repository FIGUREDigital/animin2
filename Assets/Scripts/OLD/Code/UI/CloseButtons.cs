using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CloseButtons : MonoBehaviour {

	private Button[] mButtons;
    private OpenInGamePurchaseView view;
    //private ShowForm buyForm;

    private bool Exception(Button uib){
        if (uib.gameObject == view.gameObject)
            return true;
        if (uib.gameObject.GetComponent<ShowForm>()!=null)
            return true;
        return false;
    }
	// Use this for initialization
	void Start () 
	{
		Init ();
	}
	void Init()
	{
        mButtons = GetComponentsInChildren<Button>(true);
        view = GetComponentInChildren<OpenInGamePurchaseView>();
        //if (view != null)buyForm = view.gameObject.GetComponentInChildren<ShowForm>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Close()
	{
		if(mButtons == null)
		{
			Init ();
		}
		foreach(Button button in mButtons)
		{
            if (Exception(button))continue;
			button.gameObject.SetActive(false);
		}
	}

	public void Open()
	{
		if(mButtons == null)
		{
			Init ();
		}
		foreach(Button button in mButtons)
		{
            if (Exception(button))continue;
			button.gameObject.SetActive(true);
		}
		view.gameObject.SetActive(true);
		view.Parent.SetActive(false);
		view.Form.SetActive(false);
		view.ExitWebview.SetActive(false);
	}
}
