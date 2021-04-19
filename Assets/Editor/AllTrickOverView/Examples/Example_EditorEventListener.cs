using AllTrickOverView.Core;

namespace AllTrickOverView.Examples
{
    public class Example_EditorEventListener : AExample_Base
    {
        public static TrickOverViewInfo TrickOverViewInfo =
            new TrickOverViewInfo("EditorEventListener",
                "Editor事件监听",
                "Others",
                "using System.Collections;\nusing System.Collections.Generic;\nusing UnityEditor;\nusing UnityEngine;\n\nnamespace ToolKits\n{\n    // [InitializeOnLoad]\n    public class EditorListener\n    {\n        static EditorListener()\n        {\n            EditorApplication.update += Update;\n            EditorApplication.hierarchyChanged += hierarchyChanged;\n            EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemOnGUI;\n            EditorApplication.projectChanged += projectChanged;\n            EditorApplication.projectWindowItemOnGUI += ProjectWindowItemOnGUI;\n            EditorApplication.playModeStateChanged += OnPlayerModeStateChanged;\n        }\n\n        public static void Update()\n        {\n        }\n\n        public static void hierarchyChanged()\n        {\n            Debug.Log(\"hierarchyChanged\");\n        }\n\n        public static void HierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)\n        {\n        }\n\n        public static void projectChanged()\n        {\n        }\n\n        public static void ProjectWindowItemOnGUI(string guid, Rect selectionRect)\n        {\n        }\n\n        private static void OnPlayerModeStateChanged(PlayModeStateChange playModeState)\n        {\n            Debug.LogFormat(\"state:{0} will:{1} isPlaying:{2}\", playModeState,\n                EditorApplication.isPlayingOrWillChangePlaymode, EditorApplication.isPlaying);\n        }\n    }\n}",
                "Assets/Editor/Examples/Example_23_EditorEventListener",
                typeof(Example_EditorEventListener),
                picPath : "",
                videoPath : "");

        public override TrickOverViewInfo GetTrickOverViewInfo()
        {
            return TrickOverViewInfo;
        }
    }
}
