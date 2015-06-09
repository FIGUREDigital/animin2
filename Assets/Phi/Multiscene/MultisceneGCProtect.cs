#if UNITY_EDITOR && !UNITY_CLOUD_BUILD
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
namespace Phi
{
    // This class finds any objects that are not part of the scene and ensures they will survive a garbage collection.
    public class MultisceneGCProtect
    {
        static int[] hashes = {"SceneSettings".GetHashCode(), "HaloManager".GetHashCode(), "NavMeshSettings".GetHashCode()};
        static Dictionary<Object, HideFlags> hideFlags = new Dictionary<Object, HideFlags>();
        // Use this for initialization
        static public void Protect()
        {
            if (hideFlags.Count > 0)
            {
                Debug.LogError("Bug, please report!");
            }
            hideFlags.Clear();
            Object[] obj = Resources.FindObjectsOfTypeAll(typeof(Object));
            foreach (Object o in obj)
            {
                if (o.hideFlags != HideFlags.DontSave && o.hideFlags != HideFlags.HideAndDontSave)
                {
                    if (!EditorUtility.IsPersistent(o) || o is Component)
                    {
                        if (o is RenderSettings || o is LightmapSettings)
                        {
                            continue;
                        }
                        if(o.GetType().FullName == "UnityEngine.Object")
                        {
                            int hash = o.name.GetHashCode();
                            int i;
                            for(i = 0 ; i< hashes.Length; i++)
                            {
                                if (hash == hashes[i])
                                {
                                    break;
                                }
                            }
                            if(i < hashes.Length)
                            {
                                continue;
                            }
                        }
                        // We would lose this object if we did a load...
                        // So store current HideFlags
                        hideFlags.Add(o, o.hideFlags);

                        // And replace with one that survives a GC when a new scene is loaded
                        if (o.hideFlags == HideFlags.HideInHierarchy)
                        {
                            o.hideFlags = HideFlags.HideAndDontSave;
                        }
                        else
                        {
                            o.hideFlags = HideFlags.DontSave;
                        }
                    }
                }
            }
        }

        // Update is called once per frame
        static public void Unprotect()
        {
            foreach (KeyValuePair<Object, HideFlags> p in hideFlags)
            {
                // Restore HideFlags
                if (p.Key)
                {
                    p.Key.hideFlags = p.Value;
                }
            }
            hideFlags.Clear();
        }
    }
}
#endif
