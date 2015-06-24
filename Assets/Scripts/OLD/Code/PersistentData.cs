using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;


// CaringScreenItem is no longer used except in this file and old saves
[System.Serializable]
public struct CaringScreenItem{
    public InventoryItemId Id;
    public Vector3 Position;
    public CaringScreenItem(InventoryItemId id, Vector3 pos){
        Id = id;
        Position = pos;
    }
}

[System.Serializable]
public class PersistentData
{
	static PersistentData()
	{
		
	}

	public enum TypesOfAnimin
	{
        Pi = 0,
        Tbo,
        Kelsey,
        Mandi,
        TboAdult,

        Count,
	}    

	public TypesOfAnimin PlayerAniminId;
    public string AniminName
    {
        get
		{
			switch (PlayerAniminId)
			{
			case TypesOfAnimin.Pi:
				return "Pi";
			case TypesOfAnimin.Kelsey:
				return "Kelsi";
			case TypesOfAnimin.Mandi:
				return "Mandi";
			default:
				return "T-Bo";
			}
		}
	}

	public bool IsMale
	{		
		get
		{
			return PlayerAniminId != TypesOfAnimin.Kelsey && PlayerAniminId != TypesOfAnimin.Mandi;
		}
	}

	public AniminEvolutionStageId AniminEvolutionId;
    public int EggTaps = 0;
    public bool Hatched 
    {
        get
        {
            return EggTaps > 5;
        }
    }
//	public const float MaxHappy = 125.0f;
	public const float MaxHappy = 100.0f;
	public const float MaxHungry = 100;
	public const float MaxFitness = 100;
	public const float MaxHealth = 100;
	public int ZefTokens;
	public int CrystalCount;
	public List<AniminSubevolutionStageId> SubstagesCompleted = new List<AniminSubevolutionStageId>(); 
	public string Username;
	public System.DateTime CreatedOn = DateTime.UtcNow;
	
	private int age;
	private bool audioIsOn;
	public static float happy;
	private float hungry;
	private float fitness;
	private float evolution;
	private float health;
	public DateTime lastPlay;

	public int Age
	{
		get
		{
			CalcAge();
			return age;
		}
	}
	public float Happy
	{
		get
		{
			return happy;
		}
		set
		{
			happy = value;
			if(happy > MaxHappy) happy = MaxHappy;
			if(happy < 0) happy = 0;
		}
	}
	public float Hungry
	{
		get
		{
			return hungry;
		}
		set
		{
			hungry = value;
			if(hungry > 100) hungry = 100;
			if(hungry < 0) hungry = 0;
		}
	}
	public float Fitness
	{
		get
		{
			return fitness;
		}
		set
		{
			fitness = value;
			if(fitness > 100) fitness = 100;
			if(fitness < 0) fitness = 0;
		}
	}
	public float Evolution
	{
		get
		{
			return evolution;
		}
		set
		{
			evolution = value;
			if(evolution > 100) evolution = 100;
			if(evolution < 0) evolution = 0;
		}
	}
	public float Health
	{
		get
		{
			return health;
		}
		set
		{
			health = value;
			if(health > 100) health = 100;
			if(health < 0) health = 0;
		}
	}

	private void CalcAge()
	{
		System.TimeSpan realAge = System.DateTime.UtcNow.Subtract(CreatedOn);
		age = (int)Math.Floor(realAge.TotalDays / 3);
	}

    public void SetDefault(TypesOfAnimin animin)
	{
		SubstagesCompleted.Clear();
		PlayerAniminId = animin;
		AniminEvolutionId = AniminEvolutionStageId.Baby;
		CreatedOn = System.DateTime.UtcNow;
		
		Happy = MaxHappy;
		Hungry = MaxHungry * 0.65f;
		Fitness = MaxFitness;
		Health = MaxHealth;
		ZefTokens = 0;
		CrystalCount = 0;
	}

	// Following is old data just here to allow upgrading old saves
	public List<InventoryItemData> Inventory = new List<InventoryItemData>(); 
	public CaringScreenItem[] m_CaringScreenItems;
	public void UpdateFromVersion0(Inventory newInventory)
	{
		foreach (InventoryItemData item in Inventory) 
		{
			InventoryItemId id;
			int count;
			item.GetDataForUpgrade(out id, out count);
			ItemDefinition def = ItemDefinition.GetDefinition(id);
			if (def.ItemType == PopupItemType.Item)
			{
				// Only allow one of these
				newInventory.EnsureWeOwn(id, 1);
			}
			else
			{
				// Add to tally
				newInventory.Add(id, count);
			}
		}
		Inventory.Clear ();	// No longer keep these items in the animin inventory;

		// Add the items that are on the caring page floor		
		if (m_CaringScreenItems != null) 
		{
			foreach (CaringScreenItem cSItem in m_CaringScreenItems) 
			{
				global::Inventory.Entry entry;
				ItemDefinition def = ItemDefinition.GetDefinition (cSItem.Id);
				if (def.ItemType == PopupItemType.Item) {
					// Only allow one of these
					entry = newInventory.EnsureWeOwn (cSItem.Id);
				} else {
					// Add to tally
					entry = newInventory.Add (cSItem.Id);
				}
				// And move it to the caring screen
				entry.MoveTo (global::Inventory.Locations.NonAR, cSItem.Position);
			}
		}
		m_CaringScreenItems = new CaringScreenItem[0];
    }
}
