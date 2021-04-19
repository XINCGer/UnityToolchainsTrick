using AllTrickOverView.Core;

namespace AllTrickOverView.Examples
{
    public class Example_SetObjectIcon : AExample_Base
    {
        public static TrickOverViewInfo TrickOverViewInfo =
            new TrickOverViewInfo("SetObjectIcon",
                "设置对象的图标",
                "Inspector",
                "using System.Reflection;\nusing UnityEditor;\nusing UnityEngine;\n\nnamespace SetObjectIcon\n{\n    public static class EditorGUIExtension\n    {\n        //sv_label_0 - sv_label_7\n        //sv_icon_name0 - sv_icon_name7\n        //sv_icon_dot0_sml - sv_icon_dot15_sml\n        //sv_icon_dot0_pix16_gizmo - sv_icon_dot15_pix16_gizmo\n        public static void SetIcon(Object obj, string name)\n        {\n            Texture2D tex = EditorGUIUtility.IconContent(name).image as Texture2D;\n            object[] args = new object[] {obj, tex};\n            typeof(EditorGUIUtility).InvokeMember(\"SetIconForObject\", BindingFlags.InvokeMethod | BindingFlags.Static | BindingFlags.NonPublic, null, null, args);\n        }\n    }\n}",
                "Assets/Editor/Examples/Example_35_SetObjectIcon",
                typeof(Example_SetObjectIcon),
                picPath : "Assets/Editor/Examples/Example_35_SetObjectIcon/QQ截图20210419164539.png",
                videoPath : "");

        public override TrickOverViewInfo GetTrickOverViewInfo()
        {
            return TrickOverViewInfo;
        }
    }
}
