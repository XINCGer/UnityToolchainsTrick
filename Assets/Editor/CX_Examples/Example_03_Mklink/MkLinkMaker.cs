//---------------------------------------------------------------------------------------
// Author: chenxuan@bolygon.com
// Date: 2021-11-09 00:00:00
// Name: MkLinkMaker
//---------------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace MkLink
{
    internal class MkLinkMaker : EditorWindow
    {
        private static string BATPath => Application.dataPath + "/Editor/CX_Examples/Example_03_Mklink/mklink.bat";
        [MenuItem("Assets/MkLink")]
        public static void MkLinkFormProject()
        {
            var select = Selection.activeObject;
            var path = AssetDatabase.GetAssetPath(select);
            if (!Directory.Exists(path))
            {
                EditorUtility.DisplayDialog("Error", "Please select floder", "ok");
                return;
            }
            var targetFolder = EditorUtility.OpenFolderPanel("Select TargetFolder", String.Empty, String.Empty);
            if(string.IsNullOrEmpty(targetFolder)) return;
            CreateMklink(Path.GetFullPath(path), targetFolder);
        }

        private static void CreateMklink(string origin, string targetFolder)
        {
            //批处理中斜杠代表指令
            origin = origin.Replace('/', '\\');
            targetFolder = targetFolder.Replace('/', '\\');
            var folderName = Path.GetFileName(origin);
            Process pro = new Process();
            pro.StartInfo.FileName = BATPath;
            pro.StartInfo.Arguments = targetFolder + $"\\{folderName}" + '\t' + origin;
            pro.StartInfo.CreateNoWindow = false;
            pro.Start();
            pro.WaitForExit();
            Debug.Log("执行完毕");
        }
    }
}