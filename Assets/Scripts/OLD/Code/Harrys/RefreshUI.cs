using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RefreshUI : MonoBehaviour {

	// Use this for initialization
	public void Start(){
		//This method looks through the inventory and applies a graphic to the tab.
		
		bool FoodIconSet = false;
		bool ItemIconSet = false;
		bool MediIconSet = false;
		for (int i=0; i<ProfilesManagementScript.Singleton.CurrentAnimin.Inventory.Count; ++i) {
			//So. InventoryItemData.Items is a list of InventoryItemBankData. This is not the same as InventoryItemData, which is what is saved in the Animin Profile Data.
			//Therefore we have to run the inventory type through the InventoryItemDataBank to find out its type.
			if(InventoryItemData.Items[(int)ProfilesManagementScript.Singleton.CurrentAnimin.Inventory[i].Id].ItemType == PopupItemType.Food && !FoodIconSet){
				//UIGlobalVariablesScript.Singleton.FoodButton.GetComponent<Button>().normalSprite = InventoryItemData.Items[(int)ProfilesManagementScript.Singleton.CurrentAnimin.Inventory[i].Id].SpriteName;
				UIGlobalVariablesScript.Singleton.FoodButton.GetComponent<InterfaceItemLinkToModelScript>().ItemID = ProfilesManagementScript.Singleton.CurrentAnimin.Inventory[i].Id;
				FoodIconSet = true;
			} else if(InventoryItemData.Items[(int)ProfilesManagementScript.Singleton.CurrentAnimin.Inventory[i].Id].ItemType == PopupItemType.Item && !ItemIconSet){
				//UIGlobalVariablesScript.Singleton.ItemsButton.GetComponent<Button>().normalSprite = InventoryItemData.Items[(int)ProfilesManagementScript.Singleton.CurrentAnimin.Inventory[i].Id].SpriteName;
				UIGlobalVariablesScript.Singleton.ItemsButton.GetComponent<InterfaceItemLinkToModelScript>().ItemID = ProfilesManagementScript.Singleton.CurrentAnimin.Inventory[i].Id;
				ItemIconSet = true;
			} else if(InventoryItemData.Items[(int)ProfilesManagementScript.Singleton.CurrentAnimin.Inventory[i].Id].ItemType == PopupItemType.Medicine && !MediIconSet){
				//UIGlobalVariablesScript.Singleton.MedicineButton.GetComponent<Button>().normalSprite = InventoryItemData.Items[(int)ProfilesManagementScript.Singleton.CurrentAnimin.Inventory[i].Id].SpriteName;
				UIGlobalVariablesScript.Singleton.MedicineButton.GetComponent<InterfaceItemLinkToModelScript>().ItemID = ProfilesManagementScript.Singleton.CurrentAnimin.Inventory[i].Id;
				MediIconSet = true;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
