using AllTrickOverView.Core;

namespace AllTrickOverView.Examples
{
    public class Example_TitleModifier : AExample_Base
    {
        public static TrickOverViewInfo TrickOverViewInfo =
            new TrickOverViewInfo("TitleModifier",
                "Unity Tab修改器",
                "Project",
                "using System;\nusing System.Collections;\nusing System.Collections.Generic;\nusing System.Linq;\nusing System.Reflection;\nusing System.Threading.Tasks;\nusing UnityEditor;\nusing UnityEngine;\nusing UnityEngine.SceneManagement;\n\nnamespace ToolKits\n{\n    public class TitleModifier\n    {\n        private const int Delay = 2000;\n        \n        [InitializeOnLoadMethod]\n        private static void Init()\n        {\n            ModifyTitleAsync();\n        }\n\n        private static async void ModifyTitleAsync()\n        {\n            await Task.Delay(Delay);\n            ModifyTitle();\n        }\n        \n        private static void ModifyTitle()\n        {\n            Type tEditorApplication = typeof(EditorApplication);\n            Type tApplicationTitleDescriptor = tEditorApplication.Assembly.GetTypes()\n                .First(x => x.FullName == \"UnityEditor.ApplicationTitleDescriptor\");\n\n            EventInfo eiUpdateMainWindowTitle =\n                tEditorApplication.GetEvent(\"updateMainWindowTitle\", BindingFlags.Static | BindingFlags.NonPublic);\n            MethodInfo miUpdateMainWindowTitle =\n                tEditorApplication.GetMethod(\"UpdateMainWindowTitle\", BindingFlags.Static | BindingFlags.NonPublic);\n\n            Type delegateType = typeof(Action<>).MakeGenericType(tApplicationTitleDescriptor);\n            MethodInfo methodInfo = ((Action<object>) UpdateWindowTitle).Method;\n            Delegate del = Delegate.CreateDelegate(delegateType, null, methodInfo);\n\n            eiUpdateMainWindowTitle.GetAddMethod(true).Invoke(null, new object[] {del});\n            miUpdateMainWindowTitle.Invoke(null, new object[0]);\n            eiUpdateMainWindowTitle.GetRemoveMethod(true).Invoke(null, new object[] {del});\n        }\n\n        static void UpdateWindowTitle(object desc)\n        {\n            var fieldInfo = typeof(EditorApplication).Assembly.GetTypes()\n                .First(x => x.FullName == \"UnityEditor.ApplicationTitleDescriptor\")\n                .GetField(\"title\", BindingFlags.Instance | BindingFlags.Public);\n            var str = fieldInfo.GetValue(desc) as string;\n            fieldInfo.SetValue(desc, str + \"【分支】：release\");\n        }\n        \n    }\n}",
                "Assets/Editor/Examples/Example_20_TitleModifier",
                typeof(Example_TitleModifier),
                picPath : "Assets/Editor/Examples/Example_20_TitleModifier/QQ截图20210419155810.png",
                videoPath : "");

        public override TrickOverViewInfo GetTrickOverViewInfo()
        {
            return TrickOverViewInfo;
        }
    }
}
