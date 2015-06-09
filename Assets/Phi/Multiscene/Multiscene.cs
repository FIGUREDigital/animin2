// Multiscene
// Version 1.0.17
// (C) 2014 Trusted Developments Limited
// Note a per seat license (one per member of the team) is required to use this software on your project

using UnityEngine;
//using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;
#if UNITY_EDITOR && !UNITY_CLOUD_BUILD
using UnityEditor;
namespace Phi
{
    // This class should only be used in edit mode and should not manually be added to game objects
    // it is used as a heading fro scenes in the hierarchy view.
    [ExecuteInEditMode]
    public class Multiscene : MonoBehaviour
    {
        public static string version = "1.0.17";
#region serialized fields        
        [SerializeField]
        [HideInInspector]
        private string guid;

        [HideInInspector]
        public bool isCurrentScene = false; // True for one scene only the current one which is handled differently

        [HideInInspector]
        [SerializeField]
        private bool locked = false;        
        
        [HideInInspector]
        [SerializeField]
        private bool isDirty = false;   // Have we spotted an edit to this scene since it was last loaded / saved.

        private bool spotLockChanges = false;   // A copy of locked flag to spot when undo modifies it, do not serialize this!
        List<Component> protectedHideInInspectorComponents; // No need to serialize - as a short life span.
#endregion

#region Static members - will not survive an assembly reload!
        public static Action onSaveTriggeredByUs; // Called when something starts a set of save operations
        private static bool saveTriggeredByUs = false;  // True when saving a list of scenes
        public static bool changingScenes = false;    // Set to true when we switch scenes. Change modification watch will set this back to false after it has handled the switch        
	    static string prefix = null; // indentation of heading text
#if USEDONTSAVE
        static bool redrawGameViewRequired = false; // When editing don't save objects the game view does not update.
#endif
#endregion

        public static void SaveTriggeredByUs(bool start, bool actuallySaving = true)
        {
            if (saveTriggeredByUs != start)
            {
                saveTriggeredByUs = start;
                if (saveTriggeredByUs && actuallySaving)
                {
                    if (onSaveTriggeredByUs != null)
                    {
                        onSaveTriggeredByUs();
                    }
                }
            }
        }

        public static bool GetSaveTriggeredByUs()
        {
            return saveTriggeredByUs;
        }



        // Return the path of the file, this will start with 'Assets/' or if unsaved 'Untilted'
        public string Path
        {
            get
            {
                if (HasFile)
                {
                    return AssetDatabase.GUIDToAssetPath(guid);
                }
                return guid;
            }
            set
            {
                if (value!= null && value.Length > 0 && !value.StartsWith("Untitled"))
                {
                    guid = AssetDatabase.AssetPathToGUID(value);
                }
                else
                {
                    guid = value;
                }
            }
        }

        // Returns true if the scene has a file associated with it, ie it's not an untitled / unsaved scene.
        public bool HasFile
        {
            get
            {
                return guid != null && !guid.StartsWith("Untitled");
            }
        }

        // The unity GUID for the scene, this will start with Untitled if the scene has not been saved
        // in such circumstances it's not actually a unity GUID as none exists yet.
        public string GUID
        {
            get
            {
                return guid;
            }
        }

        // The current scene this should never return null.
        public static Multiscene CurrentScene
        {
            get
            {
                return MultisceneAssemblyReload.Instance.CurrentScene;
            }
        }

        // Handle dirty state of the scene, changing this value will update the gameobject's name adding or removing an *
        public bool IsDirty
        {
            get
            {
                return isDirty;
            }
            set
            {
                if (value != isDirty)
                {
                    isDirty = value;
                    CheckName();    // Cause * to update
                }
            }
        }
       
        public bool IsLoaded
        {
            get
            {
                return true;    // Currently we only exist if we are loaded, future may allow non loaded entries in hierarchy
            }
        }

        #region Locking of scene from editing
        // Is the scene locked?
        public bool Locked
        {
            get
            {
                return locked;
            }
            set
            {
                if(locked != value)
                {
                    locked = value;
                    OnValidate();
                }
            }
        }
        // We can't use the NotEditable flag when saving as they get saved even if DontSave is also set
        // so this is called before saving with start set to true and false after saving. Note we may call
        // this more often with false as there is no call back when saving has finished
        static int updateLockedCount = 0;
        public static void UpdateLockedAllScenes(bool start = true)
        {
            if(start)
            {
                updateLockedCount++;
                if (updateLockedCount > 1)
                {
                    //Debug.Log("Skip Start UpdateLockedAllScenes" + updateLockedCount);
                    return;
                }

            }
            else
            {
                updateLockedCount--;
                if (updateLockedCount > 0)
                {
                    //Debug.Log("Skip End UpdateLockedAllScenes" + updateLockedCount);
                    return;
                }
            }
            foreach (Multiscene s in MultisceneAssemblyReload.Instance.loadedScenes.Values)
            {
                s.UpdateLocked(start);
            }
        }

        // When saving we need the flag to be don't save...
        public void UpdateLocked(bool saving = false)
        {
            HideFlags f = HideFlags.None;
#if USEDONTSAVE
            f = !saving && locked ? HideFlags.NotEditable : HideFlags.DontSave;
            if (gameObject.hideFlags == f) return;
#else
            if (saving)
            {
                f = HideFlags.DontSave;
            }
            else if (locked)
            {
                f = HideFlags.NotEditable;
            }
#endif
            FlagRecursively(gameObject, f);
        }


        void OnValidate()
        {
            // Spot when undo alters the lock state.
            if(locked != spotLockChanges)
            {
                UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
                spotLockChanges = locked;
                UpdateLocked();
            }
        }
        #endregion

        #region ProtectHideInInspector
        // If we set the hide flags of the gameobject Unity seems to lose the hide flags of components
        // so this region stores and restores them.
        public static void ProtectHideInInspector(bool start = true)
        {
            Dictionary<string, Multiscene> loadedScenes = MultisceneAssemblyReload.Instance.loadedScenes;
            foreach (Multiscene s in loadedScenes.Values)
            {
                s.DoProtectHideInInspector(start);
            }
        }

        public void DoProtectHideInInspector(bool start)
        {
            //float startTime = Time.realtimeSinceStartup;
            if (start)
            {
                protectedHideInInspectorComponents = new List<Component>();
                Component[] components = GetComponentsInChildren<Component>();
                foreach(Component c in components)
                {
                    if (c.hideFlags == HideFlags.HideInInspector)
                    {
                        protectedHideInInspectorComponents.Add(c);
                        c.hideFlags = HideFlags.HideAndDontSave;
                    }
                }
            }
            else
            {
                if (protectedHideInInspectorComponents != null)
                {
                    foreach(Component c in protectedHideInInspectorComponents)
                    {
                        c.hideFlags = HideFlags.HideInInspector;
                    }
                    protectedHideInInspectorComponents = null;
                }
            }
            //Debug.Log("DoProtectHideInInspector " + start + " " + (Time.realtimeSinceStartup - startTime).ToString());
        }
        #endregion

        public static bool IsPathLoaded(string path)
        {
            path = EnsureFullPath(path);
            return (Multiscene.GetIfExists(path) != null);
        }

        // Return the scene with the passed path if it is already loaded otherwise return null
        // Used by PhiLoadScene script to see if it needs to load the scene or not.
        public static Multiscene GetIfExists(string path)
        {
            Multiscene scene;
            string guid = GetGUIDFromPath(path);
            if (MultisceneAssemblyReload.Instance.loadedScenes.TryGetValue(guid, out scene))
            {
                return scene;
            }
            return null;
        }

        static string GetGUIDFromPath(string path)
        {
            if (!path.StartsWith("Untitled"))
            {
                return AssetDatabase.AssetPathToGUID(path);
            }
            else
            {
                return path;
            }
        }

        // Create a new scene
        public static Multiscene NewScene(string path = null)
        {
            if (path == null || path.Length == 0)
            {
                path = MultisceneAssemblyReload.Instance.GetUntitledPath();
            }
            GameObject go = new GameObject(GetHiererachyName(path));
#if USEDONTSAVE
            go.hideFlags = HideFlags.DontSave;
#endif
            Multiscene slde = go.AddComponent<Multiscene>();
            slde.Path = path;
            MultisceneAssemblyReload.AddScene(slde);
            slde.IsDirty = true;
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            return slde;
        }

        public static String EnsureFullPath(string path)
        {
            if (!path.EndsWith(".unity"))
            {
                path = path + ".unity";
            }
            if (!path.StartsWith("Assets/"))
            {
                path = "Assets/" + path;
            }
            return path;
        }

        // Load a scene returning the Multiscene component
        public static Multiscene Load(string path, bool warnAlreadyLoaded = true)
        {
            path = EnsureFullPath(path);
            if (Multiscene.GetIfExists(path) != null)
            {
                if(warnAlreadyLoaded)
                {
                    EditorUtility.DisplayDialog("Already loaded", "You can not load the same scene multiple times.", "Ok");
                }
                return null;
            }
            if (path.CompareTo(EditorApplication.currentScene) == 0)
            {
                if(warnAlreadyLoaded)
                {
                    EditorUtility.DisplayDialog("Ignoring current scene", "You can not drag the current scene into it's self.", "Ok");
                }
                return null;
            }
        
            // Create root to load scene into.
            GameObject go = new GameObject(GetHiererachyName(path));
#if USEDONTSAVE
            go.hideFlags = HideFlags.DontSave;
#endif
            Multiscene slde = go.AddComponent<Multiscene>();

            slde.Path = path;
            MultisceneAssemblyReload.AddScene(slde);
            slde.StartLoad();
            Undo.RegisterCreatedObjectUndo(go, "Load " + go.name);
            return slde;
        }

        //Reload a scene discarding any changes.
        public void Reload()
        {
            if (isCurrentScene)
            {
                if (EditorUtility.DisplayDialog("This can not be undone", "Would you like to discard your changes to the current scene?", "Discard", "Cancel"))
                {
                    Multiscene.PrepareForSceneLoad();
                    if (!HasFile || !EditorApplication.OpenScene(Path))
                    {
                        EditorApplication.NewScene();
                    }
                    IsDirty = false;
                }
            }
            else
            {
                Undo.RegisterFullObjectHierarchyUndo(gameObject, "Reload " + name);
                List<Transform> transforms = new List<Transform>();
                for (int i = 0; i < transform.childCount; i++)
                {
                    transforms.Add(transform.GetChild(i));
                }
                foreach (Transform t in transforms)
                {
                    DestroyImmediate(t.gameObject);
                }
                StartLoad();
            }
        }

        // Called by static Load on the Multiscene it creates, this actually
        // loads the scene.
        void StartLoad()
        {
            // Make a list of all objects that are currently loaded
            List<Transform> alreadyLoaded = ListExistingObjects();

            // Now open the scene
            EditorApplication.OpenSceneAdditive(Path);
            MakeNewObjectsChildren(alreadyLoaded);
            IsDirty = false;
        }    

        static public List<Transform> ListExistingObjects()
        {        
            // Make a list of all objects that are currently loaded
            List<Transform> alreadyLoaded = new List<Transform>();

            object[] obj = Resources.FindObjectsOfTypeAll(typeof(GameObject));
            foreach (object o in obj)
            {
                GameObject g = o as GameObject;
                if (g.transform.parent == null)
                {
                    if (IsASceneObject(g))
                    {
                        alreadyLoaded.Add(g.transform);
                    }
                }
            }

            return alreadyLoaded;
        }
        
        // Move any newly loaded objects into child of this gameobject.
        public void MakeNewObjectsChildren(List<Transform> alreadyLoaded)
        {
            List<GameObject> loaded = new List<GameObject>();
            if (alreadyLoaded != null)
            {
                object[] obj = Resources.FindObjectsOfTypeAll(typeof(GameObject));
                foreach (object o in obj)
                {
                    GameObject g = o as GameObject;
                    if (g.transform.parent == null)
                    {
                        if (IsASceneObject(g))
                        {
                            if (!alreadyLoaded.Contains(g.transform))
                            {
                                if (!g.GetComponent<Multiscene>())
                                {
                                    loaded.Add(g);
                                    try
                                    {
                                        g.transform.parent = transform;
                                    }
                                    catch (System.Exception e)
                                    {
                                        Debug.Log("Failed to set transform on " + g + " " + e.Message);
                                    }
                                }
                            }
                        }
                    }
                }
                alreadyLoaded.Clear();
                alreadyLoaded = null;
            }
#if USEDONTSAVE
            foreach (GameObject go in loaded)
            {
                FlagRecursively(go, HideFlags.DontSave);
            }
#endif
        }
        
        // Find all objects returns objects that unity uses internally, this function
        // checks that the object is actually in the hierarchy.
        public static bool IsASceneObject(GameObject gO)
        {
            if (gO.hideFlags == HideFlags.HideAndDontSave) return false;
            PrefabType pType = PrefabUtility.GetPrefabType(gO);
            if (pType != PrefabType.Prefab)
            {
                return !AssetDatabase.Contains(gO);
            }
            else
            {
                return false;
            }
        }

        // Get the name to display in the hierarchy window
        public static string GetHiererachyName(string path, bool isDirty = false)
        {
            // prefix is the indentation of the name giving space for the unity scene icon.
		    if(prefix == null)
		    {
                // MacOsX has a different font so need a different number of spaces
			    if(Application.platform == RuntimePlatform.OSXEditor)
			    {
				    prefix = "     ";
			    }
			    else
			    {
				    prefix = "    ";
			    }
		    }

            // Strip off Assets and file extension
            string sceneName = path;
            if (sceneName.EndsWith(".unity"))
            {
                sceneName = sceneName.Substring(0, sceneName.Length - 6);
            }
            if (sceneName.StartsWith(MultisceneAssemblyReload.Instance.PathPrefix))
            {
                sceneName = sceneName.Substring(MultisceneAssemblyReload.Instance.PathPrefixLength);
            }

            // Add * if it's dirty
            if(isDirty)
            {
                return prefix + sceneName+"*";
            }
            return prefix + sceneName;
        }

        // Ensures the name of the gameobject is correct and sets it if not.
        public void CheckName()
        {
            string sceneName = GetHiererachyName(Path, isDirty);
            if (sceneName != name)
            {
                name = sceneName;
                EditorApplication.RepaintHierarchyWindow();
            }
        }

        // Recursively set hideflags
        static public void FlagRecursively(GameObject go, HideFlags hideFlags)
        {
            go.hideFlags = hideFlags;
            foreach (Transform t in go.transform)
            {
                FlagRecursively(t.gameObject, hideFlags);
            }
        }

        public void UnloadLevel()
        {
            if (isCurrentScene)
            {
                EditorUtility.DisplayDialog("Sorry", "You can not currently unload the current scene.", "Ok");
                return;
            }
            GameObject.DestroyImmediate(gameObject);
        }

        // Catch some instances when a duplicate scene is created and remove them.
        void Awake()
        {
            string path = Path;
            //Debug.Log("Awake " + MultisceneAssemblyReload.Instance.loadedScenes.Count + " " + path);
            if (path != null && path.Length > 0)
            {
                Multiscene scene = Multiscene.GetIfExists(path);
                if (scene == null)
                {
                    MultisceneAssemblyReload.AddScene(this);
                    if(isCurrentScene)
                    {
                        MultisceneAssemblyReload.Instance.CurrentScene = this;
                    }
                }
                else
                {
                    // A duplicate ?
                    if (scene != this)
                    {
                        DestroyImmediate(gameObject);
                    }
                }
            }
        }
        void OnDestroy()
        {
            // Remove from global list etc
            MultisceneAssemblyReload.Instance.RemoveScene(this);
        }

        public void OnEnable()
        {
            UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
        }

        void OnDisable()
        {
            // Not sure why we had UpdateLocked(true); below so removed.
            //UpdateLocked(true);
            UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
        }

        // This is used to save a list of scenes setting a flag to avoid triggering
        // any other autosaves.
        public static void Save(List<Multiscene> scenes, bool force = false, bool saveCurrentSceneAgain = false)
        {
            SaveTriggeredByUs(true);
            

            // Step 1 save current scene if need be
            for (int i = 0; i < scenes.Count; i++)
            {
                if(scenes[i].isCurrentScene)
                {
                    if (scenes[i].isDirty || force || saveCurrentSceneAgain)
                    {
                        if (saveCurrentSceneAgain)
                        {
                            bool currentSetting = EditorPrefs.GetBool("VerifySavingAssets", false);
                            EditorPrefs.SetBool("VerifySavingAssets", false);
                            /*if (EditorPrefs.GetBool("VerifySavingAssets", false) == true)
                            {
                                // Warn user they MUST save current scene again...
                                EditorUtility.DisplayDialog("IMPORTANT WARNING!", "Save '" + scenes[i].Path + "' when prompted to avoid corruption. When saving using save as and you have locked scenes Multiscene has to save the scene a second time.", "Ok");
                            }*/
                            scenes[i].DoSaveScene();
                            EditorPrefs.SetBool("VerifySavingAssets", currentSetting);
                        }
                        else
                        {
                            scenes[i].DoSaveScene();
                        }
                    }
                    scenes.RemoveAt(i);
                    break;
                }
            }

            // Now move current scene's objects into scene and save any remaining scenes
            // then restore current scene.
            if (scenes.Count > 0)
            {
                Multiscene originalScene = null;
                for (int i = 0; i < scenes.Count; i++)
                {
                    if (scenes[i].isDirty || force)
                    {
                        Multiscene scene = scenes[i].ReplaceSceneWithThis(false);
                        if (originalScene == null)
                        {
                            originalScene = scene;
                        }
                        scenes[i].DoSaveScene();
                    }
                }
                if (originalScene != null)
                {
                    originalScene.ReplaceSceneWithThis(false);
                }
            }
        }

        private void DoSaveScene()
        {
            if (EditorApplication.SaveScene())
            {
                string path = Path;
                if (path.StartsWith("Untitled"))
                {
                    MultisceneAssemblyReload.Instance.Rename(this, EditorApplication.currentScene);
                    name = GetHiererachyName(path, false);
                }
            }
        }

        // Switch editor to this scene
        public Multiscene ReplaceSceneWithThis(bool userSwitchingScenes)
        {

//            Debug.Log("Start changingScenes to "+name+" from "+CurrentScene.name);
            changingScenes = true;
            //EditorApplication.SaveScene(EditorApplication.currentScene);
            UpdateLockedAllScenes(true);

            // Move current scene into heading
            Multiscene prevScene = CurrentScene;// SwitchFromExistingScene();
            List<Transform> alreadyLoaded = new List<Transform>();
            alreadyLoaded.Add(MultisceneAssemblyReload.Instance.CurrentScene.transform);
            CurrentScene.MakeNewObjectsChildren(alreadyLoaded);
            CurrentScene.UpdateLocked(true);    // Set hideflags

            // This scene becomes the current
            MultisceneAssemblyReload.Instance.CurrentScene = this;

        
            Multiscene.ProtectHideInInspector(true);

            MultisceneCameras.Instance.ReadyForSave(true);

            MultisceneGCProtect.Protect();
            string path = Path;
            if (path.StartsWith("Untitled") || !EditorApplication.OpenScene(path))
            {
                EditorApplication.NewScene();
            }
            MultisceneGCProtect.Unprotect();

            object[] obj = Resources.FindObjectsOfTypeAll(typeof(GameObject));
            foreach (object o in obj)
            {
                GameObject g = o as GameObject;
                if (g != null)
                {
                    if (g.transform.parent == null)
                    {
                        if (IsASceneObject(g))
                        {
                            if (g.GetComponent<Multiscene>() == null)
                            {
                                DestroyImmediate(g);
                            }
                        }
                    }
                }
            }
            // Now move all of our components into the root and make savable again...
            List<Transform> moveUs = new List<Transform>();
            foreach(Transform t in transform)
            {
                moveUs.Add(t);
            }
            foreach(Transform t in moveUs)
            {
                t.parent = null;
                FlagRecursively(t.gameObject, HideFlags.None);
            }
            UpdateLockedAllScenes(false);

            Multiscene.ProtectHideInInspector(false);
            MultisceneAssemblyReload.Instance.SetHeading();
            MultisceneAssemblyReload.Instance.SortInHeirarchy();
            MultisceneCameras.Instance.RefreshList();


            if (userSwitchingScenes)
            {
                foreach(Multiscene scene in MultisceneAssemblyReload.Instance.loadedScenes.Values)
                {
                    if (scene.Locked)
                    {
                        WarnLockAndQuit();
                        break;
                    }
                }
            }

            return prevScene;
        }
        
        public static bool WarnLockAndQuit()
        {
            // Do we have any unsaved scenes?
            if (!Multiscene.CurrentScene.HasFile)
            {
                if (!EditorPrefs.GetBool("WarnedUserLockAndQuit", false))
                {
                    if (EditorUtility.DisplayDialog("Warning", "If you save the current Untitled scene while closing Unity the locked scene(s) will be saved inside it.\nYou can fix this by loading the scene into unity and removing the locked scene(s).\n\nSaving the scene before closing Unity works as expected.", "Do not warn me again", "Ok"))
                    {
                        EditorPrefs.SetBool("WarnedUserLockAndQuit", true);
                    }
                }
                return true;
            }
            return false;
        }
        private static Multiscene SwitchFromExistingScene()
        {
            List<Transform> alreadyLoaded = new List<Transform>();
            alreadyLoaded.Add(MultisceneAssemblyReload.Instance.CurrentScene.transform);
            CurrentScene.MakeNewObjectsChildren(alreadyLoaded);
            return CurrentScene;
        }


        // When we enter play mode we need to remove all our scenes / headings
        public static void StartPlayMode()
        {
            Dictionary<string, Multiscene> loadedScenes = MultisceneAssemblyReload.Instance.loadedScenes;
            Undo.IncrementCurrentGroup();
            MultisceneAssemblyReload.Instance.undoGroup = Undo.GetCurrentGroup();
            List<Multiscene> removeUs = new List<Multiscene>(loadedScenes.Values);
            foreach (Multiscene s in removeUs)
            {
                if (!s.isCurrentScene)
                {
                    Undo.DestroyObjectImmediate(s.gameObject);
                }
            }
            if (MultisceneAssemblyReload.Instance.CurrentScene && MultisceneAssemblyReload.Instance.CurrentScene.gameObject)
            {
                Undo.DestroyObjectImmediate(MultisceneAssemblyReload.Instance.CurrentScene.gameObject);
            }
        }

        public static void BackToEditMode()
        {
            int undoGroup = MultisceneAssemblyReload.Instance.undoGroup;
            //Debug.Log("BackToEditMode " + undoGroup);
            if (undoGroup != -1)
            {
                Undo.RevertAllDownToGroup(undoGroup);
                MultisceneAssemblyReload.Instance.undoGroup = -1;
            }
        }

        // Return the scene that a transform is part of
        public static Multiscene Get(Transform t)
        {
            if (t != null)
            {
                Multiscene scene = t.root.GetComponent<Multiscene>();
                if(scene == null)
                {
                    // Part of current scene
                    return CurrentScene;
                }
                return scene;
            }
            return null;
        }

        public static void RemoveAll(bool saveOtherScenes)
        {        
            List<Multiscene> removeUs = new List<Multiscene>(MultisceneAssemblyReload.Instance.loadedScenes.Values);
            removeUs.Remove(Multiscene.CurrentScene); // Unity will automatically try saving current scene anyway!
            if (saveOtherScenes)
            {
                Multiscene.Save(removeUs);
            }
            foreach (Multiscene s in removeUs)
            {
                GameObject.DestroyImmediate(s.gameObject);
            }
        }

        public static void FinishedLoad()
        {
//            Debug.Log("Finished load");
            Multiscene.SaveTriggeredByUs(false);
            EditorApplication.update -= FinishedLoad;
            MultisceneAssemblyReload.Instance.SortInHeirarchy();
            Multiscene.ProtectHideInInspector(false);
            Multiscene.UpdateLockedAllScenes(false);

            MultisceneAssemblyReload.Instance.SetHeading();
            MultisceneCameras.Instance.RefreshList();
        }

        public static void PrepareForSceneLoad()
        {
//            Debug.Log("PrepareForSceneLoad");
            Multiscene.UpdateLockedAllScenes(true);
            Multiscene.ProtectHideInInspector(true);
            MultisceneCameras.Instance.ReadyForSave(false);
            EditorApplication.update += FinishedLoad;
        }
    }

}

#endif
