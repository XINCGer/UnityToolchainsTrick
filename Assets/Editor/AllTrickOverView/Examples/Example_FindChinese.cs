using AllTrickOverView.Core;

namespace AllTrickOverView.Examples
{
    public class Example_FindChinese : AExample_Base
    {
        public static TrickOverViewInfo TrickOverViewInfo =
            new TrickOverViewInfo("FindChinese",
                "查找文件夹下所有cs文件内的中文（不包括注释等）",
                "Project",
                "    private bool HasChinese(string str)\n    {\n        return Regex.IsMatch(str, @\"[\u4e00-\u9fa5]\");\n    }",
                "Assets/Editor/Examples/Example_46_FindChinese",
                typeof(Example_FindChinese),
                picPath : "Assets/Editor/Examples/Example_46_FindChinese/18290402457571.png",
                videoPath : "");

        public override TrickOverViewInfo GetTrickOverViewInfo()
        {
            return TrickOverViewInfo;
        }
    }
}
