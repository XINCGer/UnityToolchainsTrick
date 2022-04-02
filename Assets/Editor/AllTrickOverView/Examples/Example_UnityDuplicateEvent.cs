using AllTrickOverView.Core;

namespace AllTrickOverView.Examples
{
    public class Example_UnityDuplicateEvent : AExample_Base
    {
        public static TrickOverViewInfo TrickOverViewInfo =
            new TrickOverViewInfo("UnityDuplicateEvent",
                "监听Hierarchy下的Ctrl+D⌚️",
                "Hierarchy",
                "using System.Collections;\nusing System.Collections.Generic;\nusing UnityEditor;\nusing UnityEngine;\n\nnamespace ToolKits\n{\n    public class Example_71_UnityDuplicateEvent\n    {\n        [InitializeOnLoadMethod]\n        public static void EditorInitialize()\n        {\n            EditorApplication.hierarchyWindowItemOnGUI -= HierarchyWindowItemOnGUI;\n            if (EditorPrefs.GetBool(Constants.DuplicateEventListen, false))\n            {\n                EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemOnGUI;\n            }\n        }\n\n        private static void HierarchyWindowItemOnGUI(int instanceid, Rect selectionrect)\n        {\n            if (Event.current.commandName == \"Duplicate\")\n            {\n                Debug.Log (\"Duplicated\");\n            }\n        }\n    }\n}",
                "Assets/Editor/Examples/Example_71_UnityDuplicateEvent",
                typeof(Example_UnityDuplicateEvent),
                picPath : "",
                videoPath : "");

        public override TrickOverViewInfo GetTrickOverViewInfo()
        {
            return TrickOverViewInfo;
        }
    }
}
