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

	
	public static Vector3 Snap(Vector3 position)
	{
		position.x = Mathf.Round (position.x / size) * size;
		position.y = Mathf.Floor (position.y / size) * size;
		position.z = Mathf.Round (position.z / size) * size;
		return position;
	}
}
