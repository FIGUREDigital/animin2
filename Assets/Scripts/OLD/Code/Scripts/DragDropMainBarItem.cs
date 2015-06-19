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
        InterfaceItemLinkToModelScript modelLink = GetComponent<InterfaceItemLinkToModelScript>();
				
			
		GameObject child = modelLink.item.Create();
		ItemDefinition item = child.GetComponent<ItemDefinition>();
        ProfilesManagementScript.Instance.CurrentAnimin.RemoveItemFromInventory(modelLink.item.Id, 1);
					
        child.transform.parent = UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>().ActiveWorld.transform;
	//	child.transform.localScale = new Vector3 (1, 1, 1);
					
        child.transform.localRotation = Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0);

        UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>().DragedObjectedFromUIToWorld = true;
        if(OnClicked != null)
            OnClicked();

        TutorialHandler.TriggerAdHocStatic("Drop"+item.Id.ToString());
					
		switch (item.ItemType) 
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
        child.transform.position = hit.point;
        UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>().GroundItems.Add(child);
    }
}
