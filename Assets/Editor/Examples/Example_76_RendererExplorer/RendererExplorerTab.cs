///Ref:
/// https://github.com/Unity-Technologies/UnityCsReference/blob/e740821767d2290238ea7954457333f06e952bad/Editor/Mono/SceneModeWindows/LightingExplorerTab.cs
using System;
using UnityEditor;
using UnityEngine;
using System.Linq;
using SerializedPropertyTable = UnityEditorEx.SerializedPropertyTable;
using SerializedPropertyTreeView = UnityEditorEx.SerializedPropertyTreeView;
using SerializedPropertyFilters = UnityEditorEx.SerializedPropertyFilters;
using SerializedPropertyDataStore = UnityEditorEx.SerializedPropertyDataStore;

public sealed class RendererExplorerTab
{
    SerializedPropertyTable m_RendererTable;
    GUIContent m_Title;
    private static float s_indent = (float)EditorGUI.indentLevel * 15f;
    internal GUIContent title
    {
        get { return m_Title; }
    }

    public RendererExplorerTab(
        string title,
        Func<UnityEngine.Object[]> objects,
        Func<RendererExplorerTableColumn[]> columns,
        bool showFilterGUI = true)
    {
        if (objects() == null)
            throw new ArgumentException("Objects are not allowed to be null", "objects");

        if (columns() == null)
            throw new ArgumentException("Columns are not allowed to be null", "columns");

        m_RendererTable = new SerializedPropertyTable(title.Replace(" ", string.Empty),
            new SerializedPropertyDataStore.GatherDelegate(objects),
            () => { return columns().Select(item => item.internalColumn).ToArray(); }, showFilterGUI);
        m_Title = EditorGUIUtility.TrTextContent(title);
    }

    internal void OnDisable()
    {
        if(m_RendererTable != null)
            m_RendererTable.OnDisable();
    }

    public void OnInspectorUpdate()
    {
        if(m_RendererTable != null)
            m_RendererTable.OnInspectorUpdate();
    }

    public void OnSelectionChange(int[] selectedIds)
    {
        if(m_RendererTable != null)
            m_RendererTable.OnSelectionChange(selectedIds);
    }

    public void OnSelectionChange()
    {
        if(m_RendererTable != null)
            m_RendererTable.OnSelectionChange();
    }

    public void OnHierarchyChange()
    {
        if(m_RendererTable != null)
            m_RendererTable.OnHierarchyChange();
    }

    public void OnGUI()
    {
        EditorGUI.indentLevel += 1;

        int cur_indent = EditorGUI.indentLevel;
        float cur_indent_px = s_indent;
        EditorGUI.indentLevel = 0;

        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(cur_indent_px);

        using (new EditorGUILayout.VerticalScope())
        {
            if (m_RendererTable != null)
                m_RendererTable.OnGUI();
        }

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();
        EditorGUI.indentLevel = cur_indent;

        EditorGUI.indentLevel -= 1;
    }
}

public sealed class RendererExplorerTableColumn
{
    public enum DataType
    {
        Name = 0,
        Checkbox = 1,
        Enum = 2,
        Int = 3,
        Float = 4,
        EnableReceiveGI = 5,
        // ..
        Custom = 20
    }

    internal SerializedPropertyTreeView.Column internalColumn
    {
        get { return m_Column; }
    }

    SerializedPropertyTreeView.Column m_Column;
    public delegate void OnGUIDelegate(Rect r, SerializedProperty prop, SerializedProperty[] dependencies);

    public delegate int ComparePropertiesDelegate(SerializedProperty lhs, SerializedProperty rhs);

    public delegate void CopyPropertiesDelegate(SerializedProperty target, SerializedProperty source);
    public RendererExplorerTableColumn(DataType type, GUIContent headerContent, string propertyName = null,
        int width = 100, OnGUIDelegate onGUIDelegate = null, ComparePropertiesDelegate compareDelegate = null,
        CopyPropertiesDelegate copyDelegate = null, int[] dependencyIndices = null)
    {
        m_Column = new SerializedPropertyTreeView.Column();
        m_Column.headerContent = headerContent;
        m_Column.width = width;
        m_Column.minWidth = width / 2;
        m_Column.propertyName = propertyName;
        m_Column.dependencyIndices = dependencyIndices;
        
        m_Column.sortedAscending = true;
        m_Column.sortingArrowAlignment = TextAlignment.Center;
        m_Column.autoResize = false;
        m_Column.allowToggleVisibility = true;
        m_Column.headerTextAlignment = type == DataType.Checkbox ? TextAlignment.Center : TextAlignment.Left;
        
        switch (type)
        {
            case DataType.Name:
                m_Column.compareDelegate = SerializedPropertyTreeView.DefaultDelegates.CompareName;
                m_Column.drawDelegate = SerializedPropertyTreeView.DefaultDelegates.DrawName;
                m_Column.filter = new SerializedPropertyFilters.Name();
                break;
        
            case DataType.Checkbox:
                m_Column.compareDelegate = SerializedPropertyTreeView.DefaultDelegates.CompareCheckbox;
                m_Column.drawDelegate = SerializedPropertyTreeView.DefaultDelegates.DrawCheckbox;
                break;
        
            case DataType.Enum:
                m_Column.compareDelegate = SerializedPropertyTreeView.DefaultDelegates.CompareEnum;
                m_Column.drawDelegate = SerializedPropertyTreeView.DefaultDelegates.DrawDefault;
                break;
        
            case DataType.Int:
                m_Column.compareDelegate = SerializedPropertyTreeView.DefaultDelegates.CompareInt;
                m_Column.drawDelegate = SerializedPropertyTreeView.DefaultDelegates.DrawDefault;
                break;
        
            case DataType.Float:
                m_Column.compareDelegate = SerializedPropertyTreeView.DefaultDelegates.CompareFloat;
                m_Column.drawDelegate = SerializedPropertyTreeView.DefaultDelegates.DrawDefault;
                break;
        
            case DataType.EnableReceiveGI:
                m_Column.compareDelegate = (lhs, rhs) =>
                {
                    var goSerializedObject_lhs = new SerializedObject((lhs.serializedObject.targetObject as MeshRenderer).gameObject); 
                    var staticEditorFlags_lhs = goSerializedObject_lhs.FindProperty("m_StaticEditorFlags");
                    bool contributeGI_lhs = (staticEditorFlags_lhs.intValue & (int)StaticEditorFlags.ContributeGI) != 0;
                    
                    var goSerializedObject_rhs = new SerializedObject((rhs.serializedObject.targetObject as MeshRenderer).gameObject); 
                    var staticEditorFlags_rhs = goSerializedObject_rhs.FindProperty("m_StaticEditorFlags");
                    bool contributeGI_rhs = (staticEditorFlags_rhs.intValue & (int)StaticEditorFlags.ContributeGI) != 0;
                    
                    return contributeGI_lhs.CompareTo(contributeGI_rhs);
                };
                m_Column.drawDelegate = (rect, prop, dependencies) =>
                {
                    var goSerializedObject = new SerializedObject((prop.serializedObject.targetObject as MeshRenderer).gameObject); 
                    var staticEditorFlags = goSerializedObject.FindProperty("m_StaticEditorFlags");
                    bool contributeGI = (staticEditorFlags.intValue & (int)StaticEditorFlags.ContributeGI) != 0;
                    EditorGUI.BeginChangeCheck();
                    float off = System.Math.Max(0.0f, ((rect.width / 2) - 8));
                    rect.x += off;
                    rect.width -= off;
                    contributeGI = EditorGUI.Toggle(rect, contributeGI);
                    if (EditorGUI.EndChangeCheck())
                    {
                        SceneModeUtility.SetStaticFlags(goSerializedObject.targetObjects, (int)StaticEditorFlags.ContributeGI, contributeGI);
                        goSerializedObject.SetIsDifferentCacheDirty();
                        goSerializedObject.Update();    
                    }
                };
                break;
        
            default:
                break;
        }
        
        if (onGUIDelegate != null)
        {
            // when allowing the user to draw checkboxes, we will make sure that the rect is in the center
            if (type == DataType.Checkbox)
            {
                m_Column.drawDelegate = (r, prop, dep) =>
                {
                    float off = System.Math.Max(0.0f, ((r.width / 2) - 8));
                    r.x += off;
                    r.width -= off;
                    onGUIDelegate(r, prop, dep);
                };
            }
            else
                m_Column.drawDelegate = new SerializedPropertyTreeView.Column.DrawEntry(onGUIDelegate);
        }
        
        if (compareDelegate != null)
            m_Column.compareDelegate = new SerializedPropertyTreeView.Column.CompareEntry(compareDelegate);
        
        if (copyDelegate != null)
            m_Column.copyDelegate = new SerializedPropertyTreeView.Column.CopyDelegate(copyDelegate);
    }
}