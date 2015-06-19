using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
public class InventoryItemControls : MonoBehaviour {

	[SerializeField]
	private TextMeshProUGUI mText;
	private GameObject CountPanel;
	public Image image;
	public CaringPageControls caringPage;
	private ItemDefinition item;
	public ItemDefinition Item
	{
		set
		{
			item = value;
			image.sprite = item.SpriteName;
			Count();
		}
	}
	int count;

	public void OnClick()
	{
		caringPage.SetIcon(item.ItemType, item.SpriteName);
		GameObject icon = caringPage.GetIcon (item.ItemType);
		icon.GetComponent<InterfaceItemLinkToModelScript>().item = item;
		caringPage.CloseInventory();
	}

    void Count()
    {
		InventoryItemData data = ProfilesManagementScript.Instance.CurrentAnimin.GetItemData(item.Id);
		int count = data != null ? data.Count : 0;
        mText.text = count.ToString ();
        CountPanel = mText.gameObject.transform.parent.gameObject;
        CountPanel.SetActive(count > 1);
    }
}
