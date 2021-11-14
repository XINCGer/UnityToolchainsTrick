//---------------------------------------------------------------------------------------
// Author: xuan_chen_work@foxmail.com
// Date: 2021-11-12 15:50:57
// Name: PrefabIconCreator
//---------------------------------------------------------------------------------------

using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;

namespace Example_03_PrefabIconCreator
{
    public class PrefabIconCreator : EditorWindow
    {
        private static string FrontScenePath;
        private static readonly Vector2 WindowSize = new Vector2 {x = 420, y = 500};


        [MenuItem("CX_Tools/PrefabIconCreator", priority = 2)]
        private static void ShowWindow()
        {
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;
            var window = GetWindow<PrefabIconCreator>();
            window.titleContent = new GUIContent("PrefabIconCreator");
            window.maxSize = WindowSize;
            window.minSize = WindowSize;
            window.SetBgImage();
            var FrontScene = EditorSceneManager.GetActiveScene();
            FrontScenePath = FrontScene.path;
            var preScene = EditorSceneManager.OpenScene(
                "Assets/Editor/CX_Examples/Example_03_PrefabIconCreator/PreviePrefabScene.unity", OpenSceneMode.Single);
            window.Show();
        }

        private void OnDestroy()
        {
            EditorSceneManager.OpenScene(FrontScenePath, OpenSceneMode.Single);
        }

        private void OnGUI()
        {
            
        }
        

        private void SetBgImage()
        {
            var bgImage = new Image();
            var texBg = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Editor/CX_Examples/Example_03_PrefabIconCreator/rennka.png");
            bgImage.image = texBg;
            var bgCol = Color.white;
            bgCol.a = 0.1f;
            bgImage.tintColor = bgCol;
            bgImage.style.width = WindowSize.x;
            bgImage.style.height = WindowSize.y;
            this.rootVisualElement.Add(bgImage);
            var imguiContainer = new IMGUIContainer();
            bgImage.Add(imguiContainer);
            imguiContainer.onGUIHandler += () =>
            {
                GUILayout.Space(20);
                GUILayout.Label("aaaaa");
                if (GUILayout.Button("aaa"))
                {
                    Debug.Log("aaa");
                }
            };
        }
    }
}