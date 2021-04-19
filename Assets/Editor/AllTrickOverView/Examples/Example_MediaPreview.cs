using AllTrickOverView.Core;
using UnityEngine;

namespace AllTrickOverView.Examples
{
    public class Example_MediaPreview : AExample_Base
    {
        public static TrickOverViewInfo TrickOverViewInfo =
            new TrickOverViewInfo("MediaPreview",
                "多媒体预览",
                "Media",
                "using System;\nusing UnityEditor;\nusing UnityEngine;\n\npublic class MediaPreviewWindowDemo : EditorWindow\n{\n    private static MediaPreviewWindowDemo _window;\n    private static readonly Vector2 MIN_SIE = new Vector2(400, 300);\n\n    [MenuItem(\"Tools/多媒体预览\", priority = 38)]\n    private static void PopUp()\n    {\n        _window = GetWindow<MediaPreviewWindowDemo>(\"多媒体预览\");\n        _window.minSize = MIN_SIE;\n        _window.Show();\n    }\n\n    private void OnGUI()\n    {\n        if (GUILayout.Button(\"显示图片预览\"))\n        {\n            MediaPreviewWindow.ShowPic(\"Assets/Editor/Examples/Example_16_MoreSceneView/MoreSceneViewPreviewPng.png\",\n                Resources.FindObjectsOfTypeAll<MediaPreviewWindowDemo>()[0].Focus);\n        }\n\n        if (GUILayout.Button(\"显示视频预览\"))\n        {\n            MediaPreviewWindow.ShowVideo(\"Assets/Editor/Examples/Example_15_SubWindowDock/SubWindowDockVideo.mp4\",\n                Resources.FindObjectsOfTypeAll<MediaPreviewWindowDemo>()[0].Focus);\n        }\n    }\n}",
                "Assets/Editor/Examples/Example_38_MediaPreviewEditorWindow",
                typeof(Example_MediaPreview),
                picPath : "",
                videoPath : "");

        public override TrickOverViewInfo GetTrickOverViewInfo()
        {
            return TrickOverViewInfo;
        }

        public override void DrawUI(Rect rect)
        {
            if (GUILayout.Button("显示图片预览"))
            {
                MediaPreviewWindow.ShowPic("Assets/Editor/Examples/Example_16_MoreSceneView/MoreSceneViewPreviewPng.png",
                    Resources.FindObjectsOfTypeAll<AllTrickOverViewEditorWindow>()[0].Focus);
            }

            if (GUILayout.Button("显示视频预览"))
            {
                MediaPreviewWindow.ShowVideo("Assets/Editor/Examples/Example_15_SubWindowDock/SubWindowDockVideo.mp4",
                    Resources.FindObjectsOfTypeAll<AllTrickOverViewEditorWindow>()[0].Focus);
            }
        }
    }
}
