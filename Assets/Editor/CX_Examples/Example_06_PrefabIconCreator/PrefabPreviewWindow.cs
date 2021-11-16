//---------------------------------------------------------------------------------------
// Author: xuan_chen_work@foxmail.com
// Date: 2021-11-12 15:50:57
// Name: PrefabIconCreator
//---------------------------------------------------------------------------------------

using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Object = System.Object;

namespace Example_06_PrefabIconCreator
{
    public class PrefabPreviewWindow : EditorWindow
    {
        private static string _frontScenePath;
        private GameObject _obj;
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
            window._obj = Instantiate<GameObject>(select as GameObject, Vector3.zero, Quaternion.identity);
            window._preview = new PrefabPreviewGUI(window._obj, Camera.main);
            window.rootVisualElement.Add(window._preview.PreviewElement);
            window.maxSize = PrefabPreviewGUI.WindowSize;
            window.minSize = PrefabPreviewGUI.WindowSize;
            window.Show();
        }

        private void OnDestroy()
        {
            EditorSceneManager.OpenScene(_frontScenePath, OpenSceneMode.Single);
            if (_obj != null)
                DestroyImmediate(_obj);
            _preview.OnDestory();
        }

        private void OnGUI()
        {
            _preview.RefreshPreview();
        }
    }
}