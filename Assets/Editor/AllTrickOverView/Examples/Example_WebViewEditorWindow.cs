using AllTrickOverView.Core;

namespace AllTrickOverView.Examples
{
    public class Example_WebViewEditorWindow : AExample_Base
    {
        public static TrickOverViewInfo TrickOverViewInfo =
            new TrickOverViewInfo("WebViewEditorWindow",
                "WebView（在Unity2020及以上版本无效）",
                "EditorWindow",
                "using UnityEngine;\nusing UnityEditor;\nusing System;\nusing System.Reflection;\n\n#if !UNITY_2020\npublic class WebViewEditorWindow : EditorWindow\n{\n    static string Url = \"https://github.com/XINCGer/UnityToolchainsTrick\";\n\n    [MenuItem(\"Tools/WebViewWindow\", priority = 37)]\n    static void Open()\n    {\n        string typeName = \"UnityEditor.Web.WebViewEditorWindowTabs\";\n        Type type = Assembly.Load(\"UnityEditor.dll\").GetType(typeName);\n        BindingFlags Flags = BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy;\n        var methodInfo = type.GetMethod(\"Create\", Flags);\n        methodInfo = methodInfo.MakeGenericMethod(type);\n        methodInfo.Invoke(null, new object[] { \"WebView\", Url, 200, 530, 800, 600 });\n    }\n}\n#endif",
                "Assets/Editor/Examples/Example_37_WebViewEditorWindow",
                typeof(Example_WebViewEditorWindow),
                picPath : "",
                videoPath : "");

        public override TrickOverViewInfo GetTrickOverViewInfo()
        {
            return TrickOverViewInfo;
        }
    }
}
