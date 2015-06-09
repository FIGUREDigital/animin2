using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif
namespace Phi
{
    public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
    {
        [SerializeField]
        [HideInInspector]
        private bool editorModeSingleton = false;   // True if it was created whilst in edit mode allowing us to destroy / ignore it when it appears in play mode.
        static object SpinLock = new object();
        private static bool initialised = false;
        protected static T instance = null;
        [SerializeField]
        [HideInInspector]
        private bool autoCreatedInstance = false;	// Set to true if this instance was autocreated and should therfore not be saved.

        // Returns true if the instance already exists or false if it does not.
        static public bool Exists()
        {
            return instance != null;
        }
        private bool IgnoreThisInstance()
        {
            if (!instance.autoCreatedInstance) return true;
#if UNITY_EDITOR
            return (instance.editorModeSingleton && EditorApplication.isPlaying);
#else
            return false;
#endif
        }

        public static T Instance
        {
            get
            {
                // Instance required for the first time, we look for it
                lock (SpinLock)
                {
                    if (!initialised || instance == null)
                    {
                        UnityEngine.Object[] objs = Resources.FindObjectsOfTypeAll(typeof(T));
                        for (int i = 0; i < objs.Length; i++)
                        {
                            instance = objs[0] as T;
                            if (!instance.IgnoreThisInstance())
                            {
                                break;
                            }
                        }

                        // Object not found, we create a temporary one
                        if (instance == null || instance.IgnoreThisInstance())
                        {
                            try
                            {
                                //							Debug.LogWarning("Create instance " + typeof(T).ToString());
								string namePrefix = "[Play]";
#if UNITY_EDITOR
                                if (!EditorApplication.isPlaying)
                                {
                                    namePrefix = "[Edit]";
                                }
#endif
								instance = new GameObject(namePrefix + "Instance of " + typeof(T).ToString(), typeof(T)).GetComponent<T>();

                            }
                            catch (Exception e)
                            {
								Debug.LogException(e);
                                Debug.LogError("Problem during the creation of " + typeof(T).ToString() + " '" + e.Message + "'");
                            }

                            // Problem during the creation, this should not happen
                            if (!instance)
                            {
                                Debug.LogError("Problem during the creation of " + typeof(T).ToString());
                            }
                            else
                            {
                                instance.autoCreatedInstance = true;
                                instance.editorModeSingleton = false;

#if UNITY_EDITOR
                                if (!EditorApplication.isPlaying)
                                {
                                    instance.editorModeSingleton = true;
                                }
#endif
                              //  							Debug.LogWarning("Created "+ typeof(T).ToString()+" "+instance.name+" playing ="+EditorApplication.isPlaying+","+EditorApplication.isPlayingOrWillChangePlaymode);

                            }
                            if (instance && !initialised)
                            {
                                							//Debug.LogWarning("Init instance '" + instance.name + "' of type " + typeof(T).ToString());
                                instance.Init();
                                GameObject.DontDestroyOnLoad(instance);
                            }
                        }
                        else
                        {
                            						Debug.LogWarning("Found instance '"+instance.name+"' of type " + typeof(T).ToString());
                        }
                        if (instance && !initialised)
                        {
                            initialised = true;
                        }
                    }
                }
                return instance;
            }
        }

        public void EnsureInstanced()
        {
        }

        // If no other monobehaviour request the instance in an awake function
        // executing before this one, no need to search the object.
        private void Awake()
        {
//            		Debug.LogError("Awake " + typeof(T).ToString() + " " + name+" auroCreated = "+autoCreatedInstance);
            if (autoCreatedInstance)
            {
                if (!editorModeSingleton)
                {
                    				Debug.LogError("Awaking already initialized instance of " + typeof(T).ToString() + " " + name + " will destroy it.");
                    Destroy(gameObject);
                }
#if UNITY_EDITOR
                else if (EditorApplication.isPlaying)
                {
                    				Debug.Log("Awaking already editor instance in play mode. " + typeof(T).ToString() + " " + name + " will destroy it.");
                    Destroy(gameObject);
                }
#endif
            }
            else if (name.Length < 6 || name[0] != '[' || name[5] != ']')
            {
                Debug.LogError("Instance of " + typeof(T).ToString() + " '" + name + "' already exists in the scene but is not a scene singleton will destroy it.");
                Destroy(gameObject);
            }
            /*
            if (!autoCreatedInstance)
            {
                name = "[SingletonError]" + name; 
                Debug.LogError("IMPORTANT! You should not manually add an instance of " + typeof(T).ToString() + " to a component, please remove the one on gameObject " + name);
                return;
            }
//            lock (SpinLock)
            {
                if (!initialised && (!editorModeSingleton || !EditorApplication.isPlaying))
                {
                    initialised = true;
                    instance = this as T;
                    instance.Init();
                    Debug.LogWarning("Awake instance of " + typeof(T).ToString() + " " +name);
                }
                else
                {
                    if (!editorModeSingleton)
                    {
                        Debug.LogError("An instance of " + typeof(T).ToString() + " already exists, new one attached to gameObject " + name + " will be removed from the game Object.");
                    }
                    Destroy(gameObject);
                }
            }*/
        }
        /*
        void OnDisable()
        {
            Debug.LogWarning("Disable instance of " + typeof(T).ToString() + " " + name);
        }


        void OnEnable()
        {
            Debug.LogWarning("Enable instance of " + typeof(T).ToString() + " " + name);
        }
         */
        void OnPlayModeChange()
        {
            /*		Debug.LogWarning("Playmode change instance of " + typeof(T).ToString() + " " + name);
                    if (autoCreatedInstance)
                    {
                        DestroyImmediate(gameObject);
                    }*/
        }

        void OnDestroy()
        {
            lock (SpinLock)
            {
                if (instance == this as T)
                {
                    //				Debug.LogWarning("Destroy instance " + gameObject.name + " of type "+typeof(T).ToString());
                    initialised = false;
                    instance = null;
                }
                else
                {
                    //				Debug.Log("Destroy " + gameObject.name + " of type " + typeof(T).ToString());
                }
            }
        }

        // This function is called when the instance is used the first time
        // Put all the initializations you need here, as you would do in Awake
        public abstract void Init();

#if UNITY_EDITOR
        // Remove from scene if about to save to avoid it being saved in the scene.
        public void OnSave()
        {
            if (autoCreatedInstance)
            {
                DestroyImmediate(gameObject);
            }
        }
#endif

    }
}
