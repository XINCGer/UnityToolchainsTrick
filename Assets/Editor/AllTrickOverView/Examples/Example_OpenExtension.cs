using AllTrickOverView.Core;

namespace AllTrickOverView.Examples
{
    public class Example_OpenExtension : AExample_Base
    {
        public static TrickOverViewInfo TrickOverViewInfo =
            new TrickOverViewInfo("OpenExtension",
                "通过C#的Process操作文件",
                "Project",
                "由于格式问题，此示例无法在此预览代码，请点击本条目中间的 重定向链接来定位代码文件",
                "Assets/Editor/Examples/Example_41_OpenExtension",
                typeof(Example_OpenExtension),
                picPath : "",
                videoPath : "");

        public override TrickOverViewInfo GetTrickOverViewInfo()
        {
            return TrickOverViewInfo;
        }
    }
}
