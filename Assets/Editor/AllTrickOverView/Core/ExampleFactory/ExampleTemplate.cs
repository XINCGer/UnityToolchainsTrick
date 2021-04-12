//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2021年4月12日 14:50:40
//------------------------------------------------------------

using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace AllTrickOverView.Core.ExampleFactory
{
    public class ExampleTemplate
    {
        [LabelText("显示在TreeView区域的名称")] public string Name = "NoName";

        [LabelText("显示在TreeView区域的分类名")] public string Category = "Uncategorized";

        [LabelText("Example的描述")] public string Description;

        [LabelText("关键部分Code")] [TextArea(10, 15)]
        public string Code;

        [LabelText("对应的代码文件夹")] [FolderPath] public string CodePath;

        [InfoBox("注意，视频优先级大于图片，例如图片路径和视频路径都不为空，则显示视频而隐藏图片", InfoMessageType.Warning)]
        [LabelText("用于预览的图片路径")]
        [Sirenix.OdinInspector.FilePath]
        public string PicPath;

        [LabelText("用于预览的视频路径")] [Sirenix.OdinInspector.FilePath] public string VideoPath;

        [Button("自动生成OverViewExample代码", 25)]
        public void GenerateOverViewExampleCode()
        {
            ExampleFactoryUtilities.CreateOverViewExampleFromTemplate(this);
            AssetDatabase.Refresh();
        }
    }
}