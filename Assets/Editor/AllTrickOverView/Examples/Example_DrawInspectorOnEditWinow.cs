using AllTrickOverView.Core;

namespace AllTrickOverView.Examples
{
    public class Example_DrawInspectorOnEditWinow : AExample_Base
    {
        public static TrickOverViewInfo TrickOverViewInfo =
            new TrickOverViewInfo("DrawInspectorOnEditWinow",
                "在EditorWindow绘制InspectorUI",
                "EditorWindow",
                "using System;\nusing System.Collections;\nusing System.Collections.Generic;\nusing UnityEditor;\nusing UnityEngine;\n\nnamespace ToolKits\n{\n    public class Example_55_Window : EditorWindow\n    {\n        private static Example_55_Window _window;\n        private static readonly Vector2 MIN_SIZE = new Vector2(400, 400);\n\n        private const string path =\n            \"Assets/Editor/Examples/Example_55_DrawInspectorOnEditWinow/Example_55_Scriptobj.asset\";\n\n        private Editor _editor;\n\n        [MenuItem(\"Tools/DrawInspectorOnEditorWindow\", priority = 55)]\n        private static void PopUp()\n        {\n            _window = GetWindow<Example_55_Window>(\"\");\n            _window.minSize = MIN_SIZE;\n            _window.maxSize = MIN_SIZE;\n            _window.Init();\n            _window.Show();\n        }\n\n        private void Init()\n        {\n            var asset = AssetDatabase.LoadAssetAtPath<Example_55_Scriptobj>(path);\n            _editor = Editor.CreateEditor(asset);\n        }\n\n        private void OnGUI()\n        {\n            if (null != _editor)\n            {\n                _editor.OnInspectorGUI();\n            }\n        }\n    }\n}",
                "Assets/Editor/Examples/Example_55_DrawInspectorOnEditWinow",
                typeof(Example_DrawInspectorOnEditWinow),
                picPath : "Assets/Editor/Examples/Example_55_DrawInspectorOnEditWinow/QQ截图20210617215505.png",
                videoPath : "");

        public override TrickOverViewInfo GetTrickOverViewInfo()
        {
            return TrickOverViewInfo;
        }
    }
}
