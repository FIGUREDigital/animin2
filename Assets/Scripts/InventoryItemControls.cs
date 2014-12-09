using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InventoryItemControls : MonoBehaviour {

	[SerializeField]
	private Text mText;
	private GameObject CountPanel;
	public CaringPageControls caringPage;
	public InventoryControls inventoryControls;
	public InventoryItemBankData data;
	int count;
	public void OnEnable()
	{
		for(int i = 0; i < ProfilesManagementScript.Singleton.CurrentAnimin.Inventory.Count; i++)
		{
			InventoryItemData data = ProfilesManagementScript.Singleton.CurrentAnimin.Inventory[i];
			if(data.Id == data.Id)
			{
				count = data.Count;
			}
		}
		mText.text = count.ToString ();
		CountPanel = mText.gameObject.transform.parent.gameObject;
		CountPanel.SetActive(count > 1);
	}
	public void OnClick()
	{
		caringPage.SetIcon(data.ItemType, data.SpriteName);
		GameObject icon = caringPage.GetIcon (data.ItemType);
		icon.GetComponent<InterfaceItemLinkToModelScript>().Item3DPrefab = data.PrefabId;
		icon.GetComponent<InterfaceItemLinkToModelScript>().ItemID = data.Id;
		caringPage.CloseInventory();
	}
}
