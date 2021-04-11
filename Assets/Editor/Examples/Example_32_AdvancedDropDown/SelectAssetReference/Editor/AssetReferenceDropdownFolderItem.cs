using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;


#if UNITY_2019_1_OR_NEWER
public class AssetReferenceDropdownFolderItem : AdvancedDropdownItem
{
    /// <summary>
    /// 文件夹相对于Assets目录的局部路径
    /// </summary>
    public string Path { get; }

    public string FolderPath { get; }
    public bool IsRoot { get; }

    private readonly List<AssetReferenceDropdownFolderItem> _folders;
    private readonly List<AssetReferenceDropdownAssetItem> _assetItems;

    private static Texture s_CachedIcon = null;

    public static Texture Icon
    {
        get
        {
            if (s_CachedIcon == null)
            {
                s_CachedIcon = AssetDatabase.GetCachedIcon("Assets");
            }

            return s_CachedIcon;
        }
    }

    public AssetReferenceDropdownFolderItem(string path, string name, bool isRoot) : base(name)
    {
        this.Path = path;
        this.IsRoot = isRoot;
        if (isRoot == false)
        {
            this.FolderPath = PathUtility.GetAssetDirectoryName(path);
        }

        _folders = new List<AssetReferenceDropdownFolderItem>();
        _assetItems = new List<AssetReferenceDropdownAssetItem>();
    }

    public AssetReferenceDropdownFolderItem(string path, bool isRoot) : this(path, path.Substring(path.LastIndexOf("/") + 1), isRoot)
    {

    }

    /// <summary>
    /// 加子文件夹
    /// </summary>
    /// <param name="child"></param>
    public void AddFolder(AssetReferenceDropdownFolderItem child)
    {
        _folders.Add(child);
        AddChild(child);
    }

    public AssetReferenceDropdownFolderItem GetFolder(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            throw new Exception("Source folder name is invalid.");
        }

        foreach (AssetReferenceDropdownFolderItem folder in _folders)
        {
            if (folder.Path == path)
            {
                return folder;
            }
        }

        return null;
    }

    /// <summary>
    /// 加Asset
    /// </summary>
    /// <param name="child"></param>
    public void AddAsset(AssetReferenceDropdownAssetItem child)
    {
        _assetItems.Add(child);
        AddChild(child);
    }
}
#endif