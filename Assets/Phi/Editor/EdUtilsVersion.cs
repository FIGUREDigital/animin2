using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

[InitializeOnLoad]
public class EdUtilsVersion : MonoBehaviour 
{

#if !UNITY_CLOUD_BUILD// && UNITY_EDITOR
    [UnityEditor.Callbacks.PostProcessBuildAttribute(1)]
    public static void OnPostprocessBuild(UnityEditor.BuildTarget target, string pathToBuiltProject)
    {
        if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
        {
            // Create our build manifest file.
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict["buildNumber"] = UnityEditor.PlayerSettings.bundleVersion;
            dict["scmCommitId"] = System.Environment.MachineName;
            dict["buildStartTime"] = System.DateTime.UtcNow.ToString();
            string data = PhiMiniJSON.Serialize(dict);
            string path = Application.dataPath + "/Resources/Version.txt";
            using (StreamWriter writer = new StreamWriter(path, false))
            {
                try
                {
                    writer.WriteLine("{0}", data);
                }
                catch (System.Exception ex)
                {
                    string msg = " threw:\n" + ex.ToString();
                    Debug.LogError(msg);
                    UnityEditor.EditorUtility.DisplayDialog("Error", "Error when trying to write " + path, "OK");
                }
            }
        }
    }
#endif
}
