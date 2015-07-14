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

	static public Vector3 FindSpawnPoint(int avoidEdgesBy = 0, bool closestToCenter = false, float radius = 0)
	{
		possibleLocations.Clear ();
		float ws = WorldSize;
		if (ws > 11) 
		{
			ws = 11;
		}
		float dim = (ws-1 - (float)avoidEdgesBy*2) * size * 0.5f;
		RaycastHit hit;
		Vector3 pos;
		pos.y = 1000;
		float bestHeight = 2000;
		int mask = -1 ^ LayerMask.GetMask ("Character");
		for (float x = - dim; x <= dim; x+= size) 
		{
			pos.x = x;
			float lastY = -1000;
			float lastY2 = -2000;
			for (float z = - dim; z <= dim; z+= size) 
			{
				pos.z = z;
				bool hitSomething = false;
				if (radius == 0)
				{
					hitSomething = Physics.Raycast(pos, Vector3.down, out hit, Mathf.Infinity, mask);
				}
				else
				{
					hitSomething = Physics.SphereCast(pos, radius, Vector3.down, out hit, Mathf.Infinity, mask);
				}
				if(hitSomething)
				{
					float y = hit.point.y;
					if (((1<<hit.collider.gameObject.layer) & FloorLayerMask) == 0)
					{
						y += 3000;
					}
					
					float thisY = y;
					float zOffset = 0;
					if(y != lastY)
					{
						y += 2000;	// Preffer a location where two blocks next to each other are at the same height
					}
					else
					{
						if (lastY != lastY2)
						{
							y += 1000;							
							zOffset = -size / 2;	// Shift half a block back.
						}
						else
						{
							zOffset = -size;	// Shift whole block back.
						}
					}
					lastY2 = lastY;
					lastY = thisY;
					if (y<= bestHeight)
					{
						if (y < bestHeight)
						{							
							possibleLocations.Clear ();
							bestHeight = y;
						}
						pos.y = y;
						possibleLocations.Add (new Vector3(pos.x, pos.y, pos.z + zOffset));
						pos.y = 1000;
					}
				}
			}
		}
		// Select a random point from the best found
		if (closestToCenter) 
		{
			pos = possibleLocations[0];
			int index = 0;
			float dist2 = float.MaxValue;
			for(int i = 0; i < possibleLocations.Count; i++)
			{				
				Vector3 pos2 = possibleLocations[i];
				pos2.y = 0;
				float mag2 = pos2.sqrMagnitude;
				if(mag2 < dist2)
				{
					dist2 = mag2;
					pos = possibleLocations[i];
				}
			}
		} else {
			pos = possibleLocations [Random.Range (0, possibleLocations.Count)];
		}
		while (pos.y >= 1000)
		{
			pos.y -= 1000;
		}
		return pos;
	}
}
