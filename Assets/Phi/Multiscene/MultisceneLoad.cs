using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Phi
{
    [ExecuteInEditMode]
    public class MultisceneLoad : MonoBehaviour 
    {
        // Array of scenes this component should load
        public List<MultisceneLoadSettings> scenes = new List<MultisceneLoadSettings>();

        private bool isAwake = false; // Allows us to check whether the component has been awoken yet when we find it via Resources.FindObjectsOfTypeAll
	
	    // Use this for initialization
	    void Awake ()
        {
            DoEnable();
	    }	

	    void OnEnable()
        {
            if (!isAwake)
            {
                Debug.LogError("What scenario is this?");
            }
            DoEnable();
        }

        public void DoEnable()
	    {
		    isAwake = true;
            if (scenes != null)
            {
                for (int i = 0; i < scenes.Count; i++)
                {
                    if (scenes[i] != null)
                    {
                        scenes[i].Load();
                    }
                }
            }
	    }
        
#if UNITY_EDITOR
        void OnValidate()
        {
            if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode) return;
            // Check each setting to see if it has changed
            for (int i = 0; i < scenes.Count; i++)
            {
                scenes[i].Validate();
            }
            //Consider renaming gameobject to reflect contents
            /*if (scenes.Length == 1)
            {
                string sceneName = scenes[0].SceneName;
                if (sceneName.Length > 0)
                {
                    name = "Load " + sceneName;
                }
            }*/
        }
#endif

	    public bool IsAwake
        {
            get
            {
                return isAwake;
            }
        }

        public bool ShouldLoad(string path)
        {
            for (int i = scenes.Count - 1; i >= 0; i--)
            {
                if (scenes[i].ShouldLoad() && string.Compare(scenes[i].ScenePath, path, true)== 0)
                {
                    return true;
                }
            }
            return false;
        }
			
	    /*
	    void OnLevelWasLoaded()
	    {
		    Debug.Log("Level Loaded "+Application.loadedLevelName);
		    if (Application.loadedLevelName.CompareTo(loadedName) == 0)
		    {
			    loading = false;
		    }
		    PostLoadLevel();
	    }*/


    
        // Delete this component
        // If there are no more components on this game object and the gameobject has no children then delete the game object too
        // to avoid cluttering the scene with Load objects.
        void DestroyUs()
        {
            enabled = false;
#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
            {
                GameObject.DestroyImmediate(this);
            }
            else
#endif
            {
                Destroy(this);
            }
            // Now consider cleaning up and destroying this gameobject
            if (transform.childCount > 0) return;  // If we have child objects then don't destroy this game object.
            Component [] components = GetComponents<Component>();
            for(int i = 0; i < components.Length; i++)
            {
                Component c = components[i];
                MultisceneLoad msload = c as MultisceneLoad;
                if (msload)
                {
                    if (msload.enabled)
                    {
                        // We still have scenes to load 
                        return;
                    }
                }
                else
                {
                    if (!(c is Transform))
                    {
                        // Have some other component on this game object so better not destroy it
                        return;
                    }
                }
            }
            // If we got here then there are no components that we need to keep on this game object and no child objects
            // so we can destroy the game object

#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
            {
                GameObject.DestroyImmediate(gameObject);
            }
            else
#endif
            {
                Destroy(gameObject);
            }
        }
    }
}
