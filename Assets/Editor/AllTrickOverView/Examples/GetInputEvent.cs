using AllTrickOverView.Core;

namespace AllTrickOverView.Examples
{
    public class GetInputEvent : AExample_Base
    {
        public static TrickOverViewInfo TrickOverViewInfo =
            new TrickOverViewInfo("GetInputEvent",
                "获取输入事件",
                "Others",
                "using System;\nusing System.Collections;\nusing System.Collections.Generic;\nusing UnityEditor;\nusing UnityEngine;\n\nnamespace ToolKits\n{\n    public class GetInputEventWindow : EditorWindow\n    {\n        private bool isSpaceDown = false;\n        private static GetInputEventWindow _window;\n\n        [MenuItem(\"Tools/GetInputEventWindow\", priority = 24)]\n        private static void PopUp()\n        {\n            _window = GetWindow<GetInputEventWindow>(\"GetInputEventWindow\");\n            _window.Show();\n        }\n\n        private void OnGUI()\n        {\n            Event e = Event.current;\n            if (e.isKey)\n            {\n                if (e.keyCode == KeyCode.Space)\n                {\n                    isSpaceDown = e.type != EventType.KeyUp;\n                }\n\n                if (e.type == EventType.KeyUp)\n                {\n                    if (e.keyCode == KeyCode.A && isSpaceDown)\n                    {\n                        Debug.LogError(\"组合键：空格 + A\");\n                    }\n                    else if (e.keyCode == KeyCode.S && isSpaceDown)\n                    {\n                        Debug.LogError(\"组合键：空格 + S\");\n                    }\n                }\n                else if (e.type == EventType.KeyDown)\n                {\n                    if (e.keyCode == KeyCode.UpArrow)\n                    {\n                        Debug.LogError(\"组合键：空格+上箭头\");\n                    }\n                    else if (e.keyCode == KeyCode.DownArrow)\n                    {\n                        Debug.LogError(\"组合键：空格+下箭头\");\n                    }\n                }\n            }\n        }\n    }\n}",
                "Assets/Editor/Examples/Example_24_GetInputEvent",
                typeof(GetInputEvent),
                picPath : "",
                videoPath : "");

        public override TrickOverViewInfo GetTrickOverViewInfo()
        {
            return TrickOverViewInfo;
        }
    }
}
