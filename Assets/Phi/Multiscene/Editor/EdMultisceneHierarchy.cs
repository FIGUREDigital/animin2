#if !UNITY_CLOUD_BUILD
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;
namespace Phi
{
    [InitializeOnLoad]
    public class EdMultisceneHierarchy
    {
        static Texture sceneIcon;
        // static GUISkin edSkin;
        static float sceneIconOffset = 0;
        public static bool changingVisibility = false;
        //static MethodInfo createContextMenu = null;
        static MethodInfo extractMenuItemWithPath = null;

        //static MethodInfo isDirty = null;

        static EdMultisceneHierarchy()
        {
            sceneIcon = EditorGUIUtility.FindTexture("SceneAsset Icon");//.Load("sceneIcon.png") as Texture;
            // Callback for rendering into the hierarchy window
            EditorApplication.hierarchyWindowItemOnGUI += HierarchyItemCB;

            // Callback to be notified when the hierarchy changes!
            EditorApplication.hierarchyWindowChanged += HierarchyChanged;


            //edSkin = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector);

            Undo.undoRedoPerformed += UndoPerformed;
            if (Application.platform == RuntimePlatform.OSXEditor)
            {
                sceneIconOffset = -6;
            }


            System.Reflection.Assembly assembly = typeof(UnityEditor.EditorWindow).Assembly;

            Type type = assembly.GetType("UnityEditor.MenuUtils");
            if (type != null)
            {
                //MethodInfo[] infos = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance);
                extractMenuItemWithPath = type.GetMethod("ExtractMenuItemWithPath", BindingFlags.Public | BindingFlags.Static);
                //createContextMenu = type.GetMethod("AddCreateGameObjectItemsToMenu", BindingFlags.NonPublic, null, new Type[] { typeof(GenericMenu), typeof(UnityEngine.Object[]), typeof(bool) }, null);
            }

            /*type = assembly.GetType("UnityEditor.EditorUtility");

            if (type != null)
            {
                isDirty = type.GetMethod("IsDirty", BindingFlags.NonPublic | BindingFlags.Static);
            }*/
            /*
            Type type = assembly.GetType("UnityEditor.SceneHierarchyWindow");
            if (type != null)
            {
                //MethodInfo[] infos = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance);
                createContextMenu = type.GetMethod("AddCreateGameObjectItemsToMenu", BindingFlags.NonPublic | BindingFlags.Instance);
                //createContextMenu = type.GetMethod("AddCreateGameObjectItemsToMenu", BindingFlags.NonPublic, null, new Type[] { typeof(GenericMenu), typeof(UnityEngine.Object[]), typeof(bool) }, null);
            }*/
            
            //EditorWindow gameview = EditorWindow.GetWindow(type);

        }

        static void CheckSelectionChange()
        {

        }

        static void UndoPerformed()
        {
            CheckAll();
            MultisceneCameras.Instance.RefreshList();
        }



        static void HierarchyChanged()
        {
            if (MultisceneAssemblyReload.skipNextHierarchyChangeDueToSort)
            {
//                Debug.Log("Skip Hierarchy changed");
                MultisceneAssemblyReload.skipNextHierarchyChangeDueToSort = false;
                return;
            }
//            Debug.Log("Hierarchy changed");
            Transform[] trans = Selection.GetTransforms(SelectionMode.TopLevel);
            foreach (Transform t in trans)
            {
                if (CheckRoot(t, true))
                {
                    break;
                }
                Camera c = t.GetComponent<Camera>();
                if(c != null)
                {
                    MultisceneCameras.Instance.RefreshIfNew(c);
                }
            }

            Multiscene currentScene = Multiscene.CurrentScene;
            if (currentScene != null && !Multiscene.GetSaveTriggeredByUs())
            {
                if (EditorApplication.currentScene.Length == 0)
                {
                    if (!currentScene.Path.StartsWith("Untitled"))
                    {
                        // User has created a new scene causing scene to change...

                        Multiscene.RemoveAll(false);
                        //PhiSceneAssemblyReload.Instance.Rename(currentScene, PhiSceneAssemblyReload.Instance.GetUntitledPath());
                    }
                }
                else if (EditorApplication.currentScene.CompareTo(currentScene.Path) != 0)
                {
                    // Scene has changed - via Open Scene...

                    // Now that we use GUIDs if the path did not match above I assume the scene changed 


                    /*
                    // Need to work out if it's due to a rename or open scene.
                    string newGUID = UnityEditor.AssetDatabase.AssetPathToGUID(currentScene.Path);
                    string prevID = currentScene.GUID;
                    if (newGUID.CompareTo(prevID)!=0)
                    {
                        // Just rename 
                        MultisceneAssemblyReload.Instance.Rename(currentScene, EditorApplication.currentScene);
                        currentScene.CheckName();
                    }
                    else*/
                    {

                        // so just get rid of any multiscenes - should have been saved due to main scene being dirty and saved by unity triggering other saves.
                        Multiscene.RemoveAll(false);
                        /*
                        PhiSceneLoadedDuringEdit matchingScene = (PhiSceneLoadedDuringEdit.GetIfExists(EditorApplication.currentScene));
                        if (matchingScene != null)
                        {
                            // User has opened a scene that we already have loaded.
                            bool keep = false;
                            if (matchingScene.IsDirty)
                            {
                                // And our local version is changed so does user want to save it?
                                keep = EditorUtility.DisplayDialog("Discard changes?", "You already have scene '"+matchingScene.name+"' loaded and edited do you wish to discard those changes?.", "Discard", "Keep changes");
                                if (keep)
                                {
                                    Debug.Log("Ans = " + keep);
                                }
                            }
                            if(!keep)
                            {
                                GameObject.DestroyImmediate(matchingScene.gameObject);
                            }
                        }
                        if (PhiSceneAssemblyReload.Instance.Heading != null)    // Check just incase we just deleted the only multi scene and no longer have any
                        {
                            PhiSceneAssemblyReload.Instance.Rename(currentScene, EditorApplication.currentScene);
                        }*/
                    }
                }
                if (MultisceneAssemblyReload.Instance.CurrentScene != null)    // Check just in case we just deleted the only multi scene and no longer have any
                {
                    currentScene.CheckName();
                }
            }

            if (MultisceneAssemblyReload.Instance.CurrentScene != null)    // Check just in case we just deleted the only multi scene and no longer have any
            {
                // Seems like a suitable place to catch the end of loading a new scene
                MultisceneAssemblyReload.Instance.SortInHeirarchy();

                MultisceneCameras.Instance.RefreshList();
            }
        }


        static bool HasFocus()
        {
            EditorWindow w = EditorWindow.focusedWindow;
            if (w == null) return false;

            string t = w.GetType().ToString();

#if UNITY_4_5 || UNITY_4_6
            return t == "UnityEditor.SceneHierarchyWindow";
#else
            return t =="UnityEditor.HierarchyWindow";
#endif
        }

        static void CheckAll()
        {
            object[] obj = Resources.FindObjectsOfTypeAll(typeof(Transform));
            foreach (object o in obj)
            {
                Transform t = o as Transform;
                if (t != null && t.gameObject != null)
                {
                    GameObject g = t.gameObject;
                    //if (g.transform.parent == null)
                    {
                        if (Multiscene.IsASceneObject(g))
                        {
                            if (CheckRoot(t, false))
                            {
                                return; // Performed an undo which will re trigger the check.
                            }
                        }
                    }
                }
            }
        }

        static bool CheckRoot(Transform t, bool checkChildren)
        {
            Multiscene rootScene = t.GetComponent<Multiscene>();
            if (rootScene != null)
            {
                // This is a loaded scene ensure it remains in the root!
                if (t.parent != null)
                {
                    Undo.PerformUndo();
                    return true;
                }
                else
                {
                    rootScene.CheckName();
                }
                return false;
            }
            if (t.parent == null)
            {
                // Root object so ensure has no hide flags
                if (t.gameObject.hideFlags != HideFlags.None)
                {
//                    Debug.Log("Check root (t.parent == null) " + t.name);
                    if (checkChildren)
                    {
                        Multiscene.FlagRecursively(t.gameObject, HideFlags.None);
                    }
                    else
                    {
                        t.gameObject.hideFlags = HideFlags.None;
                    }
                }
            }
            else
            {
                // Child object
                Transform root = t.root;
                Multiscene scene = root.GetComponent<Multiscene>();
                if (scene != null && scene.isCurrentScene)
                {
                    //Not allowed to add stuff here so move to root instead
                    t.parent = null;
                    //Undo.PerformUndo();
                    //return true;
                }
                else if (t.gameObject.hideFlags != root.gameObject.hideFlags)
                {
//                    Debug.Log("Check root " + t.gameObject.hideFlags + " " + t.root.gameObject.hideFlags + " " + t.name);
                    if (checkChildren)
                    {
                        Multiscene.FlagRecursively(t.gameObject, t.root.gameObject.hideFlags);
                    }
                    else
                    {
                        t.gameObject.hideFlags = t.root.gameObject.hideFlags;
                    }
                }
            }
            return false;
        }


        //static float sortOrderWidth = 12;
        static void HierarchyItemCB(int instanceID, Rect r)
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode) return;    // Speed up in play mode
#if !DEBUGMULTISCENE
            if (MultisceneAssemblyReload.Instance.loadedScenes.Count > 0)
#endif
            {
                GameObject go = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
                if (!go) return;

                /*      bool windowHasFocus = false;
                      EditorWindow window = EditorWindow.focusedWindow;
                      if (window != null)
                      {
              //            Debug.Log("Focus = " + window.GetType().ToString());
                          windowHasFocus = window.GetType().ToString().Contains("Hierarchy");
                      }
                      bool isSelected = Selection.Contains(go);*/
                int d = GUI.depth;
                GUI.depth = d + 10;
                Color restoreColor = GUI.color;

                //bool isPartOfLoadedScene = (go.hideFlags == HideFlags.DontSave) || (go.hideFlags == HideFlags.NotEditable);

                Multiscene scene = go.transform.root.GetComponent<Multiscene>();    // Could look up on our list of scenes if that's faster.            
#if !DEBUGMULTISCENE
                if (scene != null)
#endif
                {
                    // Ensure styles are setup, this is the only safe place I've found to do this.
                    if (styles == null)
                    {
                        styles = new Styles();
                    }

                    Rect fullRow = r;
                    //float indent = fullRow.x;
                    fullRow.xMin = 0;

                    if (go.transform.parent == null)
                    {
                        if (scene != null)
                        {
                            // This is a scene header item               

                            styles.UpdateSkin();
                            if (scene.isCurrentScene)
                            {
                                // Render bold title
                                Rect boldRect = fullRow;
                                Styles.ColorIDs id = Styles.ColorIDs.BGNormal;
                                bool on = false;
                                if (Selection.Contains(go))
                                {
                                    on = true;
                                    id = Styles.ColorIDs.BGHighFocus;
                                }
                                if (!on)
                                {
                                    // Need to clear background
                                    GUI.color = styles.GetColor(id);
                                    boldRect.xMin += 16;
                                    GUI.DrawTexture(boldRect, EditorGUIUtility.whiteTexture, ScaleMode.StretchToFill);
                                    boldRect.xMin -= 16;
                                }
                                boldRect.yMax += 1;
                                GUI.color = Color.white;
                                if (Event.current.type == EventType.repaint)
                                {
                                    boldRect.xMin += sceneIconOffset;
                                    styles.boldWhite.Draw(boldRect, new GUIContent(go.name), false, false, on, HasFocus());
                                    boldRect.xMin -= 12 + sceneIconOffset;
                                    boldRect.xMax = boldRect.xMin + 32;
                                    styles.boldWhite.Draw(boldRect, new GUIContent("-"), false, false, on, HasFocus());
                                }
                            }

                            // Line seperator
                            GUI.color = styles.GetColor(Styles.ColorIDs.Seperator);
                            Rect seperator = fullRow;
                            seperator.yMax = seperator.yMin + 1;
                            GUI.DrawTexture(seperator, EditorGUIUtility.whiteTexture, ScaleMode.StretchToFill);

                            // Scene Icon
                            float left = r.xMin + sceneIconOffset;
                            r.width = 20;
                            r.xMin = left;
                            r.yMin += 1;
                            GUI.color = Color.white;
                            GUI.DrawTexture(r, sceneIcon, ScaleMode.ScaleToFit);

                            // Get ready for buttons on the right
                            Rect button = fullRow;
                            button.x = button.width - loadButtonWidth;
                            button.width = loadButtonWidth;

                            // Save S
                            bool canSave = scene.IsDirty;
                            Color color2 = Color.white;
                            color2.a = !canSave ? 0.3f : 0.9f;
                            GUI.color = color2;
                            if (GUI.Button(button, new GUIContent("S", canSave ? "Save scene." : ""), styles.bold))
                            {
                                // First try to save our scene...
                                SaveScene(scene);
                            }
                            GUI.color = restoreColor;

                            // Non current scene specific icons
                            if (!scene.isCurrentScene)
                            {
                                // Padlock
                                button.x -= button.width + 4;
                                button.yMin += 2;
                                bool newLock = GUI.Toggle(button, scene.Locked, new GUIContent(string.Empty, "Lock Scene from editing"), styles.lockButton);
                                if (newLock != scene.Locked)
                                {
                                    LockUnlockScene(scene, newLock);
                                }
                                button.y += 2;

                                // Visibility eye
                                button.x -= button.width;
                                bool visible = scene.gameObject.activeSelf;
                                color2 = restoreColor;
                                color2.a = !visible ? 0.4f : 0.6f;
                                GUI.color = color2;
                                GUIContent content = new GUIContent(string.Empty, !visible ? styles.visibleOff : styles.visibleOn, "Show/Hide Scene");
                                bool newVisible = GUI.Toggle(button, visible, content, GUIStyle.none);
                                if (newVisible != visible)
                                {
                                    Undo.RecordObject(scene.gameObject, "Change scene visibility");
                                    changingVisibility = true;
                                    EditorApplication.delayCall += ClearChangingVisibility;
                                    scene.gameObject.SetActive(newVisible);
                                }
                            }

                            ConsiderClicks(fullRow, scene);
                        }
                    }
                    else
                    {
#if DEBUGMULTISCENE
                        // Get ready for buttons on the right
                        Rect button = fullRow;
                        button.x = button.width - loadButtonWidth;
                        button.width = loadButtonWidth;

                        // Visualize hideFlags
                        if(go.hideFlags != HideFlags.None)
                        {
                            if (GUI.Button(button, new GUIContent(go.hideFlags == HideFlags.DontSave ? "D" : "N"), styles.bold))
                            {
                                go.hideFlags = HideFlags.None;
                            }
                        }
#endif
                    }
                }
                GUI.color = restoreColor;
                GUI.depth = d;
            }

            bool canDrag = false;
            if (!Application.isPlaying)
            {
                if (Event.current.type == EventType.DragUpdated || Event.current.type == EventType.DragPerform)
                {
                    if (stopDrag)
                    {
                        if (Event.current.type == EventType.DragPerform)
                        {
                            stopDrag = false;
                        }
                        Event.current.Use();
                    }
                    else
                    {
                        if (Event.current.type == EventType.DragPerform)
                        {
                            draggedScenes.Clear();
                        }
                        foreach (UnityEngine.Object obj2 in DragAndDrop.objectReferences)
                        {
                            if (obj2 != null && obj2.ToString().EndsWith("Asset)"))
                            {
                                canDrag = true;
                                if (Event.current.type == EventType.DragPerform)
                                {
                                    draggedScenes.Add(AssetDatabase.GetAssetPath(obj2));
                                }
                            }
                        }
                        if (canDrag)
                        {
                            DragAndDrop.visualMode = DragAndDropVisualMode.Link;
                            if (Event.current.type == EventType.DragPerform)
                            {
                                LoadScenes(draggedScenes);
                            }
                            Event.current.Use();
                        }
                    }
                }
                else if (Event.current.type == EventType.mouseDown || Event.current.type == EventType.DragExited)
                {
                    stopDrag = false;
                }
            }
        }

        const float loadButtonWidth = 12;
        //static Color seperatorCol = new Color(0, 0, 0, 0.65f);
        //    static Color sceneContentsCol = new Color(0, 0, 0, 0.2f);
        //    static Color heading = new Color(0, 0, 0, 0.2f);
        //static Color red = new Color(1, 0, 0, 1);
        static List<string> draggedScenes = new List<string>();
        static Styles styles;

        private class Styles
        {
            public readonly GUIStyle lockButton = "IN LockButton";
            public Texture2D visibleOff;// = EditorGUIUtility.LoadIcon("animationvisibilitytoggleoff");
            public Texture2D visibleOn;// = EditorGUIUtility.LoadIcon("animationvisibilitytoggleon");
            public readonly GUIStyle bold = "HeaderLabel";
            public readonly GUIStyle boldWhite = "Hi Label";

            public bool cachedSkin;
            public enum ColorIDs
            {
                BGNormal,
                BGHighFocus,
                BGHighUnFocus,
                FGNormal,
                FGOn,
                Seperator
            }
            public Color[] darkSkin = { new Color(56 / 256.0f, 56 / 256.0f, 56 / 256.0f, 1), new Color(62 / 256.0f, 95 / 256.0f, 149 / 256.0f, 1), new Color(72 / 256.0f, 72 / 256.0f, 72 / 256.0f, 1), new Color(256.0f / 256.0f, 256.0f / 256.0f, 256.0f / 256.0f, 1), Color.white, new Color(89 / 256.0f, 89 / 256.0f, 89 / 256.0f, 1) };
            public Color[] lightSkin = { new Color(193 / 256.0f, 193 / 256.0f, 193 / 256.0f, 1), new Color(62 / 256.0f, 95 / 256.0f, 149 / 256.0f, 1), new Color(72 / 256.0f, 72 / 256.0f, 72 / 256.0f, 1), new Color(0, 0, 0, 1), new Color(.918f, .918f, .918f, 1), new Color(115 / 256.0f, 115 / 256.0f, 115 / 256.0f, 1) };
            public Color GetColor(ColorIDs id)
            {
                if (EditorGUIUtility.isProSkin)
                {
                    return darkSkin[(int)id];
                }
                return lightSkin[(int)id];
            }

            public void UpdateSkin()
            {
                if (EditorGUIUtility.isProSkin != cachedSkin)
                {
                    //Color color = new Color(.691f, .691f, .691f, 1);
                    Color color = GetColor(ColorIDs.FGNormal);
                    Color colorh = GetColor(ColorIDs.FGOn);
                    boldWhite.normal.textColor = color;
                    boldWhite.focused.textColor = color;
                    boldWhite.hover.textColor = color;
                    boldWhite.active.textColor = color;
                    boldWhite.onNormal.textColor = colorh;
                    boldWhite.onHover.textColor = colorh;
                    boldWhite.onActive.textColor = colorh;
                    boldWhite.onFocused.textColor = colorh;
                    cachedSkin = EditorGUIUtility.isProSkin;
                }
            }

            public Styles()
            {
                System.Type type = typeof(EditorGUIUtility);
                MethodInfo methodInfo = type.GetMethod("LoadIcon", BindingFlags.Static | BindingFlags.NonPublic);
                //            Debug.Log("methodInfo=" + methodInfo);
                visibleOn = (Texture2D)methodInfo.Invoke(null, new System.Object[1] { "animationvisibilitytoggleon" });
                visibleOff = (Texture2D)methodInfo.Invoke(null, new System.Object[1] { "animationvisibilitytoggleoff" });
                boldWhite = new GUIStyle((GUIStyle)"Hi Label");
                boldWhite.fontStyle = FontStyle.Bold;
                cachedSkin = !EditorGUIUtility.isProSkin;
                UpdateSkin();
            }
        }

        static void EditSortLayer(Rect r, GameObject go)
        {
        }


        static void ClearChangingVisibility()
        {
            changingVisibility = false;
        }

        static bool stopDrag = false;  // Set to true if we detect the start of a drag of an object we do not want dragging
        static void ConsiderClicks(Rect fullRow, Multiscene scene)
        {
            if (fullRow.Contains(Event.current.mousePosition))
            {
                if (scene != null)
                {
                    Event e = Event.current;
                    if (e.type == EventType.mouseDown)
                    {
                        if (e.button == 0) // left click
                        {
                            if (e.clickCount == 2)
                            {
                                scene.ReplaceSceneWithThis(true);
                            }
                        }
                        else if (e.button == 1)
                        {
                            var menu = new GenericMenu();

                            if (!scene.isCurrentScene)
                            {
                                menu.AddItem(new GUIContent("Set as active scene"), false, SetActiveScene, scene);
                                menu.AddItem(new GUIContent("Remove scene"), false, RemoveScene, scene);
                            }
                            menu.AddItem(new GUIContent("Save scene"), false, SaveScene, scene);
                            menu.AddItem(new GUIContent("Save All Scenes"), false, SaveAllScenes, scene);
                            if (scene.gameObject.activeSelf)
                            {
                                menu.AddItem(new GUIContent("Hide"), false, HideScenes, scene);
                            }
                            else
                            {
                                menu.AddItem(new GUIContent("Unhide"), false, UnhideScenes, scene);
                            }
                            if (scene.Locked)
                            {
                                menu.AddItem(new GUIContent("Unlock"), false, UnlockScenes, scene);
                            }
                            else
                            {
                                menu.AddItem(new GUIContent("Lock"), false, LockScenes, scene);
                            }
                            menu.AddItem(new GUIContent("New Scene"), false, NewScene, scene);
                            if (scene.HasFile)
                            {
                                menu.AddItem(new GUIContent("Revert to file"), false, RevertToFile, scene);
                            }
                            menu.AddSeparator("");
                            menu.AddItem(new GUIContent("Paste"), false, Paste, scene);

                            AddCreateGameObjectItemsToMenu(menu, Selection.objects);
                            /*
                            if (createContextMenu != null)
                            {
                                menu.AddSeparator("");
                                createContextMenu.Invoke(EditorWindow.focusedWindow, new object[] { menu, Selection.objects, false });
                            }*/
                            /*
                            menu.AddSeparator("");
                            string[] normalMenu = Unsupported.GetSubmenusIncludingSeparators("GameObject");
                            //[MenuItem("GameObject/MyMenu/Do Something", false, 0)]
                            string[] commands = Unsupported.GetSubmenusCommands("GameObject");
                            string[] normalMenu2 = Unsupported.GetSubmenusIncludingSeparators("Component");
                            //[MenuItem("GameObject/MyMenu/Do Something", false, 0)]
                            string[] commands2 = Unsupported.GetSubmenusCommands("Component");

                            for (int i = 0; i < 5; i++)
                            {
                                menu.AddItem(new GUIContent(normalMenu[i]), false, ContextMenu, normalMenu[i]);
                            }
                            */
                            if (Application.platform == RuntimePlatform.OSXEditor)
                            {
                                Rect r = new Rect(Event.current.mousePosition.x + 1, Event.current.mousePosition.y - 13, 0f, 0f);
                                menu.DropDown(r);
                            }
                            else
                            {
                                menu.ShowAsContext();
                            }




                            Event.current.Use();
                        }
                    }
                    if (e.type == EventType.mouseDrag)
                    {
                        stopDrag = true;
//                        Debug.Log("Stop drag");
                        Event.current.Use(); // Stop user dragging scenes around TODO: Allow but only to root locations.
                    }
                }
            }


        }

        static void Paste(object data)
        {
            Multiscene scene = data as Multiscene;
            List<Transform> alreadyLoaded = null;
            if (!scene.isCurrentScene)  // No need for current scene as that's already handled
            {
                alreadyLoaded = Multiscene.ListExistingObjects();
            }
            Unsupported.PasteGameObjectsFromPasteboard();
            if (!scene.isCurrentScene)
            {
                scene.MakeNewObjectsChildren(alreadyLoaded);
            }
        }

        static void ContextMenu(object data)
        {
            string command = data as string;
            EditorApplication.ExecuteMenuItem(command);
        }


        static void RevertToFile(object data)
        {
            Multiscene scene = data as Multiscene;
            if (scene != null)
            {
                scene.Reload();
            }
        }

        static void NewScene(object data)
        {
            Multiscene.NewScene();
        }

        public static void SetActiveScene(object data)
        {
            Multiscene scene = data as Multiscene;
            if (scene != null)
            {
                scene.ReplaceSceneWithThis(true);
            }
        }

        static void HideScenes(object o)
        {
            HideScenes(true);
        }
        static void UnhideScenes(object o)
        {
            HideScenes(false);
        }
        static void HideScenes(bool hide)
        {
            Transform[] transforms = Selection.GetTransforms(SelectionMode.TopLevel);
            foreach (Transform t in transforms)
            {
                if (t.parent == null)
                {
                    Undo.RecordObject(t.gameObject, "Change scene visibility");
                    t.gameObject.SetActive(!hide);
                }
            }
        }
        static void LockScenes(object o)
        {
            LockUnlockScenes(true);
        }

        static void UnlockScenes(object o)
        {
            LockUnlockScenes(false);
        }

        static bool warnedOnceThisCall = false; // Flag used during setting of more than one object to lock to avoid opening warning more than once
        static void LockUnlockScenes(bool lockScene)
        {
            warnedOnceThisCall = false;
            Transform[] transforms = Selection.GetTransforms(SelectionMode.TopLevel);
            foreach (Transform t in transforms)
            {
                if (t.parent == null)
                {
                    Multiscene scene = t.GetComponent<Multiscene>();
                    DoLockUnlockScene(scene, lockScene);
                }
            }
        }

        static void LockUnlockScene(Multiscene scene, bool lockScene)
        {
            warnedOnceThisCall = false;
            DoLockUnlockScene(scene, lockScene);
        }

        // Does the unlock / lock an warns as long as we have not already warned.
        // Do not call this instead use LockUnlockScene, LockScene or UnlockScene
        static void DoLockUnlockScene(Multiscene scene, bool lockScene)
        {
            if (scene != null)
            {
                if (scene.Locked != lockScene)
                {
                    Undo.RecordObject(scene, "Change scene lock state");
                    scene.Locked = lockScene;
                    if (lockScene && !warnedOnceThisCall)
                    {
                        //EditorPrefs.SetBool("WarnedUserVSA", false);
/*                        if (lockScene && EditorPrefs.GetBool("VerifySavingAssets", false) && !EditorPrefs.GetBool("WarnedUserVSA", false))
                        {
                            bool ans = EditorUtility.DisplayDialog("Warning", "When Unity's 'Verify Saving Assets' option is enabled Locked scenes will be lost if you switch scene via the File->Open menu. Opening scenes from the project view is supported and safe.", "Do not warn me again", "Ok");
//                            Debug.Log("Ans = " + ans);
                            if (ans)
                            {
                                EditorPrefs.SetBool("WarnedUserVSA", true);
                            }
                        }*/
                        warnedOnceThisCall = Multiscene.WarnLockAndQuit();
                    }
                }
            }
        }


        static void RemoveScene(object data)
        {
            Multiscene scene = data as Multiscene;
            Undo.DestroyObjectImmediate(scene.gameObject);
        }

        static void SaveScene(object data)
        {
            Multiscene scene = data as Multiscene;
            List<Multiscene> saveUs = new List<Multiscene>();
            saveUs.Add(scene);
            Multiscene.Save(saveUs);
        }
        public static void SaveAllScenes(object data = null)
        {
            List<Multiscene> saveUs = new List<Multiscene>(MultisceneAssemblyReload.Instance.loadedScenes.Values);
            Multiscene.Save(saveUs);
        }
        static void LoadScenes(List<string> scenes)
        {
/*            if (MultisceneAssemblyReload.Instance.loadedScenes.Count == 0)
            {
                // We will be adding a heading so work out if current scene is dirty or not.        
                MultisceneAssemblyReload.Instance.sceneWasDirtyBeforeAdd = true;
                if(isDirty != null)
                {
                    if(EditorApplication.currentScene.Length > 0)
                    {
                        UnityEngine.Object o = AssetDatabase.LoadAssetAtPath(EditorApplication.currentScene, typeof(UnityEngine.Object));
                        int id = o.GetInstanceID();

                        
                        MultisceneAssemblyReload.Instance.sceneWasDirtyBeforeAdd = (bool) isDirty.Invoke(null, new object[] {id});
                    }
                }
                
            }*/
            foreach (string s in scenes)
            {
                Multiscene.Load(s);
            }
        }

        static private void AddCreateGameObjectItemsToMenu(GenericMenu menu, UnityEngine.Object[] context)
        {
            if (extractMenuItemWithPath == null) return;
            menu.AddSeparator("");
            foreach (string str in Unsupported.GetSubmenus("GameObject"))
            {
                UnityEngine.Object[] temporaryContext = context;
                string name = str.Substring(11);
                if (str.StartsWith("GameObject/Create Empty",StringComparison.InvariantCultureIgnoreCase))
                {
                    if (str.EndsWith(" Child", StringComparison.InvariantCultureIgnoreCase))
                    {
                        name = "Create Empty";
                    }
                    else
                    {
                        // Ignore
                        continue;
                    }
                }
                if (str.EndsWith("..."))
                {
                    temporaryContext = null;
                }
                if (str.ToLower() == "GameObject/Center On Children".ToLower())
                {
                    return;
                }
                extractMenuItemWithPath.Invoke(null, new object[] { str, menu, name, temporaryContext});
            }
        }

    }

#if false
    private void HierarchyView()
    {
        int num5;
        if ((Event.current.type == EventType.MouseDown) || (Event.current.type == EventType.KeyDown))
        {
            this.m_StillWantsNameEditing = false;
        }
        bool hasFocus = base.m_Parent.hasFocus;
        Hashtable hashtable = new Hashtable();
        foreach (int num in Selection.instanceIDs)
        {
            hashtable.Add(num, null);
        }
        IHierarchyProperty newHierarchyProperty = this.GetNewHierarchyProperty();
        float top = 0f;
        int num4 = 0;
        num4 = newHierarchyProperty.CountRemaining(this.m_ExpandedArray);
        newHierarchyProperty.Reset();
        Rect viewRect = new Rect(0f, 0f, 1f, num4 * m_RowHeight);
        if (this.m_EditNameMode == NameEditMode.PreImportNaming)
        {
            viewRect.height += m_RowHeight;
        }
        if (base.hasSearchFilter)
        {
            this.m_ScrollPositionFiltered = GUI.BeginScrollView(this.m_ScreenRect, this.m_ScrollPositionFiltered, viewRect);
            num5 = Mathf.RoundToInt(this.m_ScrollPositionFiltered.y) / Mathf.RoundToInt(m_RowHeight);
        }
        else
        {
            this.m_ScrollPosition = GUI.BeginScrollView(this.m_ScreenRect, this.m_ScrollPosition, viewRect);
            num5 = Mathf.RoundToInt(this.m_ScrollPosition.y) / Mathf.RoundToInt(m_RowHeight);
        }
        this.EditName();
        if ((Event.current.type == EventType.ExecuteCommand) || (Event.current.type == EventType.ValidateCommand))
        {
            this.ExecuteCommandGUI();
            if (Event.current.type == EventType.ValidateCommand)
            {
                GUI.EndScrollView();
                return;
            }
        }
        this.KeyboardGUI();
        if (Event.current.type == EventType.Layout)
        {
            GUI.EndScrollView();
        }
        else
        {
            int num6 = this.ControlIDForProperty(null);
            bool flag2 = false;
            top = num5 * m_RowHeight;
            float num7 = (this.m_ScreenRect.height + top) + 16f;
            newHierarchyProperty.Skip(num5, this.m_ExpandedArray);
            bool flag3 = false;
            Rect position = new Rect(0f, 0f, 0f, 0f);
            Vector2 mousePosition = Event.current.mousePosition;
            GUIContent content = new GUIContent();
            Event current = Event.current;
            int activeInstanceID = Selection.activeInstanceID;
            if (!this.NamingAsset())
            {
                this.m_EditNameMode = NameEditMode.None;
            }
            GUIStyle labelStyle = this.labelStyle;
            Color[] colorArray = !EditorGUIUtility.isProSkin ? s_HierarchyColors : s_DarkColors;
            while (newHierarchyProperty.Next(this.m_ExpandedArray) && (top <= num7))
            {
                Rect rect3 = new Rect(0f, top, GUIClip.visibleRect.width, m_RowHeight);
                if (((this.m_EditNameMode == NameEditMode.PreImportNaming) && !flag2) && (newHierarchyProperty.instanceID == this.m_NewAssetSortInstanceID))
                {
                    flag2 = true;
                    top += m_RowHeight;
                    rect3 = new Rect(0f, top, GUIClip.visibleRect.width, m_RowHeight);
                    this.DrawPreImportedIcon(top);
                }
                int instanceID = newHierarchyProperty.instanceID;
                int controlID = this.ControlIDForProperty(newHierarchyProperty);
                float num11 = 17f + (16f * newHierarchyProperty.depth);
                if (((current.type == EventType.MouseUp) || (current.type == EventType.KeyDown)) && ((activeInstanceID == instanceID) && (Selection.instanceIDs.Length == 1)))
                {
                    this.m_NameEditString = newHierarchyProperty.name;
                    this.m_NameEditRect = new Rect((rect3.x + num11) + this.IconWidth, rect3.y, rect3.width - num11, rect3.height);
                    this.m_EditNameInstanceID = instanceID;
                    if ((this.m_EditNameMode == NameEditMode.None) && newHierarchyProperty.isMainRepresentation)
                    {
                        this.m_EditNameMode = NameEditMode.Found;
                    }
                }
                if ((this.m_EditNameMode == NameEditMode.Renaming) && (this.m_EditNameInstanceID == instanceID))
                {
                    this.m_NameEditRect = new Rect((rect3.x + num11) + this.IconWidth, rect3.y, rect3.width - num11, rect3.height);
                }
                if ((current.type == EventType.ContextClick) && rect3.Contains(current.mousePosition))
                {
                    this.m_NameEditRect = new Rect((rect3.x + num11) + this.IconWidth, rect3.y, rect3.width - num11, rect3.height);
                    this.m_EditNameInstanceID = instanceID;
                    this.m_NameEditString = newHierarchyProperty.name;
                    if ((this.m_EditNameMode == NameEditMode.None) && newHierarchyProperty.isMainRepresentation)
                    {
                        this.m_EditNameMode = NameEditMode.Found;
                    }
                }
                if (Event.current.type == EventType.Repaint)
                {
                    if (base.m_HierarchyType == HierarchyType.GameObjects)
                    {
                        int colorCode = newHierarchyProperty.colorCode;
                        Color color = colorArray[colorCode & 3];
                        Color color2 = colorArray[(colorCode & 3) + 4];
                        if (colorCode >= 4)
                        {
                            color.a = color2.a = 0.6f;
                        }
                        else
                        {
                            color.a = color2.a = 1f;
                        }
                        labelStyle.normal.textColor = color;
                        labelStyle.focused.textColor = color;
                        labelStyle.hover.textColor = color;
                        labelStyle.active.textColor = color;
                        labelStyle.onNormal.textColor = color2;
                        labelStyle.onHover.textColor = color2;
                        labelStyle.onActive.textColor = color2;
                        labelStyle.onFocused.textColor = color2;
                    }
                    bool flag4 = (this.m_DropData != null) && (this.m_DropData.dropPreviousControlID == controlID);
                    bool isHover = (this.m_DropData != null) && (this.m_DropData.dropOnControlID == controlID);
                    content.text = newHierarchyProperty.name;
                    content.image = newHierarchyProperty.icon;
                    labelStyle.padding.left = (int) num11;
                    bool flag6 = this.m_CurrentDragSelectionIDs.Contains(instanceID);
                    bool on = (((this.m_CurrentDragSelectionIDs.Count == 0) && hashtable.Contains(instanceID)) || flag6) || (((flag6 && (current.control || current.shift)) && hashtable.Contains(instanceID)) || ((flag6 && hashtable.Contains(instanceID)) && hashtable.Contains(this.m_CurrentDragSelectionIDs)));
                    if ((this.m_EditNameMode == NameEditMode.Renaming) && (instanceID == this.m_EditNameInstanceID))
                    {
                        content.text = string.Empty;
                        on = false;
                    }
                    labelStyle.Draw(rect3, content, isHover, isHover, on, hasFocus);
                    if (flag4)
                    {
                        flag3 = true;
                        position = new Rect(rect3.x + num11, rect3.y - m_RowHeight, rect3.width - num11, rect3.height);
                    }
                }
                Rect drawRect = rect3;
                drawRect.x += num11;
                drawRect.width -= num11;
                if (base.m_HierarchyType == HierarchyType.Assets)
                {
                    ProjectHooks.OnProjectWindowItem(newHierarchyProperty.guid, drawRect);
                    if (EditorApplication.projectWindowItemOnGUI != null)
                    {
                        EditorApplication.projectWindowItemOnGUI(newHierarchyProperty.guid, drawRect);
                    }
                }
                if ((base.m_HierarchyType == HierarchyType.GameObjects) && (EditorApplication.hierarchyWindowItemOnGUI != null))
                {
                    EditorApplication.hierarchyWindowItemOnGUI(newHierarchyProperty.instanceID, drawRect);
                }
                if (newHierarchyProperty.hasChildren && !base.hasSearchFilter)
                {
                    bool flag8 = newHierarchyProperty.IsExpanded(this.m_ExpandedArray);
                    GUI.changed = false;
                    Rect rect5 = new Rect((17f + (16f * newHierarchyProperty.depth)) - 14f, top, 14f, m_RowHeight);
                    flag8 = GUI.Toggle(rect5, flag8, GUIContent.none, ms_Styles.foldout);
                    if (GUI.changed)
                    {
                        this.EndNameEditing();
                        if (Event.current.alt)
                        {
                            this.SetExpandedRecurse(instanceID, flag8);
                        }
                        else
                        {
                            this.SetExpanded(instanceID, flag8);
                        }
                    }
                }
                if (((current.type == EventType.MouseDown) && (Event.current.button == 0)) && rect3.Contains(Event.current.mousePosition))
                {
                    if (Event.current.clickCount == 2)
                    {
                        AssetDatabase.OpenAsset(instanceID);
                        if ((base.m_HierarchyType != HierarchyType.Assets) && (SceneView.lastActiveSceneView != null))
                        {
                            SceneView.lastActiveSceneView.FrameSelected();
                        }
                        GUIUtility.ExitGUI();
                    }
                    else
                    {
                        this.EndNameEditing();
                        this.m_CurrentDragSelectionIDs = this.GetSelection(newHierarchyProperty, true);
                        GUIUtility.hotControl = controlID;
                        GUIUtility.keyboardControl = 0;
                        DragAndDropDelay stateObject = (DragAndDropDelay) GUIUtility.GetStateObject(typeof(DragAndDropDelay), controlID);
                        stateObject.mouseDownPosition = Event.current.mousePosition;
                    }
                    current.Use();
                }
                else if ((current.type == EventType.MouseDrag) && (GUIUtility.hotControl == controlID))
                {
                    DragAndDropDelay delay2 = (DragAndDropDelay) GUIUtility.GetStateObject(typeof(DragAndDropDelay), controlID);
                    if (delay2.CanStartDrag())
                    {
                        this.StartDrag(newHierarchyProperty);
                        GUIUtility.hotControl = 0;
                    }
                    current.Use();
                }
                else if ((current.type == EventType.MouseUp) && (GUIUtility.hotControl == controlID))
                {
                    if (rect3.Contains(current.mousePosition))
                    {
                        if ((newHierarchyProperty.isMainRepresentation && (Selection.activeInstanceID == newHierarchyProperty.instanceID)) && (((Time.realtimeSinceStartup - this.m_FocusTime) > 0.5f) && !EditorGUIUtility.HasHolddownKeyModifiers(current)))
                        {
                            this.m_StillWantsNameEditing = true;
                            EditorApplication.CallDelayed(new EditorApplication.CallbackFunction(this.BeginMouseEditing), 0.5f);
                        }
                        else
                        {
                            this.SelectionClick(newHierarchyProperty);
                        }
                        GUIUtility.hotControl = 0;
                    }
                    this.m_CurrentDragSelectionIDs.Clear();
                    current.Use();
                }
                else if ((current.type == EventType.ContextClick) && rect3.Contains(current.mousePosition))
                {
                    current.Use();
                    if (base.m_HierarchyType == HierarchyType.GameObjects)
                    {
                        <HierarchyView>c__AnonStorey5C storeyc = new <HierarchyView>c__AnonStorey5C();
                        this.SelectionClickContextMenu(newHierarchyProperty);
                        GenericMenu menu = new GenericMenu();
                        menu.AddItem(EditorGUIUtility.TextContent("HierarchyPopupCopy"), false, new GenericMenu.MenuFunction(this.CopyGO));
                        menu.AddItem(EditorGUIUtility.TextContent("HierarchyPopupPaste"), false, new GenericMenu.MenuFunction(this.PasteGO));
                        menu.AddSeparator(string.Empty);
                        if ((this.m_EditNameMode == NameEditMode.Found) && !base.hasSearchFilter)
                        {
                            menu.AddItem(EditorGUIUtility.TextContent("HierarchyPopupRename"), false, new GenericMenu.MenuFunction2(this.RenameGO), newHierarchyProperty.pptrValue);
                        }
                        else
                        {
                            menu.AddDisabledItem(EditorGUIUtility.TextContent("HierarchyPopupRename"));
                        }
                        menu.AddItem(EditorGUIUtility.TextContent("HierarchyPopupDuplicate"), false, new GenericMenu.MenuFunction(this.DuplicateGO));
                        menu.AddItem(EditorGUIUtility.TextContent("HierarchyPopupDelete"), false, new GenericMenu.MenuFunction(this.DeleteGO));
                        menu.AddSeparator(string.Empty);
                        storeyc.prefab = PrefabUtility.GetPrefabParent(newHierarchyProperty.pptrValue);
                        if (storeyc.prefab != null)
                        {
                            menu.AddItem(EditorGUIUtility.TextContent("HierarchyPopupSelectPrefab"), false, new GenericMenu.MenuFunction(storeyc.<>m__DD));
                        }
                        else
                        {
                            menu.AddDisabledItem(EditorGUIUtility.TextContent("HierarchyPopupSelectPrefab"));
                        }
                        menu.ShowAsContext();
                    }
                }
                else if ((current.type == EventType.DragUpdated) || (current.type == EventType.DragPerform))
                {
                    Rect rect6 = rect3;
                    rect6.yMin -= m_DropNextToAreaHeight * 2f;
                    if (rect6.Contains(mousePosition))
                    {
                        if ((mousePosition.y - rect3.y) < (m_DropNextToAreaHeight * 0.5f))
                        {
                            this.DragElement(newHierarchyProperty, false);
                        }
                        else
                        {
                            this.DragElement(newHierarchyProperty, true);
                        }
                        GUIUtility.hotControl = 0;
                    }
                }
                top += m_RowHeight;
            }
            if (flag3)
            {
                GUIStyle insertion = ms_Styles.insertion;
                if (current.type == EventType.Repaint)
                {
                    insertion.Draw(position, false, false, false, false);
                }
            }
            if ((this.m_EditNameMode == NameEditMode.PreImportNaming) && (this.m_NewAssetSortInstanceID == 0))
            {
                this.DrawPreImportedIcon(top + 16f);
            }
            this.HandlePing();
            GUI.EndScrollView();
            switch (current.type)
            {
                case EventType.DragUpdated:
                    if (!(base.m_SearchFilter == string.Empty))
                    {
                        if (this.m_DropData == null)
                        {
                            this.m_DropData = new DropData();
                        }
                        this.m_DropData.dropOnControlID = 0;
                        this.m_DropData.dropPreviousControlID = 0;
                        return;
                    }
                    this.DragElement(null, true);
                    return;

                case EventType.DragPerform:
                    this.m_CurrentDragSelectionIDs.Clear();
                    this.DragElement(null, true);
                    return;

                case EventType.DragExited:
                    this.m_CurrentDragSelectionIDs.Clear();
                    this.DragCleanup(true);
                    return;

                case EventType.ContextClick:
                    if ((base.m_HierarchyType == HierarchyType.Assets) && this.m_ScreenRect.Contains(current.mousePosition))
                    {
                        EditorUtility.DisplayPopupMenu(new Rect(current.mousePosition.x, current.mousePosition.y, 0f, 0f), "Assets/", null);
                        current.Use();
                    }
                    return;

                case EventType.MouseDown:
                    if ((current.button == 0) && this.m_ScreenRect.Contains(current.mousePosition))
                    {
                        GUIUtility.hotControl = num6;
                        Selection.activeObject = null;
                        this.EndNameEditing();
                        current.Use();
                    }
                    break;

                case EventType.MouseUp:
                    if (GUIUtility.hotControl == num6)
                    {
                        GUIUtility.hotControl = 0;
                        current.Use();
                    }
                    break;

                case EventType.KeyDown:
                    if ((this.m_EditNameMode == NameEditMode.Found) && ((((current.keyCode == KeyCode.Return) || (current.keyCode == KeyCode.KeypadEnter)) && (Application.platform == RuntimePlatform.OSXEditor)) || ((current.keyCode == KeyCode.F2) && (Application.platform == RuntimePlatform.WindowsEditor))))
                    {
                        this.BeginNameEditing(Selection.activeInstanceID);
                        current.Use();
                    }
                    break;
            }
        }
    }
#endif
}
#endif
