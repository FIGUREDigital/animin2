#if !UNITY_CLOUD_BUILD
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;
namespace Phi
{
    [ExecuteInEditMode]
    // This class spots when files are changed on disk outside of unity, eg via source control
    // and prepares the scene for reloading of the main scene and asks the user if
    // they wish to reload any secondary scenes.
    public class EdMultisceneOutsideChange : UnityEditor.AssetPostprocessor 
    {
        public static List<Multiscene> delayedReimported = new List<Multiscene>();
        public static List<string> ignoreReimport = new List<string>();
        static void OnPostprocessAllAssets(string[] reimported, string[] deleted, string[] moved, string[] moveAssetPaths)
        {
            // This gets triggered during saves and we do not want to start loading new scenes due to them
            // so check we are not saving
            if (Multiscene.GetSaveTriggeredByUs())
            {
                return;
            }
            if(moved != null && moved.Length > 0)
            {
                foreach (string s in moved)
                {
                    ignoreReimport.Add(s);
                } 
                MultisceneAssemblyReload.Instance.CheckAllNames();
            }
            if (deleted != null && deleted.Length > 0)
            {
                MultisceneAssemblyReload.Instance.CheckForDeleted();
            }

            // Copy the list of reimported paths to a list to be processed later
            // it's not safe in this callback to load scenes.
            delayedReimported.Clear();

            // Setup scene ready for reloading the main scene if it's going to be reloaded.
            foreach (string s in reimported)
            {
                if (ignoreReimport.Contains(s))
                {
                    ignoreReimport.Remove(s);
                }
                else
                {
                    if (s.EndsWith(".unity"))
                    {
                        Multiscene scene = Multiscene.GetIfExists(s);
                        // Ignore current scene as unity handles that already.
                        if (scene != null)
                        {
                            if (scene.isCurrentScene)
                            {
                                Multiscene.PrepareForSceneLoad();
                            }
                            else
                            {
                                delayedReimported.Add(scene);
                            }
                        }
                    }
                }
            }

            // Ask user if they want to load any other scenes later..
            if (delayedReimported.Count > 0)
            {
                EditorApplication.delayCall += ProcessList;
            }
        }

        static void ProcessList()
        {
            foreach (Multiscene scene in delayedReimported)
            {
                if(EditorUtility.DisplayDialog("The open scene has been modified externally", "The open scene '" + scene.Path + "' has been changed on disk.\nDo you want to reload the scene?", scene.IsDirty ? "Reload (Lose local changes)" : "Reload", "Ignore"))
                {
                    // Reload scene
                    scene.Reload();
                }
            }
        }
	}
}
#endif
