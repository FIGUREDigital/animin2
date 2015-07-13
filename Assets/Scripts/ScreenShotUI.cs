using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Phi;

public class ScreenShotUI : MonoBehaviour {

	public Image image;
	public RectTransform panel;
	public RectTransform aspect;
	public RectTransform shareRect;
	public void TakeScreenShot()
	{
		Phi.ScreenShot.TakeScreenshot (OnTaken);
	}

	void OnTaken(Sprite sprite)
	{
		image.sprite = sprite;
		Vector3 extents = sprite.bounds.extents;
		float a = extents.y / extents.x;
		//aspect.
		Vector2 size = panel.sizeDelta;
		size.x = ((panel.sizeDelta.y + aspect.sizeDelta.y) * extents.x / extents.y) - aspect.sizeDelta.x;
		panel.sizeDelta = size;
		UiPages.Next(Pages.Screenshot);
	}

	public void OnShare()
	{
		Canvas canvas = shareRect.GetComponentInParent<Canvas> ();
		Rect r = RectTransformUtility.PixelAdjustRect (shareRect, canvas);
		Debug.Log ("Rect = " + r);
		Phi.PhiShare.Image (image.sprite.texture, r);
	}
}
