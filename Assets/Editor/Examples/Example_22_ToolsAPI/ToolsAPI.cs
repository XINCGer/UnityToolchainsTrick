using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ToolKits
{
    public class ToolsAPI : EditorWindow
    {
        private static ViewTool _viewTool;

        [MenuItem("Tools/ToolsAPI", priority = 22)]
        private static void PopUp()
        {
            var _window = GetWindow<ToolsAPI>("ToolsAPI");
            SceneView.duringSceneGui -= OnSceneViewGUI;
            SceneView.duringSceneGui += OnSceneViewGUI;
            _window.Show();
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Tools.current"))
            {
                Debug.Log("Tools.current:" + Tools.current);
            }
        }

        private static void OnSceneViewGUI(SceneView view)
        {
            if (Tools.viewTool != _viewTool)
            {
                Debug.Log("Tools.viewTool:" + Tools.viewTool);
                _viewTool = Tools.viewTool;
            }
        }
    }
}