using AllTrickOverView.Core;

namespace AllTrickOverView.Examples
{
    public class Example_EnableTitleContentRichText : AExample_Base
    {
        public static TrickOverViewInfo TrickOverViewInfo =
            new TrickOverViewInfo("EnableTitleContentRichText",
                "让 Tab 标题支持富文本，make tab‘s title render richtext again",
                "EditorWindow",
                "using UnityEditor;\nusing UnityEngine;\nusing System.Collections;\nusing System;\npublic class TinyWebServerWindow : EditorWindow\n{\n    private static EditorWindow window;\n        public bool IsListening { get; private set; }\n    private bool isTitleChangeRequired = true;\n\n    [MenuItem(\"Tools/Example_79_EnableTitleContentRichText\", priority = 79)]\n    private static void Init()\n    {\n        window = GetWindow<TinyWebServerWindow>();\n        window.minSize = new Vector2(300, 180);\n        var icon = EditorGUIUtility.IconContent(\"d_BuildSettings.Web.Small\");\n        window.titleContent = new GUIContent(\"Tiny WebServer\", icon.image);\n        window.Show();\n    }\n\n    private void TryRepaintTitleContent()\n    {\n        window ??= GetWindow<TinyWebServerWindow>();\n        var assembly = typeof(AssetDatabase).Assembly;\n        var styleType = assembly.GetType(\"UnityEditor.DockArea+Styles\");\n        var style = styleType.GetField(\"tabLabel\", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public).GetValue(null) as GUIStyle;\n        window.titleContent.text = \"Tiny WebServer<*****=###>●</*****>\";\n        var calcSize = style.CalcSize(window.titleContent);\n        styleType.GetField(\"tabMaxWidth\", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public).SetValue(null, calcSize.x);\n        var contents = assembly.GetType(\"UnityEditor.DockArea\").GetField(\"s_GUIContents\", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic).GetValue(null);\n        (contents as IDictionary).Clear();\n        window.titleContent.text = \"Tiny WebServer <color=\"+(IsListening == true ? \"green\" : \"red\")+\">●</color>\";\n        window.titleContent.tooltip = IsListening == true ? \"Server is running\" : \"Server is stoped\";\n    }\n\n    long cachedticks = 0l;\n    private void Update()\n    {\n        if (DateTime.Now.Ticks-cachedticks<= 15000000)\n        {\n            return;\n        }\n        IsListening = !IsListening;\n        isTitleChangeRequired = true;\n        Repaint();\n        cachedticks = DateTime.Now.Ticks;\n    }\n\n    private void OnGUI()\n    {\n        if (isTitleChangeRequired)\n        {\n            TryRepaintTitleContent();\n            isTitleChangeRequired = false;\n        }\n    }\n}",
                "Assets/Editor/Examples/Example_79_EnableTitleContentRichText",
                typeof(Example_EnableTitleContentRichText),
                picPath : "Assets/Editor/Examples/Example_79_EnableTitleContentRichText/Preview.gif",
                videoPath : "");

        public override TrickOverViewInfo GetTrickOverViewInfo()
        {
            return TrickOverViewInfo;
        }
    }
}
