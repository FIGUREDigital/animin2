using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Boxes : MonoBehaviour {

	public static float size = 40.0f;

	static private float WorldSize
	{
		get
		{
			if (Inventory.CurrentLocation == Inventory.Locations.AR)
			{
				return 100000.0f;
			}
			else
			{
				return 9.0f;
			}
		}
	}

	public static int FloorLayerMask
	{
		get
		{
			return floorLayerMask;
		}
	}

	static int floorLayerMask;

	static Boxes()
	{
		floorLayerMask = LayerMask.GetMask("Floor", "ExtendedFloor");
	}


	public static Vector3 GetGroundPoint(RaycastHit hit)
	{
		Debug.DrawRay (hit.point, hit.normal, Color.cyan);
		Vector3 n = hit.normal;
		if (n.y > Mathf.Max (Mathf.Abs (n.x), Mathf.Abs (n.z)))
		{
			// Vertical so can use position.
//			Debug.Log ("Normal = " + hit.normal+" just return "+hit.point);
			return hit.point;
		}
		n.y = 0;
		Vector3 r = hit.point + (n * size * 0.5f);
//		Debug.Log ("Normal = " + hit.normal+" return "+r);
		return Snap(r);
	}

	public static float Round(float v)
	{
		float round = WorldSize * 0.5f;
		v = v / size;
		float sign = size;
		if (v < 0)
		{
			sign = -sign;
			v = -v;
		}
		if (v >= (round)) 
		{
			if (v < round + 0.5f)
			{
				v = round + 0.5f;
			}
			return v * sign;
			//}
			//v -= 0.51f;
		}
		return Mathf.Round (v) * sign;
	}

	public static Vector3 Snap(Vector3 position)
	{		
		float round = WorldSize * 0.5f * size;
		float roundMax = round + size * 0.5f;
		float sign = 1;
		float x = Mathf.Abs (position.x);
		float z = Mathf.Abs (position.z);
		if (x <= roundMax && z <= roundMax)
		{
			if (z <= round)
			{
				position.x = Round(position.x);
			}
			position.y = Mathf.Floor ((0.01f + position.y) / size) * size;
			if (x <= round)
			{
				position.z = Round(position.z);
			}
		}
		return position;
	}
	
	static List<Vector3> possibleLocations = new List<Vector3> ();

	static public Vector3 FindSpawnPoint(int avoidEdgesBy = 0)
	{
		possibleLocations.Clear ();
		float dim = (WorldSize-1 - (float)avoidEdgesBy*2) * size * 0.5f;
		RaycastHit hit;
		Vector3 pos;
		pos.y = 1000;
		float bestHeight = 2000;
		for (float x = - dim; x <= dim; x+= size) 
		{
			pos.x = x;
			for (float z = - dim; z <= dim; z+= size) 
			{
				pos.z = z;
				if(Physics.Raycast(pos, Vector3.down, out hit, Mathf.Infinity))
				{
					float y = hit.point.y;
					if (((1<<hit.collider.gameObject.layer) & FloorLayerMask) == 0)
					{
						y += 1000;
					}
					if (y<= bestHeight)
					{
						if (y < bestHeight)
						{							
							possibleLocations.Clear ();
							bestHeight = y;
						}
						pos.y = y;
						possibleLocations.Add (new Vector3(pos.x, pos.y, pos.z));
						pos.y = 1000;
					}
				}
			}
		}
		// Select a random point from the best found
		pos = possibleLocations[Random.Range(0, possibleLocations.Count)];
		if (pos.y > 1000)
		{
			pos.y -= 1000;
		}
		return pos;
	}
}
