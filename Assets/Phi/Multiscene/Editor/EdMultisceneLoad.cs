using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditorInternal;
namespace Phi
{
    [CustomEditor(typeof(MultisceneLoad))]
    public class EdMultisceneLoad : Editor
    {
        ReorderableList roList;
        public void OnEnable()
        {

            SerializedProperty elements = serializedObject.FindProperty("scenes");
            roList = new ReorderableList(serializedObject, elements, true, true, true, true);
            roList.drawHeaderCallback = new ReorderableList.HeaderCallbackDelegate(this.DrawEventHeader);
            roList.drawElementCallback = new ReorderableList.ElementCallbackDelegate(this.DrawElement);
            roList.onAddDropdownCallback = new ReorderableList.AddDropdownCallbackDelegate(this.Popup);
        }

        void Popup (Rect buttonRect, ReorderableList l)
        {
            EdMultisceneLoadSettings.SelectSceneMenu("", Callback);
            //var menu = new GenericMenu();
            //menu.ShowAsContext();
        }

        void Callback(object selected)
        {
            MultisceneLoad sceneLoad = target as MultisceneLoad;
            sceneLoad.scenes.Add(new MultisceneLoadSettings(selected as string));
            /*
            SerializedProperty settings = curProperty.FindPropertyRelative("scenePath");
            if (settings == null)
                return;
            settings.stringValue = selected as string;
            settings.serializedObject.ApplyModifiedProperties();*/
        }
        public override void OnInspectorGUI()
        {
            MultisceneLoad sceneLoad = target as MultisceneLoad;
            if (sceneLoad == null)
                return;
            serializedObject.Update();
            GUILayout.Space(3);
            roList.DoLayoutList();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Load All"))
            {
                foreach(MultisceneLoadSettings load in sceneLoad.scenes)
                {
                    Multiscene.Load(load.ScenePath, false);
                }
            }
            if (GUILayout.Button("Unload All"))
            {
                foreach (MultisceneLoadSettings load in sceneLoad.scenes)
                {
                    Multiscene loadedScene = Multiscene.GetIfExists("Assets\\" + load.ScenePath + ".unity");
                    if (loadedScene != null)
                    {
                        loadedScene.UnloadLevel();
                    }
                }
            }
            GUILayout.EndHorizontal();

            //DrawDefaultInspector();

            /*
            string buttonPath = sceneLoad.ScenePath;
            if (buttonPath.Length == 0)
            {
                buttonPath = "Select scene...";
            }
            EdDelayedPopup.Popup("Load", buttonPath, SelectSceneMenu);
            if (!EditorApplication.isPlayingOrWillChangePlaymode && sceneLoad.ScenePath.Length > 0)
            {
                Multiscene loadedScene = Multiscene.GetIfExists("Assets\\"+sceneLoad.ScenePath+".unity");
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
            }
            */

            //        currentIndex = EditorGUILayout.Popup(currentIndex, names);
            /*
            Rect rect = EditorGUILayout.GetControlRect(false, 17);
            GUIStyle btn = new GUIStyle(GUI.skin.button);
            btn.margin = new RectOffset(1, 1, 1, 1);
            btn.padding = new RectOffset(1, 1, 1, 0);

            selAlignGrid_A = lineJustification_prop.enumValueIndex & ~12;
            selAlignGrid_B = (lineJustification_prop.enumValueIndex & ~3) / 4;

            GUI.Label(new Rect(rect.x, rect.y, 100, rect.height), "Alignment");
            float columnB = EditorGUIUtility.labelWidth + 15;
            selAlignGrid_A = GUI.SelectionGrid(new Rect(columnB, rect.y, 23 * 4, rect.height), selAlignGrid_A, alignContent_A, 4, btn);
            selAlignGrid_B = GUI.SelectionGrid(new Rect(columnB + 23 * 4 + 10, rect.y, 23 * 3, rect.height), selAlignGrid_B, alignContent_B, 3, btn);

            //_choiceIndex = EditorGUILayout.Popup(_choiceIndex, _choices);
            Event evt = Event.current;
            Rect contextRect = rect;
        
            if (evt.type == EventType.ContextClick)
            {
                Vector2 mousePos = evt.mousePosition;
                if (contextRect.Contains (mousePos))
                {
                    // Now create the menu, add items and show it
                    GenericMenu menu = new GenericMenu ();
            	
                    menu.AddItem (new GUIContent ("MenuItem1"), false, Callback, "item 1");
                    menu.AddItem (new GUIContent ("MenuItem2"), false, Callback, "item 2");
                    menu.AddSeparator ("");
                    menu.AddItem (new GUIContent ("SubMenu/MenuItem3"), false, Callback, "item 3");
				
                    menu.ShowAsContext ();
                    evt.Use();
                }
            }*/

            serializedObject.ApplyModifiedProperties();
        }
        /*
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            state = GetState(property);
            float height = 0f;
            if (state.list != null)
            {
                height = state.list.GetHeight();
            }
            return height;
        }*/

        public float toggleHeaderWidth = 40;
        protected virtual void DrawEventHeader(Rect headerRect)
        {
            Rect r = headerRect;
            r.width -= EdMultisceneLoadSettings.buttonWidth + EdMultisceneLoadSettings.toggleWidth + toggleHeaderWidth;
            GUI.Label(r, "Load Scenes");
            GUIStyle rightStyle = new GUIStyle(GUI.skin.GetStyle("Label"));
            rightStyle.alignment = TextAnchor.UpperRight;
            r.x += r.width;
            r.width = toggleHeaderWidth;
            GUI.Label(r, new GUIContent("Edit", "Load this when this scene is opened in edit mode"), rightStyle);
            r.x += r.width;
            GUI.Label(r, new GUIContent("Play", "Load this when this scene is opened in play mode"));

        }

        private void DrawElement(Rect rect, int index, bool isactive, bool isfocused)
        {
            SerializedProperty arrayElementAtIndex = roList.serializedProperty.GetArrayElementAtIndex(index);

            EditorGUI.PropertyField(rect, arrayElementAtIndex, GUIContent.none);
        }


        /*
        public void OnGUI(Rect position)
        {
            if ((this.m_ListenersArray != null) && this.m_ListenersArray.isArray)
            {
                this.m_DummyEvent = GetDummyEvent(this.m_Prop);
                if (this.m_DummyEvent != null)
                {
                    if (this.m_Styles == null)
                    {
                        this.m_Styles = new Styles();
                    }
                    if (this.m_ReorderableList != null)
                    {
                        int indentLevel = EditorGUI.indentLevel;
                        EditorGUI.indentLevel = 0;
                        this.m_ReorderableList.DoList(position);
                        EditorGUI.indentLevel = indentLevel;
                    }
                }
            }
        }*/
        SerializedProperty headerProperty;
        string headerText;
        /*
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            headerProperty = property.FindPropertyRelative("gameID");
            //this.m_Prop = property;
            headerText = label.text;
            state = this.GetState(property);
            int indentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            state.list.DoList(position);
            EditorGUI.indentLevel = indentLevel;


            //state.lastSelectedIndex = this.m_LastSelectedIndex;
        }*/
    }
}
