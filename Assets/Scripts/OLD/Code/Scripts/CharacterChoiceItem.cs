using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class CharacterChoiceItem : MonoBehaviour 
{
	[SerializeField]
	private GameObject mSprite;
	[SerializeField]
	private GameObject mGetNow;
	[SerializeField]
	private GameObject mDisabledSprite;
	[SerializeField]
	private GameObject mMailButton;
	[SerializeField]
	private GameObject mAgeLabel;
	[SerializeField]
	private bool mUnlocked;
	[SerializeField]
    public PersistentData.TypesOfAnimin ThisCharacter;

    public SelectCharacterClickScript CharacterClickScript;

	// Use this for initialization

    void Awake()
    {
        CharacterClickScript = GetComponentInChildren<SelectCharacterClickScript>();
    }

	void Start () 
	{    

		if(mSprite == null)
		{
			Debug.LogWarning("CANNOT FIND SPRITE FOR CHARACTER. ATTEMPTING TO RESOLVE.");
			mSprite = transform.FindChild("Sprite").gameObject;
			if(mSprite == null)
			{
				Debug.LogError("SPRITE NOT SET FOR CHARACTER");
			}
			else
			{
				Debug.Log("Sprite found!");
			}
		}

		if(mDisabledSprite == null)
		{
			Debug.LogWarning("CANNOT FIND DISABLED SPRITE FOR CHARACTER. ATTEMPTING TO RESOLVE.");
			mDisabledSprite = transform.FindChild("Disabled Sprite").gameObject;
			if(mDisabledSprite == null)
			{
				Debug.LogError("DISABLED SPRITE NOT SET FOR CHARACTER");
			}
			else
			{
				Debug.Log("Disabled sprite found!");
			}
		}
	}

	void OnEnable()
	{
		mUnlocked = false;
		for(int i =0; i < ProfilesManagementScript.Instance.CurrentProfile.UnlockedAnimins.Count; i++)
		{
			if(ProfilesManagementScript.Instance.CurrentProfile.UnlockedAnimins[i] == ThisCharacter)
			{
				mUnlocked = true;
			}
		}
        ChangeLockedState(mUnlocked);
		UpdateAge();
		//Invoke ("UpdateAge", 0.1f);
	}

	private void UpdateAge()
	{
		TextMeshProUGUI label = mAgeLabel.GetComponent<TextMeshProUGUI>();
		if(ProfilesManagementScript.Instance.CurrentProfile.Characters[(int)ThisCharacter] != null)
		{
			PersistentData pd = ProfilesManagementScript.Instance.CurrentProfile.Characters[(int)ThisCharacter];
			label.text = "Age " + pd.Age;
		}
		else
		{
			label.text = "";
		}
	}

    public void ChangeLockedState(bool unlocked)
	{
        Debug.Log("Adjusting profile image for... " + ThisCharacter);
        mUnlocked = unlocked;
        mSprite.SetActive(mUnlocked);
		mDisabledSprite.SetActive(!mUnlocked);
		if(mMailButton)
		{
			mMailButton.SetActive (mUnlocked);
			if(ThisCharacter == PersistentData.TypesOfAnimin.Tbo)
			{
				mMailButton.SetActive (false);
			}
		}
		mAgeLabel.SetActive(mUnlocked);
		if (mGetNow) {
			mGetNow.SetActive (!mUnlocked);
		}
	}

	public void GoToAddress()
	{
		UnlockCharacterManager.Instance.ID = ThisCharacter;
		UiPages.Next (Pages.AddressInputPage);
	}
}
