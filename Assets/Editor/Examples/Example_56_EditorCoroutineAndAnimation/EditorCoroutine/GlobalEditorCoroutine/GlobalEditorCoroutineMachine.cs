using System.Collections;
using UnityEditor;
using UnityEngine;

namespace CZToolKit.Core.Editors
{
    public class GlobalEditorCoroutineSetting
    {
        public bool open = false;
    }

    [InitializeOnLoad]
    public class GlobalEditorCoroutineMachine
    {
        #region Perference
        static string Name = nameof(GlobalEditorCoroutineMachine);
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

        static CoroutineMachineController CoroutineMachine = new CoroutineMachineController();

        static GlobalEditorCoroutineMachine()
        {
            UpdateStatus();
        }

        static void Update()
        {
            CoroutineMachine.Update();
        }

        public static EditorCoroutine StartCoroutine(IEnumerator _coroutine)
        {
            return CoroutineMachine.StartCoroutine(_coroutine);
        }

        public static void StopCoroutine(EditorCoroutine _coroutine)
        {
            CoroutineMachine.StopCoroutine(_coroutine);
        }
    }
}
