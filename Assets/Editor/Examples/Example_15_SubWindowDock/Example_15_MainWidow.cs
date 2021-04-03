using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ToolKits
{
    public class Example_15_MainWidow :EditorWindow
    {
        private static Example_15_MainWidow _window;
        private static readonly Vector2 MIN_SIZE = new Vector2(400,300);
        private const string KEY = "__EXAPLE_15_DOCK_LAYOUT_";
        private bool isLayoutByCode = true;

        [MenuItem("Tools/SubWindowDock",priority = 15)]
        private static void PopUp()
        {
            _window = GetWindow<Example_15_MainWidow>("主界面");
            _window.minSize = MIN_SIZE;
            _window.Init();
            _window.Show();
        }

        private void Init()
        {
            isLayoutByCode = EditorPrefs.GetBool(KEY, true);
        }

        private void OnGUI()
        {
            EditorGUI.BeginChangeCheck();
            isLayoutByCode = GUILayout.Toggle(isLayoutByCode, "是否通过代码实现布局");
            if (EditorGUI.EndChangeCheck())
            {
                EditorPrefs.SetBool(KEY,isLayoutByCode);
            }
        }
    }   
}
