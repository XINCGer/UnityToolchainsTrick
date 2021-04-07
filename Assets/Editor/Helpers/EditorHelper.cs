using System.Collections;
using System.Collections.Generic;
using System.IO;
using ColaFramework.Foundation;
using UnityEditor;
using UnityEngine;

public class EditorHelper
{
    public static string m_projectRoot;
    public static string m_projectRootWithSplit;

    /// <summary>
    /// 编辑器会用到的一些临时目录
    /// </summary>
    public static string TempCachePath
    {
        get { return Path.Combine(Application.dataPath, "../Cache"); }
    }

    public static string ProjectRoot
    {
        get
        {
            if (string.IsNullOrEmpty(m_projectRoot))
            {
                m_projectRoot = FileHelper.FormatPath(Path.GetDirectoryName(Application.dataPath));
            }

            return m_projectRoot;
        }
    }

    public static string ProjectRootWithSplit
    {
        get
        {
            if (string.IsNullOrEmpty(m_projectRootWithSplit))
            {
                m_projectRootWithSplit = ProjectRoot + "/";
            }

            return m_projectRootWithSplit;
        }
    }

    /// <summary>
    /// 打开指定文件夹(编辑器模式下)
    /// </summary>
    /// <param name="path"></param>
    public static void OpenDirectory(string path)
    {
        if (string.IsNullOrEmpty(path)) return;

        path = path.Replace("/", "\\");
        if (!Directory.Exists(path))
        {
            Debug.LogError("No Directory: " + path);
            return;
        }

        if (!path.StartsWith("file://"))
        {
            path = "file://" + path;
        }

        Application.OpenURL(path);
    }

    public static T GetScriptableObjectAsset<T>(string path) where T : ScriptableObject
    {
        var asset = AssetDatabase.LoadAssetAtPath<T>(path);
        if (asset == null)
        {
            asset = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();
        }

        return asset;
    }
    
    public static GameObject InstantiateGoByPrefab(GameObject prefab, GameObject parent)
    {
        if (null == prefab) return null;
        GameObject obj = GameObject.Instantiate(prefab);
        if (null == obj) return null;
        obj.name = prefab.name;
        if (null != parent)
        {
            obj.transform.SetParent(parent.transform, false);
        }
        obj.transform.localPosition = prefab.transform.localPosition;
        obj.transform.localRotation = prefab.transform.localRotation;
        obj.transform.localScale = prefab.transform.localScale;
        return obj;
    }
}
