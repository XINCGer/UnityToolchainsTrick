//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2021年4月11日 9:45:50
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AllTrickOverView.Core
{
    /// <summary>
    /// 一个示例条目的信息
    /// </summary>
    public class TrickOverViewInfo
    {
        /// <summary>
        /// 显示在treeView的条目名称
        /// </summary>
        public string Name = "NoName";

        /// <summary>
        /// 分组
        /// </summary>
        public string Category = "Uncategorized";

        /// <summary>
        /// The description of the example.
        /// </summary>
        public string Description;

        /// <summary>
        /// Raw code of the example.
        /// </summary>
        // Token: 0x04000422 RID: 1058
        public string Code;

        /// <summary>
        /// Raw code of the example.
        /// </summary>
        // Token: 0x04000422 RID: 1058
        public string CodePath;

        /// <summary>
        /// 预览用的图片路径
        /// </summary>
        public string PicPath;

        /// <summary>
        /// 预览用的视频路径
        /// </summary>
        public string VideoPath;

        /// <summary>
        /// 用于预览的数据，也就是AExample_Base子类
        /// </summary>
        private AExample_Base m_Example;

        public TrickOverViewInfo(string name, string description, string category, string code, string codePath,
            Type type, string picPath = "", string videoPath = "")
        {
            this.Name = name;
            this.Description = description;
            this.Category = category;

            this.Code = code;
            this.CodePath = codePath;

            this.m_Example = AllTrickOverViewUtilities.GetExampleByType(type);

            this.PicPath = picPath;
            this.VideoPath = videoPath;
        }

        public TrickOverViewInfo()
        {
        }
    }
}