using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum Slider
{
	Happiness,
	Health,
	Hunger,
	Fitness,
	Count
}
public class SliderController : MonoBehaviour 
{
	[SerializeField]
	private Slider mSlider;
	private RectTransform mImage;
//	private const float min = 0;
	private const float min = -100;
	private const float max = 100;

	void Start()
	{

	}
	void OnEnable()
	{
		mImage = GetComponent<RectTransform> ();
		float leftResult = 0;
		float rightResult = 0;
		/*Remember to comment these back in when UI is working.
        UIGlobalVariablesScript.Singleton.HungryControlBarRef.transform.localPosition = new Vector3(Mathf.Lerp(-80.51972f, 617.2906f, ProfilesManagementScript.Singleton.CurrentAnimin.Hungry / 100.0f), UIGlobalVariablesScript.Singleton.HungryControlBarRef.transform.localPosition.y, 0);
        UIGlobalVariablesScript.Singleton.HealthControlBarRef.transform.localPosition = new Vector3(Mathf.Lerp(-80.51972f, 617.2906f, ProfilesManagementScript.Singleton.CurrentAnimin.Health / 100.0f), UIGlobalVariablesScript.Singleton.HealthControlBarRef.transform.localPosition.y, 0);
        UIGlobalVariablesScript.Singleton.HapynessControlBarRef.transform.localPosition = new Vector3(Mathf.Lerp(-80.51972f, 617.2906f, 
        ProfilesManagementScript.Singleton.CurrentAnimin.Happy / PersistentData.MaxHappy), UIGlobalVariablesScript.Singleton.HapynessControlBarRef.transform.localPosition.y, 0);
        UIGlobalVariablesScript.Singleton.FitnessControlBarRef.transform.localPosition = new Vector3(Mathf.Lerp(-80.51972f, 617.2906f, ProfilesManagementScript.Singleton.CurrentAnimin.Fitness / 100.0f), UIGlobalVariablesScript.Singleton.FitnessControlBarRef.transform.localPosition.y, 0);
        //UIGlobalVariablesScript.Singleton.EvolutionControlBarRef.GetComponent<UISlider>().value = Evolution / 100.0f;
		*/
        Debug.Log("ProfilesManagementScript.Singleton : [" + ProfilesManagementScript.Instance + "];");
        Debug.Log("ProfilesManagementScript.Singleton.CurrentAnimin : [" + ProfilesManagementScript.Instance.CurrentAnimin + "];");
        Debug.Log("ProfilesManagementScript.Singleton.CurrentAnimin.Happy : [" + ProfilesManagementScript.Instance.CurrentAnimin.Happy + "];");
        switch(mSlider)
		{
		case Slider.Happiness:
			leftResult = Mathf.Lerp(min, max, ProfilesManagementScript.Instance.CurrentAnimin.Happy / PersistentData.MaxHappy);
			rightResult = Mathf.Lerp(max, min, ProfilesManagementScript.Instance.CurrentAnimin.Happy / PersistentData.MaxHappy);
			break;
		case Slider.Health:
			leftResult = Mathf.Lerp(min, max, ProfilesManagementScript.Instance.CurrentAnimin.Health / PersistentData.MaxHealth);
			rightResult = Mathf.Lerp(max, min, ProfilesManagementScript.Instance.CurrentAnimin.Health / PersistentData.MaxHealth);
			break;
		case Slider.Hunger:
			leftResult = Mathf.Lerp(min, max, ProfilesManagementScript.Instance.CurrentAnimin.Hungry / PersistentData.MaxHungry);
			rightResult = Mathf.Lerp(max, min, ProfilesManagementScript.Instance.CurrentAnimin.Hungry / PersistentData.MaxHungry);
			break;
		case Slider.Fitness:
			
			leftResult = Mathf.Lerp(min, max, ProfilesManagementScript.Instance.CurrentAnimin.Fitness / PersistentData.MaxFitness);
			rightResult = Mathf.Lerp(max, min, ProfilesManagementScript.Instance.CurrentAnimin.Fitness / PersistentData.MaxFitness);
			break;
		default:
			break;
		}

		//mImage.anchorMin = new Vector2(leftResult, mImage.anchorMin.y);
		//mImage.anchorMax = new Vector2(rightResult, mImage.anchorMax.y);
        mImage.transform.localPosition = new Vector3(leftResult, mImage.transform.localPosition.y, 0);

	}
}
