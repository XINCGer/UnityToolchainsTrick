using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ToolKits
{
    public class Example_55_Window : EditorWindow
    {
        private static Example_55_Window _window;
        private static readonly Vector2 MIN_SIZE = new Vector2(400, 400);

        private const string path =
            "Assets/Editor/Examples/Example_55_DrawInspectorOnEditWinow/Example_55_Scriptobj.asset";

        private Editor _editor;

        [MenuItem("Tools/DrawInspectorOnEditorWindow", priority = 55)]
        private static void PopUp()
        {
            _window = GetWindow<Example_55_Window>("");
            _window.minSize = MIN_SIZE;
            _window.maxSize = MIN_SIZE;
            _window.Init();
            _window.Show();
        }

        private void Init()
        {
            var asset = AssetDatabase.LoadAssetAtPath<Example_55_Scriptobj>(path);
            _editor = Editor.CreateEditor(asset);
        }

        private void OnGUI()
        {
            if (null != _editor)
            {
                _editor.OnInspectorGUI();
            }
        }
    }
}