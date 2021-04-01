using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Example_12_PerferenceExtension
{
    private static bool loaded = false;
    private static string context = "";
    private const string KEY = "__ToolChainsTrick_Context__";
    
#if UNITY_2019_1_OR_NEWER
    [SettingsProvider]
    private static SettingsProvider ToolChainsTrickSetting()
    {
        var provider = new SettingsProvider("Preferences/ToolChainsTrick", SettingsScope.User)
        {
            guiHandler = (string key) =>
            {
                PreferenceGUI();
            },
            keywords = new string[]{"Tool","Chains","Trick"},
        };
        return provider;
    }
    #else
    [PreferenceItem( "ToolChainsTrick" )]
#endif
    private static void PreferenceGUI()
    {
        if (!loaded)
        {
            Load();
        }
        EditorGUI.BeginChangeCheck();
        context = EditorGUILayout.TextField(new GUIContent("文本配置"),context);
        if (EditorGUI.EndChangeCheck())
        {
            EditorPrefs.SetString(KEY,context);
        }
    }

    private static void Load()
    {
        context = EditorPrefs.GetString(KEY, "");
        loaded = true;
    }
}
