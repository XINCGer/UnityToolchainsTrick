using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class CollectPath : EditorWindow
{
    [MenuItem("MyPackageFolder/Open")]
    static void OpenWin()
    {
        var win = GetWindow<CollectPath>();
        win.Show();
    }

    string _sharedFolder = string.Empty;
    string _uploadReportFolder;

    void OnGUI()
    {
        _sharedFolder = EditorGUILayout.TextField(new GUIContent("共享文件夹:"), _sharedFolder);
        EditorGUILayout.Space(2);
        if (GUILayout.Button("生成报告"))
        {
            GenAABBCC();
        }
    }

    void GenAABBCC()
    {
        if (Directory.Exists(_sharedFolder))
        {
            var dirInfo = new DirectoryInfo(_sharedFolder);
            var paths = new List<string>();
            foreach (var fileInfo in dirInfo.GetFiles())
            {
                if (fileInfo.Extension == ".unitypackage")
                {
                    
                    if (paths.Contains(fileInfo.FullName) == false)
                    {
                        Debug.LogWarning("添加了: " + fileInfo.FullName);
                        paths.Add(fileInfo.FullName);
                    }
                }
            }

            Debug.LogWarning($"一共找到了: {paths.Count}");

            File.WriteAllLines(@"C:\newlist.txt", paths);
        }
    }

}

