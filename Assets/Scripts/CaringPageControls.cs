using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using DG.Tweening;

public enum InventoryPages
{
    Food,
    Items,
    Medicine,
    Count
}

public class CaringPageControls : MonoBehaviour
{
	[SerializeField]
	Sprite emptySprite;
    [SerializeField]
    private RectTransform Inventory;
    private InventoryControls mInventoryControls;
    [SerializeField]
    private RectTransform Indicator;
    private bool InventoryOpen = false;
    private InventoryPages CurrentPage;
    [SerializeField]
    private InventoryItemUIIcon[] icons;
    [SerializeField]
    private GameObject InvBox;
    [SerializeField]
    private GameObject PhotoSaved;

	/*
    [SerializeField]
    private GameObject m_StereoUI;

    public GameObject StereoUI{ get { return m_StereoUI; } }

	[SerializeField]
	private GameObject m_PhoneUI;

	public GameObject PhoneUI{ get { return m_PhoneUI; } }

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
*/
    [SerializeField]
    private TutorialHandler m_TutorialHandler;

    public TutorialHandler TutorialHandler
    {
        get
        {
            if (m_TutorialHandler.Inited)
                m_TutorialHandler.Init();
            return m_TutorialHandler;
        }
    }

	public GameObject statsPointer;
	public GameObject foodPointer;

    [SerializeField]
    private RectTransform m_Triangle;
    private Vector2 m_TriangleHeight;

	
	public float flipPos = 0.5f;
	public float triangleLowOffset = 0.6f;
	public float uiScale = 1;

	float tweeningDir = 0;
	Tween uiTween = null; 

	public void OnEnable()
	{
		global::Inventory.onItemMoved += OnItemMoved;
	}

	public void OnDisable()
	{
		
		global::Inventory.onItemMoved -= OnItemMoved;
	}

	public void OnItemMoved(Inventory.Entry entry, Inventory.Locations from, Inventory.Locations to)
	{		
		PopulateButtons();
		/*
		if (from == global::Inventory.Locations.Inventory && to != global::Inventory.Locations.Inventory) 
		{
			PopulateButtons();
		}*/
	}
	
	RectTransform nextUI = null;
	GameObject nextTarget = null;
	static RectTransform currentUI = null;
	public static RectTransform CurrentUI
	{
		get
		{
			return currentUI;
		}
	}

	float GetUIScale()
	{
		return uiScale;
	}

	void SetUIScale(float scale)
	{
		uiScale = scale;
	}
	
	public void ShowUI(GameObject target, GameObject ui)
	{
		if (ui == currentUI) {
			m_TargetItem = target;
			return;
		}
		nextUI = ui != null ? ui.transform as RectTransform : null;
		nextTarget = target;
		if (uiTween != null && tweeningDir > 0) {
			// Currently tweening on so stop that and tween it off...
			uiTween.Kill ();
			uiTween = null;
			tweeningDir = 0;
		}
		if (uiTween == null && uiScale > 0) {
			// Tween off
			tweeningDir = -1;
			uiTween = DOTween.To( SetUIScale, uiScale, 0, 0.1f).OnComplete (FinishHidingUI).SetEase(Ease.InCirc);
		} else {
			FinishHidingUI ();
		}
	}

	void FinishHidingUI()
	{
		uiScale = 0;
		m_TargetItem = nextTarget;
		if (currentUI != null) {
			currentUI.gameObject.SetActive (false);
		}
		currentUI = nextUI;
		if (currentUI != null) {
			currentUI.gameObject.SetActive (true);			
			tweeningDir = 1;
			uiTween = DOTween.To (SetUIScale, uiScale, 1, 0.5f).SetEase(Ease.OutBounce);
		} else {
			tweeningDir = 0;
		}
	}


    static private GameObject m_TargetItem;

    static public GameObject TargetItem
    {
        get { return m_TargetItem; }
    }

    private LighbulbSwitchOnOffScript m_LightScript;

    private LighbulbSwitchOnOffScript LightScript
    {
        get
        {
            if (m_LightScript == null)
                m_LightScript = TargetItem.GetComponent<LighbulbSwitchOnOffScript>();
            return m_LightScript;
        }
    }

    private EDMBoxScript m_EDMScript;

    private EDMBoxScript EDMScript
    {
        get
        {
            if (m_EDMScript == null)
                m_EDMScript = TargetItem.GetComponent<EDMBoxScript>();
            return m_EDMScript;
        }
    }

	[SerializeField]
	CaringPageUI[] menus;

    void Start()
    {
		// Ensure all menus are registered
		for(int i = 0; i < menus.Length; i++)
		{
			menus[i].gameObject.SetActive(true);
			menus[i].gameObject.SetActive(false);
		}
        mInventoryControls = Inventory.GetComponent<InventoryControls>();
        PopulateButtons();
        m_TriangleHeight = new Vector2(0, m_Triangle.sizeDelta.y * 2.0f);
        DetectDragIconScript.OnClicked += EnableInvBox;
        DragDropMainBarItem.OnClicked += DisableInvBox;
        CharacterProgressScript.OnDragItem += EnableInvBox;
        CharacterProgressScript.OnDropItem += DisableInvBox;
        InvBoxControls.OnDropItem += DisableInvBox;

    }

    void ResetButtons()
    {
		for (int i = 0; i < icons.Length; i++) 
		{
			icons [i].Item = null;
		}
    }

    public void PopulateButtons()
    {
        Debug.Log("Populating buttons");
        ResetButtons();
        for (int i = 0; i < ProfilesManagementScript.Instance.CurrentProfile.Inventory.Count; ++i)
        {
			Inventory.Entry entry = ProfilesManagementScript.Instance.CurrentProfile.Inventory.Get (i);
			if(entry.Location != global::Inventory.Locations.Inventory) continue;
			ItemDefinition data = entry.Definition;
			int iconIndex = (int)data.ItemType;
			if (iconIndex < icons.Length && icons[iconIndex] != null && icons[iconIndex].Item == null)
			{
				icons[iconIndex].Item = entry;
			}
        }
    }


    public void EnableInvBox()
    {
        if (UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>().CurrentAction == ActionId.Sleep)
        {
            return;
        }
        SetInvBox(true);
    }
    public void DisableInvBox()
    {
        if (InvBox != null)
        {
            if (InvBox.GetComponentInChildren<InvBoxControls>() != null)
            {
                InvBox.GetComponentInChildren<InvBoxControls>().OnHoverEnd();
                SetInvBox(false);
            }
        }
    }

    private void SetInvBox(bool active)
    {
        if (InvBox != null)
        {
            InvBox.SetActive(active);
			for(int i = 0; i < icons.Length; i++)
			{
				icons[i].ShowHide(!active);
			}
        }
    }

    public bool GetInvBox()
    {
        return InvBox.activeSelf;
    }
	
	public void ItemAlbumButton()
	{
		UiPages.Next(Pages.ItemAlbum);
	}

	public void StatsButton()
	{
		UiPages.GetPage(Pages.CaringPage).GetComponent<CaringPageControls>().TutorialHandler.TriggerAdHoc("ViewStats");
		UiPages.Next(Pages.StatsPage);
		AudioController.Play(ProfilesManagementScript.Instance.CurrentAnimin.PlayerAniminId.ToString());
		statsPointer.SetActive(false);
	}

    public void MinigameButton()
    {
        DisappearAllItemUIs();
        UiPages.Next(Pages.MinigamesPage);
        TutorialHandler.TriggerAdHocStatic("ViewPortals");
        AudioController.Play("Portal");
    }

    public void PhotoButton()
    {
        if (Application.isEditor)
        {
            Application.CaptureScreenshot("screenshot.png");
            Invoke("PopPhotoSaved", 0.3f);
        }
        else
        {
			
            string screenshotName = "screenshot" + DateTime.Now.ToString("s") + ".png";
            Debug.Log("Saving photo to: " + screenshotName);
            #if UNITY_IOS
           // StartCoroutine(EtceteraBinding.takeScreenShot(screenshotName, imagePath =>
             //      	{
               //         EtceteraBinding.saveImageToPhotoAlbum(imagePath);
                 //   }));
			StartCoroutine(ScreenshotManager.Save( screenshotName, "Animin" , true));
			ScreenshotManager.ScreenshotFinishedSaving += PopPhoto;
			GetComponent<AudioSource>().Play();
         // Invoke("PopPhotoSaved", 0.3f);
            #elif UNITY_ANDROID
            StartCoroutine(ScreenshotManager.Save( screenshotName, "Animin" , true));
            ScreenshotManager.ScreenshotFinishedSaving += PopPhoto;
			GetComponent<AudioSource>().Play();
            #endif
        }
    }

    void PopPhoto(string eat)
    {
        PopPhotoSaved();
        ScreenshotManager.ScreenshotFinishedSaving -= PopPhoto;
    }
    void PopPhotoSaved()
    {
        if (PhotoSaved != null && PhotoSaved.GetComponent<PhotoFadeOut>() != null)
        {
            PhotoSaved.gameObject.SetActive(true);
        }
    }

    public void PhoneButton()
    {
        CharacterProgressScript script = UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>();
        script.HidePopupMenus(false);
        if (!TutorialHandler.CheckTutsCompleted("Phone"))
        {
            // Ah we are just phoning back
            TutorialHandler.TriggerAdHocStatic("CallPhone");
            return;
        }
        else
        {
            TutorialHandler.CallPhone();
        }
        // Show random phone quote
    }

    public void FoodButton()
    {
        DisappearAllItemUIs();
		foodPointer.SetActive(false);
        if (!InventoryOpen)
        {
            InventoryOpen = true;
        }
        else if (CurrentPage == InventoryPages.Food)
        {
            InventoryOpen = false;
        }
        else
        {
            SwitchInventory(CurrentPage);
        }
        CurrentPage = InventoryPages.Food;
        mInventoryControls.Init(CurrentPage);
        Inventory.gameObject.SetActive(InventoryOpen);
        Indicator.localPosition = new Vector2(-343, Indicator.localPosition.y);
   
    }

    public void ItemsButton()
    {
        DisappearAllItemUIs();
        if (!InventoryOpen)
        {
            InventoryOpen = true;
        }
        else if (CurrentPage == InventoryPages.Items)
        {
            InventoryOpen = false;
        }
        else
        {
            SwitchInventory(CurrentPage);
        }
        CurrentPage = InventoryPages.Items;
        mInventoryControls.Init(CurrentPage);
        Inventory.gameObject.SetActive(InventoryOpen);
        Indicator.localPosition = new Vector2(0, Indicator.localPosition.y);
    }

    public void MedicineButton()
    {
        DisappearAllItemUIs();
        if (!InventoryOpen)
        {
            InventoryOpen = true;
        }
        else if (CurrentPage == InventoryPages.Medicine)
        {
            InventoryOpen = false;
        }
        else
        {
            SwitchInventory(CurrentPage);
        }
        CurrentPage = InventoryPages.Medicine;
        mInventoryControls.Init(CurrentPage);
        Inventory.gameObject.SetActive(InventoryOpen);
        Indicator.localPosition = new Vector2(343, Indicator.localPosition.y);
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
	/*
    public void BroomButton()
    {
        CharacterProgressScript script = UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>();
		
        for (int i = 0; i < script.GroundItems.Count; ++i)
        {
            if (script.GroundItems[i].GetComponent<ItemDefinition>() != null)
            {
				if (script.GroundItems[i].GetComponent<ItemDefinition>().ItemType == PopupItemType.Token)
                {
                    continue;
                }
                else
                {
                    ProfilesManagementScript.Instance.CurrentAnimin.AddItemToInventory(script.GroundItems[i].GetComponent<ItemDefinition>().Id, 1);
                }
            }
            Destroy(script.GroundItems[i]);
        }
		
        script.GroundItems.Clear();
        UIGlobalVariablesScript.Singleton.SoundEngine.Play(GenericSoundId.CleanPooPiss);
        UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>().HidePopupMenus(false);
		
        for (int i = 0; i < EDMMixerScript.Singleton.KeysOn.Length; ++i)
        {
            EDMMixerScript.Singleton.KeysOn[i] = false;
        }
    }*/

    public void SetIcon(Inventory.Entry entry)
    {
		InventoryItemUIIcon icon = icons[(int)entry.Definition.ItemType];
		icon.Item = entry;	
	}	

    public void LightSwitch()
    {
        LighbulbSwitchOnOffScript script = TargetItem.GetComponent<LighbulbSwitchOnOffScript>();
        if (TargetItem.GetComponent<LighbulbSwitchOnOffScript>() != null)
        {
            script.Switch();
        }
    }

    public void JunoToggle(int Key)
    {
        Debug.Log("JunoToggle");
        int k = 16 + Key;
        EDMMixerScript.Singleton.KeysOn[k] = !EDMMixerScript.Singleton.KeysOn[k];
    }

    public void EDMToggle(int Key)
    {
        Debug.Log("EDMToggle");
        int k = 8 + Key;
        EDMMixerScript.Singleton.KeysOn[k] = !EDMMixerScript.Singleton.KeysOn[k];
    }

    public void PianoToggle(int Key)
    {
        Debug.Log("PianoToggle");
        int k = 0 + Key;
        EDMMixerScript.Singleton.KeysOn[k] = !EDMMixerScript.Singleton.KeysOn[k];
    }

    public void DisappearAllItemUIs()
    {
		ShowUI (null, null);
        /*AlarmUI.SetActive(false);
        EDMBoxUI.SetActive(false);
        JunoUI.SetActive(false);
        LightbulbUI.SetActive(false);
        PianoUI.SetActive(false);
        StereoUI.SetActive(false);
		PhoneUI.SetActive(false);*/
    }

	void LateUpdate()
	{
		UpdateTargetItem();
    }

	void UpdateTargetItem()
	{
		bool uiActive = currentUI != null && m_TargetItem != null;
		m_Triangle.gameObject.SetActive (uiActive);
		if(!uiActive) return;

		//this is the ui element
		RectTransform UI_Element = currentUI;
		
		
		//first you need the RectTransform component of your canvas
		RectTransform CanvasRect = this.GetComponent<RectTransform>();
		
		//then you calculate the position of the UI element
		//0,0 for the canvas is at the center of the screen, whereas WorldToViewPortPoint treats the lower left corner as 0,0. Because of this, you need to subtract the height / width of the canvas * 0.5 to get the correct position.
		
		Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(m_TargetItem.transform.position);
		Vector2 WorldObject_ScreenPosition = new Vector2(
			((ViewportPosition.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
			((ViewportPosition.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)));
		
		//now you can set the position of the ui element
		float sign = (WorldObject_ScreenPosition.y + CanvasRect.sizeDelta.y * 0.5f > CanvasRect.sizeDelta.y * flipPos) ? -1 : 1;
		
		float scale = sign;
		Vector2 triangleHeight = m_TriangleHeight * scale;
		if(sign < 0)
		{
			triangleHeight.y = triangleHeight.y * triangleLowOffset;
		}
		UI_Element.anchoredPosition = WorldObject_ScreenPosition + triangleHeight;
		Vector2 pivot = UI_Element.pivot;
		pivot.y = sign > 0 ? 0 : 1;
		UI_Element.pivot = pivot;

		//pivot.y = 1-pivot.y;
		//m_Triangle.pivot = pivot;

		m_Triangle.anchoredPosition = WorldObject_ScreenPosition + triangleHeight;
		Vector3 scaleV = Vector3.one;
		scaleV.y = scale;
		m_Triangle.transform.localScale = scaleV * uiScale;
		UI_Element.transform.localScale = Vector3.one * uiScale;
	}
}
