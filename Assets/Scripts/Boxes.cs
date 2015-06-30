using UnityEngine;
using System.Collections;

public class Boxes : MonoBehaviour {

	public static float size = 40.0f;

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
		v = v / size;
		float sign = size;
		if (v < 0)
		{
			sign = -sign;
			v = -v;
		}
		if (v >= 4.5f) 
		{
			if (v > 5)
			{
				return v * sign;
			}
			v -= 0.51f;
		}
		return Mathf.Round (v) * sign;
	}

	public static Vector3 Snap(Vector3 position)
	{
		float sign = 1;
		float x = position.x;

		if (Mathf.Abs (position.x) <= 5f * size && Mathf.Abs (position.z) <= 5f * size)
		{
			position.x = Round(position.x);
			position.y = Mathf.Floor (position.y / size) * size;
			position.z = Round(position.z);
		}
		return position;
	}
}
