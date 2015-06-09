#if !UNITY_CLOUD_BUILD
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
namespace Phi
{
    [InitializeOnLoad]
    public class EdMultisceneOnLoad
    {
        static EdMultisceneOnLoad()
        {
            //PhiSceneOnLoad.Create();
        }

        [UnityEditor.Callbacks.OnOpenAsset]
        public static bool openingAsset(int instance, int line)
        {
            string path = AssetDatabase.GetAssetPath(instance);
            if (!path.EndsWith(".unity")) return false;
            if(MultisceneAssemblyReload.Instance.loadedScenes.Count < 1)
            {
				if(IsCmdCtrlHeld())
                {
                    Multiscene.Load(path);
                    return true;
                }
                return false;
            }

            if (!EditorPrefs.GetBool("PhiScenesOpeningAsset", false))
            {
                bool ans;
                if (Application.platform == RuntimePlatform.OSXEditor)
                {
                    ans = EditorUtility.DisplayDialog("Multiscene", "Hold down cmd when double clicking a scene to add it to the hierarchy without replacing the currently loaded scene(s).", "Do not tell me again", "Ok");
                }
                else
                {
                    ans = EditorUtility.DisplayDialog("Multiscene", "Hold down ctrl when double clicking a scene to add it to the hierarchy without replacing the currently loaded scene(s).", "Do not tell me again", "Ok");
                }
                if (ans)
                {
                    EditorPrefs.SetBool("PhiScenesOpeningAsset", true);
                }
            }

			if (IsCmdCtrlHeld())
            {
                // User wants to keep scenes loaded...
                Multiscene scene = Multiscene.GetIfExists(path);
                if (scene == null)
                {
                    // Need to move current scene into a child heading create a new heading for the new scene being loaded
                    //scene = PhiSceneLoadedDuringEdit.NewScene(path);
                    /*Multiscene loaded = */ Multiscene.Load(path);
                    // I think it's better to stay on the current scene and just append the new one.
                    //loaded.ReplaceSceneWithThis();
                    return true;    // No need for unity to load the scene now.
                }
                if (!scene.isCurrentScene)
                {
                    // Yikes loading a scene that is already loaded as a secondary scene, do not want two loaded and
                    // do not current scene replaced with it...
                    // So switch to that scene....
                    // We set the flag directly here as we do not wish to prepare the scene for saving
                    Multiscene.SaveTriggeredByUs(true, false); // We do not want to automatically start saving other scenes in this scenario as we can keep them loaded
                    // we clear this flag in FinishLoad below.
                    scene.ReplaceSceneWithThis(true);
                }
                Multiscene.PrepareForSceneLoad();
                return false;
            }
            else
            {
                Multiscene.RemoveAll(true);
            }
            return false;
        }

		static bool IsCmdCtrlHeld()
		{
			if (Application.platform == RuntimePlatform.OSXEditor)
			{
					return Event.current.command;
			}
			return Event.current.control;
		}
    }
}
#endif
