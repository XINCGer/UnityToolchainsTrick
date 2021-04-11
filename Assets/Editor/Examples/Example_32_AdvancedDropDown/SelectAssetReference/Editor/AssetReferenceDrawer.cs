using System;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(AssetReferenceAttribute), true)]
public class AssetReferenceDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        Type referenceType = fieldInfo.FieldType;
        Rect guidRect = new Rect()
        {
            x = position.x,
            y = position.y,
            height = 16,
            width = position.width
        };
        property.objectReferenceValue = AssetReferenceField(property, guidRect, property.displayName, referenceType);
        property.serializedObject.ApplyModifiedProperties();
    }

    private UnityEngine.Object AssetReferenceField(SerializedProperty property, Rect position, string label, Type type)
    {
        float objectFieldWidth = position.width - 20;
#if UNITY_2019_1_OR_NEWER
        objectFieldWidth = position.width - 80;
#endif
        property.objectReferenceValue = EditorGUI.ObjectField(new Rect(position.x, position.y, objectFieldWidth, position.height), label,
            property.objectReferenceValue, type, false);

#if UNITY_2019_1_OR_NEWER
        if (EditorGUI.DropdownButton(new Rect(position.xMax - 80, position.y, 60, position.height), new GUIContent("Select"), FocusType.Passive))
        {
            SelectAssetReferenceWindow.Show(new AssetReferenceDropdownState(type, property), position);
        }
#endif
        if (GUI.Button(new Rect(position.xMax - 20, position.y, 20, position.height), new GUIContent("X")))
        {
            property.objectReferenceValue = null;
        }
        return property.objectReferenceValue;
    }
}