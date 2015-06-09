
#if UNITY_EDITOR && !UNITY_CLOUD_BUILD
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.IO;
using System.Text;
namespace Phi
{
    // This class is used to store values that we want to survive an assembly reload in edit mode

    public class MultisceneAssemblyReload : ScriptableObject, ISerializationCallbackReceiver
    {
        public int undoGroup = -1;
        public Multiscene nextScene = null; // Used to check new objects remain between the heading and the next scene
        //public bool sceneWasDirtyBeforeAdd = true;

        // All scenes keyed by GUID
        public Dictionary<string, Multiscene> loadedScenes = new Dictionary<string, Multiscene>();

        List<string> loadedSceneGUIDs = new List<string>();

        Multiscene currentScene = null;
        string pathPrefix = "";  // Common start of path for all scenes
        int pathPrefixLength = 0;

        public string PathPrefix
        {
            get
            {
                return pathPrefix;
            }
        }

        public int PathPrefixLength
        {
            get
            {
                return pathPrefixLength;
            }
        }

        // Used to serialize dictionary
        [Serializable]
        struct ScenePair
        {
            public ScenePair(Multiscene scene)
            {
                this.scene = scene;
                guid = scene.GUID;
            }
            public string guid;
            public Multiscene scene;
        }
        private List<ScenePair> serializeDict = new List<ScenePair>();

        public static bool skipNextHierarchyChangeDueToSort = false;

        public void OnBeforeSerialize()
        {
            serializeDict.Clear();
            foreach (Multiscene s in loadedScenes.Values)
            {
                serializeDict.Add(new ScenePair(s));
            }
        }

        public void OnAfterDeserialize()
        {
            loadedScenes = new Dictionary<string, Multiscene>();
            foreach (ScenePair s in serializeDict)
            {
                loadedScenes.Add(s.guid, s.scene);
            }
        }


        // The current scene
        public Multiscene CurrentScene
        {
            get
            {
                return currentScene;
            }
            set
            {
                if (value != currentScene)
                {
                    if (currentScene != null)
                    {
                        currentScene.isCurrentScene = false;
                    }
                    currentScene = value;
                    if (currentScene != null)
                    {
                        currentScene.isCurrentScene = true;
                        UpdateNextScene();
                        SortInHeirarchy();
                        MultisceneCameras.Instance.RefreshList();
                    }
                }
            }
        }


        // Find next scene after current scene and set nextScene
        void UpdateNextScene()
        {
            nextScene = null;
            if (currentScene == null) return;
            int currentIndex = loadedSceneGUIDs.FindIndex(x => x.CompareTo(currentScene.GUID) == 0);
            if (currentIndex >= 0)
            {
                currentIndex++;
                if (currentIndex < loadedSceneGUIDs.Count)
                {
                    nextScene = loadedScenes[loadedSceneGUIDs[currentIndex]];
                }
            }
        }


        public void RemoveScene(Multiscene slde)
        {
            // Cope with deleting items that are not in the list due to undo and already having a new heading in the scene,
            // hence new object does not get into the list and is deleted.
            Multiscene inList = Multiscene.GetIfExists(slde.Path);
            int index = -1;
            if (slde == currentScene && loadedScenes.Count > 0)
            {
                index = loadedSceneGUIDs.IndexOf(currentScene.GUID);
            }
            if (inList == slde)
            {
                loadedScenes.Remove(slde.GUID);
                loadedSceneGUIDs.Remove(slde.GUID);
            }
            if (loadedScenes.Count == 1 && Multiscene.CurrentScene != null && slde != currentScene)
            {
                // No longer need a heading
                DestroyImmediate(currentScene.gameObject);
                currentScene = null;
            }
            if (slde == currentScene && loadedScenes.Count > 0)
            {
                currentScene = null;
                // Add back
                SetHeading(index);
            }
            UpdateNextScene();
            CheckCommonPath();
        }

        public static void AddScene(Multiscene slde, int index = -1)
        {
            Instance.DoAddScene(slde, index);
            Instance.CheckCommonPath();
        }

        void CheckCommonPath()
        {
            if (loadedScenes.Count == 0)
            {
                pathPrefix = "";
                pathPrefixLength = 0;
                return;
            }
            string commonPath = null;
            int length = 0;
            int origLength = 0;
            foreach (Multiscene scene in loadedScenes.Values)
            {
                if (scene.HasFile)
                {
                    // Only worry about saved files.
                    if (commonPath == null)
                    {
                        commonPath = Path.GetDirectoryName(scene.Path) + "/";
                        origLength = length = commonPath.Length;
                    }
                    else
                    {
                        string path = scene.Path;
                        int len = path.Length;
                        if (length > len)
                        {
                            length = len;
                        }
                        int i = 0;
                        while (i < length && commonPath[i] == path[i])
                        {
                            i++;
                        }
                        length = i;
                    }
                }
            }
            if (origLength != length)
            {
                commonPath = commonPath.Substring(0, length);
            }
            if (pathPrefix.CompareTo(commonPath) != 0)
            {
                // We have a new prefix path.
                pathPrefix = commonPath;
                pathPrefixLength = length;
                CheckAllNames();
            }
        }

        void DoAddScene(Multiscene slde, int index)
        {
            loadedScenes.Add(slde.GUID, slde);
            if (slde.isCurrentScene || index >= 0)
            {
                if (index < 0)
                {
                    index = 0;
                }
                loadedSceneGUIDs.Insert(index, slde.GUID);
            }
            else
            {
                loadedSceneGUIDs.Add(slde.GUID);
            }
            if (currentScene == null)
            {
                MultisceneAssemblyReload.Instance.SetHeading();
            }
            UpdateNextScene();
            SortInHeirarchy();
            MultisceneCameras.Instance.RefreshList();
        }


        public void Rename(Multiscene scene, string path)
        {
            loadedScenes.Remove(scene.GUID);
            int index = loadedSceneGUIDs.IndexOf(scene.GUID);
            loadedSceneGUIDs.RemoveAt(index);
            scene.Path = path;
            loadedScenes.Add(scene.GUID, scene);
            // Need to keep order in name list
            loadedSceneGUIDs.Insert(index, scene.GUID);
            CheckCommonPath();
        }

        // Keep everything orederd in he hierarchy view
        public void SortInHeirarchy()
        {
            int i;
            string s;
            Multiscene loaded;
            for (i = 0; i < loadedSceneGUIDs.Count; i++)
            {
                s = loadedSceneGUIDs[i];
                loaded = loadedScenes[s];
                if (loaded == nextScene)
                {
                    break;
                }
                int curIndex = loaded.transform.GetSiblingIndex();
                //                Debug.Log(curIndex + ") ->" + i + " " + s);
                if (curIndex != i)
                {
                    loaded.transform.SetSiblingIndex(i);
                }
            }
            if (i < loadedSceneGUIDs.Count)
            {
                int end = i;
                int sindex = -1;

                for (i = loadedSceneGUIDs.Count - 1; i >= end; i--)
                {
                    s = loadedSceneGUIDs[i];
                    loaded = loadedScenes[s];
                    if (sindex == -1)
                    {
                        loaded.transform.SetAsLastSibling();
                        sindex = loaded.transform.GetSiblingIndex();
                    }
                    else
                    {
                        int curIndex = loaded.transform.GetSiblingIndex();
                        //                        Debug.Log(curIndex + ") ->" + sindex + " " + s);
                        if (curIndex != sindex)
                        {
                            loaded.transform.SetSiblingIndex(sindex);
                        }
                    }
                    sindex--;
                }
            }
            skipNextHierarchyChangeDueToSort = true;
        }


        // Return a unique path starting with Untitled
        public string GetUntitledPath()
        {
            if (!loadedSceneGUIDs.Contains("Untitled")) return "Untitled";
            int i = 2;
            while (true)
            {
                string name = "Untitled " + i;
                if (!loadedSceneGUIDs.Contains(name)) return name;
                i++;
            }
        }

        public void SetHeading(int index = -1)
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode) return;
            if (loadedSceneGUIDs.Count == 0) return;
            string path = EditorApplication.currentScene;
            if (path.Length == 0)
            {
                if (currentScene != null && currentScene.Path.StartsWith("Untitled"))
                {
                    path = currentScene.Path;
                }
                else
                {
                    path = GetUntitledPath();
                }
            }
            if (currentScene == null)
            {
                string name = Multiscene.GetHiererachyName(path, true);
                //                Debug.Log("Create new heading " + EditorApplication.currentScene);
                currentScene = new GameObject(name, typeof(Multiscene)).GetComponent<Multiscene>();
#if USEDONTSAVE
                currentScene.gameObject.hideFlags = HideFlags.DontSave;
#endif
                currentScene.isCurrentScene = true;
                //heading.transform.SetSiblingIndex(0);

                currentScene.Path = path;
                AddScene(currentScene, index);
                currentScene.IsDirty = true;// sceneWasDirtyBeforeAdd;
                //heading.hideFlags = HideFlags.HideInInspector;
            }
            else
            {
                string name = Multiscene.GetHiererachyName(path, currentScene.IsDirty);
                if (currentScene.Path.CompareTo(path) != 0)
                {
                    // Rename.
                    Rename(currentScene, path);
                }
                if (name != currentScene.name)
                {
                    currentScene.name = name;
                }
            }
        }


        static MultisceneAssemblyReload instance = null;
        static public MultisceneAssemblyReload Instance
        {
            get
            {
                if (instance == null)
                {
                    UnityEngine.Object[] o = Resources.FindObjectsOfTypeAll(typeof(MultisceneAssemblyReload));
                    if (o != null && o.Length > 0)
                    {
//                        Debug.LogWarning("Found Assembly Reload");
                        instance = o[0] as MultisceneAssemblyReload;
                    }
                    if (instance == null)
                    {
//                        Debug.LogWarning("Create Assembly Reload");
                        instance = CreateInstance<MultisceneAssemblyReload>();
                        instance.hideFlags = HideFlags.HideAndDontSave;
                    }
                }
                return instance;
            }
        }

        public void OnEnable()
        {
 //           Debug.Log("Enable Assembly Reload undoGroup =" + undoGroup);
            if (instance != null && instance != this)
            {
                Debug.LogError("Multiple instances of MultisceneAssemblyReload should not occur, please report this to support@trusteddevelopments.com");
            }
            instance = this;
        }

        public void RemoveCameraChanges(bool keepRemoved = false)
        {
            MultisceneCameras.Instance.ReadyForSave(keepRemoved);
        }

        // This is used when the project window is updated in case the user has renamed a scene.
        // It just goes through all scene headers and checks the names
        public void CheckAllNames()
        {
            foreach (Multiscene s in loadedScenes.Values)
            {
                s.CheckName();
            }
        }

        public void CheckForDeleted()
        {
            List<Multiscene> scenes = new List<Multiscene>(loadedScenes.Values);
            foreach (Multiscene s in scenes)
            {
                if (s.Path.Length == 0)
                {
                    Rename(s, GetUntitledPath());
                }

            }
        }
    }
}
#endif
