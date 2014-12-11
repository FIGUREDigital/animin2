using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InventoryItemControls : MonoBehaviour {

	[SerializeField]
	private Text mText;
	private GameObject CountPanel;
	public CaringPageControls caringPage;
	public InventoryItemBankData data;
	int count;
	public void OnEnable()
	{
        Invoke("Count",0.02f);
	}

	public void OnClick()
	{
		caringPage.SetIcon(data.ItemType, data.SpriteName);
		GameObject icon = caringPage.GetIcon (data.ItemType);
		icon.GetComponent<InterfaceItemLinkToModelScript>().Item3DPrefab = data.PrefabId;
		icon.GetComponent<InterfaceItemLinkToModelScript>().ItemID = data.Id;
		caringPage.CloseInventory();
	}

    void Update()
    {

    }

    void Count()
    {
        for(int i = 0; i < ProfilesManagementScript.Singleton.CurrentAnimin.Inventory.Count; i++)
        {
            InventoryItemData InvData = ProfilesManagementScript.Singleton.CurrentAnimin.Inventory[i];
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
