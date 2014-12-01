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
			InventoryOpen = false;
		}
		Inventory.gameObject.SetActive(InventoryOpen);
		Indicator.anchorMin = new Vector2 (110, Indicator.anchorMin.y);
		Indicator.anchorMax = new Vector2 (350, Indicator.anchorMax.y);
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
			InventoryOpen = false;
		}
		Inventory.gameObject.SetActive(InventoryOpen);
		
		Indicator.anchorMin = new Vector2 (90, Indicator.anchorMin.y);
		Indicator.anchorMax = new Vector2 (90, Indicator.anchorMax.y);
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
			InventoryOpen = false;
		}
		Inventory.gameObject.SetActive(InventoryOpen);
		Indicator.anchorMin = new Vector2 (350, Indicator.anchorMin.y);
		Indicator.anchorMax = new Vector2 (110, Indicator.anchorMax.y);
	}
	public void BroomButton()
	{
	}

}
