using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro; 

public class ProfilePanel : MonoBehaviour 
{
	//private const string PROFILE_PREFAB = "Prefabs/UI/User Profile Button";
	private GameObject[] mButtons;
	private List<PlayerProfileData> mListOfPlayerProfiles;
	private PlayerProfileData mCurrentProfile;
	[SerializeField]
	private GameObject m_ProfileButton;
	private Text profileText;

	public float profileSpacing = 600.0f;

	void Start()
	{
		Init ();
	}

	void Init()
	{
		/*if(m_ProfileButton == null)
		{
			m_ProfileButton = Resources.Load (PROFILE_PREFAB);
		}*/
		Populate ();
	}

	void OnEnable()
	{
		Init ();
	}
	void OnDisable()
	{
		Depopulate ();
	}

	void Depopulate()
	{
		if(mButtons != null)
		{
			for(int i = 1; i < mButtons.Length; i++)
			{
				Destroy(mButtons[i]);
			}
		}
	}
	void Populate () 
	{
		Depopulate ();
		List<PlayerProfileData> profiles = ProfilesManagementScript.Instance.ProfileList;
		int numUsers = profiles.Count + 1;
		mButtons = new GameObject[numUsers];
		float origin = (profileSpacing * (numUsers - 1))/2.0f;
		for(int i = 0; i<numUsers; i++)
		{
			GameObject newProfile = m_ProfileButton;
			if (i > 0)
			{
				newProfile = (GameObject)Instantiate(m_ProfileButton, Vector3.zero, Quaternion.identity);
			}
			newProfile.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;

			RectTransform rt = newProfile.GetComponent<RectTransform>();
			//rt.anchorMin = new Vector2(1,0.5f);
			//rt.anchorMax = new Vector2(1,0.5f);
			rt.anchoredPosition = new Vector2(origin - (i * profileSpacing), -80);
			rt.SetParent(transform,false);

			if(i != 0)
			{
				int profileId = i-1;
				newProfile.GetComponent<EnterProfile>().ThisProfile = profiles[profileId];
				newProfile.GetComponentInChildren<TextMeshProUGUI>().text = profiles[profileId].ProfileName;
				int childId = 1;
				if( i < 5)
				{
					childId = i;

				}
				newProfile.transform.GetChild(childId).gameObject.SetActive(true);
			}

			mButtons[i] = newProfile;
		}



		RectTransform rectTransform = GetComponent<RectTransform>();
		rectTransform.sizeDelta = new Vector2 (profileSpacing * numUsers, 338);

		Vector2 pos = rectTransform.anchoredPosition;
		pos.x = 0;
		if (numUsers > 3)
		{
			pos.x = (numUsers-3)*profileSpacing*0.5f;
		}
		rectTransform.anchoredPosition = pos;
	}

}
