using AllTrickOverView.Core;

namespace AllTrickOverView.Examples
{
    public class Example_ConversationResolver : AExample_Base
    {
        public static TrickOverViewInfo TrickOverViewInfo =
            new TrickOverViewInfo("ConversationResolver",
                "对话解析器",
                "ConversationResolver",
                "MatchCollection matches = ConversationTextRegex.Matches(_conversationText);\n\n            for (int i = 0; i < matches.Count; i++)\n            {\n                RawConversationItem rawConversationItem = new RawConversationItem();\n\n                string commandsText = matches[i].Groups[\"commands\"].Value.Trim();\n                MatchCollection commandsMatches = CommandsRegex.Matches(commandsText);\n                RawCommand[] rawCommands = new RawCommand[commandsMatches.Count];\n                for (int j = 0; j < commandsMatches.Count; j++)\n                {\n                    string command = commandsMatches[j].Groups[\"command\"].Value;\n                    if (string.IsNullOrEmpty(command))\n                        command = commandsMatches[j].Value;\n\n                    string commandParamsText = commandsMatches[j].Groups[\"params\"].Value;\n                    string[] commandParams = null;\n                    if (!string.IsNullOrEmpty(commandParamsText))\n                        commandParams = ParamsSplitRegex.Split(commandsMatches[j].Groups[\"params\"].Value);\n                    else\n                        commandParams = new string[0];\n                    rawCommands[j] = new RawCommand() { command = command, commandParams = commandParams };\n                }\n\n                rawConversationItem.text = matches[i].Groups[\"text\"].Value;\n                rawConversationItem.commands = rawCommands;\n\n                _itemAction.Invoke(rawConversationItem);\n            }",
                "Assets/Scripts/Example_58_ConversationResolver",
                typeof(Example_ConversationResolver),
                picPath : "Assets/Scripts/Example_58_ConversationResolver/1.png",
                videoPath : "");

        public override TrickOverViewInfo GetTrickOverViewInfo()
        {
            return TrickOverViewInfo;
        }
    }
}
