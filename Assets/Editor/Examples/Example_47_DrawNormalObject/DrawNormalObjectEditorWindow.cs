using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
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
        EditorGUILayoutExtension.DrawFields(data);
	}
}

public class TestData
{
    public int i;
    public float f;
    public string s;
    public long l;
    public Vector3 v3;
    public Vector4 v4;
    public GameObject gameObject;
    public StructData s1;
    public TestDataB internalB;
    public List<TestDataB> bs;
}

public class TestDataB
{
    public int i;
    public float f;
    public string s;
    public long l;
    public Vector3 v3;
    public Vector4 v4;
    public GameObject gameObject;
    public StructData s1;

}

public struct StructData
{
    public int i;
}
