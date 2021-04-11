using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.Video;
using Object = System.Object;

public static class VideoUtil
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
}