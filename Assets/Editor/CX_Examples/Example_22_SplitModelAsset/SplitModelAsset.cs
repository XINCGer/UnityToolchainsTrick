using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CX_Example_22
{
    public class SplitModelAsset : EditorWindow
    {
        [MenuItem("CX_Tools/SplitModelAsset")]
        private static void ShowWindow()
        {
            var window = GetWindow<SplitModelAsset>();
            window.titleContent = new GUIContent("SplitModelAsset");
            window.Show();
        }

        private Object _target;

        private const string _exportFolder = "Assets/Editor/CX_Examples/Example_22_SplitModelAsset/SplitAssets";
        private const string _exportMeshFolder = _exportFolder + "/Meshs";
        private const string _exportMaterialFolder = _exportFolder + "/Materials";
        private const string _exportAvatarFolder = _exportFolder + "/Avatars";

        private void OnEnable()
        {
            if (!Directory.Exists(_exportFolder))
            {
                Directory.CreateDirectory(_exportFolder);
            }

            if (!Directory.Exists(_exportMeshFolder))
            {
                Directory.CreateDirectory(_exportMeshFolder);
            }
            
            if (!Directory.Exists(_exportMaterialFolder))
            {
                Directory.CreateDirectory(_exportMaterialFolder);
            }
            
            if (!Directory.Exists(_exportAvatarFolder))
            {
                Directory.CreateDirectory(_exportAvatarFolder);
            }
            AssetDatabase.Refresh();
        }

        private void OnGUI()
        {
            _target = EditorGUILayout.ObjectField(_target, typeof(Object), false);
            if (GUILayout.Button("Split!!"))
            {
                SplitModel(_target);
            }
        }

        private void SplitModel(Object target)
        {
            var path = AssetDatabase.GetAssetPath(target);
            if (string.IsNullOrEmpty(path)) return;
            var imp = AssetImporter.GetAtPath(path) as ModelImporter;
            if (imp == null)
            {
                Debug.LogError("不是Model Asset！！");
                return;
            }

            var splitAsset = new SplitAsset
            {
                originObj = target,
                assets = new List<Object>()
            };
            var allObjs = AssetDatabase.LoadAllAssetsAtPath(path);
            foreach (var obj in allObjs)
            {
                switch (obj)
                {
                    case Mesh mesh:
                        var name = mesh.name;
                        mesh = Instantiate(mesh);
                        mesh.name = name;
                        AssetDatabase.CreateAsset(mesh, _exportMeshFolder + $"/{mesh.name}.asset");
                        splitAsset.assets.Add(mesh);
                        break;
                    case Material mat:
                        AssetDatabase.ExtractAsset(mat, _exportMaterialFolder + $"/{mat.name}.mat");
                        splitAsset.assets.Add(mat);
                        break;
                    case Avatar avatar:
                        var avatarName = avatar.name;
                        avatar = Instantiate(avatar);
                        avatar.name = avatarName;
                        AssetDatabase.CreateAsset(avatar, _exportAvatarFolder + $"/{avatar.name}.avatar");
                        splitAsset.assets.Add(avatar);
                        break;
                }
            }
            AssetDatabase.Refresh();
        }
        
    }
}


