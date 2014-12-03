using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public enum InventoryPages
{
	Food,
	Items,
	Medicine,
	Count
}
public class CaringPageControls : MonoBehaviour {

	[SerializeField]
    private RectTransform Inventory;
	private InventoryControls mInventoryControls;
    [SerializeField]
	private RectTransform Indicator;
	private bool InventoryOpen = false;
	private InventoryPages CurrentPage;
	[SerializeField]
	private UnityEngine.UI.Image Icon1;
	[SerializeField]
	private UnityEngine.UI.Image Icon2;
	[SerializeField]
	private UnityEngine.UI.Image Icon3;
	[SerializeField]
	private GameObject PhotoSaved;
	
	void Start()
	{
		mInventoryControls = Inventory.GetComponent<InventoryControls> ();
		PopulateButtons ();
	}
	void PopulateButtons ()
	{
		Debug.Log ("Populating buttons");
		bool FoodIconSet = false;
		bool ItemIconSet = false;
		bool MediIconSet = false;
		for (int i=0; i<ProfilesManagementScript.Singleton.CurrentAnimin.Inventory.Count; ++i) 
		{
			if(InventoryItemData.Items[(int)ProfilesManagementScript.Singleton.CurrentAnimin.Inventory[i].Id].ItemType == PopupItemType.Food && !FoodIconSet)
			{
				InventoryItemBankData data = InventoryItemData.Items[(int)ProfilesManagementScript.Singleton.CurrentAnimin.Inventory[i].Id];
				Icon1.sprite = data.SpriteName;
				Icon1.GetComponent<InterfaceItemLinkToModelScript>().Item3DPrefab = data.PrefabId;
				Icon1.GetComponent<InterfaceItemLinkToModelScript>().ItemID = data.Id;
				FoodIconSet = true;
				Debug.Log ("Food buttons set to " + data.Id.ToString() +"\n Currently there are "+ ProfilesManagementScript.Singleton.CurrentAnimin.Inventory[i].Count + " " + data.Id.ToString());
			} 
			else if(InventoryItemData.Items[(int)ProfilesManagementScript.Singleton.CurrentAnimin.Inventory[i].Id].ItemType == PopupItemType.Item && !ItemIconSet)
			{
				InventoryItemBankData data = InventoryItemData.Items[(int)ProfilesManagementScript.Singleton.CurrentAnimin.Inventory[i].Id];
				Icon2.sprite = data.SpriteName;
				Icon2.GetComponent<InterfaceItemLinkToModelScript>().Item3DPrefab = data.PrefabId;
				Icon2.GetComponent<InterfaceItemLinkToModelScript>().ItemID = data.Id;
				ItemIconSet = true;
				Debug.Log ("Item buttons set to " + data.Id.ToString() +"\n Currently there are "+ ProfilesManagementScript.Singleton.CurrentAnimin.Inventory[i].Count + " " + data.Id.ToString());

			}
			else if(InventoryItemData.Items[(int)ProfilesManagementScript.Singleton.CurrentAnimin.Inventory[i].Id].ItemType == PopupItemType.Medicine && !MediIconSet)
			{
				InventoryItemBankData data = InventoryItemData.Items[(int)ProfilesManagementScript.Singleton.CurrentAnimin.Inventory[i].Id];
				Icon3.sprite = data.SpriteName;
				Icon3.GetComponent<InterfaceItemLinkToModelScript>().Item3DPrefab = data.PrefabId;
				Icon3.GetComponent<InterfaceItemLinkToModelScript>().ItemID = data.Id;
				MediIconSet = true;
				Debug.Log ("Medicine buttons set to " + data.Id.ToString() +"\n Currently there are "+ ProfilesManagementScript.Singleton.CurrentAnimin.Inventory[i].Count + " " + data.Id.ToString());

			}
		}
	}
	public void StatsButton()
	{
		UiPages.Next (Pages.StatsPage);
	}
	public void MinigameButton()
	{
		UiPages.Next (Pages.MinigamesPage);
	}
	public void PhotoButton()
	{
		if(Application.isEditor)
		{
			Application.CaptureScreenshot("screenshot.png");
		}
		else
		{
			
			string screenshotName = "screenshot"  + DateTime.Now.ToString("s") + ".png";
			Debug.Log("Saving photo to: " + screenshotName);
			#if UNITY_IOS
			StartCoroutine( EtceteraBinding.takeScreenShot( screenshotName, imagePath =>
			                                               {EtceteraBinding.saveImageToPhotoAlbum (imagePath);}) );
			#elif UNITY_ANDROID
			string path = Application.persistentDataPath + screenshotName;
			Application.CaptureScreenshot(screenshotName);
			Debug.Log("Moving file from " + path);
			bool saved = EtceteraAndroid.saveImageToGallery(path,screenshotName);
			if(saved)
			{
				Debug.Log("File moved");
			}
			else
			{
				Debug.Log("File moved fail!");
			}
			#endif
		}
		Invoke("PopPhotoSaved",0.3f);
	}
	
	void PopPhotoSaved()
	{
				if (PhotoSaved != null && PhotoSaved.GetComponent<PhotoFadeOut> () != null) {
					PhotoSaved.gameObject.SetActive(true);
				}
	}

	public void FoodButton()
	{
		if(!InventoryOpen)
		{
			InventoryOpen = true;
		}
		else if(CurrentPage ==  InventoryPages.Food)
		{
			InventoryOpen = false;
		}
		else 
		{
			SwitchInventory(CurrentPage);
		}
		CurrentPage = InventoryPages.Food;
		mInventoryControls.Init (CurrentPage);
		Inventory.gameObject.SetActive(InventoryOpen);
        Indicator.localPosition = new Vector2(-120, Indicator.localPosition.y);
   
	}
	public void ItemsButton()
	{
		if(!InventoryOpen)
		{
			InventoryOpen = true;
		}
		else if(CurrentPage == InventoryPages.Items)
		{
			InventoryOpen = false;
		}
		else 
		{
			SwitchInventory(CurrentPage);
		}
		CurrentPage = InventoryPages.Items;
		mInventoryControls.Init (CurrentPage);
		Inventory.gameObject.SetActive(InventoryOpen);
        Indicator.localPosition = new Vector2(0, Indicator.localPosition.y);
	}
	public void MedicineButton()
	{
		if(!InventoryOpen)
		{
			InventoryOpen = true;
		}
		else if(CurrentPage == InventoryPages.Medicine)
		{
			InventoryOpen = false;
		}
		else 
		{
			SwitchInventory(CurrentPage);
		}
		CurrentPage = InventoryPages.Medicine;
		mInventoryControls.Init (CurrentPage);
        Inventory.gameObject.SetActive(InventoryOpen);
        Indicator.localPosition = new Vector2(120, Indicator.localPosition.y);

        
	}

	private void SwitchInventory(InventoryPages page)
	{
		InventoryOpen = false;
		switch (page) 
		{
		case InventoryPages.Food:
			FoodButton();
			break;
		case InventoryPages.Items:
			ItemsButton();
			break;
		case InventoryPages.Medicine:
			MedicineButton();
			break;
		default:
			break;
		}
	}

	public void CloseInventory()
	{
		Inventory.gameObject.SetActive(false);
	}

	public void BroomButton()
	{
		CharacterProgressScript script = UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript> ();
		
		for (int i = 0; i < script.GroundItems.Count; ++i) {
			if (script.GroundItems [i].GetComponent<UIPopupItemScript> () != null) {
				if (script.GroundItems [i].GetComponent<UIPopupItemScript> ().Type == PopupItemType.Token) {
					continue;
				} else {
					ProfilesManagementScript.Singleton.CurrentAnimin.AddItemToInventory (script.GroundItems [i].GetComponent<UIPopupItemScript> ().Id, 1);
				}
			}
			Destroy (script.GroundItems [i]);
		}
		
		script.GroundItems.Clear ();
		UIGlobalVariablesScript.Singleton.SoundEngine.Play (GenericSoundId.CleanPooPiss);
		UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript> ().HidePopupMenus ();
		
		for (int i = 0; i < EDMMixerScript.Singleton.KeysOn.Length; ++i) {
			EDMMixerScript.Singleton.KeysOn [i] = false;
		}
	}

	public void SetIcon(PopupItemType p, Sprite s)
	{
		switch(p)
		{
		case PopupItemType.Food:
			Icon1.sprite = s;
			break;
		case PopupItemType.Item:
			Icon2.sprite = s;
			break;
		case PopupItemType.Medicine:
			Icon3.sprite = s;
			break;
		default:
			break;
		}

	}

	public GameObject GetIcon (PopupItemType p)
	{
		switch(p)
		{
		case PopupItemType.Food:
			return Icon1.gameObject;
			break;
		case PopupItemType.Item:
			return Icon2.gameObject;
			break;
		case PopupItemType.Medicine:
			return Icon3.gameObject;
			break;
		default:
			break;
		}
		return null;

	}
	void Update()
	{

		if(PersistentData.InventoryUpdated)
		{
			PersistentData.InventoryUpdated = false;
			PopulateButtons();
		}
	}
}
