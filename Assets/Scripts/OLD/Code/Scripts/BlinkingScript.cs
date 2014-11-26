using UnityEngine;
using System.Collections;

public class BlinkingScript : MonoBehaviour 
{
	public Texture2D Default;
	public Texture2D BlinkTexture;
	private float NextBlinkCounter;
	private float BlinkingTimer;
	private bool DoBlink;
	private float NextHappyIdleTimer;
	private float StayHappyIdleTimer;

	public Texture2D Happy1;
	public Texture2D Happy2;
	public Texture2D Happy3;
	public Texture2D Happy4;
	public Texture2D Happy5;

	public Texture2D HappyIdle;


	// Use this for initialization
	void Start () {
	
		NextBlinkCounter = UnityEngine.Random.Range(3.0f, 6.0f);
		NextHappyIdleTimer = UnityEngine.Random.Range(10.0f, 15.0f);
	}
	
	// Update is called once per frame
	void Update () 
	{
		AnimationHappyId happyId = UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<AnimationControllerScript>().IsHappy;

		if(happyId == AnimationHappyId.Happy1)
		{
			renderer.material.mainTexture = Happy1;
		}
		else if(happyId == AnimationHappyId.Happy2)
		{
			renderer.material.mainTexture = Happy2;
		}
		else if(happyId == AnimationHappyId.Happy3)
		{
			renderer.material.mainTexture = Happy3;
		}
		else if(happyId == AnimationHappyId.Happy4)
		{
			renderer.material.mainTexture = Happy4;
		}
		else if(happyId == AnimationHappyId.Happy5)
		{
			renderer.material.mainTexture = Happy5;
		}
		else if(NextHappyIdleTimer <= 0)
		{
//			Debug.Log("WWWWWWWWWWWWWW");
			renderer.material.mainTexture = HappyIdle;
			StayHappyIdleTimer -= Time.deltaTime;
			if(StayHappyIdleTimer <= 0)
			{
				NextHappyIdleTimer = UnityEngine.Random.Range(10.0f, 15.0f);
			}
		}
		else if(DoBlink)
		{
			BlinkingTimer -= Time.deltaTime;
			if(BlinkingTimer <= 0)
			{
				DoBlink = false;
				NextBlinkCounter = UnityEngine.Random.Range(2.0f, 5.5f);
			}
		}
		else if(NextBlinkCounter <= 0)
		{
			DoBlink = true;
			BlinkingTimer = UnityEngine.Random.Range(0.1f, 0.18f);
			renderer.material.mainTexture = BlinkTexture;
		}
		else
		{
			NextBlinkCounter -= Time.deltaTime;
			renderer.material.mainTexture = Default;

			if(ProfilesManagementScript.Singleton.CurrentAnimin.Happy >= 50)
			{
				NextHappyIdleTimer -= Time.deltaTime;
				StayHappyIdleTimer = 1.5f;
			}

		}
	}
}
