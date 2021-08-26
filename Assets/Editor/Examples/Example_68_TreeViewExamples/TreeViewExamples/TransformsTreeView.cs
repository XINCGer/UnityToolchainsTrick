using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityObject = UnityEngine.Object;


namespace UnityEditor.TreeViewExamples
{

	class TransformTreeView : TreeView
	{
		public TransformTreeView (TreeViewState state)
			: base (state)
		{
			Reload ();
		}

		protected override TreeViewItem BuildRoot()
		{
			return new TreeViewItem {id = 0, depth = -1};
		}

		protected override IList<TreeViewItem> BuildRows(TreeViewItem root)
		{
			var rows = GetRows () ?? new List<TreeViewItem> (200);

			Scene scene = SceneManager.GetSceneAt (0);

			// We use the GameObject instanceIDs as ids for items as we want to 
			// select the game objects and not the transform components.
			rows.Clear ();
			var gameObjectRoots = scene.GetRootGameObjects ();
			foreach (var gameObject in gameObjectRoots)
			{
				var item = CreateTreeViewItemForGameObject (gameObject);
				root.AddChild (item);
				rows.Add (item);
				if (gameObject.transform.childCount > 0)
				{
					if (IsExpanded (item.id))
					{
						AddChildrenRecursive (gameObject, item, rows);
					}
					else
					{
						item.children = CreateChildListForCollapsedParent ();
					}
				}
			}

			SetupDepthsFromParentsAndChildren (root);
			return rows;
		}

		void AddChildrenRecursive (GameObject go, TreeViewItem item, IList<TreeViewItem> rows)
		{
			int childCount = go.transform.childCount;

			item.children = new List<TreeViewItem> (childCount);
			for (int i = 0; i < childCount; ++i)
			{
				var childTransform = go.transform.GetChild (i);
				var childItem = CreateTreeViewItemForGameObject (childTransform.gameObject);
				item.AddChild (childItem);
				rows.Add (childItem);

				if (childTransform.childCount > 0)
				{
					if (IsExpanded (childItem.id))
					{
						AddChildrenRecursive (childTransform.gameObject, childItem, rows);
					}
					else
					{
						childItem.children = CreateChildListForCollapsedParent ();
					}
				}
			}
		}

		static TreeViewItem CreateTreeViewItemForGameObject (GameObject gameObject)
		{
			// We can use the GameObject instanceID for TreeViewItem id, as it ensured to be unique among other items in the tree.
			// To optimize reload time we could delay fetching the transform.name until it used for rendering (prevents allocating strings 
			// for items not rendered in large trees)
			// We just set depth to -1 here and then call SetupDepthsFromParentsAndChildren at the end of BuildRootAndRows to set the depths.
			return new TreeViewItem(gameObject.GetInstanceID(), -1, gameObject.name);
		}

		protected override IList<int> GetAncestors (int id)
		{
			// The backend needs to provide us with this info since the item with id
			// may not be present in the rows
			var transform = GetGameObject(id).transform;

			List<int> ancestors = new List<int> ();
			while (transform.parent != null)
			{
				ancestors.Add (transform.parent.gameObject.GetInstanceID ());
				transform = transform.parent;
			}

			return ancestors;
		}

		protected override IList<int> GetDescendantsThatHaveChildren (int id)
		{
			Stack<Transform> stack = new Stack<Transform> ();

			var start = GetGameObject(id).transform;
			stack.Push (start);

			var parents = new List<int> ();
			while (stack.Count > 0)
			{
				Transform current = stack.Pop ();
				parents.Add (current.gameObject.GetInstanceID ());
				for (int i = 0; i < current.childCount; ++i)
				{
					if (current.childCount > 0)
						stack.Push (current.GetChild (i));
				}
			}

			return parents;
		}

		GameObject GetGameObject (int instanceID)
		{
			return (GameObject)EditorUtility.InstanceIDToObject(instanceID);
		}

		// Custom GUI

		protected override void RowGUI (RowGUIArgs args)
		{
			Event evt = Event.current;
			extraSpaceBeforeIconAndLabel = 18f;

			// GameObject isStatic toggle 
			var gameObject = GetGameObject(args.item.id);
			if (gameObject == null)
				return;

			Rect toggleRect = args.rowRect;
			toggleRect.x += GetContentIndent(args.item);
			toggleRect.width = 16f;

			// Ensure row is selected before using the toggle (usability)
			if (evt.type == EventType.MouseDown && toggleRect.Contains(evt.mousePosition))
				SelectionClick(args.item, false);
			
			EditorGUI.BeginChangeCheck ();
			bool isStatic = EditorGUI.Toggle(toggleRect, gameObject.isStatic); 
			if (EditorGUI.EndChangeCheck ())
				gameObject.isStatic = isStatic;

			// Text
			base.RowGUI(args);
		}

		// Selection

		protected override void SelectionChanged (IList<int> selectedIds)
		{
			Selection.instanceIDs = selectedIds.ToArray();
		}

		// Reordering

		protected override bool CanStartDrag (CanStartDragArgs args)
		{
			return true;
		}

		protected override void SetupDragAndDrop (SetupDragAndDropArgs args)
		{
			DragAndDrop.PrepareStartDrag ();

			var sortedDraggedIDs = SortItemIDsInRowOrder (args.draggedItemIDs);

			List<UnityObject> objList = new List<UnityObject> (sortedDraggedIDs.Count);
			foreach (var id in sortedDraggedIDs)
			{
				UnityObject obj = EditorUtility.InstanceIDToObject (id);
				if (obj != null)
					objList.Add (obj);
			}

			DragAndDrop.objectReferences = objList.ToArray ();

			string title = objList.Count > 1 ? "<Multiple>" : objList[0].name;
			DragAndDrop.StartDrag (title);
		}

		protected override DragAndDropVisualMode HandleDragAndDrop (DragAndDropArgs args)
		{
			// First check if the dragged objects are GameObjects
			var draggedObjects = DragAndDrop.objectReferences;
			var transforms = new List<Transform> (draggedObjects.Length);
			foreach (var obj in draggedObjects)
			{
				var go = obj as GameObject;
				if (go == null)
				{
					return DragAndDropVisualMode.None;
				}

				transforms.Add (go.transform);
			}

			// Filter out any unnecessary transforms before the reparent operation
			RemoveItemsThatAreDescendantsFromOtherItems (transforms);

			// Reparent
			if (args.performDrop)
			{
				switch (args.dragAndDropPosition)
				{
					case DragAndDropPosition.UponItem:
					case DragAndDropPosition.BetweenItems:
						Transform parent = args.parentItem != null ? GetGameObject (args.parentItem.id).transform : null;

						if (!IsValidReparenting (parent, transforms))
							return DragAndDropVisualMode.None;

						foreach (var trans in transforms)
							trans.SetParent (parent);

						if (args.dragAndDropPosition == DragAndDropPosition.BetweenItems)
						{
							int insertIndex = args.insertAtIndex;
							for (int i = transforms.Count - 1; i >= 0; i--)
							{
								var transform = transforms[i];
								insertIndex = GetAdjustedInsertIndex (parent, transform, insertIndex);
								transform.SetSiblingIndex (insertIndex);
							}
						}
						break;

					case DragAndDropPosition.OutsideItems:
						foreach (var trans in transforms)
						{
							trans.SetParent (null); // make root when dragged to empty space in treeview
						}
						break;
					default:
						throw new ArgumentOutOfRangeException ();
				}

				Reload ();
				SetSelection (transforms.Select (t => t.gameObject.GetInstanceID ()).ToList (), TreeViewSelectionOptions.RevealAndFrame);
			}

			return DragAndDropVisualMode.Move;
		}

		int GetAdjustedInsertIndex (Transform parent, Transform transformToInsert, int insertIndex)
		{
			if (transformToInsert.parent == parent && transformToInsert.GetSiblingIndex () < insertIndex)
				return --insertIndex;
			return insertIndex;
		}

		bool IsValidReparenting (Transform parent, List<Transform> transformsToMove)
		{
			if (parent == null)
				return true;

			foreach (var transformToMove in transformsToMove)
			{
				if (transformToMove == parent)
					return false;

				if (IsHoveredAChildOfDragged (parent, transformToMove))
					return false;
			}

			return true;
		}


		bool IsHoveredAChildOfDragged (Transform hovered, Transform dragged)
		{
			Transform t = hovered.parent;
			while (t)
			{
				if (t == dragged)
					return true;
				t = t.parent;
			}
			return false;
		}


		// Returns true if there is an ancestor of transform in the transforms list
		static bool IsDescendantOf (Transform transform, List<Transform> transforms)
		{
			while (transform != null)
			{
				transform = transform.parent;
				if (transforms.Contains (transform))
					return true;
			}
			return false;
		}

		static void RemoveItemsThatAreDescendantsFromOtherItems (List<Transform> transforms)
		{
			transforms.RemoveAll (t => IsDescendantOf (t, transforms));
		}
	}
}
