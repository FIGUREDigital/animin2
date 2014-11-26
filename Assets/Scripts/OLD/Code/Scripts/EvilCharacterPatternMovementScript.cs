using UnityEngine;
using System.Collections;

public class EvilCharacterPatternMovementScript : MonoBehaviour 
{
	public float Speed;
	public float Lerp;
	public Vector3[] Pattern;
	public int Index;
	public bool ApplyRotation;
	private Quaternion RotateDirectionLookAt;

    private bool m_Paused;
    public bool Paused {
        get { return m_Paused; }
        set {
            m_Paused = value;
            this.GetComponent<Animator>().enabled = !value;
        }
    }


	// Use this for initialization
	void Start () 
	{
	
	}
	// Update is called once per frame
	void Update () 
	{
        if (Paused) return;


		//Trust me, I hate this, too.

		GameObject mainChara = UIGlobalVariablesScript.Singleton.MainCharacterRef;

		Vector3 charaPos = mainChara.transform.position;
		float arb = 20f;
		Vector3 dist = this.transform.position - charaPos;
		float dist_amt = (this.transform.position - charaPos).magnitude;
		if (dist.y > -0.5f) {
			Vector3 nearest = charaPos + ((this.collider.bounds.center - charaPos).normalized * Mathf.Min (mainChara.GetComponent<CharacterController> ().radius * arb, dist_amt));
			bool contains = this.collider.bounds.Contains (nearest);
			if (contains) {
					UIGlobalVariablesScript.Singleton.CubeRunnerMinigameSceneRef.GetComponent<MinigameCollectorScript> ().OnEvilCharacterHit (this.gameObject);
			}
			Debug.DrawLine (charaPos, nearest, Color.red);
			Debug.DrawLine (charaPos, this.collider.bounds.center, Color.green);
		}
		//End hatred

		float currentRelativeSpeed = Vector3.Distance(Pattern[Index], Pattern[Index + 1]);
		
		float amount = Time.deltaTime * (Speed / currentRelativeSpeed);
		Lerp += amount;
		
		if(Lerp >= 1)
		{
			Index++;
			if(Index >= Pattern.Length - 1)
				Index = 0;
			Lerp = 0;
			//Debug.Log("Change Index: " + Index.ToString());
		}

		this.transform.localPosition = Vector3.Lerp(
			Pattern[Index], 
			Pattern[Index + 1], 
			Lerp);

		if(ApplyRotation)
		{
			RotateDirectionLookAt = Quaternion.LookRotation(Vector3.Normalize(Pattern[Index + 1] - transform.localPosition));
			transform.localRotation = Quaternion.Slerp(transform.localRotation, RotateDirectionLookAt, Time.deltaTime * 6);
		}
		else
		{
			Debug.Log("UPDATING MOVEMENT FOR CUBE");
		}
	}
}
