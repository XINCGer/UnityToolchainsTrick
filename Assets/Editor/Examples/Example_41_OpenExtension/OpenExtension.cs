using System;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 打开文件以及文件夹的扩展
/// </summary>
public static class OpenExtension
{
    /// <summary>
    /// 打开 Data Path 文件夹。
    /// </summary>
    [MenuItem("Assets/Open Folder/Data Path", false, 10)]
    public static void OpenFolderDataPath()
    {
        Execute(Application.dataPath);
    }

    /// <summary>
    /// 打开 Persistent Data Path 文件夹。
    /// </summary>
    [MenuItem("Assets/Open Folder/Persistent Data Path", false, 11)]
    public static void OpenFolderPersistentDataPath()
    {
        Execute(Application.persistentDataPath);
    }

    /// <summary>
    /// 打开 Streaming Assets Path 文件夹。
    /// </summary>
    [MenuItem("Assets/Open Folder/Streaming Assets Path", false, 12)]
    public static void OpenFolderStreamingAssetsPath()
    {
        Execute(Application.streamingAssetsPath);
    }

    /// <summary>
    /// 打开 Temporary Cache Path 文件夹。
    /// </summary>
    [MenuItem("Assets/Open Folder/Temporary Cache Path", false, 13)]
    public static void OpenFolderTemporaryCachePath()
    {
        Execute(Application.temporaryCachePath);
    }

    /// <summary>
    /// 打开指定路径的文件夹。
    /// </summary>
    /// <param name="folder">要打开的文件夹的路径。</param>
    public static void Execute(string folder)
    {
        folder = string.Format("\"{0}\"", folder);
        switch (Application.platform)
        {
            case RuntimePlatform.WindowsEditor:
                Process.Start("Explorer.exe", folder.Replace('/', '\\'));
                break;

            case RuntimePlatform.OSXEditor:
                Process.Start("open", folder);
                break;

            default:
                throw new Exception(string.Format("Not support open folder on '{0}' platform.",
                    Application.platform.ToString()));
        }
    }

    [MenuItem("Assets/Open Files/OpenExcel")]
    public static void OpenExcel()
    {
        string[] suffixs = new[]
        {
            "xlsx",
            "xls",
            "csv",
        };
        var objects = Selection.objects;
        if (objects != null && objects.Length > 0)
        {
            foreach (var o in objects)
            {
                string path = AssetDatabase.GetAssetPath(o);
                if (o is TextAsset)
                {
                    string suffix = path.Substring(path.LastIndexOf(".") + 1);
                    if (Array.IndexOf(suffixs, suffix) == -1)
                    {
                        UnityEngine.Debug.LogWarningFormat("selection file {0} , suffixes not supported", path);
                        continue;
                    }
                    path = string.Format("\"{0}\"", path);
                    switch (Application.platform)
                    {
                        case RuntimePlatform.WindowsEditor:
                            Process.Start("excel", path.Replace('/', '\\'));
                            break;

                        case RuntimePlatform.OSXEditor:
                            Process.Start("open", path);
                            break;
                        default:
                            throw new Exception(string.Format("Not support open folder on '{0}' platform.",
                                Application.platform.ToString()));
                    }
                }
                else
                {
                    UnityEngine.Debug.LogWarningFormat("selection file {0} , suffixes not supported", path);
                }
            }
        }
    }
}