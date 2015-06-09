using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
namespace Phi
{
    // Handles queuing of scene loading so that we only load one at a time.
    // This is only used for runtime loading rather than edit time loading
    public class MultisceneLoadManager : MonoBehaviour
    {
        static List<string> loadQueue = new List<string>();
        static List<string> loaded = new List<string>();
        static AsyncOperation currentLoadOperation;

        static MultisceneLoadManager instance = null;
        static MultisceneLoadManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameObject("Multiscene Load Manager", typeof(MultisceneLoadManager)).GetComponent<MultisceneLoadManager>();
                    GameObject.DontDestroyOnLoad(instance.gameObject);
                }
                return instance;
            }
        }

        public static Action onAllLoaded = null;

        public static bool IsLoaded(string path)
        {
            for (int i = loaded.Count - 1; i >= 0; i--)
            {
                if (loaded[i].EndsWith(path, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsLoading(string path)
        {
            for (int i = loadQueue.Count - 1; i >= 0; i--)
            {
                if (loadQueue[i].EndsWith(path, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsLoadedOrLoading(string path)
        {
            return IsLoaded(path) || IsLoading(path);
        }

        public static void Load(string path)
        {
            if (!IsLoadedOrLoading(path))
            {
                loadQueue.Add(path);
                if (loadQueue.Count == 1)
                {
                    // Better startup our loading coroutine again....
                    Instance.StartCoroutine(Instance.ProcessLoadQueue());
                }
            }
        }

        // This coroutine will load all scenes placed in the load queue
        IEnumerator ProcessLoadQueue()
        {
            do
            {
				string path = System.IO.Path.GetFileNameWithoutExtension(loadQueue[0].Replace('\\', '/'));
				currentLoadOperation = Application.LoadLevelAdditiveAsync(path);
                yield return currentLoadOperation;
                currentLoadOperation = null;
                loaded.Add(loadQueue[0].ToUpper());
                loadQueue.RemoveAt(0);
            } while (loadQueue.Count > 0);
            ConsiderSendingAllLoadedMsg();
        }

        // This function checks to see if there are any remaining scenes that need to be loaded:
        // Already being loaded
        // In the load queue
        // As part of an asleep MultisceneLoad
        static public bool AllLoaded()
        {
            if(loadQueue.Count > 0)
            {
                 return false;
            }
            // Also look for any objects in the scene that have not been awoken yet
            UnityEngine.Object[] objs = Resources.FindObjectsOfTypeAll(typeof(MultisceneLoad));
            for (int i = 0; i < objs.Length; i++)
            {
                UnityEngine.Object o = objs[i];
                MultisceneLoad loadScene = o as MultisceneLoad;
                Debug.Log("------------------ Check loaded state of found object " + loadScene.gameObject.name + " =" + loadScene.IsAwake);
                if (!loadScene.IsAwake)
                {
#if UNITY_EDITOR
                    if(UnityEditor.PrefabUtility.GetPrefabType(loadScene) != UnityEditor.PrefabType.Prefab)
#endif
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        static public void ConsiderSendingAllLoadedMsg()
        {
            if (onAllLoaded != null && AllLoaded())
            {
                onAllLoaded();
            }
        }

        /// <summary>
        /// Allows History to add a scene that it loads when playing from editor to the list of loaded scenes
        /// You should probably not use this function yourself.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the scene is loaded; otherwise, <c>false</c>.
        /// </returns>
        /// <param name='name'>
        /// The name of the scene to check.
        /// </param>
        static public void AddLoadedScene(string path)
        {
            if (IsLoaded(path))
            {
                Debug.LogWarning("AddLoadedScene trying to add " + path + " when it already exists.");
            }
            else
            {
                loaded.Add(path);
            }
        }

        // This should be called if you unload any scenes
        public static void Remove(string path)
        {
            for (int i = loaded.Count - 1; i >= 0; i--)
            {
                if (loaded[i].EndsWith(path, StringComparison.InvariantCultureIgnoreCase))
                {
                    loaded.RemoveAt(i);
                    return;
                }
            }
        }
    }
}
