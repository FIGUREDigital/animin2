using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CollectiblesBehaviour : MonoBehaviour 
{
    static public CollectiblesBehaviour instance;
	public static int crystalCount;
		
	private int crystalPrefabs;
	private GameObject crystal;

	private AudioClip crystalAppear;
	private AudioClip crystalPickUp;
    void Awake()
    {
        TutorialHandler.FireEvents += EventFired;
        TutorialHandler.ShouldSkipLesson += OnShouldSkipLesson;
    }
    void OnDestroy()
    {    
        TutorialHandler.FireEvents -= EventFired;
        TutorialHandler.ShouldSkipLesson -= OnShouldSkipLesson;
    }

    void EventFired(string fired)
    {        
        if (fired == "GiveCrystals")
        {
            if (!ProfilesManagementScript.Instance.CurrentProfile.m_GivenCrystals)
            {
                SpawnCrystal(); 
                SpawnCrystal();	
            }
        }
    }
    
    void OnShouldSkipLesson(string skipID)
    {
        if (skipID == "GivenZef")
        {
            TutorialHandler.ShouldSkip = ProfilesManagementScript.Instance.CurrentProfile.m_GivenZef;
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
		
	IEnumerator CrystalRoutine ()
	{
		while (true)
		{
			yield return new WaitForSeconds (60);
            if (TutorialHandler.CheckTutsCompleted("Crystals"))
            { 
				int spawnCrystals = 1;

				if (PersistentData.happy > 60 )
				{
					spawnCrystals++;
				}

				if (PersistentData.happy > 80)
				{
					Vector3 position = new Vector3(
						Random.Range(-0.9f, 0.9f),
						0.0f,
						Random.Range(-0.9f, 0.9f));
					CharacterProgressScript progressScript = UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>();
					progressScript.SpawnStageItem(InventoryItemId.Zef, position);
				}

				while(spawnCrystals > 0 && crystalPrefabs < 3)
				{					
					SpawnCrystal ();	
					spawnCrystals--;					
					AlphaEmoticonBehaviour.canFade = true;
				}
			}
		}
	}

	// Use this for initialization
	void Start () 
	{
		StartCoroutine("CrystalRoutine");
	}
	
	// Update is called once per frame
	void Update () 
	{

//		Debug.Log (crystalCount);
		bool test = false;
		Ray ray= new Ray();
		if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Began)
		{
			test = true;
			ray = Camera.main.ScreenPointToRay (Input.GetTouch(0).position);
		}
		else if (Input.GetMouseButtonDown(0))
		{
			test = true;
			ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		}
		if(test)
		{
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit))
			{
				if (hit.collider.tag == "Crystal")
				{
					Destroy (hit.collider.gameObject);
					// Crystal Pick Up
					string pickUpPath = "Sounds/Main_Stage_Non_Character_Specific_Sounds/Zef_Token_Collect";
					crystalPickUp = Resources.Load (pickUpPath) as AudioClip;
					GetComponent<AudioSource>().clip = crystalPickUp;
					GetComponent<AudioSource>().Play();
					ProfilesManagementScript.Instance.CurrentAnimin.CrystalCount += 1;
					crystalPrefabs -= 1;
                    TutorialHandler.TriggerAdHocStatic("PickupCrystal");
				}
			}			      
		}
	}

	void SpawnCrystal ()
	{
        if (crystalPrefabs >= 3) return;
		crystalPrefabs ++;
		string prefabPath = "Prefabs/Crystal/crystal";
		GameObject crystal = (GameObject)Instantiate(Resources.Load (prefabPath));
		crystal.transform.parent = GameObject.Find ("MainCharacter").transform;
		string appearPath = "Sounds/Main_Stage_Non_Character_Specific_Sounds/Zef_Token_Appear";
		crystalAppear = Resources.Load (appearPath) as AudioClip;
		GetComponent<AudioSource>().clip = crystalAppear;
		GetComponent<AudioSource>().Play();
		crystal.transform.localPosition = RandomPosition();
		crystal.transform.localScale = new Vector3 (9.0f, 9.0f, 9.0f);
		crystal.transform.parent = ActiveWorld.transform;
	}
		
	Vector3 RandomPosition()
	{
		float x, y, z;
		x = UnityEngine.Random.Range (3, 9);
		y = 1.5f;
		z = UnityEngine.Random.Range (-7, 14);
		return new Vector3 (x, y, z);
	}	
}
