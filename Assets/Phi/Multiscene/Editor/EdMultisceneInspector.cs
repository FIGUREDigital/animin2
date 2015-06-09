#if !UNITY_CLOUD_BUILD
using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Phi
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Multiscene))]
    public class EdMultisceneInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            if (targets.Length > 1)
            {
                // Multiple scenes selected
            }
            else
            {
                Multiscene scene = target as Multiscene;
                if (scene.isCurrentScene)
                {
                    EditorGUILayout.HelpBox("This is the active scene, any newly created gameobjects will be added to this.", MessageType.None, true);
                }
                else
                {
                    if (GUILayout.Button("Make Active Scene"))
                    {
                        EditorApplication.delayCall += MakeActive;
                    }
                }
                if (scene.HasFile)
                {
                    if (GUILayout.Button("Show in project view"))
                    {
                        Object o = AssetDatabase.LoadAssetAtPath(scene.Path, typeof(Object));
                        EditorGUIUtility.PingObject(o);
                    }
                }
            }
            if (GUILayout.Button("Save"))
            {
                EditorApplication.delayCall += Save;
            }
    
            EditorGUILayout.LabelField("Multiscene v" + Multiscene.version);
        }
        public void Save()
        {

            List<Multiscene> saveUs = new List<Multiscene>();
            foreach (Object o in targets)
            {
                Multiscene scene = o as Multiscene;
                if (scene != null)
                {
                    saveUs.Add(scene);
                }
            }
            if (saveUs.Count > 0)
            {
                Multiscene.Save(saveUs, true);
            }
        }

        public void MakeActive()
        {
            EdMultisceneHierarchy.SetActiveScene(target);
        }
    }

}
#endif
