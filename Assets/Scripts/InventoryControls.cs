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
	private List<GameObject> m_CurrentDisplayed;
	private CaringPageControls m_CaringPage;

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
			prefabButton = Resources.Load(PREFAB_PATH);
		}
	}
	
	private Object prefabButton;

	void Start()
	{
		m_CurrentMode = InventoryPages.Food;
	}
	void Clear()
	{
		if(m_CurrentDisplayed != null)
		{
			foreach(GameObject go in m_CurrentDisplayed)
			{
				Destroy(go);
			}
			
			m_CurrentDisplayed.Clear ();
		}
	}
	void Populate(PopupItemType type)
	{
		Clear ();
		
		m_CaringPage = transform.parent.parent.GetComponent<CaringPageControls> ();
		m_CurrentDisplayed = new List<GameObject> ();
		for (int i=0; i<ProfilesManagementScript.Singleton.CurrentAnimin.Inventory.Count; ++i)
		{
			if(InventoryItemData.Items[(int)ProfilesManagementScript.Singleton.CurrentAnimin.Inventory[i].Id].ItemType == type)
			{
				InventoryItemBankData data = InventoryItemData.Items[(int)ProfilesManagementScript.Singleton.CurrentAnimin.Inventory[i].Id];
				InventoryItemControls button = ((GameObject)Instantiate(prefabButton)).GetComponent<InventoryItemControls>();
				button.data = data;
				button.caringPage = m_CaringPage;
				button.GetComponent<UnityEngine.UI.Image>().sprite = data.SpriteName;
				m_CurrentDisplayed.Add(button.gameObject);
				button.transform.parent = m_InventoryGrid.transform;

                button.transform.localScale = Vector3.one * 0.8f;
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
		PopupItemType type = PopupItemType.Count;
		switch (CurrentPage) 
		{
		case InventoryPages.Food:
			type = PopupItemType.Food;
			break;
		case InventoryPages.Items:
			type = PopupItemType.Item;
			break;
		case InventoryPages.Medicine:
			type = PopupItemType.Medicine;
			break;
		default:
			break;
		}
		Populate (type);
	}
}
