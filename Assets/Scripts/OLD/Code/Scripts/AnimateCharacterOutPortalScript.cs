using UnityEngine;
using System.Collections;

public class AnimateCharacterOutPortalScript : MonoBehaviour {

	public float Timer;
	public enum JumbStateId
	{
		None = 0,
		JumbIn,
		Jumbout,
	}
	public JumbStateId JumbId;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		switch(JumbId)
		{
			case JumbStateId.JumbIn:
			{
				Timer += Time.deltaTime;

				if(Timer >= 0.5f)
					//this.transform.position -= this.transform.forward * Time.deltaTime * 120;

				if(Timer >= 1.3f)
                {
                    //This bit doesn't actually run. The scene changes before this thing gets a chance.
                    Debug.Log("JumBBBBBBBBBing finished");
					JumbId = JumbStateId.None;
				}

				break;
			}

			case JumbStateId.Jumbout:
			{
				Timer += Time.deltaTime;

				this.transform.position += Vector3.back * Time.deltaTime * 120;
				
				if(Timer >= 0.6f)
				{
					JumbId = JumbStateId.None;
				}

				break;
			}

			case JumbStateId.None:
			{
				break;
			}
		}
	}
}
