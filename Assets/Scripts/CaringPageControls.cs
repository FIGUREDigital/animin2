using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum InventoryPages
{
	Food,
	Items,
	Medicine,
	Count
}
public class CaringPageControls : MonoBehaviour {

	[SerializeField]
    private RectTransform Inventory;
	private InventoryControls mInventoryControls;
    [SerializeField]
	private RectTransform Indicator;
	private bool InventoryOpen = false;
	private InventoryPages CurrentPage;
	[SerializeField]
	private UnityEngine.UI.Image Icon1;
	[SerializeField]
	private UnityEngine.UI.Image Icon2;
	[SerializeField]
	private UnityEngine.UI.Image Icon3;

	void Start()
	{
		mInventoryControls = Inventory.GetComponent<InventoryControls> ();
		bool FoodIconSet = false;
		bool ItemIconSet = false;
		bool MediIconSet = false;
		for (int i=0; i<ProfilesManagementScript.Singleton.CurrentAnimin.Inventory.Count; ++i) 
		{
			if(InventoryItemData.Items[(int)ProfilesManagementScript.Singleton.CurrentAnimin.Inventory[i].Id].ItemType == PopupItemType.Food && !FoodIconSet)
			{
				Icon1.sprite = InventoryItemData.Items[(int)ProfilesManagementScript.Singleton.CurrentAnimin.Inventory[i].Id].SpriteName;
				FoodIconSet = true;
			} 
			else if(InventoryItemData.Items[(int)ProfilesManagementScript.Singleton.CurrentAnimin.Inventory[i].Id].ItemType == PopupItemType.Item && !ItemIconSet)
			{
				Icon2.sprite = InventoryItemData.Items[(int)ProfilesManagementScript.Singleton.CurrentAnimin.Inventory[i].Id].SpriteName;
				ItemIconSet = true;
			}
			else if(InventoryItemData.Items[(int)ProfilesManagementScript.Singleton.CurrentAnimin.Inventory[i].Id].ItemType == PopupItemType.Medicine && !MediIconSet)
			{
				Icon3.sprite = InventoryItemData.Items[(int)ProfilesManagementScript.Singleton.CurrentAnimin.Inventory[i].Id].SpriteName;
				MediIconSet = true;
			}
		}
	}
	public void StatsButton()
	{
		UiPages.Next (Pages.StatsPage);
	}
	public void MinigameButton()
	{
		UiPages.Next (Pages.MinigamesPage);
	}
	public void PhotoButton()
	{
	}
	public void FoodButton()
	{
		if(!InventoryOpen)
		{
			InventoryOpen = true;
		}
		else if(CurrentPage ==  InventoryPages.Food)
		{
			InventoryOpen = false;
		}
		else 
		{
			SwitchInventory(CurrentPage);
		}
		CurrentPage = InventoryPages.Food;
		mInventoryControls.Init (CurrentPage);
		Inventory.gameObject.SetActive(InventoryOpen);
        Indicator.localPosition = new Vector2(-120, Indicator.localPosition.y);
   
	}
	public void ItemsButton()
	{
		if(!InventoryOpen)
		{
			InventoryOpen = true;
		}
		else if(CurrentPage == InventoryPages.Items)
		{
			InventoryOpen = false;
		}
		else 
		{
			SwitchInventory(CurrentPage);
		}
		CurrentPage = InventoryPages.Items;
		mInventoryControls.Init (CurrentPage);
		Inventory.gameObject.SetActive(InventoryOpen);
        Indicator.localPosition = new Vector2(0, Indicator.localPosition.y);
	}
	public void MedicineButton()
	{
		if(!InventoryOpen)
		{
			InventoryOpen = true;
		}
		else if(CurrentPage == InventoryPages.Medicine)
		{
			InventoryOpen = false;
		}
		else 
		{
			SwitchInventory(CurrentPage);
		}
		CurrentPage = InventoryPages.Medicine;
		mInventoryControls.Init (CurrentPage);
        Inventory.gameObject.SetActive(InventoryOpen);
        Indicator.localPosition = new Vector2(120, Indicator.localPosition.y);

        
	}

	private void SwitchInventory(InventoryPages page)
	{
		InventoryOpen = false;
		switch (page) 
		{
		case InventoryPages.Food:
			FoodButton();
			break;
		case InventoryPages.Items:
			ItemsButton();
			break;
		case InventoryPages.Medicine:
			MedicineButton();
			break;
		default:
			break;
		}
	}

	public void BroomButton()
	{
	}

	public void SetIcon(PopupItemType p, Sprite s)
	{
		switch(p)
		{
		case PopupItemType.Food:
			Icon1.sprite = s;
			break;
		case PopupItemType.Item:
			Icon2.sprite = s;
			break;
		case PopupItemType.Medicine:
			Icon3.sprite = s;
			break;
		default:
			break;
		}

	}

}
