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
using UnityEngine;

namespace CZToolKit.ConversationResolver
{
    [CommandName("timer")]
    public struct TimerCommand : IConversationCommand
    {
        int timer;

        public int Priority => 0;

        public void SetUp(string[] _commandParams)
        {
            int.TryParse(_commandParams[0], out timer);
        }

        public void Process()
        {
            // 在这里写计时timer毫秒的代码
            Debug.Log($"等待{timer}毫秒");
        }
    }

    [CommandName("fade")]
    public struct FadeCommand : IConversationCommand
    {
        bool show;

        public int Priority => -9;

        public void SetUp(string[] _commandParams)
        {
            show = _commandParams[0] == "true";
        }

        public void Process()
        {
            if (show)
                Debug.Log("角色渐现");
            else
                Debug.Log("角色渐隐");
        }
    }

    [CommandName("character")]
    public struct CharacterCommand : IConversationCommand
    {
        string character;

        public int Priority => -10;

        public void SetUp(string[] _commandParams)
        {
            character = _commandParams[0];
        }

        public void Process()
        {
            Debug.Log(character);
        }
    }
}
