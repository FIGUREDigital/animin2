using UnityEngine;
using System.Collections;

using UnityEngine.EventSystems;

public class DetectDragIconScript : MonoBehaviour, IBeginDragHandler {

    public delegate void ClickAction();
    public static event ClickAction OnClicked;
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		InterfaceItemLinkToModelScript refScript = this.GetComponent<InterfaceItemLinkToModelScript>();
		if(refScript.ItemID == InventoryItemId.None) return;

        MainARHandler.Instance.MainARCamera.GetComponentInChildren<CameraModelScript>().SpriteRef = this.gameObject;
		//InterfaceItemLinkToModelScript popScript = refScript.Reference.GetComponent<InterfaceItemLinkToModelScript>();



		GameObject resourceLoaded = (GameObject)Resources.Load(InventoryItemData.Items[(int)refScript.ItemID].PrefabId);
		GameObject child = (GameObject)GameObject.Instantiate(resourceLoaded);

		child.GetComponent<BoxCollider>().enabled = false;

        child.transform.parent = MainARHandler.Instance.MainARCamera.GetComponentInChildren<CameraModelScript>().transform;
        MainARHandler.Instance.CurrentItem = child;
		child.transform.position = Vector3.zero;
        child.transform.localPosition += new Vector3(0, 0, 30f);
		child.transform.rotation = Quaternion.identity;
        child.transform.localScale = Vector3.one / 10f;
		child.transform.localRotation = Quaternion.identity;
		child.transform.localPosition = Vector3.zero;

		child.transform.localRotation = Quaternion.Euler(0, 180, 0);
        if(OnClicked != null)
            OnClicked();
		if (refScript.ItemID == InventoryItemId.woodSword)
						child.transform.localScale *= 2.5f;
		else if (refScript.ItemID == InventoryItemId.woodFrame)
						child.transform.localScale *= 6f;
	}
}
