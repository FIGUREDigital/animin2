using UnityEngine;
using System.Collections;

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

	public static int FloorLayer
	{
		get
		{
			return floorLayer;
		}
	}

	static int floorLayer;

	static Boxes()
	{
		floorLayer = LayerMask.NameToLayer("Floor");
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
		return r;
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
			position.y = Mathf.Floor (position.y / size) * size;
			if (x <= round)
			{
				position.z = Round(position.z);
			}
		}
		return position;
	}
}
