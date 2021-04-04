using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace ToolKits
{
    public class LayoutUtility
    {
        private static MethodInfo _miLoadWindowLayout;
        private static MethodInfo _miSaveWindowLayout;
        private static MethodInfo _miReloadWindowLayoutMenu;

        private static bool _available;

        static LayoutUtility()
        {
            Type tyWindowLayout = Type.GetType("UnityEditor.WindowLayout,UnityEditor");
            Type tyEditorUtility = Type.GetType("UnityEditor.EditorUtility,UnityEditor");
            Type tyInternalEditorUtility = Type.GetType("UnityEditorInternal.InternalEditorUtility,UnityEditor");

            if (tyWindowLayout != null && tyEditorUtility != null && tyInternalEditorUtility != null)
            {
                _miLoadWindowLayout = tyWindowLayout.GetMethod("LoadWindowLayout",
                    BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static, null,
                    new Type[] {typeof(string), typeof(bool)}, null);
                _miSaveWindowLayout = tyWindowLayout.GetMethod("SaveWindowLayout",
                    BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static, null,
                    new Type[] {typeof(string)}, null);
                _miReloadWindowLayoutMenu = tyInternalEditorUtility.GetMethod("ReloadWindowLayoutMenu",
                    BindingFlags.Public | BindingFlags.Static);

                if (_miLoadWindowLayout == null || _miSaveWindowLayout == null || _miReloadWindowLayoutMenu == null)
                    return;

                _available = true;
            }
        }

        public static bool IsAvailable
        {
            get { return _available; }
        }

        public static void SaveLayoutToAsset(string assetPath)
        {
            SaveLayout(Path.Combine(Directory.GetCurrentDirectory(), assetPath));
        }

        public static void LoadLayoutFromAsset(string assetPath)
        {
            if (_miLoadWindowLayout != null)
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), assetPath);
                _miLoadWindowLayout.Invoke(null, new object[] {path, false});
            }
        }

        public static void SaveLayout(string path)
        {
            if (_miSaveWindowLayout != null)
                _miSaveWindowLayout.Invoke(null, new object[] {path});
        }

        public static void DockEditorWindow(EditorWindow parent, EditorWindow child)
        {
            Vector2 screenPoint = parent.position.min + new Vector2(parent.position.width * .9f, 100f);

            Assembly assembly = typeof(UnityEditor.EditorWindow).Assembly;
            Type ew = assembly.GetType("UnityEditor.EditorWindow");
            Type da = assembly.GetType("UnityEditor.DockArea");
            Type sv = assembly.GetType("UnityEditor.SplitView");

            var tp = ew.GetField("m_Parent", BindingFlags.NonPublic | BindingFlags.Instance);
            var opArea = tp.GetValue(parent);
            var ocArea = tp.GetValue(child);
            var tview = da.GetProperty("parent", BindingFlags.Public | BindingFlags.Instance);
            var oview = tview.GetValue(opArea, null);
            var tDragOver = sv.GetMethod("DragOver", BindingFlags.Public | BindingFlags.Instance);
            var oDropInfo = tDragOver.Invoke(oview, new object[] {child, screenPoint});
            var tDockArea_ = da.GetField("s_OriginalDragSource", BindingFlags.NonPublic | BindingFlags.Static);
            tDockArea_.SetValue(null, ocArea);
            var tPerformDrop = sv.GetMethod("PerformDrop", BindingFlags.Public | BindingFlags.Instance);
            tPerformDrop.Invoke(oview, new object[] {child, oDropInfo, null});
        }
    }
}