using UnityEngine;
namespace Phi
{
    public abstract class SingletonScene<T> : MonoBehaviour where T : SingletonScene<T>
    {
        static object SpinLock = new object();
        protected static bool initialised = false;
        private static T instance = null;
        public static bool Exists()
        {
            return (instance != null);
        }

        public static T Instance
        {
            get
            {
                // Instance required for the first time, we look for it
                lock (SpinLock)
                {
                    // Object not found, we create a temporary one
                    if (!initialised)
                    {
                        Object[] objs = Resources.FindObjectsOfTypeAll(typeof(T));
                        if (objs.Length > 0)
                        {
                            instance = objs[0] as T;
                        }
                        //instance = GameObject.FindObjectOfType(typeof(T)) as T;

                        // Object not found!
                        if (instance)
                        {
                            initialised = true;
                            instance.Init();
                        }
                        else
                        {
                            Debug.LogError("No instance of " + typeof(T).ToString() + ", use SingletonMonoBehaviour<T> instead if the class has no dependencies on data configured in the inspector.");
                        }
                    }
                }
                return instance;
            }
        }

        // If no other monobehaviour request the instance in an awake function
        // executing before this one, no need to search the object.
        private void Awake()
        {
            lock (SpinLock)
            {
                if (!initialised || instance == null)
                {
                    initialised = true;
                    instance = this as T;
                    instance.Init();
                }
                else if (instance != this)
                {
                    Debug.LogError("An instance of " + typeof(T).ToString() + " already exists you now have two instances. One " + name + " and " + instance.name);
                    //				DestroyImmediate(this);
                }
            }
        }

        /*
        void OnDisable()
        {
            if (instance == this as T)
            {
                instance = null;
            }
            initialised = false;
        }*/

        void OnDestroy()
        {
            lock (SpinLock)
            {
                if (instance == this as T)
                {
                    initialised = false;
                    instance = null;
                }
                else
                {
                    //				Debug.LogWarning("Destroy duplicate " + gameObject.name + typeof(T).ToString());
                }
            }
        }


        // This function is called when the instance is used the first time
        // Put all the initializations you need here, as you would do in Awake
        public abstract void Init();

        // Make sure the instance isn't referenced anymore when the user quit, just in case.
        private void OnApplicationQuit()
        {
            instance = null;
        }
    }
}
