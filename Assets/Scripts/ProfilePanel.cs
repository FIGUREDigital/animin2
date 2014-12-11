using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ProfilePanel : MonoBehaviour 
{
	private const string PROFILE_PREFAB = "Prefabs/UI/User Profile Button";
	private GameObject[] mButtons;
	private List<PlayerProfileData> mListOfPlayerProfiles;
	private PlayerProfileData mCurrentProfile;
	private Object mProfilePrefab;

	void Start()
	{
		Init ();
	}

	void Init()
	{
		if(mProfilePrefab == null)
		{
			mProfilePrefab = Resources.Load (PROFILE_PREFAB);
		}
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
			foreach(GameObject button in mButtons)
			{
				Destroy(button);
			}
		}
	}
	void Populate () 
	{
		Depopulate ();
		List<PlayerProfileData> profiles = SaveAndLoad.Instance.StateData.ProfileList;
		int numUsers = profiles.Count + 1;
		mButtons = new GameObject[numUsers];
		for(int i = 0; i<numUsers; i++)
		{
			GameObject newProfile = (GameObject)Instantiate(mProfilePrefab, Vector3.zero, Quaternion.identity);

			RectTransform rt = newProfile.GetComponent<RectTransform>();
			rt.anchorMin = new Vector2(1,0.5f);
			rt.anchorMax = new Vector2(1,0.5f);
			rt.anchoredPosition = new Vector2(- (427.8f +(i * 240f)), -27.7f);
			rt.SetParent(transform,false);

			if(i != 0)
			{
				int profileId = i-1;
				newProfile.GetComponent<EnterProfile>().ThisProfile = profiles[profileId];
				newProfile.GetComponentInChildren<Text>().text = profiles[profileId].ProfileName;
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
		rectTransform.sizeDelta = new Vector2 (240 * numUsers, 338);

	}

}
