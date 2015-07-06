using UnityEngine;
using System.Collections;

public class PlaceInInventory : MonoBehaviour {


	// Use this for initialization
	void OnTriggerEnter (Collider c)
	{
		ItemLink il = c.GetComponent<ItemLink> ();
		if (il != null) 
		{
			il.item.MoveTo(Inventory.Locations.Inventory, Vector3.zero);
		}
	}
}
