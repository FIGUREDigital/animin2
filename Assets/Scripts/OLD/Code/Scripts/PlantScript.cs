using UnityEngine;
using System;
using System.Collections;

public class PlantScript : MonoBehaviour 
{
	public PlantStateId State;
	public Material EmptyMaterial;
	public Material HarvestMaterial;
	public Material GrowingMaterial;
	public DateTime TimeStarted;


	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if((DateTime.UtcNow - TimeStarted).TotalSeconds >= 10)
		{
			if(State == PlantStateId.Planted)
			{
				State = PlantStateId.ReadyForHarvest;
				this.GetComponent<Renderer>().material = HarvestMaterial;
			}
		}

		if (Input.GetButtonDown("Fire1")) 
		{
			RaycastHit hitInfo;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hitInfo))
			{
				if(hitInfo.collider == this.GetComponent<Collider>())
				{
					Interact();
				}
			}
		}

	}

	public void Interact()
	{
		TimeStarted = DateTime.UtcNow;

		if(State == PlantStateId.Empty)
		{
			State = PlantStateId.Planted;
			this.GetComponent<Renderer>().material = GrowingMaterial;
		}
		else if(State == PlantStateId.ReadyForHarvest)
		{
			State = PlantStateId.Empty;
			this.GetComponent<Renderer>().material = HarvestMaterial;
		}
	}
}


public enum PlantStateId
{
	Empty = 0,
	Planted,
	ReadyForHarvest,
}