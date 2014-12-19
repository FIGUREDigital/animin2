using UnityEngine;
using System;
using System.Collections.Generic;

[System.Serializable]
public class ItemPickupSavedData
{
    public CharacterProgressScript ScriptRef;
    public Vector3 Position;
    public Vector3 Rotation;
    public bool WasInHands;


    public void RevertToThis()
    {
        if (WasInHands)
        {

        }
    }
}


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

    Count,
}

[System.Serializable]
public class InventoryItemBankData
{
    public InventoryItemId Id;
    public string PrefabId;
    public Sprite SpriteName;
    public PopupItemType ItemType;
}

[System.Serializable]
public class InventoryItemData
{
	
    private const string ITEM_PATH = "Texture/UI/";

    #region Static Nonsense

    public static InventoryItemBankData[] Items;

    public static void Initialize()
    {
        SpriteStore store = MainARHandler.Instance.SpriteStore;
        Items = new InventoryItemBankData[(int)InventoryItemId.Count];

        Items[(int)InventoryItemId.Strawberry] = new InventoryItemBankData() { Id = InventoryItemId.Strawberry, PrefabId = "Prefabs/Items/strawberry2", SpriteName = store.GetSprite(InventoryItemId.Strawberry), ItemType = PopupItemType.Food };
        Items[(int)InventoryItemId.Spinach] = new InventoryItemBankData() { Id = InventoryItemId.Spinach, PrefabId = "Prefabs/Items/spinach", SpriteName = store.GetSprite(InventoryItemId.Spinach),  ItemType = PopupItemType.Food };
        Items[(int)InventoryItemId.Blueberry] = new InventoryItemBankData() { Id = InventoryItemId.Blueberry, PrefabId = "Prefabs/Items/blueberry", SpriteName = store.GetSprite(InventoryItemId.Blueberry),  ItemType = PopupItemType.Food };
        Items[(int)InventoryItemId.AlmondMilk] = new InventoryItemBankData() { Id = InventoryItemId.AlmondMilk, PrefabId = "Prefabs/Items/almondMilk", SpriteName = store.GetSprite(InventoryItemId.AlmondMilk),  ItemType = PopupItemType.Food };
        Items[(int)InventoryItemId.Avocado] = new InventoryItemBankData() { Id = InventoryItemId.Avocado, PrefabId = "Prefabs/Items/avocado", SpriteName = store.GetSprite(InventoryItemId.Avocado),  ItemType = PopupItemType.Food };
        Items[(int)InventoryItemId.Carrot] = new InventoryItemBankData() { Id = InventoryItemId.Carrot, PrefabId = "Prefabs/Items/carrot", SpriteName = store.GetSprite(InventoryItemId.Carrot),  ItemType = PopupItemType.Food };
        Items[(int)InventoryItemId.Chips] = new InventoryItemBankData() { Id = InventoryItemId.Chips, PrefabId = "Prefabs/Items/chips", SpriteName = store.GetSprite(InventoryItemId.Chips),  ItemType = PopupItemType.Food };
        Items[(int)InventoryItemId.Toast] = new InventoryItemBankData() { Id = InventoryItemId.Toast, PrefabId = "Prefabs/Items/toast", SpriteName = store.GetSprite(InventoryItemId.Toast),  ItemType = PopupItemType.Food };
        Items[(int)InventoryItemId.watermelon] = new InventoryItemBankData() { Id = InventoryItemId.watermelon, PrefabId = "Prefabs/Items/watermelon", SpriteName = store.GetSprite(InventoryItemId.watermelon),  ItemType = PopupItemType.Food };
        Items[(int)InventoryItemId.Carrot] = new InventoryItemBankData() { Id = InventoryItemId.Carrot, PrefabId = "Prefabs/Items/Carrot", SpriteName = store.GetSprite(InventoryItemId.Carrot),  ItemType = PopupItemType.Food };
        Items[(int)InventoryItemId.Boombox] = new InventoryItemBankData() { Id = InventoryItemId.Boombox, PrefabId = "Prefabs/Items/boombox", SpriteName = store.GetSprite(InventoryItemId.Boombox),  ItemType = PopupItemType.Item };
        Items[(int)InventoryItemId.Clock] = new InventoryItemBankData() { Id = InventoryItemId.Clock, PrefabId = "Prefabs/Items/mintclock", SpriteName = store.GetSprite(InventoryItemId.Clock),  ItemType = PopupItemType.Item };
        Items[(int)InventoryItemId.EDMJuno] = new InventoryItemBankData() { Id = InventoryItemId.EDMJuno, PrefabId = "Prefabs/Items/juno", SpriteName = store.GetSprite(InventoryItemId.EDMJuno),  ItemType = PopupItemType.Item };
        Items[(int)InventoryItemId.EDM808] = new InventoryItemBankData() { Id = InventoryItemId.EDM808, PrefabId = "Prefabs/Items/808", SpriteName = store.GetSprite(InventoryItemId.EDM808),  ItemType = PopupItemType.Item };
        Items[(int)InventoryItemId.EDMKsynth] = new InventoryItemBankData() { Id = InventoryItemId.EDMKsynth, PrefabId = "Prefabs/Items/ksynth", SpriteName = store.GetSprite(InventoryItemId.EDMKsynth),  ItemType = PopupItemType.Item };
        Items[(int)InventoryItemId.Lightbulb] = new InventoryItemBankData() { Id = InventoryItemId.Lightbulb, PrefabId = "Prefabs/Items/Lightbulb", SpriteName = store.GetSprite(InventoryItemId.Lightbulb),  ItemType = PopupItemType.Item };
        Items[(int)InventoryItemId.paperCalendar] = new InventoryItemBankData() { Id = InventoryItemId.paperCalendar, PrefabId = "Prefabs/Items/paperCalendar", SpriteName = store.GetSprite(InventoryItemId.paperCalendar),  ItemType = PopupItemType.Item };
        Items[(int)InventoryItemId.Camera] = new InventoryItemBankData() { Id = InventoryItemId.Camera, PrefabId = "Prefabs/Items/camera", SpriteName = store.GetSprite(InventoryItemId.Camera),  ItemType = PopupItemType.Item };
        Items[(int)InventoryItemId.FartButton] = new InventoryItemBankData() { Id = InventoryItemId.FartButton, PrefabId = "Prefabs/Items/fartbutton", SpriteName = store.GetSprite(InventoryItemId.FartButton),  ItemType = PopupItemType.Item };
        Items[(int)InventoryItemId.woodSword] = new InventoryItemBankData() { Id = InventoryItemId.woodSword, PrefabId = "Prefabs/Items/woodSword", SpriteName = store.GetSprite(InventoryItemId.woodSword),  ItemType = PopupItemType.Item };
        Items[(int)InventoryItemId.woodFrame] = new InventoryItemBankData() { Id = InventoryItemId.woodFrame, PrefabId = "Prefabs/Items/woodFrame", SpriteName = store.GetSprite(InventoryItemId.woodFrame),  ItemType = PopupItemType.Item };
        Items[(int)InventoryItemId.Pill] = new InventoryItemBankData() { Id = InventoryItemId.Pill, PrefabId = "Prefabs/Items/capsule", SpriteName = store.GetSprite(InventoryItemId.Pill),  ItemType = PopupItemType.Medicine };
        Items[(int)InventoryItemId.Plaster] = new InventoryItemBankData() { Id = InventoryItemId.Plaster, PrefabId = "Prefabs/Items/plaster", SpriteName = store.GetSprite(InventoryItemId.Plaster),  ItemType = PopupItemType.Medicine };
        //Items[(int)InventoryItemId.Syringe] = new InventoryItemBankData() { Id = InventoryItemId.Syringe, PrefabId = "Prefabs/Items/syringe", SpriteName = store.GetSprite(InventoryItemId.Syringe),  ItemType = PopupItemType.Medicine };
        Items[(int)InventoryItemId.Radio] = new InventoryItemBankData() { Id = InventoryItemId.Radio, PrefabId = "Prefabs/Items/radio", SpriteName = store.GetSprite(InventoryItemId.Radio),  ItemType = PopupItemType.Item };
        Items[(int)InventoryItemId.Banana] = new InventoryItemBankData() { Id = InventoryItemId.Banana, PrefabId = "Prefabs/Items/banana", SpriteName = store.GetSprite(InventoryItemId.Banana) as Sprite,  ItemType = PopupItemType.Food };
        Items[(int)InventoryItemId.Peanut] = new InventoryItemBankData() { Id = InventoryItemId.Peanut, PrefabId = "Prefabs/Items/peanut", SpriteName = store.GetSprite(InventoryItemId.Peanut)  as Sprite,  ItemType = PopupItemType.Food };
        Items[(int)InventoryItemId.Beetroot] = new InventoryItemBankData() { Id = InventoryItemId.Beetroot, PrefabId = "Prefabs/Items/beetroot", SpriteName = store.GetSprite(InventoryItemId.Beetroot)  as Sprite,  ItemType = PopupItemType.Food };
        Items[(int)InventoryItemId.Chocolate] = new InventoryItemBankData() { Id = InventoryItemId.Chocolate, PrefabId = "Prefabs/Items/chocolate", SpriteName = store.GetSprite(InventoryItemId.Chocolate)  as Sprite,  ItemType = PopupItemType.Food };
        Items[(int)InventoryItemId.ChocoCake] = new InventoryItemBankData() { Id = InventoryItemId.ChocoCake, PrefabId = "Prefabs/Items/cakeChoco", SpriteName = store.GetSprite(InventoryItemId.ChocoCake)  as Sprite,  ItemType = PopupItemType.Food };
        Items[(int)InventoryItemId.CakeVanilla] = new InventoryItemBankData() { Id = InventoryItemId.CakeVanilla, PrefabId = "Prefabs/Items/cakeVanilla", SpriteName = store.GetSprite(InventoryItemId.CakeVanilla)  as Sprite,  ItemType = PopupItemType.Food };
        Items[(int)InventoryItemId.Pizza] = new InventoryItemBankData() { Id = InventoryItemId.Pizza, PrefabId = "Prefabs/Items/pizza", SpriteName = store.GetSprite(InventoryItemId.Pizza)  as Sprite,  ItemType = PopupItemType.Food };
        Items[(int)InventoryItemId.Noodles] = new InventoryItemBankData() { Id = InventoryItemId.Noodles, PrefabId = "Prefabs/Items/noodles", SpriteName = store.GetSprite(InventoryItemId.Noodles)  as Sprite,  ItemType = PopupItemType.Food };
        Items[(int)InventoryItemId.Kiwi] = new InventoryItemBankData() { Id = InventoryItemId.Kiwi, PrefabId = "Prefabs/Items/kiwi", SpriteName = store.GetSprite(InventoryItemId.Kiwi)  as Sprite,  ItemType = PopupItemType.Food };
        Items[(int)InventoryItemId.Cereal] = new InventoryItemBankData() { Id = InventoryItemId.Cereal, PrefabId = "Prefabs/Items/cereal", SpriteName = store.GetSprite(InventoryItemId.Cereal)  as Sprite,  ItemType = PopupItemType.Food };

    }

    #endregion

    public InventoryItemId Id;
    public int Count;
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


    public List<GameObject> GroundItems
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

    //public TextMesh TextTest;
    public AnimationControllerScript animationController;
    public bool IsMovingTowardsLocation;
    //public GameObject ObjectCarryAttachmentBone;
    private GameObject DragableObject;
    public GameObject ObjectHolding;
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

    ItemPickupSavedData pickupItemSavedData = new ItemPickupSavedData();
    RaycastHit moveHitInfo;
    RaycastHit detectDragHit;
    ActionId lastActionId;
    bool hadButtonDownLastFrame;
    bool IsDetectingMouseMoveForDrag;
    bool IsDetectFlick;
    float FeedMyselfTimer;
    public bool HadUITouchLastFrame;
    private GameObject LastKnownObjectWithMenuUp;
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
    
    private const float M_HEALTH_DEGREDATION = 0.1f;
    private const float M_HUNGER_DEGREDATION = 0.3f;
    private const float M_FITNESS_DEGREDATION = 0.4f;


    private GUITexture[] m_UITextures;

    private bool m_HasStartedMoving;

    // Use this for initialization
    void Awake()
    {
        LastSavePerformed = DateTime.Now;
        LastTimeToilet = DateTime.Now;

        InventoryItemData.Initialize();

	
        //ProfilesManagementScript.Singleton.CurrentAnimin.SetDefault();
        //ProfilesManagementScript.Singleton.CurrentAnimin.Load();

        Debug.Log("CharacterProgressScript AWAKE");
        Debug.Log("ProfilesManagementScript set : [" + ProfilesManagementScript.isSet + "];");
        if (ProfilesManagementScript.isSet == false)
        {
            ProfilesManagementScript.Singleton = new ProfilesManagementScript();
        }
        if (ProfilesManagementScript.Singleton.CurrentProfile == null)
        {
            Debug.Log("NO PROFILE FOUND");
            ProfilesManagementScript.Singleton.CurrentProfile = PlayerProfileData.CreateNewProfile("buildintest");
            ProfilesManagementScript.Singleton.CurrentAnimin = ProfilesManagementScript.Singleton.CurrentProfile.Characters[(int)PersistentData.TypesOfAnimin.Tbo];
        }

        //TextTest.color = new Color(1, 1, 1, 0.0f);

        Physics.IgnoreLayerCollision(
            LayerMask.NameToLayer("IgnoreCollisionWithCharacter"),   
            LayerMask.NameToLayer("Character"));

		
        animationController = GetComponent<AnimationControllerScript>();
        CurrentAction = ActionId.EnterSleep;

        GameObject[] gos = ProfilesManagementScript.Singleton.CurrentAnimin.LoadCaringScreenItem();
        if (gos != null)
            for (int i = 0; i < gos.Length; i++)
                GroundItems.Add(gos[i]);

        //animationController.IsSleeping = true;
        //CurrentAction = ActionId.Sleep;
        //SleepBoundingBox.SetActive(true);
    }


    void Start()
    {	

        UIClickButtonMasterScript.SetSoundSprite();

        if (ProfilesManagementScript.Singleton.CurrentAnimin == null)
        {
            ProfilesManagementScript.Singleton.CurrentAnimin = new PersistentData();
            ProfilesManagementScript.Singleton.CurrentAnimin.PlayerAniminId = PersistentData.TypesOfAnimin.Tbo;
            ProfilesManagementScript.Singleton.CurrentAnimin.AniminEvolutionId = AniminEvolutionStageId.Baby;
        }

        //HARRY: REMEMEBR TO REMOVE THESE COMMENTS, FOR CHRIST'S SAKE.
        Debug.Log("ID  : [" + ProfilesManagementScript.Singleton.CurrentAnimin + "|" + ProfilesManagementScript.Singleton.CurrentAnimin.AniminEvolutionId + "];");


        this.GetComponent<CharacterSwapManagementScript>().LoadCharacter(ProfilesManagementScript.Singleton.CurrentAnimin.PlayerAniminId, ProfilesManagementScript.Singleton.CurrentAnimin.AniminEvolutionId);
        //this.GetComponent<CharacterSwapManagementScript>().LoadCharacter(AniminId.Mandi   , AniminEvolutionStageId.Adult);

        /* OLD ADD ITEMS TO INVENTORY
        ProfilesManagementScript.Singleton.CurrentAnimin.AddItemToInventory(InventoryItemId.AlmondMilk, 3);
        ProfilesManagementScript.Singleton.CurrentAnimin.AddItemToInventory(InventoryItemId.Avocado, 1);
        ProfilesManagementScript.Singleton.CurrentAnimin.AddItemToInventory(InventoryItemId.Blueberry, 1);
        ProfilesManagementScript.Singleton.CurrentAnimin.AddItemToInventory(InventoryItemId.Boombox, 1);
        ProfilesManagementScript.Singleton.CurrentAnimin.AddItemToInventory(InventoryItemId.Camera, 1);
        ProfilesManagementScript.Singleton.CurrentAnimin.AddItemToInventory(InventoryItemId.Carrot, 1);
        ProfilesManagementScript.Singleton.CurrentAnimin.AddItemToInventory(InventoryItemId.Chips, 1);
        ProfilesManagementScript.Singleton.CurrentAnimin.AddItemToInventory(InventoryItemId.Clock, 4);
        ProfilesManagementScript.Singleton.CurrentAnimin.AddItemToInventory(InventoryItemId.EDM808, 1);
        ProfilesManagementScript.Singleton.CurrentAnimin.AddItemToInventory(InventoryItemId.EDMJuno, 1);
        ProfilesManagementScript.Singleton.CurrentAnimin.AddItemToInventory(InventoryItemId.EDMKsynth, 1);
        ProfilesManagementScript.Singleton.CurrentAnimin.AddItemToInventory(InventoryItemId.FartButton, 1);
        ProfilesManagementScript.Singleton.CurrentAnimin.AddItemToInventory(InventoryItemId.Lightbulb, 1);
        //AddItemToInventory(InventoryItemId.mintclock, 1);
        //AddItemToInventory(InventoryItemId.Noodles, 1);
        ProfilesManagementScript.Singleton.CurrentAnimin.AddItemToInventory(InventoryItemId.paperCalendar, 1);
        ProfilesManagementScript.Singleton.CurrentAnimin.AddItemToInventory(InventoryItemId.Pill, 1);
        ProfilesManagementScript.Singleton.CurrentAnimin.AddItemToInventory(InventoryItemId.Plaster, 1);
        ProfilesManagementScript.Singleton.CurrentAnimin.AddItemToInventory(InventoryItemId.Spinach, 1);
        ProfilesManagementScript.Singleton.CurrentAnimin.AddItemToInventory(InventoryItemId.Strawberry, 1);
        ProfilesManagementScript.Singleton.CurrentAnimin.AddItemToInventory(InventoryItemId.Toast, 1);
        ProfilesManagementScript.Singleton.CurrentAnimin.AddItemToInventory(InventoryItemId.watermelon, 1);
        ProfilesManagementScript.Singleton.CurrentAnimin.AddItemToInventory(InventoryItemId.woodFrame, 1);
        ProfilesManagementScript.Singleton.CurrentAnimin.AddItemToInventory(InventoryItemId.woodSword, 1);
        */


        for (int i = 0; i < ProfilesManagementScript.Singleton.CurrentAnimin.Inventory.Count; i++)
        {
            for (int j = 0; j < ProfilesManagementScript.Singleton.CurrentAnimin.Inventory.Count; j++)
            {
                if (ProfilesManagementScript.Singleton.CurrentAnimin.Inventory[i].Id == ProfilesManagementScript.Singleton.CurrentAnimin.Inventory[j].Id && i != j)
                {
                    Debug.Log("Duplicate found : [" + i + "|" + j + "]; ID :[" + ProfilesManagementScript.Singleton.CurrentAnimin.Inventory[i].Id + "];");
                }
            }
        }


        if (BetweenSceneData.Instance.ReturnFromMiniGame)
        {
            if (BetweenSceneData.Instance.Points >= 0)
            {
                if (BetweenSceneData.Instance.Points >= 15000)
                {
                    AchievementsScript.Singleton.Show(AchievementTypeId.Gold, BetweenSceneData.Instance.Points);
                    SpawnChests(3);
                }
                else if (BetweenSceneData.Instance.Points >= 5000)
                {
                    AchievementsScript.Singleton.Show(AchievementTypeId.Silver, BetweenSceneData.Instance.Points);
                    SpawnChests(2);
                }
                else if (BetweenSceneData.Instance.Points >= 800)
                {
                    AchievementsScript.Singleton.Show(AchievementTypeId.Bronze, BetweenSceneData.Instance.Points);
                    SpawnChests(1);
                }
                BetweenSceneData.Instance.ResetPoints();
            }
            if (BetweenSceneData.Instance.minigame == BetweenSceneData.Minigame.Collector)
            {
                UiPages.GetPage(Pages.CaringPage).GetComponent<CaringPageControls>().TutorialHandler.TriggerAdHocStartCond("BoxLandReturn");
                if (BetweenSceneData.Instance.Points >= 7000)
                {
                    UiPages.GetPage(Pages.CaringPage).GetComponent<CaringPageControls>().TutorialHandler.TriggerAdHocStartCond("BoxLandScoreBreak1");
                }
            }
        }

		
        if (DateTime.Now.Subtract(ProfilesManagementScript.Singleton.CurrentAnimin.CreatedOn).Days >= 1)
        {
            if (UiPages.GetPage(Pages.CaringPage).GetComponent<CaringPageControls>().TutorialHandler != null)
                UiPages.GetPage(Pages.CaringPage).GetComponent<CaringPageControls>().TutorialHandler.TriggerAdHocStartCond("1DayEvolve");
        }
        if (DateTime.Now.Subtract(ProfilesManagementScript.Singleton.CurrentAnimin.CreatedOn).Days >= 3)
        {
            if (UiPages.GetPage(Pages.CaringPage).GetComponent<CaringPageControls>().TutorialHandler != null)
                UiPages.GetPage(Pages.CaringPage).GetComponent<CaringPageControls>().TutorialHandler.TriggerAdHocStartCond("3DayEvolve");
        }
		



    }
    void CleanUpItems(){
        for (int i = 0; i < groundItemsOnARscene.Count; i++)
        {
            ProfilesManagementScript.Singleton.CurrentAnimin.AddItemToInventory(groundItemsOnARscene[i].GetComponent<UIPopupItemScript>().Id, 1);
        }

        Debug.Log("ON DESTROYED! groundItemsOnNonARScene : [" + groundItemsOnNonARScene.Count + "];");
        ProfilesManagementScript.Singleton.CurrentAnimin.SaveCaringScreenItem(groundItemsOnNonARScene.ToArray());
        SaveAndLoad.Instance.SaveAllData();
    }
    void OnDestroy()
    {
        CleanUpItems();
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

    public GameObject SpawnStageItem(string prefabId, Vector3 position)
    {

        GameObject resource = Resources.Load<GameObject>(prefabId);
		
        GameObject gameObject = GameObject.Instantiate(resource) as GameObject;
        gameObject.transform.parent = ActiveWorld.transform;
		
        gameObject.transform.localPosition = position;
        //gameObject.transform.localRotation = Quaternion.Euler(0, UnityEngine.Random.Range(-180, 180), 0);

        UIPopupItemScript scriptRef = gameObject.GetComponent<UIPopupItemScript>();

        float scale = 0.1f;
        gameObject.transform.localScale = new Vector3(scale, scale, scale);

		Debug.Log ("If somebody sees this, please tell Harry. Tell him [" + gameObject.name + "]; was a thing.");
        gameObject.transform.localRotation = Quaternion.Euler(0, UnityEngine.Random.Range(130, 230), 0);

        GroundItems.Add(gameObject);
        return gameObject;
    }

    public GameObject SpawnZef(Vector3 position)
    {
        CharacterProgressScript script = UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>();

        GameObject resource = Resources.Load<GameObject>(@"Prefabs/ZefToken");
		
        GameObject gameObject = GameObject.Instantiate(resource) as GameObject;
        gameObject.transform.parent = UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>().ActiveWorld.transform;
		
        gameObject.transform.localPosition = position;
        //gameObject.transform.localRotation = Quaternion.Euler(0, UnityEngine.Random.Range(-180, 180), 0);
		
        script.GroundItems.Add(gameObject);
        return gameObject;
    }


    public GameObject GetRandomItem()
    {
        List<GameObject> list = new List<GameObject>();
		
        for (int i = 0; i < GroundItems.Count; ++i)
        {
            //if(GroundItems[i].GetComponent<ReferencedObjectScript>() == null) continue;
			
            UIPopupItemScript itemData = GroundItems[i]/*.GetComponent<ReferencedObjectScript>().Reference*/.GetComponent<UIPopupItemScript>();
            if (itemData == null)
                continue;

            if (itemData.Type == PopupItemType.Item)
            {
                list.Add(GroundItems[i]);
            }
        }

        if (list.Count > 0)
            return list[UnityEngine.Random.Range(0, list.Count)];
        else
            return null;
    }

    public void ThrowItemFromHands(Vector3 throwdirection)
    {
        //DragableObject = ObjectHolding;
        animationController.IsHoldingItem = false;

        this.gameObject.GetComponent<CharacterControllerScript>().RotateToLookAtPoint(this.transform.position + throwdirection * 50);
		
		
        //pickupItemSavedData.Position = DragableObject.transform.position;
        //pickupItemSavedData.Rotation = DragableObject.transform.rotation.eulerAngles;
		
        ObjectHolding.transform.parent = ActiveWorld.transform;

		
        GetComponent<CharacterSwapManagementScript>().CurrentModel.GetComponent<HeadReferenceScript>().HoldingObject = null;
		
        ThrowAnimationScript throwScript = ObjectHolding.AddComponent<ThrowAnimationScript>();
        float maxDistance = Vector3.Distance(Input.mousePosition, MousePositionAtDragIfMouseMoves) * 0.35f;
        if (maxDistance >= 160)
            maxDistance = 160;
		
        throwScript.Begin(throwdirection, maxDistance);

        UIGlobalVariablesScript.Singleton.SoundEngine.Play(ProfilesManagementScript.Singleton.CurrentAnimin.PlayerAniminId, ProfilesManagementScript.Singleton.CurrentAnimin.AniminEvolutionId, CreatureSoundId.Throw);
        //ObjectHolding.transform.position = this.transform.position;
		
		
		
		
        GroundItems.Add(ObjectHolding);
		
        ObjectHolding.transform.rotation = Quaternion.Euler(0, ObjectHolding.transform.rotation.eulerAngles.y, 0);
        pickupItemSavedData.WasInHands = true;
        animationController.IsThrowing = true;
        IsDetectFlick = false;
        ObjectHolding = null;
        //CurrentAction = ActionId.None;
    }

    public void HidePopupMenus()
    {
        Debug.Log("Hiding popup menus");
        //UICOMMENT
        CaringPageControls.StereoUI.SetActive(false);
        CaringPageControls.AlarmUI.SetActive(false);
        CaringPageControls.PianoUI.SetActive(false);
        CaringPageControls.LightbulbUI.SetActive(false);
        CaringPageControls.EDMBoxUI.SetActive(false);
        CaringPageControls.JunoUI.SetActive(false);
        CaringPageControls.PianoUI.SetActive(false);
    }

    private GameObject GetClosestFoodToEat()
    {
        GameObject closestFood = null;

        //Debug.Log("GROUND ITEMs: " + GroundItems.Count.ToString());

        for (int i = 0; i < GroundItems.Count; ++i)
        {
            if (GroundItems[i] == null)
                continue;
            if (GroundItems[i].GetComponent<UIPopupItemScript>() == null)
                continue;

            UIPopupItemScript itemData = GroundItems[i].GetComponent<UIPopupItemScript>();
            if (itemData.Type == PopupItemType.Food)
            {
                if (closestFood == null)
                {
                    closestFood = GroundItems[i];
                }
                else
                {
                    if (Vector3.Distance(this.transform.position, GroundItems[i].transform.position) < Vector3.Distance(this.transform.position, closestFood.transform.position))
                    {
                        closestFood = GroundItems[i];
                    }
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
        string prefab = "Prefabs/chest_bronze";
        switch (value)
        {
            case 1:
                prefab = "Prefabs/chest_bronze";
                break;
            case 2:
                prefab = "Prefabs/chest_silver";
                break;
            case 3:
                prefab = "Prefabs/chest_gold";
                break;
            case 4:
                prefab = "Prefabs/chest_evo";
                break;
            case 0:
            default:
                return null;
                break;
        }
        Debug.Log("Spawn Chests");
        GameObject resource = Resources.Load<GameObject>(prefab);

        GameObject instance = Instantiate(resource) as GameObject;
        instance.transform.parent = ActiveWorld.transform;
        instance.transform.rotation = Quaternion.Euler(0, UnityEngine.Random.Range(170, 190), 0);
        instance.transform.position = Vector3.zero;
        instance.transform.localPosition = new Vector3(UnityEngine.Random.Range(-0.67f, 0.67f), this.transform.localPosition.y, UnityEngine.Random.Range(-0.67f, 0.67f));
        instance.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

        return instance;
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
        UIGlobalVariablesScript.Singleton.SoundEngine.Play(ProfilesManagementScript.Singleton.CurrentAnimin.PlayerAniminId, ProfilesManagementScript.Singleton.CurrentAnimin.AniminEvolutionId, CreatureSoundId.JumbOutPortal);
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
	
    // Update is called once per frame
    void Update()
    {

        EvolutionManager.Instance.UpdateEvo();


        ProfilesManagementScript.Singleton.CurrentAnimin.Hungry -= Time.deltaTime * M_HUNGER_DEGREDATION;
        ProfilesManagementScript.Singleton.CurrentAnimin.Fitness -= Time.deltaTime * M_FITNESS_DEGREDATION;
        ProfilesManagementScript.Singleton.CurrentAnimin.Health -= Time.deltaTime * M_HEALTH_DEGREDATION;
	
        //TextTest.color = new Color(1,1,1, TextTest.color.a - Time.deltaTime * 0.6f);
        //if(TextTest.color.a < 0)
        //	TextTest.color = new Color(1,1,1, 0);


        ProfilesManagementScript.Singleton.CurrentAnimin.Happy = ((
            (ProfilesManagementScript.Singleton.CurrentAnimin.Hungry / 100.0f) +
            (ProfilesManagementScript.Singleton.CurrentAnimin.Fitness / 100.0f) +
            (ProfilesManagementScript.Singleton.CurrentAnimin.Health / 100.0f))
        / 3.0f)
        * PersistentData.MaxHappy;


        //Debug.Log("Hungry: " + (Hungry / 100.0f).ToString());
        //Debug.Log("Fitness: " + (Fitness / 100.0f).ToString());
        //Debug.Log("Health: " + (Health / 100.0f).ToString());
        //Debug.Log("Happy: " + (Happy / MaxHappy).ToString());
        // EVOLUTION BAR
//		{
//			//Evolution += (Happy / MaxHappy) * Time.deltaTime * 0.1f;
//			//if(Evolution >= 100) Evolution = 100;
//
        float percentage = ProfilesManagementScript.Singleton.CurrentAnimin.Evolution;
        Sprite EvoProgress = UIGlobalVariablesScript.Singleton.EvolutionProgressSprite;
        float scale = UIGlobalVariablesScript.Singleton.gameObject.transform.localScale.x;
        int screenWidth = 1330;
        int width = (int)(screenWidth * percentage);
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
//			if(NextHappynBonusTimeAt >= DateTime.Now)
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
            /* OLD NGUI HADUITOUCH CODE
            // This grabs the camera attached to the NGUI UI_Root object.
            Camera uiCam = Camera.main;
			
            if (uiCam != null)
            {
                // pos is the Vector3 representing the screen position of the input
				Ray inputRay = uiCam.ScreenPointToRay(Input.mousePosition);  
								Debug.DrawRay(uiCam.transform.position ,uiCam.ScreenPointToRay(Input.mousePosition).direction*1000,Color.green,5,false); 



                RaycastHit hit;
                if (Physics.Raycast(inputRay, out hit))
				{
					Debug.Log("Colliding with : [" + hit.collider.gameObject.name + "];");
                    //Debug.Log("TOUCH: " + hit.collider.gameObject.layer.ToString());

                    //Debug.Log("normalUIRAY:" + hit.collider.gameObject);
                    if (hit.collider.gameObject.layer == LayerMask.NameToLayer("NGUI"))
                    {
                        //Debug.Log("normalUIRAY +++++:" + hit.collider.gameObject);
                        hadUItouch = true;
                        //Debug.Log("UI TOUCH");
                    }

                }
            }
            */
            /*
            GUITexture[] UITextures = GameObject.FindObjectsOfType<GUITexture>();
            Debug.Log("Testing : [" + UITextures.Length + "];");
            for (int i = 0; i < UITextures.Length; i++)
            {
                if (UITextures[i].HitTest(Input.mousePosition)){
                    hadUItouch = true;
                    break;
                }
            }
            */
            //ADRIAN. We need to fill this out so that it can tell us Yes/No whether or not the mouse is currently over the UI. Apparently "GUIElement.HitTest(Vector3)" will work, but we need to find a way to get the UITextures to work. Perhaps we should parent all the UIs to a single object which we can access with a singleton?
            hadUItouch = UiPages.IsMouseOverUI();
        }


		
        RaycastHit hitInfo;
        bool hadRayCollision = false;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hitInfo))
        {
            //Debug.Log("TUTORIAL PLAYING? : ["+UiPages.GetPage(Pages.CaringPage).GetComponent<CaringPageControls>().TutorialHandler.IsPlaying+"]");
            //if (UiPages.GetPage(Pages.CaringPage).GetComponent<CaringPageControls>().TutorialHandler.IsPlaying)
            //{
            //    hadRayCollision = UiPages.GetPage(Pages.CaringPage).GetComponent<CaringPageControls>().TutorialHandler.CheckCharacterProgress(this, hitInfo);
            //}
            //else
            hadRayCollision = true;
            //Debug.Log ("Ray Collision : ["+hitInfo.collider.gameObject.name+"];");
        }



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
                    if (animationController.IsHoldingItemComplete)
                    {
                        Vector3 throwdirection = Vector3.Normalize(new Vector3(UnityEngine.Random.Range(-1.0f, 1.0f), 0, UnityEngine.Random.Range(-1.0f, 1.0f)));
                        ThrowItemFromHands(throwdirection);
                        CurrentAction = ActionId.None;
                    }


                    break;
                }

            case ActionId.EatItem:
                {

                    CurrentAction = ActionId.WaitEatingFinish;
                    animationController.IsHoldingItem = false;
                    animationController.IsEating = true;
                    EatAlphaTimer = 0;

                    PlayedEatingSound = false;

                    break;
                }
            case ActionId.WaitEatingFinish:
                {
                    if (!animationController.IsEating)
                    {

                        PopupItemType itemType = ObjectHolding./*GetComponent<ReferencedObjectScript>().Reference.*/GetComponent<UIPopupItemScript>().Type;

                        Debug.Log("FINISHED EATING");

                        OnInteractWithPopupItem(ObjectHolding./*GetComponent<ReferencedObjectScript>().Reference.*/GetComponent<UIPopupItemScript>());
                        this.GetComponent<CharacterProgressScript>().GroundItems.Remove(ObjectHolding);
                        Destroy(ObjectHolding);
				

                        pickupItemSavedData.WasInHands = true;
                        ObjectHolding = null;
                        CurrentAction = ActionId.None;
                    }
                    else
                    {
                        EatAlphaTimer += Time.deltaTime;

                        if (EatAlphaTimer >= 0.7f)
                        {
                            if (!PlayedEatingSound)
                            {
                                UIPopupItemScript popup = ObjectHolding/*.GetComponent<ReferencedObjectScript>().Reference*/.GetComponent<UIPopupItemScript>();

                                PlayedEatingSound = true;
                                if (popup.SpecialId == SpecialFunctionalityId.Liquid)
                                    UIGlobalVariablesScript.Singleton.SoundEngine.Play(ProfilesManagementScript.Singleton.CurrentAnimin.PlayerAniminId, ProfilesManagementScript.Singleton.CurrentAnimin.AniminEvolutionId, CreatureSoundId.FeedDrink);
                                else
                                    UIGlobalVariablesScript.Singleton.SoundEngine.Play(ProfilesManagementScript.Singleton.CurrentAnimin.PlayerAniminId, ProfilesManagementScript.Singleton.CurrentAnimin.AniminEvolutionId, CreatureSoundId.FeedFood);

                            }
                        }

                        if (EatAlphaTimer >= 1)
                        {
                            if (ObjectHolding.renderer != null)
                            {
                                ObjectHolding.renderer.material.shader = Shader.Find("Custom/ItemShader");

                                float alpha = ObjectHolding.renderer.material.color.a;
                                alpha -= Time.deltaTime * 3;
                                if (alpha <= 0)
                                    alpha = 0;
                                ObjectHolding.renderer.material.color = new Color(
                                    ObjectHolding.renderer.material.color.r,
                                    ObjectHolding.renderer.material.color.g,
                                    ObjectHolding.renderer.material.color.b,
                                    alpha);
                            }

                            for (int a = 0; a < ObjectHolding.transform.childCount; ++a)
                            {
                                if (ObjectHolding.transform.GetChild(a).renderer == null)
                                    continue;

                                ObjectHolding.transform.GetChild(a).renderer.material.shader = Shader.Find("Custom/ItemShader");
						
                                float alpha = ObjectHolding.transform.GetChild(a).renderer.material.color.a;
                                alpha -= Time.deltaTime * 3;
                                if (alpha <= 0)
                                    alpha = 0;
                                ObjectHolding.transform.GetChild(a).renderer.material.color = new Color(
                                    ObjectHolding.transform.GetChild(a).renderer.material.color.r,
                                    ObjectHolding.transform.GetChild(a).renderer.material.color.g,
                                    ObjectHolding.transform.GetChild(a).renderer.material.color.b,
                                    alpha);
                            }
                        }

                    }

                    break;
                }


            case ActionId.ExitPortalMainStage:
                {
                    Debug.Log("ExitPortalmainStage");

                    ShaderAlpha = 1;
			
                    UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<AnimateCharacterOutPortalScript>().Timer = 0;
                    UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<AnimateCharacterOutPortalScript>().JumbId = AnimateCharacterOutPortalScript.JumbStateId.Jumbout;

                    if (UIGlobalVariablesScript.Singleton.ARSceneRef.activeInHierarchy)
                        UIGlobalVariablesScript.Singleton.ARPortal.GetComponent<PortalScript>().Show(PortalStageId.ARscene, false);
                    else
                        UIGlobalVariablesScript.Singleton.ARPortal.GetComponent<PortalScript>().Show(PortalStageId.NonARScene, false);
			
                    UIGlobalVariablesScript.Singleton.SoundEngine.Play(ProfilesManagementScript.Singleton.CurrentAnimin.PlayerAniminId, ProfilesManagementScript.Singleton.CurrentAnimin.AniminEvolutionId, CreatureSoundId.JumbOutPortal);

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
                        GetComponent<CharacterSwapManagementScript>().CurrentModel.gameObject.SetActive(true);
                        CurrentAction = ActionId.None;
                        MainARHandler.Instance.OnCharacterEnterARScene();

                        m_JumpedIn = false;
                    }
                    else if (PortalTimer >= StopAt)
                    {
                        if (!m_JumpedIn)
                        {
                            m_JumpedIn = true;
                            GetComponent<CharacterSwapManagementScript>().CurrentModel.gameObject.SetActive(false);

                            MainARHandler.Instance.PauseJumpOutIntoAR();

                            UIGlobalVariablesScript.Singleton.NonSceneRef.SetActive(false);
                            UIGlobalVariablesScript.Singleton.ARSceneRef.SetActive(true);


                            UIGlobalVariablesScript.Singleton.ARPortal.GetComponent<PortalScript>().Show(PortalStageId.ARscene, false);

                            UIGlobalVariablesScript.Singleton.SoundEngine.Play(ProfilesManagementScript.Singleton.CurrentAnimin.PlayerAniminId, ProfilesManagementScript.Singleton.CurrentAnimin.AniminEvolutionId, CreatureSoundId.JumbOutPortal);
                        }
                    }
                    else if (PortalTimer >= StartFade)
                    {
                        /*
                        Color c = ShaderColor;

                        float diff = PortalTimer * (1 / (StopAt - StartFade));
                        float alpha = 1 - diff;
                        c.a = alpha;


                        Debug.Log("Diff : [" + diff + "]; Alpha : [" + alpha + "];");

                        ShaderColor = c;
                        */
                        //ShaderColor = Color.Lerp(ShaderColor, Color.clear, 0.5f);
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
                    CurrentAction = ActionId.Sleep;
                    SleepBoundingBox.SetActive(true);
                    UIGlobalVariablesScript.Singleton.SoundEngine.PlayLoop(ProfilesManagementScript.Singleton.CurrentAnimin.PlayerAniminId, ProfilesManagementScript.Singleton.CurrentAnimin.AniminEvolutionId, CreatureSoundId.SnoringSleeping);

                    break;
                }

            case ActionId.Sleep:
                {
                    if (Input.GetButtonUp("Fire1"))
                    {
                        if (!hadUItouch && hadRayCollision && hitInfo.collider.gameObject == SleepBoundingBox)
                        {
                            exitSleep();
						
                        }
                    }

                    break;
                }
			
            case ActionId.None:
                {
                    //Debug.Log("INSIDE NONE");

                    if (Input.GetButton("Fire1"))
                        HoldingLeftButtonDownTimer += Time.deltaTime;
                    else
                        HoldingLeftButtonDownTimer = 0;

							
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
                                IsDetectFlick = true;
                                //CurrentAction = ActionId.DetectFlickAndThrow;
                                MousePositionAtDragIfMouseMoves = Input.mousePosition;
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
                        //Debug.Log("IsDetectingMouseMoveForDrag");

                        #region Some Commented Out Code
					
                        // DRAG ITEM FROM CHARACTER AWAY FROM HIM
                        /*if(hit.collider.name.StartsWith("MainCharacter") && DragableObject == null && animationController.IsHoldingItem)
					{
						//pickupItemSavedData.Position = DragableObject.transform.position;
						//pickupItemSavedData.Rotation = DragableObject.transform.rotation.eulerAngles;
						//pickupItemSavedData.WasInHands = true;

						//Debug.Log("SELECTED OBJECT:" + hit.collider.name);
						//DragableObject = ObjectHolding;
						//DragableObject.GetComponent<BoxCollider>().enabled = false;
						DragableObject.transform.parent = this.transform.parent;
						animationController.IsHoldingItem = false;
						animationController.IsThrowing = true;
						//Physics.IgnoreCollision(DragableObject.collider, this.collider, true);


					}
					
					// GRAB ITEM ITSELF EITHER FROM HANDS OR FLOOR
					else*/ 
					
                        // THROW IT AWAY
                        //if(animationController.IsHoldingItem)
                        //{
                        //	CurrentAction = ActionId.DetectFlickAndThrow;
                        //}
					
                        // GRAB FROM FLOOR
                        //else
                        #endregion
					

                        DragableObject = detectDragHit.collider.gameObject;
					
                        if (DragableObject.GetComponent<UIPopupItemScript>().Type != PopupItemType.Token)
                        {

                            pickupItemSavedData.WasInHands = false;
                            //Debug.Log("IT SHOULD GO AND DRAG NOW");
                            pickupItemSavedData.WasInHands = false;
						
                            pickupItemSavedData.Position = DragableObject.transform.position;
                            pickupItemSavedData.Rotation = DragableObject.transform.rotation.eulerAngles;
                            //Physics.IgnoreCollision(DragableObject.collider, this.collider, true);
                            //Debug.Log("DISABLING COLLISION");
						
                            CurrentAction = ActionId.DragItemAround;
                            IsDetectingMouseMoveForDrag = false;

                            DragableObject.layer = LayerMask.NameToLayer("IgnoreCollisionWithCharacter");
                            DragableObject.GetComponent<BoxCollider>().enabled = false;
                        }

                    }
                    else if (Input.GetButton("Fire1"))
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
                                if (TouchesObjcesWhileSwiping[i].tag == "Shit" || TouchesObjcesWhileSwiping[i].tag == "Items")
                                {
                                    if (TouchesObjcesWhileSwiping[i].tag == "Shit")
                                        cleanedShit = true;

                                    if (TouchesObjcesWhileSwiping[i].GetComponent<UIPopupItemScript>() != null)
                                    {
                                        ProfilesManagementScript.Singleton.CurrentAnimin.AddItemToInventory(TouchesObjcesWhileSwiping[i].GetComponent<UIPopupItemScript>().Id, 1);
                                    }

                                    if (TouchesObjcesWhileSwiping[i].GetComponent<EDMBoxScript>() != null)
                                    {
                                        TouchesObjcesWhileSwiping[i].GetComponent<EDMBoxScript>().Stop();
                                    }
							   
                                    GroundItems.Remove(TouchesObjcesWhileSwiping[i]);
                                    Destroy(TouchesObjcesWhileSwiping[i]);
                                    TouchesObjcesWhileSwiping.RemoveAt(i);

                                    Debug.Log("SwipeDetected!");
                                    HidePopupMenus();

                                    if (ObjectHolding == hitInfo.collider.gameObject)
                                    {
                                        ObjectHolding = null;
                                        animationController.IsHoldingItem = false;
                                    }

                                    i--;
                                }
                            }

                            if (cleanedShit)
                            {
                                UIGlobalVariablesScript.Singleton.SoundEngine.Play(GenericSoundId.CleanPooPiss);
                                UiPages.GetPage(Pages.CaringPage).GetComponent<CaringPageControls>().TutorialHandler.TriggerAdHocStartCond("CleanPiss");
                            }
												
                            if (TouchesObjcesWhileSwiping.Contains(this.gameObject) && !cleanedShit && !animationController.IsTickled)
                            {
                                Stop(true);
                                animationController.IsTickled = true;

                                UIGlobalVariablesScript.Singleton.SoundEngine.Play(ProfilesManagementScript.Singleton.CurrentAnimin.PlayerAniminId, ProfilesManagementScript.Singleton.CurrentAnimin.AniminEvolutionId, 
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


                        //if (UIGlobalVariablesScript.Singleton.DragableUI3DObject.transform.childCount == 1 && UIGlobalVariablesScript.Singleton.DragableUI3DObject.transform.GetChild(0).name == "Broom")
                        if (CameraModelScript.Instance.transform.childCount == 1 && CameraModelScript.Instance.transform.GetChild(00).name == "Broom")
                        {
                            if (hadRayCollision && (hitInfo.collider.tag == "Items" || hitInfo.collider.tag == "Shit") && GroundItems.Contains(hitInfo.collider.gameObject))
                            {



                                GroundItems.Remove(hitInfo.collider.gameObject);
                                Destroy(hitInfo.collider.gameObject);
                            }
                        }
                        else if (!AtLeastOneSwipeDetected && hadRayCollision/* && !TriggeredHoldAction*/)
                        {

                            if (hitInfo.collider.name.StartsWith("MainCharacter") || hitInfo.collider.gameObject == ObjectHolding)
                            {
                                //Debug.Log("HIT THE CHARACTER FOR INTERACTION");

                                if (ObjectHolding != null && !ObjectHolding./*GetComponent<ReferencedObjectScript>().Reference.*/GetComponent<UIPopupItemScript>().NonInteractable)
                                {
                                    //Debug.Log("HIT THE CHARACTER FOR INTERACTION 2");

                                    UIPopupItemScript item = ObjectHolding./*GetComponent<ReferencedObjectScript>().Reference.*/GetComponent<UIPopupItemScript>();


                                    if (item.Type == PopupItemType.Food)
                                    {
                                        //Debug.Log("HIT THE CHARACTER FOR INTERACTION 3");

                                        this.GetComponent<CharacterProgressScript>().CurrentAction = ActionId.EatItem;
                                    }
                                    else if (OnInteractWithPopupItem(item))
                                    {
                                        //Debug.Log("HIT THE CHARACTER FOR INTERACTION 4");
                                        Destroy(ObjectHolding);
                                        ObjectHolding = null;
                                        animationController.IsHoldingItem = false;
                                    }

                                }
                                else if (ObjectHolding != null && ObjectHolding/*.GetComponent<ReferencedObjectScript>().Reference*/.GetComponent<UIPopupItemScript>().NonInteractable)
                                {
                                    //Debug.Log("HIT THE CHARACTER FOR INTERACTION 3");
                                    UiPages.GetPage(Pages.CaringPage).GetComponent<CaringPageControls>().TutorialHandler.TriggerAdHocStartCond("Fart");
                                    UIGlobalVariablesScript.Singleton.SoundEngine.PlayFart();
                                }
                                else if (ObjectHolding == null && CameraModelScript.Instance.transform.childCount == 0 && !animationController.IsPat)
                                {
                                    //Debug.Log("HIT THE CHARACTER FOR INTERACTION 4");
                                    Stop(true);
                                    animationController.IsPat = true;
                                    //Debug.Log("IS TICKLED");
								
                                    UIGlobalVariablesScript.Singleton.SoundEngine.Play(ProfilesManagementScript.Singleton.CurrentAnimin.PlayerAniminId, ProfilesManagementScript.Singleton.CurrentAnimin.AniminEvolutionId, CreatureSoundId.PatReact);
                                }
                                Debug.Log("Tap");
                                //UiPages.GetPage(Pages.CaringPage).GetComponent<CaringPageControls>().TutorialHandler.TriggerAdHocExitCond("Attention", "tap");
                            }
                            else if ((hitInfo.collider.tag == "Items") && hitInfo.collider/*.GetComponent<ReferencedObjectScript>().Reference*/.GetComponent<UIPopupItemScript>().Type == PopupItemType.Token)
                            {
                                OnInteractWithPopupItem(hitInfo.collider./*GetComponent<ReferencedObjectScript>().Reference.*/GetComponent<UIPopupItemScript>());
                                this.GetComponent<CharacterProgressScript>().GroundItems.Remove(hitInfo.collider.gameObject);
                                Destroy(hitInfo.collider.gameObject);
                            }
                            else if (hitInfo.collider.name.StartsWith("Invisible Ground Plane") || (hitInfo.collider.tag == "Items"))
                            {
                                //float distane = Vector3.Distance(hitInfo.point, this.transform.position);
                                //MoveTo(hitInfo.point, distane > 220.0f ? true : false);
                                if (RequestedToMoveToCounter == 0)
                                    RequestedTime = Time.time;
                                RequestedToMoveToCounter++;
                                moveHitInfo = hitInfo;

                                if (ObjectHolding != null && (hitInfo.collider.tag == "Items"))
                                {
                                    ObjectHolding.layer = LayerMask.NameToLayer("Default");
                                    ObjectHolding.transform.parent = ActiveWorld.transform;

                                    GetComponent<CharacterSwapManagementScript>().CurrentModel.GetComponent<HeadReferenceScript>().HoldingObject = null;

                                    ObjectHolding.transform.localPosition = new Vector3(ObjectHolding.transform.localPosition.x, 0, ObjectHolding.transform.localPosition.z);

                                    GroundItems.Add(ObjectHolding);
                                    ObjectHolding = null;
                                    animationController.IsHoldingItem = false;

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

                    //Debug.Log("END OF NONE");


                    if (RequestedToMoveToCounter > 0)
                    {
                        if ((Time.time - RequestedTime) >= 0.17f)
                        {
                            Stop(true);

                            bool preventMovingTo = false;
                            Vector3 point = moveHitInfo.point;
                            if (moveHitInfo.collider.tag == "Items")
                            {
                                moveHitInfo.collider.gameObject.AddComponent<FlashObjectScript>();

                                point = moveHitInfo.transform.position;
							

                                bool isItemAlreadyOn = false;
                                if ((CaringPageControls.AlarmUI.activeInHierarchy
                                    || CaringPageControls.PianoUI.activeInHierarchy
                                    || CaringPageControls.JunoUI.activeInHierarchy
                                    || CaringPageControls.EDMBoxUI.activeInHierarchy
                                    || CaringPageControls.PianoUI.activeInHierarchy
                                    || CaringPageControls.LightbulbUI.activeInHierarchy)
                                    && (LastKnownObjectWithMenuUp == moveHitInfo.collider.gameObject))
                                {
                                    isItemAlreadyOn = true;
                                }

                                if (RequestedToMoveToCounter == 1 && !isItemAlreadyOn && (moveHitInfo.collider.GetComponent<UIPopupItemScript>().Menu != MenuFunctionalityUI.None) && !hadUItouch)
                                {
                                    if (moveHitInfo.collider/*.GetComponent<ReferencedObjectScript>().Reference*/.GetComponent<UIPopupItemScript>().Menu == MenuFunctionalityUI.Clock)
                                    {
                                        HidePopupMenus();
									
                                        CaringPageControls.TargetItem = moveHitInfo.collider.gameObject;
                                        //TriggeredHoldAction = true;
                                        CaringPageControls.AlarmUI.SetActive(true);
                                        LastKnownObjectWithMenuUp = moveHitInfo.collider.gameObject;
                                        preventMovingTo = true;
                                    }
                                    else if (moveHitInfo.collider/*.GetComponent<ReferencedObjectScript>().Reference*/.GetComponent<UIPopupItemScript>().Menu == MenuFunctionalityUI.EDMBox)
                                    {
                                        EDMBoxScript edmScript = moveHitInfo.collider.gameObject.GetComponent<EDMBoxScript>();
                                        HidePopupMenus();

                                        if (edmScript != null)
                                        {
                                            edmScript.SetInterface(CaringPageControls.EDMBoxUI);
                                            CaringPageControls.TargetItem = moveHitInfo.collider.gameObject;
                                            CaringPageControls.EDMBoxUI.SetActive(true);
                                        }
                                        else
                                        {
                                            Debug.Log("edmScript is null");
                                        }
									
                                        LastKnownObjectWithMenuUp = moveHitInfo.collider.gameObject;
                                        preventMovingTo = true;
                                    }
                                    else if (moveHitInfo.collider/*.GetComponent<ReferencedObjectScript>().Reference*/.GetComponent<UIPopupItemScript>().Menu == MenuFunctionalityUI.Juno)
                                    {

                                        HidePopupMenus();
                                        moveHitInfo.collider.gameObject.GetComponent<EDMBoxScript>().SetInterface(CaringPageControls.JunoUI);
                                        CaringPageControls.TargetItem = moveHitInfo.collider.gameObject;
                                        CaringPageControls.JunoUI.SetActive(true);
									
                                        LastKnownObjectWithMenuUp = moveHitInfo.collider.gameObject;
                                        preventMovingTo = true;
                                    }
                                    else if (moveHitInfo.collider/*.GetComponent<ReferencedObjectScript>().Reference*/.GetComponent<UIPopupItemScript>().Menu == MenuFunctionalityUI.Piano)
                                    {
                                        HidePopupMenus();

                                        moveHitInfo.collider.gameObject.GetComponent<EDMBoxScript>().SetInterface(CaringPageControls.PianoUI);
                                        CaringPageControls.TargetItem = moveHitInfo.collider.gameObject;
                                        CaringPageControls.PianoUI.SetActive(true);
									
                                        LastKnownObjectWithMenuUp = moveHitInfo.collider.gameObject;
                                        preventMovingTo = true;
                                    }
                                    //else if (moveHitInfo.collider.GetComponent<UIPopupItemScript>().Menu == MenuFunctionalityUI.Mp3Player)
                                    else if (false)
                                    {
                                        HidePopupMenus();
                                        CaringPageControls.TargetItem = moveHitInfo.collider.gameObject;
                                        CaringPageControls.StereoUI.SetActive(true);

                                        LastKnownObjectWithMenuUp = moveHitInfo.collider.gameObject;
                                        preventMovingTo = true;
                                    }
                                    else if (moveHitInfo.collider.GetComponent<UIPopupItemScript>().Menu == MenuFunctionalityUI.Lightbulb)
                                    {
                                        HidePopupMenus();
                                        //CaringPageControls.LightbulbUI.GetComponent<UIWidget>().SetAnchor(moveHitInfo.collider.gameObject);
                                        CaringPageControls.TargetItem = moveHitInfo.collider.gameObject;
                                        CaringPageControls.LightbulbUI.SetActive(true);
									
                                        LastKnownObjectWithMenuUp = moveHitInfo.collider.gameObject;
                                        preventMovingTo = true;
                                    }
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

                            if (!preventMovingTo)
                            {
                                if (!hadUItouch)
                                    HidePopupMenus();

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
                    else
                    {
                        if (!IsMovingTowardsLocation && !animationController.IsWakingUp && ObjectHolding == null && ProfilesManagementScript.Singleton.CurrentAnimin.Hungry <= ConsideredHungryLevels && !animationController.IsTickled)
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
                    bool validDrop = false;
				  
                    // DRAG ITEM ON TO THE CHARACTER
                    if (hadRayCollision && hitInfo.collider.name.StartsWith("MainCharacter") && !animationController.IsHoldingItem)
                    {

                        //if(GroundItems.Contains(DragableObject)) Debug.Log("BUG REPORT!!!!!!!");

                        GroundItems.Remove(DragableObject);
                        PutItemInHands(DragableObject);
                        validDrop = true;
                    }
                    else if (hadRayCollision && hitInfo.collider.name.StartsWith("Invisible Ground Plane"))
                    {
                        DragableObject.transform.parent = ActiveWorld.transform;
                        validDrop = true;
                        //GroundItems.Add(DragableObject);
                        DragableObject.layer = LayerMask.NameToLayer("Default");

                        if(OnDropItem != null)
                            OnDropItem();
                    }
                    else if(InvBoxControls.listening)
                    {
                        DragableObject = null;
                        CurrentAction = ActionId.None;
                    } 
                    else
                    {
                        Debug.Log("DROPED IN UNKNOWN LOCATION");
                        if(OnDropItem != null)
                            OnDropItem();
                    }


                if(DragableObject != null)
                {
                    DragableObject.GetComponent<BoxCollider>().enabled = true;
                    DragableObject = null;
                    CurrentAction = ActionId.None;
                }
                    break;
                }

            case ActionId.DragItemAround:
                {


                if(OnDragItem != null)
                    OnDragItem();

                    MainARHandler.Instance.CurrentItem = DragableObject;
                    MainARHandler.Instance.DraggedFromStage = true;
                    if (hadRayCollision && (hitInfo.collider.name.StartsWith("Invisible Ground Plane") || hitInfo.collider.name.StartsWith("Extended")))
			//if(hadRayCollision && hitInfo.collider.name.StartsWith("SecondGroundPlane"))
                    {
                        Debug.Log("DRAGGING");
                        DragableObject.transform.position = hitInfo.point;
                        //DragableObject.transform.parent = hit.transform;
                    }

                    if (!Input.GetButton("Fire1"))
                    {
                        if(InvBoxControls.listening)
                        {
                            DragableObject = null;
                            CurrentAction = ActionId.None;
                        } 
                        else
                        {
                            if (hitInfo.collider != null && hitInfo.collider.name.StartsWith("Extended"))
                            {
                                DragableObject.AddComponent<DroppedItemScript>();
                            }
                            CurrentAction = ActionId.DropItem;
                        }
                    }
				

                    break;
                }
        }

        if ((DateTime.Now - LastTimeToilet).TotalSeconds >= M_SHIT_TIME && !animationController.IsSleeping && animationController.IsIdle && !IsMovingTowardsLocation)
        {
            GameObject newPoo;
            if (UnityEngine.Random.Range(0, 2) == 0)
            {
                newPoo = GameObject.Instantiate(PooPrefab) as GameObject;
                UIGlobalVariablesScript.Singleton.SoundEngine.Play(GenericSoundId.TakePoo);
                UiPages.GetPage(Pages.CaringPage).GetComponent<CaringPageControls>().TutorialHandler.TriggerAdHocStartCond("Shit"); //Hey, we have naming conventions. I'm gonna stick to them.
            }
            else
            {
                newPoo = GameObject.Instantiate(PissPrefab) as GameObject;
                UIGlobalVariablesScript.Singleton.SoundEngine.Play(GenericSoundId.TakePiss);
            }

            newPoo.transform.parent = ActiveWorld.transform;
            newPoo.transform.position = this.transform.position;
            newPoo.transform.rotation = Quaternion.Euler(0, 180 + UnityEngine.Random.Range(-30.0f, 30.0f), 0);

            GroundItems.Add(newPoo);

            int sign = -1;
            if (UnityEngine.Random.Range(0, 2) == 0)
                sign = 1;
            float randomDistanceA = UnityEngine.Random.Range(30, 40);
            //if(Physics.Raycast(new Ray(UIGlobalVariablesScript.Singleton.main

            MoveTo(this.transform.position + new Vector3(UnityEngine.Random.Range(-40, 40), 0, randomDistanceA * sign), false);

            LastTimeToilet = DateTime.Now;
        }

        if ((DateTime.Now - LastGiftTime).TotalSeconds >= M_GIFT_TIME && !animationController.IsSleeping && animationController.IsIdle && !IsMovingTowardsLocation)
        {
            GetRandomItem();
            LastGiftTime = DateTime.Now;
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
		if((DateTime.Now - LastSavePerformed).TotalSeconds >= 4)
		{
//            ProfilesManagementScript.Singleton.CurrentProfile.Characters[(int)ProfilesManagementScript.Singleton.CurrentProfile.ActiveAnimin].;
            SaveAndLoad.Instance.SaveAllData();
            LastSavePerformed = DateTime.Now;
            Debug.Log("just saved...");
		}
*/
        if (Input.GetButtonDown("Fire1"))
            hadButtonDownLastFrame = true;
        else
            hadButtonDownLastFrame = false;


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
        LastTimeToilet = DateTime.Now;
        Debug.Log("exit sleep");
        animationController.IsSleeping = false;
        CurrentAction = ActionId.None;
        SleepBoundingBox.SetActive(false);
        UIGlobalVariablesScript.Singleton.SoundEngine.Play(ProfilesManagementScript.Singleton.CurrentAnimin.PlayerAniminId, ProfilesManagementScript.Singleton.CurrentAnimin.AniminEvolutionId, CreatureSoundId.SleepToIdle);
        UIGlobalVariablesScript.Singleton.SoundEngine.StopLoop();
		
//        UiPages.GetPage(Pages.CaringPage).GetComponent<CaringPageControls>().TutorialHandler.TriggerExitCond("Initial", "WakeUp");
    }




    public void PickupItem(GameObject item)
    {
        Debug.Log("void PickupItem(GameObject item)");
        UIGlobalVariablesScript.Singleton.SoundEngine.Play(GenericSoundId.ItemPickup);
        this.GetComponent<CharacterProgressScript>().GroundItems.Remove(item);
        Stop(true);
        //Physics.IgnoreCollision(item.collider, this.collider, true);
        PutItemInHands(item);
//		Debug.Log("DISABLING COLLISION");
    }



    public void PutItemInHands(GameObject item)
    {
        //item.GetComponent<BoxCollider>().enabled = false;
        //if(item.collider.gameObject.activeInHierarchy)



        item.layer = LayerMask.NameToLayer("IgnoreCollisionWithCharacter");
        //item.transform.parent = GetComponent<CharacterSwapManagementScript>().CurrentModel.GetComponent<HeadReferenceScript>().ObjectCarryAttachmentBone.transform;
        GetComponent<CharacterSwapManagementScript>().CurrentModel.GetComponent<HeadReferenceScript>().HoldingObject = item;
        animationController.IsHoldingItem = true;
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

    public bool OnInteractWithPopupItem(UIPopupItemScript item)
    {
        switch (item.Type)
        {
            case PopupItemType.Token:
                {
                    //Stop(true);
                    EvolutionManager.Instance.AddZef();

//				for(int i=0;i<(int)AniminSubevolutionStageId.Count;++i)
//				{
//					if(ProfilesManagementScript.Singleton.CurrentAnimin.Evolution >= AniminSubevolutionStageData.Stages[i])
//					{
//					if(!ProfilesManagementScript.Singleton.CurrentAnimin.SubstagesCompleted.Contains((AniminSubevolutionStageId)i))
//						{
//						ProfilesManagementScript.Singleton.CurrentAnimin.SubstagesCompleted.Add((AniminSubevolutionStageId)i);
//							AchievementsScript.Singleton.Show(AchievementTypeId.Evolution, 0);
//						}
//					}
//
//				}

                    /*if(ProfilesManagementScript.Singleton.CurrentAnimin.Evolution >= 100)
				{
				if(ProfilesManagementScript.Singleton.CurrentAnimin.AniminEvolutionId != AniminEvolutionStageId.Adult)
					{
					ProfilesManagementScript.Singleton.CurrentAnimin.Evolution = 0;
					ProfilesManagementScript.Singleton.CurrentAnimin.AniminEvolutionId = (AniminEvolutionStageId)((int)ProfilesManagementScript.Singleton.CurrentAnimin.AniminEvolutionId + 1);
						UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterSwapManagementScript>().LoadCharacter(ProfilesManagementScript.Singleton.CurrentAnimin.PlayerAniminId, ProfilesManagementScript.Singleton.CurrentAnimin.AniminEvolutionId);

					}
				}*/


                    //UIGlobalVariablesScript.Singleton.EvolutionProgressSprite.width = (int)(1330.0f * (Evolution / 100.0f));
                    Debug.Log("TOKEN COLLECTED");

                    break;
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
                    ProfilesManagementScript.Singleton.CurrentAnimin.Hungry += item.Points;

                    if (item.Id == InventoryItemId.Blueberry || item.Id == InventoryItemId.Strawberry || item.Id == InventoryItemId.watermelon)
                    {
                        AchievementManager.Instance.AddToAchievment(AchievementManager.Achievements.EatFruit);
                    }
                    //Stop(true);
						

		
                    //}
                    break;
                }

            case PopupItemType.Item:
                {
			
                    AchievementManager.Instance.AddToAchievment(AchievementManager.Achievements.PlayMusic);
                    //ShowText("I can't use this item");
                    return false;
                    break;
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


                    if (ProfilesManagementScript.Singleton.CurrentAnimin.Health / PersistentData.MaxHealth <= 0.4f)
                    {
                        UiPages.GetPage(Pages.CaringPage).GetComponent<CaringPageControls>().TutorialHandler.TriggerAdHocStartCond("HealFrom40");
                    }

                    //ShowText("I feel good");
                    ProfilesManagementScript.Singleton.CurrentAnimin.Health += item.Points;
                    Stop(true);
                    animationController.IsTakingPill = true;
                    AchievementManager.Instance.AddToAchievment(AchievementManager.Achievements.Heal);


                    if (item.SpecialId == SpecialFunctionalityId.Injection)
                        UIGlobalVariablesScript.Singleton.SoundEngine.Play(ProfilesManagementScript.Singleton.CurrentAnimin.PlayerAniminId, ProfilesManagementScript.Singleton.CurrentAnimin.AniminEvolutionId, CreatureSoundId.InjectionReact);
                    else
                        UIGlobalVariablesScript.Singleton.SoundEngine.Play(ProfilesManagementScript.Singleton.CurrentAnimin.PlayerAniminId, ProfilesManagementScript.Singleton.CurrentAnimin.AniminEvolutionId, CreatureSoundId.EatPill);

                    //}
                    break;
                }

        }

        return true;
    }

    public void GiveMedicine(ItemId id)
    {
		
    }


    public void MoveTo(Vector3 location, bool run)
    {
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
}

public enum ActionId
{
    None = 0,
    EnterSleep,
    Sleep,
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

