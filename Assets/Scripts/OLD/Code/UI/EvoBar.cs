using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EvoBar : MonoBehaviour 
{
	[SerializeField]
	private UnityEngine.UI.Image mEvoFill;
	private int mSpriteWidth = 1330;
	private List<GameObject> mMarkers = new List<GameObject>();
	private const string EVO_EX_NAME = "evoMarker_exclamation";
	private const string EVO_STAR_NAME = "evoMarker_star";
	private int mNumBabyMarkers;
	private int mNumKidMarkers;
	private int mNumAdultMarkers;
	private AniminEvolutionStageId mEvoID;
	private AniminEvolutionStageId mPrevEvoID;
	private bool initial = true;

	void OnEnable()
	{
		PlaceMarkers();
		AniminEvolutionStageId id = ProfilesManagementScript.Instance.CurrentAnimin.AniminEvolutionId;

		CalcEvolution ();
	}
	private void CalcEvolution()
	{
		mEvoFill.fillAmount = ProfilesManagementScript.Instance.CurrentAnimin.Evolution/100;
	}
	private void PlaceMarkers()
	{
		mEvoID = ProfilesManagementScript.Instance.CurrentAnimin.AniminEvolutionId;
		if(mEvoID == mPrevEvoID && !initial)
		{
			return;
		}
		mPrevEvoID = ProfilesManagementScript.Instance.CurrentAnimin.AniminEvolutionId;
		initial = false;

		if(mMarkers.Count > 0)
		{
			foreach(GameObject go in mMarkers)
			{
				Destroy(go);
			}
			mMarkers.Clear();
		}

		int baby = EvolutionManager.Instance.BabyEvolutionThreshold;
		int kid = EvolutionManager.Instance.KidEvolutionThreshold;
		int adult = EvolutionManager.Instance.KidEvolutionThreshold;
		int markerRate = EvolutionManager.Instance.MarkerRate;

		int min = 0;
		int max = 0;
		int diff = 0;
		AniminEvolutionStageId stage = ProfilesManagementScript.Instance.CurrentAnimin.AniminEvolutionId;

		switch(stage)
		{
		case AniminEvolutionStageId.Baby:
			min = 0;
			max = baby;
			break;
		case AniminEvolutionStageId.Kid:
			min = baby;
			max = kid;
			break;
		case AniminEvolutionStageId.Adult:
			min = kid;
			max = adult;
			break;
		default:
			break;
		}

		diff = max - min;

		int number = diff / markerRate;
		float spacing = (float)((float)markerRate/(float)diff);

		bool marker = false;
		float halfWidth = (mSpriteWidth * 0.5f) - ((0.5f * spacing) * mSpriteWidth);
		float scale = UIGlobalVariablesScript.Singleton.gameObject.transform.localScale.x;
		for(int i = 0; i < number; i++)
		{
			Vector3 pos = transform.position + new Vector3((((i * spacing) * mSpriteWidth) - halfWidth) * scale,0,0);
			GameObject go = MakeThing(pos, marker ? EVO_EX_NAME : EVO_STAR_NAME);
			marker = !marker;
			mMarkers.Add(go);
		}

	}
	
	private GameObject MakeExclamation(Vector3 pos)
	{
		return MakeThing(pos, EVO_EX_NAME);
	}

	private GameObject MakeStar(Vector3 pos)
	{
		return MakeThing(pos, EVO_STAR_NAME);
	}

	private GameObject MakeThing(Vector3 pos, string name)
	{
		GameObject exclamation = new GameObject();
		exclamation.layer = gameObject.layer;
		exclamation.transform.parent = transform;
		exclamation.transform.position = pos;
//		Image sprite = exclamation.AddComponent<Image>();
//		sprite.spriteName = name;
//		sprite.depth = 3;
//		sprite.width = 97;
//		sprite.height = 121;
		return exclamation;
	}
}
