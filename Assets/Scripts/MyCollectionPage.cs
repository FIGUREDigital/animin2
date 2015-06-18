using UnityEngine;
using System.Collections;
using Phi;

public class MyCollectionPage : MonoBehaviour 
{
	UIImagePro itemImage;
	GameObject unknown;
	GameObject locked;
	UIPages pages;
	int curIndex = -1;
	/*
	public void Init()
	{
		if (helmets == null || helmets.Length == 0)
		{
			helmets = GetComponentsInChildren<FEHelmet>();
		}
	}
	
	void Start()
	{
		Init();
		if (curIndex >= 0)
		{
			UpdateVisuals();
		}
	}
	
	void OnSetUpPage(UIPages.SetupPage setupPage)
	{
		pages = setupPage.pages;
		fehelmets = pages.userData as FEHelmets;
		if (!fehelmets) return;
		
		curIndex = setupPage.page;
		UpdateVisuals();
	}
	
	void UpdateVisuals()
	{
		if (helmets == null) return;
		int offset = curIndex * NumHelmetsPerPage;
		for (int i = 0; i < helmets.Length; i++ )
		{
			helmet = fehelmets.GetHelmet(offset + i);
			helmets[i].Setup(helmet, fehelmets);
		}
	}
*/
}
