using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CZToolKit.Core.Editors;
using UnityEditor.IMGUI.Controls;
using UnityEditor;
using System.Linq;

public class MenuEditorWindow : UnityObjectMenuEditorWindow
{
    [MenuItem("Tools/MenuEditorWindow")]
    public static void Open()
    {
        GetWindow<MenuEditorWindow>();
    }

    protected override CZMenuTreeView BuildMenuTree(TreeViewState _treeViewState)
    {
        CZMenuTreeView menuTree = new CZMenuTreeView(_treeViewState);
        menuTree.AddMenuItem("PlayerSetting", new CZMenuTreeViewItem() { userData = Resources.FindObjectsOfTypeAll<PlayerSettings>().FirstOrDefault() });
        return menuTree;
    }
}
