using UnityEngine;
using System.Collections.Generic;

/*
public enum PresentationLayerId
{
	Default = 0,
	Count,
}

public static class PresentationEventManager
{
	public static PresentationEventScript ScriptRef;
	
	static PresentationEventManager()
	{
		GameObject scriptOwner = new GameObject("Presentation Event Manager - Managed by Code");
		ScriptRef = scriptOwner.AddComponent<PresentationEventScript>();
		GameObject.DontDestroyOnLoad(scriptOwner);
		
		for(int i=0;i<(int)PresentationLayerId.Count;++i)
		{
			ScriptRef.Layers[i] = new List<PresentationEvent>();
			ScriptRef.RemovalHelpers[i] = new List<PresentationEvent>();
		}
	}
	
	public static void Create(PresentationEvent eventData)
	{
		ScriptRef.Layers[(int)eventData.LayerId].Add(eventData);
	}
	
	public static void Remove(PresentationEvent eventData)
	{
		ScriptRef.RemovalHelpers[(int)eventData.LayerId].Add(eventData);
	}
}

public class PresentationEventScript : MonoBehaviour
{
	public List<PresentationEvent>[] Layers = new List<PresentationEvent>[(int)PresentationLayerId.Count];
	public List<PresentationEvent>[] RemovalHelpers = new List<PresentationEvent>[(int)PresentationLayerId.Count];
	
	void Update()
	{
		for(int i=0;i<Layers.Length;++i)
		{
			for(int j=0;j<Layers[i].Count;++j)
			{
				Layers[i][j].Update();
			}
		}
		
		for(int i=0;i<RemovalHelpers.Length;++i)
		{
			for(int j=0;j<RemovalHelpers[i].Count;++j)
			{
				Layers[i].Remove(RemovalHelpers[i][j]);
			}
			
			RemovalHelpers[i].Clear();
		}
	}
	
}

public abstract class PresentationEvent
{
	public PresentationLayerId LayerId;
	
	public abstract void Update();
}
*/
public class TemporaryDisableCollisionEvent : MonoBehaviour
{
	private GameObject GameObjectRef;
	private float Timer;
	
	void  Start()
	{
		//LayerId = PresentationLayerId.Default;
		GameObjectRef = gameObject;
		Timer = 2.0f;
		BoxCollider colliderScript = GameObjectRef.GetComponent<BoxCollider>();
		colliderScript.enabled = false;

	}
	
	 void Update()
	{
		Timer -= Time.deltaTime;
		if(Timer <= 0)
		{
			BoxCollider colliderScript = GameObjectRef.GetComponent<BoxCollider>();
			if(colliderScript != null)
			{
				colliderScript.enabled = true;
			}
			else
			{
				Debug.Log("No Box Collider found!");
			}

			Destroy(this);
			//PresentationEventManager.Remove(this);
		}
	}
}