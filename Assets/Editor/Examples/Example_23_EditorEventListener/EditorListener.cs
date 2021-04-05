using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ToolKits
{
    // [InitializeOnLoad]
    public class EditorListener
    {
        static EditorListener()
        {
            EditorApplication.update += Update;
            EditorApplication.hierarchyChanged += hierarchyChanged;
            EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemOnGUI;
            EditorApplication.projectChanged += projectChanged;
            EditorApplication.projectWindowItemOnGUI += ProjectWindowItemOnGUI;
            EditorApplication.playModeStateChanged += OnPlayerModeStateChanged;
        }

        public static void Update()
        {
        }

        public static void hierarchyChanged()
        {
            Debug.Log("hierarchyChanged");
        }

        public static void HierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
        {
        }

        public static void projectChanged()
        {
        }

        public static void ProjectWindowItemOnGUI(string guid, Rect selectionRect)
        {
        }

        private static void OnPlayerModeStateChanged(PlayModeStateChange playModeState)
        {
            Debug.LogFormat("state:{0} will:{1} isPlaying:{2}", playModeState,
                EditorApplication.isPlayingOrWillChangePlaymode, EditorApplication.isPlaying);
        }
    }
}