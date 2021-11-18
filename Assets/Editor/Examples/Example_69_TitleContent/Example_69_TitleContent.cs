using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ToolKits
{
    public class Example_69_TitleContent : EditorWindow
    {
        private static Example_69_TitleContent _window;
        
        [MenuItem("Tools/Example_69_TitleContent", priority = 69)]
        private static void PopUp()
        {
            _window = GetWindow<Example_69_TitleContent>("TitleContent用法展示");
            var titleContent = new GUIContent();
            titleContent.image = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Editor/Textures/logo.png");
            _window.titleContent = titleContent;
            _window.Show();
        }
    }   
}
