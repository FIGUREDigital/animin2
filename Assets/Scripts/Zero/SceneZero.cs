using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class SceneZero : MonoBehaviour
{
	private static bool LoadedOBB = false;
	public Text errormess;

	void Start ()
	{
		
		Application.targetFrameRate = 60;
		UITextTextMeshPro.Init();
		UITextTextMeshProNG.Init();
        Debug.Log ("SceneZero");

		#if UNITY_ANDROID
            if(!Application.isEditor)
            {
		        GetOBB ();
		        StartCoroutine(ExtractObbDatasets());
            } 
            else
                Application.LoadLevel (Application.loadedLevel+1);
		#else
            Application.LoadLevel (Application.loadedLevel+1);
		#endif
	}

	#if UNITY_ANDROID
	private void GetOBB ()
	{
		OBBDebug("GetOBB Called");
		if (Application.isEditor)
			return;
		if (!GooglePlayDownloader.RunningOnAndroid ()) {
			OBBDebug("Use GooglePlayDownloader only on Android device!");
			return;
		}

		string expPath = GooglePlayDownloader.GetExpansionFilePath ();
		if (expPath == null) {
			OBBDebug("External storage is not available!");
		} else {
			string mainPath = GooglePlayDownloader.GetMainOBBPath (expPath);
			string patchPath = GooglePlayDownloader.GetPatchOBBPath (expPath);

			OBBDebug("Main = ..." + (mainPath == null ? " NOT AVAILABLE" : mainPath.Substring (expPath.Length)));
			OBBDebug("Patch = ..." + (patchPath == null ? " NOT AVAILABLE" : patchPath.Substring (expPath.Length)));

			//if (mainPath == null || patchPath == null)
			if (mainPath == null )
				if (!LoadedOBB) {
					GooglePlayDownloader.FetchOBB ();
					LoadedOBB = true;
				}
		}
	}


	private IEnumerator ExtractObbDatasets () {
		OBBDebug("ExtractObbDatasets Called");
		string[] filesInOBB = {"Animin_release1.dat", "Animin_release1.xml"};
		foreach (var filename in filesInOBB) {
			string uri = Application.streamingAssetsPath + "/QCAR/" + filename;

			string outputFilePath = Application.persistentDataPath + "/QCAR/" + filename;
			if(!Directory.Exists(Path.GetDirectoryName(outputFilePath)))
				Directory.CreateDirectory(Path.GetDirectoryName(outputFilePath));

			var www = new WWW(uri);
			yield return www;

			Save(www, outputFilePath);
			yield return new WaitForEndOfFrame();
		}

		// When done extracting the datasets, Start Vuforia AR scene
        Application.LoadLevel(Application.loadedLevel+1);
	} 
	private void Save(WWW www, string outputPath) {
		File.WriteAllBytes(outputPath, www.bytes);


		// Verify that the File has been actually stored
		if(File.Exists(outputPath))
			OBBDebug("File successfully saved at: " + outputPath);
		else
			OBBDebug("Failure!! - File does not exist at: " + outputPath);
	}

	private void OBBDebug(string s){
		Debug.Log (s);
		if (errormess != null)
			errormess.text += ("\n" + s);
	}
		#endif
}
