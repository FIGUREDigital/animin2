using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Phi;


public enum PopupItemType
{
	Food = 0,
	Item,
	Medicine,
	Token,
	Box,
	Count
}

public enum MenuFunctionalityUI
{
	None = 0,
	Mp3Player,
	Phone,
	Clock,
	Lightbulb,
	EDMBox,
	Juno,
	Piano,
	Alarm,
	Radio,
	ItemAlbum
}

public enum SpecialFunctionalityId
{
	None,
	Liquid,
	Injection,
	
}

public class ItemDefinition : MonoBehaviour
{
	[SerializeField]
	public int points;

	[SerializeField]
	PopupItemType itemType;

	[SerializeField]
	MenuFunctionalityUI menu;

	[SerializeField]
	SpecialFunctionalityId specialId;	// Defines the sound to play when the item is consumed

	[SerializeField]
	InventoryItemId id;
	
	[SerializeField]
	Sprite sprite;
	
	[SerializeField]
	UnityEngine.Gradient gradient;


	public SpecialFunctionalityId SpecialId
	{
		get
		{
			return specialId;
		}
	}

	public MenuFunctionalityUI Menu
	{
		get
		{
			return menu;
		}
	}

	public int Points
	{
		get
		{
			return points;
		}
	}

	public InventoryItemId Id
	{
		get
		{
			return id;
		}
	}

	public PopupItemType ItemType
	{
		get
		{
			return itemType;
		}
	}

	
	public Sprite SpriteName
	{
		get
		{
			return sprite;
		}
	}

	public UnityEngine.Gradient Gradient
	{
		get
		{
			return gradient;
		}
	}
	
	public GameObject Create(Inventory.Entry entry, bool asInventoryModel = false)
	{
		GameObject go = (GameObject)GameObject.Instantiate(gameObject);
		if (asInventoryModel) 
		{
			go.tag = "Untagged";
		}
		else
		{
			go.AddComponent<ItemLink> ().item = entry;
		}

		Destroy (go.GetComponent<ItemDefinition>());	// Remove this component and replace with the ItemLink
		go.SetActive(true);
		return go;
	}

	public void Awake()
	{
		ItemDefinition existing;
		if (itemDefinitions.TryGetValue(id, out existing)) 
		{
			// Must be a duplicate which is actually used so leave it allone unless it has the same parent as other
			// registered definitions.
			if (existing.transform.parent == transform.parent)
			{
				Debug.LogError ("Duplicate ("+name+") item of type "+Id.ToString ()+" found and ignored");
				gameObject.SetActive(false);
			}
		}
		else
		{
			itemDefinitions.Add (id, this);
			gameObject.SetActive(false);
		}
	}	
	
	private static Dictionary<InventoryItemId, ItemDefinition> itemDefinitions = new Dictionary<InventoryItemId, ItemDefinition>();

	public static List<ItemDefinition> GetAllDefinitions()
	{
		List<ItemDefinition> list = new List<ItemDefinition> (itemDefinitions.Values);
		return list;
	}

	public static ItemDefinition GetDefinition(InventoryItemId id)
	{
		ItemDefinition def = null;
		if (itemDefinitions.TryGetValue (id, out def)) 
		{
			return def;
		}
		return null;
	}

	public override string ToString()
	{
		return id.ToString();
	}
}
