using UnityEditor;
using UnityEngine;
using System.Collections;
namespace Phi
{
    [CustomEditor(typeof(SingletonPrefab))]
    public class EdSingletonPrefab : Editor
    {
        public SerializedProperty nameProp;
        public void OnEnable()
        {
            if (target == null) return;
            nameProp = serializedObject.FindProperty("singletonName");
        }

        public override void OnInspectorGUI()
        {
            SingletonPrefab prefabSingleton = target as SingletonPrefab;
            if (prefabSingleton == null)
                return;


            EditorGUILayout.PropertyField(nameProp);

            serializedObject.ApplyModifiedProperties();

            if (PrefabUtility.GetPrefabType(prefabSingleton) == PrefabType.None)
            {
                EditorGUILayout.HelpBox("This object should be a prefab or instance of one to help ensure the settings are the same across scenes", MessageType.Warning, true);
            }
            else
            {
                PropertyModification[] mods = PrefabUtility.GetPropertyModifications(prefabSingleton);
                if (mods != null)
                {

                    foreach (PropertyModification mod in mods)
                    {
                        bool partOfTransform = false;
                        if (mod.propertyPath.StartsWith("m_Local"))
                        {
                            if (mod.propertyPath.StartsWith("m_LocalRotation") || mod.propertyPath.StartsWith("m_LocalPosition"))
                            {
                                partOfTransform = true;
                            }
                        }
                        if (!partOfTransform)
                        {
                            bool ignoreDifference = false;

                            if ((mod.propertyPath.CompareTo("m_RootMapOrder") == 0) || (mod.propertyPath.CompareTo("m_RootOrder") == 0))
                            {
                                ignoreDifference = true;
                            }
                            else if (mod.propertyPath.CompareTo("m_Name") == 0)
                            {
                                if (mod.value.CompareTo(mod.target.name) == 0)
                                {
                                    ignoreDifference = true;
                                }
                            }
                            if (!ignoreDifference)
                            {
                                EditorGUILayout.HelpBox("This object is different from the prefab (" + mod.propertyPath + " is changed), note as it's a singleton the settings from the first loaded instance will be used, they should all be the same", MessageType.Warning, true);
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}
