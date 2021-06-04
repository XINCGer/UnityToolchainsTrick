using AllTrickOverView.Core;

namespace AllTrickOverView.Examples
{
    public class Example_CustomAssetsIcon : AExample_Base
    {
        public static TrickOverViewInfo TrickOverViewInfo =
            new TrickOverViewInfo("CustomAssetsIcon",
                "自定义资产图标",
                "Assets",
                "None",
                "Assets/Editor/Examples/Example_53_CustomAssetsIcon",
                typeof(Example_CustomAssetsIcon),
                picPath : "Assets/Editor/Examples/Example_53_CustomAssetsIcon/QQ截图20210604131306.png",
                videoPath : "");

        public override TrickOverViewInfo GetTrickOverViewInfo()
        {
            return TrickOverViewInfo;
        }
    }
}
