using AllTrickOverView.Core;

namespace AllTrickOverView.Examples
{
    public class Example_SaveAssets : AExample_Base
    {
        public static TrickOverViewInfo TrickOverViewInfo =
            new TrickOverViewInfo("SaveAssets",
                "保存Asset测试",
                "Assets",
                "using System;\nusing System.Collections;\nusing System.Collections.Generic;\nusing UnityEditor;\nusing UnityEngine;\n\nnamespace ToolKits\n{\n    public class Example05Window : EditorWindow\n    {\n        private static Example05Window _window;\n        private static readonly Vector2 MIN_SIZE = new Vector2(300, 200);\n        private const string PATH = \"\";\n\n        private string Name;\n        private int Age;\n        private ExampleScriptableObject setting;\n\n        [MenuItem(\"Tools/资源保存测试窗口\", priority = 5)]\n        public static void PopUp()\n        {\n            _window = GetWindow<Example05Window>(\"Example05Window\");\n            _window.minSize = _window.maxSize = MIN_SIZE;\n            _window.Init();\n            _window.Show();\n        }\n\n        private void Init()\n        {\n            setting =\n                EditorHelper.GetScriptableObjectAsset<ExampleScriptableObject>(\n                    \"Assets/Editor/Examples/Example_05_SaveAssets/ExampleAssets.asset\");\n            Name = setting.Name;\n            Age = setting.Age;\n        }\n\n        private void OnGUI()\n        {\n            var orginColor = GUI.color;\n            GUI.color = Color.green;\n            EditorGUI.BeginChangeCheck();\n            Name = EditorGUILayout.TextField(\"姓名\", Name);\n            if (EditorGUI.EndChangeCheck())\n            {\n                setting.Name = Name;\n            }\n\n            EditorGUI.BeginChangeCheck();\n\n            GUI.color = Color.yellow;\n            Age = EditorGUILayout.IntField(\"年龄\", Age);\n            if (EditorGUI.EndChangeCheck())\n            {\n                setting.Age = Age;\n            }\n\n            GUI.color = Color.red;\n            if (GUILayout.Button(\"保存\"))\n            {\n                EditorUtility.SetDirty(setting);\n                AssetDatabase.SaveAssets();\n                AssetDatabase.Refresh();\n            }\n\n            GUI.color = orginColor;\n        }\n    }\n}",
                "Assets/Editor/Examples/Example_05_SaveAssets",
                typeof(Example_SaveAssets),
                picPath : "Assets/Editor/Examples/Example_05_SaveAssets/QQ截图20210419153057.png",
                videoPath : "");

        public override TrickOverViewInfo GetTrickOverViewInfo()
        {
            return TrickOverViewInfo;
        }
    }
}
