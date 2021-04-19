using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

[Serializable]
public abstract class BasicMenuEditorWindow : EditorWindow
{
    [SerializeField]
    ResizableArea resizableArea = new ResizableArea();
    protected Rect resizableAreaRect = new Rect(0, 0, 150, 150);

    string searchText;
    SearchField searchField;
    CZMenuTreeView menuTreeView;
    TreeViewState treeViewState = new TreeViewState();

    protected virtual float LeftMinWidth { get { return 50; } }
    protected virtual float RightMinWidth { get { return 500; } }

    void OnEnable()
    {
        resizableArea = new ResizableArea();
        resizableArea.minSize = new Vector2(LeftMinWidth, 50);
        resizableArea.side = 10;
        resizableArea.EnableSide(UIDirection.Right);
        resizableArea.SideOffset[UIDirection.Right] = resizableArea.side / 2;

        searchField = new SearchField();
        menuTreeView = BuildMenuTree(treeViewState);
        menuTreeView.Reload();
    }

    void OnGUI()
    {
        resizableArea.maxSize = position.size;

        resizableAreaRect.height = position.height;
        resizableAreaRect = resizableArea.OnGUI(resizableAreaRect);

        Rect searchFieldRect = resizableAreaRect;
        searchFieldRect.height = 20;
        searchFieldRect.y += 3;
        searchFieldRect.x += 5;
        searchFieldRect.width -= 10;
        string tempSearchText = searchField.OnGUI(searchFieldRect, searchText);
        if (tempSearchText != searchText)
        {
            searchText = tempSearchText;
            menuTreeView.searchString = searchText;
        }

        Rect treeviewRect = resizableAreaRect;
        treeviewRect.y += 20;
        treeviewRect.height -= 20;
        menuTreeView.OnGUI(treeviewRect);

        Rect sideRect = resizableAreaRect;
        sideRect.x += sideRect.width;
        sideRect.width = 1;
        EditorGUI.DrawRect(sideRect, new Color(0.5f, 0.5f, 0.5f, 1));

        Rect rightRect = new Rect(resizableAreaRect.width + 2, 0, position.width - resizableAreaRect.width - 4, position.height);
        rightRect.width = Mathf.Max(rightRect.width, RightMinWidth);
        GUILayout.BeginArea(rightRect);
        IList<int> selection = menuTreeView.GetSelection();
        if (selection.Count > 0)
            OnRightGUI(rightRect, menuTreeView.Find(selection[0]));
        GUILayout.EndArea();
    }

    protected abstract CZMenuTreeView BuildMenuTree(TreeViewState _treeViewState);

    protected virtual void OnRightGUI(Rect _rect, CZMenuTreeViewItem _selectedItem)
    {
        _selectedItem.rightDrawer?.Invoke(_rect);
    }
}

public class CZMenuTreeView : TreeView
{
    public static GUIStyle labelStyle;
    public static GUIStyle LabelStype
    {
        get
        {
            if (labelStyle == null)
            {
                labelStyle = new GUIStyle(GUI.skin.label);
                labelStyle.alignment = TextAnchor.MiddleLeft;
            }
            return labelStyle;
        }
    }

    List<TreeViewItem> items = new List<TreeViewItem>();
    Dictionary<int, CZMenuTreeViewItem> treeViewItemMap = new Dictionary<int, CZMenuTreeViewItem>();

    public CZMenuTreeView(TreeViewState state) : base(state)
    {
        rowHeight = 20;
#if !UNITY_2019_1_OR_NEWER
            foldoutOverride = DrawFoldout;
#endif
    }

    private bool DrawFoldout(Rect position, bool expandedState, GUIStyle style)
    {
        position.y += rowHeight / 2 - 8;
        return EditorGUI.Foldout(position, expandedState, "", style);
    }

    protected override TreeViewItem BuildRoot()
    {
        TreeViewItem root = new TreeViewItem(-1, -1, "Root");
        root.children = items;

        SetupDepthsFromParentsAndChildren(root);
        return root;
    }

    public CZMenuTreeViewItem AddMenuItem(string _path)
    {
        return AddMenuItem(_path, null);
    }

    int itemCount = 0;
    public CZMenuTreeViewItem AddMenuItem(string _path, Texture2D _icon)
    {
        if (string.IsNullOrEmpty(_path)) return null;
        List<TreeViewItem> current = items;
        string[] path = _path.Split('/');
        if (path.Length > 1)
        {
            for (int i = 0; i < path.Length - 1; i++)
            {
                CZMenuTreeViewItem currentParent = current.Find(t => t.displayName == path[i]) as CZMenuTreeViewItem;
                if (currentParent == null)
                {
                    currentParent = new CZMenuTreeViewItem(path[i]);
                    currentParent.children = new List<TreeViewItem>();
                    currentParent.displayName = path[i];
                    currentParent.id = itemCount;
                    current.Add(currentParent);
                    treeViewItemMap[itemCount] = currentParent;
                    itemCount++;
                }
                current = currentParent.children;
            }
        }

        CZMenuTreeViewItem item = new CZMenuTreeViewItem(_path);
        item.id = itemCount;
        item.displayName = path[path.Length - 1];
        item.children = new List<TreeViewItem>();
        item.icon = _icon;
        current.Add(item);
        treeViewItemMap[itemCount] = item;
        itemCount++;
        return item;
    }

    public string GetParentPath(string _path)
    {
        int index = _path.LastIndexOf('/');
        if (index == -1) return null;
        return _path.Substring(0, index);
    }

    public CZMenuTreeViewItem Find(int id)
    {
        CZMenuTreeViewItem item = null;
        treeViewItemMap.TryGetValue(id, out item);
        return item;
    }

    protected override bool CanRename(TreeViewItem item)
    {
        return false;
    }

    protected override bool CanMultiSelect(TreeViewItem item)
    {
        return false;
    }

    protected override void RenameEnded(RenameEndedArgs args)
    {
    }

    protected override void RowGUI(RowGUIArgs args)
    {
        Rect lineRect = args.rowRect;
        lineRect.y += lineRect.height;
        lineRect.height = 1;
        EditorGUI.DrawRect(lineRect, new Color(0.5f, 0.5f, 0.5f, 1));

        CZMenuTreeViewItem item = args.item as CZMenuTreeViewItem;

        Rect labelRect = args.rowRect;
        labelRect.x += item.depth * depthIndentWidth + depthIndentWidth;
        GUI.Label(labelRect, new GUIContent(item.displayName, item.icon), LabelStype);
        item.itemDrawer?.Invoke(args.rowRect);
    }
}

public class CZMenuTreeViewItem : TreeViewItem
{
    string path;
    public Action<Rect> itemDrawer;
    public Action<Rect> rightDrawer;

    public string Path { get { return path; } }

    public CZMenuTreeViewItem(string _path) : base()
    {
        path = _path;
    }
}
