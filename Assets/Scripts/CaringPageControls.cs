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


    [SerializeField]
    private GameObject m_StereoUI;
    public GameObject StereoUI{ get { return m_StereoUI; } }
    [SerializeField]
    private GameObject m_AlarmUI;
    public GameObject AlarmUI{ get { return m_AlarmUI; } }
    [SerializeField]
    private GameObject m_LightbulbUI;
    public GameObject LightbulbUI{ get { return m_LightbulbUI; } }
    [SerializeField]
    private GameObject m_EDMBoxUI;
    public GameObject EDMBoxUI{ get { return m_EDMBoxUI; } }
    [SerializeField]
    private GameObject m_JunoUI;
    public GameObject JunoUI{ get { return m_JunoUI; } }
    [SerializeField]
    private GameObject m_PianoUI;
    public GameObject PianoUI{ get { return m_PianoUI; } }

    [SerializeField]
    private TutorialHandler m_TutorialHandler;
    public TutorialHandler TutorialHandler{
        get {
            if (m_TutorialHandler.Inited) m_TutorialHandler.Init();
            return m_TutorialHandler;
        }
    }

    [SerializeField]
    private RectTransform m_Triangle;
    private Vector2 m_TriangleHeight;

    private GameObject m_TargetItem;
    public GameObject TargetItem{
        get { return m_TargetItem; }
        set { m_TargetItem = value;}
    }

    private GameObject[] m_PopupUIs;
    private GameObject[] PopupUIs{ get { return m_PopupUIs; } }

    private LighbulbSwitchOnOffScript m_LightScript;
    private LighbulbSwitchOnOffScript LightScript{
        get {
            if (m_LightScript == null)
                m_LightScript = TargetItem.GetComponent<LighbulbSwitchOnOffScript>();
            return m_LightScript;
        }
    }
    private EDMBoxScript m_EDMScript;
    private EDMBoxScript EDMScript {
        get {
            if (m_EDMScript == null)
                m_EDMScript = TargetItem.GetComponent<EDMBoxScript>();
            return m_EDMScript;
        }
    }
	
	void Start()
	{
        m_PopupUIs = new GameObject[]{ AlarmUI, LightbulbUI, EDMBoxUI, JunoUI, PianoUI };
		mInventoryControls = Inventory.GetComponent<InventoryControls> ();
		PopulateButtons ();
        m_TriangleHeight = new Vector2(0,m_Triangle.sizeDelta.y);
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

    public void LightSwitch(){
        LighbulbSwitchOnOffScript script = TargetItem.GetComponent<LighbulbSwitchOnOffScript>();
        if (TargetItem.GetComponent<LighbulbSwitchOnOffScript>() != null)
        {
            script.Switch();
        }
    }
    public void JunoToggle(int Key){
        Debug.Log("JunoToggle");
        int k = 16 + Key;
        EDMMixerScript.Singleton.KeysOn[k] = !EDMMixerScript.Singleton.KeysOn[k];
    }
    public void EDMToggle(int Key){
        Debug.Log("EDMToggle");
        int k = 8 + Key;
        EDMMixerScript.Singleton.KeysOn[k] = !EDMMixerScript.Singleton.KeysOn[k];
    }
    public void PianoToggle(int Key){
        Debug.Log("PianoToggle");
        int k = 0 + Key;
        EDMMixerScript.Singleton.KeysOn[k] = !EDMMixerScript.Singleton.KeysOn[k];
    }

    public void DisappearAllItemUIs(){

        AlarmUI.SetActive(false);
        EDMBoxUI.SetActive(false);
        JunoUI.SetActive(false);
        LightbulbUI.SetActive(false);
        PianoUI.SetActive(false);
        StereoUI.SetActive(false);
    }

	void Update()
	{
		if(PersistentData.InventoryUpdated)
		{
			PersistentData.InventoryUpdated = false;
			PopulateButtons();
		}
        m_Triangle.gameObject.SetActive(false);
        if(m_TargetItem!=null){
            for (int i = 0; i < PopupUIs.Length; i++)
            {
                if (PopupUIs[i] != null && PopupUIs[i].activeInHierarchy)
                {
                    //this is your object that you want to have the UI element hovering over

                    //this is the ui element
                    RectTransform UI_Element = PopupUIs[i].GetComponent<RectTransform>();


                    //first you need the RectTransform component of your canvas
                    RectTransform CanvasRect=this.GetComponent<RectTransform>();

                    //then you calculate the position of the UI element
                    //0,0 for the canvas is at the center of the screen, whereas WorldToViewPortPoint treats the lower left corner as 0,0. Because of this, you need to subtract the height / width of the canvas * 0.5 to get the correct position.

                    Vector2 ViewportPosition=Camera.main.WorldToViewportPoint(m_TargetItem.transform.position);
                    Vector2 WorldObject_ScreenPosition=new Vector2(
                        ((ViewportPosition.x*CanvasRect.sizeDelta.x)-(CanvasRect.sizeDelta.x*0.5f)),
                        ((ViewportPosition.y*CanvasRect.sizeDelta.y)-(CanvasRect.sizeDelta.y*0.5f)));

                    //now you can set the position of the ui element
                    UI_Element.anchoredPosition=WorldObject_ScreenPosition + m_TriangleHeight;
                    m_Triangle.anchoredPosition = WorldObject_ScreenPosition + m_TriangleHeight;
                    m_Triangle.gameObject.SetActive(true);
                }
            }
        }
	}
}
