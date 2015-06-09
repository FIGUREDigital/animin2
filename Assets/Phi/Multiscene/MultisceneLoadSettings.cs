using UnityEngine;
using System.Collections;

namespace Phi
{
    [System.Serializable]
    public class MultisceneLoadSettings
    {
        [Tooltip("When ticked the scene will not be loaded when the scene is loaded in play mode, instead the only functionality will be to allow you to load / unload the scene in the editor.")]
        [SerializeField]
        private bool loadInEditMode = false;
        [SerializeField]
        private bool loadInPlayMode = false;

        public bool LoadInEditMode
        {
            get { return loadInEditMode; }
            set { loadInEditMode = value; }
        }

        public bool LoadInPlayMode
        {
            get { return loadInPlayMode; }
            set { loadInPlayMode = value; }
        }

        [SerializeField]
        [HideInInspector]
        private string scenePath = "";

        // Script access to changing the path
        public string ScenePath
        {
            get { return scenePath; }
            set
            {
                if (value != scenePath)
                {
                    scenePath = value;
                    #if UNITY_EDITOR
                    Validate();
#endif
                }
            }
        }

        private string curScenePath = "";    // Currently validated scene - allows us to spot when the path changes via undo etc

        // Serialize this to cope with going between editor edit mode and play mode
        // when saved this should always be saved as false due to OnSave setting it.
        //[SerializeField]
        //[HideInInspector]
//        private string loadedName = ""; // This will be "" if the scene is not loaded or the name of the scene that is currently loaded
        //[SerializeField]
        //[HideInInspector]
        private string sceneName = "";
//        private bool loading = false;

//        private bool alreadyLoadedFlag = false;
        public string SceneName
        {
            get { return sceneName; }
        }

        public MultisceneLoadSettings(string path)
        {
            ScenePath = path;
        }

#if UNITY_EDITOR
        // Check to see if the scene name has changed and if so possibly load the new scene.
        public void Validate()
        {
            if (curScenePath == scenePath) return;  // No change
            if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode) return;

            // If currently loaded unload
            if (curScenePath != null && Loaded())
            {
                // Are there any other MultisceneLoad components also referencing this scene?

                Object[] objs = Resources.FindObjectsOfTypeAll(typeof(MultisceneLoad));
                bool keepLoaded = false;
                foreach (Object o in objs)
                {
                    MultisceneLoad loadScene = o as MultisceneLoad;
                    bool check = UnityEditor.PrefabUtility.GetPrefabType(loadScene) != UnityEditor.PrefabType.Prefab;
                    if (check && loadScene.IsAwake)
                    {
                        if (loadScene.ShouldLoad(curScenePath))
                        {
                            keepLoaded = true;
                            break;
                        }
                    }
                }
                if (!keepLoaded)
                {
                    UnloadLevel();
                }
            }

            // Get scene name
            sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            Load();
        }
#endif
        // Checks the current play mode state / editor state with the load flags to see if this scene should be loaded
        // note this does not take into account whether the scene is already loaded or in the process of being loaded
        public bool ShouldLoad()
        {
            #if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
            {
                return LoadInEditMode;
            }
            #endif
            return LoadInPlayMode;
        }

        public void DoLoadInEditor()
        {
            
            #if UNITY_EDITOR
            Multiscene.Load(ScenePath, false);
#endif
        }

        public void Load()
        {
            if (!ShouldLoad()) return;
#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
            {
                //Debug.Log("DoLoad(" + ScenePath + ")");
                UnityEditor.EditorApplication.delayCall += DoLoadInEditor;
                //Multiscene.Load(ScenePath);
                /*
                Multiscene loadedScene = Multiscene.GetIfExists("Assets\\" + ScenePath + ".unity");
                if (loadedScene == null || !loadedScene.IsLoaded)
                {
                    if (GUILayout.Button("Load Scene"))
                    {
                        Multiscene.Load(ScenePath);
                    }
                }
                else
                {
                    if (GUILayout.Button("Unload Scene"))
                    {
                        loadedScene.UnloadLevel();
                    }
                }*/
            }
            else
#endif
            {
                MultisceneLoadManager.Load(ScenePath);
            }
        }

        public bool Loaded()
        {
#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
            {
                return Multiscene.IsPathLoaded(ScenePath);
            }
            else
#endif
            {
                return MultisceneLoadManager.IsLoaded(ScenePath);
            }
        }

        
        public void UnloadLevel(bool doNotUseImmediate = false)
        {
#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
            {
                Multiscene loadedScene = Multiscene.GetIfExists("Assets\\" + ScenePath + ".unity");
                if (loadedScene != null)
                {
                    loadedScene.UnloadLevel();
                }
            }
            else
#endif
            {
                // No support for unloading runtime scenes at present
            }
        }

    }
}
