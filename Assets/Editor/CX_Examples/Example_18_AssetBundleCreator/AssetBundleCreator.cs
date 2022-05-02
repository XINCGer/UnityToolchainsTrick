using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CX_Example_18
{
    public enum PackType
    {
        Include,
        Independent,
        Unpack,
    }
    
    public class DependItem
    {
        public string ItemPath;
        public string ItemName;
        public PackType PackType;
    }

    public class BundleInfo
    {
        public string Name;
        public string Path;
        public float Size;
    }

    public class AssetBundleCreator : EditorWindow
    {
        [MenuItem("CX_Tools/AssetBundleCreator")]
        private static void ShowWindow()
        {
            var window = GetWindow<AssetBundleCreator>();
            window.titleContent = new GUIContent("AssetBundleCreator");
            window.Show();
        }
        
        private Object _originObj;

        private void OnGUI()
        {
            using (new GUILayout.VerticalScope("Box"))
            {
                _originObj = EditorGUILayout.ObjectField(new GUIContent("Origin Object:"), _originObj, typeof(Object), false);
            }
            ShowDependencies();

            ShowAssetBundleInfo();
            
            using (new GUILayout.VerticalScope("Box"))
            {
                if (GUILayout.Button("Collect Dependencies!") && _originObj != null)
                {
                    CollectDependencies();
                }

                if (GUILayout.Button("Create AssetBundle!") && _originObj != null)
                {
                    CreateAssetBundle();
                }

                if (GUILayout.Button("Clear StreamingAsset"))
                {
                    if (Directory.Exists(Application.streamingAssetsPath))
                    {
                        AssetDatabase.DeleteAsset("Assets/StreamingAssets");
                        AssetDatabase.Refresh();
                    }
                }
            }
        }

        private List<DependItem> _dependItems;

        private void ShowDependencies()
        {
            if(_dependItems == null) return;
            using (new GUILayout.VerticalScope("Box"))
            {
                GUILayout.Label("Dependencies:");
                foreach (var item in _dependItems)
                {
                    using (new GUILayout.HorizontalScope("Box"))
                    {
                        GUILayout.Label(item.ItemName);
                        item.PackType = (PackType)EditorGUILayout.EnumPopup(item.PackType, GUILayout.Width(100f));
                    }
                }
            }
        }

        private void CollectDependencies()
        {
            _dependItems = new List<DependItem>();
            var path = AssetDatabase.GetAssetPath(_originObj);
            if (string.IsNullOrEmpty(path))
            {
                Debug.LogError("Not found Path!");
                return;
            }

            var dep = AssetDatabase.GetDependencies(path, true);
            for (int i = 0; i < dep.Length; i++)
            {
                if(dep[i] == path) continue;
                var item = new DependItem
                {
                    PackType = PackType.Include,
                    ItemPath = dep[i],
                    ItemName = Path.GetFileName(dep[i])
                };
                _dependItems.Add(item);
            }
            Repaint();
        }
        
        private void CreateAssetBundle()
        {
            var path = AssetDatabase.GetAssetPath(_originObj);
            var abbList = new Dictionary<string, AssetBundleBuild>();
            var mainABPaths = new List<string>{path};
            foreach (var item in _dependItems)
            {
                switch (item.PackType)
                {
                    case PackType.Include:
                        mainABPaths.Add(item.ItemPath);
                        break;
                    case PackType.Independent:
                        var newAB = new AssetBundleBuild
                        {
                            assetBundleName = GetAssetBundleName(item.ItemPath),
                            assetNames = new []{item.ItemPath}
                        };
                        abbList[item.ItemPath] = newAB;
                        break;
                    case PackType.Unpack:
                        break;
                }
            }
            
            var mainAB = new AssetBundleBuild
            {
                assetBundleName = GetAssetBundleName(path),
            };
            mainAB.assetNames = mainABPaths.ToArray();
            abbList[path] = mainAB;
            
            if (!Directory.Exists(Application.streamingAssetsPath))
            {
                Directory.CreateDirectory(Application.streamingAssetsPath);
            }
            
            var assetBundleManifest = BuildPipeline.BuildAssetBundles($"{Application.streamingAssetsPath}",
                abbList.Values.ToArray(), BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
            AssetDatabase.Refresh();
            _BundleInfos = new List<BundleInfo>();
            var allBundles = assetBundleManifest.GetAllAssetBundles();
            foreach (var bundle in allBundles)
            {
                var bundlePath = $"{Application.streamingAssetsPath}/{bundle}";
                var fileInfo = new FileInfo(bundlePath);
                var bundleInfo = new BundleInfo
                {
                    Path = bundlePath,
                    Name = fileInfo.Name,
                    Size = fileInfo.Length / 1000f
                };
                _BundleInfos.Add(bundleInfo);
            }
        }

        private List<BundleInfo> _BundleInfos;
        private void ShowAssetBundleInfo()
        {
            if(_BundleInfos == null) return;
            using (new GUILayout.VerticalScope("Box"))
            {
                foreach (var bundleInfo in _BundleInfos)
                {
                    using (new GUILayout.HorizontalScope("Box"))
                    {
                        GUILayout.Label($"{bundleInfo.Name} {bundleInfo.Size}KB");
                    }
                } 
            }
        }

        private string GetAssetBundleName(string path)
        {
            var name = Path.GetFileNameWithoutExtension(path);
            var extension = Path.GetExtension(path).Replace(".", "");
            if (string.IsNullOrEmpty(extension))
            {
                return name;
            }
            else
            {
                return $"{name}_{extension}";
            }
        }

    }
}

