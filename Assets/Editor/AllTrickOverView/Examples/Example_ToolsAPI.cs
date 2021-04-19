using AllTrickOverView.Core;

namespace AllTrickOverView.Examples
{
    public class Example_ToolsAPI : AExample_Base
    {
        public static TrickOverViewInfo TrickOverViewInfo =
            new TrickOverViewInfo("ToolsAPI",
                "获取SceneView的Tools状态",
                "Scene",
                "using System;\nusing System.Collections;\nusing System.Collections.Generic;\nusing UnityEditor;\nusing UnityEngine;\n\nnamespace ToolKits\n{\n    public class ToolsAPI : EditorWindow\n    {\n        private static ViewTool _viewTool;\n\n        [MenuItem(\"Tools/ToolsAPI\", priority = 22)]\n        private static void PopUp()\n        {\n            var _window = GetWindow<ToolsAPI>(\"ToolsAPI\");\n            SceneView.duringSceneGui -= OnSceneViewGUI;\n            SceneView.duringSceneGui += OnSceneViewGUI;\n            _window.Show();\n        }\n\n        private void OnGUI()\n        {\n            if (GUILayout.Button(\"Tools.current\"))\n            {\n                Debug.Log(\"Tools.current:\" + Tools.current);\n            }\n        }\n\n        private static void OnSceneViewGUI(SceneView view)\n        {\n            if (Tools.viewTool != _viewTool)\n            {\n                Debug.Log(\"Tools.viewTool:\" + Tools.viewTool);\n                _viewTool = Tools.viewTool;\n            }\n        }\n    }\n}",
                "Assets/Editor/Examples/Example_22_ToolsAPI",
                typeof(Example_ToolsAPI),
                picPath : "Assets/Editor/Examples/Example_22_ToolsAPI/QQ截图20210419160327.png",
                videoPath : "");

        public override TrickOverViewInfo GetTrickOverViewInfo()
        {
            return TrickOverViewInfo;
        }
    }
}
