using AllTrickOverView.Core;

namespace AllTrickOverView.Examples
{
    public class Example_TitleContent : AExample_Base
    {
        public static TrickOverViewInfo TrickOverViewInfo =
            new TrickOverViewInfo("TitleContent",
                "利用TitleContent替换EditorWindow的背景图",
                "EditorWindow",
                "using System.Collections;\nusing System.Collections.Generic;\nusing UnityEditor;\nusing UnityEngine;\n\nnamespace ToolKits\n{\n    public class Example_69_TitleContent : EditorWindow\n    {\n        private static Example_69_TitleContent _window;\n        \n        [MenuItem(\"Tools/Example_69_TitleContent\", priority = 69)]\n        private static void PopUp()\n        {\n            _window = GetWindow<Example_69_TitleContent>(\"TitleContent用法展示\");\n            var titleContent = new GUIContent();\n            titleContent.image = AssetDatabase.LoadAssetAtPath<Texture2D>(\"Assets/Editor/Textures/logo.png\");\n            _window.titleContent = titleContent;\n            _window.Show();\n        }\n    }   \n}",
                "Assets/Editor/Examples/Example_69_TitleContent",
                typeof(Example_TitleContent),
                picPath : "Assets/Editor/Examples/Example_69_TitleContent/preview.png",
                videoPath : "");

        public override TrickOverViewInfo GetTrickOverViewInfo()
        {
            return TrickOverViewInfo;
        }
    }
}
