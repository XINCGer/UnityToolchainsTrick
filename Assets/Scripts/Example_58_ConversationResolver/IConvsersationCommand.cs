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

public interface IConversationCommand
{
    int Priority { get; }

    void SetUp(string[] _commandParams);

    void Process();
}
