///Ref:
/// https://github.com/Unity-Technologies/UnityCsReference/blob/e740821767d2290238ea7954457333f06e952bad/Editor/Mono/SceneModeWindows/DefaultLightingExplorerExtension.cs

using UnityEditor;
using UnityEditor.Presets;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

public class DefaultRendererExplorerExtension : IRendererExplorerExtension
{
    public static class Styles
    {
        public static readonly GUIContent Name = EditorGUIUtility.TrTextContent("Name");
        public static readonly GUIContent Enabled = EditorGUIUtility.TrTextContent("Enabled");
        public static readonly GUIContent ShadowCastingMode = EditorGUIUtility.TrTextContent("ShadowCastingMode");
        public static readonly GUIContent StaticShadowCaster = EditorGUIUtility.TrTextContent("StaticCaster");
        public static readonly GUIContent ContributeGI = EditorGUIUtility.TrTextContent("ContributeGI");
        public static readonly GUIContent ReceiveGIType = EditorGUIUtility.TrTextContent("ReceiveGIType");
        public static readonly GUIContent[] receiveGILightmapStrings =
        {
            EditorGUIUtility.TrTextContent("Lightmaps"),
            EditorGUIUtility.TrTextContent("Light Probes")
        };
        public static readonly int[] receiveGILightmapValues = { (int)ReceiveGI.Lightmaps, (int)ReceiveGI.LightProbes };
        public static readonly GUIContent ScaleInLightmap = EditorGUIUtility.TrTextContent("Scale In Lm");
        public static readonly GUIContent StitchLightmapSeams = EditorGUIUtility.TrTextContent("Stitch Seams");
        public static readonly GUIContent LightmapParameters = EditorGUIUtility.TrTextContent("LightmapParameters");
        public static readonly GUIContent LightProbeUsage = EditorGUIUtility.TrTextContent("LightProbeUsage");
        public static readonly GUIContent ProbeAnchor = EditorGUIUtility.TrTextContent("ProbeAnchor");
        public static readonly GUIContent DynamicOccludee = EditorGUIUtility.TrTextContent("DynamicOccludee");
        
        public static readonly GUIContent[] lightProbeUsageStrings =
        {
            EditorGUIUtility.TrTextContent("Off"),
            EditorGUIUtility.TrTextContent("BlendProbes"),
            EditorGUIUtility.TrTextContent("UseProxyVolume"),
            EditorGUIUtility.TrTextContent("CustomProvided"),
        };
        public static readonly int[] lightProbeUsageValues = {0,1,2,4};
    }


    public virtual RendererExplorerTab[] GetContentTabs()
    {
        return new RendererExplorerTab[2]
        {
            new RendererExplorerTab("Mesh Renderer", GetMeshRenderer, GetMeshRendererColumns),
            new RendererExplorerTab("Skinned Mesh Renderer", GetSkinnedMeshRenderer, GetSkinnedRendererColumns),
        };
    }

    protected virtual Object[] GetMeshRenderer()
    {
        return Resources.FindObjectsOfTypeAll<MeshRenderer>();
    }

    protected virtual Object[] GetSkinnedMeshRenderer()
    {
        return Resources.FindObjectsOfTypeAll<SkinnedMeshRenderer>();
    }
    
    protected virtual RendererExplorerTableColumn[] GetMeshRendererColumns()
    {
        return new[]
        {
            new RendererExplorerTableColumn(RendererExplorerTableColumn.DataType.Checkbox,Styles.Enabled,"m_Enabled",50), // 0: Enabled
            new RendererExplorerTableColumn(RendererExplorerTableColumn.DataType.Name, Styles.Name,null,200), // 1:Name
            new RendererExplorerTableColumn(RendererExplorerTableColumn.DataType.Enum, Styles.ShadowCastingMode,"m_CastShadows",150), // 2:ShadowCastingMode
            new RendererExplorerTableColumn(RendererExplorerTableColumn.DataType.Checkbox, Styles.StaticShadowCaster,"m_StaticShadowCaster",50), // 3:m_StaticShadowCaster
            new RendererExplorerTableColumn(RendererExplorerTableColumn.DataType.EnableReceiveGI, Styles.ContributeGI,"m_RayTraceProcedural",50), //4:ContributeGI
            new RendererExplorerTableColumn(RendererExplorerTableColumn.DataType.Int, Styles.ReceiveGIType,"m_ReceiveGI",50,(
                (rect, prop, dependencies) =>
                {
                    var receiveGI = (ReceiveGI)prop.intValue;
                    EditorGUI.BeginChangeCheck();
                    receiveGI = (ReceiveGI)EditorGUI.IntPopup(rect,(int)receiveGI, Styles.receiveGILightmapStrings, Styles.receiveGILightmapValues);
                    if (EditorGUI.EndChangeCheck())
                        prop.intValue = (int)receiveGI;
                })), // 5:ReceiveGIType
             new RendererExplorerTableColumn(RendererExplorerTableColumn.DataType.Checkbox, Styles.DynamicOccludee,"m_DynamicOccludee",50), // 6:m_DynamicOccludee
             new RendererExplorerTableColumn(RendererExplorerTableColumn.DataType.Float, Styles.ScaleInLightmap,"m_ScaleInLightmap",100,(
                 (rect, prop, dep) =>
                 {
                     if (prop != null)
                     {
                         var useLightmaps = dep.Length > 0 && (dep[0].intValue == (int)ReceiveGI.Lightmaps);
                         if (useLightmaps)
                         {
                             EditorGUI.BeginChangeCheck();
                             var scaleInLightmapProp = prop;
                             var newFloat = EditorGUI.FloatField(rect, scaleInLightmapProp.floatValue);
                             if (EditorGUI.EndChangeCheck())
                             {
                                 scaleInLightmapProp.floatValue = Mathf.Clamp(newFloat,0f,65f);
                             }
                         }
                     }
                 }),null,null,new []{5}), // 7:m_ScaleInLightmap
             new RendererExplorerTableColumn(RendererExplorerTableColumn.DataType.Checkbox, Styles.StitchLightmapSeams,"m_StitchLightmapSeams",50,(
                 (rect, prop, dep) =>
                 {
                     if (prop != null)
                     {
                         var useLightmaps = dep.Length > 0 && (dep[0].intValue == (int)ReceiveGI.Lightmaps);
                         if (useLightmaps)
                         {
                             EditorGUI.BeginChangeCheck();
                             var boolValue = EditorGUI.Toggle(rect, prop.boolValue);
                             if (EditorGUI.EndChangeCheck())
                             {
                                 prop.boolValue = boolValue;
                             }
                         }
                     }
                 }),null,null,new []{5}), // 8:m_StitchLightmapSeams
             new RendererExplorerTableColumn(RendererExplorerTableColumn.DataType.Int, Styles.LightProbeUsage,"m_LightProbeUsage",100,(
                (rect, prop, dep) =>
                {
                    var useLightmaps = dep.Length > 0 && (dep[0].intValue == (int)ReceiveGI.Lightmaps);
                    if (!useLightmaps)
                    {
                        var lightProbeUsage = (LightProbeUsage)prop.intValue;
                        EditorGUI.BeginChangeCheck();
                        lightProbeUsage = (LightProbeUsage)EditorGUI.IntPopup(rect,(int)lightProbeUsage, Styles.lightProbeUsageStrings, Styles.lightProbeUsageValues);
                        if (EditorGUI.EndChangeCheck())
                            prop.intValue = (int)lightProbeUsage;
                    }
                }),null,null,new []{5}), // 9:m_LightProbeUsage
            new RendererExplorerTableColumn(RendererExplorerTableColumn.DataType.Custom, Styles.ProbeAnchor,"m_ProbeAnchor",150,(
                (rect, prop, dep) =>
                {
                    var useLightmaps = dep.Length > 0 && (dep[0].intValue == (int)ReceiveGI.Lightmaps);
                    if (!useLightmaps)
                    {
                        EditorGUI.PropertyField(rect, prop, GUIContent.none);
                    }
                }),null,null,new []{5}), // 10:m_ProbeAnchor
        };
    }
    protected virtual RendererExplorerTableColumn[] GetSkinnedRendererColumns()
    {
        return new[]
        {
            new RendererExplorerTableColumn(RendererExplorerTableColumn.DataType.Checkbox,Styles.Enabled,"m_Enabled",50), // 0: Enabled
            new RendererExplorerTableColumn(RendererExplorerTableColumn.DataType.Name, Styles.Name,null,200), // 1:Name
            new RendererExplorerTableColumn(RendererExplorerTableColumn.DataType.Enum, Styles.ShadowCastingMode,"m_CastShadows",150), // 2:ShadowCastingMode
            new RendererExplorerTableColumn(RendererExplorerTableColumn.DataType.Checkbox, Styles.StaticShadowCaster,"m_StaticShadowCaster",50), // 3:m_StaticShadowCaster
            new RendererExplorerTableColumn(RendererExplorerTableColumn.DataType.Checkbox, Styles.DynamicOccludee,"m_DynamicOccludee",50), // 4:m_DynamicOccludee
            new RendererExplorerTableColumn(RendererExplorerTableColumn.DataType.Int, Styles.LightProbeUsage,"m_LightProbeUsage",100,(
                (rect, prop, dep) =>
                {
                    var lightProbeUsage = (LightProbeUsage)prop.intValue;
                    EditorGUI.BeginChangeCheck();
                    lightProbeUsage = (LightProbeUsage)EditorGUI.IntPopup(rect,(int)lightProbeUsage, Styles.lightProbeUsageStrings, Styles.lightProbeUsageValues);
                    if (EditorGUI.EndChangeCheck())
                        prop.intValue = (int)lightProbeUsage;
                })), // 5:m_LightProbeUsage
            new RendererExplorerTableColumn(RendererExplorerTableColumn.DataType.Custom, Styles.ProbeAnchor,"m_ProbeAnchor",150,(
                (rect, prop, dep) =>
                {
                    var useLightmaps = dep.Length > 0 && (dep[0].intValue != (int)LightProbeUsage.Off);
                    if (!useLightmaps)
                    {
                        EditorGUI.PropertyField(rect, prop, GUIContent.none);
                    }
                }),null,null,new []{5}), // 6:m_ProbeAnchor
        };
    }

    public virtual void OnEnable()
    {
    }

    public virtual void OnDisable()
    {
    }
}