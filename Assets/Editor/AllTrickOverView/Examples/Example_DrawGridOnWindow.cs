using AllTrickOverView.Core;

namespace AllTrickOverView.Examples
{
    public class Example_DrawGridOnWindow : AExample_Base
    {
        public static TrickOverViewInfo TrickOverViewInfo =
            new TrickOverViewInfo("DrawGridOnWindow",
                "在EditorWindow上绘制网格",
                "Draw",
                "using System;\nusing System.Collections;\nusing System.Collections.Generic;\nusing UnityEditor;\nusing UnityEngine;\n\nnamespace ToolKits\n{\n    public class DrawGridWindow : EditorWindow\n    {\n        private static DrawGridWindow _window;\n        private static Vector2 MIN_SIZE = new Vector2(400, 300);\n\n        private Color backgroundColor;\n        private Color gridColor;\n        private Vector2 drag;\n        private Vector2 offset;\n\n        [MenuItem(\"Tools/绘制网格的示例窗口\", priority = 61)]\n        private static void PopUp()\n        {\n            _window = GetWindow<DrawGridWindow>(\"绘制网格的示例窗口\");\n            _window.minSize = MIN_SIZE;\n            _window.Init();\n            _window.Show();\n        }\n\n        private void Init()\n        {\n            backgroundColor = new Color(0.4f, 0.4f, 0.4f);\n            gridColor = new Color(0.1f, 0.1f, 0.1f);\n        }\n\n        private void OnGUI()\n        {\n            ProcessEvents(Event.current);\n            DrawBackground();\n            DrawGrid(10, 0.2f);\n            DrawGrid(50, 0.4f);\n            if (GUI.changed) Repaint();\n        }\n\n        private void DrawBackground()\n        {\n            EditorGUI.DrawRect(new Rect(0, 0, position.width, position.height), backgroundColor);\n        }\n\n        private void DrawGrid(float gridSpacing, float gridOpacity)\n        {\n            int widthDivs = Mathf.CeilToInt(position.width / gridSpacing);\n            int heightDivs = Mathf.CeilToInt(position.height / gridSpacing);\n            Handles.BeginGUI();\n            Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);\n            offset += drag * 0.5f;\n            Vector3 newOffset = new Vector3(offset.x % gridSpacing, offset.y % gridSpacing, 0);\n            for (int i = 0; i < widthDivs; i++)\n            {\n                Handles.DrawLine(new Vector3(gridSpacing * i, -gridSpacing, 0) + newOffset,\n                    new Vector3(gridSpacing * i, position.height, 0f) + newOffset);\n            }\n\n            for (int j = 0; j < heightDivs; j++)\n            {\n                Handles.DrawLine(new Vector3(-gridSpacing, gridSpacing * j, 0) + newOffset,\n                    new Vector3(position.width, gridSpacing * j, 0f) + newOffset);\n            }\n\n            Handles.color = Color.white;\n            Handles.EndGUI();\n        }\n\n        private void ProcessEvents(Event e)\n        {\n            drag = Vector2.zero;\n            switch (e.type)\n            {\n                case EventType.MouseDrag:\n                    if (e.button == 0)\n                    {\n                        OnDrag(e.delta);\n                    }\n\n                    break;\n            }\n        }\n\n        private void OnDrag(Vector2 delta)\n        {\n            drag = delta;\n            GUI.changed = true;\n        }\n    }\n}",
                "Assets/Editor/Examples/Example_61_DrawGridOnWindow",
                typeof(Example_DrawGridOnWindow),
                picPath : "Assets/Editor/Examples/Example_61_DrawGridOnWindow/Preview.png",
                videoPath : "");

        public override TrickOverViewInfo GetTrickOverViewInfo()
        {
            return TrickOverViewInfo;
        }
    }
}
