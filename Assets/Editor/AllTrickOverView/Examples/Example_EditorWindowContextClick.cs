using AllTrickOverView.Core;

namespace AllTrickOverView.Examples
{
    public class Example_EditorWindowContextClick : AExample_Base
    {
        public static TrickOverViewInfo TrickOverViewInfo =
            new TrickOverViewInfo("EditorWindowContextClick",
                "EditorWindow右键事件-上下文菜单",
                "EditorWindow",
                "using System;\nusing System.Collections;\nusing System.Collections.Generic;\nusing UnityEditor;\nusing UnityEngine;\n\nnamespace ToolKits\n{\n    public class EditorWindowContextClick : EditorWindow\n    {\n        private static EditorWindowContextClick _window;\n        private static readonly Vector2 MIN_SIZE = new Vector2(400, 200);\n\n        [MenuItem(\"Tools/EditorWindowContextClick\", priority = 27)]\n        private static void PopUp()\n        {\n            _window = GetWindow<EditorWindowContextClick>(\"EditorWindow右键弹出菜单演示\");\n            _window.Show();\n        }\n\n        private void OnGUI()\n        {\n            var e = Event.current;\n            if (null != e)\n            {\n                if (e.type == EventType.MouseDown && e.button == 1)\n                {\n                    var genericMenu = new GenericMenu();\n                    genericMenu.AddItem(new GUIContent(\"功能1\"), false, () => { Debug.Log(\"功能1\"); });\n                    genericMenu.AddItem(new GUIContent(\"功能合集/功能2\"), false, () => { Debug.Log(\"功能2\"); });\n                    genericMenu.AddItem(new GUIContent(\"功能合集/功能3\"), false, () => { Debug.Log(\"功能3\"); });\n                    genericMenu.AddSeparator(\"功能合集/\");\n                    genericMenu.AddItem(new GUIContent(\"功能合集/功能4\"), false, () => { Debug.Log(\"功能4\"); });\n                    genericMenu.ShowAsContext();\n                }\n            }\n        }\n    }\n}",
                "Assets/Editor/Examples/Example_27_EditorWindowContextClick",
                typeof(Example_EditorWindowContextClick),
                picPath : "Assets/Editor/Examples/Example_27_EditorWindowContextClick/QQ截图20210419161455.png",
                videoPath : "");

        public override TrickOverViewInfo GetTrickOverViewInfo()
        {
            return TrickOverViewInfo;
        }
    }
}
