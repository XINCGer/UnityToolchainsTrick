using AllTrickOverView.Core;

namespace AllTrickOverView.Examples
{
    public class Example_MultiWindowLayerManagement : AExample_Base
    {
        public static TrickOverViewInfo TrickOverViewInfo =
            new TrickOverViewInfo("MultiWindowLayerManagement",
                "多重窗口弹出管理器",
                "EditorWindow",
                "using System.Collections;\nusing System.Collections.Generic;\nusing UnityEditor;\nusing UnityEngine;\n\nnamespace ToolKits\n{\n    /// <summary>\n    /// 编辑器主界面\n    /// </summary>\n    public class MainWindow : EditorWindowBase\n    {\n        private static MainWindow window;\n        private static Vector2 minResolution = new Vector2(800, 600);\n        private static Rect middleCenterRect = new Rect(200, 100, 400, 400);\n        private GUIStyle labelStyle;\n\n        /// <summary>\n        /// 对外的访问接口\n        /// </summary>\n        [MenuItem(\"Tools/MultiWindowLayerManagement\", priority = 17)]\n        public static void Popup()\n        {\n            window = EditorWindow.GetWindow(typeof(MainWindow), true, \"多重窗口编辑器\") as MainWindow;\n            window.minSize = minResolution;\n            window.Init();\n            EditorWindowMgr.AddEditorWindow(window);\n            window.Show();\n        }\n\n        /// <summary>\n        /// 在这里可以做一些初始化工作\n        /// </summary>\n        private void Init()\n        {\n            Priority = 1;\n\n            labelStyle = new GUIStyle();\n            labelStyle.normal.textColor = Color.red;\n            labelStyle.alignment = TextAnchor.MiddleCenter;\n            labelStyle.fontSize = 14;\n            labelStyle.border = new RectOffset(1, 1, 2, 2);\n        }\n\n        private void OnGUI()\n        {\n            ShowEditorGUI();\n        }\n\n        /// <summary>\n        /// 绘制编辑器界面\n        /// </summary>\n        private void ShowEditorGUI()\n        {\n            GUILayout.BeginArea(middleCenterRect);\n            GUILayout.BeginVertical();\n            EditorGUILayout.LabelField(\"点击下面的按钮创建重复弹出窗口\", labelStyle, GUILayout.Width(220));\n            if (GUILayout.Button(\"创建窗口\", GUILayout.Width(80)))\n            {\n                RepeateWindow.Popup(window.position.position);\n            }\n\n            GUILayout.EndVertical();\n            GUILayout.EndArea();\n        }\n\n        private void OnDestroy()\n        {\n            //主界面销毁的时候，附带销毁创建出来的子界面\n            EditorWindowMgr.RemoveEditorWindow(window);\n            EditorWindowMgr.DestoryAllWindow();\n        }\n\n        private void OnFocus()\n        {\n            //重写OnFocus方法，让EditorWindowMgr去自动排序汇聚焦点\n            EditorWindowMgr.FoucusWindow();\n        }\n    }\n}",
                "Assets/Editor/Examples/Example_17_MultiWindowLayerManagement",
                typeof(Example_MultiWindowLayerManagement),
                picPath : "Assets/Editor/Examples/Example_17_MultiWindowLayerManagement/QQ截图20210419155222.png",
                videoPath : "");

        public override TrickOverViewInfo GetTrickOverViewInfo()
        {
            return TrickOverViewInfo;
        }
    }
}
