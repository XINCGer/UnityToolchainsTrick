using AllTrickOverView.Core;

namespace AllTrickOverView.Examples
{
    public class Example_MenuEditorWindow : AExample_Base
    {
        public static TrickOverViewInfo TrickOverViewInfo =
            new TrickOverViewInfo("MenuEditorWindow",
                "右侧TreeView的EditorWindow",
                "EditorWindow",
                "using UnityEditor.IMGUI.Controls;\nusing UnityEditor;\nusing UnityEngine;\n\npublic class CZMenuEditorWindow : BasicMenuEditorWindow\n{\n    [MenuItem(\"Tools/MenuEditorWindow\")]\n    public static void Open()\n    {\n        GetWindow<CZMenuEditorWindow>();\n    }\n\n    protected override float LeftMinWidth { get { return 0; } }\n\n    protected override CZMenuTreeView BuildMenuTree(TreeViewState _treeViewState)\n    {\n        CZMenuTreeView treeView = new CZMenuTreeView(_treeViewState);\n\n        treeView.AddMenuItem(\"1\");\n        treeView.AddMenuItem(\"3\").rightDrawer = (_rect)=>{\n            GUILayout.Button(\"3\");\n            GUILayout.Button(\"4\");\n        };\n        treeView.AddMenuItem(\"3/5\");\n        treeView.AddMenuItem(\"2\");\n\n        return treeView;\n    }\n}\n",
                "Assets/Editor/Examples/Example_45_MenuEditorWindow",
                typeof(Example_MenuEditorWindow),
                picPath : "Assets/Editor/Examples/Example_45_MenuEditorWindow/1.png",
                videoPath : "");

        public override TrickOverViewInfo GetTrickOverViewInfo()
        {
            return TrickOverViewInfo;
        }
    }
}
