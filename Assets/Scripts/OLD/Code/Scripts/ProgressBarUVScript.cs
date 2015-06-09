using UnityEngine;
using System.Collections;

public class ProgressBarUVScript : MonoBehaviour 
{
	public GameObject ZefToken;

	float HappyAverage;
	const float SamplesPerTrigger = 60.0f * 15;
	private float Accumulator;
	private float Timer;
	private int SamplesTaken;


	/*
	 * AH Disable this spawning relly on one in CollectiblesBehaviour
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Debug.Log("UPDATING!!!");
		CharacterProgressScript script = UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>();

		Timer -= Time.deltaTime;
		if(Timer <= 0)
		{
			Timer = 1;
			Accumulator += ProfilesManagementScript.Singleton.CurrentAnimin.Happy;
			SamplesTaken++;

			if(SamplesTaken >= SamplesPerTrigger)
			{
				int TokensPerHappyAverage = (int)((Accumulator / (PersistentData.MaxHappy * SamplesTaken)) * 3);

				for(int i=0;i<TokensPerHappyAverage;++i)
				{
					Vector3 position = new Vector3(
						Random.Range(-0.9f, 0.9f),
						0.0f,
						Random.Range(-0.9f, 0.9f));

					script.SpawnZef(position);
				}


				SamplesTaken = 0;
				Accumulator = 0;
			}
		}
	}
	*/
}
