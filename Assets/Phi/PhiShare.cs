using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.IO;

namespace Phi
{
	public class PhiShare
	{		
		[DllImport ("__Internal")]
		private static extern void ShareImageI(string image64, float x, float y, float w, float h);

		static public void Image(Texture2D texture, Rect from) 
		{
			#if (UNITY_IPHONE && !UNITY_EDITOR)			
			float x = (from.xMin / Screen.width) + 0.5f;
			float y = (from.yMin / Screen.height) +0.5f;
			float w = from.size.x / Screen.width;
			float h = from.size.y / Screen.height;
			byte[] imageBytes = texture.EncodeToPNG();
			string image64 = System.Convert.ToBase64String (imageBytes);
			ShareImageI(image64, x, y, w, h);
			#endif
#if UNITY_ANDROID

            byte[] bytes = texture.EncodeToPNG();
            string path = Application.persistentDataPath + "/ScreenShot.png";
            File.WriteAllBytes(path, bytes);
            AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
            AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
            intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
            intentObject.Call<AndroidJavaObject>("setType", "image/*");
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), "Animin");
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TITLE"), "Animin");
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), "Animin Screenshot");

            AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
            AndroidJavaClass fileClass = new AndroidJavaClass("java.io.File");

            AndroidJavaObject fileObject = new AndroidJavaObject("java.io.File", path);// Set Image Path Here

            AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("fromFile", fileObject);

            //			string uriPath =  uriObject.Call<string>("getPath");
            bool fileExist = fileObject.Call<bool>("exists");
            Debug.Log("File exist : " + fileExist);
            if (fileExist)
                intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject);

            AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
            currentActivity.Call("startActivity", intentObject);
#endif
		}

	}
}
