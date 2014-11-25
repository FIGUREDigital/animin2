using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SceneZero : MonoBehaviour
{

		private static bool LoadedOBB = false;
		private Text errormess;

		void Start ()
		{
				GameObject go = GameObject.Find ("error");
				if (go != null) {
						errormess = go.GetComponent<Text> ();
				}
				GetOBB ();
				Debug.Log ("SceneZero");
				Application.LoadLevel ("TestMenu");
		}

		private void GetOBB ()
		{
				#if UNITY_ANDROID
				if (Application.isEditor)
						return;
				if (!GooglePlayDownloader.RunningOnAndroid ()) {
						errormess.text += ("\nUse GooglePlayDownloader only on Android device!");
						return;
				}

				string expPath = GooglePlayDownloader.GetExpansionFilePath ();
				if (expPath == null) {
						errormess.text += ("\nExternal storage is not available!");
				} else {
						string mainPath = GooglePlayDownloader.GetMainOBBPath (expPath);
						string patchPath = GooglePlayDownloader.GetPatchOBBPath (expPath);

						errormess.text += ("\nMain = ..." + (mainPath == null ? " NOT AVAILABLE" : mainPath.Substring (expPath.Length)));
						errormess.text += ("\nPatch = ..." + (patchPath == null ? " NOT AVAILABLE" : patchPath.Substring (expPath.Length)));
						if (mainPath == null || patchPath == null)
						if (!LoadedOBB) {
								GooglePlayDownloader.FetchOBB ();
								LoadedOBB = true;
						}
				}
				#endif
		}
}
