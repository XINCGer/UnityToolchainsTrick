using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using CZToolKit.Core;
using CZToolKit.Core.Editors;
using ToolKits;

using EditorGUILayoutExtension = CZToolKit.Core.Editors.EditorGUILayoutExtension;

namespace CZToolKit.Examples
{
    public class DrawNormalObjectEditorWindow : EditorWindow
    {
        [MenuItem("Tools/DrawNormalObject")]
        public static void Open()
        {
            GetWindow<DrawNormalObjectEditorWindow>();
        }
        TestData data = new TestData();

        void OnGUI()
        {
            EditorGUILayoutExtension.DrawFields(data);

            if (GUILayout.Button("绘制一个普通对象到Inspector"))
            {
                EditorGUILayoutExtension.DrawFieldsInInspector("Test", data);
            }
        }
    }

    public class TestData
    {
        public float f;
        [FloatRange(0, 1)]
        public List<float> sliders;
    }

    public class FloatRangeAttribute : ObjectDrawerAttribute
    {
        public float minLimit, maxLimit;
        public FloatRangeAttribute(float _minLimit, float _maxLimit)
        {
            minLimit = _minLimit;
            maxLimit = _maxLimit;
        }
    }

    [CustomFieldDrawer(typeof(FloatRangeAttribute))]
    public class FloatRangeDrawer : FieldDrawer
    {
        public override void OnGUI(GUIContent label)
        {
            FloatRangeAttribute rangeAttribute = Attribute as FloatRangeAttribute;
            Value = EditorGUILayout.Slider(label, (float)Value, rangeAttribute.minLimit, rangeAttribute.maxLimit);
        }
    }
}