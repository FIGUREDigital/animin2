using UnityEngine;
using System.Collections;
using Phi;

public class MyCollectionPage : MonoBehaviour 
{
	public InventoryItemUIIcon icon;
	public GameObject locked;
	ItemDefinition def;
	UIPages pages;
	int curIndex = -1;
	UIItemAlbum album;

	
	void OnSetUpPage(UIPages.SetupPage setupPage)
	{
		pages = setupPage.pages;
		album = pages.userData as UIItemAlbum;		
		curIndex = setupPage.page;
		UpdateVisuals();
	}
	
	void UpdateVisuals()
	{
		if (album == null) return;
		icon.ItemDef = (curIndex >= 0 && curIndex < album.definitions.Count) ? album.definitions [curIndex] : null;
		locked.SetActive (!ProfilesManagementScript.Instance.CurrentProfile.Inventory.OwnItem (icon.ItemDef.Id));
	}

	/*
	public void Init()
	{
		if (helmets == null || helmets.Length == 0)
		{
			helmets = GetComponentsInChildren<FEHelmet>();
		}
	}
	
	void Start()
	{
		Init();
		if (curIndex >= 0)
		{
			UpdateVisuals();
		}
	}


*/
}
