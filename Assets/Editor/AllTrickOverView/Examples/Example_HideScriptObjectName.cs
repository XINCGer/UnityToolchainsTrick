using AllTrickOverView.Core;

namespace AllTrickOverView.Examples
{
    public class Example_HideScriptObjectName : AExample_Base
    {
        public static TrickOverViewInfo TrickOverViewInfo =
            new TrickOverViewInfo("HideScriptObjectName",
                "隐藏ScriptableObject的脚本名称",
                "Inspector",
                "using System.Collections;\nusing System.Collections.Generic;\nusing UnityEditor;\nusing UnityEngine;\n\nnamespace ToolKits\n{\n    [CustomEditor(typeof(HideScriptObjectName), true)]\n    public class HideScriptObjectNameInspector : Editor\n    {\n        public override void OnInspectorGUI()\n        {\n            EditorGUI.BeginChangeCheck();\n            serializedObject.Update();\n\n            SerializedProperty property = serializedObject.GetIterator();\n            bool expanded = true;\n            while (property.NextVisible(expanded))\n            {\n                expanded = false;\n                if (SkipField(property.propertyPath))\n                    continue;\n                EditorGUILayout.PropertyField(property, true);\n            }\n\n            serializedObject.ApplyModifiedProperties();\n            EditorGUI.EndChangeCheck();\n        }\n\n        public virtual void ApplyChanges()\n        {\n        }\n\n        static bool SkipField(string fieldName)\n        {\n            return fieldName == \"m_Script\";\n        }\n    }\n}",
                "Assets/Editor/Examples/Example_60_HideScriptObjectName",
                typeof(Example_HideScriptObjectName),
                picPath : "Assets/Editor/Examples/Example_60_HideScriptObjectName/preview.png",
                videoPath : "");

        public override TrickOverViewInfo GetTrickOverViewInfo()
        {
            return TrickOverViewInfo;
        }
    }
}
