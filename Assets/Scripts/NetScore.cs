using UnityEngine;
using System.Collections;

public class NetScore : MonoBehaviour 
{

	public UIText textScore;
	float score = 0;
	// Use this for initialization
	void OnTriggerExit(Collider coll)
	{
		if (coll.GetComponent<ThrowAnimationScript>() == null) return;	// Item not thrown
		ItemLink il = coll.GetComponent<ItemLink> ();
		if (il != null) 
		{
			if (il.item.Id == InventoryItemId.BasketBall)
			{
				score++;
				textScore.Text = score.ToString ();
			}
		}
	}
}
