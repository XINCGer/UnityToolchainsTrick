using CZToolKit.Core.Editors;
using UnityEditor;
using UnityEngine;

namespace CZToolKit.Examples
{

    public class LocalizationInEditorModeExample : EditorWindow
    {
        [MenuItem("Tools/±‡º≠∆˜∂‡”Ô—‘")]
        public static void Open()
        {
            GetWindow<LocalizationInEditorModeExample>();
        }

        Localization lo;

        private void OnEnable()
        {
            lo = new Localization(Resources.Load<TextAsset>("language").text);
        }

        void OnGUI()
        {
            lo.Language = EditorGUILayout.Popup(lo.Language, lo.Languages);

            GUILayout.Button(lo.GetGUIContent("btn1"));
            GUILayout.Button(lo.GetGUIContent("btn2"));
            GUILayout.Button(lo.GetGUIContent("btn3"));
        }
    }
}
