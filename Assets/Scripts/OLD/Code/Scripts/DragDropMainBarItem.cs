using UnityEngine;
using System.Collections;
/*
public class DragDropMainBarItem : DragDropMainBarItem
{

	/// <summary>
	/// Prefab object that will be instantiated on the DragDropSurface if it receives the OnDrop event.
	/// </summary>
	
	//public GameObject prefab;
	
	/// <summary>
	/// Drop a 3D game object onto the surface.
	/// </summary>

	protected override void OnDragDropStart ()
	{
		//this.gameObject.GetComponent<BoxCollider>().enabled = false;

		base.OnDragDropStart ();
	}
	
	protected override void OnDragDropRelease (GameObject surface)
	{
		Debug.Log("DETECTED DRAG DROP RELEASE");
		if(surface != null) Debug.Log(surface.name);
		else Debug.Log("the surface is null");

		//this.gameObject.GetComponent<BoxCollider>().enabled = true;

		if (surface != null)
		{
			ExampleDragDropSurface dds = surface.GetComponent<ExampleDragDropSurface>();
			
			if (dds != null)
			{
				ReferencedObjectScript refScript = this.GetComponent<ReferencedObjectScript>();
				InterfaceItemLinkToModelScript modelLink = refScript.Reference.GetComponent<InterfaceItemLinkToModelScript>();

				// its a character drag and drop
				if(dds.GetComponent<CharacterProgressScript>() != null)
				{
					//UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>().DragedObjectedFromUIToWorld = true;
					//dds.GetComponent<CharacterProgressScript>().OnInteractWithPopupItem(popScript);
				}
				else
				{
					GameObject resourceLoaded = (GameObject)Resources.Load(modelLink.Item3DPrefab);
					GameObject child = (GameObject)GameObject.Instantiate(resourceLoaded);
					UIPopupItemScript popScript = child.GetComponent<UIPopupItemScript>();

					child.transform.parent = UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>().ActiveWorld.transform;

					child.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
					child.transform.localRotation = Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0);

					child.GetComponent<ReferencedObjectScript>().Reference = refScript.Reference;

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
					trans.position = UICamera.lastHit.point;	
					
//					if (dds.rotatePlacedObject)
//					{
//						trans.rotation = Quaternion.LookRotation(UICamera.lastHit.normal) * Quaternion.Euler(90f, 0f, 0f);
//					}

					UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>().GroundItems.Add(child);
					//Debug.Log("DCREATED!!!");
					// Destroy this icon as it's no longer needed
					//NGUITools.Destroy(gameObject);
					//return;
				}
			}
		}

		base.OnDragDropRelease(surface);
	}
}
*/