using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace MkLink
{
    public class FolderData
    {
        public string FullPath { get; set; }
        public TreeViewItem Item { get; set; }
        public bool IsSelected { get; set; }
    }

    public class FolderSelectView : UnityEditor.IMGUI.Controls.TreeView
    {
        private List<FolderData> _folderDatas;

        public FolderSelectView(TreeViewState state) : base(state)
        {
            _folderDatas = new List<FolderData>();
            Reload();
        }

        protected override TreeViewItem BuildRoot()
        {
            return new TreeViewItem {id = 0, depth = -1};
        }

        protected override IList<TreeViewItem> BuildRows(TreeViewItem root)
        {
            var rows = GetRows() ?? new List<TreeViewItem>(200);
            _folderDatas ??= new List<FolderData>();
            //_folderDatas.Clear();
            rows.Clear();
            var rootParent = CreateTreeViewItemFormDirPath(Application.dataPath);
            rows.Add(rootParent);
            SetFolderLayout(ref rows, ref rootParent, Application.dataPath);
            SetupDepthsFromParentsAndChildren(rootParent);
            return rows;
        }

        private void SetFolderLayout(ref IList<TreeViewItem> rowRoot,ref TreeViewItem parent, string parentPath)
        {
            var dirInfo = new DirectoryInfo(parentPath);
            var subFolders = dirInfo.GetDirectories();
            if(subFolders.Length <= 0) return;
            foreach (var subfolder in subFolders)
            {
                var subF = CreateTreeViewItemFormDirPath(subfolder.FullName);
                parent.AddChild(subF);
                rowRoot.Add(subF);
                if (IsExpanded (subF.id))
                {
                    SetFolderLayout(ref rowRoot,ref subF, subfolder.FullName);
                }
                else
                {
                    subF.children = CreateChildListForCollapsedParent ();
                }
                
            }
        }

        private TreeViewItem CreateTreeViewItemFormDirPath(string path)
        {
            int id = _folderDatas.FindIndex(0,item => item.FullPath == path);
            FolderData folderData;
            if ( id == -1)
            {
                folderData = new FolderData{FullPath = path, IsSelected = false};
                _folderDatas.Add(folderData);
                id = _folderDatas.Count - 1;
            }
            else
            {
                folderData = GetFolderData(id);
            }
            var res = new TreeViewItem(id, -1, Path.GetFileName(path));
            folderData.Item = res;
            return res;
        }
        
        protected override IList<int> GetAncestors (int id)
        {
            // The backend needs to provide us with this info since the item with id
            // may not be present in the rows
            var folderData = GetFolderData(id);
        
            List<int> ancestors = new List<int> ();
            var item = folderData.Item;
            while (item.parent.id > 0)
            {
                ancestors.Add(folderData.Item.parent.id);
                item = item.parent;
            }
        
            return ancestors;
        }
        
        protected override IList<int> GetDescendantsThatHaveChildren (int id)
        {
            Stack<FolderData> stack = new Stack<FolderData> ();
        
            var start = GetFolderData(id);
            stack.Push (start);
        
            var parents = new List<int> ();
            while (stack.Count > 0)
            {
                FolderData current = stack.Pop ();
                parents.Add (current.Item.id);
                for (int i = 0; i < current.Item.children.Count; i++)
                {
                    stack.Push(GetFolderData(current.Item.children[i].id));
                }
            }
        
            return parents;
        }

        private FolderData GetFolderData(int id)
        {
            return _folderDatas[id];
        }

        //Custom GUI
        protected override void RowGUI(RowGUIArgs args)
        {
            Event evt = Event.current;
            var folderData = GetFolderData(args.item.id);
            extraSpaceBeforeIconAndLabel = 18f;
            Rect toggleRect = args.rowRect;
            toggleRect.x += GetContentIndent(args.item);
            toggleRect.width = 16f;
            if (evt.type == EventType.MouseDown && toggleRect.Contains(evt.mousePosition))
                SelectionClick(args.item, false);
            if (args.item.id != 0)
            {
                EditorGUI.BeginChangeCheck ();
                var isSelect = EditorGUI.Toggle(toggleRect, folderData.IsSelected);
                if (EditorGUI.EndChangeCheck())
                    SetSelectFolder(folderData, isSelect);
            }
            // Text
            base.RowGUI(args);
        }

        private void SetSelectFolder(FolderData data, bool isSelected)
        {
            if (isSelected == false)
            {
                data.IsSelected = false;
                return;
            }

            var item = data.Item;
            bool isParentSelected = false;
            while (item.parent.id > 0)
            {
                var parentData = GetFolderData(item.parent.id);
                if (parentData.IsSelected)
                {
                    isParentSelected = true;
                    break;
                }
                item = parentData.Item;
            }
            if(isParentSelected) return;
            data.IsSelected = true;
            ClearChildrenSelect(data.Item.children);
        }

        private void ClearChildrenSelect(List<TreeViewItem> childrenItem)
        {
            if(childrenItem.Count <= 0) return;
            foreach (var item in childrenItem)
            {
                if(item == null) continue;
                var folderData = GetFolderData(item.id);
                folderData.IsSelected = false;
                ClearChildrenSelect(item.children);
            }
        }

        public List<string> GetAllSelectFolderPath()
        {
            var res = new List<string>();
            foreach (var data in _folderDatas)
            {
                if(data.IsSelected)
                    res.Add(data.FullPath);
            }
            return res;
        }
    }
}