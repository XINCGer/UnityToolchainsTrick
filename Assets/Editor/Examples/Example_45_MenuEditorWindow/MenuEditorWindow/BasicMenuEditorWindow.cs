using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace CZToolKit.Core.Editors
{
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

        Rect rightRect;
        Vector2 rightScroll;

        protected virtual float LeftMinWidth
        {
            get
            {
                return 50;
            }
        }
        protected virtual float RightMinWidth
        {
            get
            {
                return 500;
            }
        }
        protected Rect RightRect
        {
            get
            {
                return rightRect;
            }
        }

        protected virtual void OnEnable()
        {
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

            resizableArea.maxSize = position.size;
            resizableAreaRect.height = position.height;
            resizableAreaRect = resizableArea.OnGUI(resizableAreaRect);

            Rect treeviewRect = resizableAreaRect;
            treeviewRect.y += searchFieldRect.height;
            treeviewRect.height -= searchFieldRect.height;
            menuTreeView.OnGUI(treeviewRect);

            Rect sideRect = resizableAreaRect;
            sideRect.x += sideRect.width;
            sideRect.width = 1;
            EditorGUI.DrawRect(sideRect, new Color(0.5f, 0.5f, 0.5f, 1));

            rightRect = sideRect;
            rightRect.x += rightRect.width + 1;
            rightRect.width = position.width - resizableAreaRect.width - sideRect.width - 2;
            rightRect.width = Mathf.Max(rightRect.width, RightMinWidth);

            GUILayout.BeginArea(rightRect);
            rightRect.x = 0;
            rightRect.y = 0;
            IList<int> selection = menuTreeView.GetSelection();
            if (selection.Count > 0)
            {
                rightScroll = GUILayout.BeginScrollView(rightScroll, false, false);
                OnRightGUI(menuTreeView.Find(selection[0]) as CZMenuTreeViewItem);
                GUILayout.EndScrollView();
            }
            GUILayout.EndArea();
        }

        protected abstract CZMenuTreeView BuildMenuTree(TreeViewState _treeViewState);

        protected virtual void OnRightGUI(CZMenuTreeViewItem _selectedItem) { }
    }

    public class CZMenuTreeView : CZTreeView
    {
        public CZMenuTreeView(TreeViewState state) : base(state)
        {
            rowHeight = 30;
#if !UNITY_2019_1_OR_NEWER
            customFoldoutYOffset = rowHeight / 2 - 8;
#endif
        }

        public T AddMenuItem<T>(string _path) where T : CZMenuTreeViewItem, new()
        {
            return AddMenuItem<T>(_path, (Texture2D)null);
        }

        public T AddMenuItem<T>(string _path, Texture2D _icon) where T : CZMenuTreeViewItem, new()
        {
            if (string.IsNullOrEmpty(_path))
                return null;
            T item = new T();
            item.icon = _icon;
            return item;
        }

        public string GetParentPath(string _path)
        {
            int index = _path.LastIndexOf('/');
            if (index == -1)
                return null;
            return _path.Substring(0, index);
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
            base.RowGUI(args);
            CZMenuTreeViewItem item = args.item as CZMenuTreeViewItem;
			if(item != null)
				item.itemDrawer?.Invoke(args.rowRect, item);
        }
    }

    public class CZMenuTreeViewItem : CZTreeViewItem
    {
        public Action<Rect, CZMenuTreeViewItem> itemDrawer;
        public CZMenuTreeViewItem() : base() { }
    }
}
