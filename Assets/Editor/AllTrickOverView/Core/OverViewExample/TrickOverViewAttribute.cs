//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2021年4月11日 9:31:54
//------------------------------------------------------------

using System;

namespace AllTrickOverView.Core
{
    /// <summary>
    /// 使用此特性标记的类会被收集到TreeView
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class TrickOverViewAttribute:Attribute
    {
    }
}