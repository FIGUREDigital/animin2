using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class JoystickPageControls : MonoBehaviour
{
    public bool Paused;

    private MinigameCollectorScript MinigameScript;
    private MinigameAnimationControllerScript CharacterAnimationRef;
    private CharacterControllerScript CharacterControllerRef;

    [SerializeField]
    private GameObject m_JoystickFront, m_JoystickBack, m_JumpButton;

    private bool isButtonDown = false;
    private int fingerID = -1;

    private bool m_JumpButtonPressed;
    private bool m_Jump;
    public bool PressedJump{
        get{
            return m_Jump;
        }
    }
    private bool m_IsMoving;
    public bool IsMovingWithJoystick{
        get{
			if (Paused) return false;
            return m_IsMoving;
        }
    }

    // Use this for initialization
    void Start(){
        Init();
    }
    void OnEnable(){
        Init();
    }
    void Init () {
        //MinigameScript = UIGlobalVariablesScript.Singleton.CubeRunnerMinigameSceneRef.GetComponent<MinigameCollectorScript>();
        CharacterControllerRef = UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterControllerScript>();
        CharacterAnimationRef = UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<MinigameAnimationControllerScript>();

        if (MainARHandler.Instance.CurrentGameScene == GameScenes.MinigameCubeRunner)
            m_JumpButton.SetActive(true);
        else if (MainARHandler.Instance.CurrentGameScene == GameScenes.MinigameCannon)
            m_JumpButton.SetActive(false);
        
    }
	
    void Update () {
		if (Paused) return;

		isButtonDown = false;
		Camera cam = Phi.SingletonPrefab.instances["UICamera"].GetComponent<Camera>();
		Vector3 mousePosition = Vector3.zero;
        #if UNITY_EDITOR
        if(Input.GetMouseButton(0))
        {
            isButtonDown = true;
			mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        }
        #endif
        for(int i=0;i<Input.touchCount;++i)
		{
			Vector3 pos = Input.GetTouch(i).position;//cam.ScreenToWorldPoint(Input.GetTouch(i).position);
			//Debug.Log ("pos = "+pos+" "+Input.GetTouch(i).position);
            if(fingerID == -1)
            {
                if(isHittingThumbBack(pos))
                {
                    fingerID = Input.GetTouch(i).fingerId;
                    isButtonDown = true;
					mousePosition = cam.ScreenToWorldPoint(pos);
                }
            }
            else
            {
                if(Input.GetTouch(i).fingerId == fingerID)
                {
                    isButtonDown = true;
					mousePosition = cam.ScreenToWorldPoint(pos);
                }
            }
		}
		//mousePosition.x -= Screen.width/2;
		//mousePosition.y -= Screen.height/2;
		
		Debug.DrawLine(m_JoystickBack.transform.position, mousePosition, Color.blue);

        if(!isButtonDown) fingerID = -1;

        float movementSpeed = 0;

//        bool fingerTouchValid = false;

        if (isButtonDown)
        {
			Vector3 diff = mousePosition - m_JoystickBack.transform.position;
			diff.z = 0;
			Debug.DrawLine(m_JoystickBack.transform.position, m_JoystickBack.transform.position + diff, Color.red);
            float maxRadius = m_JoystickBack.GetComponent<RectTransform>().rect.width/2;
            if (diff.magnitude >= maxRadius)
                diff = diff.normalized * maxRadius;


			
			Debug.DrawLine(m_JoystickBack.transform.position, m_JoystickBack.transform.position + diff, Color.yellow);

            m_JoystickFront.transform.position = m_JoystickBack.GetComponent<RectTransform>().position + diff;

			diff = diff * (50.0f / maxRadius);	// Normalize into range the game was deisgned for
			maxRadius = 50.0f;

            CharacterControllerRef.MovementDirection = Camera.main.transform.right * diff.x;//new Vector3(VJRnormals.x, 0, VJRnormals.y);
            CharacterControllerRef.MovementDirection += Vector3.Normalize(new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.transform.forward.z)) * diff.y;
            CharacterControllerRef.MovementDirection.y = 0;

			movementSpeed = diff.magnitude / maxRadius;
			Debug.Log ("movementSpeed ="+movementSpeed);
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
            CharacterControllerRef.walkSpeed =  movementSpeed * 2.4f;
            CharacterControllerRef.RotateToLookAtPoint(CharacterControllerRef.transform.position + CharacterControllerRef.MovementDirection * 6);

            m_IsMoving = true;
        }
        else
        {
            m_JoystickFront.transform.localPosition = m_JoystickBack.transform.localPosition;
            CharacterAnimationRef.IsRunning = false;
            CharacterAnimationRef.IsWalking = false;
            CharacterControllerRef.MovementDirection = Vector3.zero;

            m_IsMoving = false;
        }

    }
    void LateUpdate(){
        //I'm paranoid about script execution. That's why this stuff is doing all this. I don't know when events get called in the Update/LateUpdate cycle.
        if (m_Jump)
            m_Jump = false;
        if (m_JumpButtonPressed)
        {
            m_JumpButtonPressed = false;
            m_Jump = true;
        }
    }
    private bool isHittingThumbBack(Vector3 position){

        PointerEventData pe = new PointerEventData(EventSystem.current);
        pe.position =  position;

        List<RaycastResult> hits = new List<RaycastResult>();
        EventSystem.current.RaycastAll( pe, hits );

        foreach (RaycastResult hit in hits)
        {
            if (hit.gameObject == m_JoystickBack)
            {
                return true;
            }
        }
        return false;
    }

    public void JumpButton(){
        m_JumpButtonPressed = true;
        Debug.Log("Jump Button Pressed");
        CharacterControllerRef.PressedJumb = true;
    }
}
