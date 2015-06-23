using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
public class InventoryItemControls : InventoryItemUIIcon {

	[SerializeField]
	private TextMeshProUGUI mText;
	private GameObject CountPanel;
	public CaringPageControls caringPage;
	override public Inventory.Entry Item
	{
		set
		{
			base.Item = value;
			Count();
		}
		get
		{
			return base.Item;
		}
	}
	int count;

	public void OnClick()
	{
		caringPage.SetIcon(Item);
		caringPage.CloseInventory();
	}

    void Count()
    {
		int count = ProfilesManagementScript.Instance.CurrentProfile.Inventory.GetNumItemsInInventory(Item.Id);
        mText.text = count.ToString ();
        CountPanel = mText.gameObject.transform.parent.gameObject;
        CountPanel.SetActive(count > 1);
    }
}
