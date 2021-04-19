using AllTrickOverView.Core;

namespace AllTrickOverView.Examples
{
    public class Example_TimeControl : AExample_Base
    {
        public static TrickOverViewInfo TrickOverViewInfo =
            new TrickOverViewInfo("TimeControl",
                "时间线控制",
                "Animation",
                "using System;\nusing System.Collections;\nusing System.Collections.Generic;\nusing UnityEditor;\nusing UnityEngine;\n\nnamespace ToolKits\n{\n    public class TimeControlWindow : EditorWindow\n    {\n        private static TimeControlWindow _window;\n        private static readonly Vector2 MIN_SIZE = new Vector2(400, 300);\n        private TimeControl _timeControl;\n        private static readonly Rect TIME_RECR = new Rect(20, 20, 300, 50);\n\n        [MenuItem(\"Tools/TimeControlWindow\", priority = 10)]\n        private static void PopUp()\n        {\n            _window = GetWindow<TimeControlWindow>(\"TimeControlWindow\");\n            _window.minSize = _window.maxSize = MIN_SIZE;\n            _window.Init();\n            _window.Show();\n        }\n\n        private void Init()\n        {\n            _timeControl = new TimeControl();\n        }\n\n        private void OnGUI()\n        {\n            _timeControl.DoTimeControl(TIME_RECR);\n            if (Event.current.type == EventType.Repaint)\n            {\n                _timeControl.Update();\n            }\n            Repaint();\n        }\n    }\n}",
                "Assets/Editor/Examples/Example_10_TimeControl",
                typeof(Example_TimeControl),
                picPath : "Assets/Editor/Examples/Example_10_TimeControl/QQ截图20210419154143.png",
                videoPath : "");

        public override TrickOverViewInfo GetTrickOverViewInfo()
        {
            return TrickOverViewInfo;
        }
    }
}
