///Ref:
/// https://github.com/Unity-Technologies/UnityCsReference/blob/e740821767d2290238ea7954457333f06e952bad/Editor/Mono/SceneModeWindows/LightingExplorerWindow.cs
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public interface IRendererExplorerExtension
{
    RendererExplorerTab[] GetContentTabs();
    void OnEnable();
    void OnDisable();
}

[EditorWindowTitle(title = "Renderer Explorer",icon = "Lighting")]
public sealed class RendererExplorerWindow : EditorWindow
{
    private RendererExplorerTab[] m_TableTabs;
    private GUIContent[] m_TabTitles;
    private int m_SelectedTab = 0;
    Type m_CurrentSRPType = null;
    IRendererExplorerExtension m_CurrentExplorerExtension = null;
    static IRendererExplorerExtension s_DefaulExplorerExtension = null;

    [MenuItem("Window/Rendering/Renderer Explorer", false, 2)]
    static void CreateRendererExplorerWindow()
    {
        RendererExplorerWindow window = EditorWindow.GetWindow<RendererExplorerWindow>();
        window.minSize = new Vector2(500, 250);
        window.Show();
    }
    void OnEnable()
    {
        titleContent = new GUIContent();
        titleContent.text = "Renderer Explorer";
        UpdateTabs();
        EditorApplication.searchChanged += Repaint;
        Repaint();
    }

    void OnDisable()
    {
        if (m_TableTabs != null)
        {
            for (int i = 0; i < m_TableTabs.Length; i++)
            {
                m_TableTabs[i].OnDisable();
            }
        }

        if (m_CurrentExplorerExtension != null)
        {
            m_CurrentExplorerExtension.OnDisable();
        }

        EditorApplication.searchChanged -= Repaint;
    }

    void OnInspectorUpdate()
    {
        if (m_TableTabs != null && (int)m_SelectedTab >= 0 && (int)m_SelectedTab < m_TableTabs.Length)
        {
            m_TableTabs[(int)m_SelectedTab].OnInspectorUpdate();
        }
    }

    void OnSelectionChange()
    {
        if (m_TableTabs != null)
        {
            for (int i = 0; i < m_TableTabs.Length; i++)
            {
                if (i == (m_TableTabs.Length - 1)) // last tab containing materials
                {
                    int[] selectedIds = UnityEngine.Object.FindObjectsOfType<MeshRenderer>().Where((MeshRenderer mr) => {
                        return Selection.instanceIDs.Contains(mr.gameObject.GetInstanceID());
                    }).SelectMany(meshRenderer => meshRenderer.sharedMaterials).Where((Material m) => {
                        return m != null && (m.globalIlluminationFlags & MaterialGlobalIlluminationFlags.AnyEmissive) != 0;
                    }).Select(m => m.GetInstanceID()).Union(Selection.instanceIDs).Distinct().ToArray();

                    m_TableTabs[i].OnSelectionChange(selectedIds);
                }
                else
                    m_TableTabs[i].OnSelectionChange();
            }
        }

        Repaint();
    }

    void OnHierarchyChange()
    {
        if (m_TableTabs != null)
        {
            for (int i = 0; i < m_TableTabs.Length; i++)
            {
                m_TableTabs[i].OnHierarchyChange();
            }
        }
    }

    void OnGUI()
    {
        UpdateTabs();

        EditorGUIUtility.labelWidth = 130;

        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();

        GUILayout.FlexibleSpace();
        if (m_TabTitles != null)
            m_SelectedTab = GUILayout.Toolbar(m_SelectedTab, m_TabTitles, "LargeButton",GUI.ToolbarButtonSize.FitToContents);
        GUILayout.FlexibleSpace();

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        if (m_TableTabs != null && (int)m_SelectedTab >= 0 && (int)m_SelectedTab < m_TableTabs.Length)
            m_TableTabs[(int)m_SelectedTab].OnGUI();
        EditorGUILayout.Space();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();
    }
    private System.Type GetSRPType()
    {
        System.Type SRPType = null;
        if (GraphicsSettings.currentRenderPipeline != null)
        {
            SRPType = GraphicsSettings.currentRenderPipeline.GetType();
        }
        return SRPType;
    }
    
    private void UpdateTabs()
    {
        var SRPType = GetSRPType();
        
        if (m_CurrentExplorerExtension == null || m_CurrentSRPType != SRPType)
        {
            m_CurrentSRPType = SRPType;
        
            if (m_CurrentExplorerExtension != null)
            {
                m_CurrentExplorerExtension.OnDisable();
            }
        
            m_CurrentExplorerExtension = GetExplorerExtension(SRPType);
            m_CurrentExplorerExtension.OnEnable();
        
            m_SelectedTab = EditorSettings.defaultBehaviorMode == EditorBehaviorMode.Mode2D ? /* 2D Lights */ 1 : /* Lights */ 0;
        
            if (m_CurrentExplorerExtension.GetContentTabs() == null || m_CurrentExplorerExtension.GetContentTabs().Length == 0)
            {
                throw new ArgumentException("There must be atleast 1 content tab defined for the Renderer Explorer.");
            }
            
            m_TableTabs =  m_CurrentExplorerExtension.GetContentTabs();
            m_TabTitles = m_TableTabs != null ? m_TableTabs.Select(item => item.title).ToArray() : null;
        }
    }
    private IRendererExplorerExtension GetDefaultExplorerExtension()
    {
        if (s_DefaulExplorerExtension == null)
        {
            s_DefaulExplorerExtension = new DefaultRendererExplorerExtension();
        }
        return s_DefaulExplorerExtension;
    }
    //为了适配SRP,暂无用.
    private IRendererExplorerExtension GetExplorerExtension(System.Type currentSRPType)
    {
        if (currentSRPType == null)
            return GetDefaultExplorerExtension();
        return GetDefaultExplorerExtension();
    }
}