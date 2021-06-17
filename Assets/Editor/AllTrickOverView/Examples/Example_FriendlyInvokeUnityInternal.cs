using AllTrickOverView.Core;

namespace AllTrickOverView.Examples
{
    public class Example_FriendlyInvokeUnityInternal : AExample_Base
    {
        public static TrickOverViewInfo TrickOverViewInfo =
            new TrickOverViewInfo("FriendlyInvokeUnityInternal",
                "利用友元程序集直接调用Unity内部函数",
                "Others",
                "//None 请直接定位到文件夹查看用法",
                "Assets/Editor/Examples/Example_49_Friend",
                typeof(Example_FriendlyInvokeUnityInternal),
                picPath : "",
                videoPath : "");

        public override TrickOverViewInfo GetTrickOverViewInfo()
        {
            return TrickOverViewInfo;
        }
    }
}
