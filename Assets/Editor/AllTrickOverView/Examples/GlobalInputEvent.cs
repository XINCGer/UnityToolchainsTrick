using AllTrickOverView.Core;

namespace AllTrickOverView.Examples
{
    public class GlobalInputEvent : AExample_Base
    {
        public static TrickOverViewInfo TrickOverViewInfo =
            new TrickOverViewInfo("GlobalInputEvent",
                "获取输入事件（全局）",
                "Others",
                "using System;\nusing System.Collections;\nusing System.Collections.Generic;\nusing System.Reflection;\nusing UnityEditor;\nusing UnityEngine;\n\nnamespace ToolKits\n{\n    public class GlobalInputEvent\n    {\n        private static bool isSpaceDown = false;\n\n        [InitializeOnLoadMethod]\n        public static void EditorInitialize()\n        {\n            // globalEventHandler\n            FieldInfo info = typeof(EditorApplication).GetField(\"globalEventHandler\",\n                BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic);\n            EditorApplication.CallbackFunction functions = (EditorApplication.CallbackFunction) info.GetValue(null);\n            functions -= OnGlobalEventHandler;\n\n            if (EditorPrefs.GetBool(Constants.GLOBAL_INPUT_ENEVT_ENABLE, false))\n            {\n                functions += OnGlobalEventHandler;\n                info.SetValue(null, (object) functions);\n            }\n\n            info.SetValue(null, (object) functions);\n        }\n\n        private static void OnGlobalEventHandler()\n        {\n            var e = Event.current;\n            if (null != e)\n            {\n                if (e.isKey)\n                {\n                    if (e.keyCode == KeyCode.Space)\n                    {\n                        isSpaceDown = e.type != EventType.KeyUp;\n                    }\n\n                    if (e.type == EventType.KeyUp)\n                    {\n                        if (e.keyCode == KeyCode.A && isSpaceDown)\n                        {\n                            Debug.LogError(\"组合键：空格 + A\");\n                        }\n                        else if (e.keyCode == KeyCode.S && isSpaceDown)\n                        {\n                            Debug.LogError(\"组合键：空格 + S\");\n                        }\n                    }\n                    else if (e.type == EventType.KeyDown)\n                    {\n                        if (e.keyCode == KeyCode.UpArrow)\n                        {\n                            Debug.LogError(\"组合键：空格+上箭头\");\n                        }\n                        else if (e.keyCode == KeyCode.DownArrow)\n                        {\n                            Debug.LogError(\"组合键：空格+下箭头\");\n                        }\n                    }\n                }\n            }\n        }\n    }\n}",
                "Assets/Editor/Examples/Example_25_GlobalInputEvent",
                typeof(GlobalInputEvent),
                picPath : "",
                videoPath : "");

        public override TrickOverViewInfo GetTrickOverViewInfo()
        {
            return TrickOverViewInfo;
        }
    }
}
