using System;
using UnityEditor;
using UnityEngine;

public class MediaPreviewWindowDemo : EditorWindow
{
    private static MediaPreviewWindowDemo _window;
    private static readonly Vector2 MIN_SIE = new Vector2(400, 300);

    [MenuItem("Tools/多媒体预览", priority = 38)]
    private static void PopUp()
    {
        _window = GetWindow<MediaPreviewWindowDemo>("多媒体预览");
        _window.minSize = MIN_SIE;
        _window.Show();
    }

    private void OnGUI()
    {
        if (GUILayout.Button("显示图片预览"))
        {
            MediaPreviewWindow.ShowPic("Assets/Editor/Examples/Example_16_MoreSceneView/MoreSceneViewPreviewPng.png",
                Resources.FindObjectsOfTypeAll<MediaPreviewWindowDemo>()[0].Focus);
        }

        if (GUILayout.Button("显示视频预览"))
        {
            MediaPreviewWindow.ShowVideo("Assets/Editor/Examples/Example_15_SubWindowDock/SubWindowDockVideo.mp4",
                Resources.FindObjectsOfTypeAll<MediaPreviewWindowDemo>()[0].Focus);
        }
    }
}