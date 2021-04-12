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
        private System.Object m_PreviewID;
        private Texture2D m_PreviewPic;

        public virtual void Init()
        {
            if (!string.IsNullOrEmpty(this.GetTrickOverViewInfo().VideoPath))
            {
                m_PlayingClip = AssetDatabase.LoadAssetAtPath<VideoClip>(this.GetTrickOverViewInfo().VideoPath);
                if (m_PlayingClip != null)
                {
                    m_PreviewID = VideoUtil.PlayPreview(m_PlayingClip);
                    return;
                }
            }

            if (!string.IsNullOrEmpty(this.GetTrickOverViewInfo().PicPath))
            {
                m_PreviewPic = AssetDatabase.LoadAssetAtPath<Texture2D>(this.GetTrickOverViewInfo().PicPath);
            }
        }

        public virtual void DrawUI(Rect rect)
        {
            if (m_PlayingClip != null)
            {
                Texture image = VideoUtil.GetPreviewTexture(m_PreviewID);
                if (image != null)
                {
                    GetImageAppropriateSize(image, out var targetVideoSize);
                    GUILayout.Label("", GUILayout.Width(targetVideoSize.x), GUILayout.Height(targetVideoSize.y));
                    EditorGUI.DrawTextureTransparent(
                        new Rect(rect.x + 10, rect.y + 10, targetVideoSize.x, targetVideoSize.y), image,
                        ScaleMode.ScaleToFit);
                }

                return;
            }

            if (m_PreviewPic != null)
            {
                GetImageAppropriateSize(m_PreviewPic, out var targetVideoSize);
                GUILayout.Label("", GUILayout.Width(targetVideoSize.x), GUILayout.Height(targetVideoSize.y));
                EditorGUI.DrawTextureTransparent(
                    new Rect(rect.x + 10, rect.y + 10, targetVideoSize.x, targetVideoSize.y), m_PreviewPic,
                    ScaleMode.ScaleToFit);
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
            if (m_PreviewID != null)
                VideoUtil.StopPreview(m_PreviewID);

            m_PreviewPic = null;
            m_PlayingClip = null;
            m_PreviewID = null;
        }
    }
}