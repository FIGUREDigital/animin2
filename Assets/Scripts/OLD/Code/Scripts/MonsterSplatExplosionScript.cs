using UnityEngine;
using System.Collections;

public class MonsterSplatExplosionScript : MonoBehaviour 
{
	float myGravity = 400;
	public string SplatPrefab;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(this.transform.localPosition.y <= 0.0f)
		{
			if(SplatPrefab != null)
			{
				GameObject resourceLoad = Resources.Load(SplatPrefab) as GameObject;

				GameObject instance = (GameObject)Instantiate(resourceLoad);
				instance.transform.parent = this.transform.parent;
				instance.transform.localPosition = new Vector3(this.transform.localPosition.x, 0, this.transform.localPosition.z);
				//instance.transform.position = new Vector3(this.transform.position.x, 0, this.transform.position.z);
				instance.transform.rotation = Quaternion.Euler(
					instance.transform.rotation.eulerAngles.x, 
					instance.transform.rotation.eulerAngles.y,
					Random.Range(0, 360));
				
				//UIGlobalVariablesScript.Singleton.GunGameScene.GetComponent<GunsMinigameScript>().SpawnedObjects.Add(instance);

			}


			//UIGlobalVariablesScript.Singleton.GunGameScene.GetComponent<GunsMinigameScript>().SpawnedObjects.Remove(this.gameObject);
			Destroy(this.gameObject);
		}
	}

	void FixedUpdate()
	{
		var curVel = GetComponent<Rigidbody>().velocity;
		curVel.y -= myGravity * Time.deltaTime; // apply fake gravity
		GetComponent<Rigidbody>().velocity = curVel;
	}
}
