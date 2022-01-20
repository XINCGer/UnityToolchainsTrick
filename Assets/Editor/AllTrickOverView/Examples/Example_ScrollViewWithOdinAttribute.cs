using AllTrickOverView.Core;

namespace AllTrickOverView.Examples
{
    public class Example_ScrollViewWithOdinAttribute : AExample_Base
    {
        public static TrickOverViewInfo TrickOverViewInfo =
            new TrickOverViewInfo("ScrollViewWithOdinAttribute",
                "",
                "Draw",
                "using Sirenix.OdinInspector;\nusing Sirenix.OdinInspector.Editor;\nusing UnityEditor;\nusing UnityEngine;\n\nnamespace ToolKits\n{\n    public class Example_70_ScrollViewWithOdinAttribute : OdinEditorWindow\n    {\n        private static Example_70_ScrollViewWithOdinAttribute _window;\n\n        private Vector2 m_scrollPos;\n        \n        \n        [OnInspectorGUI(\"OnScrollBegin\",false)]\n        [LabelWidth(100f)]\n        [TextArea]\n        public string a;\n        \n        [LabelWidth(200f)]\n        [TextArea]\n        public string b;\n        \n        [TextArea]\n        [LabelWidth(200f)]\n        public string c;\n        \n        [LabelWidth(200f)]\n        [CustomValueDrawer(\"OnFloatDraw\")]\n        public float d;\n\n        [OnInspectorGUI(\"OnScrollEnd\")]\n        [TextArea]\n        [LabelWidth(200f)]\n        public string e;\n        \n        \n        private void OnScrollBegin()\n        {\n            m_scrollPos = EditorGUILayout.BeginScrollView(m_scrollPos, false,false, GUILayout.Height(100));\n            EditorGUILayout.BeginHorizontal();\n        }\n\n        private void OnScrollEnd()\n        {\n            EditorGUILayout.EndHorizontal();\n            EditorGUILayout.EndScrollView();\n        }\n\n        private float OnFloatDraw(float value, GUIContent label)\n        {\n            EditorGUILayout.BeginVertical();\n            EditorGUILayout.LabelField(label);\n            value = EditorGUILayout.FloatField(value);\n            EditorGUILayout.EndVertical();\n            return value;\n        }\n\n        [MenuItem(\"Tools/Example_70_ScrollViewWithOdinAttribute\", priority = 70)]\n        private static void Open()\n        {\n            _window = GetWindow<Example_70_ScrollViewWithOdinAttribute>();\n            _window.Show();\n        }\n    }\n}\n\n",
                "Assets/Editor/Examples/Example_70_ScrollViewWithOdinAttribute",
                typeof(Example_ScrollViewWithOdinAttribute),
                picPath : "Assets/Editor/Examples/Example_70_ScrollViewWithOdinAttribute/Preview.png",
                videoPath : "");

        public override TrickOverViewInfo GetTrickOverViewInfo()
        {
            return TrickOverViewInfo;
        }
    }
}
