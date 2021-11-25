//---------------------------------------------------------------------------------------
// Author: xuan_chen_work@foxmail.com
// Date: 2021-11-25 15:52:46
// Name: MkLinkMaker
//---------------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace MkLink
{
    internal class MkLinkMaker : EditorWindow
    {
        [SerializeField] private TreeViewState m_treeViewState;
        private FolderSelectView m_TreeView;
        private static string BATPath => Application.dataPath + "/Editor/CX_Examples/Example_03_Mklink/mklink.bat";
        //private static string BATPath => Application.dataPath + "/mklink.bat";
        
        [MenuItem("CX_Tools/MkLink", false,3)]
        public static void OpenWindow()
        {
            var win = EditorWindow.GetWindow<MkLinkMaker>("MkLinker");
            win.Show();
        }

        private void OnEnable()
        {
            if (m_treeViewState == null)
                m_treeViewState = new TreeViewState ();
            m_TreeView = new FolderSelectView (m_treeViewState);
            //DoTreeView();
        }

        private void OnDisable()
        {
            EditorUtility.ClearProgressBar();
        }

        private void OnGUI()
        {
            DoTreeView ();
            if (GUILayout.Button("Create MkLink"))
            {
                CreateMkLink();
            }
        }
        
        void DoTreeView ()
        {
            Rect rect = GUILayoutUtility.GetRect (0, 100000, 0, 100000);
            m_TreeView.OnGUI(rect);
        }

        void CreateMkLink()
        {
            var target = EditorUtility.OpenFolderPanel("Select Target Assets", string.Empty, string.Empty);
            if(string.IsNullOrEmpty(target)) return;
            var folders = m_TreeView.GetAllSelectFolderPath();
            for (var i = 0; i < folders.Count; i++)
            {
                var assetPath = Application.dataPath.Replace('/', '\\');
                var folder = folders[i].Replace(assetPath, string.Empty);
                var targetFolder = target + folder;
                EditorUtility.DisplayProgressBar("Create Link...", $"{folders[i]} LINK TO {targetFolder}",(float)i / folders.Count);
                try
                {
                    CreateMklink(folders[i], targetFolder);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Failed: {folders[i]} LINK TO {targetFolder} :{e.Message}");
                }
                
            }
            EditorUtility.ClearProgressBar();
        }

        private static void CreateMklink(string origin, string targetFolder)
        {
#if !UNITY_EDITOR_WIN
            Debug.LogError("暂不支持Win以外平台");
            return;
#endif
            //批处理中斜杠代表指令
            origin = origin.Replace('/', '\\');
            targetFolder = targetFolder.Replace('/', '\\');
            if (!Directory.Exists(Path.GetDirectoryName(targetFolder)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(targetFolder) ?? string.Empty);
            }

            Process pro = new Process();
            pro.StartInfo.FileName = BATPath;
            //以防有空格
            origin ='\"' + origin + '\"';
            targetFolder = '\"' + targetFolder + '\"';
            Debug.Log($"{origin} LINK TO {targetFolder}");
            pro.StartInfo.Arguments = targetFolder + '\t' + origin;
            pro.StartInfo.CreateNoWindow = true;
            pro.Start();
            pro.WaitForExit();
            Debug.Log("执行完毕");
        }
        
        //[MenuItem("Assets/MkLink")]
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
            targetFolder += '\\' + Path.GetFileName(path);
            if(string.IsNullOrEmpty(targetFolder)) return;
            CreateMklink(Path.GetFullPath(path), targetFolder);
        }
    }
}