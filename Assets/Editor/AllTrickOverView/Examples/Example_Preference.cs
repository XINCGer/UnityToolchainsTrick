using AllTrickOverView.Core;

namespace AllTrickOverView.Examples
{
    public class Example_Preference : AExample_Base
    {
        public static TrickOverViewInfo TrickOverViewInfo =
            new TrickOverViewInfo("Preference",
                "Preference添加自定义内容",
                "Preference",
                "using System.Collections;\nusing System.Collections.Generic;\nusing UnityEditor;\nusing UnityEngine;\n\nnamespace ToolKits\n{\n    public class Example_12_PerferenceExtension\n    {\n        private static bool loaded = false;\n        private static string context = \"\";\n        private static bool sceneviewSwitch;\n        private const string KEY = \"__ToolChainsTrick_Context__\";\n        private static bool globalInputEventEnable = false;\n\n#if UNITY_2019_1_OR_NEWER\n        [SettingsProvider]\n        private static SettingsProvider ToolChainsTrickSetting()\n        {\n            var provider = new SettingsProvider(\"Preferences/ToolChainsTrick\", SettingsScope.User)\n            {\n                guiHandler = (string key) => { PreferenceGUI(); },\n                keywords = new string[] {\"Tool\", \"Chains\", \"Trick\"},\n            };\n            return provider;\n        }\n#else\n    [PreferenceItem( \"ToolChainsTrick\" )]\n#endif\n        private static void PreferenceGUI()\n        {\n            if (!loaded)\n            {\n                Load();\n            }\n\n            EditorGUI.BeginChangeCheck();\n            context = EditorGUILayout.TextField(new GUIContent(\"文本配置\"), context);\n            if (EditorGUI.EndChangeCheck())\n            {\n                EditorPrefs.SetString(KEY, context);\n            }\n\n            EditorGUI.BeginChangeCheck();\n            sceneviewSwitch = EditorGUILayout.Toggle(\"SceneView拓展开关\", sceneviewSwitch);\n            if (EditorGUI.EndChangeCheck())\n            {\n                EditorPrefs.SetBool(Constants.SCENE_VIEW_EXTENSITON_SWITH, sceneviewSwitch);\n                SceneViewExtension.EditorInitialize();\n            }\n            \n            EditorGUI.BeginChangeCheck();\n            globalInputEventEnable = EditorGUILayout.Toggle(\"是否开启全局检测事件\", globalInputEventEnable);\n            if (EditorGUI.EndChangeCheck())\n            {\n                EditorPrefs.SetBool(Constants.GLOBAL_INPUT_ENEVT_ENABLE,globalInputEventEnable);\n                GlobalInputEvent.EditorInitialize();\n            }\n        }\n\n        private static void Load()\n        {\n            context = EditorPrefs.GetString(KEY, \"\");\n            sceneviewSwitch = EditorPrefs.GetBool(Constants.SCENE_VIEW_EXTENSITON_SWITH, false);\n            loaded = true;\n        }\n    }\n}",
                "Assets/Editor/Examples/Example_12_Preference",
                typeof(Example_Preference),
                picPath : "Assets/Editor/Examples/Example_12_Preference/QQ截图20210419154551.png",
                videoPath : "");

        public override TrickOverViewInfo GetTrickOverViewInfo()
        {
            return TrickOverViewInfo;
        }
    }
}
