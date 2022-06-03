using AllTrickOverView.Core;

namespace AllTrickOverView.Examples
{
    public class Example_FadeGroup : AExample_Base
    {
        public static TrickOverViewInfo TrickOverViewInfo =
            new TrickOverViewInfo("FadeGroup",
                "FadeGroup与AnimBool结合使用",
                "Draw",
                "using Sirenix.Utilities.Editor;\nusing UnityEngine;\nusing UnityEditor;\nusing UnityEditor.AnimatedValues;\n\nnamespace UnityToolchinsTrick\n{\n    /// <summary>\n    /// \n    /// https://blog.csdn.net/qq_42139931/article/details/120686951\n    /// https://blog.csdn.net/tom_221x/article/details/78475300\n    /// https://docs.unity3d.com/cn/2017.4/ScriptReference/EditorGUILayout.BeginFadeGroup.html\n    /// </summary>\n    public class Example_75_FadeGroupWindow : EditorWindow\n    {\n        private AnimBool _showExtraFields;\n        private string _string;\n        private Color _color = Color.white;\n        private int _number = 0;\n        private bool _foldout;\n\n        [MenuItem(\"Tools/Example_75_FadeGroup\", priority = 75)]\n        static void Init()\n        {\n            Example_75_FadeGroupWindow window =\n                (Example_75_FadeGroupWindow) EditorWindow.GetWindow(typeof(Example_75_FadeGroupWindow));\n            window.titleContent = new GUIContent(\"Example_75_FadeGroup\");\n        }\n\n        void OnEnable()\n        {\n            _showExtraFields = new AnimBool(true);\n            _showExtraFields.valueChanged.AddListener(Repaint);\n        }\n\n        void OnGUI()\n        {\n            _showExtraFields.target =\n                EditorGUILayout.Foldout(_showExtraFields.target, _showExtraFields.target ? \"折叠\" : \"展开\", true);\n\n            //Extra block that can be toggled on and off.\n            if (EditorGUILayout.BeginFadeGroup(_showExtraFields.faded))\n            {\n                EditorGUI.indentLevel++;\n                EditorGUILayout.PrefixLabel(\"Color\");\n                _color = EditorGUILayout.ColorField(_color);\n                EditorGUILayout.PrefixLabel(\"Text\");\n                _string = EditorGUILayout.TextField(_string);\n                EditorGUILayout.PrefixLabel(\"Number\");\n                _number = EditorGUILayout.IntSlider(_number, 0, 10);\n                EditorGUI.indentLevel--;\n            }\n\n            EditorGUILayout.EndFadeGroup();\n        }\n    }\n}",
                "Assets/Editor/Examples/Example_75_FadeGroup",
                typeof(Example_FadeGroup),
                picPath : "Assets/Editor/Examples/Example_75_FadeGroup/Preview.png",
                videoPath : "");

        public override TrickOverViewInfo GetTrickOverViewInfo()
        {
            return TrickOverViewInfo;
        }
    }
}
