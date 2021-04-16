using AllTrickOverView.Core;

namespace AllTrickOverView.Examples
{
    public class Example_MoreInspector : AExample_Base
    {
        public static TrickOverViewInfo TrickOverViewInfo =
            new TrickOverViewInfo("MoreInspector",
                "多Inspector预览",
                "Inspector",
                "using System;\nusing System.Collections;\nusing System.Collections.Generic;\nusing System.Reflection;\nusing UnityEditor;\nusing UnityEngine;\n\npublic class MoreInspector : EditorWindow\n{\n    private static MoreInspector _window;\n    private EditorWindow inspectorWindow;\n    private static readonly Vector2 MIN_SIE = new Vector2(400, 300);\n    private static MoreInspectorSOForTest m_MoreInspectorSOForTest;\n\n    [MenuItem(\"Tools/额外Inspector面板\", priority = 43)]\n    private static void PopUp()\n    {\n        _window = GetWindow<MoreInspector>(\"额外Inspector面板\");\n        _window.minSize = MIN_SIE;\n        m_MoreInspectorSOForTest =\n            AssetDatabase.LoadAssetAtPath<MoreInspectorSOForTest>(\n                \"Assets/Editor/Examples/Example_43_MoreInspector/New More Inspector SO For Test.asset\");\n        _window.Show();\n    }\n\n    private void OnGUI()\n    {\n        if (GUILayout.Button(\"显示额外Inspector面板\"))\n        {\n            inspectorWindow = GetInspectTarget(m_MoreInspectorSOForTest);\n            inspectorWindow.Show();\n            DockUtilities.DockWindow(this, inspectorWindow, DockUtilities.DockPosition.Right);\n        }\n    }\n\n    public EditorWindow GetInspectTarget(UnityEngine.Object targetGO)\n    {\n        // Get Unity Internal Objects\n        Type inspectorType = typeof(Editor).Assembly.GetType(\"UnityEditor.InspectorWindow\");\n        // Create an inspector window Instance\n        EditorWindow inspectorInstance = ScriptableObject.CreateInstance(inspectorType) as EditorWindow;\n        // We display it - currently, it will inspect whatever gameObject is currently selected\n        // So we need to find a way to let it inspect/aim at our target GO that we passed\n\n        // 1. Cache the current selected gameObject\n        UnityEngine.Object prevSelection = Selection.activeObject;\n        // 2. Set the current selection to our target GO\n        Selection.activeObject = targetGO;\n        // 3. Get a ref to the \"locked\" property, which will lock the state of the inspector to the current inspected target\n        var isLocked = inspectorType.GetProperty(\"isLocked\", BindingFlags.Instance | BindingFlags.Public);\n        // 4. Invoke 'isLocked' setter method passing \"true\" to lock the inspector\n        isLocked.GetSetMethod().Invoke(inspectorInstance, new object[] {true});\n        // 5. Finally revert back to the previous selection so that other inspector will continue to inspector whatever they were inspecting\n        Selection.activeObject = prevSelection;\n\n        return inspectorInstance;\n    }\n}",
                "$CODE_PATH$",
                typeof(Example_MoreInspector),
                picPath : "Assets/Editor/Examples/Example_43_MoreInspector/QQ截图20210416191814.png",
                videoPath : "");

        public override TrickOverViewInfo GetTrickOverViewInfo()
        {
            return TrickOverViewInfo;
        }
    }
}
