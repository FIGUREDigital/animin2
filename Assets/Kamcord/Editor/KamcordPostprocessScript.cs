
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

using System;
using System.Diagnostics;
using System.IO;

public class KamcordPostprocessScript : MonoBehaviour
{
    // Replaces PostprocessBuildPlayer functionality
    [PostProcessBuild(10000)]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuildProject)
    {
		UnityEngine.Debug.Log ("Kamcord post build");
#if (UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6)
        if (target == BuildTarget.iPhone)
#else
		if (target == BuildTarget.iOS)
#endif
        {
            OnPostprocessBuildForIOS(pathToBuildProject);
        }

#if UNITY_ANDROID && ((!(UNITY_2_6 || UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_1)))
        else if ( target == BuildTarget.Android )
        {
            OnPostprocessBuildForAndroid(pathToBuildProject);
        }
#endif

        else
        {
            Console.WriteLine ("--- Kamcord --- no post build process for non-mobile.");
        }
    }

    private static string GetKamcordUnityVersion()
    {
        string kamcordUnityVersion = "";
#if UNITY_3_5
        kamcordUnityVersion = "350";
#elif (UNITY_4_0 || UNITY_4_0_1)
        kamcordUnityVersion = "400";
#elif UNITY_4_1
        kamcordUnityVersion = "410";
#elif UNITY_4_2
        kamcordUnityVersion = "420";
#elif UNITY_4_3
        kamcordUnityVersion = "430";
#elif UNITY_4_5
        kamcordUnityVersion = "450";
#elif UNITY_4_6
        kamcordUnityVersion = "460";
#elif UNITY_5_0
        kamcordUnityVersion = "500";
#endif // Unity version check
        return kamcordUnityVersion;
    }

    private static void OnPostprocessBuildForIOS(string pathToBuildProject)
    {
        Console.WriteLine ("--- Kamcord --- Executing post process build phase for iOS.");

        Process p = new Process();
        p.StartInfo.FileName = "perl";

        p.StartInfo.Arguments = string.Format("Assets/Kamcord/Editor/KamcordPostprocessbuildPlayer1 \"{0}\" \"{1}\"", pathToBuildProject, GetKamcordUnityVersion());

        p.StartInfo.UseShellExecute = false;
        p.StartInfo.RedirectStandardOutput = true;
        p.OutputDataReceived += (sender, args) => Console.WriteLine("--- Kamcord output: {0}", args.Data);
        p.Start();
        p.BeginOutputReadLine();

        p.WaitForExit();

        Console.WriteLine ("--- Kamcord --- Done with iOS post-build.");
    }


#if UNITY_ANDROID && ((!(UNITY_2_6 || UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_1)))
    private static void OnPostprocessBuildForAndroid(string pathToBuildProject)
    {
        Console.WriteLine ("--- Kamcord --- Executing post-build process for Android.");

        string lib_in_assets = Path.Combine(pathToBuildProject, Path.Combine(PlayerSettings.productName, Path.Combine("assets", Path.Combine("libs", Path.Combine("armeabi-v7a", "libunity.so")))));
        string lib_in_libs = Path.Combine(pathToBuildProject, Path.Combine(PlayerSettings.productName, Path.Combine("libs", Path.Combine("armeabi-v7a", "libunity.so"))));

        if( File.Exists(lib_in_assets) )
        {
            try
            {
                if( File.Exists(lib_in_libs) )
                {
                    File.Delete(lib_in_libs);
                }

                File.Copy(lib_in_assets, lib_in_libs); // Needs to be in both locations.
            }
            catch( Exception e )
            {
                Console.WriteLine (e.ToString());
            }
        }

        Console.WriteLine ("--- Kamcord --- Done with Android post-build.");
    }
#endif

}
