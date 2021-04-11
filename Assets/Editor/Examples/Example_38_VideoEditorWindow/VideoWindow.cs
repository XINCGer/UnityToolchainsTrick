using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Video;
using Object = System.Object;

public class VideoWindow : EditorWindow
{
    private Object m_PreviewID;
    private VideoClip m_PlayingClip;

    [MenuItem("Tools/VideoEditorWindow", priority = 38)]
    private static void ShowWindow()
    {
        var window = GetWindow<VideoWindow>();
        window.titleContent = new GUIContent("VideoWindow");

        window.Show();
    }

    private void OnEnable()
    {
        m_PlayingClip = AssetDatabase.LoadAssetAtPath<VideoClip>("Assets/Editor/Examples/Example_38_VideoEditorWindow/Test.MP4");
        m_PreviewID = VideoUtil.PlayPreview(m_PlayingClip);
    }

    private void OnDestroy()
    {
        VideoUtil.StopPreview(m_PreviewID);
    }

    private void OnGUI()
    {
        Texture image = VideoUtil.GetPreviewTexture(m_PreviewID);
        if (image != null)
        {
            EditorGUI.DrawTextureTransparent(new Rect(0, 0, image.width, image.height), image, ScaleMode.ScaleToFit);
        }

        Repaint();
    }
}