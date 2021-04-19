using AllTrickOverView.Core;

namespace AllTrickOverView.Examples
{
    public class Example_CompositeEnum : AExample_Base
    {
        public static TrickOverViewInfo TrickOverViewInfo =
            new TrickOverViewInfo("CompositeEnum",
                "复合枚举，支持位运算",
                "Others",
                "using System;\nusing System.Collections;\nusing System.Collections.Generic;\nusing UnityEditor;\nusing UnityEngine;\n\nnamespace ToolKits\n{\n    [System.Flags]\n    public enum CompositeEnum\n    {\n        Red = 1 << 1,\n        Blue = 1 << 2,\n        Yellow = 1 << 3,\n        All = Red | Blue | Yellow,\n    }\n\n    public class CompositeEnumWindow : EditorWindow\n    {\n        private static CompositeEnumWindow _window;\n        private static readonly Vector2 MIN_SIE = new Vector2(400, 300);\n\n        private CompositeEnum _compositeEnum;\n\n        [MenuItem(\"Tools/复合枚举\", priority = 30)]\n        private static void PopUp()\n        {\n            _window = GetWindow<CompositeEnumWindow>(\"复合枚举\");\n            _window.minSize = MIN_SIE;\n            _window.Show();\n        }\n\n        private void OnGUI()\n        {\n            _compositeEnum = (CompositeEnum) EditorGUILayout.EnumFlagsField(\"复合枚举\", _compositeEnum);\n            if (GUILayout.Button(\"是否包含Red\"))\n            {\n                var contain = (_compositeEnum & CompositeEnum.Red) > 0;\n                Debug.Log(\"是否包含Red？{contain}\");\n            }\n        }\n    }\n}",
                "Assets/Editor/Examples/Example_30_CompositeEnum",
                typeof(Example_CompositeEnum),
                picPath : "Assets/Editor/Examples/Example_30_CompositeEnum/QQ截图20210419161705.png",
                videoPath : "");

        public override TrickOverViewInfo GetTrickOverViewInfo()
        {
            return TrickOverViewInfo;
        }
    }
}
