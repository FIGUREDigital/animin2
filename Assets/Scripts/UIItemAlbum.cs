using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIItemAlbum : MonoBehaviour {

	public UIText curNo;
	public UIText curName;
	public UIPages pages;
	public UIText weHaveText;
	public int curPage = -1;
	[System.NonSerialized]
	public List<ItemDefinition> definitions;

	void OnEnable()
	{
		pages.onSetScrollPos += SetScrollPos;
		definitions = ItemDefinition.GetAllDefinitions ();
		int weHave = 0;
		List<ItemDefinition> filtered = new List<ItemDefinition> ();
		for (int i = 0; i < definitions.Count; i++) 
		{
			if(definitions[i].ItemType == PopupItemType.Item)
			{
				filtered.Add (definitions[i]);
				if (ProfilesManagementScript.Instance.CurrentProfile.Inventory.OwnItem (definitions[i].Id))
				{
					Debug.Log ("We Have "+definitions[i]);
					weHave++;
				}
			}
		}
		definitions = filtered;
		weHaveText.Text = weHave.ToString () + "/" + definitions.Count.ToString ();
		pages.userData = this;
		pages.numPages = definitions.Count;
		pages.SetScrollPos (0.0f, true);
	}
	void OnDisable()
	{
		pages.onSetScrollPos -= SetScrollPos;
	}
	public void SetScrollPos(UIPages pages, float pos)
	{
		UpdatePage(pos);
	}

	public void UpdatePage(float pos)
	{
		int page = Mathf.RoundToInt (pos);
		if (page != curPage) 
		{
			curPage = page;
			ItemDefinition def = (curPage >= 0 && curPage< definitions.Count) ? definitions[page] : null;
			curNo.gameObject.SetActive(def != null);
			curName.gameObject.SetActive(def != null);
			if(def != null)
			{
				curNo.Text = "# "+(curPage+1).ToString();
				curName.Text = def.name;
			}
		}
	}
}
