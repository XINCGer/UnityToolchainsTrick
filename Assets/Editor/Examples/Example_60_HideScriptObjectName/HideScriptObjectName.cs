using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToolKits
{
    [CreateAssetMenu(fileName = "HideScriptObjectName.asset",menuName = "CustomAssets/HideScriptObjectName")]
    public class HideScriptObjectName : ScriptableObject
    {
        public string Name;
        public int Age;
        public string Readme = "隐藏ScriptableObject的脚本信息展示";
    }
}

