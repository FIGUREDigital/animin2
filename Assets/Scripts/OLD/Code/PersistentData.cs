using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

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

	public List<InventoryItemData> Inventory = new List<InventoryItemData>();
    public TypesOfAnimin PlayerAniminId;
	public AniminEvolutionStageId AniminEvolutionId;
	public const float MaxHappy = 125.0f;
	public const float MaxHungry = 100;
	public const float MaxFitness = 100;
	public const float MaxHealth = 100;
	public int ZefTokens;
	public List<AniminSubevolutionStageId> SubstagesCompleted = new List<AniminSubevolutionStageId>(); 
	public string Username;
	public System.DateTime CreatedOn;
	public static bool InventoryUpdated;


	private int age;
	private bool audioIsOn;
	private float happy;
	private float hungry;
	private float fitness;
	private float evolution;
	private float health;

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
		System.TimeSpan realAge = System.DateTime.Now.Subtract(CreatedOn);
		age = (int)Math.Floor(realAge.TotalDays / 3);
	}

    public void SetDefault(TypesOfAnimin animin)
	{
		SubstagesCompleted.Clear();
		PlayerAniminId = animin;
		AniminEvolutionId = AniminEvolutionStageId.Baby;
		CreatedOn = System.DateTime.Now;
		
		Happy = MaxHappy;
		Hungry = MaxHungry;
		Fitness = MaxFitness;
		Health = MaxHealth;
		ZefTokens = 0;

	}

	public void AddItemToInventory(InventoryItemId id, int count)
	{
		if (InventoryItemData.Items [(int)id] == null) {
						Debug.Log ("Something has gone terribely wrong");
						return;
				}
		if(!((InventoryItemData.Items [(int)id].ItemType == PopupItemType.Food) ||
		     (InventoryItemData.Items [(int)id].ItemType == PopupItemType.Medicine) ||
		     (InventoryItemData.Items [(int)id].ItemType == PopupItemType.Item)))
		    {
			Debug.Log ("Cannot add item! /nID : [" + id + "]; Data : [" + InventoryItemData.Items [(int)id].ItemType + "];");
			return;
		}
		for(int i=0;i<Inventory.Count;++i)
		{
			if(Inventory[i].Id == id)
			{
				Inventory[i].Count += count;
				return;
			}
		}

		Inventory.Add(new InventoryItemData() { Id = id, Count = count });

		if(InventoryItemData.Items[(int)id].ItemType == PopupItemType.Food)
		{
			if(UIGlobalVariablesScript.Singleton.FoodButton!=null&&UIGlobalVariablesScript.Singleton.FoodButton.GetComponent<Button>().image.name == "empty_icon")
			{
				//UIGlobalVariablesScript.Singleton.FoodButton.GetComponent<Button>().normalSprite = InventoryItemData.Items[(int)id].SpriteName;
				UIGlobalVariablesScript.Singleton.FoodButton.GetComponent<InterfaceItemLinkToModelScript>().ItemID = id;

			}
		}
		else if(InventoryItemData.Items[(int)id].ItemType == PopupItemType.Item)
		{
			if(UIGlobalVariablesScript.Singleton.ItemsButton!=null&&UIGlobalVariablesScript.Singleton.ItemsButton.GetComponent<Button>().image.name == "empty_icon")
			{
				//UIGlobalVariablesScript.Singleton.ItemsButton.GetComponent<Button>().normalSprite = InventoryItemData.Items[(int)id].SpriteName;
				UIGlobalVariablesScript.Singleton.ItemsButton.GetComponent<InterfaceItemLinkToModelScript>().ItemID = id;
				
			}
        }
        else if (InventoryItemData.Items[(int)id].ItemType == PopupItemType.Medicine)
        {
			if (UIGlobalVariablesScript.Singleton.MedicineButton!=null&&UIGlobalVariablesScript.Singleton.MedicineButton.GetComponent<Button>().image.name == "empty_icon")
            {
                //UIGlobalVariablesScript.Singleton.MedicineButton.GetComponent<Button>().normalSprite = InventoryItemData.Items[(int)id].SpriteName;
                UIGlobalVariablesScript.Singleton.MedicineButton.GetComponent<InterfaceItemLinkToModelScript>().ItemID = id;
            }
        }
	}

	public bool HasItem(InventoryItemId id)
	{
		for(int i=0;i<Inventory.Count;++i)
		{
			if(Inventory[i].Id == id)
			{
				return true;
			}
		}

		return false;
	}

	public InventoryItemData GetNextItemType(PopupItemType type)
	{
		for(int i=0;i<Inventory.Count;++i)
		{
			if(InventoryItemData.Items[(int)Inventory[i].Id].ItemType == type)
			{
				return Inventory[i];
			}
		}
		
		return null;
	}

	public bool RemoveItemFromInventory(InventoryItemId id, int count)
	{
		for(int i=0;i<Inventory.Count;++i)
		{
			if(Inventory[i].Id == id)
			{
				Inventory[i].Count -= count;
				Debug.Log ("Removed "+ count + " " + id.ToString());

				InventoryUpdated = true;
				if(Inventory[i].Count <= 0)
				{
					Inventory.RemoveAt(i);
					return false;
				}
				else
				{
					return true;
				}

			}
		}

		return false;
	}
	
//	public void Save(SaveLoadDictionary dictionary)
//	{
//		dictionary.Write("Hungry", Hungry);
//		dictionary.Write("Fitness", Fitness);
//		dictionary.Write("Evolution", Evolution);
//		dictionary.Write("AniminId", (int)PlayerAniminId);
//		dictionary.Write("AniminEvolutionId", (int)AniminEvolutionId);
//		dictionary.Write("ZefTokens", ZefTokens);
//	}
//
//
//	public void Load(SaveLoadDictionary dictionary)
//	{
//		dictionary.ReadFloat("Hungry", ref hungry);
//		dictionary.ReadFloat("Fitness", ref fitness);
//		dictionary.ReadFloat("Evolution", ref evolution);
//		dictionary.ReadAniminId("AniminId", ref PlayerAniminId);
//		dictionary.ReadAniminEvolutionId("AniminEvolutionId", ref AniminEvolutionId);
//		dictionary.ReadInt("ZefTokens", ref ZefTokens);
//	
//	}
}