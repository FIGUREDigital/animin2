using UnityEngine;
using System.Collections;

public class AniminNameSprite : MonoBehaviour {

	[SerializeField]
	private string mPiSprite;
	[SerializeField]
	private string mTboSprite;
	[SerializeField]
	private string mKelseySprite;
	[SerializeField]
	private string mMandiSprite;
	// Use this for initialization
	void Start () 
	{
		//Image sprite = GetComponent<Image>();
		//sprite.atlas = mAtlas;

		string name = "";
        switch(ProfilesManagementScript.Singleton.CurrentProfile.ActiveAnimin)
		{
            case PersistentData.TypesOfAnimin.Pi:
			name = mPiSprite;
			break;
            case PersistentData.TypesOfAnimin.Tbo:
			name = mTboSprite;
			break;
            case PersistentData.TypesOfAnimin.Kelsey:
			name = mKelseySprite;
			break;
            case PersistentData.TypesOfAnimin.Mandi:
			name = mMandiSprite;
			break;
		default:
			break;
		}
		//sprite.spriteName = name;

	}

}
