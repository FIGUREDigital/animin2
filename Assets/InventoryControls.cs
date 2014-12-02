using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryControls : MonoBehaviour 
{
	private const string PREFAB_PATH = "Prefabs/UI/ItemButton";
	[SerializeField]
	private Text m_InventoryLabel;
	public Text InventoryLabel
	{
		get
		{
			return m_InventoryLabel;
		}
		set
		{
			m_InventoryLabel = value;
		}
	}
	[SerializeField]
	private ScrollRect m_InventoryRect;
	[SerializeField]
	private GridLayoutGroup m_InventoryGrid;
	private List<GameObject> mCurrentDisplayed;

	private InventoryPages m_CurrentMode;
	public InventoryPages CurrentMode
	{
		get
		{
			return m_CurrentMode;
		}
		set
		{
			m_CurrentMode = value;
		}
	}
	
	private Object prefabButton;

	void Start()
	{
		m_CurrentMode = InventoryPages.Food;
	}
	void Clear()
	{
		if(mCurrentDisplayed != null)
		{
			foreach(GameObject go in mCurrentDisplayed)
			{
				Destroy(go);
			}
		}
		mCurrentDisplayed.Clear ();
	}
	void Populate(PopupItemType type)
	{
		Clear ();
		mCurrentDisplayed = new List<GameObject> ();
		for (int i=0; i<ProfilesManagementScript.Singleton.CurrentAnimin.Inventory.Count; ++i)
		{
			if(InventoryItemData.Items[(int)ProfilesManagementScript.Singleton.CurrentAnimin.Inventory[i].Id].ItemType == type)
			{
				InventoryItemBankData data = InventoryItemData.Items[(int)ProfilesManagementScript.Singleton.CurrentAnimin.Inventory[i].Id];
				InventoryItemControls button = (InventoryItemControls)Instantiate(prefabButton);
				button.ID = data.Id;
				button.GetComponent<UnityEngine.UI.Image>().sprite = data.SpriteName;
				mCurrentDisplayed.Add(button.gameObject);
				button.transform.parent = m_InventoryGrid.transform;
			}
		}
	}

	void OnEnable()
	{
	}

	public void Init (InventoryPages CurrentPage) 
	{
		CurrentMode = CurrentPage;
		InventoryLabel.text = CurrentPage.ToString ();
		m_InventoryRect.horizontalNormalizedPosition = -0.2f;
	}
}
