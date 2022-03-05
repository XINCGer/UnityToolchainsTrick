using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace ToolKits
{
    public class Example_70_ScrollViewWithOdinAttribute : OdinEditorWindow
    {
        private static Example_70_ScrollViewWithOdinAttribute _window;

        private Vector2 m_scrollPos;
        
        
        [OnInspectorGUI("OnScrollBegin",false)]
        [LabelWidth(100f)]
        [TextArea]
        public string a;
        
        [LabelWidth(200f)]
        [TextArea]
        public string b;
        
        [TextArea]
        [LabelWidth(200f)]
        public string c;
        
        [LabelWidth(200f)]
        [CustomValueDrawer("OnFloatDraw")]
        public float d;

        [OnInspectorGUI("OnScrollEnd")]
        [TextArea]
        [LabelWidth(200f)]
        public string e;
        
        
        private void OnScrollBegin()
        {
            m_scrollPos = EditorGUILayout.BeginScrollView(m_scrollPos, false,false, GUILayout.Height(100));
            EditorGUILayout.BeginHorizontal();
        }

        private void OnScrollEnd()
        {
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndScrollView();
        }

        private float OnFloatDraw(float value, GUIContent label)
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField(label);
            value = EditorGUILayout.FloatField(value);
            EditorGUILayout.EndVertical();
            return value;
        }

        [MenuItem("Tools/Example_70_ScrollViewWithOdinAttribute", priority = 70)]
        private static void Open()
        {
            _window = GetWindow<Example_70_ScrollViewWithOdinAttribute>();
            _window.Show();
        }
    }
}

