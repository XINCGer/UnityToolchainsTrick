//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2021年4月11日 9:29:49
//------------------------------------------------------------

using UnityEngine;

namespace Plugins.AllTrickOverView.Core
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

        public virtual void DrawUI(Rect rect)
        {

        }

        public virtual void Init()
        {

        }
    }
}