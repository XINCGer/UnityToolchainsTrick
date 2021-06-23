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
using CZToolKit.Core;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace CZToolKit.ConversationResolver
{
    public struct RawConversationItem
    {
        public string text;
        public RawCommand[] commands;
    }

    public struct RawCommand
    {
        public string command;
        public string[] commandParams;
    }

    public class ConversationManager : MonoBehaviour
    {
        Dictionary<string, Type> CommandResolverTypeCache;
        static Regex ConversationTextRegex = new Regex(@"(?<=\n|^)\s*(?<commands>[^\n:]*):(?<text>.*)(?=\n|$)");
        static Regex CommandsRegex = new Regex(@"(?<= |^)(?<command>\S+)\((?<params>[^\(\)]*)\)|\S+");
        static Regex ParamsSplitRegex = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

        public ConversationManager()
        {
            if (CommandResolverTypeCache == null)
            {
                CommandResolverTypeCache = new Dictionary<string, Type>();
                foreach (var type in Utility_Reflection.GetChildrenTypes<IConversationCommand>())
                {
                    if (Utility_Attribute.TryGetTypeAttribute(type, out CommandNameAttribute attribute))
                        CommandResolverTypeCache[attribute.commandText] = type;
                }
            }
        }

        public static void Process(string _conversationText, Action<RawConversationItem> _itemAction)
        {
            MatchCollection matches = ConversationTextRegex.Matches(_conversationText);

            for (int i = 0; i < matches.Count; i++)
            {
                RawConversationItem rawConversationItem = new RawConversationItem();

                string commandsText = matches[i].Groups["commands"].Value.Trim();
                MatchCollection commandsMatches = CommandsRegex.Matches(commandsText);
                RawCommand[] rawCommands = new RawCommand[commandsMatches.Count];
                for (int j = 0; j < commandsMatches.Count; j++)
                {
                    string command = commandsMatches[j].Groups["command"].Value;
                    if (string.IsNullOrEmpty(command))
                        command = commandsMatches[j].Value;

                    string commandParamsText = commandsMatches[j].Groups["params"].Value;
                    string[] commandParams = null;
                    if (!string.IsNullOrEmpty(commandParamsText))
                        commandParams = ParamsSplitRegex.Split(commandsMatches[j].Groups["params"].Value);
                    else
                        commandParams = new string[0];
                    rawCommands[j] = new RawCommand() { command = command, commandParams = commandParams };
                }

                rawConversationItem.text = matches[i].Groups["text"].Value;
                rawConversationItem.commands = rawCommands;

                _itemAction.Invoke(rawConversationItem);
            }
        }

        public void BeginConversation(string _conversationText)
        {
            Process(_conversationText, _rawItem =>
            {
                List<IConversationCommand> conversationCommands = new List<IConversationCommand>();
                foreach (var rawCommand in _rawItem.commands)
                {
                    if (CommandResolverTypeCache.TryGetValue(rawCommand.command, out Type commandType))
                    {
                        IConversationCommand command = Activator.CreateInstance(commandType) as IConversationCommand;
                        command.SetUp(rawCommand.commandParams);
                        conversationCommands.Add(command);
                    }
                }
                foreach (var command in conversationCommands)
                {
                    command.Process();
                }
                Debug.Log($"说:{_rawItem.text}");
            });
        }
    }
}
