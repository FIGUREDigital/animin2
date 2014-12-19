#pragma warning disable 0168 // variable declared but not used.
#pragma warning disable 0219 // variable assigned but not used.

using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Runtime.InteropServices;

public class ScreenshotManager : MonoBehaviour {
	
	public static event Action<string> ScreenshotFinishedSaving;
	public static event Action<string> ImageFinishedSaving;
	
	public static ScreenshotManager instance;
	
	#if UNITY_IPHONE
	
	[DllImport("__Internal")]
    private static extern bool saveToGallery( string path );
	
	#endif
	
	
	public static IEnumerator Save(string fileName, string albumName = "MyScreenshots", bool callback = false)
	{
		GameObject go = new GameObject();
		go.name = "Screenshot";
		ScreenshotManager instance = go.AddComponent<ScreenshotManager>();
		
		bool photoSaved = false;
		
		string date = System.DateTime.Now.ToString("dd-MM-yy");
		
		ScreenshotManager.ScreenShotNumber++;
		
		string screenshotFilename = fileName + "_" + ScreenshotManager.ScreenShotNumber + "_" + date + ".png";
		
		Debug.Log("Save screenshot " + screenshotFilename); 
		
		string path = Application.persistentDataPath + "/" + screenshotFilename;;

		#if UNITY_IPHONE
		
		if(Application.platform == RuntimePlatform.IPhonePlayer) 
		{
			Application.CaptureScreenshot(screenshotFilename);
			
			while(!photoSaved) 
			{
				photoSaved = saveToGallery(path);
				
				yield return instance.StartCoroutine(ScreenshotManager.Wait(.5f));
			}
		
			iPhone.SetNoBackupFlag(path);
		} 
		else 
		{
			Application.CaptureScreenshot(screenshotFilename);
		}


		#elif UNITY_ANDROID	
				
		if(Application.platform == RuntimePlatform.Android) 
		{
			string androidPath = Path.Combine(albumName, screenshotFilename);
			path = Path.Combine(Application.persistentDataPath, androidPath);
			string pathonly = Path.GetDirectoryName(path);
			Directory.CreateDirectory(pathonly);
			Application.CaptureScreenshot(androidPath);

			AndroidJavaClass obj = new AndroidJavaClass("com.ryanwebb.androidscreenshot.MainActivity");
			
			while(!photoSaved) 
			{
				photoSaved = obj.CallStatic<bool>("addImageToGallery", path);
			
				yield return instance.StartCoroutine(ScreenshotManager.Wait(.5f));
			}
		} 
		else 
		{
			Application.CaptureScreenshot(screenshotFilename);
		}


		#elif UNITY_WP8

		if(Application.platform == RuntimePlatform.WP8Player)
		{
			Application.CaptureScreenshot(path);

			while(!UnityEngine.Windows.File.Exists(path))
			{
				yield return instance.StartCoroutine(ScreenshotManager.Wait(.2f));
			}
			
			byte[] bytes = UnityEngine.Windows.File.ReadAllBytes(path);

			WP8Screenshot.Main.SaveImage(bytes, screenshotFilename + ".png");
		}
		else 
		{
			Application.CaptureScreenshot(screenshotFilename);
		}


		#else
			
		while(!photoSaved) 
		{
			yield return null;
	
			Debug.Log("Screenshots only available in iOS/Android/WP8 mode!");
			
			path = "";
		
			photoSaved = true;
		}
		
		#endif
		
		if(callback) ScreenshotFinishedSaving(path);

		Destroy(go);
	}
	
	
	public static IEnumerator SaveExisting(byte[] bytes, string fileName, bool callback = false)
	{
		GameObject go = new GameObject();
		go.name = "Screenshot";
		ScreenshotManager instance = go.AddComponent<ScreenshotManager>();

		bool photoSaved = false;
		
		Debug.Log("Save existing file to gallery " + fileName);
		
		string path = Application.persistentDataPath + "/" + fileName + ".png";
		
		
		#if UNITY_IPHONE
		
			if(Application.platform == RuntimePlatform.IPhonePlayer) 
			{
				System.IO.File.WriteAllBytes(path, bytes);
			
				while(!photoSaved) 
				{
					photoSaved = saveToGallery(path);
					
					yield return instance.StartCoroutine(ScreenshotManager.Wait(.5f));
				}
			
				iPhone.SetNoBackupFlag(path);
			}


		#elif UNITY_ANDROID	
				
			if(Application.platform == RuntimePlatform.Android) 
			{
				AndroidJavaClass obj = new AndroidJavaClass("com.ryanwebb.androidscreenshot.MainActivity");
			
				System.IO.File.WriteAllBytes(path, bytes);
					
				while(!photoSaved) 
				{
					photoSaved = obj.CallStatic<bool>("addImageToGallery", path);
							
					yield return instance.StartCoroutine(ScreenshotManager.Wait(.5f));
				}
			}
		
		
		#elif UNITY_WP8
		
			if(Application.platform == RuntimePlatform.WP8Player)
			{
				WP8Screenshot.Main.SaveImage(bytes, fileName);

				yield return null;
			}
		
		
		#else
			
			while(!photoSaved) 
			{
				yield return null;
		
				Debug.Log("Save existing file only available in iOS/Android/WP8 mode!");
			
				path = "";
			
				photoSaved = true;
			}
		
		#endif
		
		if(callback) ImageFinishedSaving(path);

		Destroy(go);
	}
	
	
	public static IEnumerator Wait(float delay)
	{
		float pauseTarget = Time.realtimeSinceStartup + delay;
		
		while(Time.realtimeSinceStartup < pauseTarget)
		{
			yield return null;	
		}
	}
	
	
	public static int ScreenShotNumber 
	{
		set { PlayerPrefs.SetInt("screenShotNumber", value); }
	
		get { return PlayerPrefs.GetInt("screenShotNumber"); }
	}
}