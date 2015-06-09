#if UNITY_EDITOR && !UNITY_CLOUD_BUILD
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;
namespace Phi
{
    public class MultisceneCameras : ScriptableObject, ISerializationCallbackReceiver
    {
        // Won't be serialized!
        Dictionary<Camera, CameraControl> cameras = new Dictionary<Camera, CameraControl>();

        //[HideInInspector]
        [SerializeField]
        List<CameraControl> cameraList = new List<CameraControl>();

        //    private PhiSceneLoadedDuringEdit scene;

        [Serializable]
        class CameraControl
        {
            public CameraControl(Camera c)
            {
                camera = c;
            }

            public Camera camera;
            public bool temporaryDisable = false;
            [HideInInspector]
            public bool temporaryDisabled = false;  // Set to true when we have set the enabled flag
            public void Set(bool enabled)
            {
                if (enabled)
                {
                    if (temporaryDisabled)
                    {
                        camera.enabled = true;
                        temporaryDisabled = false;
                    }
                }
                else
                {
                    temporaryDisabled |= (camera.enabled == true);
                    camera.enabled = false;
                }
                temporaryDisable = !enabled;
            }
        }


        static MultisceneCameras instance = null;
        static public MultisceneCameras Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = CreateInstance<MultisceneCameras>();
                    instance.hideFlags = HideFlags.DontSave;
                }
                return instance;
            }
        }

        public void OnBeforeSerialize()
        {

        }
        public void OnAfterDeserialize()
        {
            cameras = new Dictionary<Camera, CameraControl>();
            foreach (CameraControl c in cameraList)
            {
                cameras.Add(c.camera, c);
            }
        }
        public void OnEnable()
        {
            RefreshList();
        }

        void OnDestroy()
        {
            ReadyForSave();
        }
        
        // This removes the re-enables any temporary disabled cameras back to the state they were originally in
        // if keepRemoved is true then the local flag of whether or not the camera should be disabled is also cleared.
        public void ReadyForSave(bool keepRemoved = false)
        {
            foreach (CameraControl cc in cameraList)
            {
                if (cc.temporaryDisabled && cc.camera && cc.camera.enabled == false)
                {
                    cc.camera.enabled = true;
                    cc.temporaryDisabled = false;
                }
                if (keepRemoved)
                {
                    cc.temporaryDisabled = false;
                }
            }
        }
        
        public void ReApply()
        {
            foreach (CameraControl cc in cameraList)
            {
                if (cc.temporaryDisable)
                {
                    if (cc.camera.enabled != false)
                    {
                        cc.camera.enabled = false;
                        cc.temporaryDisabled = true;
                    }
                }
                else
                {
                    if (cc.temporaryDisabled)
                    {
                        if (!cc.camera.enabled)
                        {
                            cc.camera.enabled = true;
                        }
                        cc.temporaryDisabled = false;
                    }
                }
            }
            UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
        }
        //Called when we spot new objects being created.
        public void RefreshIfNew(Camera c)
        {
            if (!cameras.ContainsKey(c))
            {
                RefreshList();
            }
        }
        // Recreate list of cameras whenever hierarchy changes but keep current settings for any that still exist
        public void RefreshList()
        {
            if (EditorApplication.isPlaying) return;
            if (MultisceneAssemblyReload.Instance.loadedScenes.Count < 2) return;
            // Get all in scene and remove any that are parts of PhiSceneLoadedDuringEdit
            List<Camera> cams = new List<Camera>(Resources.FindObjectsOfTypeAll<Camera>());
            int i = 0;
            // Remove any cameras that are no longer our children..
            i = 0;
            while (i < cameraList.Count)
            {
                Camera c = cameraList[i].camera;
                if (cams.Contains(c))
                {
                    i++;
                }
                else
                {
                    cameras.Remove(c);
                    cameraList.RemoveAt(i);
                }
            }
            // Add any new cameras
            foreach (Camera c in cams)
            {
                if (Multiscene.IsASceneObject(c.gameObject))
                {
                    if (!cameras.ContainsKey(c))
                    {
                        CameraControl cc = new CameraControl(c);
                        cameras.Add(c, cc);
                        cameraList.Add(cc);
                    }
                }
            }

            cameraList.Sort((a, b) =>
            {
                int result = a.camera.depth.CompareTo(b.camera.depth);
                if (result == 0)
                {
                    return a.camera.transform.root.GetSiblingIndex().CompareTo(b.camera.transform.root.GetSiblingIndex());
                }
                return result;
            });

            float lastDepth = float.MinValue;
            CameraControl currentEnabled = null;
            for (i = 0; i < cameraList.Count; i++)
            {
                CameraControl cc = cameraList[i];
                Camera c = cc.camera;

                if (c.depth == lastDepth && (currentEnabled.camera.enabled || currentEnabled.temporaryDisabled))
                {
                    // Same depth so do we want it active?
                    // check previous camera was it active?
                    if (currentEnabled.camera.enabled || currentEnabled.temporaryDisabled)
                    {
                        // Yes so do we out rank it or not? - ie are we the main scene?
                        Multiscene scene = Multiscene.Get(c.transform);
                        if (scene != null && scene.isCurrentScene && (cc.temporaryDisabled || c.enabled))
                        {
                            // Yes so we should be active!
                            // disable other camera
                            currentEnabled.Set(false);
                            currentEnabled = cc;
                            cc.Set(true);
                        }
                        else
                        {
                            // No so we should be disabled                        
                            cc.Set(false);
                        }
                    }
                }
                else
                {
                    if (currentEnabled != null)
                    {
                        currentEnabled.Set(true);
                    }
                    else
                    {
                        //cc.Set(true);
                    }
                    lastDepth = c.depth;
                    currentEnabled = cc;
                }
            }

            if (currentEnabled != null)
            {
                currentEnabled.Set(true);
            }
            UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
        }
    }
}

#endif
