using UnityEngine;
using System.Collections;


public enum Animations { Idle = 0, IsWalking = 1, IsRunning = 2 }


public class PlayerController : MonoBehaviour {

	//[RequireComponent(typeof(CharacterController))]

	// SET ONCE
	private 	Animator 				__animator;
	private		CharacterController		__characterController;
	private 	float 					__movementSpeed;
    private 	float 					__gravity;

	private 	bool					__localPlayer;


	// SET THROUGHOUT
	private		float					__horizontalInput;
	private		float					__verticalInput;
	//private		AnimatorStateInfo 		__previousBaseState;
	//private		AnimatorStateInfo 		__currentBaseState;
	//private		AnimatorStateInfo 		__nextBaseState;
	private		Animations				__currentAnimation;
	private		bool					__running;

	

	// CORE
	// ---------------------------------------------------------------------------------------------------------------------------------------------------

	public void Start () {
		__animator = (Animator)transform.GetComponent(typeof(Animator));
		__characterController = GetComponent<CharacterController>();
		__movementSpeed = 6.0F;
		__gravity = 20.0F;
	}

	public void Update () {
		if (!__localPlayer) return;

		if (Globals.dragDistance != 0.0F) {
			UpdateAnimation(Animations.IsWalking);

			__horizontalInput = Globals.joystickPosition.x;
			__verticalInput = Globals.joystickPosition.y;

			UpdatePosition(CalculatePosition());
			UpdateRotation(CalculateRotation());
		} else {
			if (!__running) UpdateAnimation(Animations.Idle);
		}
	}

	public void LateUpdateHandler() {
		//__previousBaseState = __currentBaseState;
		//__currentBaseState = __animator.GetCurrentAnimatorStateInfo(0);
		//__nextBaseState = __animator.GetNextAnimatorStateInfo(0);
	}

	public void OnGUI() {
		if (!__localPlayer) return;

//		if (PhotonNetwork.connectionStateDetailed == PeerState.Joined) {
//			float buttonWidth = Screen.width * 0.1F;
//			float buttonHeight = Screen.height * 0.05F;
//			if (GUI.Button(new Rect(Screen.width - buttonWidth, 0, buttonWidth, buttonHeight), "Run!")) {
//				GetComponent<PhotonView>().RPC("Run", PhotonTargets.All); // updates others as well as local
//				//GetComponent<PhotonView>().RPC("Run", PhotonTargets.Others); // updates others only
//			}
//		}
	}


	// PUBLIC (NETWORK)
	// ---------------------------------------------------------------------------------------------------------------------------------------------------

	[RPC]
	public void Run() { StartCoroutine(Running()); }


	// PUBLIC (LOCAL)
	// ---------------------------------------------------------------------------------------------------------------------------------------------------

	public bool localPlayer { get { return __localPlayer;} }
	public Animations currentAnimation { get { return __currentAnimation;} }

	public void SetLocalPlayer(bool localPlayer) { __localPlayer = localPlayer; }

	public void UpdatePositionRemotely(Vector3 position) {
		transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * 5);
	}

	public void UpdateRotationRemotely(Quaternion rotation) {
		transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * 5);
	}

	///*
	public void UpdateAnimation(Animations newAnimation) {
		if (__currentAnimation == newAnimation) return;
		__currentAnimation = newAnimation;
	//*/
	/*
	public void UpdateAnimation(int newAnimation) {
		if ((int)__currentAnimation == newAnimation) return;
		__currentAnimation = (Animations)newAnimation;
	*/

		if (__currentAnimation == Animations.Idle) {
			//Debug.Log ("IDLE");
			
			__animator.SetBool(Animations.IsWalking.ToString(), false);

			__running = false;
			__animator.SetBool(Animations.IsRunning.ToString(), false);
		} else if (__currentAnimation == Animations.IsWalking && !__animator.GetBool(Animations.IsWalking.ToString())) {
			//Debug.Log ("WALKING");

			__animator.SetBool(Animations.IsWalking.ToString(), true);

			__running = false;
			__animator.SetBool(Animations.IsRunning.ToString(), false);
		} else if (__currentAnimation == Animations.IsRunning && !__animator.GetBool(Animations.IsRunning.ToString())) {
			//Debug.Log ("RUNNING");

			__animator.SetBool(Animations.IsWalking.ToString(), false);
			__animator.SetBool(Animations.IsRunning.ToString(), true);
		}
	}






	// PRIVATE
	// ---------------------------------------------------------------------------------------------------------------------------------------------------

	private Vector3 CalculatePosition() {
		// SPEED
		float speed = __movementSpeed * Globals.dragDistance;
		//Debug.Log ("speed			:	"	+	speed);
		

		// DIRECTION
		float directionX = 0F;
		float directionY = 0F;
		if (__horizontalInput >= 0) directionX = __horizontalInput;
		if (__horizontalInput <= 0) directionX = (__horizontalInput * -1);
		if (__verticalInput >= 0) directionY = __verticalInput;
		if (__verticalInput <= 0) directionY = (__verticalInput * -1);

		Vector3	direction = Vector3.zero;
		//if (__characterController.isGrounded) {
			float clampDirection = Mathf.Clamp01(directionX + directionY);
			direction = new Vector3(0, 0, clampDirection);
			direction *= speed;
			direction.y -= __gravity * Time.deltaTime;
		/*
        } else {
			direction.y -= __gravity * Time.deltaTime;
			__characterController.Move(direction * Time.deltaTime);
			return;
		}
		*/


		// CHANGE TO WORLD UNITS
		Vector3 movementDirection = transform.TransformDirection(direction);

		return movementDirection * Time.deltaTime;
	}

	private Vector3 CalculateRotation() {
		Vector3 lookTarget = new Vector3(__horizontalInput, 0F, __verticalInput);
		return lookTarget;
	}

	private void UpdatePosition(Vector3 position) {
		__characterController.Move(position);
	}
	
	private void UpdateRotation(Vector3 lookTarget) {
		transform.localRotation = Quaternion.Euler(0.0F, (Mathf.Atan2(lookTarget.x, lookTarget.z) * Mathf.Rad2Deg), 0.0F);
	}

	IEnumerator Running() {
		__running = true;
		UpdateAnimation(Animations.IsRunning);
		
		yield return new WaitForSeconds(1.0F);
		
		UpdateAnimation(Animations.Idle);
	}
}