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
	GameObject prefab;
	
	[SerializeField]
	Sprite sprite;

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
	
	public GameObject Create()
	{
		GameObject go = (GameObject)GameObject.Instantiate(gameObject);
		go.SetActive(true);
		return go;
	}

	public void Awake()
	{
		if (itemDefinitions.ContainsKey (id)) 
		{
			// Must be a duplicate which is actually used so leave it allone unless it has the same parent as other
			// registered definitions.
			if (itemDefinitions[id].transform.parent == transform.parent)
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
