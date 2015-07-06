using UnityEngine;
using System.Collections;

public class InvBoxControls : MonoBehaviour {

    
    public delegate void DropAction();
    public static event DropAction OnDropItem;
    public static bool listening;
    public static bool stopListening;

	// Update is called once per frame
	void Update () 
    {
        Debug.Log("Listening/StopListening : [" + listening + "|" + stopListening + "];");
        if (!listening)
            return;
        Debug.Log("Button : ["+Input.GetButtonUp("Fire1")+"];");
        if (Input.GetButtonUp("Fire1"))
        {
            DropItem();
        }
        if (stopListening)
        {
            stopListening = false;
            listening = false;
        }
	}

    public void OnHoverBegin()
    {
        listening = true;
        stopListening = false;
        transform.parent.localScale = new Vector3(1.1f, 1.1f, 1.1f);
    }
    public void OnHoverEnd()
    {
        //listening = false;
        stopListening = true;
        transform.parent.localScale = Vector3.one;
    }

    private ItemDefinition m_Item;
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
		ItemLink link = GO.GetComponent<ItemLink> ();
		if (link != null) 
		{
			link.item.MoveTo(Inventory.Locations.Inventory, Vector3.zero);
		}
		CharacterProgressScript.SwitchGravity (MainARHandler.Instance.CurrentItem, true);
		m_CaringPageControls = UiPages.GetPage(Pages.CaringPage).GetComponent<CaringPageControls>();
		m_CaringPageControls.DisappearAllItemUIs();

		OnDropItem();
		MainARHandler.Instance.CurrentItem = null;
    }
}
