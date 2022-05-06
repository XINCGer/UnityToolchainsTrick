using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UnityToolchinsTrick
{
    
    /// <summary>
    /// 文档
    /// https://www.jianshu.com/p/4b022fe9bffa
    /// https://blog.csdn.net/sunny__chen/article/details/51323265
    /// </summary>
    public class Example_74_GUISkinWindow : EditorWindow
    {
        private static Example_74_GUISkinWindow _window;
        private GUISkin _skin;
        private GUIStyle _btn_style;
        private GUIStyle _label_style;
        
        [MenuItem("Tools/Example_74_GUISkinWindow", priority = 74)]
        private static void PopUpWindow()
        {
            _window = GetWindow<Example_74_GUISkinWindow>("GUISkinWindow");
            _window.Init();
            _window.Show();
        }

        private void Init()
        {
            _skin = AssetDatabase.LoadAssetAtPath<GUISkin>(
                "Assets/Editor/Examples/Example_74_GUISkin/GUISkin01.guiskin");
            _btn_style = _skin.button;
            _label_style = _skin.label;
        }

        private void OnGUI()
        {
            GUILayout.Label("鼠标悬停在下面的button上可以看到测试效果",_label_style);
            if (GUILayout.Button("带有guiskin样式的按钮", _btn_style))
            {
            }
        }
    }
}