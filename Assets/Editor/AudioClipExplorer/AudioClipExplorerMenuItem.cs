using UnityEngine;
using UnityEditor;

public static class AudioClipExplorerMenuItem
{
    [MenuItem("Window/AudioClip Explorer")]
    static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(AudioClipExplorer.MainWindow));
    }
}
