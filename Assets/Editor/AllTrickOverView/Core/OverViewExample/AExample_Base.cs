//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2021年4月11日 9:29:49
//------------------------------------------------------------

using UnityEditor;
using UnityEngine;
using UnityEngine.Video;

namespace AllTrickOverView.Core
{
    /// <summary>
    /// Example的基类
    /// </summary>
    [TrickOverView]
    public abstract class AExample_Base
    {
        public static TrickOverViewInfo TrickOverViewInfo = new TrickOverViewInfo();

        public virtual TrickOverViewInfo GetTrickOverViewInfo()
        {
            return TrickOverViewInfo;
        }

        private VideoClip m_PlayingClip;
        private Texture2D m_PreviewPic;
        private Texture m_PlayVideoIcon;

        public virtual void Init()
        {
            m_PlayVideoIcon =
                AssetDatabase.LoadAssetAtPath<Texture>("Assets/Editor/AllTrickOverView/EditorResources/Video.png");
            if (!string.IsNullOrEmpty(this.GetTrickOverViewInfo().VideoPath))
            {
                m_PlayingClip = AssetDatabase.LoadAssetAtPath<VideoClip>(this.GetTrickOverViewInfo().VideoPath);
                if (m_PlayingClip != null)
                {
                    return;
                }
            }

            if (!string.IsNullOrEmpty(this.GetTrickOverViewInfo().PicPath))
            {
                m_PreviewPic = AssetDatabase.LoadAssetAtPath<Texture2D>(this.GetTrickOverViewInfo().PicPath);
            }
        }

        Texture GetAssetPreviewTexture(UnityEngine.Object target)
        {
            Texture tex = null;
            tex = AssetPreview.GetAssetPreview(target);
            if (!tex)
            {
                tex = AssetPreview.GetMiniThumbnail(target);
            }

            return tex;
        }

        public virtual void DrawUI(Rect rect)
        {
            if (m_PlayingClip != null)
            {
                Texture image = GetAssetPreviewTexture(m_PlayingClip);
                if (image != null)
                {
                    GetImageAppropriateSize(image, out var targetVideoSize);
                    GUILayout.Label("", GUILayout.Width(targetVideoSize.x), GUILayout.Height(targetVideoSize.y));
                    Rect videoRect = new Rect(rect.x + 10, rect.y + 10, targetVideoSize.x, targetVideoSize.y);
                    if (GUI.Button(videoRect, ""))
                    {
                        MediaPreviewWindow.ShowVideo(this.GetTrickOverViewInfo().VideoPath,
                            delegate { Resources.FindObjectsOfTypeAll<AllTrickOverViewEditorWindow>()[0].Focus(); });
                    }

                    EditorGUI.DrawPreviewTexture(videoRect, image);
                    Rect playIconRect = new Rect(videoRect.center - new Vector2(25, 25), new Vector2(50, 50));
                    GUI.DrawTexture(playIconRect, m_PlayVideoIcon);
                    EditorGUIUtility.AddCursorRect(videoRect, MouseCursor.Link);
                }

                return;
            }

            if (m_PreviewPic != null)
            {
                Texture image = GetAssetPreviewTexture(m_PreviewPic);
                if (image != null)
                {
                    GetImageAppropriateSize(image, out var targetPicSize);
                    GUILayout.Label("", GUILayout.Width(targetPicSize.x), GUILayout.Height(targetPicSize.y));
                    Rect videoRect = new Rect(rect.x + 10, rect.y + 10, targetPicSize.x, targetPicSize.y);
                    if (GUI.Button(videoRect, ""))
                    {
                        MediaPreviewWindow.ShowPic(this.GetTrickOverViewInfo().PicPath,
                            delegate { Resources.FindObjectsOfTypeAll<AllTrickOverViewEditorWindow>()[0].Focus(); });
                    }

                    EditorGUI.DrawPreviewTexture(videoRect, image);
                    Rect playIconRect = new Rect(videoRect.center - new Vector2(25, 25), new Vector2(50, 50));
                    GUI.DrawTexture(playIconRect, m_PlayVideoIcon);
                    EditorGUIUtility.AddCursorRect(videoRect, MouseCursor.Link);
                }
            }
        }

        private static void GetImageAppropriateSize(Texture image, out Vector2 size)
        {
            size = (image.height > 720 || image.width > 1280)
                ? new Vector2(1280, 720)
                : new Vector2(image.width, image.height);
        }

        public virtual void Destroy()
        {
            m_PreviewPic = null;
            m_PlayingClip = null;
        }
    }
}