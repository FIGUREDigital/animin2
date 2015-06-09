using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EmoticonBehaviour : MonoBehaviour 
{
	public Image face;
	public Graphic arrow;

	public Sprite unHappySprite;
	public Sprite sadSprite;
	public Sprite tongueSprite;
	public Sprite winkSprite;
	public Sprite happySprite;
	
	public Color happyColor;
	public Color sadColor;
	
	public Vector3 sadScale;
	public Vector3 happyScale;
	public bool updateEachFrame = true;

	
	// Update is called once per frame
	void Update()
	{
		if(updateEachFrame)
		{
			UpdateIcons();
		}
	}

	public void UpdateAlpha(float a)
	{
		happyColor.a = a;
		sadColor.a = a;
		Color c = face.color;
		c.a = a;
		face.color = c;
		c = arrow.color;
		c.a = a;
		arrow.color = c;
	}

	public void UpdateIcons () 
	{
		bool happy = true;
		Sprite s;
		if (PersistentData.happy < 20)
		{
			s = unHappySprite;
			happy = false;
		}		
		else if (PersistentData.happy < 40)
		{
			s = sadSprite;
			happy = false;
		}
		else if (PersistentData.happy < 60)
		{
			s = tongueSprite;
		}
		else if (PersistentData.happy < 80)
		{
			s = winkSprite;
		}
		else
		{
			s = happySprite;
		}
		if(s != face.sprite)
		{
			face.sprite = s;
			arrow.transform.localScale = happy ? happyScale : sadScale;
			arrow.color = happy? happyColor : sadColor;
		}
	}
}
