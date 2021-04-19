using System;
using System.IO;
using UnityEngine;


/// <summary>路径相关的实用函数。</summary>
public static class PathUtility
{

  /// <summary>获取规范的路径。</summary>
  /// <param name="path">要规范的路径。</param>
  /// <returns>规范的路径。</returns>
  public static string GetRegularPath(string path)
  {
    return path?.Replace('\\', '/');
  }

  /// <summary>获取连接后的路径。</summary>
  /// <param name="path">路径片段。</param>
  /// <returns>连接后的路径。</returns>
  public static string GetCombinePath(params string[] path)
  {
    if (path == null || path.Length < 1)
      return (string) null;
    string str = path[0];
    for (int index = 1; index < path.Length; ++index)
      str = System.IO.Path.Combine(str, path[index]);
    return PathUtility.GetRegularPath(str);
  }

  public static string GetCombinePath(int startIndex, int endIndex, params string[] path)
  {
    if (path == null || path.Length < 1)
      return (string) null;
    if (startIndex < 0 || startIndex >= path.Length - 1 || endIndex < 0 || endIndex > path.Length - 1 || startIndex > endIndex)
    {
      return (string) null;
    }

    string str = path[startIndex];
    for (int index = startIndex + 1; index <= endIndex; ++index)
      str = System.IO.Path.Combine(str, path[index]);
    return PathUtility.GetRegularPath(str);
  }

  /// <summary>获取远程格式的路径（带有file:// 或 http:// 前缀）。</summary>
  /// <param name="path">原始路径。</param>
  /// <returns>远程格式路径。</returns>
  public static string GetRemotePath(params string[] path)
  {
    string combinePath = PathUtility.GetCombinePath(path);
    if (combinePath == null)
      return (string) null;
    if (!combinePath.Contains("://"))
      return ("file:///" + combinePath).Replace("file:////", "file:///");
    return combinePath;
  }

  /// <summary>获取带有后缀的资源名。</summary>
  /// <param name="resourceName">原始资源名。</param>
  /// <returns>带有后缀的资源名。</returns>
  public static string GetResourceNameWithSuffix(string resourceName)
  {
    if (string.IsNullOrEmpty(resourceName))
      throw new Exception("Resource name is invalid.");
    return string.Format("{0}.dat", (object) resourceName);
  }

  /// <summary>获取带有 CRC32 和后缀的资源名。</summary>
  /// <param name="resourceName">原始资源名。</param>
  /// <param name="hashCode">CRC32 哈希值。</param>
  /// <returns>带有 CRC32 和后缀的资源名。</returns>
  public static string GetResourceNameWithCrc32AndSuffix(string resourceName, int hashCode)
  {
    if (string.IsNullOrEmpty(resourceName))
      throw new Exception("Resource name is invalid.");
    return string.Format("{0}.{1:x8}.dat", (object) resourceName, (object) hashCode);
  }

  /// <summary>移除空文件夹。</summary>
  /// <param name="directoryName">要处理的文件夹名称。</param>
  /// <returns>是否移除空文件夹成功。</returns>
  public static bool RemoveEmptyDirectory(string directoryName)
  {
    if (string.IsNullOrEmpty(directoryName))
      throw new Exception("Directory name is invalid.");
    try
    {
      if (!Directory.Exists(directoryName))
        return false;
      string[] directories = Directory.GetDirectories(directoryName, "*");
      int length = directories.Length;
      foreach (string directoryName1 in directories)
      {
        if (RemoveEmptyDirectory(directoryName1))
          --length;
      }

      if (length > 0 || Directory.GetFiles(directoryName, "*").Length != 0)
        return false;
      Directory.Delete(directoryName);
      return true;
    }
    catch
    {
      return false;
    }
  }

  /// <summary>
  /// 获取Assets下的相对目录
  /// </summary>
  /// <param name="path">完整目录</param>
  /// <param name="applicationDataPath">applicationDataPath</param>
  /// <returns></returns>
  public static string GetAssetsRelativePath(string path, string applicationDataPath)
  {
    if (path.StartsWith(applicationDataPath))
    {
      return path.Replace(applicationDataPath, "Assets");
    }

    return null;
  }

  public static string GetAssetFolderFullPath(string assetPath)
  {
    return Application.dataPath + assetPath.Substring("Assets".Length);
  }

  public static string GetAssetDirectoryName(string assetPath)
  {
    return System.IO.Path.GetDirectoryName(assetPath);
  }

  public static string[] GetProgressiveAssetFolderPath(string assetFolderPath)
  {
    if (assetFolderPath.IndexOf(Path.DirectorySeparatorChar) != -1)
    {
      string[] folderName = assetFolderPath.Split(Path.DirectorySeparatorChar);
      string[] result = new string[folderName.Length];
      for (int i = 0; i < folderName.Length; i++)
      {
        result[i] = GetCombinePath(0, i, folderName);
      }

      return result;
    }

    return null;
  }
}