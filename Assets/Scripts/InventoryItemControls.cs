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
	public InventoryItemBankData data;
	public InventoryItemBankData Data
	{
		set
		{
			data = value;
			image.sprite = data.SpriteName;
			Count();
		}
	}
	int count;

	public void OnClick()
	{
		caringPage.SetIcon(data.ItemType, data.SpriteName);
		GameObject icon = caringPage.GetIcon (data.ItemType);
		icon.GetComponent<InterfaceItemLinkToModelScript>().Item3DPrefab = data.PrefabId;
		icon.GetComponent<InterfaceItemLinkToModelScript>().ItemID = data.Id;
		caringPage.CloseInventory();
	}

    void Count()
    {
        for(int i = 0; i < ProfilesManagementScript.Instance.CurrentAnimin.Inventory.Count; i++)
        {
            InventoryItemData InvData = ProfilesManagementScript.Instance.CurrentAnimin.Inventory[i];
            if(data.Id == InvData.Id)
            {
                count = InvData.Count;
            }
        }
        mText.text = count.ToString ();
        CountPanel = mText.gameObject.transform.parent.gameObject;
        CountPanel.SetActive(count > 1);
    }
}
