using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
namespace Phi
{
    [CustomPropertyDrawer(typeof(MultisceneLoadSettings))]
    public class EdMultisceneLoadSettings : PropertyDrawer
    {
        public static float toggleWidth = 16;
        public static float buttonWidth = 54;
        SerializedProperty curProperty;
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            label = EditorGUI.BeginProperty(position, label, property);
            Rect contentPosition = EditorGUI.PrefixLabel(position, label);
            contentPosition.width -= toggleWidth*2 + buttonWidth + 2;
            EditorGUI.indentLevel = 0;
            contentPosition.y += 1;

            SerializedProperty settings = property.FindPropertyRelative("scenePath");
            EdDelayedPopup.Popup(contentPosition, settings.stringValue, SelectSceneMenu, property);
            /*
            if (!EditorApplication.isPlayingOrWillChangePlaymode && sceneLoad.ScenePath.Length > 0)
            {
                Multiscene loadedScene = Multiscene.GetIfExists("Assets\\" + sceneLoad.ScenePath + ".unity");
                if (loadedScene == null || !loadedScene.IsLoaded)
                {
                    if (GUILayout.Button("Load Scene"))
                    {
                        Multiscene.Load(sceneLoad.ScenePath);
                    }
                }
                else
                {
                    if (GUILayout.Button("Unload Scene"))
                    {
                        loadedScene.UnloadLevel();
                    }
                }
            }*/
            contentPosition.x += contentPosition.width + 2;
            contentPosition.width = toggleWidth;
            contentPosition.y -= 1;
            SerializedProperty edit = property.FindPropertyRelative("loadInEditMode");
            edit.boolValue = GUI.Toggle(contentPosition, edit.boolValue, new GUIContent("", "Auto load in edit mode"));

            contentPosition.x += contentPosition.width;
            edit = property.FindPropertyRelative("loadInPlayMode");
            edit.boolValue = GUI.Toggle(contentPosition, edit.boolValue, new GUIContent("", "Auto load in play mode"));

            //EditorGUI.PropertyField(contentPosition, edit, new GUIContent("Edit"));
            contentPosition.x += contentPosition.width;
            contentPosition.width = buttonWidth;
            contentPosition.height -= 3;

            if (!EditorApplication.isPlayingOrWillChangePlaymode && settings.stringValue.Length > 0)
            {
                Multiscene loadedScene = Multiscene.GetIfExists("Assets\\" + settings.stringValue + ".unity");
                if (loadedScene == null || !loadedScene.IsLoaded)
                {
                    if (GUI.Button(contentPosition, "Load"))
                    {
                        Multiscene.Load(settings.stringValue);
                    }
                }
                else
                {
                    if (GUI.Button(contentPosition, "Unload"))
                    {
                        loadedScene.UnloadLevel();
                    }
                }
            }
            //EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("loadInPlayMode"), new GUIContent("Play"));
            //contentPosition.width /= 3f;
            //EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("color"), new GUIContent("C"));*/
            EditorGUI.EndProperty();
        }

        void SelectSceneMenu(object userData)
        {
            //MultisceneLoadSettings settings = curProperty.serializedObject.targetObject as MultisceneLoadSettings;
            //if (settings == null)
            //    return;

            curProperty = userData as SerializedProperty;

            SerializedProperty settings = curProperty.FindPropertyRelative("scenePath");
            SelectSceneMenu(settings.stringValue, Callback);
        }

        public static void SelectSceneMenu(string curPath, GenericMenu.MenuFunction2 callBack)
        {
            Debug.Log(Application.dataPath);
            int row = 0;
            int offset = 0;
            bool itemsAdded = false;
            GenericMenu menu = new GenericMenu();
            int entries = EditorBuildSettings.scenes.Length;
            //		List<string> paths = new List<string>();
            bool firstEnabled = true;
            bool firstDisabled = true;
            for (int i = 0; i < entries; i++)
            {
                EditorBuildSettingsScene scene = EditorBuildSettings.scenes[i];

                if (scene.path.CompareTo(EditorApplication.currentScene) != 0)
                {
                    string path = scene.path.Substring(7);  // Strip off assets/
                    path = path.Substring(0, path.Length - 6).Replace('/', '\\');
                    menu.AddItem(new GUIContent((scene.enabled ? "Build Settings/" : "Build Disabled/") + path), path.CompareTo(curPath) == 0, callBack, path);
                    itemsAdded = true;
                    if (scene.enabled)
                    {
                        if (firstEnabled)
                        {
                            row++;
                            firstEnabled = false;
                        }
                    }
                    else
                    {
                        if (firstDisabled)
                        {
                            row++;
                            firstDisabled = false;
                        }
                    }
                }
                //names[i] = PhiScene.GetNameFromPath(scene.path);
            }
            if (itemsAdded)
            {//
                menu.AddSeparator("");
                offset = 12;
            }
            string[] find = System.IO.Directory.GetFiles(Application.dataPath, "*.unity", System.IO.SearchOption.AllDirectories);
            List<string> sortPaths = new List<string>(find);
            sortPaths.Sort();
            int stripLen = Application.dataPath.Length + 1;
            string shortendCurrent = EditorApplication.currentScene;
            if (shortendCurrent.Length > 6)
            {
                shortendCurrent = shortendCurrent.Substring(7).Replace('/', '\\');
            }
            int currentRow = 0;
            foreach (string f in sortPaths)
            {
                string shortenedPath = f.Substring(stripLen).Replace('/', '\\');
                if (shortenedPath.CompareTo(shortendCurrent) != 0)
                {
                    shortenedPath = shortenedPath.Substring(0, shortenedPath.Length - 6);
                    bool isCurrent = shortenedPath.CompareTo(curPath) == 0;
                    menu.AddItem(new GUIContent(shortenedPath), isCurrent, callBack, shortenedPath);

                    if (isCurrent)
                    {
                        currentRow = row;
                    }
                    row++;
                }
            }

            // Now create the menu, add items and show it
            Debug.Log("BR = " + EdDelayedPopup.ButtonRect);

            Rect r = EdDelayedPopup.ButtonRect;//ButtonRect;
            if (Application.platform == RuntimePlatform.OSXEditor)
            {
                Vector3 c = r.center;
                c.y = (c.y - (currentRow * 0x10) - offset) - 20f;
                r.center = c;
            }
            menu.DropDown(r);

        }



        void Callback(object selected)
        {
            SerializedProperty settings = curProperty.FindPropertyRelative("scenePath");
            if (settings == null)
                return;
            settings.stringValue = selected as string;
            settings.serializedObject.ApplyModifiedProperties();
        }

    }
}
