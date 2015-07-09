using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class DragDropMainBarItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public bool dragOnSurfaces = true;
	
    private GameObject m_DraggingIcon;
    private RectTransform m_DraggingPlane;
    public delegate void ClickAction();
    public static event ClickAction OnDropped;

    public void OnBeginDrag(PointerEventData eventData)
    {
    }

    public void OnDrag(PointerEventData data)
    {

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("DETECTED DRAG DROP RELEASE");

        RaycastHit hit;
		Debug.Log ("Main Camera: "+Camera.main.name);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Floor", "ExtendedFloor")))
        {
            Debug.Log(hit.point);
        }
		
		GameObject GO = MainARHandler.Instance.CurrentItem;
		if (GO == null) 
		{
			Debug.LogError ("OnEndDrag caled when MainARHandler.Instance.CurrentItem == null");
			return;
		}
		ItemLink modelLink = GO.GetComponent<ItemLink>();
		Vector3 hitPos = Vector3.zero;	// Possible bug if no hit position exists, need to work out if this occurs and what to do!
		if (hit.collider != null) 
		{
			hitPos = Boxes.GetGroundPoint (hit);
		}
		modelLink.item.MoveTo (Inventory.CurrentLocation, hitPos, new Vector3(0, UnityEngine.Random.Range (180-45, 180+45), 0));

		CharacterProgressScript.SwitchGravity (GO, true);
		if (hit.collider == null || hit.collider.name.StartsWith("Extended")) 
		{
			GO.AddComponent<DroppedItemScript>();			
		}
			
        //ReferencedObjectScript refScript = this.GetComponent<ReferencedObjectScript>();

				
			
		GameObject child = modelLink.item.Instance;
		/*if (modelLink.item.Definition.ItemType != PopupItemType.Box) {
			child.transform.localRotation = Quaternion.Euler (0, UnityEngine.Random.Range (0, 360), 0);
		}*/		
//		child.GetComponent<BoxCollider>().enabled = true;

        UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>().DragedObjectedFromUIToWorld = true;
        if(OnDropped != null)
            OnDropped();

		TutorialHandler.TriggerAdHocStatic("Drop"+modelLink.item.Definition.ToString());
					
		switch (modelLink.item.Definition.ItemType) 
		{
		case PopupItemType.Food:
			UIGlobalVariablesScript.Singleton.SoundEngine.Play (GenericSoundId.DropFood);
			break;
		case PopupItemType.Item:
			UIGlobalVariablesScript.Singleton.SoundEngine.Play (GenericSoundId.DropItem);
			break;
		case PopupItemType.Medicine:
			UIGlobalVariablesScript.Singleton.SoundEngine.Play (GenericSoundId.DropMeds);
			break;
		}
		MainARHandler.Instance.CurrentItem = null;
    }
}
