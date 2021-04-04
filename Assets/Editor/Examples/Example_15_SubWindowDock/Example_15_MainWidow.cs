using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace ToolKits
{
    public class Example_15_MainWidow : EditorWindow
    {
        private static Example_15_MainWidow _window;
        private static readonly Vector2 MIN_SIZE = new Vector2(400, 300);
        private const string KEY = "__EXAPLE_15_DOCK_LAYOUT_";
        private const string PATH = "Assets/Editor/Examples/Example_15_SubWindowDock/layout.wlt";

        private bool isLayoutByCode = false;
        private Example_15_SubWindowA _windowA;
        private Example_15_SubWindowB _windowB;

        [MenuItem("Tools/SubWindowDock", priority = 15)]
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
            _windowA = Example_15_SubWindowA.PopUp();
            _windowB = Example_15_SubWindowB.PopUp();
            if (isLayoutByCode)
            {
                LoadLayoutByCode();
            }
        }

        private async void LoadLayoutByCode()
        {
            await Task.Delay(100);
            LayoutUtility.DockEditorWindow(_window, _windowB);
            await Task.Delay(100);
            LayoutUtility.DockEditorWindow(_window, _windowA);
        }

        private void OnGUI()
        {
            EditorGUI.BeginChangeCheck();
            isLayoutByCode = GUILayout.Toggle(isLayoutByCode, "是否通过代码实现布局");
            if (EditorGUI.EndChangeCheck())
            {
                EditorPrefs.SetBool(KEY, isLayoutByCode);
            }

            if (GUILayout.Button("加载布局"))
            {
                LayoutUtility.LoadLayoutFromAsset(PATH);
            }

            if (GUILayout.Button("保存布局"))
            {
                LayoutUtility.SaveLayoutToAsset(PATH);
                AssetDatabase.Refresh();
            }
        }
    }
}