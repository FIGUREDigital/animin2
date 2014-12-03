using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AchievementItem : MonoBehaviour 
{
	private UnityEngine.UI.Image mTick;
	private UnityEngine.UI.Image mDot;
	public Text mDescription;

	public bool Achieved;

	private AchievementManager.Achievements mAchievement;

	void Start()
	{
		Init();
	}

	void Init()
	{
		mDot = gameObject.transform.FindChild("Dot").gameObject.GetComponent<UnityEngine.UI.Image>();
		if(mDot == null){Debug.Log("Error: dot not found");}
		mTick = gameObject.transform.FindChild("Tick").gameObject.GetComponent<UnityEngine.UI.Image>();
		if(mTick == null){Debug.Log("Error: tick not found");}
		mDescription = gameObject.GetComponentInChildren<Text>();
		if(mDescription == null){Debug.Log("Error: tick not found");};
	}
	void OnEnable()
	{
		if(mTick == null)
		{
			Init();
		}
		mTick.gameObject.SetActive(Achieved?true:false);
		mDot.gameObject.SetActive(Achieved?false:true);
	}

	public void Description(string text)
	{
		if(mDescription == null)
		{
			Init();
		}
		mDescription.text = text;
	}


}
