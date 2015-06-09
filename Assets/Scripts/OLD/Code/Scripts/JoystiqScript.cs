using UnityEngine;
using System.Collections;

public class JoystiqScript : MonoBehaviour {

	public Sprite ThumbpadFront;
	public Sprite ThumbpadBack;

	private float LeftAnchorStartPosition;
	private float RightAnchorStartPosition;
	private float TopAnchorStartPosition;
	private float BottomAnchorPosition;

	public static Vector2 VJRvector;    // Joystick's controls in Screen-Space.
	
	public static Vector2 VJRnormals;   // Joystick's normalized controls.
	
	public static bool VJRdoubleTap;    // Player double tapped this Joystick.
	
	//public Color activeColor;           // Joystick's color when active.
	
	//public Color inactiveColor;         // Joystick's color when inactive.
	
	//public Texture2D joystick2D;        // Joystick's Image.
	
	//public Texture2D background2D;      // Background's Image.
	//public Texture2D jumb2D;
	
	//private GameObject backOBJ;         // Background's Object.

	//private GameObject jumbOBJ;

	//private GSprite jumbTexture;
	
	//private GSprite joystick;        // Joystick's GUI.
	
	//private GSprite background;      // Background's GUI.
	
	private Vector2 origin;             // Touch's Origin in Screen-Space.
	
	private Vector2 position;           // Pixel Position in Screen-Space.
	
	private int size;                   // Screen's smaller side.
	
	private float length;               // The maximum distance the Joystick can be pushed.
	
	private bool gotPosition;           // Joystick has a position.
	
	private int fingerID;               // ID of finger touching this Joystick.
	
	private int lastID;                 // ID of last finger touching this Joystick.
	
	private float tapTimer;             // Double-tap's timer.
	
	private bool enable;                // VJR external control.

    private bool m_Paused;
    public bool Paused
    {
        get { return m_Paused; }
        set { m_Paused = value; }
    }

	
	public MinigameAnimationControllerScript CharacterAnimationRef;
	public CharacterControllerScript CharacterControllerRef;

	
	
	private void Awake() 
	{
	

		fingerID = -1; 
		lastID = -1; 
		VJRdoubleTap = false; 
		tapTimer = 0; 
		length = 140;

	
	}

	void Start()
	{
//		LeftAnchorStartPosition = ThumbpadFront.leftAnchor.absolute;
//		RightAnchorStartPosition = ThumbpadFront.rightAnchor.absolute;
//		TopAnchorStartPosition = ThumbpadFront.topAnchor.absolute;
//		BottomAnchorPosition = ThumbpadFront.bottomAnchor.absolute;
	}
	

	private Vector2 finalMovementDirection = Vector2.zero;
	
	private void Update() 
	{
		/*if(Input.GetKey(KeyCode.A))
		{
			Debug.Log("DOING A: " + ThumbpadBack.leftAnchor.relative.ToString() + "_" + ThumbpadBack.leftAnchor.absolute.ToString() + "_" + ThumbpadFront.width.ToString());


			ThumbpadFront.leftAnchor.Set(ThumbpadFront.leftAnchor.relative, ThumbpadFront.leftAnchor.absolute + 1);
			ThumbpadFront.rightAnchor.Set(ThumbpadFront.rightAnchor.relative, ThumbpadFront.rightAnchor.absolute + 1);
			ThumbpadFront.ResetAnchors();
			ThumbpadFront.UpdateAnchors();
		}*/

		bool isButtonDown = false;
		Vector3 mousePosition = Vector3.zero;

#if UNITY_EDITOR
		if(Input.GetMouseButton(0))
		{
			isButtonDown = true;
			mousePosition = Input.mousePosition;
		}
#endif
		if(!isButtonDown) fingerID = -1;
		
		float movementSpeed = 0;
		
//		bool fingerTouchValid = false;
		/*
		for(int i=0;i<Input.touchCount;++i)
		{
			TouchPhase phase = Input.GetTouch(i).phase;

			if(fingerID == -1)
			{
				Vector3 bottomLeftWorld = Camera.main.WorldToScreenPoint(ThumbpadBack.worldCorners[0]);
				Vector3 topRightWorld = Camera.main.WorldToScreenPoint(ThumbpadBack.worldCorners[2]);

				if(Input.GetTouch(i).position.x < topRightWorld.x 
				   && Input.GetTouch(i).position.y < topRightWorld.y 
				   && Input.GetTouch(i).phase == TouchPhase.Began)
				{
					fingerID = Input.GetTouch(i).fingerId;
					isButtonDown = true;
					mousePosition = Input.GetTouch(i).position;


				}
			}
			else
			{
				if(Input.GetTouch(i).fingerId == fingerID)
				{
					isButtonDown = true;
					mousePosition = Input.GetTouch(i).position;
				}
			}
		}



		if(isButtonDown && !Paused)
		{
			//float ffff = (ThumbpadBack.worldCorners[2].x - ThumbpadBack.worldCorners[0].x) * ThumbpadBack.width;
			fingerTouchValid = true;
			Vector3 bottomLeftWorld = Camera.main.WorldToScreenPoint(ThumbpadBack.worldCorners[0]);
			Vector3 topRightWorld = Camera.main.WorldToScreenPoint(ThumbpadBack.worldCorners[2]);
			//Debug.Log("bottomLeftWorld: " + bottomLeftWorld.ToString());
			//Debug.Log("topRightWorld: " + topRightWorld.ToString());

			if (UIGlobalVariablesScript.Singleton.CubeRunnerMinigameSceneRef!= null && UIGlobalVariablesScript.Singleton.CubeRunnerMinigameSceneRef.GetComponent<MinigameCollectorScript>()!=null)
				if( UIGlobalVariablesScript.Singleton.CubeRunnerMinigameSceneRef.GetComponent<MinigameCollectorScript>().TutorialId == MinigameCollectorScript.TutorialStateId.ShowMovement)
					UIGlobalVariablesScript.Singleton.CubeRunnerMinigameSceneRef.GetComponent<MinigameCollectorScript>().AdvanceTutorial();

            if (UIGlobalVariablesScript.Singleton.GunGameScene!= null && UIGlobalVariablesScript.Singleton.GunGameScene.GetComponent<GunsMinigameScript>()!=null)
            if( UIGlobalVariablesScript.Singleton.GunGameScene.GetComponent<GunsMinigameScript>().TutorialID == GunsMinigameScript.TutorialStateId.ShowMove)
                UIGlobalVariablesScript.Singleton.GunGameScene.GetComponent<GunsMinigameScript>().AdvanceTutorial();


			Vector3 middle = bottomLeftWorld + (topRightWorld - bottomLeftWorld) / 2;

			//float invertedY = Screen.height - Input.mousePosition.y;

			//if(Input.mousePosition.x >= bottomLeftWorld.x && Input.mousePosition.x <= topRightWorld.x)
			{
				//if(Input.mousePosition.y >= bottomLeftWorld.y && Input.mousePosition.y <= topRightWorld.y)
				{




					float horizontalDistance = (mousePosition.x - middle.x);
					horizontalDistance /= ((topRightWorld.x - bottomLeftWorld.x) / 2);
					if(horizontalDistance < -1) horizontalDistance = -1;
					if(horizontalDistance > 1) horizontalDistance = 1;

					float verticalDistance = (mousePosition.y - middle.y);
					verticalDistance /= ((topRightWorld.y - bottomLeftWorld.y) / 2);
					if(verticalDistance < -1) verticalDistance = -1;
					if(verticalDistance > 1) verticalDistance = 1;

					float maxRadius = (topRightWorld.y - middle.y);// + (topRightWorld.y - middle.y) * 0.8f;
					Vector2 directionVector = (new Vector2(mousePosition.x, mousePosition.y) - new Vector2(middle.x, middle.y));
					directionVector.Normalize();

					float currentDistance = Vector2.Distance(new Vector2(mousePosition.x, mousePosition.y), new Vector2(middle.x, middle.y));
					if(currentDistance >= maxRadius) currentDistance = maxRadius;
					movementSpeed = currentDistance / maxRadius;
					//Debug.Log("speeD: " + movementSpeed.ToString());

					float lerpH = (directionVector.x * Mathf.Abs(horizontalDistance)) * ((ThumbpadFront.width * 0.9f) / 2);
					float lerpV = (directionVector.y * Mathf.Abs(verticalDistance)) * ((ThumbpadFront.height * 0.9f) / 2);

					ThumbpadFront.leftAnchor.Set(ThumbpadFront.leftAnchor.relative, LeftAnchorStartPosition + lerpH);
					ThumbpadFront.rightAnchor.Set(ThumbpadFront.rightAnchor.relative, RightAnchorStartPosition + lerpH);

					ThumbpadFront.topAnchor.Set(ThumbpadFront.topAnchor.relative, TopAnchorStartPosition + lerpV);
					ThumbpadFront.bottomAnchor.Set(ThumbpadFront.bottomAnchor.relative, BottomAnchorPosition + lerpV);
				
					finalMovementDirection.x = directionVector.x;
					finalMovementDirection.y = directionVector.y;

				}
			}
		}


		if(!fingerTouchValid)
		{
			finalMovementDirection = Vector2.zero;
			ThumbpadFront.leftAnchor.Set(ThumbpadFront.leftAnchor.relative, LeftAnchorStartPosition);
			ThumbpadFront.rightAnchor.Set(ThumbpadFront.rightAnchor.relative, RightAnchorStartPosition);
			
			ThumbpadFront.topAnchor.Set(ThumbpadFront.topAnchor.relative, TopAnchorStartPosition);
			ThumbpadFront.bottomAnchor.Set(ThumbpadFront.bottomAnchor.relative, BottomAnchorPosition);
		}
		*/
		if(CharacterControllerRef != null)
		{
			CharacterControllerRef.MovementDirection = Camera.main.transform.right * finalMovementDirection.x;//new Vector3(VJRnormals.x, 0, VJRnormals.y);
			CharacterControllerRef.MovementDirection += Vector3.Normalize(new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.transform.forward.z)) * finalMovementDirection.y;
			CharacterControllerRef.MovementDirection.y = 0;


			if(finalMovementDirection != Vector2.zero)
			{
				if(movementSpeed < 0.5f)
				{

					CharacterAnimationRef.IsRunning = false;
					CharacterAnimationRef.IsWalking = true;
				}
				else
				{
					CharacterAnimationRef.IsRunning = true;
					CharacterAnimationRef.IsWalking = false;
				}
				CharacterControllerRef.walkSpeed =  movementSpeed * 150.0f;
				CharacterControllerRef.RotateToLookAtPoint(CharacterControllerRef.transform.position + CharacterControllerRef.MovementDirection * 6);
			}
			else
			{
				CharacterAnimationRef.IsRunning = false;
				CharacterAnimationRef.IsWalking = false;
				CharacterControllerRef.MovementDirection = Vector3.zero;
			}
		}

	}
}
