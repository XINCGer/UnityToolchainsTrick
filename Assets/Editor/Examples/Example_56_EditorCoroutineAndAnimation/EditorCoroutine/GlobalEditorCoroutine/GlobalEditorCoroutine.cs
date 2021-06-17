using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CZToolKit.Core.Editors
{
    public class GlobalEditorCoroutineSetting
    {
        public bool open = false;
    }

    [InitializeOnLoad]
    public class GlobalEditorCoroutine
    {
        #region Perference
        static string Name = nameof(GlobalEditorCoroutine);
        static string key = "GlobalEditorCoroutine.Settings";

#if UNITY_2019_1_OR_NEWER
        [SettingsProvider]
        public static SettingsProvider CreateSettingsProvider()
        {
            SettingsProvider provider = new SettingsProvider("Preferences/" + Name, SettingsScope.User)
            {
                guiHandler = (searchContext) => { PreferencesGUI(); },
            };
            return provider;
        }
#endif

#if !UNITY_2019_1_OR_NEWER
        [PreferenceItem(Name)]
#endif
        private static void PreferencesGUI()
        {
            EditorGUI.BeginChangeCheck();
            Settings.open = EditorGUILayout.Toggle("Open", Settings.open);
            if (EditorGUI.EndChangeCheck())
            {
                EditorPrefs.SetString(key, JsonUtility.ToJson(Settings));
                UpdateStatus(); 

            }
        }
        #endregion

        static GlobalEditorCoroutineSetting settings;

        static GlobalEditorCoroutineSetting Settings
        {
            get
            {
                if (settings == null)
                {
                    if (EditorPrefs.HasKey(key))
                        settings = JsonUtility.FromJson<GlobalEditorCoroutineSetting>(EditorPrefs.GetString(key));
                    else
                        settings = new GlobalEditorCoroutineSetting();
                }
                return settings;
            }
        }

        static void UpdateStatus()
        {
            if (Settings.open)
                EditorApplication.update += Update;
            else
                EditorApplication.update -= Update;
        }

        
        static GlobalEditorCoroutine()
        {
            UpdateStatus();
        }

        static Stack<EditorCoroutine> coroutineStack = new Stack<EditorCoroutine>();

        static void Update()
        {
            int count = coroutineStack.Count;
            while (count-- > 0)
            {
                EditorCoroutine coroutine = coroutineStack.Pop();
                if (!coroutine.IsRunning) continue;
                ICondition condition = coroutine.Current as ICondition;
                if (condition == null || condition.Result(coroutine))
                {
                    if (!coroutine.MoveNext())
                        continue;
                }
                coroutineStack.Push(coroutine);
            }
        }

        public static EditorCoroutine StartCoroutine(IEnumerator _coroutine)
        {

            EditorCoroutine coroutine = new EditorCoroutine(_coroutine);
            coroutineStack.Push(coroutine);
            return coroutine;
        }

        public static void StopCoroutine(EditorCoroutine _coroutine)
        {
            _coroutine.Stop();
        }
    }
}
