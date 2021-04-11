using UnityEditor;
using UnityEngine;

public static class EditorGUILayoutExtension
{
    private static GUIStyle _linkStyle = null;

    private static GUIStyle LinkStyle
    {
        get
        {
            if (_linkStyle == null)
            {
                _linkStyle = new GUIStyle(EditorStyles.label);
                _linkStyle.wordWrap = false;
                _linkStyle.fontSize = 11;
                _linkStyle.normal.textColor = new Color(0x00 / 255f, 0x78 / 255f, 0xDA / 255f, 1f);
                _linkStyle.stretchWidth = false;
            }

            return _linkStyle;
        }
    }

    public static void LinkFileLabelField(string label, string assetPath, params GUILayoutOption[] options)
    {
        LinkFileLabelField_internal(new GUIContent(label), AssetDatabase.LoadAssetAtPath<Object>(assetPath), options);
    }

    public static void LinkFileLabelField(string label, Object linkObject, params GUILayoutOption[] options)
    {
        LinkFileLabelField_internal(new GUIContent(label), linkObject, options);
    }

    public static void LinkFileLabelField(GUIContent label, string assetPath, params GUILayoutOption[] options)
    {
        LinkFileLabelField_internal(label, AssetDatabase.LoadAssetAtPath<Object>(assetPath), options);
    }

    public static void LinkFileLabelField(GUIContent label, Object linkObject, params GUILayoutOption[] options)
    {
        LinkFileLabelField_internal(label, linkObject, options);
    }

    private static void LinkFileLabelField_internal(GUIContent label, Object linkObject, params GUILayoutOption[] options)
    {
        var position = LinkLabelField_internal(label, options);
        if (GUI.Button(position, label, LinkStyle))
        {
            Selection.activeObject = linkObject;
            EditorGUIUtility.PingObject(linkObject);
        }
    }

    public static void LinkUrlLabelField(string label, string url, params GUILayoutOption[] options)
    {
        LinkUrlLabelField_internal(new GUIContent(label), url, options);
    }

    public static void LinkUrlLabelField(GUIContent label, string url, params GUILayoutOption[] options)
    {
        LinkUrlLabelField_internal(label, url, options);
    }

    private static void LinkUrlLabelField_internal(GUIContent label, string url, params GUILayoutOption[] options)
    {
        var position = LinkLabelField_internal(label, options);
        if (GUI.Button(position, label, LinkStyle))
        {
            Application.OpenURL(url);
        }
    }

    private static Rect LinkLabelField_internal(GUIContent label, params GUILayoutOption[] options)
    {
        var position = GUILayoutUtility.GetRect(label, LinkStyle, options);
        Handles.BeginGUI();
        Handles.color = LinkStyle.normal.textColor;
        Handles.DrawLine(new Vector3(position.xMin, position.yMax), new Vector3(position.xMax, position.yMax));
        Handles.color = Color.white;
        Handles.EndGUI();
        EditorGUIUtility.AddCursorRect(position, MouseCursor.Link);
        return position;
    }
}