using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using Object = UnityEngine.Object;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class HierarchyDataBaseDemo
{
    
    [MenuItem("Tools/Find In Hierarchy")]
    private static void FindInHierarchy(MenuCommand command)
    {
        var searchFilter = "t:RectTransform";
        List<HierarchyDataBase.GameObjectTreeViewItem> result = HierarchyDataBase.FindInHierarchy(searchFilter);
        for (int i = 0; i < result.Count; i++)
        {
            UnityEngine.Debug.LogError(result[i].objectPPTR);
        }
    }
}

public static class HierarchyDataBase
{
    public static void SetHierarchySearchFilter(string searchFilter)
    {
        var searchableWindows = Resources.FindObjectsOfTypeAll<SearchableEditorWindow>();
        foreach (var sw in searchableWindows)
        {
            var type = sw.GetType();
            var hierarchyTypeField = type.GetField("m_HierarchyType", BindingFlags.Instance | BindingFlags.NonPublic);
            var hierarchyType = (HierarchyType)hierarchyTypeField.GetValue(sw);
            if (hierarchyType != HierarchyType.GameObjects) continue;
            var setSearchFilterMethod =
                type.GetMethod("SetSearchFilter", BindingFlags.Instance | BindingFlags.NonPublic);
            setSearchFilterMethod.Invoke(sw,
                new object[] { searchFilter, SearchableEditorWindow.SearchMode.All, false, false });
            sw.Repaint();
        }
    }

    public static List<GameObjectTreeViewItem> FindInHierarchy(string searchFilter)
    {
        SetHierarchySearchFilter(searchFilter);
        Assembly editorAssembly = Assembly.Load("UnityEditor.dll");
        Type hierarchyWindowType = editorAssembly.GetType("UnityEditor.SceneHierarchyWindow");
        UnityEngine.Object[] hierarchyWindows = Resources.FindObjectsOfTypeAll(hierarchyWindowType);
        var sceneHierarchyField =
            hierarchyWindowType.GetField("m_SceneHierarchy", BindingFlags.Instance | BindingFlags.NonPublic);
        var treeViewField = editorAssembly.GetType("UnityEditor.SceneHierarchy")
            .GetField("m_TreeView", BindingFlags.Instance | BindingFlags.NonPublic);
        var treeViewControllerType = editorAssembly.GetType("UnityEditor.IMGUI.Controls.TreeViewController");
        var treeViewDataType = treeViewControllerType.GetProperty("data", BindingFlags.Instance | BindingFlags.Public);
        var getRowsMethod = editorAssembly.GetType("UnityEditor.GameObjectTreeViewDataSource")
            .GetMethod("GetRows", BindingFlags.Instance | BindingFlags.Public);
        var hierarchy = hierarchyWindows[0];
        object result = sceneHierarchyField.GetValue(hierarchy);
        object treeView = treeViewField.GetValue(result);
        object treeViewData = treeViewDataType.GetValue(treeView);
        object rows = getRowsMethod.Invoke(treeViewData, new object[] { });
        IList<TreeViewItem> treeViewItems = rows as IList<TreeViewItem>;
        List<GameObjectTreeViewItem> goTreeViewItems = new List<GameObjectTreeViewItem>();
        //去掉场景那个Item
        for (int i = 1; i < treeViewItems.Count; i++)
        {
            var treeViewItem = treeViewItems[i];
            GameObjectTreeViewItem goItem = new GameObjectTreeViewItem();
            var itemProperties = treeViewItem.GetType().GetProperties();
            foreach (PropertyInfo itemProperty in itemProperties)
            {
                if (itemProperty.CanWrite)
                {
                    goItem.GetType().GetProperty(itemProperty.Name, itemProperty.PropertyType)
                        .SetValue(goItem, itemProperty.GetValue(treeViewItem));
                }
            }

            goTreeViewItems.Add(goItem);
        }

        SetHierarchySearchFilter(string.Empty);
        return goTreeViewItems;
    }

    public class GameObjectTreeViewItem : TreeViewItem
    {
        int m_ColorCode;
        Object m_ObjectPPTR;
        Scene m_UnityScene;

        bool m_LazyInitializationDone;
        bool m_ShowPrefabModeButton;
        Texture2D m_OverlayIcon;
        Texture2D m_SelectedIcon;

        public int colorCode
        {
            get { return m_ColorCode; }
            set { m_ColorCode = value; }
        }

        public Object objectPPTR
        {
            get { return m_ObjectPPTR; }
            set { m_ObjectPPTR = value; }
        }

        public bool lazyInitializationDone
        {
            get { return m_LazyInitializationDone; }
            set { m_LazyInitializationDone = value; }
        }

        public bool showPrefabModeButton
        {
            get { return m_ShowPrefabModeButton; }
            set { m_ShowPrefabModeButton = value; }
        }

        public Texture2D overlayIcon
        {
            get { return m_OverlayIcon; }
            set { m_OverlayIcon = value; }
        }

        public Texture2D selectedIcon
        {
            get { return m_SelectedIcon; }
            set { m_SelectedIcon = value; }
        }

        public bool isSceneHeader { get; set; }

        public Scene scene
        {
            get { return m_UnityScene; }
            set { m_UnityScene = value; }
        }
    }
}