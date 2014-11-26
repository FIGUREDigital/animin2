using UnityEngine;
using System.Collections;

public class AchievementPanel : MonoBehaviour 
{

	private AchievementItem[] mItems;
	// Use this for initialization
	void Start () 
	{
		Init();
	}

	void Init()
	{
		mItems = GetComponentsInChildren<AchievementItem>();
		if(mItems.Length > (int)AchievementManager.Achievements.Count)
		{
			Debug.LogError("Error: Incorrect Achievement Length");
		}
	}

	void OnEnable()
	{
		if(mItems == null)
		{
			Init();
		}
		int i = 0;
		foreach(AchievementItem item in mItems)
		{
			bool achieved = AchievementManager.Instance.Achieved(i);
			item.Achieved = achieved;
			item.Description(AchievementManager.Instance.Description(i));
			i++;
		}
	}

	void OnDisable()
	{
		AchievementManager.Instance.SaveAchievments();
	}

	// Update is called once per frame
	void Update () {
	
	}
}
