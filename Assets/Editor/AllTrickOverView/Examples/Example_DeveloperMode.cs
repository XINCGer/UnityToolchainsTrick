using AllTrickOverView.Core;

namespace AllTrickOverView.Examples
{
    public class Example_DeveloperMode : AExample_Base
    {
        public static TrickOverViewInfo TrickOverViewInfo =
            new TrickOverViewInfo("DeveloperMode",
                "DeveloperMode",
                "Project",
                "using System.Collections;\nusing System.Collections.Generic;\nusing UnityEditor;\nusing UnityEngine;\n\npublic class DeveloperMode : Editor\n{\n    [MenuItem(\"Tools/DeveloperMode/EnableDeveloperMode\", priority = 36)]\n    private static void Enable()\n    {\n        UnityEditor.EditorPrefs.SetBool(\"DeveloperMode\", true);\n    }\n    \n    [MenuItem(\"Tools/DeveloperMode/DisableDeveloperMode\", priority = 36)]\n    private static void Disable()\n    {\n        UnityEditor.EditorPrefs.SetBool(\"DeveloperMode\", false);\n    }\n}\n",
                "Assets/Editor/Examples/Example_36_DeveloperMode",
                typeof(Example_DeveloperMode),
                picPath : "Assets/Editor/Examples/Example_36_DeveloperMode/QQ截图20210419165101.png",
                videoPath : "");

        public override TrickOverViewInfo GetTrickOverViewInfo()
        {
            return TrickOverViewInfo;
        }
    }
}
