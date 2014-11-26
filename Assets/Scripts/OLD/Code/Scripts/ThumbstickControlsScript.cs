using UnityEngine;
using System.Collections;

public class ThumbstickControlsScript : MonoBehaviour 
{
	public float MovementSpeed = 1;

	private bool IsMoving;
	private int MovementFingerId;
	private Vector2 LastMovementFingerPosition;
	private Vector2 MovementDirection;
	//private int JumbFingerId;


	public GameObject MinigameRef;

	public Vector2 FirstTouchMovement;


	// Use this for initialization
	void Start () 
	{

	
	}
	
	// Update is called once per frame
	void Update () 
	{
		bool foundMovementFinger = false;
		bool hadJumb = false;


		if (Input.touchCount > 0) {
		
			for (int i=0; i<Input.touchCount; ++i) {
				if ((Input.touches[i].fingerId == MovementFingerId)) {
					foundMovementFinger = true;

					if(UIGlobalVariablesScript.Singleton.CubeRunnerMinigameSceneRef.GetComponent<MinigameCollectorScript>().TutorialId == MinigameCollectorScript.TutorialStateId.ShowMovement)
						UIGlobalVariablesScript.Singleton.CubeRunnerMinigameSceneRef.GetComponent<MinigameCollectorScript>().AdvanceTutorial();


					if(Input.touches[i].position != LastMovementFingerPosition && Vector2.Distance(Input.touches[i].position, LastMovementFingerPosition) >= 3)
					{
						//Debug.Log(Vector2.Distance(Input.touches[i].position, LastMovementFingerPosition).ToString());
						MovementDirection = Vector3.Normalize (Input.touches [i].position - LastMovementFingerPosition);
						//MovementDirection.Normalize();
					}

					LastMovementFingerPosition = Input.touches[i].position; 

					//MovementDirection = Vector3.Normalize (Input.touches [i].position - FirstTouchMovement);


					break;
				}
				/*else if ((Input.touches[i].fingerId == JumbFingerId)) {
					hadJumb = true;
					break;
				}*/
			}
		}


		/*if (hadJumb) {	

			this.GetComponent<CharacterControllerScript>().PressedJumb = true;

		} 
		else {*/
			for (int i=0; i<Input.touchCount; ++i) 
			{
				if ((Input.touches [i].fingerId == MovementFingerId))
						continue;

				if (Input.touches [i].phase == TouchPhase.Began) {

					if (Input.touches [i].position.x > Screen.width / 2) {
							//Debug.Log ("jumbed!");
							//JumbFingerId = Input.touches[i].fingerId;
							this.GetComponent<CharacterControllerScript> ().PressedJumb = true;
							break;
					}
				}
			}
		//}


		if (foundMovementFinger) {	
			//Move (new Vector3(MovementDirection.x, 0, MovementDirection.y));


		} 
		else {
			for (int i=0; i<Input.touchCount; ++i) {

				if(Input.touches[i].position.x <= Screen.width/ 2)
				{
					foundMovementFinger = true;
					MovementFingerId = Input.touches[i].fingerId;
					LastMovementFingerPosition = Input.touches[i].position;
					FirstTouchMovement = Input.touches[i].position;
					break;
				}
			}
		}

		if (!foundMovementFinger) {
			MovementFingerId = -1;
			MovementDirection = Vector2.zero;
		}

		if (!hadJumb) {
		
			//JumbFingerId = -1;
		}

#if UNITY_EDITOR
		//this.GetComponent<CharacterControllerScript>().HorizontalDirection = Input.GetAxisRaw("Horizontal");
		//this.GetComponent<CharacterControllerScript>().VerticalDirection = Input.GetAxisRaw("Vertical");

		if(Input.GetButton("Jump"))
			this.GetComponent<CharacterControllerScript>().PressedJumb = true;
#else

		//this.GetComponent<CharacterControllerScript>().HorizontalDirection = MovementDirection.x;
		//this.GetComponent<CharacterControllerScript>().VerticalDirection = MovementDirection.y;
#endif



	}


	/*
	public void Jumb()
	{
	}

	public void Move(Vector3 direction)
	{
		this.transform.position += direction * Time.deltaTime * 50;
	}*/
}
