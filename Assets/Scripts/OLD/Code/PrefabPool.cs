using UnityEngine;
using System;
using System.Collections.Generic;

public class PrefabPool
{
	public List<GameObject> BulletsPool = new List<GameObject>();
	public List<GameObject> ActivePool = new List<GameObject>();

	public PrefabPool (GameObject prefab)
	{
		for(int i=0;i<60;++i)
		{
			GameObject newBullet = (GameObject)GameObject.Instantiate(prefab);
			BulletsPool.Add(newBullet);
			
			newBullet.transform.localScale *= 10;
			newBullet.transform.localPosition = Vector3.zero;
			//newBullet.transform.localRotation = Quaternion.identity;
			
			newBullet.SetActive(false);
		}
	}

	public void GetPooledItem(GameObject parent, Vector3 position)
	{
		if(BulletsPool.Count > 0)
		{
			GameObject empty = BulletsPool[BulletsPool.Count - 1];
			BulletsPool.RemoveAt(BulletsPool.Count - 1);
			ActivePool.Add(empty);
			empty.transform.parent =  parent.transform;

			empty.SetActive(true);
			empty.transform.localPosition = position;
		}
	}

	public void Recycle(GameObject gameObject)
	{
		ActivePool.Remove(gameObject);
		BulletsPool.Add(gameObject);
		gameObject.SetActive(false);
	}
}


