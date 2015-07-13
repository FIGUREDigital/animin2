using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
namespace Phi
{
	// Whenever this component is enabled it will grab the screen and call onTaken with a sprite
	public class ScreenShot : SingletonMonoBehaviour<ScreenShot>
	{
		Action<Sprite> onTaken;
		public override void Init()
		{
		}

		static public void TakeScreenshot(Action<Sprite> onTakenCB)
		{
			ScreenShot.Instance.onTaken = onTakenCB;
			ScreenShot.Instance.gameObject.SetActive(true);
		}

		void OnEnable () 
		{
			StartCoroutine (TakeScreenShot ());
		}
			
		private IEnumerator TakeScreenShot()
		{
			yield return new WaitForEndOfFrame();
			Texture2D tex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
			tex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
			tex.Apply ();
			onTaken(Sprite.Create(tex,new Rect(0,0,Screen.width, Screen.height),new Vector2(0,0)));
			gameObject.SetActive (false);
		}
	}
}
