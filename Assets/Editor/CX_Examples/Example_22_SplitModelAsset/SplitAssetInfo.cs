using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CX_Example_22
{
    [Serializable]
    public class SplitAsset
    {
        public Object originObj;
        public List<Object> assets;
    }

    public class SplitAssetInfo : ScriptableObject
    {
        private static SplitAssetInfo _instance;
        
        private const string AssetPath = "Assets/Editor/CX_Examples/Example_22_SplitModelAsset/SplitAssets/splitAssetInfo.asset";

        public static SplitAssetInfo Instance
        {
            get
            {
                if (_instance == null)
                {
                    if (File.Exists(AssetPath))
                    {
                        _instance = AssetDatabase.LoadAssetAtPath<SplitAssetInfo>(AssetPath);
                    }
                    else
                    {
                        _instance = ScriptableObject.CreateInstance<SplitAssetInfo>();
                        AssetDatabase.CreateAsset(_instance, "Assets/Editor/PomodoroTimer/PomodoroTimerSO.asset");
                        AssetDatabase.Refresh();
                    }
                }
                return _instance;
            }
        }

        public List<SplitAsset> AssetsList = new List<SplitAsset>();

    }
}