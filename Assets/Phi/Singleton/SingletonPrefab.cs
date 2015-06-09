using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Phi
{
    /// <summary>
    /// This component should be added to a game object / prefab which occurs in multiple scenes
    ///  but which you only want to exist once when scenes are loaded additively.
    /// An example is using it for a GUI camera where you want to test individual scenes but when they are all loaded into game you only want one camera to exist
    /// </summary>
    public class SingletonPrefab : MonoBehaviour
    {
        public string singletonName = "";

        private string registeredName = "";

#if UNITY_EDITOR
        void OnValidate()
        {
            if (singletonName != registeredName)
            {
                if (instances.ContainsKey(registeredName))
                {
                    if (instances[registeredName] == this)
                    {
						instances.Remove(registeredName);
						instances.Add(singletonName, this);
					}
                }
                registeredName = singletonName;
                //Debug.Log(singletonName);
                name = singletonName;
            }
        }
#endif

        public static SingletonPrefab GetInstance(string name)
        {
            if (instances.ContainsKey(name))
            {
                return instances[name];
            }
            return null;
        }

        public static Dictionary<string, SingletonPrefab> instances = new Dictionary<string, SingletonPrefab>();

        // Need this here so that the script gets treated with the same priority as other scripts that have an OnEnable
        void OnEnable()
        {
        }

        void Awake()
        {
            if (instances.ContainsKey(singletonName))
            {
                DestroyImmediate(gameObject);
            }
            else
            {
                instances.Add(singletonName, this);
            }
        }

        void OnDestroy()
        {
            if (instances[singletonName] == this)
            {
                instances.Remove(singletonName);
            }
        }
    }
}
