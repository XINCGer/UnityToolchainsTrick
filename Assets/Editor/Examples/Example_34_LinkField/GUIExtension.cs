using UnityEditor;
using UnityEngine;


public static class GUIExtension
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

    public static void LinkFileLabelField(Rect position, string label, string assetPath)
    {
        LinkFileLabelField_internal(position, new GUIContent(label), AssetDatabase.LoadAssetAtPath<Object>(assetPath));
    }

    public static void LinkFileLabelField(Rect position, string label, Object linkObject)
    {
        LinkFileLabelField_internal(position, new GUIContent(label), linkObject);
    }

    public static void LinkFileLabelField(Rect position, GUIContent label, string assetPath)
    {
        LinkFileLabelField_internal(position, label, AssetDatabase.LoadAssetAtPath<Object>(assetPath));
    }

    public static void LinkFileLabelField(Rect position, GUIContent label, Object linkObject)
    {
        LinkFileLabelField_internal(position, label, linkObject);
    }

    private static void LinkFileLabelField_internal(Rect position, GUIContent label, Object linkObject)
    {
        LinkLabelField_internal(position);
        if (GUI.Button(position, label, LinkStyle))
        {
            Selection.activeObject = linkObject;
            EditorGUIUtility.PingObject(linkObject);
        }
    }

    public static void LinkUrlLabelField(Rect position, string label, string url)
    {
        LinkUrlLabelField_internal(position, new GUIContent(label), url);
    }

    public static void LinkUrlLabelField(Rect position, GUIContent label, string url)
    {
        LinkUrlLabelField_internal(position, label, url);
    }

    private static void LinkUrlLabelField_internal(Rect position, GUIContent label, string url)
    {
        LinkLabelField_internal(position);

        if (GUI.Button(position, label, LinkStyle))
        {
            Application.OpenURL(url);
        }
    }

    private static void LinkLabelField_internal(Rect position)
    {
        Handles.BeginGUI();
        Handles.color = LinkStyle.normal.textColor;
        Handles.DrawLine(new Vector3(position.xMin, position.yMax), new Vector3(position.xMax, position.yMax));
        Handles.color = Color.white;
        Handles.EndGUI();
        EditorGUIUtility.AddCursorRect(position, MouseCursor.Link);
    }
}