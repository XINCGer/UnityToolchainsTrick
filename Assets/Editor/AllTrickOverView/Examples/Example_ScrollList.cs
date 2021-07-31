using AllTrickOverView.Core;

namespace AllTrickOverView.Examples
{
    public class Example_ScrollList : AExample_Base
    {
        public static TrickOverViewInfo TrickOverViewInfo =
            new TrickOverViewInfo("ScrollList",
                "绘制一个可滚动显示的List，通用",
                "Draw",
                "\nnumsScroll = EditorGUILayoutExtension.ScrollList(serializedObject.FindProperty(\"nums\"), numsScroll, ref numsFoldout, 15);",
                "Assets/Editor/Examples/Example_65_ScrollList",
                typeof(Example_ScrollList),
                picPath : "Assets/Editor/Examples/Example_65_ScrollList/1.png",
                videoPath : "");

        public override TrickOverViewInfo GetTrickOverViewInfo()
        {
            return TrickOverViewInfo;
        }
    }
}
