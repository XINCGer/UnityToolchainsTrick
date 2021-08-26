using UnityEngine;
using UnityEditor.IMGUI.Controls;


namespace UnityEditor.TreeViewExamples
{
	
	class TransformTreeWindow : EditorWindow
	{
		[SerializeField] TreeViewState m_TreeViewState;

		TreeView m_TreeView;

		[MenuItem("TreeView Examples/Transform Hierarchy")]
		static void ShowWindow()
		{
			var window = GetWindow<TransformTreeWindow>();
			window.titleContent = new GUIContent("My Hierarchy");
			window.Show();
		}

		void OnEnable ()
		{
			if (m_TreeViewState == null)
				m_TreeViewState = new TreeViewState ();

			m_TreeView = new TransformTreeView (m_TreeViewState);
		}

		void OnSelectionChange ()
		{
			if (m_TreeView != null)
				m_TreeView.SetSelection (Selection.instanceIDs);
			Repaint ();
		}

		void OnHierarchyChange()
		{
			if (m_TreeView != null)
				m_TreeView.Reload();
			Repaint ();
		}

		void OnGUI ()
		{
			DoToolbar ();
			DoTreeView ();
			
		}
		
		void DoTreeView ()
		{
			Rect rect = GUILayoutUtility.GetRect (0, 100000, 0, 100000);
			m_TreeView.OnGUI(rect);
		}

		void DoToolbar()
		{
			GUILayout.BeginHorizontal(EditorStyles.toolbar);
			GUILayout.FlexibleSpace ();
			GUILayout.EndHorizontal();
		}
	}
}
