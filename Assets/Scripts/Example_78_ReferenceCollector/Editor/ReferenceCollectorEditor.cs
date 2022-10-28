#region 注 释
/***
 *
 *  Title:
 *  
 *  Description:
 *  
 *  Date:
 *  Version:
 *  Writer: 半只龙虾人
 *  Github: https://github.com/HalfLobsterMan
 *  Blog: https://www.crosshair.top/
 *
 */
#endregion
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace CZToolKit.Core.Editors
{
    [CustomEditor(typeof(ReferenceCollector))]
    public class ReferenceCollectorEditor : Editor
    {
        public static class Styles
        {
            public static readonly GUIContent ClearLabel = new GUIContent(EditorGUIUtility.FindTexture("Refresh"), "Clear All");
            public static readonly GUIContent CleanLabel = new GUIContent(EditorGUIUtility.FindTexture("UnityEditor.ConsoleWindow"), "Clear Empty");
            public static readonly GUIContent SortLabel = new GUIContent(EditorGUIUtility.FindTexture("AlphabeticalSorting"), "Sort");
            public static readonly GUIContent ComponentsLabel = new GUIContent(EditorGUIUtility.FindTexture("UnityEditor.HierarchyWindow"), "Components");
        }

        private ReorderableList referencesList;
        private ReferenceCollector referenceCollector;

        private void OnEnable()
        {
            referenceCollector = serializedObject.targetObject as ReferenceCollector;
            referencesList = new ReorderableList(serializedObject, serializedObject.FindProperty("references"), true, true, true, true);
            referencesList.drawHeaderCallback = (a) =>
            {
                GUI.Label(a, "References");

                var clearButtonRect = new Rect(a.x + a.width - 30, a.y, 30, a.height);
                if (GUI.Button(clearButtonRect, Styles.ClearLabel, EditorStyles.toolbarButton))
                {
                    Undo.RecordObject(referenceCollector, "Clear All");
                    referenceCollector.Clear();
                    serializedObject.ApplyModifiedProperties();
                    serializedObject.UpdateIfRequiredOrScript();
                }

                var cleanButtonRect = new Rect(a.x + a.width - 60, a.y, 30, a.height);
                if (GUI.Button(cleanButtonRect, Styles.CleanLabel, EditorStyles.toolbarButton))
                {
                    Undo.RecordObject(referenceCollector, "Clear Empty");
                    referenceCollector.ClearEmpty();
                    serializedObject.ApplyModifiedProperties();
                    serializedObject.UpdateIfRequiredOrScript();
                }

                var sortButtonRect = new Rect(a.x + a.width - 90, a.y, 30, a.height);
                if (GUI.Button(sortButtonRect, Styles.SortLabel, EditorStyles.toolbarButton))
                {
                    Undo.RecordObject(referenceCollector, "Sort ReferenceData");
                    referenceCollector.Sort();
                    serializedObject.ApplyModifiedProperties();
                    serializedObject.UpdateIfRequiredOrScript();
                }
            };
            referencesList.drawElementCallback += (a, b, c, d) =>
            {
                var element = referencesList.serializedProperty.GetArrayElementAtIndex(b);
                var key = element.FindPropertyRelative("key");
                var value = element.FindPropertyRelative("value");

                var keyFieldRect = new Rect(a.x, a.y, a.width * 0.3f - 1, a.height);
                var objFieldRect = new Rect(a.x + a.width * 0.3f + 1, a.y, a.width * 0.7f - 26, a.height);
                var dropDownButtonRect = new Rect(a.xMax - 25, a.y, 25, a.height);

                EditorGUI.BeginChangeCheck();
                var sourceK = key.stringValue;
                var k = EditorGUI.DelayedTextField(keyFieldRect, sourceK);
                if (!string.IsNullOrEmpty(k) && k != sourceK && !referenceCollector.ReferencesDict.ContainsKey(k))
                    key.stringValue = k;

                var sourceV = value.objectReferenceValue;
                EditorGUI.PropertyField(objFieldRect, value, GUIContent.none);
                if (EditorGUI.EndChangeCheck())
                {
                    serializedObject.ApplyModifiedProperties();
                    serializedObject.UpdateIfRequiredOrScript();
                }

                EditorGUI.BeginDisabledGroup(value.objectReferenceValue == null && !(value.objectReferenceValue is GameObject) && !(value.objectReferenceValue is Component));
                if (GUI.Button(dropDownButtonRect, Styles.ComponentsLabel, EditorStyles.toolbarButton))
                {
                    GenericMenu menu = new GenericMenu();
                    Component[] components = null;
                    GameObject gameObject = null;
                    switch (value.objectReferenceValue)
                    {
                        case GameObject _gameObject:
                            components = _gameObject.GetComponents<Component>();
                            gameObject = _gameObject;
                            break;
                        case Component _component:
                            components = _component.GetComponents<Component>();
                            gameObject = _component.gameObject;
                            break;
                    }
                    menu.AddItem(EditorGUIUtility.TrTextContent($"0:GameObject"), false, () =>
                    {
                        Undo.RecordObject(referenceCollector, "Change Component");
                        value.objectReferenceValue = gameObject;
                        serializedObject.ApplyModifiedProperties();
                        serializedObject.UpdateIfRequiredOrScript();
                    });
                    for (int i = 0; i < components.Length; i++)
                    {
                        var component = components[i];
                        menu.AddItem(EditorGUIUtility.TrTextContent($"{i + 1}:{component.GetType().Name}"), false, () =>
                        {
                            Undo.RecordObject(referenceCollector, "Change Component");
                            value.objectReferenceValue = component;
                            serializedObject.ApplyModifiedProperties();
                            serializedObject.UpdateIfRequiredOrScript();
                        });
                    }
                    menu.DropDown(dropDownButtonRect);
                }
                EditorGUI.EndDisabledGroup();
            };
            referencesList.onAddCallback += (list) =>
            {
                Undo.RecordObject(referenceCollector, "Add ReferenceData");
                referenceCollector.Add();
                serializedObject.ApplyModifiedProperties();
                serializedObject.UpdateIfRequiredOrScript();
            };
            referencesList.onRemoveCallback += (a) =>
            {
                Undo.RecordObject(referenceCollector, "Remove ReferenceData");
                referenceCollector.RemoveAt(referencesList.index);
                serializedObject.ApplyModifiedProperties();
                serializedObject.UpdateIfRequiredOrScript();
            };
        }

        public override void OnInspectorGUI()
        {
            referencesList.DoLayoutList();
        }
    }
}
