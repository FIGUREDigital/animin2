﻿using UnityEngine;
using System.Collections;

public class InvBoxControls : MonoBehaviour {

    
    public delegate void DropAction();
    public static event DropAction OnDropItem;
    UnityEngine.UI.Image box;
    public static bool listening;

	// Use this for initialization
	void Start () 
    {
        box = GetComponent<UnityEngine.UI.Image>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (!listening)
            return;
        if(Input.GetButtonUp("Fire1"))
        {
            DropItem();
        }
	}

    public void OnHoverBegin()
    {
        listening = true;
        transform.parent.localScale = new Vector3(1.1f, 1.1f, 1.1f);
    }
    public void OnHoverEnd()
    {
        listening = false;
        transform.parent.localScale = Vector3.one;
    }

    private UIPopupItemScript m_ItemScript;
    CaringPageControls m_CaringPageControls;
    void DropItem()
    {
        Debug.Log("[Drop Item]");
        GameObject GO = MainARHandler.Instance.CurrentItem;
        if (GO == null)
        {
            Debug.LogWarning("[Drop Item]: ERROR NO ITEM");
            return;
        }
        Debug.Log("[Drop Item]: Item = "+GO.name);
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
