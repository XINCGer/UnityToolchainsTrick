using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ToolKits
{
    public static class CreateMoreSceneView
    {
        [MenuItem("Tools/CreateMoreSceneView", priority = 16)]
        private static void PopUp()
        {
            var _window = ScriptableObject.CreateInstance<SceneView>();
            _window.Init();
            _window.Show();
        }

        private static void Init(this SceneView sceneView)
        {
            if (null != sceneView)
            {
                Debug.Log("SceneView Init!");
            }
        }
    }
}