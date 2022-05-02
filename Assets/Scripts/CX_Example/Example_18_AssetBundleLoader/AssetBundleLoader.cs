using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace CX_Example_18
{
    public enum AssetType
    {
        Prefab,
        Scene,
        Material,
    }

    public class AssetBundleLoader : MonoBehaviour
    {
        public string AssetBundleName;
        public string AssetName;
        public AssetType AssetType;

        private void Start()
        {
            if(string.IsNullOrEmpty(AssetBundleName) || string.IsNullOrEmpty(AssetName)) return;
            Loader(AssetBundleName, AssetType);
        }

        private void Loader(string name, AssetType type, bool isDependency = false)
        {
            // load manifest
            var manifestAB = AssetBundle.LoadFromFile(GetManifestPath());
            var manifest = manifestAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            var dep = manifest.GetAllDependencies(name);
            for (int i = 0; i < dep.Length; i++)
            {
                var ab = AssetBundle.LoadFromFile(GetPath(dep[i]));
            }

            if(isDependency) return;
            var mainAB = AssetBundle.LoadFromFile(GetPath(name));
            switch (type)
            {
                case AssetType.Prefab:
                    var prefab = mainAB.LoadAsset<GameObject>(AssetName);
                    Object.Instantiate(prefab);
                    break;
                case AssetType.Material:
                    var mat = mainAB.LoadAsset<Material>(AssetName);
                    var obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    var render = obj.GetComponent<MeshRenderer>();
                    render.material = mat;
                    break;
                case AssetType.Scene:
                    SceneManager.LoadScene(AssetName);
                    break;
            }
        }

        private string GetPath(string name)
        {
            return Application.streamingAssetsPath + $"/{name}";
        }

        private string GetManifestPath()
        {
            return Application.streamingAssetsPath + $"/StreamingAssets";
        }
    }
}