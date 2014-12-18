using UnityEngine;
using System.Collections;

public class InvBoxControls : MonoBehaviour {

    
    public delegate void DropAction();
    public static event DropAction OnDropItem;
    UnityEngine.UI.Image box;
	// Use this for initialization
	void Start () 
    {
        box = GetComponent<UnityEngine.UI.Image>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnHoverBegin()
    {
        transform.parent.localScale = new Vector3(1.1f, 1.1f, 1.1f);
    }
    public void OnHoverEnd()
    {
        transform.parent.localScale = Vector3.one;
    }

    private UIPopupItemScript m_ItemScript;
    CaringPageControls m_CaringPageControls;
    public void DropItem()
    {
        GameObject GO = MainARHandler.Instance.CurrentItem;
        if (GO == null)
        {
            return;
        }
        m_ItemScript = GO.GetComponent<UIPopupItemScript> ();
        m_ItemScript.NonInteractable = true;
        
        m_CaringPageControls = UiPages.GetPage(Pages.CaringPage).GetComponent<CaringPageControls>();
        
        bool isNonArScene = UIGlobalVariablesScript.Singleton.NonSceneRef.activeInHierarchy;

        ProfilesManagementScript.Singleton.CurrentAnimin.AddItemToInventory(m_ItemScript.Id,1);
        UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>().GroundItems.Remove(GO);
        
        m_CaringPageControls.DisappearAllItemUIs();
        
        UnityEngine.Object.Destroy(GO);
        OnDropItem();
    }
}
