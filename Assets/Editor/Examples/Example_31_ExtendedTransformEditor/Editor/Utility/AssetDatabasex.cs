using System;
using System.IO;
using UnityEditor;
using Object = UnityEngine.Object;

namespace Beans.Unity.ETE
{
	public static class AssetDatabasex
	{
		public enum SearchFilter
		{
			All,
			Assets,
			Packages
		}

		public static T LoadAssetOfType<T> (string contains = null, SearchFilter searchAssets = SearchFilter.All, Action error = null, Action success = null) where T : Object
		{
			bool allowScriptAssets = typeof (T) == typeof (MonoScript);

			T t = null;
			string[] assetGUIDs = AssetDatabase.FindAssets ($"t:{typeof (T).Name}", GetSearchDirectories (searchAssets));
			foreach (var assetGUID in assetGUIDs)
			{
				string assetPath = AssetDatabase.GUIDToAssetPath (assetGUID);
				if (string.IsNullOrEmpty (assetPath) || !allowScriptAssets && assetPath.EndsWith (".cs") || contains != null && !Path.GetFileName (assetPath).Contains (contains))
					continue;
				t = AssetDatabase.LoadAssetAtPath<T> (assetPath);
				break;
			}

			if (t == null)
				error?.Invoke ();
			else
				success?.Invoke ();

			return t;
		}

		private static string[] GetSearchDirectories (SearchFilter searchAssets)
		{
			string[] searchDirs;
			switch (searchAssets)
			{
				case SearchFilter.All:
					searchDirs = new[] { "Assets", "Packages" };
					break;
				case SearchFilter.Assets:
					searchDirs = new[] { "Assets" };
					break;
				case SearchFilter.Packages:
					searchDirs = new[] { "Packages" };
					break;
				default:
					throw new ArgumentOutOfRangeException (nameof (searchAssets), searchAssets, null);
			}

			return searchDirs;
		}
	}
}