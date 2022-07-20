using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using NUnit.Framework;
using UnityEditor;
using UnityEditorInternal;
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
        
        private List<Object> _originObjs;
        private ReorderableList _originObjList;
        private bool _buildSingle;
        public string _oneABRName = "mainBundle";

        private void OnEnable()
        {
            _originObjs = new List<Object>();
            _originObjList = new ReorderableList(_originObjs, typeof(Object))
                    {
                        //是否能改变顺序
                        draggable = true,
                        //title
                        drawHeaderCallback = rect => GUI.Label(rect, "Objects"),
                        //set elementHeight
                        elementHeightCallback = index => EditorGUIUtility.singleLineHeight,
                        //set element layout
                        drawElementCallback = (rect, index, isActive, isFocused) =>
                        {
                            _originObjs[index] = EditorGUI.ObjectField(rect,new GUIContent("Origin Object:"), _originObjs[index], typeof(Object), false);
                        },
                        //set add callback
                        onAddCallback = list =>
                        {
                            list.list.Add(null);
                        },
                        //Set remove callback
                        onRemoveCallback = list =>
                        {
                            list.list.RemoveAt(list.index);
                        }
                    };
        }

        private void OnGUI()
        {
            using (new GUILayout.VerticalScope("Box"))
            {
                _originObjList.DoLayoutList();
                _buildSingle = GUILayout.Toggle(_buildSingle, "Single Build");
                if (!_buildSingle)
                    _oneABRName = EditorGUILayout.TextField("ABName:", _oneABRName);
            }
            ShowDependencies();

            ShowAssetBundleInfo();
            
            using (new GUILayout.VerticalScope("Box"))
            {
                if (GUILayout.Button("Collect Dependencies!"))
                {
                    CollectDependencies();
                }

                if ( _dependItems != null && _dependItems.Count == _originObjs.Count && GUILayout.Button("Create AssetBundle!"))
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

        private Dictionary<string, List<DependItem>> _dependItems;

        private void ShowDependencies()
        {
            if(_dependItems == null) return;
            using (new GUILayout.VerticalScope("Box"))
            {
                GUILayout.Label("Dependencies:");
                foreach (var pair in _dependItems)
                {
                    GUILayout.Label(pair.Key + ":");
                    var items = pair.Value;
                    if (items.Count <= 0) continue;
                    foreach (var item in items)
                    {
                        using (new GUILayout.HorizontalScope("Box"))
                        {
                                
                            GUILayout.Label(item.ItemName);
                            item.PackType = (PackType)EditorGUILayout.EnumPopup(item.PackType, GUILayout.Width(100f));
                        }

                    }
                }
            }
        }

        private void CollectDependencies()
        {
            _dependItems = new Dictionary<string, List<DependItem>>();
            foreach (var originObj in _originObjs)
            {
                var items = new List<DependItem>();
                var path = AssetDatabase.GetAssetPath(originObj);
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
                    items.Add(item);
                }
                _dependItems.Add(path, items);
            }
            Repaint();
        }

        private Dictionary<string, AssetBundleBuild> GetAllAssetBundle()
        {
            var abbList = new Dictionary<string, AssetBundleBuild>();
            var mainABPaths = new HashSet<string>();
            foreach (var itemPair in _dependItems)
            {
                var path = itemPair.Key;
                mainABPaths.Add(path);
                foreach (var item in itemPair.Value)
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
            }

            foreach (var path in abbList.Keys)
            {
                if (mainABPaths.Contains(path))
                {
                    mainABPaths.Remove(path);
                }
            }

            var mainAB = new AssetBundleBuild
            {
                assetBundleName = _oneABRName,
                assetNames = mainABPaths.ToArray()
            };
            abbList[_oneABRName] = mainAB;
            return abbList;
        }

        private Dictionary<string, AssetBundleBuild> GetSingleAssetBundle()
        {
            var abbList = new Dictionary<string, AssetBundleBuild>();
            foreach (var itemPair in _dependItems)
            {
                var mainABPaths = new HashSet<string>();
                foreach (var item in itemPair.Value)
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
                
                foreach (var path in abbList.Keys)
                {
                    if (mainABPaths.Contains(path))
                    {
                        mainABPaths.Remove(path);
                    }
                }

                mainABPaths.Add(itemPair.Key);
                var mainAB = new AssetBundleBuild
                {
                    assetBundleName = GetAssetBundleName(itemPair.Key),
                    assetNames = mainABPaths.ToArray()
                };
                abbList[mainAB.assetBundleName] = mainAB;
            }

            return abbList;
        }

        private void CreateAssetBundle()
        {
            var abbList = _buildSingle ? GetSingleAssetBundle() : GetAllAssetBundle();

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
                    Size = fileInfo.Length / 1024f
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

