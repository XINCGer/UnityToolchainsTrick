using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Video;
using Object = System.Object;

public static class MediaPreviewUtil
{
    //获取Preview Texture
    public static Texture GetPreviewTexture(Object previewID)
    {
        Type videoUtilType = Assembly.Load("UnityEditor.dll").GetType("UnityEditor.VideoUtil");
        MethodInfo GetPreviewTextureMethodInfo =
            videoUtilType.GetMethod("GetPreviewTexture", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        Texture image = (Texture) GetPreviewTextureMethodInfo.Invoke(null, new object[]
        {
            previewID
        });
        return image;
    }

    //开始播放Video
    public static Object PlayPreview(VideoClip audioClip)
    {
        Type videoUtilType = Assembly.Load("UnityEditor.dll").GetType("UnityEditor.VideoUtil");
        MethodInfo startPreviewMethodInfo = videoUtilType.GetMethod("StartPreview", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        Object previewID = startPreviewMethodInfo.Invoke(null, new object[] {audioClip});
        videoUtilType.GetMethod("PlayPreview", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).Invoke(null, new object[]
        {
            previewID, true
        });
        return previewID;
    }

    //停止播放Video
    public static void StopPreview(Object previewID)
    {
        Type videoUtilType = Assembly.Load("UnityEditor.dll").GetType("UnityEditor.VideoUtil");
        videoUtilType.GetMethod("StopPreview", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).Invoke(null, new object[]
        {
            previewID
        });
    }
    
    internal static Rect GetCenteredWindowPosition(Rect parentWindowPosition, Vector2 size)
    {
        var pos = new Rect
        {
            x = 0, y = 0,
            width = Mathf.Min(size.x, parentWindowPosition.width * 0.90f),
            height = Mathf.Min(size.y, parentWindowPosition.height * 0.90f)
        };
        var w = (parentWindowPosition.width - pos.width) * 0.5f;
        var h = (parentWindowPosition.height - pos.height) * 0.5f;
        pos.x = parentWindowPosition.x + w;
        pos.y = parentWindowPosition.y + h;
        return pos;
    }

    static UnityEngine.Object s_MainWindow = null;

    internal static Type[] GetAllDerivedTypes(Type aType)
    {
        return TypeCache.GetTypesDerivedFrom(aType).ToArray();
    }

    internal static Rect GetEditorMainWindowPos()
    {
        if (s_MainWindow == null)
        {
            var containerWinType = GetAllDerivedTypes(typeof(ScriptableObject)).FirstOrDefault(t => t.Name == "ContainerWindow");
            if (containerWinType == null)
                throw new MissingMemberException("Can't find internal type ContainerWindow. Maybe something has changed inside Unity");
            var showModeField = containerWinType.GetField("m_ShowMode", BindingFlags.NonPublic | BindingFlags.Instance);
            if (showModeField == null)
                throw new MissingFieldException("Can't find internal fields 'm_ShowMode'. Maybe something has changed inside Unity");
            var windows = Resources.FindObjectsOfTypeAll(containerWinType);
            foreach (var win in windows)
            {
                var showMode = (int) showModeField.GetValue(win);
                if (showMode == 4) // main window
                {
                    s_MainWindow = win;
                    break;
                }
            }
        }

        if (s_MainWindow == null)
            return new Rect(0, 0, 800, 600);

        var positionProperty = s_MainWindow.GetType().GetProperty("position", BindingFlags.Public | BindingFlags.Instance);
        if (positionProperty == null)
            throw new MissingFieldException("Can't find internal fields 'position'. Maybe something has changed inside Unity.");
        return (Rect) positionProperty.GetValue(s_MainWindow, null);
    }

    internal static Rect GetMainWindowCenteredPosition(Vector2 size)
    {
        var mainWindowRect = GetEditorMainWindowPos();
        return GetCenteredWindowPosition(mainWindowRect, size);
    }
}