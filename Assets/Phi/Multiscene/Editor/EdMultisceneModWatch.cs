#if !UNITY_CLOUD_BUILD
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;
namespace Phi
{
    // This class watches out and tracks when game objects are changed.
    [InitializeOnLoad]
    public class EdMultisceneModWatch {
        static EdMultisceneModWatch()
        {
            Undo.postprocessModifications += OnPostProcessModifications;
           // Undo.undoRedoPerformed += UndoPerformed;
        
            // We can try to spot asset deletion and creation by tracking selections
            // when creating an object it is usually selected and then a hierarchy change callback occurs
            // when deleting an object it's too late to find out about it, but we get a hierarchy change callback
            // so if we were to track the selection of objects and check if any previously selected were deleted
            // that may work. Alternative is to track all objects which sounds more cumbersome and possibly
            // performance affecting.
            // Can't currently find a callback for when selection changes, but we can probably use the hierarchy redraw or
            // check more frequently.

            // As we also set up this callback else where it may be better for performance to piggy back that
            // but for now keeping things simple and register another function
            EditorApplication.hierarchyWindowItemOnGUI += HierarchyItemCB;
            EditorApplication.hierarchyWindowChanged += HierarchyChanged;
            EditorApplication.update += OnUpdate;
        }



        
        static void HierarchyItemCB(int instanceID, Rect selectionRect)
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode) return;
            CheckSelectionChange(); 
	    }
        
        static int lastActiveNumComponents = 0;
        static bool lastActiveHadCamera = false;
        static Transform lastActiveTransform = null;
        static Transform[] lastSelected = new Transform[0];
        struct SelectedInfo
        {
            public Transform parent;
            public int siblingOrder;
        }
        static SelectedInfo[] lastSelectedInfo = new SelectedInfo[0];
        static bool changeRegistered = false;   // Set to true when we have spotted a selection change but not processed it yet.
        static bool hierarchyChanged = false;
        static void CheckSelectionChange()
        {
            if (changeRegistered) return;

            if (Selection.transforms.Length != lastSelected.Length || lastActiveTransform != Selection.activeTransform)
            {
//                Debug.Log("Selection change");
                EditorApplication.delayCall += FinishSelectionChange;
                changeRegistered = true;
            }
        }

        static void HierarchyChanged()
        {
//            Debug.Log("Hierarchy changed - SMod");
            hierarchyChanged = true;
            if (changeRegistered) return;
            changeRegistered = true;
            EditorApplication.delayCall += FinishSelectionChange;
        }

        static void CheckForMovedOrDeletedObjects()
        {
            for (int i = lastSelected.Length - 1; i >= 0; i--)
            {
                if (!lastSelected[i] || (lastSelectedInfo[i].parent != lastSelected[i].parent) || (lastSelectedInfo[i].siblingOrder != lastSelected[i].GetSiblingIndex()))
                {
                    Multiscene scene = Multiscene.Get(lastSelectedInfo[i].parent);
                    SetDirty(scene);
                    if (lastSelected[i])
                    {
                        Multiscene newScene = Multiscene.Get(lastSelected[i].parent);
                        if (newScene != scene)
                        {
                            SetDirty(newScene);
                        }
                    }
                    UpdateViewsIfNeeded();
                }
            }
        }

        static void CheckForNewObjects()
        {
            // Go through selection and see if they were in the original selection,
            // if not assume they have just been created!
            Transform[] newSelection = Selection.GetTransforms(SelectionMode.TopLevel);
            Dictionary<Transform, Transform> quickLookup = new Dictionary<Transform, Transform>();
            foreach(Transform t in lastSelected)
            {
                quickLookup.Add(t,t);
            }
            bool isDirty = false;
            foreach(Transform t in newSelection)
            {
                if (!quickLookup.ContainsKey(t))
                {
                    Multiscene scene = Multiscene.Get(t);
                    SetDirty(scene);
                    isDirty = true;
                }
            }
            if(isDirty)
            {
                UpdateViewsIfNeeded();
            }
        }

        public static void CheckForComponentChange()
        {
            bool isDirty = false;
            if (lastActiveTransform && !Multiscene.changingScenes)
            {
                int now = lastActiveTransform.GetComponents<Component>().Length;
                bool nowHaveCamera = lastActiveTransform.GetComponent<Camera>() != null;
                if (now != lastActiveNumComponents)
                {
                // Number of components changed
                    for (int i = lastSelected.Length - 1; i >= 0; i--)
                    {
                        if (lastSelected[i])
                        {
                            Multiscene scene = Multiscene.Get(lastSelected[i]);
                            SetDirty(scene);
                            isDirty = true;
                        }
                    }

                }
                if(nowHaveCamera != lastActiveHadCamera)
                {
                    lastActiveHadCamera = nowHaveCamera;
                    MultisceneCameras.Instance.RefreshList();
                }
                lastActiveNumComponents = now;
            }
            if(isDirty)
            {
                UpdateViewsIfNeeded();
            }
        }

        static void FinishSelectionChange()
        {
            //EditorApplication.update -= FinishSelectionChange;
            changeRegistered = false;

            // See which scenes have changed
            if (hierarchyChanged && !Multiscene.changingScenes)
            {
                CheckForMovedOrDeletedObjects();
                if (Selection.transforms.Length != lastSelected.Length || lastActiveTransform != Selection.activeTransform)
                {
                    CheckForNewObjects();
                }
            }
            hierarchyChanged = false;


            CheckForComponentChange();
            lastSelected = Selection.GetTransforms(SelectionMode.TopLevel);
            if (lastSelected.Length > lastSelectedInfo.Length)
            {
                lastSelectedInfo = new SelectedInfo[lastSelected.Length + 50];  // Claim an extra few to reduce number of times array is reclaimed
            }
            for (int i = lastSelected.Length - 1; i >= 0; i-- )
            {
                lastSelectedInfo[i].parent = lastSelected[i].parent;// PhiSceneLoadedDuringEdit.Get(lastSelected[i]);
                lastSelectedInfo[i].siblingOrder = lastSelected[i].GetSiblingIndex();
            }
            lastActiveTransform = Selection.activeTransform;
            if (lastActiveTransform)
            {
                lastActiveNumComponents = lastActiveTransform.GetComponents<Component>().Length;
                lastActiveHadCamera = lastActiveTransform.GetComponent<Camera>() != null;
            }
            if (Multiscene.changingScenes)
            {
                //Debug.Log("FinishSelectionChange Finish changingScenes");
                Multiscene.changingScenes = false;
            }
            //Debug.Log("Finish selection change");
            //Debug.Log("Selection overhead = " + duration);
        }

        // Use the undo callback to spot when objects are changed
        static UndoPropertyModification[] OnPostProcessModifications(UndoPropertyModification[] modifications)
        {
            foreach(UndoPropertyModification m in modifications)
            {
#if !UNITY_5_0 && !UNITY_4_6 && !UNITY_4_5
                GameObject go = m.currentValue.target as GameObject;
#else				
				GameObject go = m.propertyModification.target as GameObject;
#endif
                if (go != null)
                {					
#if !UNITY_5_0 && !UNITY_4_6 && !UNITY_4_5
					if (!EdMultisceneHierarchy.changingVisibility || m.currentValue.propertyPath.CompareTo("m_IsActive") != 0)
#else
					if (!EdMultisceneHierarchy.changingVisibility || m.propertyModification.propertyPath.CompareTo("m_IsActive") != 0)
#endif
                    {
                        SetDirty(go.transform);
                        UpdateViewsIfNeeded();
                    }
                }
                else
                {
					
					#if !UNITY_5_0 && !UNITY_4_6 && !UNITY_4_5
					Component c = m.currentValue.target as Component;
#else
					Component c = m.propertyModification.target as Component;
#endif
                    if (c != null && !(c is Multiscene))
                    {
                        SetDirty(c.transform);
                        Camera cam = c as Camera;
                        if(cam != null)
                        {
							
							#if !UNITY_5_0 && !UNITY_4_6 && !UNITY_4_5
							if (m.currentValue.propertyPath.CompareTo("m_Depth") == 0 || m.currentValue.propertyPath.CompareTo("m_Enabled") == 0)
								#else
								if (m.propertyModification.propertyPath.CompareTo("m_Depth") == 0 || m.propertyModification.propertyPath.CompareTo("m_Enabled") == 0)
#endif
                            {
                                MultisceneCameras.Instance.RefreshList();
                            }
                        }
                        UpdateViewsIfNeeded();
                    }
                }
            }
            return modifications;
        }

        static void UpdateViewsIfNeeded()
        {
#if USEDONTSAVE
            //redrawGameViewRequired = false;
            //            Debug.Log("Update all " + Time.realtimeSinceStartup);
            UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
            
            
            //System.Reflection.Assembly assembly = typeof(UnityEditor.EditorWindow).Assembly;
            //Type type = assembly.GetType("UnityEditor.GameView");
            //EditorWindow gameview = EditorWindow.GetWindow(type);
            //gameview.Repaint();
            //SceneView.RepaintAll();
            //SceneView.sceneViews
#endif
        }

        static void SetDirty(Multiscene scene)
        {
            if (scene)
            {
                scene.IsDirty = true;
            }
        }
        static void SetDirty(Transform t)
        {
            Multiscene scene = Multiscene.Get(t);
            if (scene != null)
            {
                scene.IsDirty = true;
            }
        }


        static float lastUpdate = 0;
        static void OnUpdate()
        {
            // Keep checking for a changing number of components but lazily every so often
            if (!Application.isPlaying && !EditorApplication.isPlayingOrWillChangePlaymode)
            {
                float now = Time.realtimeSinceStartup;
                if (Mathf.Abs(now - lastUpdate) > 0.5f) // Somehow goes backwards during editing of code.
                {
                    CheckForComponentChange();
                    lastUpdate = now;
                }
            }
        }

    }
}
#endif
