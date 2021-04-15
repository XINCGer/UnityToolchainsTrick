using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Video;
using Object = System.Object;

public class MediaPreviewWindow : EditorWindow
{
    private Object previewID;
    private VideoClip playingClip;
    private Texture2D playingPic;

    private static MediaPreviewWindow instance = null;

    public const float videoClipSizeScale = 0.8f;

    private Action onVideoWindowClosed = null;

    void Update()
    {
        if (EditorWindow.focusedWindow != this)
        {
            onVideoWindowClosed?.Invoke();
            this.Close();
        }
    }

    public static void ShowVideo(string videoAssetPath, Action onVideoWindowClose = null)
    {
        ShowMediaInternal(videoAssetPath, typeof(VideoClip));
        instance.onVideoWindowClosed = onVideoWindowClose;
    }
    
    public static void ShowPic(string picAssetPath, Action onVideoWindowClose = null)
    {
        ShowMediaInternal(picAssetPath, typeof(Texture2D));
        instance.onVideoWindowClosed = onVideoWindowClose;
    }

    private static void ShowMediaInternal(string mediaPath, Type mediaType)
    {
        var window = (MediaPreviewWindow) ScriptableObject.CreateInstance<MediaPreviewWindow>();

        Vector2 size = Vector2.zero;
        if (mediaType == typeof(VideoClip))
        {
            window.playingClip = AssetDatabase.LoadAssetAtPath<VideoClip>(mediaPath);
            window.previewID = MediaPreviewUtil.PlayPreview(window.playingClip);
            size = new Vector2(window.playingClip.width * videoClipSizeScale,
                window.playingClip.height * videoClipSizeScale);
        }

        if (mediaType == typeof(Texture2D))
        {
            window.playingPic = AssetDatabase.LoadAssetAtPath<Texture2D>(mediaPath);
            size = new Vector2(window.playingPic.width * videoClipSizeScale,
                window.playingPic.height * videoClipSizeScale);
        }

        window.maxSize = window.minSize = size;
        window.position = MediaPreviewUtil.GetMainWindowCenteredPosition(size);
        window.titleContent = new GUIContent("VideoWindow");
        window.ShowPopup();
        window.Focus();
        instance = window;
    }

    private void OnDestroy()
    {
        playingClip = null;
        playingPic = null;
        previewID = null;
        MediaPreviewUtil.StopPreview(previewID);
    }

    private void OnGUI()
    {
        if (playingClip != null)
        {
            Texture image = MediaPreviewUtil.GetPreviewTexture(previewID);
            if (image != null)
            {
                EditorGUI.DrawTextureTransparent(
                    new Rect(0, 0, image.width * videoClipSizeScale, image.height * videoClipSizeScale), image,
                    ScaleMode.ScaleToFit);
            }

            Repaint();
            return;
        }

        if (playingPic != null)
        {
            EditorGUI.DrawTextureTransparent(
                new Rect(0, 0, playingPic.width * videoClipSizeScale, playingPic.height * videoClipSizeScale),
                playingPic);
        }
    }
}