using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;


#if UNITY_2019_1_OR_NEWER
public class SelectAssetReferenceWindow : AdvancedDropdown
{
    private static AssetReferenceDropdownState _assetReferenceDropdownState;
    private HashSet<string> _tempGuids;

    public static SelectAssetReferenceWindow Show(AssetReferenceDropdownState state, Rect rect)
    {
        _assetReferenceDropdownState = state;
        SelectAssetReferenceWindow window = new SelectAssetReferenceWindow(state);
        window.minimumSize = new Vector2(rect.width, 200);
        window.Show(rect);
        return window;
    }

    public SelectAssetReferenceWindow(AssetReferenceDropdownState state) : base(state)
    {
    }

    protected override AdvancedDropdownItem BuildRoot()
    {
        if (_tempGuids == null) _tempGuids = new HashSet<string>();
        _tempGuids.Clear();
        string[] sourceAssetSearchPaths = new[] {"Assets"};
        _tempGuids.UnionWith(AssetDatabase.FindAssets($"t:{_assetReferenceDropdownState.AssetType.Name}", sourceAssetSearchPaths));

        var root = new AssetReferenceDropdownFolderItem("Assets", "Assets", true);

        string[] assetGuids = new List<string>(_tempGuids).ToArray();
        for (int i = 0; i < assetGuids.Length; i++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(assetGuids[i]);


            var assetItem = new AssetReferenceDropdownAssetItem(assetPath, System.IO.Path.GetFileName(assetPath));

            //找到父节点
            AssetReferenceDropdownFolderItem parent = GetParentDropdownItem(root, assetItem);
            if (parent == null)
            {
                parent = CreateParentDropdownItem(root, assetItem);
            }

            parent.AddAsset(assetItem);
            assetItem.ParentFolder = parent;
        }

        return root;
    }

    /// <summary>
    /// 获取父节点
    /// </summary>
    /// <param name="root">根结点</param>
    /// <param name="child">子节点</param>
    /// <returns></returns>
    private AssetReferenceDropdownFolderItem GetParentDropdownItem(AssetReferenceDropdownFolderItem root, AssetReferenceDropdownAssetItem child)
    {
        return GetParentDropdownItem(root, child.FolderPath);
    }

    private AssetReferenceDropdownFolderItem GetParentDropdownItem(AssetReferenceDropdownFolderItem root, string folderPath)
    {
        if (root.IsRoot && root.Path == folderPath)
        {
            return root;
        }

        if (root.IsRoot == false && root.FolderPath == folderPath)
        {
            return root;
        }

        AssetReferenceDropdownFolderItem parent = root.GetFolder(folderPath) as AssetReferenceDropdownFolderItem;
        if (parent != null)
        {
            return parent;
        }

        foreach (AdvancedDropdownItem advancedDropdownItem in root.children)
        {
            if (advancedDropdownItem is AssetReferenceDropdownFolderItem)
            {
                parent = GetParentDropdownItem(advancedDropdownItem as AssetReferenceDropdownFolderItem,
                    (advancedDropdownItem as AssetReferenceDropdownFolderItem).Path);
                if (parent != null)
                {
                    break;
                }
            }
        }

        return parent;
    }

    private AssetReferenceDropdownFolderItem CreateParentDropdownItem(AssetReferenceDropdownFolderItem root, AssetReferenceDropdownAssetItem child)
    {
        string childFolderPath = child.FolderPath;

        //获取child的递进式父级文件夹相对路径
        string[] childProgressiveAssetFolderPath = PathUtility.GetProgressiveAssetFolderPath(childFolderPath);
        AssetReferenceDropdownFolderItem lastParent = root;
        if (childProgressiveAssetFolderPath != null)
        {
            for (int i = 0; i < childProgressiveAssetFolderPath.Length; i++)
            {
                AssetReferenceDropdownFolderItem advancedDropdownItem = GetParentDropdownItem(lastParent, childProgressiveAssetFolderPath[i]);
                if (advancedDropdownItem == null)
                {
                    var parent = new AssetReferenceDropdownFolderItem(childProgressiveAssetFolderPath[i], false);
                    lastParent.AddFolder(parent);
                    lastParent = parent;
                }
                else
                {
                    lastParent = advancedDropdownItem;
                }
            }
        }

        return lastParent;
    }

    protected override void ItemSelected(AdvancedDropdownItem item)
    {
        base.ItemSelected(item);
        if (item is AssetReferenceDropdownAssetItem)
        {
            AssetReferenceDropdownAssetItem assetReferenceDropdownItem = item as AssetReferenceDropdownAssetItem;

            if (_assetReferenceDropdownState != null && _assetReferenceDropdownState.Property != null)
            {
                _assetReferenceDropdownState.Property.objectReferenceValue =
                    AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(assetReferenceDropdownItem.AssetPath);

                _assetReferenceDropdownState.Property.serializedObject.ApplyModifiedProperties();
            }
        }
    }
}
#endif