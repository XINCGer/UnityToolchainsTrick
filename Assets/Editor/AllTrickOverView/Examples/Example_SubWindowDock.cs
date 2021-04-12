//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2021年4月11日 22:45:57
//------------------------------------------------------------

using AllTrickOverView.Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.Video;

namespace AllTrickOverView.Examples
{
    /// <summary>
    /// 这是一个视频示例
    /// </summary>
    public class Example_SubWindowDock : AExample_Base
    {
        public static TrickOverViewInfo TrickOverViewInfo =
            new TrickOverViewInfo("SubWindowDock",
                "Window的Dock模式。",
                "EditorWindow",
                "",
                "Assets/Editor/Examples/Example_15_SubWindowDock",
                typeof(Example_SubWindowDock),
                videoPath:"Assets/Editor/Examples/Example_15_SubWindowDock/SubWindowDockVideo.mp4");

        public override TrickOverViewInfo GetTrickOverViewInfo()
        {
            return TrickOverViewInfo;
        }
    }
}