using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

class MyEditorScript {
	static string[] SCENES = FindEnabledEditorScenes();
	
	static string APP_NAME = "Animin";
	static string TARGET_DIR = "target";
	static string IOS = "_iOS";
	static string ANDROID = "_Android.apk";

	[MenuItem ("Custom/Build iOS/Release")]
	static void PerformiOSBuild()
	{
		string target_dir = APP_NAME + IOS;
		string target = TARGET_DIR + "/" + target_dir;
		if(Directory.Exists(target))
		{
			GenericBuild(SCENES, target, BuildTarget.iPhone, BuildOptions.AcceptExternalModificationsToPlayer);
		}
		else
		{
			GenericBuild(SCENES, target, BuildTarget.iPhone, BuildOptions.None);
		}
		CorrectBundleID ();
	}

	[MenuItem ("Custom/Build iOS/Dev")]
	static void PerformiOSBuildDev()
	{
		string target_dir = APP_NAME + IOS;
		string target = TARGET_DIR + "/" + target_dir;
		if(Directory.Exists(target))
		{
			GenericBuild (SCENES, target, BuildTarget.iPhone, BuildOptions.Development | BuildOptions.AcceptExternalModificationsToPlayer);
		}
		else
		{
			GenericBuild (SCENES, target, BuildTarget.iPhone, BuildOptions.Development);
		}
		CorrectBundleID ();	
	}

	[MenuItem ("Custom/Build iOS/Clean")]
	static void CleaniOSBuild()
	{
		string target_dir = APP_NAME + IOS;
		string target = TARGET_DIR + "/" + target_dir;
		if(Directory.Exists(target))
		{
			Directory.Delete(target);
		}
	}

	[MenuItem ("Custom/Build Android/Release")]
	static void PerformAndroidBuild()
	{
		string target_dir = APP_NAME + ANDROID;
		string target = TARGET_DIR + "/" + target_dir;
		if(File.Exists(target))
		{
			File.Delete(target);
		}
		GenericBuild(SCENES, TARGET_DIR + "/" + target_dir, BuildTarget.Android, BuildOptions.None);
	}
	
	[MenuItem ("Custom/Build Android/Dev")]
	static void PerformAndroidBuildDev()
	{
		string target_dir = APP_NAME + ANDROID;
		string target = TARGET_DIR + "/" + target_dir;
		if(File.Exists(target))
		{
			File.Delete(target);
		}
		GenericBuild (SCENES, TARGET_DIR + "/" + target_dir, BuildTarget.Android, BuildOptions.Development);
	}
	[MenuItem ("Custom/Build Android/Clean")]
	static void CleanAndroidBuild()
	{
		string target_dir = APP_NAME + ANDROID;
		string target = TARGET_DIR + "/" + target_dir;
		if(File.Exists(target))
		{
			File.Delete(target);
		}
	}
	
	private static string[] FindEnabledEditorScenes() {
		List<string> EditorScenes = new List<string>();
		foreach(EditorBuildSettingsScene scene in EditorBuildSettings.scenes) {
			if (!scene.enabled) continue;
			EditorScenes.Add(scene.path);
		}
		return EditorScenes.ToArray();
	}
	
	static void GenericBuild(string[] scenes, string target_dir, BuildTarget build_target, BuildOptions build_options)
	{
		if(!Directory.Exists(target_dir))
		{
			Debug.Log("Creating folder: " + target_dir);
			Directory.CreateDirectory(target_dir);
		}
		EditorUserBuildSettings.SwitchActiveBuildTarget(build_target);
		string res = BuildPipeline.BuildPlayer(scenes,target_dir,build_target,build_options);
		if (res.Length > 0) {
			throw new Exception("BuildPlayer failure: " + res);
		}
		Debug.Log("BUILD SUCCEEDED!");
		Debug.Log("Output: " + target_dir);
	}


	static void CorrectBundleID()
	{
		string file = TARGET_DIR + "/" + APP_NAME + IOS + "/Info.plist";
		string text = File.ReadAllText(file);
		text = text.Replace(@"com.figuredigital.${PRODUCT_NAME}", @"com.do.dog");
		File.WriteAllText(file, text);

	}
}