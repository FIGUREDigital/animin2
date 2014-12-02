using UnityEngine;
using System.Collections;

public class InventoryItemControls : MonoBehaviour {

	public CaringPageControls caringPage;
	public InventoryItemBankData data;
	public void OnClick()
	{
		caringPage.SetIcon(data.ItemType, data.SpriteName);
	}
}
