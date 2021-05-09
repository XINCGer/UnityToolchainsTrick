#region 注 释
/***
 *
 *  Title: 
 *  自定义Object绘制
 *  Description:
 *  自定义ObjectEditor类添加此特性，就会替换原始绘制方式
 *  Date:
 *  Version:
 *  Writer: 
 *
 */
#endregion
using System;

namespace CZToolKit.Core.Editors
{
    public class CustomObjectEditorAttribute : Attribute
    {
        public Type targetType;

        public CustomObjectEditorAttribute(Type _targetType) { targetType = _targetType; }
    }
}
