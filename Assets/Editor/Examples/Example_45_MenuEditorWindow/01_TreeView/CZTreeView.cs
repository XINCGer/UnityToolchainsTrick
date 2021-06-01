#region 注 释
/***
 *
 *  Title:
 *  
 *  Description:
 *  
 *  Date:
 *  Version:
 *  Writer: 
 *
 */
#endregion
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace CZToolKit.Core.Editors
{
    public class CZTreeViewItem : TreeViewItem
    {
        public object userData;
    }

    public abstract class CZTreeView : TreeView
    {
        int itemCount = 0;

        protected List<TreeViewItem> items = new List<TreeViewItem>();
        protected Dictionary<int, CZTreeViewItem> treeViewItemMap = new Dictionary<int, CZTreeViewItem>();

        public float RowHeight { get => rowHeight; set => rowHeight = value; }
        public bool ShowBoder { get => showBorder; set => showBorder = value; }
        public bool ShowAlternatingRowBackgrounds { get => showAlternatingRowBackgrounds; set => showAlternatingRowBackgrounds = value; }

        public CZTreeView(TreeViewState state) : base(state) { }

        public CZTreeView(TreeViewState state, MultiColumnHeader multiColumnHeader) : base(state, multiColumnHeader) { }

        protected override TreeViewItem BuildRoot()
        {
            TreeViewItem root = new TreeViewItem(-1, -1, "Root");
            root.children = items;

            SetupDepthsFromParentsAndChildren(root);
            return root;
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            Rect rowRect = args.rowRect;
            rowRect.y += rowRect.height;
            rowRect.height = 1;
            EditorGUI.DrawRect(rowRect, new Color(0.5f, 0.5f, 0.5f, 1));

            CZTreeViewItem item = args.item as CZTreeViewItem;

            Rect labelRect = args.rowRect;
            if (hasSearch)
            {
                labelRect.x += depthIndentWidth;
                labelRect.width -= labelRect.x;
            }
            else
            {
                labelRect.x += item.depth * depthIndentWidth + depthIndentWidth;
                labelRect.width -= labelRect.x;
            }
            GUI.Label(labelRect, EditorGUIExtension.GetGUIContent(item.displayName, item.icon), EditorStylesExtension.LeftLabelStyle);
        }

        public void AddMenuItem<T>(string _path, T _treeViewItem) where T : CZTreeViewItem
        {
            if (string.IsNullOrEmpty(_path)) return;

            List<TreeViewItem> current = items;
            string[] path = _path.Split('/');
            if (path.Length > 1)
            {
                for (int i = 0; i < path.Length - 1; i++)
                {
                    CZMenuTreeViewItem currentParent = current.Find(t => t.displayName == path[i]) as CZMenuTreeViewItem;
                    if (currentParent == null)
                    {
                        currentParent = new CZMenuTreeViewItem();
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

            _treeViewItem.id = itemCount;
            _treeViewItem.displayName = path[path.Length - 1];
            _treeViewItem.children = new List<TreeViewItem>();

            current.Add(_treeViewItem);
            treeViewItemMap[itemCount] = _treeViewItem;
            itemCount++;
        }

        public void Remove(CZTreeViewItem _treeViewItem)
        {
            items.Remove(_treeViewItem as TreeViewItem);
            treeViewItemMap.Remove(_treeViewItem.id);
        }

        public CZTreeViewItem Find(int id)
        {
            CZTreeViewItem item = null;
            treeViewItemMap.TryGetValue(id, out item);
            return item;
        }

        public void Clear()
        {
            items.Clear();
            treeViewItemMap.Clear();
        }
    }
}
