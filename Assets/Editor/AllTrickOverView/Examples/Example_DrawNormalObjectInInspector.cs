using AllTrickOverView.Core;

namespace AllTrickOverView.Examples
{
    public class Example_DrawNormalObjectInInspector : AExample_Base
    {
        public static TrickOverViewInfo TrickOverViewInfo =
            new TrickOverViewInfo("DrawNormalObjectInInspector",
                "绘制一个普通对象到Inspector",
                "Inspector",
                "using System.Collections;\nusing System.Collections.Generic;\nusing UnityEngine;\nusing UnityEditor;\nusing CZToolKit.Core;\nusing CZToolKit.Core.Editors;\nusing ToolKits;\n\npublic class DrawNormalObjectEditorWindow : EditorWindow\n{\n    [MenuItem(\"Tools/DrawNormalObject\")]\n    public static void Open()\n    {\n        GetWindow<DrawNormalObjectEditorWindow>();\n    }\n    TestData data = new TestData();\n\n    void OnGUI()\n    {\n        EditorGUILayoutExtension.DrawFields(data);\n\n        if (GUILayout.Button(\"绘制一个普通对象到Inspector\"))\n        {\n            EditorGUILayoutExtension.DrawFieldsInInspector(\"Test\", data);\n        }\n    }\n}\n\npublic class TestData\n{\n    public float f;\n    [FloatRange(0, 1)]\n    public List<float> sliders;\n}\n\npublic class FloatRangeAttribute : ObjectDrawerAttribute\n{\n    public float minLimit, maxLimit;\n    public FloatRangeAttribute(float _minLimit, float _maxLimit)\n    {\n        minLimit = _minLimit;\n        maxLimit = _maxLimit;\n    }\n}\n\n[CustomFieldDrawer(typeof(FloatRangeAttribute))]\npublic class FloatRangeDrawer : FieldDrawer\n{\n    public override void OnGUI(GUIContent label)\n    {\n        FloatRangeAttribute rangeAttribute = Attribute as FloatRangeAttribute;\n        Value = EditorGUILayout.Slider(label, (float)Value, rangeAttribute.minLimit, rangeAttribute.maxLimit);\n    }\n}",
                "Assets/Editor/Examples/Example_47_DrawNormalObject",
                typeof(Example_DrawNormalObjectInInspector),
                picPath : "Assets/Editor/Examples/Example_47_DrawNormalObject/QQ截图20210617214242.png",
                videoPath : "");

        public override TrickOverViewInfo GetTrickOverViewInfo()
        {
            return TrickOverViewInfo;
        }
    }
}
