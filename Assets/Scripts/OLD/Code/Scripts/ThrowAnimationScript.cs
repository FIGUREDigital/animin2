using UnityEngine;
using System.Collections;

public class ThrowAnimationScript : MonoBehaviour
{
	System.Action onLand;

	Rigidbody addedBody = null;
	static public void Throw(GameObject throwMe, Vector3 direction, float speed, System.Action onLand = null)
	{
		
		ThrowAnimationScript throwScript = throwMe.GetComponent<ThrowAnimationScript>();
		if (throwScript == null)
		{
			throwScript = throwMe.AddComponent<ThrowAnimationScript> (); 
		}
		throwScript.onLand = onLand;
		throwScript.Begin (direction, speed);
	}

    public void Begin(Vector3 direction, float speed)
    {
        Rigidbody body = this.gameObject.GetComponent<Rigidbody>();
		if (body == null) 
		{
			addedBody = this.gameObject.AddComponent<Rigidbody>();
			addedBody.freezeRotation = true;
			body = addedBody;
		}
		body.velocity = new Vector3(speed * direction.x, 400.0f, speed * direction.z);		
		// Can't collide with charcter in the first few frames
		this.gameObject.layer = LayerMask.NameToLayer("IgnoreCollisionWithCharacter");
    }

	public void OnCollisionStay(Collision col)
	{
		if (((1 << col.gameObject.layer) & Boxes.FloorLayerMask) != 0) 
		{
			if(onLand != null)
			{
				onLand();
			}
			// Hit floor			
			Destroy(this);	
			if(addedBody != null)
			{
				Destroy (addedBody);
			}
			this.gameObject.layer = LayerMask.NameToLayer("Default");
			ItemLink il = gameObject.GetComponent<ItemLink>();
			if(il != null)
			{
				il.item.UpdatePosAndRotFromTransform();
				il.item.SetupLayer();
            }
			
			SpinObjectScript spinScript = this.gameObject.GetComponent<SpinObjectScript>();
			if(spinScript != null)
			{
				spinScript.enabled = true;
			}
        }
    }
    
    public void OnCollisionEnter(Collision col)
    {
        OnCollisionStay(col);
	}

}
