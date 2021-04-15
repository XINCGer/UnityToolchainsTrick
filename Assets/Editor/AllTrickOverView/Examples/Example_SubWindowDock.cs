using AllTrickOverView.Core;

namespace AllTrickOverView.Examples
{
    public class Example_SubWindowDock : AExample_Base
    {
        public static TrickOverViewInfo TrickOverViewInfo =
            new TrickOverViewInfo("SubWindowDock",
                "EditorWindow的Docker模式",
                "EditorWindow",
                "using System;\nusing System.Collections;\nusing System.Collections.Generic;\nusing System.IO;\nusing System.Threading.Tasks;\nusing UnityEditor;\nusing UnityEngine;\n\nnamespace ToolKits\n{\n    public class Example_15_MainWidow : EditorWindow\n    {\n        private static Example_15_MainWidow _window;\n        private static readonly Vector2 MIN_SIZE = new Vector2(400, 300);\n        private const string KEY = \"__EXAPLE_15_DOCK_LAYOUT_\";\n        private const string PATH = \"Assets/Editor/Examples/Example_15_SubWindowDock/layout.wlt\";\n\n        private bool isLayoutByCode = false;\n        private Example_15_SubWindowA _windowA;\n        private Example_15_SubWindowB _windowB;\n\n        [MenuItem(\"Tools/SubWindowDock\", priority = 15)]\n        private static void PopUp()\n        {\n            _window = GetWindow<Example_15_MainWidow>(\"主界面\");\n            _window.minSize = MIN_SIZE;\n            _window.Init();\n            _window.Show();\n        }\n\n        private void Init()\n        {\n            isLayoutByCode = EditorPrefs.GetBool(KEY, true);\n            _windowA = Example_15_SubWindowA.PopUp();\n            _windowB = Example_15_SubWindowB.PopUp();\n            if (isLayoutByCode)\n            {\n                LoadLayoutByCode();\n            }\n        }\n\n        private async void LoadLayoutByCode()\n        {\n            await Task.Delay(100);\n            LayoutUtility.DockEditorWindow(_window, _windowB);\n            await Task.Delay(100);\n            LayoutUtility.DockEditorWindow(_window, _windowA);\n        }\n\n        private void OnGUI()\n        {\n            EditorGUI.BeginChangeCheck();\n            isLayoutByCode = GUILayout.Toggle(isLayoutByCode, \"是否通过代码实现布局\");\n            if (EditorGUI.EndChangeCheck())\n            {\n                EditorPrefs.SetBool(KEY, isLayoutByCode);\n            }\n\n            if (GUILayout.Button(\"加载布局\"))\n            {\n                LayoutUtility.LoadLayoutFromAsset(PATH);\n            }\n\n            if (GUILayout.Button(\"保存布局\"))\n            {\n                LayoutUtility.SaveLayoutToAsset(PATH);\n                AssetDatabase.Refresh();\n            }\n        }\n    }\n}",
                "$CODE_PATH$",
                typeof(Example_SubWindowDock),
                picPath : "",
                videoPath : "Assets/Editor/Examples/Example_15_SubWindowDock/SubWindowDockVideo.mp4");

        public override TrickOverViewInfo GetTrickOverViewInfo()
        {
            return TrickOverViewInfo;
        }
    }
}
