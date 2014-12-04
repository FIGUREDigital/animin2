using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class DragDropMainBarItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	public bool dragOnSurfaces = true;
	
	private GameObject m_DraggingIcon;
	private RectTransform m_DraggingPlane;
	
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
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out hit))
		{
			Debug.Log(hit.point);
		}
        if(!hit.collider.gameObject.CompareTag("Ground")) // Gross, check the floor layer
		{
			return;
		}
			
				//ReferencedObjectScript refScript = this.GetComponent<ReferencedObjectScript>();
				InterfaceItemLinkToModelScript modelLink = GetComponent<InterfaceItemLinkToModelScript>();
				
			
					GameObject resourceLoaded = (GameObject)Resources.Load(modelLink.Item3DPrefab);
					GameObject child = (GameObject)GameObject.Instantiate(resourceLoaded);
					UIPopupItemScript popScript = child.GetComponent<UIPopupItemScript>();
					ProfilesManagementScript.Singleton.CurrentAnimin.RemoveItemFromInventory (modelLink.ItemID,1);
					
					child.transform.parent = UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>().ActiveWorld.transform;
					
					child.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
					child.transform.localRotation = Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0);
					
					//child.GetComponent<ReferencedObjectScript>().Reference = refScript.Reference;
					
					UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>().DragedObjectedFromUIToWorld = true;
					
					
					if(popScript.Type == PopupItemType.Food)
						UIGlobalVariablesScript.Singleton.SoundEngine.Play(GenericSoundId.DropFood);
					else if(popScript.Type == PopupItemType.Item)
						UIGlobalVariablesScript.Singleton.SoundEngine.Play(GenericSoundId.DropItem);
					else if(popScript.Type == PopupItemType.Medicine)
						UIGlobalVariablesScript.Singleton.SoundEngine.Play(GenericSoundId.DropMeds);
					Transform trans = child.transform;
					trans.position = hit.point;
					UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>().GroundItems.Add(child);
				
		

	}
}
