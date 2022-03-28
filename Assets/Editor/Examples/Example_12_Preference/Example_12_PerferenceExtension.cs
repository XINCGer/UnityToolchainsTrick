using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ToolKits
{
    public class Example_12_PerferenceExtension
    {
        private static bool loaded = false;
        private static string context = "";
        private static bool sceneviewSwitch;
        private static bool duplicateEventListen = false;
        private const string KEY = "__ToolChainsTrick_Context__";
        private static bool globalInputEventEnable = false;

#if UNITY_2019_1_OR_NEWER
        [SettingsProvider]
        private static SettingsProvider ToolChainsTrickSetting()
        {
            var provider = new SettingsProvider("Preferences/ToolChainsTrick", SettingsScope.User)
            {
                guiHandler = (string key) => { PreferenceGUI(); },
                keywords = new string[] {"Tool", "Chains", "Trick"},
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
            context = EditorGUILayout.TextField(new GUIContent("文本配置"), context);
            if (EditorGUI.EndChangeCheck())
            {
                EditorPrefs.SetString(KEY, context);
            }

            EditorGUI.BeginChangeCheck();
            sceneviewSwitch = EditorGUILayout.Toggle("SceneView拓展开关", sceneviewSwitch);
            if (EditorGUI.EndChangeCheck())
            {
                EditorPrefs.SetBool(Constants.SCENE_VIEW_EXTENSITON_SWITH, sceneviewSwitch);
                SceneViewExtension.EditorInitialize();
            }
            
            EditorGUI.BeginChangeCheck();
            globalInputEventEnable = EditorGUILayout.Toggle("是否开启全局检测事件", globalInputEventEnable);
            if (EditorGUI.EndChangeCheck())
            {
                EditorPrefs.SetBool(Constants.GLOBAL_INPUT_ENEVT_ENABLE,globalInputEventEnable);
                GlobalInputEvent.EditorInitialize();
            }
            
            EditorGUI.BeginChangeCheck();
            duplicateEventListen = EditorGUILayout.Toggle("是否监听Hierarchy的Ctrl+D事件", duplicateEventListen);
            if (EditorGUI.EndChangeCheck())
            {
                EditorPrefs.SetBool(Constants.DuplicateEventListen,duplicateEventListen);
                Example_71_UnityDuplicateEvent.EditorInitialize();
            }
        }

        private static void Load()
        {
            context = EditorPrefs.GetString(KEY, "");
            sceneviewSwitch = EditorPrefs.GetBool(Constants.SCENE_VIEW_EXTENSITON_SWITH, false);
            loaded = true;
            duplicateEventListen = EditorPrefs.GetBool(Constants.DuplicateEventListen, false);
        }
    }
}