using UnityEngine;
using System.Collections;

public class ThrowAnimationScript : MonoBehaviour 
{
	private enum StateEnum
	{
		None = 0,
		Begin,
		Throw,
		End,
	}

	StateEnum State;
	Vector3 Direction;

	float VerticalSpeed;
	private float ThrowSpeed = 100;

	public void Begin(Vector3 direction, float speed)
	{
		Direction = direction;
		State = StateEnum.Begin;
		ThrowSpeed = speed;

		//Rigidbody body = this.gameObject.AddComponent<Rigidbody>();
		//body.AddForce(direction * 100);

		//body.velocity = new Vector3(1 * direction.x, -1000, 1 * direction.z);
		//Physics.gravity = new Vector3(0, -10.0F, 0);
		VerticalSpeed = -260;

	}

	// Update is called once per frame
	void Update () 
	{
		VerticalSpeed = VerticalSpeed + (Time.deltaTime * 600);

		switch(State)
		{
			case StateEnum.Begin:
			{
				this.transform.position += Direction * Time.deltaTime * ThrowSpeed;
				this.transform.position -= new Vector3(0, VerticalSpeed * Time.deltaTime, 0);

				bool isNonArScene = false;
				if(UIGlobalVariablesScript.Singleton.NonSceneRef.activeInHierarchy)
				{
					isNonArScene = true;
				}
				
				Vector3 expetectedPosition =  new Vector3(this.transform.position.x, UIGlobalVariablesScript.Singleton.MainCharacterRef.transform.position.y, this.transform.position.z);
				bool hasLanded = false;

				if(this.transform.position.y <= UIGlobalVariablesScript.Singleton.MainCharacterRef.transform.position.y)
				{
					if(isNonArScene && (this.transform.position.x <= -150 || this.transform.position.x>= 150 || this.transform.position.z <= -150 || this.transform.position.z >= 150))
					{
						if(this.transform.position.y <= -350)
						{
							//hasLanded = true;
						this.transform.position =  new Vector3(Random.Range(-140, 140), 100, Random.Range(-140, 140));
						}
					}
					else
					{
						hasLanded = true;
					}
				}

			if(hasLanded)
			{
				UIGlobalVariablesScript.Singleton.SoundEngine.Play(GenericSoundId.ItemLand);
				
				//Destroy(this.rigidbody);
				this.transform.position = expetectedPosition;
				Destroy(this);
				
				this.gameObject.layer = LayerMask.NameToLayer("Default");
			}

				

				break;
			}

			case StateEnum.Throw:
			{
				break;
			}

			case StateEnum.End:
			{
				break;
			}
		}
	
	}
}
