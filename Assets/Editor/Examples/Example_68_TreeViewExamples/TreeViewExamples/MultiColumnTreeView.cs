using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.Assertions;

namespace UnityEditor.TreeViewExamples
{
	internal class MultiColumnTreeView : TreeViewWithTreeModel<MyTreeElement>
	{
		const float kRowHeights = 20f;
		const float kToggleWidth = 18f;
		public bool showControls = true;

		static Texture2D[] s_TestIcons =
		{
			EditorGUIUtility.FindTexture ("Folder Icon"),
			EditorGUIUtility.FindTexture ("AudioSource Icon"),
			EditorGUIUtility.FindTexture ("Camera Icon"),
			EditorGUIUtility.FindTexture ("Windzone Icon"),
			EditorGUIUtility.FindTexture ("GameObject Icon")

		};

		// All columns
		enum MyColumns
		{
			Icon1,
			Icon2,
			Name,
			Value1,
			Value2,
			Value3,
		}

		public enum SortOption
		{
			Name,
			Value1,
			Value2,
			Value3,
		}

		// Sort options per column
		SortOption[] m_SortOptions = 
		{
			SortOption.Value1, 
			SortOption.Value3, 
			SortOption.Name, 
			SortOption.Value1, 
			SortOption.Value2,
			SortOption.Value3
		};

		public static void TreeToList (TreeViewItem root, IList<TreeViewItem> result)
		{
			if (root == null)
				throw new NullReferenceException("root");
			if (result == null)
				throw new NullReferenceException("result");

			result.Clear();
	
			if (root.children == null)
				return;

			Stack<TreeViewItem> stack = new Stack<TreeViewItem>();
			for (int i = root.children.Count - 1; i >= 0; i--)
				stack.Push(root.children[i]);

			while (stack.Count > 0)
			{
				TreeViewItem current = stack.Pop();
				result.Add(current);

				if (current.hasChildren && current.children[0] != null)
				{
					for (int i = current.children.Count - 1; i >= 0; i--)
					{
						stack.Push(current.children[i]);
					}
				}
			}
		}

		public MultiColumnTreeView (TreeViewState state, MultiColumnHeader multicolumnHeader, TreeModel<MyTreeElement> model) : base (state, multicolumnHeader, model)
		{
			Assert.AreEqual(m_SortOptions.Length , Enum.GetValues(typeof(MyColumns)).Length, "Ensure number of sort options are in sync with number of MyColumns enum values");

			// Custom setup
			rowHeight = kRowHeights;
			columnIndexForTreeFoldouts = 2;
			showAlternatingRowBackgrounds = true;
			showBorder = true;
			customFoldoutYOffset = (kRowHeights - EditorGUIUtility.singleLineHeight) * 0.5f; // center foldout in the row since we also center content. See RowGUI
			extraSpaceBeforeIconAndLabel = kToggleWidth;
			multicolumnHeader.sortingChanged += OnSortingChanged;
			
			Reload();
		}


		// Note we We only build the visible rows, only the backend has the full tree information. 
		// The treeview only creates info for the row list.
		protected override IList<TreeViewItem> BuildRows(TreeViewItem root)
		{
			var rows = base.BuildRows (root);
			SortIfNeeded (root, rows);
			return rows;
		}

		void OnSortingChanged (MultiColumnHeader multiColumnHeader)
		{
			SortIfNeeded (rootItem, GetRows());
		}

		void SortIfNeeded (TreeViewItem root, IList<TreeViewItem> rows)
		{
			if (rows.Count <= 1)
				return;
			
			if (multiColumnHeader.sortedColumnIndex == -1)
			{
				return; // No column to sort for (just use the order the data are in)
			}
			
			// Sort the roots of the existing tree items
			SortByMultipleColumns ();
			TreeToList(root, rows);
			Repaint();
		}

		void SortByMultipleColumns ()
		{
			var sortedColumns = multiColumnHeader.state.sortedColumns;

			if (sortedColumns.Length == 0)
				return;

			var myTypes = rootItem.children.Cast<TreeViewItem<MyTreeElement> >();
			var orderedQuery = InitialOrder (myTypes, sortedColumns);
			for (int i=1; i<sortedColumns.Length; i++)
			{
				SortOption sortOption = m_SortOptions[sortedColumns[i]];
				bool ascending = multiColumnHeader.IsSortedAscending(sortedColumns[i]);

				switch (sortOption)
				{
					case SortOption.Name:
						orderedQuery = orderedQuery.ThenBy(l => l.data.name, ascending);
						break;
					case SortOption.Value1:
						orderedQuery = orderedQuery.ThenBy(l => l.data.floatValue1, ascending);
						break;
					case SortOption.Value2:
						orderedQuery = orderedQuery.ThenBy(l => l.data.floatValue2, ascending);
						break;
					case SortOption.Value3:
						orderedQuery = orderedQuery.ThenBy(l => l.data.floatValue3, ascending);
						break;
				}
			}

			rootItem.children = orderedQuery.Cast<TreeViewItem> ().ToList ();
		}

		IOrderedEnumerable<TreeViewItem<MyTreeElement>> InitialOrder(IEnumerable<TreeViewItem<MyTreeElement>> myTypes, int[] history)
		{
			SortOption sortOption = m_SortOptions[history[0]];
			bool ascending = multiColumnHeader.IsSortedAscending(history[0]);
			switch (sortOption)
			{
				case SortOption.Name:
					return myTypes.Order(l => l.data.name, ascending);
				case SortOption.Value1:
					return myTypes.Order(l => l.data.floatValue1, ascending);
				case SortOption.Value2:
					return myTypes.Order(l => l.data.floatValue2, ascending);
				case SortOption.Value3:
					return myTypes.Order(l => l.data.floatValue3, ascending);
				default:
					Assert.IsTrue(false, "Unhandled enum");
					break;
			}

			// default
			return myTypes.Order(l => l.data.name, ascending);
		}

		int GetIcon1Index(TreeViewItem<MyTreeElement> item)
		{
			return (int)(Mathf.Min(0.99f, item.data.floatValue1) * s_TestIcons.Length);
		}

		int GetIcon2Index (TreeViewItem<MyTreeElement> item)
		{
			return Mathf.Min(item.data.text.Length, s_TestIcons.Length-1);
		}

		protected override void RowGUI (RowGUIArgs args)
		{
			var item = (TreeViewItem<MyTreeElement>) args.item;

			for (int i = 0; i < args.GetNumVisibleColumns (); ++i)
			{
				CellGUI(args.GetCellRect(i), item, (MyColumns)args.GetColumn(i), ref args);
			}
		}

		void CellGUI (Rect cellRect, TreeViewItem<MyTreeElement> item, MyColumns column, ref RowGUIArgs args)
		{
			// Center cell rect vertically (makes it easier to place controls, icons etc in the cells)
			CenterRectUsingSingleLineHeight(ref cellRect);

			switch (column)
			{
				case MyColumns.Icon1:
					{
						GUI.DrawTexture(cellRect, s_TestIcons[GetIcon1Index(item)], ScaleMode.ScaleToFit);
					}
					break;
				case MyColumns.Icon2:
					{
						GUI.DrawTexture(cellRect, s_TestIcons[GetIcon2Index(item)], ScaleMode.ScaleToFit);
					}
					break;

				case MyColumns.Name:
					{
						// Do toggle
						Rect toggleRect = cellRect;
						toggleRect.x += GetContentIndent(item);
						toggleRect.width = kToggleWidth;
						if (toggleRect.xMax < cellRect.xMax)
							item.data.enabled = EditorGUI.Toggle(toggleRect, item.data.enabled); // hide when outside cell rect

						// Default icon and label
						args.rowRect = cellRect;
						base.RowGUI(args);
					}
					break;

				case MyColumns.Value1:
				case MyColumns.Value2:
				case MyColumns.Value3:
					{
						if (showControls)
						{
							cellRect.xMin += 5f; // When showing controls make some extra spacing

							if (column == MyColumns.Value1)
								item.data.floatValue1 = EditorGUI.Slider(cellRect, GUIContent.none, item.data.floatValue1, 0f, 1f);
							if (column == MyColumns.Value2)
								item.data.material = (Material)EditorGUI.ObjectField(cellRect, GUIContent.none, item.data.material, typeof(Material), false);
							if (column == MyColumns.Value3)
								item.data.text = GUI.TextField(cellRect, item.data.text);
						}
						else
						{
							string value = "Missing";
							if (column == MyColumns.Value1)
								value = item.data.floatValue1.ToString("f5");
							if (column == MyColumns.Value2)
								value = item.data.floatValue2.ToString("f5");
							if (column == MyColumns.Value3)
								value = item.data.floatValue3.ToString("f5");

							DefaultGUI.LabelRightAligned(cellRect, value, args.selected, args.focused);
						}
					}
					break;
			}
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

		protected override Rect GetRenameRect (Rect rowRect, int row, TreeViewItem item)
		{
			Rect cellRect = GetCellRectForTreeFoldouts (rowRect);
			CenterRectUsingSingleLineHeight(ref cellRect);
			return base.GetRenameRect (cellRect, row, item);
		}

		// Misc
		//--------

		protected override bool CanMultiSelect (TreeViewItem item)
		{
			return true;
		}

		public static MultiColumnHeaderState CreateDefaultMultiColumnHeaderState(float treeViewWidth)
		{
			var columns = new[] 
			{
				new MultiColumnHeaderState.Column 
				{
					headerContent = new GUIContent(EditorGUIUtility.FindTexture("FilterByLabel"), "Lorem ipsum dolor sit amet, consectetur adipiscing elit. "),
					contextMenuText = "Asset",
					headerTextAlignment = TextAlignment.Center,
					sortedAscending = true,
					sortingArrowAlignment = TextAlignment.Right,
					width = 30, 
					minWidth = 30,
					maxWidth = 60,
					autoResize = false,
					allowToggleVisibility = true
				},
				new MultiColumnHeaderState.Column 
				{
					headerContent = new GUIContent(EditorGUIUtility.FindTexture("FilterByType"), "Sed hendrerit mi enim, eu iaculis leo tincidunt at."),
					contextMenuText = "Type",
					headerTextAlignment = TextAlignment.Center,
					sortedAscending = true,
					sortingArrowAlignment = TextAlignment.Right,
					width = 30, 
					minWidth = 30,
					maxWidth = 60,
					autoResize = false,
					allowToggleVisibility = true
				},
				new MultiColumnHeaderState.Column 
				{
					headerContent = new GUIContent("Name"),
					headerTextAlignment = TextAlignment.Left,
					sortedAscending = true,
					sortingArrowAlignment = TextAlignment.Center,
					width = 150, 
					minWidth = 60,
					autoResize = false,
					allowToggleVisibility = false
				},
				new MultiColumnHeaderState.Column 
				{
					headerContent = new GUIContent("Multiplier", "In sed porta ante. Nunc et nulla mi."),
					headerTextAlignment = TextAlignment.Right,
					sortedAscending = true,
					sortingArrowAlignment = TextAlignment.Left,
					width = 110,
					minWidth = 60,
					autoResize = true
				},
				new MultiColumnHeaderState.Column 
				{
					headerContent = new GUIContent("Material", "Maecenas congue non tortor eget vulputate."),
					headerTextAlignment = TextAlignment.Right,
					sortedAscending = true,
					sortingArrowAlignment = TextAlignment.Left,
					width = 95,
					minWidth = 60,
					autoResize = true,
					allowToggleVisibility = true
				},
				new MultiColumnHeaderState.Column 
				{
					headerContent = new GUIContent("Note", "Nam at tellus ultricies ligula vehicula ornare sit amet quis metus."),
					headerTextAlignment = TextAlignment.Right,
					sortedAscending = true,
					sortingArrowAlignment = TextAlignment.Left,
					width = 70,
					minWidth = 60,
					autoResize = true
				}
			};

			Assert.AreEqual(columns.Length, Enum.GetValues(typeof(MyColumns)).Length, "Number of columns should match number of enum values: You probably forgot to update one of them.");

			var state =  new MultiColumnHeaderState(columns);
			return state;
		}
	}

	static class MyExtensionMethods
	{
		public static IOrderedEnumerable<T> Order<T, TKey>(this IEnumerable<T> source, Func<T, TKey> selector, bool ascending)
		{
			if (ascending)
			{
				return source.OrderBy(selector);
			}
			else
			{
				return source.OrderByDescending(selector);
			}
		}

		public static IOrderedEnumerable<T> ThenBy<T, TKey>(this IOrderedEnumerable<T> source, Func<T, TKey> selector, bool ascending)
		{
			if (ascending)
			{
				return source.ThenBy(selector);
			}
			else
			{
				return source.ThenByDescending(selector);
			}
		}
	}
}
