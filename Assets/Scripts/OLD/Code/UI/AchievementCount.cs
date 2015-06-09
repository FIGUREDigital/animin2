using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class AchievementCount : MonoBehaviour 
{
	TextMeshProUGUI label;
	void Start()
	{
		label = GetComponent<TextMeshProUGUI>();
		label.text = AchievementManager.Instance.CompletedAchievements.ToString();
	}
	void OnEnable()
	{
		if(label == null)
		{
			label = GetComponent<TextMeshProUGUI>();
		}
		label.text = AchievementManager.Instance.CompletedAchievements.ToString();
	}

}
