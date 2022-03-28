using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ToolKits
{
    public class Example_71_UnityDuplicateEvent
    {
        [InitializeOnLoadMethod]
        public static void EditorInitialize()
        {
            EditorApplication.hierarchyWindowItemOnGUI -= HierarchyWindowItemOnGUI;
            if (EditorPrefs.GetBool(Constants.DuplicateEventListen, false))
            {
                EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemOnGUI;
            }
        }

        private static void HierarchyWindowItemOnGUI(int instanceid, Rect selectionrect)
        {
            if (Event.current.commandName == "Duplicate")
            {
                Debug.Log ("Duplicated");
            }
        }
    }
}