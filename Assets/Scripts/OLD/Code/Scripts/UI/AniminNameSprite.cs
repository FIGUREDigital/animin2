using UnityEngine;
using System.Collections;

public class AniminNameSprite : MonoBehaviour {

	[SerializeField]
	private RectTransform mPiSprite;
	[SerializeField]
	private RectTransform mTboSprite;
	[SerializeField]
	private RectTransform mKelseySprite;
	[SerializeField]
	private RectTransform mMandiSprite;
	// Use this for initialization
	void Start () 
	{
		mPiSprite.gameObject.SetActive (false);
		mTboSprite.gameObject.SetActive (false);
		mKelseySprite.gameObject.SetActive (false);
		mMandiSprite.gameObject.SetActive (false);

        switch(ProfilesManagementScript.Instance.CurrentProfile.ActiveAnimin)
		{
            case PersistentData.TypesOfAnimin.Pi:
			mPiSprite.gameObject.SetActive (true);
			break;
			case PersistentData.TypesOfAnimin.TboAdult:
            case PersistentData.TypesOfAnimin.Tbo:
			mTboSprite.gameObject.SetActive (true);
			break;
            case PersistentData.TypesOfAnimin.Kelsey:
			mKelseySprite.gameObject.SetActive (true);
			break;
            case PersistentData.TypesOfAnimin.Mandi:
			
			mMandiSprite.gameObject.SetActive (true);
			break;
		default:
			break;
		}

	}

}
