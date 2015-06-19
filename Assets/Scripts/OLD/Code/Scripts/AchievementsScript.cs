using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
public enum AchievementMedels
{
	achievementIconGold,
	achievementIconSilver,
	achievementIconBronze,
	achievementIconStar,
	achievementIconMarker1,
	achievementIconMarker2,
	achievementIconMarker3,
	achievementIconWorm,
	achievementIconBirthday,
	Count
}
public class AchievementsScript : MonoBehaviour 
{
	public static AchievementsScript Singleton;

	public GameObject AchievementObject;
	public UnityEngine.UI.Image MedalIcon;
	public TextMeshProUGUI Title;
	public TextMeshProUGUI Description;
	public Gradient BackgroundGradient;

	private float Timer;
	private float Alpha = 0;

	void Awake()
	{
		Singleton = this;
		//Show(AchievementTypeId.Gold, 400);
	}

	public void Show(AchievementTypeId id, int points)
	{
		GetComponent<AudioSource>().Play();
		AchievementObject.SetActive(true);
		Timer = 5;
        //AchievementObject.GetComponent<RectTransform>().bottomAnchor.absolute = -509;

        if (UIGlobalVariablesScript.Singleton == null)
            return;
        if (UIGlobalVariablesScript.Singleton.MainCharacterRef == null)
            return;
        if (UIGlobalVariablesScript.Singleton.MainCharacterRef.GetComponent<CharacterProgressScript> () == null)
            return;

		switch(id)
		{
		case AchievementTypeId.Gold:
			{
				Title.text = "Gold Award!";
				BackgroundGradient.vertex1 = new Color32(247,255,38,255);
				Description.text = string.Format(@"Well done! you scored {0} points.", points);
				//MedalIcon.sprite = store.GetMedel(AchievementMedels.achievementIconGold);
				break;
			}
		case AchievementTypeId.Bronze:
			{
				Title.text = "Bronze Award!";
			BackgroundGradient.vertex1 = new Color32(247,255,38,255);
				Description.text = string.Format(@"Well done! you scored {0} points.", points);
				//MedalIcon.sprite = store.GetMedel(AchievementMedels.achievementIconBronze);
				break;
			}
		case AchievementTypeId.Silver:
			{
				Title.text = "Silver Award!!";
			BackgroundGradient.vertex1 = new Color32(247,255,38,255);
				Description.text = string.Format(@"Well done! you scored {0} points.", points);
				//MedalIcon.sprite = store.GetMedel(AchievementMedels.achievementIconSilver);
				break;
			}
		case AchievementTypeId.Achievement:
			{
				Title.text = "Achievement!";
			BackgroundGradient.vertex1 = new Color32(89,255,38,255);
				Description.text = "Congratulations, you got an achievement.";
				//MedalIcon.sprite = store.GetMedel(AchievementMedels.achievementIconStar);
				break;
			}

		case AchievementTypeId.Evolution:
			{
				Title.text = "Your Animin has evolved!";
			BackgroundGradient.vertex1 = new Color32(89,255,38,255);
				Description.text = "Well done! Keep taking care of your Animin and training them up.";
				//MedalIcon.sprite = store.GetMedel(AchievementMedels.achievementIconMarker3);
				break;
			}

		case AchievementTypeId.EvolutionExclamation:
			{
				Title.text = "Your animin has grown!";
			BackgroundGradient.vertex1 = new Color32(89,255,38,255);
				Description.text = "Well done! Keep taking care of your Animin and training them up.";
				//MedalIcon.sprite = store.GetMedel(AchievementMedels.achievementIconMarker3);
				break;
			}
		case AchievementTypeId.EvolutionStar:
			{
				Title.text = "You unlocked a surprise!";
			BackgroundGradient.vertex1 = new Color32(89,255,38,255);
				Description.text = "Well done! Keep taking care of your Animin and training them up.";
				//MedalIcon.sprite = store.GetMedel(AchievementMedels.achievementIconMarker1);
				break;
			}
		case AchievementTypeId.Tutorial:
			{
				Title.text = "Yo!";
			BackgroundGradient.vertex1 = new Color32(255,190,38,0);
				Description.text = "Well done! Keep taking care of your Animin and training them up.";
				//MedalIcon.sprite = store.GetMedel(AchievementMedels.achievementIconWorm);
				break;
			}
		case AchievementTypeId.Birthday:
			{
				Title.text = "Happy Birthday!";
			BackgroundGradient.vertex1 = new Color32(255,38,124,255);
				Description.text = "Well done! Keep taking care of your Animin and training them up.";
				//MedalIcon.sprite = store.GetMedel(AchievementMedels.achievementIconBirthday);
				break;
			}
		}
	}

	
	// Update is called once per frame
	void Update () 
	{
		Timer -= Time.deltaTime;
		if(Timer <= 0)
		{
			Alpha -= Time.deltaTime * 2.0f;
			if(Alpha <= 0)
			{
				Alpha = 0;
				AchievementObject.SetActive(false);
			}
		}
		else
		{
//			Alpha += Time.deltaTime * 0.8f;
//			if(Alpha >= 1) 
//			{
//				Alpha = 1;
//			}
		}

		if(Timer > 0)
		{
			Alpha = Mathf.Lerp(Alpha, 1, Time.deltaTime * 2.6f);
			//float height = Mathf.Lerp(AchievementObject.GetComponent<UIWidget>().bottomAnchor.absolute, 245, Time.deltaTime * 2.6f);
			//AchievementObject.GetComponent<UIWidget>().bottomAnchor.absolute = (int)height;
		}

		
		for(int i=0;i<AchievementObject.transform.childCount;++i)
		{
			GameObject transform = AchievementObject.transform.GetChild(i).gameObject;

			
			Text label = transform.GetComponent<Text>();
			if(label != null)
			{
				label.color = new Color(1,1,1, Alpha);
			}
			
		}
	}
}

public enum AchievementTypeId
{
	None = 0,
	Gold,
	Silver,
	Bronze,
	Evolution,
	Achievement,
	Tutorial,
	EvolutionExclamation,
	EvolutionStar,
	Birthday,
}
