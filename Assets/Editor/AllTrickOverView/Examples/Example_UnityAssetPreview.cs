using AllTrickOverView.Core;

namespace AllTrickOverView.Examples
{
    public class Example_Unity资源预览 : AExample_Base
    {
        public static TrickOverViewInfo TrickOverViewInfo =
            new TrickOverViewInfo("Unity资源预览",
                "Unity资源预览，只要带有预览窗口的都可以",
                "Others",
                "\nUnityEngine.Object model = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(\"Assets/Editor/Examples/Example_63_ObjectPreview/SF_Animal_Boar.fbx\");\nmodelEditor = Editor.CreateEditor(model);\nmodelEditor.DrawPreview(new Rect(50, 50, 300, 300));",
                "Assets/Editor/Examples/Example_63_ObjectPreview",
                typeof(Example_Unity资源预览),
                picPath : "Assets/Editor/Examples/Example_63_ObjectPreview/16B93I)M7OD$}_A$_F{DY82.png",
                videoPath : "");

        public override TrickOverViewInfo GetTrickOverViewInfo()
        {
            return TrickOverViewInfo;
        }
    }
}
