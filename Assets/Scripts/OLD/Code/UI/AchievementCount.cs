using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AchievementCount : MonoBehaviour 
{
	Text label;
	void Start()
	{
		label = GetComponent<Text>();
		label.text = AchievementManager.Instance.CompletedAchievements.ToString();
	}
	void OnEnable()
	{
		if(label == null)
		{
			label = GetComponent<Text>();
		}
		label.text = AchievementManager.Instance.CompletedAchievements.ToString();
	}

}
