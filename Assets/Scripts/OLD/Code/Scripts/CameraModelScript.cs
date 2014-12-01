using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class CameraModelScript : MonoBehaviour 
{
    #region Singleton

    private static CameraModelScript s_Instance;

    public static CameraModelScript Instance
    {
        get
        {
            if ( s_Instance == null )
            {
                s_Instance = new CameraModelScript();
            }
            return s_Instance;
        }
    }

    #endregion

	public GameObject SpriteRef;

	// Use this for initialization
	void Start () {
        s_Instance = this;
	}
	
	// Update is called once per frame
	void Update () {

		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		this.transform.position = ray.origin + ray.direction * 0.25f;//new Vector3(ray.origin.x * 4, ray.origin.y * 4, this.transform.localPosition.z);
		//this.transform.localScale = new Vector3(0.1f,0.1f,0.1f);

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
						ReferencedObjectScript refScript = SpriteRef.GetComponent<ReferencedObjectScript>();
						//UIPopupItemScript popScript = refScript.Reference.GetComponent<UIPopupItemScript>();

						CharacterProgressScript progressScript = UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>();


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
	}

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
			
			GameObject resourceLoaded = (GameObject)Resources.Load(InventoryItemData.Items[(int)refScript.ItemID].PrefabId);

			GameObject child = (GameObject)GameObject.Instantiate(resourceLoaded);

			UIPopupItemScript popScript = child.GetComponent<UIPopupItemScript>();

			bool hasRemainingItemsOfThisId = ProfilesManagementScript.Singleton.CurrentAnimin.RemoveItemFromInventory(popScript.Id, 1);

		
			if(!hasRemainingItemsOfThisId)
			{
				//Debug.Log("!hasRemainingItemsOfThisId");
				InventoryItemData itemData = ProfilesManagementScript.Singleton.CurrentAnimin.GetNextItemType(InventoryItemData.Items[(int)popScript.Id].ItemType);

				if(itemData != null)
				{
					//SpriteRef.GetComponent<Button>().normalSprite = InventoryItemData.Items[(int)itemData.Id].SpriteName;
					SpriteRef.GetComponent<InterfaceItemLinkToModelScript>().ItemID = itemData.Id;

	//				List<GameObject> allSprites = new List<GameObject>();
	//				UIClickButtonMasterScript.PopulateInterfaceItems(InventoryItemData.Items[(int)popScript.Id].ItemType, allSprites);
	//
	//				for(int i=0;i<allSprites.Count;++i)
	//				{
	//				
	//					string prefabId = allSprites[i].GetComponent<InterfaceItemLinkToModelScript>().Item3DPrefab;
	//					Debug.Log(prefabId);
	//					if(prefabId == InventoryItemData.Items[(int)popScript.Id].PrefabId)
	//					{
	//						Debug.Log("FOUND!");
	//						SpriteRef.GetComponent<Button>().normalSprite = InventoryItemData.Items[(int)popScript.Id].SpriteName;
	//						SpriteRef.GetComponent<ReferencedObjectScript>().Reference  = allSprites[i];
	//						break;
	//					}
	//				}

					//subItems[panelCount].transform.GetChild(0).gameObject.GetComponent<InterfaceItemLinkToModelScript>().Item3DPrefab = InventoryItemData.Items[(int)inventoryItems[i + 0].Id].PrefabId;
					//su
				 	/*InventoryItemData itemData = ProfilesManagementScript.Singleton.CurrentAnimin.GetNextItemType(InventoryItemData.Items[(int)popScript.Id].ItemType);

					if(itemData != null)
					{
						Debug.Log(itemData.Id.ToString());
					}

					//if(itemData == null)
					{
						SpriteRef.GetComponent<Button>().normalSprite = "empty_icon";
						SpriteRef.GetComponent<ReferencedObjectScript>().Reference  = null;
					}
					//else
					{
					//	SpriteRef.GetComponent<Button>().normalSprite = InventoryItemData.Items[(int)popScript.Id].SpriteName;
						//SpriteRef.GetComponent<ReferencedObjectScript>().Reference  = InventoryItemData.Items[(int)popScript.Id].PrefabId;

					}*/
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
			if(holdInHands)
			{
				UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>().PickupItem(child);

			}

            UIGlobalVariablesScript.Singleton.TutHandler.TriggerAdHocExitCond("Hungry", "EatStrawberry");

            if (popScript.Id == InventoryItemId.Boombox)
            {
                UIGlobalVariablesScript.Singleton.TutHandler.TriggerAdHocStartCond("BBPlace");
            }


			//Debug.Log("DCREATED!!!");
			// Destroy this icon as it's no longer needed
			//NGUITools.Destroy(gameObject);
			//return;
		}

		DestroyChildModelAndHide();

	}
}
