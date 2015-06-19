using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;


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

	public List<InventoryItemData> Inventory = new List<InventoryItemData>();

	public InventoryItemData GetItemData(InventoryItemId id)
	{
		for (int i = 0; i < Inventory.Count; i++) 
		{
			InventoryItemData InvData = ProfilesManagementScript.Instance.CurrentAnimin.Inventory [i];
			if (id == Inventory[i].Id)
			{
				return Inventory[i];
			}
		}
		return null;
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
	public static bool InventoryUpdated;
	
	private int age;
	private bool audioIsOn;
	public static float happy;
	private float hungry;
	private float fitness;
	private float evolution;
	private float health;
	public DateTime lastPlay;
			
    public CaringScreenItem[] m_CaringScreenItems;

    public void SaveCaringScreenItem(GameObject[] objects, GameObject holding){
        Debug.Log("Objects.length : [" + objects.Length + "];");
        //m_CaringScreenItems = new CaringScreenItem[objects.Length];
        List<CaringScreenItem> ItemList = new List<CaringScreenItem>();
        if (holding != null)
        {
            Vector3 pos = holding.transform.localPosition;
            pos.y = 0;
            holding.transform.localPosition = pos;
            Save(ItemList, holding);
        }
        for (int i = 0; i < objects.Length; i++)
        {
            Save(ItemList, objects[i]);
        }
        m_CaringScreenItems = ItemList.ToArray();
    }

    void Save(List<CaringScreenItem> ItemList, GameObject o)
    {
        if (o == null) return;
        Debug.Log("Saving Item : [" + o + "];");
		ItemDefinition item = o.GetComponent<ItemDefinition>();
        //AH Avoid saving items that have a none ID, causes an exception on loading - Note Zef tokens use none
        if (item != null && item.Id != InventoryItemId.None)
        {
            ItemList.Add(new CaringScreenItem(item.Id, o.transform.position));
        }
    }

    public GameObject[] LoadCaringScreenItem(){
        Debug.Log("LoadCaringScreenItem called! m_CaringScreenItems : [" + m_CaringScreenItems + "];");
        if (m_CaringScreenItems == null)
            return null;
        GameObject[] returnGOs = new GameObject[m_CaringScreenItems.Length];
        for (int i = 0; i < m_CaringScreenItems.Length; i++)
        {
//            Debug.Log ("Prefab : [" + InventoryItemData.Items [(int)m_CaringScreenItems [i].Id].PrefabId + "];");
			ItemDefinition def =  ItemDefinition.GetDefinition(m_CaringScreenItems[i].Id);
			if(def !=null)
			{
				returnGOs[i] = def.Create();

	            returnGOs[i].transform.parent = UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>().ActiveWorld.transform;

	            //returnGOs[i].transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
	            //returnGOs[i].transform.localScale *= 10;
	            returnGOs[i].transform.localRotation = Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0);

	            returnGOs[i].transform.position = m_CaringScreenItems[i].Position;

	            Debug.Log("Scale of [" + returnGOs[i].name + "] is [" + returnGOs[i].transform.localScale + "];");
			}
			else
			{
				Debug.LogError ("Warning caring system index "+i+" ("+(m_CaringScreenItems[i].Id.ToString ())+") has a null item");
			}
        }
        return returnGOs;
    }




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

	public void AddItemToInventory(InventoryItemId id, int count)
	{
		ItemDefinition def = ItemDefinition.GetDefinition(id);
		if (def == null) {
						Debug.Log ("Something has gone terribely wrong");
						return;
				}
		if(!((def.ItemType == PopupItemType.Food) ||
		     (def.ItemType == PopupItemType.Medicine) ||
		     (def.ItemType == PopupItemType.Item)))
		    {
			Debug.Log ("Cannot add item! /nID : [" + id + "]; Data : [" + def.ItemType + "];");
			return;
		}
        int key = 0;
        bool doThing = false;
        if (id == InventoryItemId.EDMKsynth)
        {
            key = 0;
            doThing = true;
        }
        if (id == InventoryItemId.EDM808)
        {
            key = 8;
            doThing = true;

        }
        if (id == InventoryItemId.EDMJuno)
        {
            key = 16;
            doThing = true;
        }
        if (doThing)
        {
            for (int i = key; i < key + 8; i++)
            {
                EDMMixerScript.Singleton.KeysOn[i] = false;
            }
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
				UiPages.GetPage (Pages.CaringPage).GetComponent<CaringPageControls> ().PopulateButtons ();
				/*
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
        */
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
			if(Inventory[i].Definition.ItemType == type)
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
