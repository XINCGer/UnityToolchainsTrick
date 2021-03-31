using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ToolKits
{
    public class EditorWindowContextClick : EditorWindow
    {
        private static EditorWindowContextClick _window;
        private static readonly Vector2 MIN_SIZE = new Vector2(400, 200);

        [MenuItem("Tools/EditorWindowContextClick", priority = 27)]
        private static void PopUp()
        {
            _window = GetWindow<EditorWindowContextClick>("EditorWindow右键弹出菜单演示");
            _window.Show();
        }

        private void OnGUI()
        {
            var e = Event.current;
            if (null != e)
            {
                if (e.type == EventType.MouseDown && e.button == 1)
                {
                    var genericMenu = new GenericMenu();
                    genericMenu.AddItem(new GUIContent("功能1"), false, () => { Debug.Log("功能1"); });
                    genericMenu.AddItem(new GUIContent("功能合集/功能2"), false, () => { Debug.Log("功能2"); });
                    genericMenu.AddItem(new GUIContent("功能合集/功能3"), false, () => { Debug.Log("功能3"); });
                    genericMenu.AddSeparator("功能合集/");
                    genericMenu.AddItem(new GUIContent("功能合集/功能4"), false, () => { Debug.Log("功能4"); });
                    genericMenu.ShowAsContext();
                }
            }
        }
    }
}