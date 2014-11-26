using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class SpaceshipMinigameScript : MonoBehaviour 
{
	public GameObject SpaceshipRef;
	private float SpaceshipNormalizedPosition;
	private float FireWeaponsCooldown;
	private float EnemySpawnCooldown;
	public PrefabPool BulletPool;
	public GameObject BulletPrefab;

	public PrefabPool EnemyPool;
	public GameObject EnemyPrefab;

	public PrefabPool Enemy2Pool;
	public GameObject Enemy2Prefab;

	public PrefabPool BulletEnemyPool;
	public GameObject BulletEnemyPrefab;


	public Button MoveLeftButtonRef;
	public Button MoveRightButtonRef;

	void Awake()
	{
		BulletPool = new PrefabPool(BulletPrefab);
		EnemyPool = new PrefabPool(EnemyPrefab);
		Enemy2Pool = new PrefabPool(Enemy2Prefab);
		BulletEnemyPool = new PrefabPool(BulletEnemyPrefab);

		for(int i=0;i<EnemyPool.BulletsPool.Count;++i)
		{
			EnemyPool.BulletsPool[i].GetComponent<SpaceEnemyScript>().SpaceMinigameScriptRef = this;
		}

		for(int i=0;i<Enemy2Pool.BulletsPool.Count;++i)
		{
			Enemy2Pool.BulletsPool[i].GetComponent<SpaceEnemyScript>().SpaceMinigameScriptRef = this;
		}


	}

	// Use this for initialization
	void Start () 
	{
	
	}


	void OnGUI()
	{

	}


	public Vector3 GetArcPosition(float screenPercent)
	{
		return Vector3.Lerp(new Vector3(-0.2620256f, 0, 0), new Vector3(0.311053f, 0, 0), screenPercent);
	}
	
	public void MoveLeft()
	{
		SpaceshipNormalizedPosition -= 1 * Time.deltaTime;
		if(SpaceshipNormalizedPosition <= 0) SpaceshipNormalizedPosition = 0;
	}

	public void MoveRight()
	{
		SpaceshipNormalizedPosition += 1 * Time.deltaTime;
		if(SpaceshipNormalizedPosition >= 1) SpaceshipNormalizedPosition = 1;
	}

	// Update is called once per frame
	void Update () 
	{
//		if(Input.GetKey(KeyCode.A) || (MoveLeftButtonRef.state == ButtonColor.State.Pressed)) MoveLeft();
//		if(Input.GetKey(KeyCode.D) || (MoveRightButtonRef.state == ButtonColor.State.Pressed)) MoveRight();

		SpaceshipRef.transform.localPosition = GetArcPosition(SpaceshipNormalizedPosition);


		FireWeaponsCooldown -= Time.deltaTime;

		if(FireWeaponsCooldown <= 0)
		{
			BulletPool.GetPooledItem(this.gameObject, GetArcPosition(SpaceshipNormalizedPosition));
			FireWeaponsCooldown = UnityEngine.Random.Range(0.3f, 0.35f);
		}

		for(int i=0;i<BulletPool.ActivePool.Count;++i)
		{
			BulletPool.ActivePool[i].transform.localPosition += new Vector3(0, 0, 1) * Time.deltaTime;

			if(BulletPool.ActivePool[i].transform.localPosition.z >= 5)
			{
				BulletPool.Recycle(BulletPool.ActivePool[i]);
				i--;
			}
		}


		EnemySpawnCooldown -= Time.deltaTime;
		if(EnemySpawnCooldown <= 0)
		{
			//if(UnityEngine.Random.Range(1, 10) < 8)
				EnemyPool.GetPooledItem(this.gameObject, GetArcPosition(UnityEngine.Random.Range(0.0f, 1.0f)) + new Vector3(0, 0, 2));
			//else
			//	Enemy2Pool.GetPooledItem(this.gameObject, GetArcPosition(UnityEngine.Random.Range(0.0f, 1.0f)) + new Vector3(0, 0, 2));

			EnemySpawnCooldown = UnityEngine.Random.Range(0.4f, 0.75f);
		}


		/*for(int i=0;i<BulletEnemyPool.ActivePool.Count;++i)
		{
			BulletEnemyPool.ActivePool[i].transform.localPosition += new Vector3(0, 0, -1) * Time.deltaTime * 0.4f;
			
			if(BulletEnemyPool.ActivePool[i].transform.localPosition.z <= 0)
			{
				BulletEnemyPool.Recycle(BulletEnemyPool.ActivePool[i]);
				i--;
			}
		}*/


		for(int i=0;i<EnemyPool.ActivePool.Count;++i)
		{
			EnemyPool.ActivePool[i].transform.localPosition += new Vector3(0, 0, -1) * Time.deltaTime * 0.4f;
			
			if(EnemyPool.ActivePool[i].transform.localPosition.z <= 0)
			{
				EnemyPool.Recycle(EnemyPool.ActivePool[i]);
				i--;
			}
		}

		/*for(int i=0;i<Enemy2Pool.ActivePool.Count;++i)
		{
			Enemy2Pool.ActivePool[i].transform.localPosition += new Vector3(0, 0, -1) * Time.deltaTime * 0.4f;
			
			if(Enemy2Pool.ActivePool[i].transform.localPosition.z <= 0)
			{
				Enemy2Pool.Recycle(Enemy2Pool.ActivePool[i]);
				i--;
			}
		}*/

	}

}
