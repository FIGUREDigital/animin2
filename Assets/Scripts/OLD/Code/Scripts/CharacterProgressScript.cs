using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum InventoryItemId
{
    None = 0,

    Strawberry,
    Blueberry,
    Spinach,
    AlmondMilk,
    Avocado,
    Carrot,
    Chips,
    Toast,
    watermelon,

    Boombox,
    Clock,
    EDMJuno,
    EDM808,
    EDMKsynth,
    Lightbulb,
    paperCalendar,
    woodSword,
    woodFrame,
    Camera,
    FartButton,

    Pill,
    Plaster,

    Radio,

    Banana,
    Peanut,
    Beetroot,
    Chocolate,
    ChocoCake,
    CakeVanilla,
    Pizza,
    Noodles,
    Kiwi,
    Cereal,

	Phone,
    Zef,
	ItemAlbum,

	Box1,
	Box2,

	ChestBronze,
	ChestSilver,
	ChestGold,
	ChestEvo,

	BasketBall,

    Count,
}

[System.Serializable]
public class InventoryItemData
{
	// The following fields should not be used and are public for serialization only :-(
    public InventoryItemId Id;
	public int Count;

	public void GetDataForUpgrade(out InventoryItemId id, out int count)
	{
		id = Id;
		count = Count;
	}
}

public enum AniminSubevolutionStageId
{
    First = 0,
    Second,
    Third,
    Fourth,
    Fifth,
    Sixth,
    Seventh,
    Eighth,

    Count,
}

[System.Serializable]
public class AniminSubevolutionStageData
{
    public static float[] Stages = new float[] { 10, 20, 30, 40, 50, 60, 70, 80 };

}

[System.Serializable]
public class HappyStateRange
{
    public static HappyStateRange[] HappyStates = new HappyStateRange[]
    {
        new HappyStateRange() { Min = 0, Max = 14, Id = AnimationHappyId.Sad4 },
        new HappyStateRange() { Min = 8, Max = 26, Id = AnimationHappyId.Sad3 },
        new HappyStateRange() { Min = 20, Max = 38, Id = AnimationHappyId.Sad2 },
        new HappyStateRange() { Min = 32, Max = 50, Id = AnimationHappyId.Sad1 },
        new HappyStateRange() { Min = 45, Max = 65, Id = AnimationHappyId.Happy1 },
        new HappyStateRange() { Min = 60, Max = 80, Id = AnimationHappyId.Happy2 },
        new HappyStateRange() { Min = 75, Max = 95, Id = AnimationHappyId.Happy3 },
        new HappyStateRange() { Min = 90, Max = 110, Id = AnimationHappyId.Happy4 },
        new HappyStateRange() { Min = 105, Max = 125, Id = AnimationHappyId.Happy5 },
    };

    public float Min;
    public float Max;
    public AnimationHappyId Id;
}

public class CharacterProgressScript : MonoBehaviour
{

	void OnEnable()
	{
		Debug.Log ("OnEnable");
	}
	
	
	void OnDisable()
	{
		Debug.Log ("OnDisable");
	}
    public delegate void DragAction();
    public static event DragAction OnDragItem;
    
    public delegate void DropAction();
    public static event DropAction OnDropItem;
    private CaringPageControls m_CaringPageControls;

    public CaringPageControls CaringPageControls
    {
        get
        {
            if (m_CaringPageControls == null)
                m_CaringPageControls = UiPages.GetPage(Pages.CaringPage).GetComponent<CaringPageControls>();
            return m_CaringPageControls;
        }
    }

    //public List<AchievementId> Achievements = new List<AchievementId>();
    public DateTime NextHappynBonusTimeAt;
    public DateTime LastSavePerformed;
    public DateTime LastTimeToilet;
    public DateTime LastGiftTime;

    private bool IsDetectingSwipeRight;
    private int SwipesDetectedCount;
    private bool AtLeastOneSwipeDetected;
    private List<GameObject> TouchesObjcesWhileSwiping = new List<GameObject>();
    //private Vector3 lastMousePosition;

    private List<GameObject> groundItemsOnARscene = new List<GameObject>();
    private List<GameObject> groundItemsOnNonARScene = new List<GameObject>();


    public List<GameObject> GroundItemsNoLongerUsed
    {
        get
        {
            if (UIGlobalVariablesScript.Singleton.ARSceneRef.activeInHierarchy)
            {
                return groundItemsOnARscene;
            }
            else
            {
                return groundItemsOnNonARScene;
            }
        }
    }

    public GameObject ActiveWorld
    {
        get
        {
            if (UIGlobalVariablesScript.Singleton.ARSceneRef.activeInHierarchy)
                return UIGlobalVariablesScript.Singleton.ARWorldRef;
            else
                return UIGlobalVariablesScript.Singleton.NonARWorldRef;
        }
    }


    public GameObject PooPrefab;
    public GameObject PissPrefab;

    private Vector3 DestinationLocation;

	public bool IsSleeping
	{
		get
		{
			return animationController.IsSleeping;
		}
	}

    //public TextMesh TextTest;
    public AnimationControllerScript animationController;
    public bool IsMovingTowardsLocation;
    //public GameObject ObjectCarryAttachmentBone;
    //private GameObject DragableObject;
	GameObject objectHolding;
    public GameObject ObjectHolding
	{
		get
		{
			return objectHolding;
		}
		set
		{
			if (value == objectHolding) return;

			if (objectHolding != null)
			{
				//objectHolding.transform.parent = ActiveWorld.transform;
				// ToDo if item set layer
				Rigidbody body = objectHolding.GetComponent<Rigidbody>();
				if(body != null)
				{
					body.isKinematic = false;
				}
                ItemLink il = objectHolding.GetComponent<ItemLink>();
				if(il != null)
				{
					il.item.SetupLayer();
				}
				else
				{					
					objectHolding.layer = LayerMask.NameToLayer("Default");
				}
			}
			objectHolding = value;
			if (value != null)
			{
				objectHolding.layer = LayerMask.NameToLayer("IgnoreCollisionWithCharacter");
				Rigidbody body = objectHolding.GetComponent<Rigidbody>();
				if(body != null)
				{
                    body.isKinematic = true;
                }
            }

			GetComponent<CharacterSwapManagementScript>().CurrentModel.GetComponent<HeadReferenceScript>().HoldingObject = value;
            animationController.IsHoldingItem = (value != null);
            
        }
    }
    private float LastTapClick;

    private int RequestedToMoveToCounter;
    private float RequestedTime;

    private bool RequestedToMoveTo;
    private bool ShouldDragIfMouseMoves;
    private Vector3 MousePositionAtDragIfMouseMoves;
    public GameObject IsGoingToPickUpObject;
    public bool InteractWithItemOnPickup;
    public bool ShouldThrowObjectAfterPickup;
    public bool DragedObjectedFromUIToWorld;
    public ActionId CurrentAction;

    public GameObject SleepBoundingBox;

    public GameObject GroundPlane;

    RaycastHit moveHitInfo;
    RaycastHit detectDragHit;
    ActionId lastActionId;
    //bool hadButtonDownLastFrame;
    bool IsDetectingMouseMoveForDrag;
    bool IsDetectFlick;
    float FeedMyselfTimer;
    public bool HadUITouchLastFrame;
    public const float ConsideredHungryLevels = 70;

    //float TimeForNextHungryUnwellSadAnimation;
    //float LengthOfHungryUnwellSadAnimation;
    //HungrySadUnwellLoopId sadUnwellLoopState;
    float HoldingLeftButtonDownTimer;
    private bool TriggeredHoldAction;
    private List<Vector3> SwipeHistoryPositions = new List<Vector3>();
    public float PortalTimer;
    private bool m_JumpedIn = false;
    public float SmallCooldownTimer;
    private const float M_SHIT_TIME = 120.0f;
    private const float M_GIFT_TIME = 100.0f;
    private const float M_HAPPINESS_DEGREDATION = 0.1f;
    
   // private const float M_HEALTH_DEGREDATION = 0.1f;
	private const float M_HEALTH_DEGREDATION = 0.02f;
   // private const float M_HUNGER_DEGREDATION = 0.3f;
	private const float M_HUNGER_DEGREDATION = 0.06f;
   // private const float M_FITNESS_DEGREDATION = 0.4f;
	private const float M_FITNESS_DEGREDATION = 0.08f;
	
    private GUITexture[] m_UITextures;

    private bool m_HasStartedMoving;


    bool m_AllowEggTaps = false;
    bool m_AllowMoveAnimin = false; // Set to true once we allow the player to tell the animin to walk to a point
    bool m_AllowAutonomousMove = false;

    public bool AllowAutonomousMove
    {
        get
        {
            return m_AllowAutonomousMove;
        }
    }

    void TutorialEventFired(string fired)
    {
        if (fired == "AllowEggTaps")
        {
            m_AllowEggTaps = true;
        }
        else if (fired == "AllowMoveAnimin")
        {
            m_AllowMoveAnimin = true;
        }
    }

    void Awake()
    {
        m_AllowEggTaps = TutorialHandler.CheckTutorialContainingEventCompleted("AllowEggTaps");
        m_AllowAutonomousMove = m_AllowMoveAnimin = TutorialHandler.CheckTutorialContainingEventCompleted("AllowMoveAnimin");
        TutorialHandler.FireEvents += TutorialEventFired;

        LastSavePerformed = DateTime.UtcNow;
		LastTimeToilet = DateTime.UtcNow;
        
        //InventoryItemData.Initialize();

	
        //ProfilesManagementScript.Singleton.CurrentAnimin.SetDefault();
        //ProfilesManagementScript.Singleton.CurrentAnimin.Load();

        Debug.Log("CharacterProgressScript AWAKE");
 
        if (ProfilesManagementScript.Instance.CurrentProfile == null)
        {
            Debug.LogError("NO PROFILE FOUND");
            ProfilesManagementScript.Instance.CurrentProfile = PlayerProfileData.CreateNewProfile("buildintest");
            ProfilesManagementScript.Instance.CurrentAnimin = ProfilesManagementScript.Instance.CurrentProfile.Characters[(int)PersistentData.TypesOfAnimin.Tbo];
        }

        //TextTest.color = new Color(1, 1, 1, 0.0f);

        Physics.IgnoreLayerCollision(
            LayerMask.NameToLayer("IgnoreCollisionWithCharacter"),   
            LayerMask.NameToLayer("Character"));

		
        animationController = GetComponent<AnimationControllerScript>();
		Vector3 sleepPos = MainARHandler.FindFlatArea ();
		transform.localPosition = sleepPos;
		CurrentAction = ActionId.EnterSleep;
		ProfilesManagementScript.Instance.CurrentProfile.Inventory.SetLocationRoot(Inventory.Locations.AR, UIGlobalVariablesScript.Singleton.ARWorldRef);
		ProfilesManagementScript.Instance.CurrentProfile.Inventory.SetLocationRoot(Inventory.Locations.NonAR, UIGlobalVariablesScript.Singleton.NonARWorldRef);

		if (!ProfilesManagementScript.Instance.CurrentAnimin.Hatched)
		{
			ProfilesManagementScript.Instance.CurrentProfile.Inventory.PutAllItemsAway();
		}
    }


    void Start()
    {
		ConfigurableData.Instance.EnsureInstanced ();
        //UIClickButtonMasterScript.SetSoundSprite();

        if (ProfilesManagementScript.Instance.CurrentAnimin == null)
        {
            ProfilesManagementScript.Instance.CurrentAnimin = new PersistentData();
            ProfilesManagementScript.Instance.CurrentAnimin.PlayerAniminId = PersistentData.TypesOfAnimin.Tbo;
            ProfilesManagementScript.Instance.CurrentAnimin.AniminEvolutionId = AniminEvolutionStageId.Baby;
        }

        //HARRY: REMEMEBR TO REMOVE THESE COMMENTS, FOR CHRIST'S SAKE.
        Debug.Log("ID  : [" + ProfilesManagementScript.Instance.CurrentAnimin + "|" + ProfilesManagementScript.Instance.CurrentAnimin.AniminEvolutionId + "];");


        this.GetComponent<CharacterSwapManagementScript>().LoadCharacter(ProfilesManagementScript.Instance.CurrentAnimin.PlayerAniminId, ProfilesManagementScript.Instance.CurrentAnimin.AniminEvolutionId, !ProfilesManagementScript.Instance.CurrentAnimin.Hatched);
        //this.GetComponent<CharacterSwapManagementScript>().LoadCharacter(AniminId.Mandi   , AniminEvolutionStageId.Adult);

		/*
		ProfilesManagementScript.Instance.CurrentProfile.Inventory.EnsureWeHave(InventoryItemId.Radio, 1);
		ProfilesManagementScript.Instance.CurrentProfile.Inventory.EnsureWeHave(InventoryItemId.Boombox, 1);
		ProfilesManagementScript.Instance.CurrentProfile.Inventory.EnsureWeHave(InventoryItemId.ItemAlbum, 1);
		*/
		
		ProfilesManagementScript.Instance.CurrentProfile.Inventory.EnsureWeOwn(InventoryItemId.Radio, 1);
//		ProfilesManagementScript.Instance.CurrentProfile.Inventory.EnsureWeOwn(InventoryItemId.BasketBall, 1);
		
		ProfilesManagementScript.Instance.CurrentProfile.Inventory.EnsureWeOwn(InventoryItemId.Camera, 1);
//		ProfilesManagementScript.Instance.CurrentProfile.Inventory.EnsureWeOwn(InventoryItemId.Strawberry, 2);
//		ProfilesManagementScript.Instance.CurrentProfile.Inventory.EnsureWeOwn(InventoryItemId.FartButton, 1);
//		ProfilesManagementScript.Instance.CurrentProfile.Inventory.EnsureWeOwn(InventoryItemId.Box1, 10);
//		ProfilesManagementScript.Instance.CurrentProfile.Inventory.EnsureWeOwn(InventoryItemId.Box2, 5);

        if (BetweenSceneData.Instance.ReturnFromMiniGame)
        {
			if (!TutorialHandler.CheckTutsCompleted("MiniGameDone") && BetweenSceneData.Instance.chest == 0)
			{
				BetweenSceneData.Instance.chest = 1;
			}
			if(BetweenSceneData.Instance.chest > 0)
			{
				SpawnChests(BetweenSceneData.Instance.chest);
				ProfilesManagementScript.Instance.CurrentAnimin.Fitness += BetweenSceneData.Instance.chest*20;
			}
			TutorialHandler.TriggerAdHocStatic("MiniGameDone");
			BetweenSceneData.Instance.ResetData();
            if (BetweenSceneData.Instance.minigame == BetweenSceneData.Minigame.Collector)
            {
								UiPages.GetPage (Pages.CaringPage).GetComponent<CaringPageControls> ().TutorialHandler.BeginBlock ();
                UiPages.GetPage(Pages.CaringPage).GetComponent<CaringPageControls>().TutorialHandler.TriggerAdHoc("BoxLandReturn");
                if (BetweenSceneData.Instance.Points >= 7000)
                {
                    UiPages.GetPage(Pages.CaringPage).GetComponent<CaringPageControls>().TutorialHandler.TriggerAdHoc("BoxLandScoreBreak1");
                }
            }
        }

		
		if (DateTime.UtcNow.Subtract(ProfilesManagementScript.Instance.CurrentAnimin.CreatedOn).Days >= 1)
        {
            if (UiPages.GetPage(Pages.CaringPage).GetComponent<CaringPageControls>().TutorialHandler != null)
                UiPages.GetPage(Pages.CaringPage).GetComponent<CaringPageControls>().TutorialHandler.TriggerAdHoc("1DayEvolve");
        }
		if (DateTime.UtcNow.Subtract(ProfilesManagementScript.Instance.CurrentAnimin.CreatedOn).Days >= 3)
        {
            if (UiPages.GetPage(Pages.CaringPage).GetComponent<CaringPageControls>().TutorialHandler != null)
                UiPages.GetPage(Pages.CaringPage).GetComponent<CaringPageControls>().TutorialHandler.TriggerAdHoc("3DayEvolve");
        }
    }


   public void CleanUpItems(){
        /*for (int i = 0; i < groundItemsOnARscene.Count; i++)
        {
            ProfilesManagementScript.Instance.CurrentAnimin.AddItemToInventory(groundItemsOnARscene[i].GetComponent<ItemDefinition>().Id, 1);
        }*/

        //Debug.Log("ON DESTROYED! groundItemsOnNonARScene : [" + groundItemsOnNonARScene.Count + "];");
        //ProfilesManagementScript.Instance.CurrentAnimin.SaveCaringScreenItem(groundItemsOnNonARScene.ToArray(), ObjectHolding);
		ProfilesManagementScript.Instance.Save();
    }
    void OnDestroy()
    {
        TutorialHandler.FireEvents -= TutorialEventFired;
    }
    void OnApplicationQuit(){
		CleanUpItems();
    }

    
    void OnApplicationPause(bool pauseStatus)
    {

        //Debug.Log("pauseStatus:" + pauseStatus.ToString());
        if (pauseStatus)
        {
            CleanUpItems();
            Stop(true);		
            //CurrentAction = ActionId.EnterSleep;
        }
        else
        {
            //Stop(true);
        }
    }

	public GameObject SpawnStageItem(InventoryItemId itemID, Vector3 position, bool isZefRewardItem = false)
    {
		Inventory.Entry entry = ProfilesManagementScript.Instance.CurrentProfile.Inventory.Add (itemID);
		GameObject go = entry.Instance;
		entry.MoveTo (Inventory.CurrentLocation, position, true);
        //gameObject.transform.localRotation = Quaternion.Euler(0, UnityEngine.Random.Range(-180, 180), 0);

		if(isZefRewardItem)
		{
			ItemUnlockBehaviour.Show(itemID);
		}

        float scale = 0.1f;
		go.transform.localScale = new Vector3(scale, scale, scale);

		Debug.Log ("If somebody sees this, please tell Harry. Tell him [" + gameObject.name + "]; was a thing.");
		go.gameObject.transform.localRotation = Quaternion.Euler(0, UnityEngine.Random.Range(130, 230), 0);

		return go;
    }

    public GameObject GetRandomItem()
    {
		List<Inventory.Entry> entries = ProfilesManagementScript.Instance.CurrentProfile.Inventory.GetEntries (Inventory.CurrentLocation, PopupItemType.Item);
		if (entries.Count > 0) {
			
			return entries[UnityEngine.Random.Range(0, entries.Count)].Instance;
		}
		return null;
    }

    public void ThrowItemFromHands(Vector3 throwdirection)
    {
        //DragableObject = ObjectHolding;
        animationController.IsHoldingItem = false;

        this.gameObject.GetComponent<CharacterControllerScript>().RotateToLookAtPoint(this.transform.position + throwdirection * 50);



        float maxDistance = Vector3.Distance(Input.mousePosition, MousePositionAtDragIfMouseMoves) * 0.35f;
        if (maxDistance >= 160)
            maxDistance = 160;

        
        UIGlobalVariablesScript.Singleton.SoundEngine.Play(ProfilesManagementScript.Instance.CurrentAnimin.PlayerAniminId, ProfilesManagementScript.Instance.CurrentAnimin.AniminEvolutionId, CreatureSoundId.Throw);
        //ObjectHolding.transform.position = this.transform.position;
        //GroundItems.Add(ObjectHolding);
		
		//ObjectHolding.transform.rotation = Quaternion.Euler(0, ObjectHolding.transform.rotation.eulerAngles.y, 0);
		Rigidbody body = ObjectHolding.GetComponent<Rigidbody>();
		ObjectHolding = null;
		animationController.IsThrowing = true;
        IsDetectFlick = false;

		ThrowAnimationScript.Throw (body.gameObject, throwdirection, maxDistance);
        
        //CurrentAction = ActionId.None;
    }

    bool hiddenTutorial = false;

	ActionId queuedAction;
	
	public void Eat()
	{
		queuedAction = ActionId.EatItem;
	}
	public void Throw()
	{
		queuedAction = ActionId.ThrowItemAfterPickup;
    }
    
    public void HidePopupMenus(bool AboutToShow)
    {
        Debug.Log("Hiding popup menus");
        if (AboutToShow)
        {
            if (!hiddenTutorial)
            {
                TutorialHandler.Hide(true);
                hiddenTutorial = true;
            }
        }
        else if (hiddenTutorial)
        {
            TutorialHandler.Hide(false);
            hiddenTutorial = false;			
			CaringPageControls.ShowUI(null, null);
        }
        //UICOMMENT
 /*       CaringPageControls.StereoUI.SetActive(false);
        CaringPageControls.AlarmUI.SetActive(false);
        CaringPageControls.PhoneUI.SetActive(false);
        CaringPageControls.LightbulbUI.SetActive(false);
        CaringPageControls.EDMBoxUI.SetActive(false);
        CaringPageControls.JunoUI.SetActive(false);
        CaringPageControls.PianoUI.SetActive(false);*/
    }

    private GameObject GetClosestFoodToEat()
    {
		
		if (ProfilesManagementScript.Instance.CurrentAnimin.Hungry >= CharacterProgressScript.ConsideredHungryLevels) return null; // Not hungry
		
		List<Inventory.Entry> entries = ProfilesManagementScript.Instance.CurrentProfile.Inventory.GetEntries (Inventory.CurrentLocation, PopupItemType.Food);

		
		GameObject closestFood = null;
		for(int i = 0; i < entries.Count; i++)
		{
			GameObject go = entries[i].Instance;
			if (closestFood == null)
			{
				closestFood = go;
			}
			else
			{
				if (Vector3.Distance(this.transform.position, go.transform.position) < Vector3.Distance(this.transform.position, closestFood.transform.position))
				{
					closestFood = go;
				}
			}
		}
        return closestFood;
    }
    public void SpawnChests(int value){
        GetAndSpawnChests(value);
    }
    public GameObject GetAndSpawnChests(int value)
    {
		CharacterProgressScript progressScript = UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>();

		GameObject chest = progressScript.SpawnStageItem((InventoryItemId)((int)InventoryItemId.ChestBronze + value - 1), Boxes.FindSpawnPoint (2));
		chest.transform.rotation = Quaternion.Euler (0, UnityEngine.Random.Range (170, 190), 0);
		chest.GetComponent<ItemLink> ().item.UpdatePosAndRotFromTransform ();
		return chest;
    }

    public void AnimateJumpOutOfPortal()
    {

        //Harry copied this from OnTrackingLost
        CharacterProgressScript progressScript = UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>();

        UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<AnimationControllerScript>().IsExitPortal = true;
        UIGlobalVariablesScript.Singleton.MainCharacterRef.transform.rotation = Quaternion.Euler(0, 180, 0);
        UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterControllerScript>().ResetRotation();

        UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<AnimateCharacterOutPortalScript>().Timer = 0;
        UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<AnimateCharacterOutPortalScript>().JumbId = AnimateCharacterOutPortalScript.JumbStateId.Jumbout;
        UIGlobalVariablesScript.Singleton.SoundEngine.Play(ProfilesManagementScript.Instance.CurrentAnimin.PlayerAniminId, ProfilesManagementScript.Instance.CurrentAnimin.AniminEvolutionId, CreatureSoundId.JumbOutPortal);
        //UIGlobalVariablesScript.Singleton.ARPortal.GetComponent<PortalScript>().Show(true);
        progressScript.CurrentAction = ActionId.SmallCooldownPeriod;
        progressScript.SmallCooldownTimer = 0.5f;

        //End Guesswork
    }

    private float EatAlphaTimer;
    private bool PlayedEatingSound;

    public Color ShaderColor
    {
        get
        {
            //For simplicity's sake this just returns the first colour it can find. However, because the set method changes all materials, it should all be the same. Regardless, let that be known.
            return this.GetComponent<CharacterSwapManagementScript>().CurrentModel.GetComponentInChildren<SkinnedMeshRenderer>().material.color;
        }
        set
        {
            SkinnedMeshRenderer[] Meshes = this.GetComponent<CharacterSwapManagementScript>().CurrentModel.GetComponentsInChildren<SkinnedMeshRenderer>();
            for (int i = 0; i < Meshes.Length; i++)
            {
                Meshes[i].material.color = value;
            }
        }
    }

    public float ShaderAlpha
    {
        get
        {
            //For simplicity's sake this just returns the first colour it can find. However, because the set method changes all materials, it should all be the same. Regardless, let that be known.
            return this.GetComponent<CharacterSwapManagementScript>().CurrentModel.GetComponentInChildren<SkinnedMeshRenderer>().material.color.a;
        }
        set
        {
            SkinnedMeshRenderer[] Meshes = this.GetComponent<CharacterSwapManagementScript>().CurrentModel.GetComponentsInChildren<SkinnedMeshRenderer>();
            for (int i = 0; i < Meshes.Length; i++)
            {
                Color tmp = Meshes[i].material.color;
                tmp.a = value;
                Meshes[i].material.color = tmp;
            }
        }
    }


    private void ReplaceTexture(Texture tex)
    {
        for (int i = 0; i < GetComponentsInChildren<SkinnedMeshRenderer>().Length; i++)
        {
            GetComponentsInChildren<SkinnedMeshRenderer>()[i].material.mainTexture = tex;
        }
    }

	void UpdateAfterPause(float minutes)
	{
		ConfigurableData.Instance.UpdateAfterPause (minutes);//);
		//Debug.Log ("data fitness:" + data.fitness.Count);
	}
	
    // Update is called once per frame
    void Update()
	{
		float oldHealth = ProfilesManagementScript.Instance.CurrentAnimin.Health;
		DateTime lastPlay = ProfilesManagementScript.Instance.CurrentAnimin.lastPlay;
		DateTime now = DateTime.UtcNow;
		TimeSpan span = now.Subtract (lastPlay);
		if (span.TotalMinutes > 1 && span.TotalMinutes < 60*24*365) 
		{
			UpdateAfterPause ((float)span.TotalMinutes);
		}
		ProfilesManagementScript.Instance.CurrentAnimin.lastPlay = now;

        EvolutionManager.Instance.UpdateEvo();

        ProfilesManagementScript.Instance.CurrentAnimin.Hungry -= Time.deltaTime * M_HUNGER_DEGREDATION;
        ProfilesManagementScript.Instance.CurrentAnimin.Fitness -= Time.deltaTime * M_FITNESS_DEGREDATION;
        ProfilesManagementScript.Instance.CurrentAnimin.Health -= Time.deltaTime * M_HEALTH_DEGREDATION;

        if (oldHealth >= 40 && ProfilesManagementScript.Instance.CurrentAnimin.Health < 40)
        {
            TutorialHandler.TriggerAdHocStatic("Unhealthy");
        }
	
        //TextTest.color = new Color(1,1,1, TextTest.color.a - Time.deltaTime * 0.6f);
        //if(TextTest.color.a < 0)
        //	TextTest.color = new Color(1,1,1, 0);


       // ProfilesManagementScript.Singleton.CurrentAnimin.Happy = ((
           // (ProfilesManagementScript.Singleton.CurrentAnimin.Hungry / 100.0f) +
           // (ProfilesManagementScript.Singleton.CurrentAnimin.Fitness / 100.0f) +
           // (ProfilesManagementScript.Singleton.CurrentAnimin.Health / 100.0f))
        // 3.0f)
       // * PersistentData.MaxHappy;

		ProfilesManagementScript.Instance.CurrentAnimin.Happy = (((ProfilesManagementScript.Instance.CurrentAnimin.Hungry / 100.0f) + 
		                                                           (ProfilesManagementScript.Instance.CurrentAnimin.Fitness / 100.0f) + 
		                                                           (ProfilesManagementScript.Instance.CurrentAnimin.Health / 100.0f)) / 3.0f * PersistentData.MaxHappy);


        //Debug.Log("Hungry: " + (Hungry / 100.0f).ToString());
        //Debug.Log("Fitness: " + (Fitness / 100.0f).ToString());
        //Debug.Log("Health: " + (Health / 100.0f).ToString());
        //Debug.Log("Happy: " + (Happy / MaxHappy).ToString());
        // EVOLUTION BAR
//		{
//			//Evolution += (Happy / MaxHappy) * Time.deltaTime * 0.1f;
//			//if(Evolution >= 100) Evolution = 100;
//
//        float percentage = ProfilesManagementScript.Instance.CurrentAnimin.Evolution;
//        Sprite EvoProgress = UIGlobalVariablesScript.Singleton.EvolutionProgressSprite;
//        float scale = UIGlobalVariablesScript.Singleton.gameObject.transform.localScale.x;
//        int screenWidth = 1330;
//        int width = (int)(screenWidth * percentage);
//        Vector3 pos = EvoProgress.transform.position;
//        pos.x = ((-screenWidth + width) * 0.5f) * scale;
//        pos.y = 275.0f * scale;
//        EvoProgress.transform.position = pos;
//        EvoProgress.width = width;
//        EvoProgress.uvRect = new Rect(0, 0, percentage, 1);
//        EvoProgress.MarkAsChanged();
//
//
//			//UIGlobalVariablesScript.Singleton.EvolutionProgressSprite.width = (int)(1330.0f * (Evolution / 100.0f));
//			
		//			if(NextHappynBonusTimeAt >= DateTime.UtcNow)
//			{
//				// do bonus of happyness
//			}
//		}

        GameObject indicator = GetComponent<CharacterSwapManagementScript>().CurrentModel.GetComponent<HeadReferenceScript>().Indicator;

        if (ObjectHolding != null || animationController.IsSleeping == true)
        {
            indicator.SetActive(true);
        }
        else
        {
            indicator.SetActive(false);
        }

        bool hadUItouch = false;

        // CHECK FOR UI TOUCH
        if (Input.GetButton("Fire1") || Input.GetButtonDown("Fire1") || Input.GetButtonUp("Fire1"))
        {
           hadUItouch = UiPages.IsMouseOverUI();
        }

        RaycastHit hitInfo = new RaycastHit();
        bool hadRayCollision = false;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		int layerMask = -1;
		if (MainARHandler.Instance.CurrentItem != null) 
		{
			ItemLink il = MainARHandler.Instance.CurrentItem.GetComponent<ItemLink>();
			if (il != null)
			{
				if (il.item.Definition.ItemType == PopupItemType.Box)
				{
					layerMask = LayerMask.GetMask("Floor", "ExtendedFloor");
				}
			}
		}
		if (CurrentAction == ActionId.DragItemAround) 
		{			
			layerMask = LayerMask.GetMask("Floor",  "ExtendedFloor");
		}

		RaycastHit [] hits = Physics.RaycastAll(ray, Mathf.Infinity, layerMask);

		float dist = float.MaxValue;
		hadRayCollision = false;
		for (int i = 0; i < hits.Length; i++) 
		{
			if (hits[i].collider.gameObject != MainARHandler.Instance.CurrentItem)
			{
				if (hits[i].distance < dist)
				{
					dist = hits[i].distance;
					hitInfo = hits[i];
					hadRayCollision = true;
				}
			}
		}


		/*if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, layerMask))
        {
            hadRayCollision = true;
        }*/


        //Debug.Log(CurrentAction.ToString());
        switch (CurrentAction)
        {
            case ActionId.SmallCooldownPeriod:
                {
                    SmallCooldownTimer -= Time.deltaTime;
                    if (SmallCooldownTimer <= 0)
                    {
                        CurrentAction = ActionId.None;
                    }

                    break;
                }

            case ActionId.ThrowItemAfterPickup:
                {
					if(ObjectHolding == null)
					{				
						CurrentAction = ActionId.None;
	                }
	                else if (animationController.IsHoldingItemComplete)
                    {
                        Vector3 throwdirection = Vector3.Normalize(new Vector3(UnityEngine.Random.Range(-1.0f, 1.0f), 0, UnityEngine.Random.Range(-1.0f, 1.0f)));
                        ThrowItemFromHands(throwdirection);
                        CurrentAction = ActionId.None;
                    }
                    break;
                }

            case ActionId.EatItem:
            {
				bool canEat = false;
				if(ObjectHolding != null)
				{
					ItemLink il = ObjectHolding.GetComponent<ItemLink>();
					if(il != null)
					{
						canEat = il.item.Definition.ItemType == PopupItemType.Food;
					}
				}
				if(canEat)
				{
                   CurrentAction = ActionId.WaitEatingFinish;
                    animationController.IsHoldingItem = false;
                    animationController.IsEating = true;
                    EatAlphaTimer = 0;
                    PlayedEatingSound = false;
                }
				else
				{
					CurrentAction = ActionId.None;
				}
				break;
			}
            case ActionId.WaitEatingFinish:
                {
                    if (!animationController.IsEating)
                    {
                        //PopupItemType itemType = ObjectHolding./*GetComponent<ReferencedObjectScript>().Reference.*/GetComponent<UIPopupItemScript>().Type;
				
                        OnInteractWithPopupItem(ObjectHolding.GetComponent<ItemLink>().item);

                        ObjectHolding = null;
                        CurrentAction = ActionId.None;
						Debug.Log("FINISHED EATING");
						UiPages.GetPage(Pages.CaringPage).GetComponent<CaringPageControls>().TutorialHandler.TriggerAdHoc("EatStrawberry");
                    }
                    else
                    {
                        EatAlphaTimer += Time.deltaTime;

                        if (EatAlphaTimer >= 0.7f)
                        {
                            if (!PlayedEatingSound)
                            {
								ItemDefinition popup = ObjectHolding.GetComponent<ItemLink>().item.Definition;

                                PlayedEatingSound = true;
                                if (popup.SpecialId == SpecialFunctionalityId.Liquid)
                                    UIGlobalVariablesScript.Singleton.SoundEngine.Play(ProfilesManagementScript.Instance.CurrentAnimin.PlayerAniminId, ProfilesManagementScript.Instance.CurrentAnimin.AniminEvolutionId, CreatureSoundId.FeedDrink);
                                else
                                    UIGlobalVariablesScript.Singleton.SoundEngine.Play(ProfilesManagementScript.Instance.CurrentAnimin.PlayerAniminId, ProfilesManagementScript.Instance.CurrentAnimin.AniminEvolutionId, CreatureSoundId.FeedFood);

                            }
                        }

                        if (EatAlphaTimer >= 1)
                        {
                            if (ObjectHolding.GetComponent<Renderer>() != null)
                            {
                                ObjectHolding.GetComponent<Renderer>().material.shader = Shader.Find("Custom/ItemShader");

                                float alpha = ObjectHolding.GetComponent<Renderer>().material.color.a;
                                alpha -= Time.deltaTime * 3;
                                if (alpha <= 0)
                                    alpha = 0;
                                ObjectHolding.GetComponent<Renderer>().material.color = new Color(
                                    ObjectHolding.GetComponent<Renderer>().material.color.r,
                                    ObjectHolding.GetComponent<Renderer>().material.color.g,
                                    ObjectHolding.GetComponent<Renderer>().material.color.b,
                                    alpha);
                            }

                            for (int a = 0; a < ObjectHolding.transform.childCount; ++a)
                            {
                                if (ObjectHolding.transform.GetChild(a).GetComponent<Renderer>() == null)
                                    continue;

                                ObjectHolding.transform.GetChild(a).GetComponent<Renderer>().material.shader = Shader.Find("Custom/ItemShader");
						
                                float alpha = ObjectHolding.transform.GetChild(a).GetComponent<Renderer>().material.color.a;
                                alpha -= Time.deltaTime * 3;
                                if (alpha <= 0)
                                    alpha = 0;
                                ObjectHolding.transform.GetChild(a).GetComponent<Renderer>().material.color = new Color(
                                    ObjectHolding.transform.GetChild(a).GetComponent<Renderer>().material.color.r,
                                    ObjectHolding.transform.GetChild(a).GetComponent<Renderer>().material.color.g,
                                    ObjectHolding.transform.GetChild(a).GetComponent<Renderer>().material.color.b,
                                    alpha);
                            }
                        }

                    }

                    break;
                }


            case ActionId.ExitPortalMainStage:
                {
                    Debug.Log("ExitPortalmainStage");
			
					TutorialHandler.TriggerAdHocStatic("ExitSleep");
                    ShaderAlpha = 1;
			
                    UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<AnimateCharacterOutPortalScript>().Timer = 0;
                    UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<AnimateCharacterOutPortalScript>().JumbId = AnimateCharacterOutPortalScript.JumbStateId.Jumbout;

                    if (UIGlobalVariablesScript.Singleton.ARSceneRef.activeInHierarchy)
                        UIGlobalVariablesScript.Singleton.ARPortal.GetComponent<PortalScript>().Show(PortalStageId.ARscene, false);
                    else
                        UIGlobalVariablesScript.Singleton.ARPortal.GetComponent<PortalScript>().Show(PortalStageId.NonARScene, false);
			
                    UIGlobalVariablesScript.Singleton.SoundEngine.Play(ProfilesManagementScript.Instance.CurrentAnimin.PlayerAniminId, ProfilesManagementScript.Instance.CurrentAnimin.AniminEvolutionId, CreatureSoundId.JumbOutPortal);

                    animationController.IsExitPortal = true;

                    CurrentAction = ActionId.SmallCooldownPeriod;
                    SmallCooldownTimer = 0.5f;

                    break;
                }



            case ActionId.EnterPortalToAR:
                {
                    const float StartFade = 0.5f;
                    const float StopAt = 0.7f;
                    const float JumpOutAt = 1.5f;
                    //OnEnterARScene();
                    PortalTimer += Time.deltaTime;

                    //Debug.Log("Portal Timer : [" + PortalTimer + "];");

                    if (PortalTimer >= JumpOutAt)
                    {
                        //GetComponent<CharacterSwapManagementScript>().CurrentModel.gameObject.SetActive(true);
                        ShaderAlpha = 1;
                        CurrentAction = ActionId.None;
                        MainARHandler.Instance.OnCharacterEnterARScene();

                        m_JumpedIn = false;
                    }
                    else if (PortalTimer >= StopAt)
                    {
                        if (!m_JumpedIn)
                        {
                            m_JumpedIn = true;
                            //GetComponent<CharacterSwapManagementScript>().CurrentModel.gameObject.SetActive(false);
                            ShaderAlpha = 0;

                            UIGlobalVariablesScript.Singleton.NonSceneRef.SetActive(false);
                            UIGlobalVariablesScript.Singleton.ARSceneRef.SetActive(true);
					
							MainARHandler.Instance.PauseJumpOutIntoAR();
                        
                        	UIGlobalVariablesScript.Singleton.ARPortal.GetComponent<PortalScript>().Show(PortalStageId.ARscene, false);

                            UIGlobalVariablesScript.Singleton.SoundEngine.Play(ProfilesManagementScript.Instance.CurrentAnimin.PlayerAniminId, ProfilesManagementScript.Instance.CurrentAnimin.AniminEvolutionId, CreatureSoundId.JumbOutPortal);
                        }
                    }
                    else if (PortalTimer >= StartFade)
                    {

                    }

                    break;
                }

        /*case ActionId.EnterPortalToNonAR:
        			{
        				PortalTimer += Time.deltaTime;
        			
        				if(PortalTimer >= 1.10f)
        				{
        					Debug.Log("EnterPortalToNonAR finished");
        					CurrentAction = ActionId.None;
        					UIGlobalVariablesScript.Singleton.Vuforia.OnExitAR();
        					UIGlobalVariablesScript.Singleton.ARPortal.GetComponent<PortalScript>().Show(false);
        				}
        				
        				break;
        			}
                */
            case ActionId.EnterSleep:
                {
                    animationController.IsSleeping = true;
					CurrentAction = ActionId.None;
					SleepBoundingBox.SetActive(ProfilesManagementScript.Instance.CurrentAnimin.Hatched);
                    if (ProfilesManagementScript.Instance.CurrentAnimin.Hatched)
					{
                        UIGlobalVariablesScript.Singleton.SoundEngine.PlayLoop(ProfilesManagementScript.Instance.CurrentAnimin.PlayerAniminId, ProfilesManagementScript.Instance.CurrentAnimin.AniminEvolutionId, CreatureSoundId.SnoringSleeping);
                    }

                    break;
                }

           // case ActionId.Sleep:			
            case ActionId.None:
                {
                    //Debug.Log("INSIDE NONE");

                    if (Input.GetButton("Fire1"))
                        HoldingLeftButtonDownTimer += Time.deltaTime;
                    else
                        HoldingLeftButtonDownTimer = 0;

							
					if(queuedAction != ActionId.None)
					{
						CurrentAction = queuedAction;
						queuedAction = ActionId.None;
						break;
					}

					if (lastActionId != ActionId.None)
					{
                        //Debug.Log("if(lastActionId != ActionId.None)");
                    }
                    else if (HadUITouchLastFrame || hadUItouch || DragedObjectedFromUIToWorld)
                    {
                        //Debug.Log("UI TOUCH");

                        break;
                    }
                    else if (Input.GetButtonDown("Fire1"))
                    {
                        //Debug.Log("else if(Input.GetButtonDown(Fire1))");

                        if (hadRayCollision)
                        {
                            if (hitInfo.collider.gameObject == ObjectHolding || hitInfo.collider.gameObject == this.gameObject)
                            {
								if(!IsSleeping)
								{
	                                IsDetectFlick = true;
	                                //CurrentAction = ActionId.DetectFlickAndThrow;
	                                MousePositionAtDragIfMouseMoves = Input.mousePosition;
								}
                            }
                            else if (hitInfo.collider.tag == "Items" && hitInfo.collider.transform)
                            {
                                IsDetectingMouseMoveForDrag = true;
                                //CurrentAction = ActionId.DetectMouseMoveAndDrag;
                                MousePositionAtDragIfMouseMoves = Input.mousePosition;
                                detectDragHit = hitInfo;
                                //Debug.Log("DetectMouseMoveAndDrag");
                            }
                        }

                    }
				/*else if(HoldingLeftButtonDownTimer >= 0.70f && Input.GetButton("Fire1") && hadRayCollision && hitInfo.collider.tag == "Items" && (hitInfo.collider.GetComponent<ReferencedObjectScript>().Reference.GetComponent<UIPopupItemScript>().Menu != MenuFunctionalityUI.None))
				{


					if(hitInfo.collider.GetComponent<ReferencedObjectScript>().Reference.GetComponent<UIPopupItemScript>().Menu == MenuFunctionalityUI.Clock)
					{
						UIGlobalVariablesScript.Singleton.Item3DPopupMenu.GetComponent<UIWidget>().SetAnchor(hitInfo.collider.gameObject);
						TriggeredHoldAction = true;
						UIGlobalVariablesScript.Singleton.Item3DPopupMenu.SetActive(true);
					}
				

				}*/
				    else if (IsDetectFlick && !Input.GetButton("Fire1") && (Vector3.Distance(Input.mousePosition, MousePositionAtDragIfMouseMoves) > 25) && ObjectHolding != null)
                    {
                        //Debug.Log("IsDetectFlick");

                        Vector3 throwdirection = Vector3.Normalize(Input.mousePosition - MousePositionAtDragIfMouseMoves);
                        throwdirection.z = throwdirection.y;
                        throwdirection.y = 0;

                        ThrowItemFromHands(throwdirection);

                    }
                    else if (IsDetectingMouseMoveForDrag && Vector3.Distance(Input.mousePosition, MousePositionAtDragIfMouseMoves) >= 5 && Input.GetButton("Fire1"))
                    {
						// Start actual drag once dragged far enough
                        //Debug.Log("IsDetectingMouseMoveForDrag");

						Inventory.Entry e = detectDragHit.collider.gameObject.GetComponent<ItemLink>().item;
                        if (e.Definition.ItemType != PopupItemType.Token)
						{
							MainARHandler.Instance.CurrentItem = detectDragHit.collider.gameObject;		
		                    CurrentAction = ActionId.DragItemAround;
							IsDetectingMouseMoveForDrag = false;
							MainARHandler.Instance.CurrentItem.layer = LayerMask.NameToLayer("IgnoreCollisionWithCharacter");
//							MainARHandler.Instance.CurrentItem.GetComponent<BoxCollider>().enabled = false;
							SwitchGravity(MainARHandler.Instance.CurrentItem, false);
                        }

                    }
                    else if (Input.GetButton("Fire1") && !IsSleeping)
                    {


                        //Debug.Log("DETECT SWIPE MECHANISM");

                        /*if(SwipeHistoryPositions.Count == 0)
					{
						SwipeHistoryPositions.Add(Input.mousePosition);
					}
					else
					{*/
//						Debug.Log("DIFFERENCE: " + (Input.mousePosition.x - SwipeHistoryPositions[SwipeHistoryPositions.Count - 1].x).ToString());

                        bool triedOnce = false;
                        if (IsDetectingSwipeRight || SwipesDetectedCount == 0)
                        {
                            bool hadEnoughMovement = false;
                            for (int i = 0; i < SwipeHistoryPositions.Count; ++i)
                                if ((Input.mousePosition.x - SwipeHistoryPositions[i].x) >= 30)
                                {
                                    hadEnoughMovement = true;
                                    break;
                                }

                            if (hadEnoughMovement)
                            {
                                //Debug.Log((Input.mousePosition.x - SwipeHistoryPositions[SwipeHistoryPositions.Count - 1].x).ToString());
								
                                IsDetectingSwipeRight = !IsDetectingSwipeRight;
                                SwipesDetectedCount++;
                                //Debug.Log("swipe moving right: " + SwipesDetectedCount.ToString());
                                triedOnce = true;
                                SwipeHistoryPositions.Clear();
                            }
                        }
						
                        if (!triedOnce && (!IsDetectingSwipeRight || SwipesDetectedCount == 0))
                        {
                            bool hadEnoughMovement = false;
                            for (int i = 0; i < SwipeHistoryPositions.Count; ++i)
                                if ((SwipeHistoryPositions[i].x - Input.mousePosition.x) >= 30)
                                    hadEnoughMovement = true;

                            if (hadEnoughMovement)
                            {
                                //Debug.Log((Input.mousePosition.x - SwipeHistoryPositions[SwipeHistoryPositions.Count - 1].x).ToString());
                                //SwipeHistoryPositions.Add(Input.mousePosition);
                                IsDetectingSwipeRight = !IsDetectingSwipeRight;
                                SwipesDetectedCount++;
                                //Debug.Log("swipe moving left: " + SwipesDetectedCount.ToString());
                                SwipeHistoryPositions.Clear();
                            }
                        }

                        if (hadRayCollision)
                        {
                            if (!TouchesObjcesWhileSwiping.Contains(hitInfo.collider.gameObject))
                                TouchesObjcesWhileSwiping.Add(hitInfo.collider.gameObject);
                        }

                        if (SwipesDetectedCount >= 3)
                        {
                            AtLeastOneSwipeDetected = true;
                            //Debug.Log("SWIPE DETECTED");
                            IsDetectingSwipeRight = !IsDetectingSwipeRight;
                            SwipesDetectedCount = 0;
							

                            bool cleanedShit = false;
                            for (int i = 0; i < TouchesObjcesWhileSwiping.Count; ++i)
                            {
                                if (TouchesObjcesWhileSwiping[i].tag == "Shit")
                                {
                                    if (TouchesObjcesWhileSwiping[i].tag == "Shit")
                                        cleanedShit = true;

                                    Destroy(TouchesObjcesWhileSwiping[i]);
                                    TouchesObjcesWhileSwiping.RemoveAt(i);

                                    Debug.Log("SwipeDetected!");
                                    HidePopupMenus(false);									

                                    i--;
                                }
                            }

                            if (cleanedShit)
                            {
                                UIGlobalVariablesScript.Singleton.SoundEngine.Play(GenericSoundId.CleanPooPiss);
                                TutorialHandler.TriggerAdHocStatic("ShitCleaned");
                            }
												
                            if (TouchesObjcesWhileSwiping.Contains(this.gameObject) && !cleanedShit && !animationController.IsTickled)
                            {
                                Stop(true);
                                animationController.IsTickled = true;

                                UIGlobalVariablesScript.Singleton.SoundEngine.Play(ProfilesManagementScript.Instance.CurrentAnimin.PlayerAniminId, ProfilesManagementScript.Instance.CurrentAnimin.AniminEvolutionId, 
                                    (CreatureSoundId)((int)CreatureSoundId.Tickle + UnityEngine.Random.Range(0, 3)));
							
							
							
                            }
                        }

				
                        SwipeHistoryPositions.Add(Input.mousePosition);
                        if (SwipeHistoryPositions.Count >= 100)
                        {
                            SwipeHistoryPositions.RemoveAt(0);
                        }




                    }
                    else if (Input.GetButtonUp("Fire1"))
                    {
                        //Debug.Log("Input.GetButtonUp(Fire1)");
                        IsDetectFlick = false;
                        IsDetectingMouseMoveForDrag = false;

						if (IsSleeping)
						{
							Egg egg = hitInfo.collider != null ? hitInfo.collider.GetComponent<Egg>() : null;
							if (!hadUItouch && hadRayCollision && (hitInfo.collider.gameObject == SleepBoundingBox || egg != null))
							{
								if (egg != null)
								{
									// Only allow tapping on the egg once the tutorial has told you about it!
									if (m_AllowEggTaps)
									{                            
										if(egg.Tap ())
										{
											GetComponent<CharacterSwapManagementScript>().CurrentModel.transform.localScale = GetComponent<CharacterSwapManagementScript>().defaultScale;
											
											ProfilesManagementScript.Instance.CurrentAnimin.CreatedOn = System.DateTime.UtcNow;

											TutorialHandler.TriggerAdHocStatic("Hatched");
											
											exitSleep();
										}
									}
								}
								else
								{
									// Not in an egg so awake straight away
									exitSleep();
									TutorialHandler.TriggerAdHocStatic("ExitSleep");
								}								
							}
						}
						else
						{
							//if (UIGlobalVariablesScript.Singleton.DragableUI3DObject.transform.childCount == 1 && UIGlobalVariablesScript.Singleton.DragableUI3DObject.transform.GetChild(0).name == "Broom")
	                        if (CameraModelScript.Instance.transform.childCount == 1 && CameraModelScript.Instance.transform.GetChild(00).name == "Broom")
	                        {
								Debug.LogError ("Complete refactor following code not been considered");
								/*
	                            if (hadRayCollision && (hitInfo.collider.tag == "Items" || hitInfo.collider.tag == "Shit") && GroundItems.Contains(hitInfo.collider.gameObject))
	                            {



	                                GroundItems.Remove(hitInfo.collider.gameObject);
	                                Destroy(hitInfo.collider.gameObject);
	                            }*/
	                        }
	                        else if (!AtLeastOneSwipeDetected && hadRayCollision/* && !TriggeredHoldAction*/)
	                        {

	                            if (hitInfo.collider.name.StartsWith("MainCharacter") || hitInfo.collider.gameObject == ObjectHolding)
	                            {
	                                //Debug.Log("HIT THE CHARACTER FOR INTERACTION");

	                                if (ObjectHolding != null)
	                                {
	                                    //Debug.Log("HIT THE CHARACTER FOR INTERACTION 2");

										ItemLink item = ObjectHolding./*GetComponent<ReferencedObjectScript>().Reference.*/GetComponent<ItemLink>();


										if (item.item.Definition.ItemType == PopupItemType.Food)
	                                    {
	                                        //Debug.Log("HIT THE CHARACTER FOR INTERACTION 3");

	                                        this.GetComponent<CharacterProgressScript>().CurrentAction = ActionId.EatItem;
	                                    }
										else if (OnInteractWithPopupItem(item.item))
	                                    {
	                                        //Debug.Log("HIT THE CHARACTER FOR INTERACTION 4");
	                                        //Destroy(ObjectHolding);
	                                        ObjectHolding = null;
	                                    }

	                                }
	                                else if (ObjectHolding == null && CameraModelScript.Instance.transform.childCount == 0 && !animationController.IsPat)
	                                {
	                                    //Debug.Log("HIT THE CHARACTER FOR INTERACTION 4");
	                                    Stop(true);
	                                    animationController.IsPat = true;
	                                    //Debug.Log("IS TICKLED");
									
	                                    UIGlobalVariablesScript.Singleton.SoundEngine.Play(ProfilesManagementScript.Instance.CurrentAnimin.PlayerAniminId, ProfilesManagementScript.Instance.CurrentAnimin.AniminEvolutionId, CreatureSoundId.PatReact);
	                                }
	                                Debug.Log("Tap");
									UiPages.GetPage(Pages.CaringPage).GetComponent<CaringPageControls>().TutorialHandler.TriggerAdHoc("StrokeAnimin");
								}
								else if ((hitInfo.collider.tag == "Items") && hitInfo.collider.GetComponent<ItemLink>().item.Definition.ItemType == PopupItemType.Box && hitInfo.collider.GetComponent<ItemLink>().item.justSpawnedFromChest)
								{
									OnInteractWithPopupItem(hitInfo.collider.GetComponent<ItemLink>().item);
								}
	                            else if ((hitInfo.collider.tag == "Items") && hitInfo.collider.GetComponent<ItemLink>().item.Definition.ItemType == PopupItemType.Token)
	                            {
	                                OnInteractWithPopupItem(hitInfo.collider.GetComponent<ItemLink>().item);
	                            }
								else if (!IsSleeping && (hitInfo.collider.name.StartsWith("Invisible Ground Plane") || (hitInfo.collider.tag == "Items")))
	                            {
	                                //float distane = Vector3.Distance(hitInfo.point, this.transform.position);
	                                //MoveTo(hitInfo.point, distane > 220.0f ? true : false);
	                                if (RequestedToMoveToCounter == 0)
	                                    RequestedTime = Time.time;
	                                RequestedToMoveToCounter++;
	                                moveHitInfo = hitInfo;

	                                if (ObjectHolding != null && (hitInfo.collider.tag == "Items"))
	                                {
										// Put object down as we want to play with another item
	                                    ObjectHolding = null;
	                                }
	                            }
	                            else
	                            {
	                                Stop(true);
	                                Debug.Log("STOPING BECAUSE NOTHING HIT");
	                            }
	                        }
		//					else if(UIGlobalVariablesScript.Singleton.CubeRunnerMinigameSceneRef.activeInHierarchy)
		//					{
		//						UIGlobalVariablesScript.Singleton.ImageTarget.transform.rotation = 
		//							Quaternion.Euler(
		//								UIGlobalVariablesScript.Singleton.ImageTarget.transform.rotation.eulerAngles.x, 
		//								UIGlobalVariablesScript.Singleton.ImageTarget.transform.rotation.eulerAngles.y + 90, 
		//								UIGlobalVariablesScript.Singleton.ImageTarget.transform.rotation.eulerAngles.z);
		//					}

	                        //TriggeredHoldAction = false;
	                    }
					}

                    //Debug.Log("END OF NONE");


					if (RequestedToMoveToCounter > 0 && moveHitInfo.collider)
                    {
                        if ((Time.time - RequestedTime) >= 0.17f)
                        {
                            Stop(true);

                            bool preventMovingTo = false;
                            Vector3 point = moveHitInfo.point;
							ItemLink il = moveHitInfo.collider.GetComponent<ItemLink>();
							Inventory.Entry entry = null;
							if (il != null)
							{
								entry = il.item;
							}
							if (moveHitInfo.collider.tag == "Items" && (entry == null || entry.Definition.ItemType != PopupItemType.Box))
                            {
                                moveHitInfo.collider.gameObject.AddComponent<FlashObjectScript>();

                                point = moveHitInfo.transform.position;							

								MenuFunctionalityUI menuUI = moveHitInfo.collider.GetComponent<ItemLink>().item.Definition.Menu;
								GameObject menu = CaringPageUI.GetUI(menuUI);

								if (menu != null && !menu.activeInHierarchy && RequestedToMoveToCounter == 1 && (menuUI != MenuFunctionalityUI.None) && !hadUItouch)
								{
									HidePopupMenus(true);
									CaringPageControls.ShowUI(moveHitInfo.collider.gameObject, menu);
									preventMovingTo = true;
/*									EDMBoxScript edmScript = moveHitInfo.collider.gameObject.GetComponent<EDMBoxScript>();
									if (edmScript != null)
									{
										edmScript.SetInterface(menu);
									}*/
                                }
                                else if (ObjectHolding == null)
                                {
                                    IsGoingToPickUpObject = moveHitInfo.collider.gameObject;
                                    Debug.Log("going to pickup");
                                }
                                else
                                {
                                    Debug.Log("will not pick up, i already have item");
                                }
                            }
                            point.y = this.transform.position.y;

                            if (!preventMovingTo && m_AllowMoveAnimin)
                            {
                                if (!hadUItouch)
                                    HidePopupMenus(false);

                                m_AllowAutonomousMove = true;

                                if (RequestedToMoveToCounter > 1)
                                {
                                    MoveTo(point, true);
                                    //UICOMMENT: UiPages.GetPage(Pages.CaringPage).GetComponent<CaringPageControls>().TutorialHandler.TriggerAdHocExitCond("Walk", "runto");
                                }
                                else
                                {
                                    MoveTo(point, false);
                                    //UICOMMENT: UiPages.GetPage(Pages.CaringPage).GetComponent<CaringPageControls>().TutorialHandler.TriggerAdHocExitCond("Walk", "walkto");
                                }
                            }
						
						
                            RequestedToMoveToCounter = 0;
                        }
                    }
					else if(!animationController.IsSleeping)
			        {
                        if (!IsMovingTowardsLocation && !animationController.IsWakingUp && ObjectHolding == null && ProfilesManagementScript.Instance.CurrentAnimin.Hungry <= ConsideredHungryLevels && !animationController.IsTickled)
                        {
                            //Debug.Log("Famished!");
                            FeedMyselfTimer += Time.deltaTime;

                            if (FeedMyselfTimer >= 1)
                            {
                                //Debug.Log("Feed myself!");
                                GameObject closestFood = GetClosestFoodToEat();

                                //Debug.Log(closestFood == null ? "Didn't find a closest food" : "Found a closest food");
                                if (closestFood != null)
                                {
                                    IsGoingToPickUpObject = closestFood;
                                    MoveTo(closestFood.transform.position, false);
                                }

                                FeedMyselfTimer = 0;
                            }
                        }
                    }

				
				

                    break;
                }




		

            case ActionId.DropItem:
                {
//                    bool validDrop = false;
				  
                    // DRAG ITEM ON TO THE CHARACTER
					if(!InvBoxControls.listening)
					{
	                    if (hadRayCollision && hitInfo.collider.name.StartsWith("MainCharacter") && !animationController.IsHoldingItem)
	                    {
							PutItemInHands(MainARHandler.Instance.CurrentItem);
	                    }
	                    else //if (hadRayCollision && hitInfo.collider.name.StartsWith("Invisible Ground Plane"))
	                    {
							MainARHandler.Instance.CurrentItem.transform.parent = ActiveWorld.transform;
							MainARHandler.Instance.CurrentItem.layer = LayerMask.NameToLayer("Default");
							ItemLink il = MainARHandler.Instance.CurrentItem.GetComponent<ItemLink>();
							if(il != null)
							{
						il.item.MoveTo(Inventory.CurrentLocation, MainARHandler.Instance.CurrentItem.transform.position, MainARHandler.Instance.CurrentItem.transform.localEulerAngles);
								il.item.SetupLayer();
	                        }
					//!!!!! is this happening AndroidJNI AsyncCallback SetupLayer                       
	                        
	                    }
				
						if(OnDropItem != null)
							OnDropItem();
                    /*else
						{
	                        Debug.Log("DROPED IN UNKNOWN LOCATION");
	                        if(OnDropItem != null)
	                            OnDropItem();
	                    }*/
					}

//					MainARHandler.Instance.CurrentItem.GetComponent<BoxCollider>().enabled = true;
			
					SwitchGravity(MainARHandler.Instance.CurrentItem, true);
                    CurrentAction = ActionId.None;
					MainARHandler.Instance.CurrentItem = null;
					Debug.Log ("Dropped");
                    break;
                }

            case ActionId.DragItemAround:
                {


                if(OnDragItem != null)
                    OnDragItem();

			if (hadRayCollision && (((1<<hitInfo.collider.gameObject.layer) & Boxes.FloorLayerMask) != 0))//(hitInfo.collider.name.StartsWith("Invisible Ground Plane") || hitInfo.collider.name.StartsWith("Extended")))
			//if(hadRayCollision && hitInfo.collider.name.StartsWith("SecondGroundPlane"))
                    {
						ItemLink li = MainARHandler.Instance.CurrentItem.GetComponent<ItemLink>();
//						Debug.Log("DRAGGING "+(li != null).ToString ());
						if (li != null)
						{
							Vector3 pos = Boxes.GetGroundPoint(hitInfo);
							pos.y += 1000;
							RaycastHit sphereHit;
							int layersMask;
							if(li.item.Definition.ItemType == PopupItemType.Box)
							{
								layersMask = LayerMask.GetMask("Floor",  "ExtendedFloor");
							}
							else
							{
								layersMask = LayerMask.GetMask("Floor","Items","ExtendedFloor") | 1;
							}
							if (Physics.SphereCast(pos, 5, Vector3.down, out sphereHit, float.MaxValue, layersMask))
							{
								Debug.DrawLine(pos, sphereHit.point);
								pos.y = sphereHit.point.y;
								Debug.DrawLine(pos, sphereHit.point);
							}
							else
							{
								pos.y -= 1000;
							}

							Vector3 rot = MainARHandler.Instance.CurrentItem.transform.localEulerAngles;
							rot.x = 0;
							rot.y = rot.y % 360;
							if (rot.y < 0)
							{
								rot.y +=360;
							}
							rot.z = 0;
							if (rot.y > 180+45)
							{
								rot.y = 180+45;
							}
							if (rot.y < 180-45)
							{
								rot.y = 180-45;
							}
							li.item.MoveTo(li.item.Location, pos, rot);
						}
						else
						{
							MainARHandler.Instance.CurrentItem.transform.position = hitInfo.point;
						}
                        //DragableObject.transform.parent = hit.transform;
                    }

                    if (!Input.GetButton("Fire1"))
                    {

                        if(InvBoxControls.listening)
                        {
                            CurrentAction = ActionId.None;
                        } 
                        else				
                        {
                            if (hitInfo.collider != null && hitInfo.collider.name.StartsWith("Extended"))
                            {
								MainARHandler.Instance.CurrentItem.AddComponent<DroppedItemScript>();
                            }
                            CurrentAction = ActionId.DropItem;
                        }
                    }
				

                    break;
                }
        }

		if ((DateTime.UtcNow - LastTimeToilet).TotalSeconds >= M_SHIT_TIME && !animationController.IsSleeping && animationController.IsIdle && !IsMovingTowardsLocation && TutorialHandler.CheckTutsCompleted("MiniGameDone"))
        {
            GameObject newPoo;
            if (UnityEngine.Random.Range(0, 2) == 0 || !TutorialHandler.CheckTutsCompleted("Shit"))
            {
                newPoo = GameObject.Instantiate(PooPrefab) as GameObject;
                UIGlobalVariablesScript.Singleton.SoundEngine.Play(GenericSoundId.TakePoo);
                TutorialHandler.TriggerAdHocStatic("Shit"); //Hey, we have naming conventions. I'm gonna stick to them.
            }
            else
            {
                newPoo = GameObject.Instantiate(PissPrefab) as GameObject;
                UIGlobalVariablesScript.Singleton.SoundEngine.Play(GenericSoundId.TakePiss);
            }

            newPoo.transform.parent = ActiveWorld.transform;
            newPoo.transform.position = this.transform.position;
            newPoo.transform.rotation = Quaternion.Euler(0, 180 + UnityEngine.Random.Range(-30.0f, 30.0f), 0);

            int sign = -1;
            if (UnityEngine.Random.Range(0, 2) == 0)
                sign = 1;
            float randomDistanceA = UnityEngine.Random.Range(30, 40);
            //if(Physics.Raycast(new Ray(UIGlobalVariablesScript.Singleton.main

            MoveTo(this.transform.position + new Vector3(UnityEngine.Random.Range(-40, 40), 0, randomDistanceA * sign), false);

			LastTimeToilet = DateTime.UtcNow;
		}
        
		if ((DateTime.UtcNow - LastGiftTime).TotalSeconds >= M_GIFT_TIME && !animationController.IsSleeping && animationController.IsIdle && !IsMovingTowardsLocation)
        {
            GetRandomItem();
			LastGiftTime = DateTime.UtcNow;
		}
        
        
        if (UIGlobalVariablesScript.Singleton.NonSceneRef != null && UIGlobalVariablesScript.Singleton.NonSceneRef.activeInHierarchy && UIGlobalVariablesScript.Singleton.MainCharacterRef.transform.localPosition.y <= -2)
        {

            UIGlobalVariablesScript.Singleton.MainCharacterRef.transform.localPosition = new Vector3(0, 1.01f, 0);
            UIGlobalVariablesScript.Singleton.MainCharacterRef.transform.rotation =
				Quaternion.Euler(0,
                UIGlobalVariablesScript.Singleton.MainCharacterRef.transform.rotation.eulerAngles.y,
                0);
            UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterControllerScript>().ResetRotation();
            Stop(true);
        }


		
        if (IsMovingTowardsLocation)
        {
            Vector3 direction = Vector3.Normalize(DestinationLocation - this.transform.position);

            CharacterControllerScript controller = this.gameObject.GetComponent<CharacterControllerScript>();

            controller.MovementDirection = direction;
            controller.RotateToLookAtPoint(DestinationLocation);

            if (Vector3.Distance(DestinationLocation, transform.position) <= 5)
            {
                Stop(true);
            }
            else
            {
                CharacterController charControl = GetComponent<CharacterController>();
                //Debug.Log("Velocity : ["+(int)charControl.velocity.magnitude+"|"+(int)controller.Movement.magnitude+"];");

                Vector3 proj = Vector3.Project(charControl.velocity, controller.Movement);
                if (proj.magnitude >= 1f && !m_HasStartedMoving)
                {
                    m_HasStartedMoving = true;
                } else if (proj.magnitude <= 1f && m_HasStartedMoving)
                    {
                    m_HasStartedMoving = false;
                    Stop(true);
                }
            }
        }
        else
        {
           m_HasStartedMoving = false;
        }
		 
        /*Remember to comment these back in when UI is working.
        UIGlobalVariablesScript.Singleton.HungryControlBarRef.transform.localPosition = new Vector3(Mathf.Lerp(-80.51972f, 617.2906f, ProfilesManagementScript.Singleton.CurrentAnimin.Hungry / 100.0f), UIGlobalVariablesScript.Singleton.HungryControlBarRef.transform.localPosition.y, 0);
        UIGlobalVariablesScript.Singleton.HealthControlBarRef.transform.localPosition = new Vector3(Mathf.Lerp(-80.51972f, 617.2906f, ProfilesManagementScript.Singleton.CurrentAnimin.Health / 100.0f), UIGlobalVariablesScript.Singleton.HealthControlBarRef.transform.localPosition.y, 0);
        UIGlobalVariablesScript.Singleton.HapynessControlBarRef.transform.localPosition = new Vector3(Mathf.Lerp(-80.51972f, 617.2906f, 
        ProfilesManagementScript.Singleton.CurrentAnimin.Happy / PersistentData.MaxHappy), UIGlobalVariablesScript.Singleton.HapynessControlBarRef.transform.localPosition.y, 0);
        UIGlobalVariablesScript.Singleton.FitnessControlBarRef.transform.localPosition = new Vector3(Mathf.Lerp(-80.51972f, 617.2906f, ProfilesManagementScript.Singleton.CurrentAnimin.Fitness / 100.0f), UIGlobalVariablesScript.Singleton.FitnessControlBarRef.transform.localPosition.y, 0);
        //UIGlobalVariablesScript.Singleton.EvolutionControlBarRef.GetComponent<UISlider>().value = Evolution / 100.0f;
		*/
        /*
		if((DateTime.UtcNow - LastSavePerformed).TotalSeconds >= 4)
		{
//            ProfilesManagementScript.Singleton.CurrentProfile.Characters[(int)ProfilesManagementScript.Singleton.CurrentProfile.ActiveAnimin].;
            SaveAndLoad.Instance.SaveAllData();
            LastSavePerformed = DateTime.UtcNow;
            Debug.Log("just saved...");
		}
*/
		/*
        if (Input.GetButtonDown("Fire1"))
            hadButtonDownLastFrame = true;
        else
            hadButtonDownLastFrame = false;
*/

        if (!Input.GetButton("Fire1"))
        {
            IsDetectingSwipeRight = false;
            SwipesDetectedCount = 0;
            SwipeHistoryPositions.Clear();
            AtLeastOneSwipeDetected = false;
            TouchesObjcesWhileSwiping.Clear();
        }
	

        //UICOMMENT: UIGlobalVariablesScript.Singleton.ZefTokensUI.text = ProfilesManagementScript.Singleton.CurrentAnimin.ZefTokens.ToString();
	
        //lastMousePosition = Input.mousePosition;
        DragedObjectedFromUIToWorld = false;
        lastActionId = CurrentAction;
        HadUITouchLastFrame = hadUItouch;
    }

    //	public void ShowText(string text)
    //	{
    //		TextTest.text = text;
    //		TextTest.color = new Color(1,1,1,1);
    //	}


    public void exitSleep()
    {
		LastTimeToilet = DateTime.UtcNow;
        Debug.Log("exit sleep");
		UiPages.GetPage(Pages.CaringPage).GetComponent<CaringPageControls>().TutorialHandler.TriggerAdHoc("WakeAnimin");
        animationController.IsSleeping = false;
        CurrentAction = ActionId.None;
        SleepBoundingBox.SetActive(false);
        UIGlobalVariablesScript.Singleton.SoundEngine.Play(ProfilesManagementScript.Instance.CurrentAnimin.PlayerAniminId, ProfilesManagementScript.Instance.CurrentAnimin.AniminEvolutionId, CreatureSoundId.SleepToIdle);
        UIGlobalVariablesScript.Singleton.SoundEngine.StopLoop();
		
//        UiPages.GetPage(Pages.CaringPage).GetComponent<CaringPageControls>().TutorialHandler.TriggerExitCond("Initial", "WakeUp");
    }




    public void PickupItem(GameObject item)
    {
        Debug.Log("void PickupItem(GameObject item)");
        UIGlobalVariablesScript.Singleton.SoundEngine.Play(GenericSoundId.ItemPickup);
        Stop(true);
        //Physics.IgnoreCollision(item.collider, this.collider, true);
        PutItemInHands(item);
//		Debug.Log("DISABLING COLLISION");
    }



    public void PutItemInHands(GameObject item)
    {
        //item.GetComponent<BoxCollider>().enabled = false;
        //if(item.collider.gameObject.activeInHierarchy)
        //item.transform.parent = GetComponent<CharacterSwapManagementScript>().CurrentModel.GetComponent<HeadReferenceScript>().ObjectCarryAttachmentBone.transform;

        ObjectHolding = item;

        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;
		
        /*item.transform.localPosition = new Vector3(2.137475f, 
		                                           -1.834323f, 
		                                           0.3105991f);
		
		item.transform.localRotation = Quaternion.Euler(44.08633f,
		                                                159.2195f,
		                                                -100.7192f);*/

    }

	public bool OnInteractWithPopupItem(Inventory.Entry entry)
    {
		switch (entry.Definition.ItemType)
        {
            case PopupItemType.Token:
                {
                    //Stop(true);
                    AudioController.Play("ZefToken");
                    TutorialHandler.TriggerAdHocStatic("PickupZef");
                    EvolutionManager.Instance.AddZef();
                    Debug.Log("TOKEN COLLECTED");

                    break;
                }
			
			case PopupItemType.Box:
			{
				if (entry.justSpawnedFromChest)
				{
					entry.MoveTo(Inventory.Locations.Inventory, Vector3.zero);
					Debug.Log("Box COLLECTED");
				}
				return false;
			}
            case PopupItemType.Food:
                {
                    /*if(Hungry >= 95)
			{
				animationController.IsNo = true;
				return false;
			}
			else
			{*/
                    //ShowText("yum yum");
					ProfilesManagementScript.Instance.CurrentAnimin.Hungry += entry.Definition.Points;

					if (entry.Definition.Id == InventoryItemId.Blueberry || entry.Definition.Id == InventoryItemId.Strawberry || entry.Definition.Id == InventoryItemId.watermelon)
                    {
                        AchievementManager.Instance.AddToAchievment(AchievementManager.Achievements.EatFruit);
                    }
                    //Stop(true);
						

		
                    //}
                    break;
                }

            case PopupItemType.Item:
                {
					/*if(entry.Definition.Id == InventoryItemId.FartButton)
					{
						UiPages.GetPage(Pages.CaringPage).GetComponent<CaringPageControls>().TutorialHandler.TriggerAdHoc("Fart");
						UIGlobalVariablesScript.Singleton.SoundEngine.PlayFart();
					}*/

                    return false;
                }

            case PopupItemType.Medicine:
                {
                    /*if(Health >= 95)
			{
				animationController.IsNo = true;
				return false;
			}
			else
			{*/


                    if (ProfilesManagementScript.Instance.CurrentAnimin.Health / PersistentData.MaxHealth <= 0.4f)
                    {
                        UiPages.GetPage(Pages.CaringPage).GetComponent<CaringPageControls>().TutorialHandler.TriggerAdHoc("HealFrom40");
                    }

                    //ShowText("I feel good");

					ProfilesManagementScript.Instance.CurrentAnimin.Health += entry.Definition.Points;
                    Stop(true);
                    animationController.IsTakingPill = true;
                    AchievementManager.Instance.AddToAchievment(AchievementManager.Achievements.Heal);


					if (entry.Definition.SpecialId == SpecialFunctionalityId.Injection)
                        UIGlobalVariablesScript.Singleton.SoundEngine.Play(ProfilesManagementScript.Instance.CurrentAnimin.PlayerAniminId, ProfilesManagementScript.Instance.CurrentAnimin.AniminEvolutionId, CreatureSoundId.InjectionReact);
                    else
                        UIGlobalVariablesScript.Singleton.SoundEngine.Play(ProfilesManagementScript.Instance.CurrentAnimin.PlayerAniminId, ProfilesManagementScript.Instance.CurrentAnimin.AniminEvolutionId, CreatureSoundId.EatPill);

                    //}
                    break;
                }

        }
		// And consume item
		ProfilesManagementScript.Instance.CurrentProfile.Inventory.Remove(entry);
        return true;
    }

    public void GiveMedicine(ItemId id)
    {
		
    }


    public void MoveTo(Vector3 location, bool run)
    {
        // Note can't trigger the move to point stuff here unless we also check that the user caused the move or stop the animin from moving until that tutorial has played
        // that's probably the best idea.
        TutorialHandler.TriggerAdHocStatic("MoveAnimin");
        if (Mathf.Abs(location.x) >= 180 || Mathf.Abs(location.z) >= 180)
            return;
        Debug.Log("Moving to point: " + location.ToString());
        IsMovingTowardsLocation = true;
	
        DestinationLocation = location;


        animationController.IsRunning = run;
        animationController.IsWalking = !run;

        GetComponent<CharacterControllerScript>().walkSpeed = 35;
        if (run)
            GetComponent<CharacterControllerScript>().walkSpeed = 120;

        Vector3 direction = Vector3.Normalize(location - this.transform.position);

//		Debug.Log(direction.ToString());
        Debug.DrawLine(this.transform.position, location, Color.red, 10);
        this.gameObject.GetComponent<CharacterControllerScript>().MovementDirection = direction;
    }

    public void Stop(bool stopMovingAsWell)
    {
        IsGoingToPickUpObject = null;

        if (animationController.RawAnimator == null)
        {
            Debug.Log("Animator not found!");
            if (stopMovingAsWell)
            {
                this.gameObject.GetComponent<CharacterControllerScript>().MovementDirection = Vector3.zero;
                IsMovingTowardsLocation = false;
            }
            return;
        }
        animationController.IsNotWell = false;
        animationController.IsHungry = false;
        InteractWithItemOnPickup = false;
        ShouldThrowObjectAfterPickup = false;

        /*sadUnwellLoopState = HungrySadUnwellLoopId.OnCooldown;

		if(TimeForNextHungryUnwellSadAnimation <= 1)
		{
			TimeForNextHungryUnwellSadAnimation += 1;
		}*/

        if (stopMovingAsWell)
        {
            animationController.IsWalking = false;
            animationController.IsRunning = false;
            this.gameObject.GetComponent<CharacterControllerScript>().MovementDirection = Vector3.zero;
            IsMovingTowardsLocation = false;
        }
    }

	public static void SwitchGravity(GameObject go, bool gravity)
	{		
		Rigidbody rb = go.GetComponent<Rigidbody>();
		if (rb != null) {
			Debug.Log ("Switch gravity " + go.name + " " + gravity);
			rb.useGravity = gravity;
		} else {
			Debug.Log ("Switch gravity " + go.name + " (No RB)");
		}
    }
}

public enum ActionId
{
    None = 0,
    EnterSleep,
    DetectMouseMoveAndDrag,
    DropItem,
    MoveToRequestedLocation,
    DragItemAround,
    DetectFlickAndThrow,
    EnterPortalToAR,
    EnterPortalToNonAR,
    SmallCooldownPeriod,
    EatItem,
    WaitEatingFinish,
    ThrowItemAfterPickup,
    ExitPortalMainStage,
}

public enum HungrySadUnwellLoopId
{
    DetectAnimation = 0,
    PlayHungry,
    PlaySad,
    PlayUnwell,
    OnCooldown,
}

//public enum AniminStateId
//{
//	Idle = 0,
//	GoingToTheToilet,
//	ToiletTime,
//	MovingToTapedLocation,
//	MovingToToiletLocation,
//}


public enum AchievementId
{
    None = 0,
    TestAchievent1,
    TestAchievent2,
    TestAchievent3,
    TestAchievent4,
    Count,
}

public enum ItemId
{
    None = 0,
    Strawberry,
}

