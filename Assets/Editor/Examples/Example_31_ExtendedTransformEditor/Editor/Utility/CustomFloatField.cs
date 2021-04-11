using System;
using System.Reflection;
using UnityEngine;
using UnityEditor;

namespace Beans.Unity.ETE
{
	public static class CustomFloatField
	{
		private static readonly int Hint = "EditorTextField".GetHashCode ();
		private static readonly Type EditorGUIType = typeof (EditorGUI);
		private static readonly Type RecycledTextEditorType = Assembly.GetAssembly (EditorGUIType).GetType ("UnityEditor.EditorGUI+RecycledTextEditor");
		private static readonly Type[] ArgumentTypes =
		{
			RecycledTextEditorType,
			typeof (Rect),
			typeof (Rect),
			typeof (int),
			typeof (float),
			typeof (string),
			typeof (GUIStyle),
			typeof (bool)
		};

		private static readonly MethodInfo DoFloatFieldMethodInfo = EditorGUIType.GetMethod ("DoFloatField", BindingFlags.NonPublic | BindingFlags.Static, null, ArgumentTypes, null);
		private static readonly FieldInfo FieldInfo = EditorGUIType.GetField ("s_RecycledEditor", BindingFlags.NonPublic | BindingFlags.Static);
		private static readonly object RecycledEditor = FieldInfo.GetValue (null);

		public static float Draw (Rect draw, Rect drag, float value, GUIStyle style)
		{
			var controlID = GUIUtility.GetControlID (Hint, FocusType.Keyboard, draw);
			var parameters = new object[] { RecycledEditor, draw, drag, controlID, value, "g7", style, true };

			return (float)DoFloatFieldMethodInfo.Invoke (null, parameters);
		}
	}
}