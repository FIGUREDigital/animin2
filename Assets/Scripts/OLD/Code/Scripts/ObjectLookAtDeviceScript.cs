using UnityEngine;
using System.Collections;

public class ObjectLookAtDeviceScript : MonoBehaviour 

{

	float angle =0;
	float speed = (2 * Mathf.PI) / 5; //2*PI in degress is 360, so you get 5 seconds to complete a circle
	float radius=5;
	//public Transform ObjectTrasnform;

	//public Vector3 Angles;
	public float Weight;
	private float IdleTimer;
	private float TimeStartingAwayFromCamera;
	public float IsActiveTimer;


	void Start () 
	{
	
	}

	void Update () 
	{
        if (UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>() == null)
            return;
		angle += speed*Time.deltaTime; //if you want to switch direction, use -= instead of +=


		//Camera.main.transform.position = new Vector3 (Mathf.Sin(angle)*radius, 6.25f, Mathf.Cos(angle)*radius);
		//Camera.main.transform.LookAt (Vector3.zero);

		//this.transform.localRotation = Input.gyro.attitude;// new Quaternion(Input.gyro.attitude.x, 0, 0, 0);

		//this.transform.localRotation = new Quaternion (Input.gyro.attitude.x, Input.gyro.attitude.y, -Input.gyro.attitude.z, -Input.gyro.attitude.w);

		//this.transform.localRotation = Quaternion.Euler (new Vector3(360 - (Input.gyro.attitude.x * 90), 0, 0));

		//Debug.Log ("gyro rotation: " + Input.gyro.attitude.ToString());


		//Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x, Camera.main.transform.position.y -  Time.deltaTime * 1, Camera.main.transform.position.z);

		//Vector3 euler = Camera.main.transform.rotation.eulerAngles;
		//euler.x += 90;

		//DoBillboardOrientation ();
		//DoWholeOrientation ();
		//DoLookRotation ();

		//GetComponent<Animator>().SetLayerWeight(2, 1);

		float dotProduct = Vector3.Dot(Camera.main.transform.forward, this.transform.forward);
		//Debug.Log(dotProduct.ToString());

		if(!UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript>().IsMovingTowardsLocation &&
			(GetComponent<AnimationControllerScript>().IsIdle 
		 	|| GetComponent<AnimationControllerScript>().IsIdleWave 
		 	|| GetComponent<AnimationControllerScript>().IsIdleLook1 
		 	|| GetComponent<AnimationControllerScript>().IsIdleLook2 
		 	|| GetComponent<AnimationControllerScript>().IsIdleLook3 
		 	
		 	|| GetComponent<AnimationControllerScript>().IsHappy == AnimationHappyId.None)
		   && dotProduct >= -0.9f && UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<AnimateCharacterOutPortalScript>().JumbId == AnimateCharacterOutPortalScript.JumbStateId.None)
		{
			TimeStartingAwayFromCamera += Time.deltaTime;
			//transform.parent.LookAt(this.transform.position);
			//transform.parent.GetComponent<CharacterControllerScript>().moveDirection = transform.parent.position + transform.parent.forward * 5;
//			Debug.Log("turn around man");

			if(TimeStartingAwayFromCamera >= 1.5f)
			{
				//transform.parent.LookAt(Camera.main.transform.position);
				Vector3 newCameraPoint = new Vector3(Camera.main.transform.position.x, 0, Camera.main.transform.position.z);
				Vector3 playerPoint = new Vector3(transform.parent.position.x, 0, transform.parent.position.z);
				//transform.parent.GetComponent<CharacterControllerScript>().moveDirection = Vector3.Normalize(Camera.main.transform.position - newCameraPoint);
				//transform.parent.GetComponent<CharacterControllerScript>().MovementDirection = Vector3.Normalize(transform.parent.position - newCameraPoint);
				transform.GetComponent<CharacterControllerScript>().RotateToLookAtPoint(transform.position + Vector3.Normalize(newCameraPoint - playerPoint ) * 20);
				//Debug.Log("MOVING FOR ROTATION");
			}
		}
		else
		{
			TimeStartingAwayFromCamera = 0;
		}

	}

	void LateUpdate()
	{
		//ObjectTrasnform.rotation *= Quaternion.Euler(Angles);

		var relativePos = Camera.main.transform.position - this.transform.position;
		var rotation = Quaternion.LookRotation (relativePos);
//		Debug.Log(rotation.eulerAngles.ToString());

		float dotProduct = Vector3.Dot(Camera.main.transform.forward, this.transform.forward);

		//Debug.Log(dotProduct.ToString());

		IsActiveTimer -= Time.deltaTime;

		if(GetComponent<AnimationControllerScript>().IsIdle) IdleTimer += Time.deltaTime;
		else IdleTimer = 0;

		if(dotProduct < -0.2f && IdleTimer >= 0.2f && IsActiveTimer > 0)
		{
			Weight = Mathf.Lerp(Weight, 0.8f, Time.deltaTime * 3);
		}
		else
		{
			Weight = Mathf.Lerp(Weight, 0.0f, Time.deltaTime * 7);
		}

		HeadReferenceScript head = this.GetComponent<AnimationControllerScript>().CharacterModel.GetComponent<HeadReferenceScript>();

		head.HeadBoneToRotate.transform.rotation = Quaternion.Slerp(head.HeadBoneToRotate.transform.rotation, Quaternion.Euler(head.HeadRotationOffset + new Vector3(rotation.eulerAngles.x, rotation.eulerAngles.y, rotation.eulerAngles.z)), Weight);
	
	
		//head.HeadBoneToRotate.transform.rotation = rotation;
	}

	void OnAnimatorIK()
	{
		//DoLookRotation();
		//DoLookRotation();
		//Debug.Log("IKKKK");
	}

	private void DoLookRotation()
	{
		HeadReferenceScript head = GetComponent<CharacterSwapManagementScript>().CurrentModel.GetComponent<HeadReferenceScript>();


		var relativePos = Camera.main.transform.position - head.HeadBoneToRotate.transform.position;
				var rotation = Quaternion.LookRotation (relativePos);
		head.HeadBoneToRotate.transform.rotation = rotation;// * Quaternion.Euler(-90, 90, -90);
	}

	private void DoWholeOrientation()
	{
		Vector3 ray = Camera.main.transform.position - this.transform.position;
		ray.Normalize ();
		
		//transform.LookAt(transform.position + Quaternion.Euler(euler) * Vector3.back,
		//                 Camera.main.transform.rotation * Vector3.up);
		transform.LookAt (Camera.main.transform.position, Camera.main.transform.up);
		transform.localRotation = Quaternion.Euler (transform.localRotation.eulerAngles.x + 90, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z);
		
		//this.transform.LookAt (Camera.main.transform.position);
		//this.transform.localRotation = Quaternion.Euler (this.transform.localRotation.eulerAngles.x, this.transform.localRotation.eulerAngles.y - 180, this.transform.localRotation.eulerAngles.z);
		//Debug.Log(this.transform.localRotation.eulerAngles);

	}

	private void DoBillboardOrientation()
	{
		Vector3 v = Camera.main.transform.position - transform.position;
		
		v.x = v.z = 0.0f;
		
		transform.LookAt(Camera.main.transform.position - v); 
	}
}
