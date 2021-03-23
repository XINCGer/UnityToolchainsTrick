using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Example05Window : EditorWindow
{
    private static Example05Window _window;
    private static readonly Vector2 MIN_SIZE = new Vector2(300, 200);
    private const string PATH = "";

    private string Name;
    private int Age;
    private ExampleScriptableObject setting;

    [MenuItem("Tools/资源保存测试窗口",priority = 5)]
    public static void PopUp()
    {
        _window = GetWindow<Example05Window>("Example05Window");
        _window.minSize = _window.maxSize = MIN_SIZE;
        _window.Init();
        _window.Show();
    }

    private void Init()
    {
        setting =
            EditorHelper.GetScriptableObjectAsset<ExampleScriptableObject>(
                "Assets/Editor/Examples/Example_05_SaveAssets/ExampleAssets.asset");
        Name = setting.Name;
        Age = setting.Age;
    }
    
    private void OnGUI()
    {
        var orginColor = GUI.color;
        GUI.color = Color.green;
        EditorGUI.BeginChangeCheck();
        Name = EditorGUILayout.TextField("姓名", Name);
        if (EditorGUI.EndChangeCheck())
        {
            setting.Name = Name;
        }
        EditorGUI.BeginChangeCheck();
        
        GUI.color = Color.yellow;
        Age = EditorGUILayout.IntField("年龄", Age);
        if (EditorGUI.EndChangeCheck())
        {
            setting.Age = Age;
        }
        
        GUI.color = Color.red;
        if (GUILayout.Button("保存"))
        {
            EditorUtility.SetDirty(setting);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        GUI.color = orginColor;
    }
}