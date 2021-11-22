//---------------------------------------------------------------------------------------
// Author: xuan_chen_work@foxmail.com
// Date: 2021-11-12 15:50:57
// Name: PrefabIconCreator
//---------------------------------------------------------------------------------------

using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Object = System.Object;

namespace Example_06_PrefabIconCreator
{
    public class PrefabPreviewWindow : EditorWindow
    {
        private static string _frontScenePath;
        private PrefabPreviewGUI _preview;
        [MenuItem("Assets/PrefabIconCreator")]
        private static void ShowWindow()
        {
            var select = Selection.activeObject;
            if(!(select is GameObject)) return;
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;
            var window = GetWindow<PrefabPreviewWindow>();
            window.titleContent = new GUIContent("PrefabIconCreator");
            var FrontScene = EditorSceneManager.GetActiveScene();
            _frontScenePath = FrontScene.path;
            EditorSceneManager.OpenScene("Assets/Editor/CX_Examples/Example_06_PrefabIconCreator/PreviePrefabScene.unity", OpenSceneMode.Single);
            window._preview = new PrefabPreviewGUI(select, Camera.main);
            window.rootVisualElement.Add(window._preview.PreviewElement);
            window.maxSize = PrefabPreviewGUI.WindowSize;
            window.minSize = PrefabPreviewGUI.WindowSize;
            window.Show();
        }

        private void OnDestroy()
        {
            EditorSceneManager.OpenScene(_frontScenePath, OpenSceneMode.Single);
            _preview.OnDestory();
        }

        private void OnGUI()
        {
            _preview.RefreshPreview();
        }
        
        private void ShowButton(Rect rect)
        {
            if (UnityEngine.GUI.Button(rect, EditorGUIUtility.IconContent("d_Toolbar Plus More@2x")))
            {
                var path = EditorUtility.OpenFilePanel("Import Json", "Assets", "json");
                if (string.IsNullOrEmpty(path)) return;
                var json = File.ReadAllText(path);
                var settingData = JsonUtility.FromJson(json, typeof(PreviewSettingData));
                if (settingData == null) return;
                _preview.InputSetting((PreviewSettingData) settingData);
            }
        }
    }
}