using AllTrickOverView.Core;

namespace AllTrickOverView.Examples
{
    public class Example_DrawArrowLine : AExample_Base
    {
        public static TrickOverViewInfo TrickOverViewInfo =
            new TrickOverViewInfo("DrawArrowLine",
                "绘制带箭头的线",
                "Draw",
                "using System;\nusing System.Collections;\nusing System.Collections.Generic;\nusing UnityEditor;\nusing UnityEngine;\n\nnamespace ToolKits\n{\n    public class DrawArrowWindow : EditorWindow\n    {\n        private static DrawArrowWindow _window;\n        private static readonly Vector2 MIN_SIZE = new Vector2(300, 200);\n        private const int LABEL_SIZE = 25;\n\n        [MenuItem(\"Tools/在窗口上绘制一条带有箭头的线\", priority = 6)]\n        public static void PopUp()\n        {\n            _window = GetWindow<DrawArrowWindow>(\"箭头线窗口\");\n            _window.minSize = _window.maxSize = MIN_SIZE;\n            _window.Show();\n        }\n\n        private void OnGUI()\n        {\n            DrawArrow(new Vector2(10, 10), new Vector2(150, 10), Color.white);\n            DrawArrow(new Vector2(10, 10), new Vector2(10, 150), Color.white);\n\n            GUI.Label(new Rect(160, 10, LABEL_SIZE, LABEL_SIZE), \"X轴\");\n            GUI.Label(new Rect(10, 160, LABEL_SIZE, LABEL_SIZE), \"Y轴\");\n        }\n\n        private void DrawArrow(Vector2 from, Vector2 to, Color color)\n        {\n            Handles.BeginGUI();\n            Handles.color = color;\n            Handles.DrawAAPolyLine(3, from, to);\n            Vector2 v0 = from - to;\n            v0 *= 10 / v0.magnitude;\n            Vector2 v1 = new Vector2(v0.x * 0.866f - v0.y * 0.5f, v0.x * 0.5f + v0.y * 0.866f);\n            Vector2 v2 = new Vector2(v0.x * 0.866f + v0.y * 0.5f, v0.x * -0.5f + v0.y * 0.866f);\n            ;\n            Handles.DrawAAPolyLine(3, to + v1, to, to + v2);\n            Handles.EndGUI();\n        }\n    }\n}",
                "Assets/Editor/Examples/Example_06_DrawArrowLine",
                typeof(Example_DrawArrowLine),
                picPath : "Assets/Editor/Examples/Example_06_DrawArrowLine/QQ截图20210419153358.png",
                videoPath : "");

        public override TrickOverViewInfo GetTrickOverViewInfo()
        {
            return TrickOverViewInfo;
        }
    }
}
