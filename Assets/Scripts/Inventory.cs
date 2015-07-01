using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[System.Serializable]
public class Inventory 
{
	public static Action<Entry, Locations, Locations> onItemMoved;
	public enum Locations
	{
		Inventory,
		NonAR,
		AR,
		Count
	}

	public static Locations CurrentLocation
	{
		get
		{
			return UIGlobalVariablesScript.Singleton.ARSceneRef.activeInHierarchy ? Locations.AR : Locations.NonAR;
		}
	}

	private static bool scanItemHeightRequired = false;

	[System.Serializable]
	public class Entry
	{
		static int layerUI;
		static int layerFloor;
		static Entry()
		{
			layerUI = LayerMask.NameToLayer("UI");
			layerFloor = LayerMask.NameToLayer("Floor");
		}

		// Our private data has to be public due to the use of the xmlserializer.
		public InventoryItemId privateId;	
		public Locations privateLocation = Locations.Count;	
		public Vector3 privatePosition = Vector3.zero;
		public bool justSpawnedFromChest = false;

		public ItemDefinition Definition
		{
			get
			{
				return ItemDefinition.GetDefinition(privateId);
			}
		}

		public InventoryItemId Id
		{
			get
			{
				return privateId;
			}
		}

		public Locations Location
		{
			get
			{
				return privateLocation;
			}
		}

		public Entry(InventoryItemId id)
		{
			this.privateId = id;
		}

		public Entry()
		{
		}

		public void MoveTo(Locations location, Vector3 position, bool justSpawnedFromChest = false)
		{
			this.justSpawnedFromChest = justSpawnedFromChest;
			Locations oldLoc = this.privateLocation;
			if (Definition.ItemType == PopupItemType.Box) 
			{
				GameObject go = Instance;
				SpinObjectScript spin = go.GetComponent<SpinObjectScript>();
				if(!justSpawnedFromChest)
				{
					position = Boxes.Snap (position);
					if (location != Locations.Inventory)
					{
						go.SetActive(true);	// We disable the item sometimes while in the inventory
	                }
					go.transform.localRotation = Quaternion.identity;
					scanItemHeightRequired = true;
				}
			}
			this.privateLocation = location;
			this.privatePosition = position;
			SetTransform ();
			SetupLayer ();
			if (onItemMoved != null) {
				onItemMoved(this, oldLoc, location);
			}
		}

		private void SetupLayer()
		{			
			if (Definition.ItemType == PopupItemType.Box) 
			{
				// Switch layer between UI and world cameras
				SetLayer(Instance.transform, privateLocation == Locations.Inventory ? layerUI : layerFloor);
			}
		}

		static public void SetLayer(Transform tr, int layer)
		{
			tr.gameObject.layer = layer;
			for(int i = tr.childCount -1; i >= 0; i--)
			{
				SetLayer(tr.GetChild(i), layer);
			}
		}

		public void SetTransform()
		{

			GameObject instance = Instance;
			SpinObjectScript spin = instance.GetComponent<SpinObjectScript>();
			instance.transform.parent = Inventory.roots[(int)privateLocation] != null ? Inventory.roots[(int)privateLocation].transform : null;
			instance.transform.position = privatePosition;
			
			if (Definition.ItemType == PopupItemType.Box)
			{
				if(justSpawnedFromChest)
				{
					instance.transform.localScale = new Vector3 (.1f, .1f, .1f);
					if(spin == null)
					{
						spin = instance.AddComponent<SpinObjectScript>();
					}
				}
				else
				{
					if(spin != null)
					{
						GameObject.Destroy(spin);
					}
					instance.transform.localScale = new Vector3 (.2f, .2f, .2f);
				}
			} else {
				instance.transform.localScale = new Vector3 (.1f, .1f, .1f);
			}
		}

		GameObject instance;
		public GameObject Instance
		{
			get
			{
				if (instance == null)
				{
					instance = Definition.Create(this);
					SetupLayer();
				}
				return instance;
			}
		}

		
		GameObject inventoryModel;
		public GameObject InventoryModel
		{
			get
			{
				if (inventoryModel == null)
				{
					inventoryModel = Definition.Create(this, true);					
					SetLayer(inventoryModel.transform, layerUI);
				}
				return inventoryModel;
			}
		}


		// This should only be called by Inventory
		public void DestroyInstance()
		{
			if (instance != null)
			{
				GameObject.Destroy(instance);
				instance = null;
			}
		}
	}

	public List<Entry> privateAllItems = new List<Entry>();

	// Not serialized
	static GameObject[] roots = new GameObject[(int)Locations.Count];

	public int Count
	{
		get
		{
			return privateAllItems.Count;
		}
	}

	public Entry Get(int index)
	{
		return privateAllItems[index];
	}

	public int GetNumItemsInInventory(InventoryItemId id)
	{
		int count = 0;
		for (int i = privateAllItems.Count - 1; i >= 0; i--) 
		{
			if(privateAllItems[i].Id == id && privateAllItems[i].Location == Locations.Inventory)
			{
				count++;
			}
		}
		return count;
	}

	
	public int GetNumItemsOwned(InventoryItemId id)
	{
		int count = 0;
		for (int i = privateAllItems.Count - 1; i >= 0; i--) 
		{
			if(privateAllItems[i].Id == id)
			{
                count++;
            }
        }
        return count;
    }

	// If we own an item return one from the inventory if possible else anywhere else
	public Entry GetOwnedItem(InventoryItemId id)
	{
		Entry e = null;
		for (int i = privateAllItems.Count - 1; i >= 0; i--) 
		{
			if(privateAllItems[i].Id == id)
			{
				e = privateAllItems[i];
				if (e.Location == Locations.Inventory)
				{
					return e;
				}
            }
        }
        return e;
    }
    
    public bool OwnItem(InventoryItemId id)
	{
		for (int i = privateAllItems.Count - 1; i >= 0; i--) 
		{
			if(privateAllItems[i].Id == id)
			{
				return true;
            }
        }
		return false;
    }

	// Ensure we own at least count items and if not add them to the inventory
	// Returns number of items added.
	public int EnsureWeOwn(InventoryItemId id, int count)
	{
		count -= GetNumItemsOwned(id);
		int added = 0;
		while (added < count) 
		{
			Add(id);
			added++;
		}
		return added;
	}

	
	public Entry EnsureWeOwn(InventoryItemId id)
	{
		Entry e = GetOwnedItem(id);
		if (e == null) 
		{
			e = Add (id);
		}
		return e;
    }

	public Entry Add(InventoryItemId id)
    {
		Entry newEntry = new Entry (id);
		privateAllItems.Add (newEntry);
		newEntry.MoveTo (Locations.Inventory, Vector3.zero);
		return newEntry;
	}
	
	public void Add(InventoryItemId id, int count)
	{
		while (count > 0) 
		{
			Add (id);
			count--;
		}
    }

	public void SetLocationRoot(Locations location, GameObject root)
	{
		roots [(int)location] = root;		
		for (int i = privateAllItems.Count - 1; i >= 0; i--) 
		{
			if(privateAllItems[i].Location == location)
			{
				privateAllItems[i].SetTransform();
			}
		}
	}

	public void Remove(Entry entry)
	{
		privateAllItems.Remove(entry);
		entry.DestroyInstance();
	}

	public List<Entry> GetEntries(Locations location, PopupItemType type = PopupItemType.Count)
	{
		List<Entry> result = new List<Entry> ();		
		for (int i = privateAllItems.Count - 1; i >= 0; i--) 
		{
			if(privateAllItems[i].Location == location)
			{
				if (type == PopupItemType.Count || type == privateAllItems[i].Definition.ItemType)
				{
					result.Add (privateAllItems[i]);
				}
            }
        }
		return result;
    }

	public void Update()
	{
		ScanItemHeights (false);
	}

	public static void ScanItemHeights(bool force = true)
	{	
		Inventory inv = ProfilesManagementScript.Instance.CurrentProfile.Inventory;
		if (!force && !scanItemHeightRequired)
		{
			return;
		}
		// Go through all items and do a raycast to position thier height
		Locations curLoc = CurrentLocation;
		BoxCollider boxCollider = null;
		if (MainARHandler.Instance.CurrentItem != null) 
		{
			ItemLink il = MainARHandler.Instance.CurrentItem.GetComponent<ItemLink>();
			if(il.item.Definition.ItemType == PopupItemType.Box)
			{
				boxCollider = il.GetComponent<BoxCollider>();
				if(!boxCollider.enabled)
				{
					boxCollider.enabled = true;
				}
				else
				{
					boxCollider = null;
				}
			}
		}
		for (int i = inv.privateAllItems.Count - 1; i >= 0; i--) 
		{
			Entry e = inv.privateAllItems[i];
			if(e.Location == curLoc && e.Definition.ItemType != PopupItemType.Box)
			{
				GameObject o = e.Instance;
				Vector3 pos = o.transform.position;
				pos.y = 1000;
				RaycastHit hit;
				if(Physics.Raycast(pos, Vector3.down, out hit, Mathf.Infinity, 1<<Boxes.FloorLayer))
				{
					pos.y = hit.point.y;
					o.transform.position = pos;
				}
			}
		}
		if (boxCollider != null) 
		{			
			boxCollider.enabled = false;
		}
		scanItemHeightRequired = false;
	}
}
