using AllTrickOverView.Core;

namespace AllTrickOverView.Examples
{
    public class Example_ShowButtonEditorWindow : AExample_Base
    {
        public static TrickOverViewInfo TrickOverViewInfo =
            new TrickOverViewInfo("ShowButtonEditorWindow",
                "在EditorWindowTab栏显示按钮",
                "EditorWindow",
                "using System.Collections;\nusing System.Collections.Generic;\nusing UnityEditor;\nusing UnityEngine;\n\nnamespace ToolKits\n{\n    public class Example_11_ShowButtonEditorWindow : EditorWindow\n    {\n        private static Example_11_ShowButtonEditorWindow _window;\n        private static readonly Vector2 MIN_SIZE = new Vector2(300, 200);\n\n        [MenuItem(\"Tools/ShowButtonWindow\", priority = 11)]\n        private static void PopUp()\n        {\n            _window = GetWindow<Example_11_ShowButtonEditorWindow>();\n            _window.minSize = MIN_SIZE;\n            _window.Show();\n        }\n\n        private void ShowButton(Rect rect)\n        {\n            if (GUI.Button(rect, EditorGUIUtility.IconContent(\"BuildSettings.Editor\")))\n            {\n                Application.OpenURL(\"https://www.baidu.com\");\n            }\n        }\n    }\n}",
                "Assets/Editor/Examples/Example_11_ShowButtonEditorWindow",
                typeof(Example_ShowButtonEditorWindow),
                picPath : "Assets/Editor/Examples/Example_11_ShowButtonEditorWindow/QQ截图20210419154434.png",
                videoPath : "");

        public override TrickOverViewInfo GetTrickOverViewInfo()
        {
            return TrickOverViewInfo;
        }
    }
}
