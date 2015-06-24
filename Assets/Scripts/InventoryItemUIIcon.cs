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
	private Inventory.Entry item;
	private ItemDefinition itemDef;
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
		icon.enabled = show;
		GetComponent<UIImagePro> ().enabled = show;
	}

}
