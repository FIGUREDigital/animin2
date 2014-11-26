using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class GooglePlayDownloader
{

		#if UNITY_ANDROID
		private static AndroidJavaClass detectAndroidJNI;
		#endif
		public static bool RunningOnAndroid()
		{
				#if UNITY_ANDROID
				if (detectAndroidJNI == null)
				detectAndroidJNI = new AndroidJavaClass("android.os.Build");
				return detectAndroidJNI.GetRawClass() != IntPtr.Zero;
				#else
				return false;
				#endif
		}

		#if UNITY_ANDROID
		private static AndroidJavaClass Environment;
		private const string Environment_MEDIA_MOUNTED = "mounted";

		private const string bundleID = "com.unity3d.player.UnityPlayer";

		static GooglePlayDownloader()
		{
		if (!RunningOnAndroid())
		return;

		Environment = new AndroidJavaClass("android.os.Environment");

		using (AndroidJavaClass dl_service = new AndroidJavaClass("com.unity3d.plugin.downloader.UnityDownloaderService"))
		{
		// stuff for LVL -- MODIFY FOR YOUR APPLICATION!
		dl_service.SetStatic("BASE64_PUBLIC_KEY", "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAqWSuyg1d82CxPSUf3S/qMjxGZ4JsCEMPzfTAbqxVUTS9QSt7bdtJP8EJyWfEr/OlNKWtAk+ZeqS6fzZnwv3Z+/F2Wv1UUIEkI8qFlwaFtljFCpR2AAZnUlK0myym5LNs8yTvmX5shGe3SNGW5tDsj4RLuP9pq+iiJer6ZjbXthubABF6VF6xPH4Dy4MOQOw52JaOJxPaLXpthEWdytlxOcSR1IlzDfOz+Ky7QH1Li0TV9PPlgGXlHLAOwGG5Tyw4iJmjeLuBrNLH0e8ihp2im9gWnrMzVMTMHN8xzfKv+ZiwDaFaP9FY5srMljdnyCtZg9tMjPVf6yAZKIeJGA2eFwIDAQAB");
		// used by the preference obfuscater
		dl_service.SetStatic("SALT", new byte[]{1, 43, 256-12, 256-1, 54, 98, 256-100, 256-12, 43, 2, 256-8, 256-4, 9, 5, 256-106, 256-108, 256-33, 45, 256-1, 84});
		}
		}
		public static string GetExpansionFilePath()
		{
		populateOBBData();

		if (Environment.CallStatic<string>("getExternalStorageState") != Environment_MEDIA_MOUNTED)
		return null;

		const string obbPath = "Android/obb";

		using (AndroidJavaObject externalStorageDirectory = Environment.CallStatic<AndroidJavaObject>("getExternalStorageDirectory"))
		{
		string root = externalStorageDirectory.Call<string>("getPath");
		return String.Format("{0}/{1}/{2}", root, obbPath, obb_package);
		}
		}
		public static string GetMainOBBPath(string expansionFilePath)
		{
		populateOBBData();

		if (expansionFilePath == null)
		return null;
		string main = String.Format("{0}/main.{1}.{2}.obb", expansionFilePath, obb_version, obb_package);
		if (!File.Exists(main))
		return null;
		return main;
		}
		public static string GetPatchOBBPath(string expansionFilePath)
		{
		populateOBBData();

		if (expansionFilePath == null)
		return null;
		string main = String.Format("{0}/patch.{1}.{2}.obb", expansionFilePath, obb_version, obb_package);
		if (!File.Exists(main))
		return null;
		return main;
		}



		public static void FetchOBB()
		{
		using (AndroidJavaClass unity_player = new AndroidJavaClass(bundleID))
		{
		AndroidJavaObject current_activity = unity_player.GetStatic<AndroidJavaObject>("currentActivity");

		AndroidJavaObject intent = new AndroidJavaObject("android.content.Intent",
		current_activity,
		new AndroidJavaClass("com.unity3d.plugin.downloader.UnityDownloaderActivity"));

		int Intent_FLAG_ACTIVITY_NO_ANIMATION = 0x10000;
		intent.Call<AndroidJavaObject>("addFlags", Intent_FLAG_ACTIVITY_NO_ANIMATION);
		intent.Call<AndroidJavaObject>("putExtra", "unityplayer.Activity", 
		current_activity.Call<AndroidJavaObject>("getClass").Call<string>("getName"));
		current_activity.Call("startActivity", intent);

		if (AndroidJNI.ExceptionOccurred() != System.IntPtr.Zero)
		{
		Debug.LogError("Exception occurred while attempting to start DownloaderActivity - is the AndroidManifest.xml incorrect?");
		AndroidJNI.ExceptionDescribe();
		AndroidJNI.ExceptionClear();
		}
		}
		}

		// This code will reuse the package version from the .apk when looking for the .obb
		// Modify as appropriate
		private static string obb_package;
		private static int obb_version = 0;
		private static void populateOBBData()
		{
		if (obb_version != 0)
		return;
		using (AndroidJavaClass unity_player = new AndroidJavaClass(bundleID))
		{
		AndroidJavaObject current_activity = unity_player.GetStatic<AndroidJavaObject>("currentActivity");
		obb_package = current_activity.Call<string>("getPackageName");
		AndroidJavaObject package_info = current_activity.Call<AndroidJavaObject>("getPackageManager").Call<AndroidJavaObject>("getPackageInfo", obb_package, 0);
		obb_version = package_info.Get<int>("versionCode");
		}
		}
		#endif
}
