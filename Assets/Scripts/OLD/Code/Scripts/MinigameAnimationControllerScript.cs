using UnityEngine;
using System.Collections;

public class MinigameAnimationControllerScript : MonoBehaviour {

	//public GameObject CharacterModel;

	public Animations GetAnimationEnum()
	{
		if(this.IsWalking) 
			return Animations.IsWalking;
		else if(this.IsRunning) 
			return Animations.IsRunning;
		
		return Animations.Idle;
	}
	
	public void SetAnimationFromEnum(Animations newAnimation)
	{
		Debug.Log("SetAnimationFromEnum: " + newAnimation.ToString());
		
		if (newAnimation == Animations.Idle) 
		{
			//Debug.Log ("IDLE");
			
			IsWalking = false;
			IsRunning = false;
		} 
		else if (newAnimation == Animations.IsWalking) 
		{
			IsWalking = true;
			IsRunning = false;
		} 
		else if (newAnimation == Animations.IsRunning) 
		{
			IsWalking = false;
			IsRunning = true;
		}
	}
	
	protected Animator animator;

	
	public bool IsJumbing 
	{
		get
		{
			
			return animator.GetCurrentAnimatorStateInfo(0).IsName("IsJumbing") || animator.GetBool("IsJumbing");
		}
		
		set
		{
			if (animator !=null)
				animator.SetBool("IsJumbing", value );
		}
	}

	
	void Awake()
	{
		
		
	}
		
	
	// Use this for initialization
	void Start () 
	{

	}

	public void LoadAnimator(GameObject gameObject)
	{
		animator = gameObject.GetComponent<Animator>();
	}
	
	public void SetMovementNormalized(float speed)
	{
		//Debug.Log(speed);
		animator.SetFloat("Movement", Mathf.Abs(speed));
	}
	
	public bool IsRunning
	{
		get
		{
			return animator.GetBool("IsRunning");
		}
		
		set
		{
			animator.SetBool("IsRunning", value);
		}
	}
	
	public bool IsWalking
	{
		get
		{
			return animator.GetBool("IsWalking");
		}
		
		set
		{
			animator.SetBool("IsWalking", value);
		}
	}
	
	
	// Update is called once per frame
	void Update () 
	{
		//animator = GetComponent<Animator>();
	}
	
		
	public bool TrueOrFalse()
	{
		if(Random.Range(0, 2) == 0) return true;
		return false;
	}
	
	
	void LateUpdate()
	{
		
	}
}
