using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class InventoryControls : MonoBehaviour 
{
	private const string PREFAB_PATH = "Prefabs/UI/ItemButton";
	[SerializeField]
	private TextMeshProUGUI m_InventoryLabel;
	public TextMeshProUGUI InventoryLabel
	{
		get
		{
			return m_InventoryLabel;
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
		}
	}

	[SerializeField]
	private GameObject itemButton;

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
				if(go != itemButton)
				{
					Destroy(go);
				}
				else
				{
					go.SetActive(false);
				}
			}
			
			m_CurrentDisplayed.Clear ();
		}
	}
	void Populate(PopupItemType type)
	{
		Clear ();
		
		m_CaringPage = transform.parent.parent.GetComponent<CaringPageControls> ();
		m_CurrentDisplayed = new List<GameObject> ();		
		itemButton.SetActive (false);
//		int count = 0;

		List<InventoryItemId> ids = new List<InventoryItemId> ();

		for (int i=0; i<ProfilesManagementScript.Instance.CurrentProfile.Inventory.Count; ++i)
		{
			Inventory.Entry entry = ProfilesManagementScript.Instance.CurrentProfile.Inventory.Get(i);
			if(entry.Location != Inventory.Locations.Inventory) continue;
			ItemDefinition def = entry.Definition;
			if(def.ItemType != type) continue;
			if(ids.Contains (def.Id)) continue;
			ids.Add (def.Id);

			InventoryItemControls button;
			if(m_CurrentDisplayed.Count == 0)
			{
				itemButton.SetActive(true);
				button = itemButton.GetComponent<InventoryItemControls>();
			}
			else
			{
				button = ((GameObject)Instantiate(itemButton)).GetComponent<InventoryItemControls>();
			}
			button.Item = entry;
			button.caringPage = m_CaringPage;
			m_CurrentDisplayed.Add(button.gameObject);
			button.transform.parent = m_InventoryGrid.transform;

            button.transform.localScale = Vector3.one * 0.8f;
		}
		float spacing = 0;
		int columns = Mathf.CeilToInt(m_CurrentDisplayed.Count * 0.5f);
		if (columns > 1)
		{
			spacing = m_InventoryGrid.spacing.x * (columns - 1);
		}
		RectTransform rt = m_InventoryGrid.transform as RectTransform;
		Vector2 size = rt.sizeDelta; 
		size.x = spacing + m_InventoryGrid.cellSize.x * columns;
		rt.sizeDelta = size;
		Vector2 pos = rt.anchoredPosition;
		// Now position so that the first item is at the left of the window fi we have 5 or more columns otherwise it's centered
		if (columns <= 4)
		{
			pos.x = 0;
		}
		else
		{
			pos.x = ((float)(columns - 4))*0.5f;
		}
		rt.anchoredPosition = pos;
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
