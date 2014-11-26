using UnityEngine;
using System.Collections;

public class GatewayButton : MonoBehaviour {


	[SerializeField]
	private bool mActive;
	public bool Active {
		get { return mActive;}
		set { mActive = value;}
	}
	private ParentalGateway gateway;

	void Start()
	{
		gateway = GetComponentInParent<ParentalGateway>();
	}

	void OnClick()
	{
		if(mActive)
		{
			gateway.Pass();
		}
		else
		{
			gateway.Fail();
		}
	}

}
