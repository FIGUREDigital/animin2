using UnityEngine;
using System.Collections;

public class CollideSound : MonoBehaviour {

	public void OnCollisionEnter(Collision col)
	{
		if (((1 << col.gameObject.layer) & Boxes.FloorLayerMask) != 0) 
		{
			Debug.Log ("Collide "+name+" with "+col.gameObject.name+" "+col.relativeVelocity.sqrMagnitude+" "+transform.position);
			if (col.relativeVelocity.sqrMagnitude > 5000)
			{
				UIGlobalVariablesScript.Singleton.SoundEngine.Play(GenericSoundId.ItemLand);
			}
		}
	}
}
