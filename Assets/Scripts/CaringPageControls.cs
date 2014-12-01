using UnityEngine;
using System.Collections;
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
    [SerializeField]
	private RectTransform Indicator;
	bool InventoryOpen = false;
	InventoryPages CurrentPage;


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
		else // switch page
		{
			//InventoryOpen = false;
		}
		Inventory.gameObject.SetActive(InventoryOpen);
        Indicator.localPosition = new Vector2(-120, Indicator.localPosition.y);
        CurrentPage = InventoryPages.Food;
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
		else // switch page
		{
			//InventoryOpen = false;
		}
		Inventory.gameObject.SetActive(InventoryOpen);

        Indicator.localPosition = new Vector2(0, Indicator.localPosition.y);
        CurrentPage = InventoryPages.Items;
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
		else // switch page
		{
			//InventoryOpen = false;
		}
        Inventory.gameObject.SetActive(InventoryOpen);
        Indicator.localPosition = new Vector2(120, Indicator.localPosition.y);
        CurrentPage = InventoryPages.Medicine;
	}
	public void BroomButton()
	{
	}

}
