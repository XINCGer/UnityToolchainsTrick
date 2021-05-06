using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public static class ShowFileExtensions
{
    public static bool ShowFileExtensionsEnable
    {
        get { return EditorPrefs.GetBool("ShowFileExtensions", true); }
        set { EditorPrefs.SetBool("ShowFileExtensions", value); }
    }
    
    [MenuItem("Tools/ShowFileExtensions/OpenShowFileExtensions")]
    private static void OpenPlaySize()
    {
        ShowFileExtensionsEnable = true;
    }

    [MenuItem("Tools/ShowFileExtensions/CloseShowFileExtensions")]
    private static void ClosePlaySize()
    {
        ShowFileExtensionsEnable = false;
    }
    
    static ShowFileExtensions()
    {
        EditorApplication.projectWindowItemOnGUI += ProjectWindowItemOnGUI;
    }
    private static void ProjectWindowItemOnGUI(string guid, Rect rect)
    {
        if (ShowFileExtensionsEnable && Event.current.alt)
        {
            EditorWindow window = GetProjectWindow();            
           string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(assetPath);

            if (obj != null && AssetDatabase.IsMainAsset(obj) && !IsDirectory(obj))
            {
                if (showsBigIcons)
                {
                    // just draw it bold in upper left
                    string extension = Path.GetExtension(assetPath);
                    GUI.Label(rect, extension, EditorStyles.boldLabel);
                }
                else
                {
                    // we overpaint the filename here, does look a bit dirty and might need adjustment of the offset
                    var font = GUI.skin.font;
                    var fonSize = GUI.skin.label.fontSize;
                    var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(assetPath);
                    var width = 18.0f;
                    foreach (var t in fileNameWithoutExtension)
                    {
                        font.GetCharacterInfo(t, out var characterInfo, fonSize);
                        width += characterInfo.advance;
                    }

                    var labelRect = rect.Translate(new Vector2(width, -1.5f));
                    GUI.Label(labelRect, Path.GetExtension(assetPath));
                }
            }

            EditorApplication.RepaintProjectWindow();
        }
    }

    // ================================================================================
    //  getting infos about the project window
    // --------------------------------------------------------------------------------

    private static bool showsBigIcons
    {
        get
        {
            return isTwoColumnMode && listAreaGridSize > 16f;
        }
    }

    private static bool isTwoColumnMode
    {
        get
        {
            var projectWindow = GetProjectWindow();

            var projectWindowType = projectWindow.GetType();
            var modeFieldInfo = projectWindowType.GetField("m_ViewMode", BindingFlags.Instance | BindingFlags.NonPublic);

            int mode = (int)modeFieldInfo.GetValue(projectWindow);

            // 0 == ViewMode.OneColumn
            // 1 == ViewMode.TwoColum

            return mode == 1;
        }
    }

    private static float listAreaGridSize
    {
        get
        {
            var projectWindow = GetProjectWindow();

            var projectWindowType = projectWindow.GetType();
            var propertyInfo = projectWindowType.GetProperty("listAreaGridSize", BindingFlags.Instance | BindingFlags.Public);
            return (float)propertyInfo.GetValue(projectWindow, null);
        }
    }

    /// <summary>
    /// there's a chance here we get the wrong one when two project windows are open
    /// </summary>
    private static EditorWindow GetProjectWindow()
    {
        if (EditorWindow.focusedWindow != null && EditorWindow.focusedWindow.titleContent.text == "Project")
        {
            return EditorWindow.focusedWindow;
        }

        return GetExistingWindowByName("Project");
    }

    private static EditorWindow GetExistingWindowByName(string name)
    {
        EditorWindow[] windows = Resources.FindObjectsOfTypeAll<EditorWindow>();
        foreach (var item in windows)
        {
            if (item.titleContent.text == name)
            {
                return item;
            }
        }

        return default(EditorWindow);
    }

    // ================================================================================
    //  utilities
    // --------------------------------------------------------------------------------

    private static Rect Translate(this Rect rect, Vector2 delta)
    {
        rect.x += delta.x;
        rect.y += delta.y;

        return rect;
    }

    private static bool IsDirectory(UnityEngine.Object obj)
    {
        if (obj == null)
        {
            return false;
        }

        return obj is DefaultAsset && !AssetDatabase.IsForeignAsset(obj);
    }
}