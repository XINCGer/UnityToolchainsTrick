using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class FileCapacityPostprocessor : AssetPostprocessor
{
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        FileCapacity.RefreshFileCapacity();
    }
}

public static class FileCapacity
{
    private const string REMOVE_STR = "Assets";
    private const string FILESIZE = "FileSize";

    private static readonly int mRemoveCount = REMOVE_STR.Length;
    private static readonly Color mColor = new Color(0.635f, 0.635f, 0.635f, 1);
    private static Dictionary<string, string> DirSizeDictionary = new Dictionary<string, string>();
    private static List<string> DirList = new List<string>();

    public static bool FileSizeEnable
    {
        get { return EditorPrefs.GetBool(FILESIZE, true); }
        set { EditorPrefs.SetBool(FILESIZE, value); }
    }
    
    [MenuItem("Tools/FileSize/OpenPlaySize")]
    private static void OpenPlaySize()
    {
        FileSizeEnable = true;
        GetPropjectDirs();
    }

    [MenuItem("Tools/FileSize/ClosePlaySize")]
    private static void ClosePlaySize()
    {
        FileSizeEnable = false;
        Init();
    }

    [InitializeOnLoadMethod]
    private static void InitializeOnLoadMethod()
    {
        Init();
#if UNITY_2018_1_OR_NEWER
        EditorApplication.projectChanged += GetPropjectDirs;
#endif
        EditorApplication.projectWindowItemOnGUI += OnGUI;
    }

    [UnityEditor.Callbacks.DidReloadScripts]
    private static void OnScriptsReloaded()
    {
        GetPropjectDirs();
    }

    public static void RefreshFileCapacity()
    {
        GetPropjectDirs();
    }
    
    private static void GetPropjectDirs()
    {
        Init();
        if (FileSizeEnable == false) return;
        GetAllDirecotries(Application.dataPath);
        foreach (string path in DirList)
        {
            string newPath = path.Replace("\\", "/");
            DirSizeDictionary.Add(newPath, GetFormatSizeString((int)GetDirectoriesSize(path)));
        }
    }
    private static void Init()
    {
        DirSizeDictionary.Clear();
        DirList.Clear();
    }

    private static void OnGUI(string guid, Rect selectionRect)
    {
        if (FileSizeEnable == false || selectionRect.height > 16) return;
        var dataPath = Application.dataPath;
        var startIndex = dataPath.LastIndexOf(REMOVE_STR);
        var dir = dataPath.Remove(startIndex, mRemoveCount);
        var path = dir + AssetDatabase.GUIDToAssetPath(guid);
        string text = null;
        if (DirSizeDictionary.ContainsKey(path))
        {
            text = DirSizeDictionary[path];
        }
        else if (File.Exists(path))
        {
            var fileInfo = new FileInfo(path);
            var fileSize = fileInfo.Length;
            text = GetFormatSizeString(fileSize);
        }
        else
        {
            return;
        }



        var label = EditorStyles.label;
        var content = new GUIContent(text);
        var width = label.CalcSize(content).x + 10;

        var pos = selectionRect;
        pos.x = pos.xMax - width;
        pos.width = width;
        pos.yMin++;

        var color = GUI.color;
        GUI.color = mColor;
        GUI.DrawTexture(pos, EditorGUIUtility.whiteTexture);
        GUI.color = color;
        GUI.Label(pos, text);
    }

    private static string GetFormatSizeString(long size)
    {
        return GetFormatSizeString(size, 1024);
    }

    private static string GetFormatSizeString(long size, int p)
    {
        return GetFormatSizeString(size, p, "#,##0.##");
    }

    private static string GetFormatSizeString(long size, int p, string specifier)
    {
        var suffix = new[] { "", "K", "M", "G", "T", "P", "E", "Z", "Y" };
        int index = 0;

        while (size >= p)
        {
            size /= p;
            index++;
        }

        return string.Format(
            "{0}{1}B",
            size.ToString(specifier),
            index < suffix.Length ? suffix[index] : "-"
        );
    }

    private static void GetAllDirecotries(string dirPath)
    {
        if (Directory.Exists(dirPath) == false)
        {
            return;
        }
        DirList.Add(dirPath);
        DirectoryInfo[] dirArray = new DirectoryInfo(dirPath).GetDirectories();
        foreach (DirectoryInfo item in dirArray)
        {
            GetAllDirecotries(item.FullName);
        }
    }

    private static long GetDirectoriesSize(string dirPath)
    {
        if (Directory.Exists(dirPath) == false)
        {
            return 0;
        }

        long size = 0;
        DirectoryInfo dir = new DirectoryInfo(dirPath);
        foreach (FileInfo info in dir.GetFiles())
        {
            size += info.Length;
        }

        DirectoryInfo[] dirBotton = dir.GetDirectories();
        foreach (DirectoryInfo info in dirBotton)
        {
            size += GetDirectoriesSize(info.FullName);
        }
        return size;
    }
}