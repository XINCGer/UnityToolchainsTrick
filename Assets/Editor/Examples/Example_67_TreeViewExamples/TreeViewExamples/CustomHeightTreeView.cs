using UnityEditor.IMGUI.Controls;
using UnityEngine;


namespace UnityEditor.TreeViewExamples
{

	internal class CustomHeightTreeView : TreeViewWithTreeModel<MyTreeElement>
	{
		static class Styles
		{
			public static GUIStyle background = "RL Background";
			public static GUIStyle headerBackground = "RL Header";
		}
		
		public CustomHeightTreeView(TreeViewState state, TreeModel<MyTreeElement> model)
			: base(state, model)
		{
			// Custom setup
			showBorder = true;
			customFoldoutYOffset = 3f;

			Reload();
		}

		protected override float GetCustomRowHeight (int row, TreeViewItem item)
		{
			var myItem = (TreeViewItem<MyTreeElement>)item;
			
			if (myItem.data.enabled)
				return 85f;
			
			return 30f;
		}

		public override void OnGUI (Rect rect)
		{
			// Background
			if (Event.current.type == EventType.Repaint)
				DefaultStyles.backgroundOdd.Draw(rect, false, false, false, false);

			// TreeView
			base.OnGUI (rect);
		}

		protected override void RowGUI (RowGUIArgs args)
		{
			var item = (TreeViewItem<MyTreeElement>) args.item;
			var contentIndent = GetContentIndent (item);

			// Background
			var bgRect = args.rowRect;
			bgRect.x = contentIndent;
			bgRect.width = Mathf.Max (bgRect.width - contentIndent, 155f) - 5f;
			bgRect.yMin += 2f;
			bgRect.yMax -= 2f;
			DrawItemBackground(bgRect);

			// Custom label
			var headerRect = bgRect;
			headerRect.xMin += 5f;
			headerRect.xMax -= 10f;
			headerRect.height = Styles.headerBackground.fixedHeight;
			HeaderGUI (headerRect, args.label, item);

			// Controls
			var controlsRect = headerRect;
			controlsRect.xMin += 20f;
			controlsRect.y += headerRect.height;
			if (item.data.enabled)
				ControlsGUI (controlsRect, item);
		}

		void DrawItemBackground (Rect bgRect)
		{
			if (Event.current.type == EventType.Repaint)
			{
				var rect = bgRect;
				rect.height = Styles.headerBackground.fixedHeight;
				Styles.headerBackground.Draw(rect, false, false, false, false);

				rect.y += rect.height;
				rect.height = bgRect.height - rect.height;
				Styles.background.Draw(rect, false, false, false, false);
			}
		}

		void HeaderGUI (Rect headerRect, string label, TreeViewItem<MyTreeElement> item)
		{
			headerRect.y += 1f;

			// Do toggle
			Rect toggleRect = headerRect;
			toggleRect.width = 16;
			EditorGUI.BeginChangeCheck ();
			item.data.enabled = EditorGUI.Toggle(toggleRect, item.data.enabled); // hide when outside cell rect
			if (EditorGUI.EndChangeCheck ())
				RefreshCustomRowHeights ();

			Rect labelRect = headerRect;
			labelRect.xMin += toggleRect.width + 2f;
			GUI.Label (labelRect, label);
		}

		void ControlsGUI(Rect controlsRect, TreeViewItem<MyTreeElement> item)
		{
			var rect = controlsRect;
			rect.y += 3f;
			rect.height = EditorGUIUtility.singleLineHeight;
			item.data.floatValue1 = EditorGUI.Slider(rect, GUIContent.none, item.data.floatValue1, 0f, 1f);
			rect.y += rect.height + EditorGUIUtility.standardVerticalSpacing;
			item.data.material = (Material)EditorGUI.ObjectField(rect, GUIContent.none, item.data.material, typeof(Material), false);
			rect.y += rect.height + EditorGUIUtility.standardVerticalSpacing;
			item.data.text = GUI.TextField(rect, item.data.text);
		}

		protected override Rect GetRenameRect (Rect rowRect, int row, TreeViewItem item)
		{
			// Match label perfectly
			var renameRect = base.GetRenameRect (rowRect, row, item);
			renameRect.xMin += 25f;
			renameRect.y += 2f;
			return renameRect;
		}

		// Rename
		//--------

		protected override bool CanRename(TreeViewItem item)
		{
			// Only allow rename if we can show the rename overlay with a certain width (label might be clipped by other columns)
			Rect renameRect = GetRenameRect (treeViewRect, 0, item);
			return renameRect.width > 30;
		}

		protected override void RenameEnded(RenameEndedArgs args)
		{
			// Set the backend name and reload the tree to reflect the new model
			if (args.acceptedRename)
			{
				var element = treeModel.Find(args.itemID);
				element.name = args.newName;
				Reload();
			}
		}
	}
}
