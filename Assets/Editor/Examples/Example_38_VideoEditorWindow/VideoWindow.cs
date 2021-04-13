using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Video;
using Object = System.Object;

public class VideoWindow : EditorWindow
{
    private Object previewID;
    private VideoClip playingClip;

    private static VideoWindow instance = null;

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
        var window = (VideoWindow) ScriptableObject.CreateInstance<VideoWindow>();
        window.playingClip = AssetDatabase.LoadAssetAtPath<VideoClip>(videoAssetPath);
        window.previewID = VideoUtil.PlayPreview(window.playingClip);
        Vector2 size = new Vector2(window.playingClip.width * videoClipSizeScale, window.playingClip.height * videoClipSizeScale);
        window.maxSize = window.minSize = size;
        window.position = VideoUtil.GetMainWindowCenteredPosition(size);
        window.titleContent = new GUIContent("VideoWindow");
        window.ShowPopup();
        window.Focus();
        window.onVideoWindowClosed = onVideoWindowClose;
        instance = window;
    }

    private void OnDestroy()
    {
        VideoUtil.StopPreview(previewID);
    }

    private void OnGUI()
    {
        Texture image = VideoUtil.GetPreviewTexture(previewID);
        if (image != null)
        {
            EditorGUI.DrawTextureTransparent(new Rect(0, 0, image.width * videoClipSizeScale, image.height * videoClipSizeScale), image, ScaleMode.ScaleToFit);
        }

        Repaint();
    }
}