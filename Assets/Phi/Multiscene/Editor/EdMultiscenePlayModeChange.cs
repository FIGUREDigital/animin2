#if !UNITY_CLOUD_BUILD
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace Phi
{
    [InitializeOnLoad]
    [System.Serializable]
    public static class EdMultiscenePlayModeChange
    {
        static EdMultiscenePlayModeChange()
        {
            EditorApplication.playmodeStateChanged += PlayModeChanged;
        }

        static void PlayModeChanged()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode && !EditorApplication.isPlaying)
            {
                Multiscene.StartPlayMode();
            }
            if (!EditorApplication.isPlayingOrWillChangePlaymode && !EditorApplication.isPlaying)
            {
                Multiscene.BackToEditMode();
            }
        }
    }
}
#endif
