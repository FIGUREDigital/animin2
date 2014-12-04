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
	private List<string> FoodItems = new List<string>();
	private List<string> MedicalItems = new List<string>();


	// Use this for initialization
	void Start () 
	{
		//State = AnimationStateId.StartingUp;
		FoodItems.Add("Prefabs/almondMilk");
		FoodItems.Add("Prefabs/avocado");
		FoodItems.Add("Prefabs/blueberry");
		FoodItems.Add("Prefabs/carrot");
		FoodItems.Add("Prefabs/chips");
		FoodItems.Add("Prefabs/spinach");
		FoodItems.Add("Prefabs/strawberry2");
		FoodItems.Add("Prefabs/toast");
		FoodItems.Add("Prefabs/watermelon");

		MedicalItems.Add("Prefabs/plaster");
		MedicalItems.Add("Prefabs/capsule");
	}

//	void LateUpdate()
//	{
//
//	}
	
	// Update is called once per frame
	void Update () 
	{

		if(State == AnimationStateId.None)
		{
			if(Input.GetButtonUp("Fire1"))
			{
				RaycastHit hitInfo;
				bool hadRayCollision = false;
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				if (Physics.Raycast(ray, out hitInfo))
				{
					if(hitInfo.collider == this.collider)
					{
						State = AnimationStateId.StartingUp;
					}
				}
			}
		}

		//this.GetComponent<Animator>().SetBool("Open", false);

		switch(State)
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
				if(Timer <= 0)
				{
					CharacterProgressScript progressScript = UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>();
					
					for(int i=0;i<Coins.Length;++i)
					{
					GameObject zef = null;

					if(i < Coins.Length * 0.33f)
					{
						zef = progressScript.SpawnStageItem(FoodItems[Random.Range(0, FoodItems.Count)], Vector3.zero);
					}
					else if(i < Coins.Length * 0.5f)
					{
						zef = progressScript.SpawnStageItem(MedicalItems[Random.Range(0, MedicalItems.Count)], Vector3.zero);
					}
					else
					{
						zef = UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>().SpawnZef(Vector3.zero);
					}

					SpinObjectScript spinScript = zef.GetComponent<SpinObjectScript>();
					if(spinScript != null)
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

                        TutorialHandler.Instance.TriggerAdHocStartCond("Prize");
				}

				

				break;
			}
			case AnimationStateId.ThrowItemsOut:
			{

				Timer -= Time.deltaTime;

				if(Timer <= 0)
				{
					BeginFadeOut = true;
					State = AnimationStateId.Completed;
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
					for(int i=0;i<Coins.Length;++i)
					{
						if(Coins[i] == null) 
					{
						counter++;
						continue;
					}

					AnimatorStateInfo stateInfo = Coins[i].transform.parent.transform.parent.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);

					if(stateInfo.length == 0) continue;

					//Debug.Log(stateInfo.normalizedTime.ToString());
					//Debug.Log(stateInfo.length.ToString());

						if(Coins[i].transform.parent.transform.parent.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
						{
							SpinObjectScript spinScript = Coins[i].GetComponent<SpinObjectScript>();
							if(spinScript != null)
							{
								spinScript.enabled = true;
								Coins[i].GetComponent<SpinObjectScript>().SetRotationAngle();
							}

							
							Coins[i].transform.parent = UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>().ActiveWorld.transform;	
							
							Coins[i].tag = "Items";
							Coins[i] = null;
						}
					}
					
				if(counter == Coins.Length)
				{
					State = AnimationStateId.ThrowItemsOut;
					Timer = 3;
				
				}
				}
	
				break;
			}
		}
	

		UpdateFading();
	}

	private void RecurviseSetAlpha(GameObject gameObject, Color alphaColor)
	{
		if(gameObject.renderer != null)
		{
			gameObject.renderer.material.shader = Shader.Find("Transparent/Diffuse"); 
			gameObject.renderer.material.color = alphaColor;
		}

		for(int i=0;i<gameObject.transform.childCount;++i)
		{
			RecurviseSetAlpha(gameObject.transform.GetChild(i).gameObject, alphaColor);
		}
	}

	private float Alpha = 1;
	private bool BeginFadeOut;
	private void UpdateFading()
	{
		if(BeginFadeOut)
		{
			bool destroy = false;


			Alpha -= Time.deltaTime *  3;
			if(Alpha <= 0) 
				{
				Alpha = 0;
					destroy = true;
				}

				RecurviseSetAlpha(this.gameObject, new Color(
					1,
					1,
					1,
				Alpha));


			if(destroy)
			{
				Destroy(this.gameObject);
			}
		}
	}
}
