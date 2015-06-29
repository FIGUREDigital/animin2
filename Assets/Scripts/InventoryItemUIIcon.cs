using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
public class InventoryItemUIIcon : MonoBehaviour {

	static UnityEngine.Gradient noGradient;
	
	static GradientColorKey [] cKeys = {new GradientColorKey(new Color (1, 1, 1), 0)};
	static GradientAlphaKey [] aKeys = {new GradientAlphaKey(0.6f, 0)};

	static InventoryItemUIIcon()
	{
		noGradient = new UnityEngine.Gradient();
		noGradient.colorKeys = cKeys;
		noGradient.alphaKeys = aKeys;
	}

	[SerializeField]
	public Image icon;
	public GameObject noIcon;
	public UIGradientPro gradient;
	public GameObject modelParent;
	public PopupItemType[] acceptedItemTypes = new PopupItemType[0];	// Null or 0 items = all types
	private Inventory.Entry item;
	private ItemDefinition itemDef;
	private GameObject model;
	private bool showHide = true;
	virtual public Inventory.Entry Item
	{
		set
		{
			item = value;
			ItemDef = item != null ? item.Definition : null;

			ItemLink il = GetComponent<ItemLink>();
			if (il != null)
			{
                il.item = value;
            }
			
			if (model != null && model.transform.parent.gameObject == modelParent)
			{
				model.transform.parent = null;
				model.SetActive(false);
				model = null;
			}
			if(modelParent != null && itemDef != null && itemDef.SpriteName == null)
			{
				GameObject go = item.Instance;
				model = item.Instance;
				model.SetActive (true);
				modelParent.SetActive(showHide);
				model.transform.parent = modelParent.transform;
				model.transform.localPosition = Vector3.zero;
				model.transform.localScale = Vector3.one;
				model.transform.localRotation = Quaternion.identity;
			}
        }
        get
		{
			return item;
		}
	}

	
	public ItemDefinition ItemDef
	{
		set
		{
			if(modelParent != null)
			{
				modelParent.SetActive(false);
			}
			itemDef = value;
			icon.gameObject.SetActive (itemDef != null);
			if (itemDef == null)
			{
				gradient.grad = noGradient;
			}
			else
			{				
				icon.sprite = itemDef.SpriteName;
				if(noIcon != null)
				{
					noIcon.SetActive (icon.sprite == null);				
				}
				icon.gameObject.SetActive(icon.sprite != null);
				if(gradient != null)
				{
					gradient.grad = itemDef.Gradient;
					gradient.GetComponent<Graphic>().SetVerticesDirty();
				}
			}
		}
		get
		{
			return itemDef;
		}
	}

	public void ShowHide(bool show)
	{		
		showHide = show;
		icon.enabled = show;
		GetComponent<UIImagePro> ().enabled = show;
		if (modelParent != null) {
			modelParent.SetActive (show && (model != null));
		}
	}

}
