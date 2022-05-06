using AllTrickOverView.Core;

namespace AllTrickOverView.Examples
{
    public class Example_GUISkin : AExample_Base
    {
        public static TrickOverViewInfo TrickOverViewInfo =
            new TrickOverViewInfo("GUISkin",
                "GUISkin用法",
                "Others",
                "using System.Collections;\nusing System.Collections.Generic;\nusing UnityEditor;\nusing UnityEngine;\n\nnamespace UnityToolchinsTrick\n{\n    \n    /// <summary>\n    /// 文档\n    /// https://www.jianshu.com/p/4b022fe9bffa\n    /// https://blog.csdn.net/sunny__chen/article/details/51323265\n    /// </summary>\n    public class Example_74_GUISkinWindow : EditorWindow\n    {\n        private static Example_74_GUISkinWindow _window;\n        private GUISkin _skin;\n        private GUIStyle _btn_style;\n        private GUIStyle _label_style;\n        \n        [MenuItem(\"Tools/Example_74_GUISkinWindow\", priority = 74)]\n        private static void PopUpWindow()\n        {\n            _window = GetWindow<Example_74_GUISkinWindow>(\"GUISkinWindow\");\n            _window.Init();\n            _window.Show();\n        }\n\n        private void Init()\n        {\n            _skin = AssetDatabase.LoadAssetAtPath<GUISkin>(\n                \"Assets/Editor/Examples/Example_74_GUISkin/GUISkin01.guiskin\");\n            _btn_style = _skin.button;\n            _label_style = _skin.label;\n        }\n\n        private void OnGUI()\n        {\n            GUILayout.Label(\"鼠标悬停在下面的button上可以看到测试效果\",_label_style);\n            if (GUILayout.Button(\"带有guiskin样式的按钮\", _btn_style))\n            {\n            }\n        }\n    }\n}",
                "Assets/Editor/Examples/Example_74_GUISkin",
                typeof(Example_GUISkin),
                picPath : "Assets/Editor/Examples/Example_74_GUISkin/preview.png",
                videoPath : "");

        public override TrickOverViewInfo GetTrickOverViewInfo()
        {
            return TrickOverViewInfo;
        }
    }
}
