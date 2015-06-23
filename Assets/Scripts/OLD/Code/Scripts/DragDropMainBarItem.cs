using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class DragDropMainBarItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public bool dragOnSurfaces = true;
	
    private GameObject m_DraggingIcon;
    private RectTransform m_DraggingPlane;
    public delegate void ClickAction();
    public static event ClickAction OnClicked;

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
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log(hit.point);
        }
        if (!hit.collider.gameObject.CompareTag("Ground")) // Gross, check the floor layer
        {
            if(OnClicked != null)
                OnClicked();
            return;
        }
			
        //ReferencedObjectScript refScript = this.GetComponent<ReferencedObjectScript>();
		
		GameObject GO = MainARHandler.Instance.CurrentItem;
		ItemLink modelLink = GO.GetComponent<ItemLink>();
				
			
		GameObject child = modelLink.item.Instance;
		modelLink.item.MoveTo (Inventory.CurrentLocation, hit.point);					
        child.transform.localRotation = Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0);
		
		child.GetComponent<BoxCollider>().enabled = true;

        UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>().DragedObjectedFromUIToWorld = true;
        if(OnClicked != null)
            OnClicked();

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
    }
}
