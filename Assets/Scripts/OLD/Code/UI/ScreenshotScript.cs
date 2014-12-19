using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class ScreenshotScript : MonoBehaviour {

	[SerializeField]
	private Sprite PhotoSaved;

	// Use this for initialization
	void OnClick()
	{
		if(Application.isEditor)
		{
			Application.CaptureScreenshot("screenshot.png");
		}
		else
		{

			string screenshotName = "screenshot"  + DateTime.Now.ToString("s") + ".png";
			Debug.Log("Saving photo to: " + screenshotName);
            string path = Application.persistentDataPath + screenshotName;
#if UNITY_IOS
			StartCoroutine( EtceteraBinding.takeScreenShot( screenshotName, imagePath =>
			{EtceteraBinding.saveImageToPhotoAlbum (imagePath);}) );
            iPhone.SetNoBackupFlag(path);
#elif UNITY_ANDROID
			
			Application.CaptureScreenshot(screenshotName);
			Debug.Log("Moving file from " + path);
			bool saved = EtceteraAndroid.saveImageToGallery(path,screenshotName);
			if(saved)
			{
				Debug.Log("File moved");
			}
			else
			{
				Debug.Log("File moved fail!");
			}
#endif
		}
		Invoke("PopPhotoSaved",0.3f);
	}

	void PopPhotoSaved()
	{
//		if (PhotoSaved != null && PhotoSaved.GetComponent<PhotoFadeOut> () != null) {
//			PhotoSaved.gameObject.SetActive(true);
//		}
	}

	IEnumerator CaptureScreenshot()
	{
		Texture2D screenshot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, true);
		yield return new WaitForEndOfFrame();
		screenshot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, true);
		screenshot.Apply();
		var bytes = screenshot.EncodeToPNG();
		Destroy (screenshot);
		// For testing purposes, also write to a file in the project folder
		//			File.WriteAllBytes(Application.dataPath + "screenshot.png", bytes);
		string path = Application.persistentDataPath + "/screenshot" + DateTime.Now.ToString("s") + ".png";
		Debug.Log("Photo saved to: " + path);
		File.WriteAllBytes(path, bytes);
	}
	
}
