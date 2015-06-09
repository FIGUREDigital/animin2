#if !UNITY_CLOUD_BUILD
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System;
namespace Phi
{
	[InitializeOnLoad]
    [ExecuteInEditMode]
    public class EdMultisceneWatchForSave : UnityEditor.AssetModificationProcessor
    {
        private static bool saveOtherScenesRequired = false;    // Set during the save of the current scene to trigger saving of other scenes later
        private static bool saveCurrentSceneAgain = false;      // When saved as a new scene we get no callback before the save causing the save to be corrupt!
        private static bool doSaveCurrentSceneAgain = false;    // Delay saving currentScene again as it results in a blank file if done during quit. So we wait one editor update which does not occur during quit.
        public static Action<string> onWillCreate;
        public static Action onWillSave;
        public static bool preparedForSave = false;

        static List<string> savedOrCreated = new List<string>();   // List of paths that have been created or saved, when we have finished saving we check to see if any of our loaded scenes have this path and mark as not dirty.

        static EdMultisceneWatchForSave()
        {
            Multiscene.onSaveTriggeredByUs += PrepareForSave;
        }

        static string[] OnWillSaveAssets(string[] paths)
        {
            //Debug.Log("OnWillSaveAssets " + paths.Length);
            //PrepareForSave();   // We have to do this even if we are not saving an asset because by the time we know we are
            // creating a new asset via OnWillCreateAsset we have already saved the scene.
            // Without this adding a scene and locking it and then saving a new scene saves it with the locked scene intact!
            if(saveOtherScenesRequired)
            {
                SaveOtherScenes();
            }
            foreach(string path in paths)
            {
                if(path.EndsWith(".unity"))
                {
//                    MultisceneAssemblyReload.Instance.SaveOccured(path);
                    if (!Multiscene.GetSaveTriggeredByUs())
                    {
                        Multiscene.SaveTriggeredByUs(true);
                        SaveOtherScenes(); // We can save the other scenes and then save current scene
                    }
                    savedOrCreated.Add(path);
                }
            }
            if (onWillSave != null)
            {
                onWillSave();
            }
            return paths;
        }

        // This function will be called before any saving occurs and is used to ensure everything in the scene is ready for saving.
        // i.e Any locked items are unlocked and hidden if part of a child scene
        // Any cameras are re-enabled.
        // When saving is finished these things are reapplied
        static void PrepareForSave()
        {
            if (preparedForSave) return;
            //Debug.Log("Prepare for save");
            MultisceneCameras.Instance.ReadyForSave();
#if USEDONTSAVEFLAG
            bool foundLocked = false;
            // When doing a save as
            foreach(Multiscene scene in MultisceneAssemblyReload.Instance.loadedScenes.Values)
            {
                if (scene.Locked)
                {
                    foundLocked = true;
                }
                scene.UpdateLocked(true);   // Same as UpdateLockedAllScenes but for an individual scene
            }
            if(!foundLocked)
            {
                saveCurrentSceneAgain = false;  // No need to save the scene a second time
            }
#else
            Multiscene.UpdateLockedAllScenes(true);
#endif
            preparedForSave = true;
            EditorApplication.delayCall += AfterSave;
        }

        static void AfterSave()
        {
            //Debug.Log("Finished save");
            MultisceneCameras.Instance.RefreshList();
            Multiscene.UpdateLockedAllScenes(false);

            List<Multiscene> scenes = new List<Multiscene>(MultisceneAssemblyReload.Instance.loadedScenes.Values);

            foreach (Multiscene scene in scenes)
            {
                if (scene.Locked)
                {                    
                    scene.UpdateLocked();
                }
                if(scene.isCurrentScene)
                {
                    // Check to see if we have suddenly become saved
                    if (/*!scene.HasFile && */EditorApplication.currentScene.Length > 0)
                    {
                        MultisceneAssemblyReload.Instance.Rename(scene, EditorApplication.currentScene);
                    }
                    //scene.IsDirty = false;
                }
                if (savedOrCreated.Contains(scene.Path))
                {
                    scene.IsDirty = false;
                }
                else
                {
                    // Just check in case the file has been renamed
                    scene.CheckName();
                }
            }
            savedOrCreated.Clear();
            Multiscene.SaveTriggeredByUs(false);
            preparedForSave = false;
            if (saveCurrentSceneAgain)
            {
                doSaveCurrentSceneAgain = true;
            }
            if (saveOtherScenesRequired || doSaveCurrentSceneAgain)
            {
                SaveOtherScenes();
            }
        }

        static void OnWillCreateAsset(string path)
        {
            //Debug.Log("OnWillCreateAsset " + path);

            if (path.EndsWith(".unity.meta"))
            {
                path = path.Substring(0, path.Length - 5);
                saveCurrentSceneAgain = true;

//                MultisceneAssemblyReload.Instance.SaveOccured(path);
                if (!Multiscene.GetSaveTriggeredByUs())
                {
                    Multiscene.SaveTriggeredByUs(true);
                    // We do not want to save our list of scenes just yet as we want unity to save the current scene first.
                    saveOtherScenesRequired = true;
                    // Unity crashes if we try to load files (required to change scenes) during creation of a new file.
                    // so we delay the call until to a safer callback, can't user delayCall as during quit it is not executed.
                }
                savedOrCreated.Add(path);
            }
        }

        // Check for any other scenes that need to be saved
        static void SaveOtherScenes()
        {
            //Debug.Log("SaveOtherScenes");
            saveOtherScenesRequired = false;
            EdMultisceneModWatch.CheckForComponentChange(); // Just in case a selection change has not occurred since removing / adding a component
            List<Multiscene> saveUs = new List<Multiscene>(MultisceneAssemblyReload.Instance.loadedScenes.Values);
            if (!doSaveCurrentSceneAgain)
            {
                for (int i = 0; i < saveUs.Count; i++)
                {
                    if (saveUs[i].isCurrentScene)
                    {
                        saveUs.RemoveAt(i);
                        break;
                    }
                }
            }
            Multiscene.Save(saveUs, false, doSaveCurrentSceneAgain);
            if(doSaveCurrentSceneAgain)
            {
                doSaveCurrentSceneAgain = false;
                saveCurrentSceneAgain = false;
            }
        }


    }
}
#endif
