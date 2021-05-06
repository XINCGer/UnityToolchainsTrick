using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using CZToolKit.Core;
using CZToolKit.Core.Editors;

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
        CZToolKit.Core.Editors.EditorGUILayoutExtension.DrawFields(data);
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

[CustomObjectDrawer(typeof(FloatRangeAttribute))]
public class FloatRangeDrawer : ObjectDrawer
{
    public override void OnGUI(GUIContent label)
    {
        FloatRangeAttribute rangeAttribute = attribute as FloatRangeAttribute;
        value = EditorGUILayout.Slider(label, (float)value, rangeAttribute.minLimit, rangeAttribute.maxLimit);
    }
}