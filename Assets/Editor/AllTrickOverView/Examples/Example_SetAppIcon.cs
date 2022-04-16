using AllTrickOverView.Core;

namespace AllTrickOverView.Examples
{
    public class Example_SetAppIcon : AExample_Base
    {
        public static TrickOverViewInfo TrickOverViewInfo =
            new TrickOverViewInfo("SetAppIcon",
                "通过代码设置AppIcon",
                "Project",
                "using System.Collections;\nusing System.Collections.Generic;\nusing UnityEditor;\nusing UnityEngine;\n\nnamespace UnityToolchinsTrick\n{\n    public class Example_73_SetAppIcon\n    {\n        private const string icon_path = \"Assets/Editor/Examples/Example_73_SetAppIcon/icon.png\";\n\n        [MenuItem(\"Tools/Example_73_SetAppIcon/SetDefaultIcon\", priority = 73)]\n        private static void SetDefaultIcon()\n        {\n            Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(icon_path);\n            UnityInternalAPIEngineBridger.SetDefaultIcon(texture);\n        }\n\n        [MenuItem(\"Tools/Example_73_SetAppIcon/SetAppIcon\", priority = 73)]\n        private static void SetAppIcon()\n        {\n            Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(icon_path);\n\n            int[] iconSize = PlayerSettings.GetIconSizesForTargetGroup(BuildTargetGroup.iOS);\n            Texture2D[] textureArray = new Texture2D[iconSize.Length];\n            for (int i = 0; i < textureArray.Length; i++)\n            {\n                textureArray[i] = texture;\n            }\n\n            textureArray[0] = texture;\n            PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.iOS, textureArray);\n            AssetDatabase.SaveAssets();\n        }\n    }\n}",
                "Assets/Editor/Examples/Example_73_SetAppIcon",
                typeof(Example_SetAppIcon),
                picPath : "Assets/Editor/Examples/Example_73_SetAppIcon/preview.png",
                videoPath : "");

        public override TrickOverViewInfo GetTrickOverViewInfo()
        {
            return TrickOverViewInfo;
        }
    }
}
