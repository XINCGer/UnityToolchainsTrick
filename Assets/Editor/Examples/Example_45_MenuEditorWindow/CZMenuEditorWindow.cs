using UnityEditor.IMGUI.Controls;
using UnityEditor;
using UnityEngine;

public class CZMenuEditorWindow : BasicMenuEditorWindow
{
    [MenuItem("Tools/MenuEditorWindow")]
    public static void Open()
    {
        GetWindow<CZMenuEditorWindow>();
    }

    protected override float LeftMinWidth { get { return 0; } }

    protected override CZMenuTreeView BuildMenuTree(TreeViewState _treeViewState)
    {
        CZMenuTreeView treeView = new CZMenuTreeView(_treeViewState);

        treeView.AddMenuItem("1");
        treeView.AddMenuItem("3").rightDrawer = (_rect)=>{
            GUILayout.Button("3");
            GUILayout.Button("4");
        };
        treeView.AddMenuItem("3/5");
        treeView.AddMenuItem("2");

        return treeView;
    }
}
