#region 注 释
/***
 *
 *  Title:
 *  
 *  Description:
 *  
 *  Date:
 *  Version:
 *  Writer: 
 *
 */
#endregion
using System;

namespace CZToolKit.ConversationResolver
{
    [AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class)]
    public class CommandNameAttribute : Attribute
    {
        public string commandText;

        public CommandNameAttribute(string _commandText)
        {
            commandText = _commandText;
        }
    }
}