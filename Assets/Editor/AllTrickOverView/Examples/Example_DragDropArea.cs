using AllTrickOverView.Core;

namespace AllTrickOverView.Examples
{
    public class Example_DragDropArea : AExample_Base
    {
        public static TrickOverViewInfo TrickOverViewInfo =
            new TrickOverViewInfo("DragDropArea",
                "创建一个接口拖拽资源的区域",
                "Others",
                "obj = EditorGUIExtension.DragDropAreaSingle(r, DragAndDropVisualMode.Link);",
                "Assets/Editor/Examples/Example_64_DragDropArea",
                typeof(Example_DragDropArea),
                picPath : "Assets/Editor/Examples/Example_64_DragDropArea/88YKIHJ3ZZS}JQV}C124RPG.png",
                videoPath : "");

        public override TrickOverViewInfo GetTrickOverViewInfo()
        {
            return TrickOverViewInfo;
        }
    }
}
