using AllTrickOverView.Core;

namespace AllTrickOverView.Examples
{
    public class Example_ReplacementComponents : AExample_Base
    {
        public static TrickOverViewInfo TrickOverViewInfo =
            new TrickOverViewInfo("ReplacementComponents",
                "快捷替换组件",
                "Inspector",
                "//Create: Icarus\n//ヾ(•ω•`)o\n//2021-04-17 10:34\n//Assembly-CSharp-Editor\n\nusing UnityEditor;\nusing UnityEngine;\nusing UnityEngine.UI;\n\nnamespace CabinIcarus.EditorFrame.Utils\n{\n    public class Example\n    {\n        [MenuItem(\"CONTEXT/Image/Replace To Test Image\")]\n        static void _imageReplaceToTestImage(MenuCommand command)\n        {\n            ((Component) command.context).ReplaceComponent<TestImage>();\n        } \n        \n        [MenuItem(\"CONTEXT/TestImage/Replace To Image\")]\n        static void _testImageReplaceToImage(MenuCommand command)\n        {\n            ((Component) command.context).ReplaceComponent<Image>();\n        } \n    }\n\n    class TestImage:Image\n    {\n        \n    }\n}",
                "Assets/Editor/Examples/Example_44_ReplacementComponents",
                typeof(Example_ReplacementComponents),
                picPath : "Assets/Editor/Examples/Example_44_ReplacementComponents/QQ截图20210419170049.png",
                videoPath : "");

        public override TrickOverViewInfo GetTrickOverViewInfo()
        {
            return TrickOverViewInfo;
        }
    }
}
