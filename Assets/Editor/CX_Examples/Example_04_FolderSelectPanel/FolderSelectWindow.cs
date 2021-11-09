using System;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace CX_Examples.Example_04
{
    public class FolderSelectWindow : EditorWindow
    {
        [SerializeField] private TreeViewState m_treeViewState;
        private FolderSelectView m_TreeView;
        
        [MenuItem("CX_Tools/FolderSelect")]
        private static void ShowWindow()
        {
            var window = GetWindow<FolderSelectWindow>();
            window.Show();
        }

        private void OnEnable()
        {
            if (m_treeViewState == null)
                m_treeViewState = new TreeViewState ();
            m_TreeView = new FolderSelectView (m_treeViewState);
            //DoTreeView();
        }

        private void OnGUI()
        {
            DoTreeView ();
            if (GUILayout.Button("Export"))
            {
                ShowSelectedFolder();
            }
        }
        
        void DoTreeView ()
        {
            Rect rect = GUILayoutUtility.GetRect (0, 100000, 0, 100000);
            m_TreeView.OnGUI(rect);
        }

        void ShowSelectedFolder()
        {
           var folders = m_TreeView.GetAllSelectFolderPath();
           foreach (var folder in folders)
           {
               Debug.Log(folder);
           }
        }
    }
}