using UnityEngine;
using System.Collections;

using UnityEngine.EventSystems;

public class DetectDragIconScript : MonoBehaviour, IBeginDragHandler, IEndDragHandler {

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

	
	public void OnEndDrag(PointerEventData eventData)
	{
		CameraModelScript.Instance.SetDragging(null);
	}
        
    public void OnBeginDrag(PointerEventData eventData)
	{
		ItemLink refScript = this.GetComponent<ItemLink>();
        if (refScript.item == null) return;
        Debug.Log("Begin drag " + refScript.item.ToString());

		//CameraModelScript.Instance.SpriteRef = this.gameObject;

		GameObject child = refScript.item.Instance;
		CharacterProgressScript.SwitchGravity (child, false);

//		child.GetComponent<BoxCollider>().enabled = false;

		CameraModelScript.Instance.SetDragging(child, false);
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
		/*if (refScript.ItemID == InventoryItemId.woodSword)
						child.transform.localScale *= 2.5f;
		else if (refScript.ItemID == InventoryItemId.woodFrame)
						child.transform.localScale *= 6f;*/
	}
}
