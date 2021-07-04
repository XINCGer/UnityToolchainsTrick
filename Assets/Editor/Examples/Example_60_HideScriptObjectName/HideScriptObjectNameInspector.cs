using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ToolKits
{
    [CustomEditor(typeof(HideScriptObjectName), true)]
    public class HideScriptObjectNameInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            serializedObject.Update();

            SerializedProperty property = serializedObject.GetIterator();
            bool expanded = true;
            while (property.NextVisible(expanded))
            {
                expanded = false;
                if (SkipField(property.propertyPath))
                    continue;
                EditorGUILayout.PropertyField(property, true);
            }

            serializedObject.ApplyModifiedProperties();
            EditorGUI.EndChangeCheck();
        }

        public virtual void ApplyChanges()
        {
        }

        static bool SkipField(string fieldName)
        {
            return fieldName == "m_Script";
        }
    }
}