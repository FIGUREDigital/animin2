using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class CameraModelScript : Phi.SingletonMonoBehaviour<CameraModelScript>
{
	// Use this for initialization
	public override void Init () {
	}

	bool destroyAtEnd = false;

	public void SetDragging(GameObject go, bool destroyAtEnd = true)
	{
		//Debug.Log ("CameraModelScript SetDraggin: " + (go == null ? "null" : go.name));
		if (transform.childCount > 0) 
		{
			if (go != null) 
			{
				Debug.LogError ("Should not be dragging something already - deleting old item!");
			}
			while (transform.childCount > 0)
			{
				Transform c = transform.GetChild(0);
				c.parent = null;
				if(this.destroyAtEnd)
				{
					Destroy (c.gameObject);
				}
			}
		}
		if (go != null) 
		{
			this.destroyAtEnd = destroyAtEnd;
			go.transform.parent = transform;
		}
	}

	// Update is called once per frame
	void LateUpdate () {

		if (Camera.main == null)
			return;
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		this.transform.position = ray.origin + ray.direction * 0.25f;

		//new Vector3(ray.origin.x * 4, ray.origin.y * 4, this.transform.localPosition.z);
		//this.transform.localScale = new Vector3(0.1f,0.1f,0.1f);
#if false
		// AH should no longer need this anything starting a drag should stop it!
		if(this.transform.childCount > 0)
		{
			if(Input.GetButtonUp("Fire1"))
			{
				bool hadUItouch = false;				
				Camera nguiCam = Camera.main;
				
				if( nguiCam != null )
				{
					Ray inputRay = nguiCam.ScreenPointToRay( Input.mousePosition );    
					RaycastHit hit;
					
					if (Physics.Raycast(inputRay, out hit))
					{
						if(hit.collider.gameObject.layer == LayerMask.NameToLayer( "NGUI" ))
						{
							DestroyChildModelAndHide();
							return;
						}
					}
				}

				RaycastHit hitInfo;
				if (Physics.Raycast(ray, out hitInfo))
				{

					if(hitInfo.collider.name.StartsWith("Invisible Ground Plane"))
					{
						Spawn(false);
					}
					else if(hitInfo.collider.gameObject == UIGlobalVariablesScript.Singleton.MainCharacterRef)
					{
						//ReferencedObjectScript refScript = SpriteRef.GetComponent<ReferencedObjectScript>();
						//UIPopupItemScript popScript = refScript.Reference.GetComponent<UIPopupItemScript>();

						//CharacterProgressScript progressScript = UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>();


						/*if(popScript.Type == PopupItemType.Food)
						{
							progressScript.OnInteractWithPopupItem(popScript);
							DestroyChildModelAndHide();
						}
						else if(popScript.Type == PopupItemType.Item)
						{
							if(progressScript.ObjectHolding == null)
							{
								Spawn(true);
							}
							else
							{
								DestroyChildModelAndHide();
							}
						}
						else if(popScript.Type == PopupItemType.Medicine)
						{
							progressScript.OnInteractWithPopupItem(popScript);
							DestroyChildModelAndHide();
						}*/

						DestroyChildModelAndHide();
					}
					else
					{
						DestroyChildModelAndHide();
					}
				}
				else
				{
					DestroyChildModelAndHide();
				}
			}
		}
#endif
	}

	/*
	private void DestroyChildModelAndHide()
	{
		for(int i=0;i<this.transform.childCount;++i)
		{
			Destroy(this.transform.GetChild(i).gameObject);
		}

		SpriteRef = null;
	}

	private void Spawn(bool holdInHands)
	{
		Debug.Log ("SpriteRef : ["+SpriteRef+"];");
		if(SpriteRef != null)
		{
			InterfaceItemLinkToModelScript refScript = SpriteRef.GetComponent<InterfaceItemLinkToModelScript>();
//			InterfaceItemLinkToModelScript modelItem = refScript.Reference.GetComponent<InterfaceItemLinkToModelScript>();
			
			GameObject resourceLoaded = (GameObject)Resources.Load(InventoryItemData.GetDefinition(refScript.ItemID).PrefabId);

			GameObject child = (GameObject)GameObject.Instantiate(resourceLoaded);

			UIPopupItemScript popScript = child.GetComponent<UIPopupItemScript>();

			bool hasRemainingItemsOfThisId = ProfilesManagementScript.Instance.CurrentAnimin.RemoveItemFromInventory(popScript.Id, 1);

		
			if(!hasRemainingItemsOfThisId)
			{
				//Debug.Log("!hasRemainingItemsOfThisId");
				InventoryItemData itemData = ProfilesManagementScript.Instance.CurrentAnimin.GetNextItemType(InventoryItemData.GetDefinition(popScript.Id).ItemType);

				if(itemData != null)
				{
					//SpriteRef.GetComponent<Button>().normalSprite = InventoryItemData.Items[(int)itemData.Id].SpriteName;
					SpriteRef.GetComponent<InterfaceItemLinkToModelScript>().ItemID = itemData.Id;

				}
				else
				{
					//SpriteRef.GetComponent<Button>().normalSprite = "empty_icon";
					SpriteRef.GetComponent<InterfaceItemLinkToModelScript>().ItemID = InventoryItemId.None;
				}
			}
			else
			{

			}

			child.transform.parent = UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>().ActiveWorld.transform;


			float scale = 0.1f;
			if (popScript != null) {
				if (popScript.Id == InventoryItemId.woodSword)
					scale = 0.3f;
				else if (popScript.Id == InventoryItemId.woodFrame)
					scale = 0.3f;
				else
					scale = 0.1f;
			}  else {
			}
			child.transform.localScale = new Vector3(scale,scale,scale);
			child.transform.localRotation = Quaternion.Euler(0, UnityEngine.Random.Range(130, 230), 0);
			
//			child.GetComponent<ReferencedObjectScript>().Reference = refScript.Reference;
			
			UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>().DragedObjectedFromUIToWorld = true;

			if(popScript.Type == PopupItemType.Food)
				UIGlobalVariablesScript.Singleton.SoundEngine.Play(GenericSoundId.DropFood);
			else if(popScript.Type == PopupItemType.Item)
				UIGlobalVariablesScript.Singleton.SoundEngine.Play(GenericSoundId.DropItem);
			else if(popScript.Type == PopupItemType.Medicine)
				UIGlobalVariablesScript.Singleton.SoundEngine.Play(GenericSoundId.DropMeds);
			
			//child.AddComponent<UIPopupItemScript>();
			//child.GetComponent<UIPopupItemScript>().Points = this.gameObject.GetComponent<UIPopupItemScript>().Points;
			//child.GetComponent<UIPopupItemScript>().Type = this.gameObject.GetComponent<UIPopupItemScript>().Type;
			
			Transform trans = child.transform;
			//trans.position = UICamera.lastHit.point;	
			
			//					if (dds.rotatePlacedObject)
			//					{
			//						trans.rotation = Quaternion.LookRotation(UICamera.lastHit.normal) * Quaternion.Euler(90f, 0f, 0f);
			//					}


			//CharacterProgressScript progressScript = UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>();

			UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>().GroundItems.Add(child);

		}

		DestroyChildModelAndHide();

	}
	*/
}
