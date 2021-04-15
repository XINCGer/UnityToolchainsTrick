using AllTrickOverView.Core;

namespace AllTrickOverView.Examples
{
    public class Example_MoreSceneView : AExample_Base
    {
        public static TrickOverViewInfo TrickOverViewInfo =
            new TrickOverViewInfo("MoreSceneView",
                "多Scene窗口示例",
                "Scene",
                "using System.Collections;\nusing System.Collections.Generic;\nusing UnityEditor;\nusing UnityEngine;\n\nnamespace ToolKits\n{\n    public static class CreateMoreSceneView\n    {\n        [MenuItem(\"Tools/CreateMoreSceneView\", priority = 16)]\n        private static void PopUp()\n        {\n            var _window = ScriptableObject.CreateInstance<SceneView>();\n            _window.Init();\n            _window.Show();\n        }\n\n        private static void Init(this SceneView sceneView)\n        {\n            if (null != sceneView)\n            {\n                Debug.Log(\"SceneView Init!\");\n            }\n        }\n    }\n}",
                "$CODE_PATH$",
                typeof(Example_MoreSceneView),
                picPath : "Assets/Editor/Examples/Example_16_MoreSceneView/MoreSceneViewPreviewPng.png",
                videoPath : "");

        public override TrickOverViewInfo GetTrickOverViewInfo()
        {
            return TrickOverViewInfo;
        }
    }
}
