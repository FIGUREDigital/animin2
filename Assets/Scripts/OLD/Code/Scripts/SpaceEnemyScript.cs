using UnityEngine;
using System.Collections;

public class SpaceEnemyScript : MonoBehaviour 
{

	public bool CanShootMissile;
	private float WeaponsCooldown;
	public SpaceshipMinigameScript SpaceMinigameScriptRef;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{

		if(CanShootMissile)
		{
			WeaponsCooldown -= Time.deltaTime;
			if(WeaponsCooldown <= 0)
			{
				SpaceMinigameScriptRef.BulletEnemyPool.GetPooledItem(
					SpaceMinigameScriptRef.gameObject, 
					this.gameObject.transform.localPosition);
			}
		}
	}


	void OnTriggerEnter (Collider col)
	{
		if(col.gameObject.name.StartsWith("Bullet"))
		{
			//Debug.Log("COLLISION");
			col.gameObject.transform.localPosition = new Vector3(0, 0, 100);
			this.transform.localPosition = new Vector3(0, 0, -100);
		}
	}
}
