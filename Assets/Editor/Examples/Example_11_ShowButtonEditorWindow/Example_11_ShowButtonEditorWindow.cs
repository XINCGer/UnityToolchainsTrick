using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Example_11_ShowButtonEditorWindow : EditorWindow
{
    private static Example_11_ShowButtonEditorWindow _window;
    private static readonly Vector2 MIN_SIZE = new Vector2(300,200);

    [MenuItem("Tools/ShowButtonWindow", priority = 11)]
    private static void PopUp()
    {
        _window = GetWindow<Example_11_ShowButtonEditorWindow>();
        _window.minSize = MIN_SIZE;
        _window.Show();
    }

    private void ShowButton(Rect rect)
    {
        if (GUI.Button(rect, EditorGUIUtility.IconContent("BuildSettings.Editor")))
        {
            Application.OpenURL("https://www.baidu.com");
        }
    }
}
