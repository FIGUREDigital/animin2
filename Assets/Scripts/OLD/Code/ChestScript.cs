using UnityEngine;
using System.Collections.Generic;

public class ChestScript : MonoBehaviour
{
    private enum AnimationStateId
    {
        None = 0,
        StartingUp,
        OpeningLid,
        LidOpened,
        ThrowItemsOut,
        FadeOut,
        Completed,
    }

    private AnimationStateId State;
    private float Timer = 6.0f;

    public GameObject[] Coins;

    private enum ChestType {Bronze, Silver, Gold, Evo};
    [SerializeField]
    ChestType m_ChestType;

    private enum ItemType
    {
        Zef,

        WeakFood,
        MediumFood,
        StrongFood,

        WeakMed,
        MediumMed,
        StrongMed,

		NormalBox
    }

    private ItemType[][] BronzeRewards = new ItemType[4][]
    {
        new ItemType[]{ ItemType.Zef, ItemType.WeakFood, ItemType.NormalBox },
		new ItemType[]{ ItemType.WeakFood, ItemType.NormalBox },
		new ItemType[]{ ItemType.WeakFood, ItemType.WeakFood, ItemType.NormalBox },
		new ItemType[]{ ItemType.Zef, ItemType.WeakMed, ItemType.NormalBox }
    };
    private ItemType[][] SilverRewards = new ItemType[4][]
    {
		new ItemType[]{ ItemType.Zef, ItemType.Zef, ItemType.Zef, ItemType.MediumMed, ItemType.NormalBox },
		new ItemType[]{ ItemType.Zef, ItemType.Zef, ItemType.MediumFood, ItemType.NormalBox },
		new ItemType[]{ ItemType.Zef, ItemType.WeakFood, ItemType.MediumFood, ItemType.NormalBox },
		new ItemType[]{ ItemType.Zef, ItemType.MediumFood, ItemType.MediumFood, ItemType.NormalBox  }
    };
    private ItemType[][] GoldRewards = new ItemType[4][]
    {
		new ItemType[]{ ItemType.StrongFood, ItemType.MediumFood, ItemType.StrongMed, ItemType.Zef, ItemType.Zef, ItemType.Zef, ItemType.Zef, ItemType.NormalBox},
		new ItemType[]{ ItemType.Zef, ItemType.Zef, ItemType.Zef, ItemType.Zef, ItemType.Zef, ItemType.StrongFood, ItemType.NormalBox},
		new ItemType[]{ ItemType.Zef, ItemType.Zef, ItemType.Zef, ItemType.MediumFood, ItemType.StrongFood, ItemType.StrongFood, ItemType.NormalBox },
		new ItemType[]{ ItemType.Zef, ItemType.Zef, ItemType.Zef, ItemType.Zef, ItemType.Zef, ItemType.Zef, ItemType.MediumFood, ItemType.MediumFood, ItemType.NormalBox }
    };

    InventoryItemId[] StrongFoods = new InventoryItemId[]{
        InventoryItemId.CakeVanilla,
        InventoryItemId.Noodles,
        InventoryItemId.Chocolate,
        InventoryItemId.Spinach,
        InventoryItemId.Kiwi,
        InventoryItemId.Blueberry
    };
    InventoryItemId[] MediumFoods = new InventoryItemId[]{
        InventoryItemId.Chips, InventoryItemId.Pizza,
        InventoryItemId.ChocoCake,
        InventoryItemId.Avocado,
        InventoryItemId.AlmondMilk,
        InventoryItemId.Toast,
        InventoryItemId.Banana,
        InventoryItemId.Cereal
    };
    InventoryItemId[] WeakFoods = new InventoryItemId[]{
        InventoryItemId.Strawberry,
        InventoryItemId.Beetroot,
        InventoryItemId.Peanut,
        InventoryItemId.watermelon,
        InventoryItemId.Carrot
    };

	
	InventoryItemId[] NormalBoxes = new InventoryItemId[]{
		InventoryItemId.Box1,
		InventoryItemId.Box2
	};

    InventoryItemId[] StrongMeds = new InventoryItemId[]{ InventoryItemId.Pill };
    InventoryItemId[] MediumMeds = new InventoryItemId[]{ InventoryItemId.Pill };
    InventoryItemId[] WeakMeds = new InventoryItemId[]{ InventoryItemId.Plaster };

    // Update is called once per frame
    void Update()
    {

        if (State == AnimationStateId.None)
        {
            if (Input.GetButtonUp("Fire1"))
            {
                RaycastHit hitInfo;
//                bool hadRayCollision = false;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hitInfo))
                {
                    if (hitInfo.collider == this.GetComponent<Collider>())
                    {
                        State = AnimationStateId.StartingUp;
                    }
                }
            }
        }

        //this.GetComponent<Animator>().SetBool("Open", false);

        switch (State)
        {
            case AnimationStateId.None:
                {
				

                    break;

                }

            case AnimationStateId.StartingUp:
                {

                    //Timer -= Time.deltaTime;
                    //if(Timer <= 0)
                    {
                        this.GetComponent<Animator>().SetBool("Open", true);
                        State = AnimationStateId.OpeningLid;
                        Timer = 1.2f;

                    }

                    break;
			
                }

            case AnimationStateId.OpeningLid:
                {
                    Timer -= Time.deltaTime;
                    if (Timer <= 0)
                    {
                        CharacterProgressScript progressScript = UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>();
					

                        ItemType[][] items = BronzeRewards;
                        switch (m_ChestType)
                        {
                            case (ChestType.Bronze):
                                items = BronzeRewards;
                                break;
                            case (ChestType.Silver):
                                items = SilverRewards;
                                break;
                            case (ChestType.Gold):
                                items = GoldRewards;
                                break;
                        }
                        int cur = Random.Range(0, items.Length);

                        int length = (m_ChestType == ChestType.Evo ? 1 : items[cur].Length);

                        for (int i = 0; i < length; ++i)
                        {
                            GameObject zef = null;
							ItemType t = items[cur][i];

                            if (m_ChestType == ChestType.Evo)
                            {
								zef = progressScript.SpawnStageItem(GetComponent<EvolutionChestItem>().id, Vector3.zero, true);
                            }
                            else if (t != ItemType.Zef)
                            {
                                InventoryItemId[] types = new InventoryItemId[]{ };
                                switch (t)
                                {
                                    case (ItemType.StrongFood):
                                        {
                                            types = StrongFoods;
                                        }
                                        break;
                                    case (ItemType.MediumFood):
                                        {
                                            types = MediumFoods;
                                        }
                                        break;
                                    case (ItemType.WeakFood):
                                        {
                                            types = WeakFoods;
                                        }
                                        break;

                                    case (ItemType.StrongMed):
                                        {
                                            types = StrongMeds;
                                        }
                                        break;
                                    case (ItemType.MediumMed):
                                        {
                                            types = MediumMeds;
                                        }
                                        break;
                                    case (ItemType.WeakMed):
                                        {
                                            types = WeakMeds;
                                        }
										break;
									case (ItemType.NormalBox):
										{
											types = NormalBoxes;
										}
										break;
                                }
						
								//zef = progressScript.SpawnStageItem(InventoryItemId.Box1, Vector3.zero);
								zef = progressScript.SpawnStageItem(types[Random.Range(0, types.Length)], Vector3.zero);
                            }
                            else
                            {
								zef = progressScript.SpawnStageItem(InventoryItemId.Zef, Vector3.zero);
                            }

                            SpinObjectScript spinScript = zef.GetComponent<SpinObjectScript>();
                            if (spinScript != null)
                            {
                                spinScript.enabled = false;
                            }
                            zef.transform.parent = Coins[i].transform.GetChild(0).transform;
                            zef.transform.localPosition = Vector3.zero;	
                            //zef.transform.localScale *= 0.8f;

                            Coins[i].transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
                            Coins[i].GetComponent<Animator>().SetBool("play", true);
                            Coins[i] = zef;
                            Coins[i].tag = "Untagged";

                            //Coins[i].transform.localScale = new Vector3(12, 12, 12);
						
                        }

                        Timer = 4;
                        State = AnimationStateId.LidOpened;

                        UiPages.GetPage(Pages.CaringPage).GetComponent<CaringPageControls>().TutorialHandler.TriggerAdHoc("Prize");
                    }

				

                    break;
                }
            case AnimationStateId.LidOpened:
                {

//			for(int i=0;i<Coins.Length;++i)
//			{
//				if(Coins[i] == null) continue;
//
//				//if(Coins[i].GetComponent<Animator>().
//			}

                    //Timer -= Time.deltaTime;
                    //if(Timer <= 0)
                    {
                        int counter = 0;
                        for (int i = 0; i < Coins.Length; ++i)
                        {
                            if (Coins[i] == null || Coins[i].transform.parent.transform.parent.GetComponent<Animator>() == null)
                            {
                                counter++;
                                continue;
                            }

                            AnimatorStateInfo stateInfo = Coins[i].transform.parent.transform.parent.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);

                            if (stateInfo.length <= 1)
                                continue;

                            Debug.Log(stateInfo.normalizedTime.ToString());
                            //Debug.Log(stateInfo.length.ToString());

                            if (Coins[i].transform.parent.transform.parent.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
                            {
                                SpinObjectScript spinScript = Coins[i].GetComponent<SpinObjectScript>();
                                if (spinScript != null)
                                {
                                    spinScript.enabled = true;
                                    Coins[i].GetComponent<SpinObjectScript>().SetRotationAngle();
                                }

							
                                Coins[i].transform.parent = UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>().ActiveWorld.transform;	
								ItemLink il = Coins[i].GetComponent<ItemLink>();
								if (il != null)
								{
									il.item.MoveTo(Inventory.CurrentLocation, Coins[i].transform.position, true);
								}							
                                Coins[i].tag = "Items";
                                Coins[i] = null;
                            }
                        }
					
                        if (counter == Coins.Length)
                        {
                            State = AnimationStateId.ThrowItemsOut;
                            Timer = 3;
				
                        }
                    }
	
                    break;
                }

            case AnimationStateId.ThrowItemsOut:
                {

                    Timer -= Time.deltaTime;

                    if (Timer <= 0)
                    {
                        BeginFadeOut = true;
                        State = AnimationStateId.Completed;
                    }

                    break;
                }
        }
	

        UpdateFading();
    }

    private void RecurviseSetAlpha(GameObject gameObject, Color alphaColor)
    {
        if (gameObject.GetComponent<Renderer>() != null)
        {
            gameObject.GetComponent<Renderer>().material.shader = Shader.Find("Transparent/Diffuse"); 
            gameObject.GetComponent<Renderer>().material.color = alphaColor;
        }

        for (int i = 0; i < gameObject.transform.childCount; ++i)
        {
            RecurviseSetAlpha(gameObject.transform.GetChild(i).gameObject, alphaColor);
        }
    }

    private float Alpha = 1;
    private bool BeginFadeOut;

    private void UpdateFading()
    {
        if (BeginFadeOut)
        {
            bool destroy = false;


            Alpha -= Time.deltaTime * 3;
            if (Alpha <= 0)
            {
                Alpha = 0;
                destroy = true;
            }

            RecurviseSetAlpha(this.gameObject, new Color(
                    1,
                    1,
                    1,
                    Alpha));


            if (destroy)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
